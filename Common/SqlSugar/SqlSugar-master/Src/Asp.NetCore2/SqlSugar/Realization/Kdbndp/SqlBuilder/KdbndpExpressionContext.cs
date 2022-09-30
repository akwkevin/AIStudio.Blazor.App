﻿using System;
using System.Linq;
namespace SqlSugar
{
    public class KdbndpExpressionContext : ExpressionContext, ILambdaExpressions
    {
        public SqlSugarProvider Context { get; set; }
        public KdbndpExpressionContext()
        {
            base.DbMehtods = new KdbndpMethod();
        }
        public override string SqlTranslationLeft
        {
            get
            {
                return "\"";
            }
        }
        public override string SqlTranslationRight
        {
            get
            {
                return "\"";
            }
        }
        public override string GetTranslationText(string name)
        {
            return SqlTranslationLeft + name.ToUpper() + SqlTranslationRight;
        }
    
        public override string GetTranslationTableName(string entityName, bool isMapping = true)
        {
            Check.ArgumentNullException(entityName, string.Format(ErrorMessage.ObjNotExist, "Table Name"));
            if (IsTranslationText(entityName)) return entityName;
            isMapping = isMapping && this.MappingTables.HasValue();
            var isComplex = entityName.Contains(UtilConstants.Dot);
            if (isMapping && isComplex)
            {
                var columnInfo = entityName.Split(UtilConstants.DotChar);
                var mappingInfo = this.MappingTables.FirstOrDefault(it => it.EntityName.Equals(columnInfo.Last(), StringComparison.CurrentCultureIgnoreCase));
                if (mappingInfo != null)
                {
                    columnInfo[columnInfo.Length - 1] = mappingInfo.EntityName;
                }
                return string.Join(UtilConstants.Dot, columnInfo.Select(it => GetTranslationText(it)));
            }
            else if (isMapping)
            {
                var mappingInfo = this.MappingTables.FirstOrDefault(it => it.EntityName.Equals(entityName, StringComparison.CurrentCultureIgnoreCase));
                return SqlTranslationLeft + (mappingInfo == null ? entityName : mappingInfo.DbTableName).ToUpper() + SqlTranslationRight;
            }
            else if (isComplex)
            {
                return string.Join(UtilConstants.Dot, entityName.Split(UtilConstants.DotChar).Select(it => GetTranslationText(it)));
            }
            else
            {
                return GetTranslationText(entityName);
            }
        }
        public override string GetTranslationColumnName(string columnName)
        {
            Check.ArgumentNullException(columnName, string.Format(ErrorMessage.ObjNotExist, "Column Name"));
            if (columnName.Substring(0, 1) == this.SqlParameterKeyWord)
            {
                return columnName;
            }
            if (IsTranslationText(columnName)) return columnName;
            if (columnName.Contains(UtilConstants.Dot))
            {
                return string.Join(UtilConstants.Dot, columnName.Split(UtilConstants.DotChar).Select(it => GetTranslationText(it)));
            }
            else
            {
                return GetTranslationText(columnName);
            }
        }
        public override string GetDbColumnName(string entityName, string propertyName)
        {
            if (this.MappingColumns.HasValue())
            {
                var mappingInfo = this.MappingColumns.SingleOrDefault(it => it.EntityName == entityName && it.PropertyName == propertyName);
                return (mappingInfo == null ? propertyName : mappingInfo.DbColumnName).ToUpper();
            }
            else
            {
                return propertyName.ToUpper();
            }
        }
    }
    public class KdbndpMethod : DefaultDbMethod, IDbMethods
    {
        public override string DateDiff(MethodCallExpressionModel model)
        {
            var parameter = (DateType)(Enum.Parse(typeof(DateType), model.Args[0].MemberValue.ObjToString()));
            var begin = model.Args[1].MemberName;
            var end = model.Args[2].MemberName;
            switch (parameter)
            {
                case DateType.Year:
                    return $" ( DATE_PART('Year',  {end}   ) - DATE_PART('Year',  {begin}) )";
                case DateType.Month:
                    return $" (  ( DATE_PART('Year',  {end}   ) - DATE_PART('Year',  {begin}) ) * 12 + (DATE_PART('month', {end}) - DATE_PART('month', {begin})) )";
                case DateType.Day:
                    return $" ( DATE_PART('day', {end} - {begin}) )";
                case DateType.Hour:
                    return $" ( ( DATE_PART('day', {end} - {begin}) ) * 24 + DATE_PART('hour', {end} - {begin} ) )";
                case DateType.Minute:
                    return $" ( ( ( DATE_PART('day', {end} - {begin}) ) * 24 + DATE_PART('hour', {end} - {begin} ) ) * 60 + DATE_PART('minute', {end} - {begin} ) )";
                case DateType.Second:
                    return $" ( ( ( DATE_PART('day', {end} - {begin}) ) * 24 + DATE_PART('hour', {end} - {begin} ) ) * 60 + DATE_PART('minute', {end} - {begin} ) ) * 60 + DATE_PART('minute', {end} - {begin} )";
                case DateType.Millisecond:
                    break;
                default:
                    break;
            }
            throw new Exception(parameter + " datediff no support");
        }
        public override string DateValue(MethodCallExpressionModel model)
        {
            var parameter = model.Args[0];
            var parameter2 = model.Args[1];
            var format = "dd";
            if (parameter2.MemberValue.ObjToString() == DateType.Year.ToString())
            {
                format = "yyyy";
            }
            if (parameter2.MemberValue.ObjToString() == DateType.Month.ToString())
            {
                format = "MM";
            }
            if (parameter2.MemberValue.ObjToString() == DateType.Day.ToString())
            {
                format = "dd";
            }
            if (parameter2.MemberValue.ObjToString() == DateType.Hour.ToString())
            {
                format = "hh";
            }
            if (parameter2.MemberValue.ObjToString() == DateType.Minute.ToString())
            {
                format = "mi";
            }
            if (parameter2.MemberValue.ObjToString() == DateType.Second.ToString())
            {
                format = "ss";
            }
            if (parameter2.MemberValue.ObjToString() == DateType.Millisecond.ToString())
            {
                format = "ms";
            }
            if (parameter2.MemberValue.ObjToString() == DateType.Weekday.ToString())
            {
                return $"  extract(DOW FROM cast({parameter.MemberName} as TIMESTAMP)) ";
            }

            return string.Format(" cast( to_char({1},'{0}')as integer ) ", format, parameter.MemberName);
        }

