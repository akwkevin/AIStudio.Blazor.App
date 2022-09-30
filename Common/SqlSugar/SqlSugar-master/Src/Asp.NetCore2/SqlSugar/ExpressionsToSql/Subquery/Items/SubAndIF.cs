﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SqlSugar
{
    public class SubAndIF : ISubOperation
    {
        public bool HasWhere
        {
            get; set;
        }

        public string Name
        {
            get { return "WhereIF"; }
        }

        public Expression Expression
        {
            get; set;
        }

        public int Sort
        {
            get
            {
                return 400;
            }
        }

        public ExpressionContext Context
        {
            get; set;
        }

        public string GetValue(Expression expression)
        {
            var exp = expression as MethodCallExpression;
            object value = null;
            try
            {
                value = ExpressionTool.DynamicInvoke(exp.Arguments[0]);
            }
            catch 
            {
                Check.Exception(true, ErrorMessage.WhereIFCheck,exp.Arguments[0].ToString());
            }
            var isWhere= Convert.ToBoolean(value);
            if (!Convert.ToBoolean(isWhere)) {
                return "";
            }
            var argExp = exp.Arguments[1];
            var result = "AND " + SubTools.GetMethodValue(Context, argExp, ResolveExpressType.WhereMultiple); ;
            var selfParameterName = Context.GetTranslationColumnName((argExp as LambdaExpression).Parameters.First().Name) + UtilConstants.Dot;
            result = result.Replace(selfParameterName, SubTools.GetSubReplace(this.Context));
            return result;
        }
    }
}