        public override string Contains(MethodCallExpressionModel model)
        {
            var parameter = model.Args[0];
            var parameter2 = model.Args[1];
            return string.Format(" ({0} like concat('%',{1},'%')) ", parameter.MemberName, parameter2.MemberName  );
        }

        public override string StartsWith(MethodCallExpressionModel model)
        {
            var parameter = model.Args[0];
            var parameter2 = model.Args[1];
            return string.Format(" ({0} like concat({1},'%')) ", parameter.MemberName, parameter2.MemberName);
        }

        public override string EndsWith(MethodCallExpressionModel model)
        {
            var parameter = model.Args[0];
            var parameter2 = model.Args[1];
            return string.Format(" ({0} like concat('%',{1}))", parameter.MemberName,parameter2.MemberName);
        }

        public override string DateIsSameDay(MethodCallExpressionModel model)
        {
            var parameter = model.Args[0];
            var parameter2 = model.Args[1];
            return string.Format(" (date_part('day',{0}-{1})=0) ", parameter.MemberName, parameter2.MemberName); ;
        }

        public override string DateIsSameByType(MethodCallExpressionModel model)
        {
            var parameter = model.Args[0];
            var parameter2 = model.Args[1];
            var parameter3 = model.Args[2];
            return string.Format(" (date_part('{2}',{0}-{1})=0) ", parameter.MemberName, parameter2.MemberName, parameter3.MemberValue);
        }

        public override string ToDate(MethodCallExpressionModel model)
        {
            var parameter = model.Args[0];
            return string.Format(" CAST({0} AS timestamp)", parameter.MemberName);
        }
        public override string DateAddByType(MethodCallExpressionModel model)
        {
            var parameter = model.Args[0];
            var parameter2 = model.Args[1];
            var parameter3 = model.Args[2];
            return string.Format(" ({1} +  ({2}||'{0}')::INTERVAL) ", parameter3.MemberValue, parameter.MemberName, parameter2.MemberName);
        }

        public override string DateAddDay(MethodCallExpressionModel model)
        {
            var parameter = model.Args[0];
            var parameter2 = model.Args[1];
            return string.Format(" ({0} + ({1}||'day')::INTERVAL) ", parameter.MemberName, parameter2.MemberName);
        }

        public override string ToInt32(MethodCallExpressionModel model)
        {
            var parameter = model.Args[0];
            return string.Format(" CAST({0} AS SIGNED)", parameter.MemberName);
        }

        public override string ToInt64(MethodCallExpressionModel model)
        {
            var parameter = model.Args[0];
            return string.Format(" CAST({0} AS SIGNED)", parameter.MemberName);
        }

        public override string ToString(MethodCallExpressionModel model)
        {
            var parameter = model.Args[0];
            return string.Format(" CAST({0} AS VARCHAR)", parameter.MemberName);
        }

        public override string ToGuid(MethodCallExpressionModel model)
        {
            var parameter = model.Args[0];
            return string.Format(" CAST({0} AS VARCHAR)", parameter.MemberName);
        }

        public override string ToDouble(MethodCallExpressionModel model)
        {
            var parameter = model.Args[0];
            return string.Format(" CAST({0} AS DECIMAL(18,4))", parameter.MemberName);
        }

        public override string ToBool(MethodCallExpressionModel model)
        {
            var parameter = model.Args[0];
            return string.Format(" CAST({0} AS SIGNED)", parameter.MemberName);
        }

        public override string ToDecimal(MethodCallExpressionModel model)
        {
            var parameter = model.Args[0];
            return string.Format(" CAST({0} AS DECIMAL(18,4))", parameter.MemberName);
        }

        public override string Length(MethodCallExpressionModel model)
        {
            var parameter = model.Args[0];
            return string.Format(" LENGTH({0})", parameter.MemberName);
        }
        public override string MergeString(params string[] strings)
        {
            return " concat("+string.Join(",", strings).Replace("+", "") + ") ";
        }
        public override string IsNull(MethodCallExpressionModel model)
        {
            var parameter = model.Args[0];
            var parameter1 = model.Args[1];
            return string.Format("(CASE WHEN  {0} IS NULL THEN  {1} ELSE {0} END)", parameter.MemberName, parameter1.MemberName);
        }
        public override string GetDate()
        {
            return "NOW()";
        }
        public override string GetRandom()
        {
            return "RANDOM()";
        }

        public override string EqualTrue(string fieldName)
        {
            return "( " + fieldName + "=true )";
        }
    }
}
