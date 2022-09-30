﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;

namespace SqlSugar
{
    public class SubAnd:ISubOperation
    {
        public string Name
        {
            get { return "And"; }
        }

        public Expression Expression
        {
            get; set;
        }

        public int Sort
        {
            get
            {
                return 401;
            }
        }

        public ExpressionContext Context
        {
            get;set;
        }

        public bool HasWhere
        {
            get; set;
        }

        public string GetValue(Expression expression)
        {
            var exp = expression as MethodCallExpression;
            var argExp = exp.Arguments[0];
            var result = "AND " + SubTools.GetMethodValue(this.Context, argExp, ResolveExpressType.WhereMultiple);


            var regex = @"^AND  (\@Const\d+) $";
            if (this.Context is OracleExpressionContext)
            {
                regex = @"^AND  (\:Const\d+) $";
            }
            if (this.Context is DmExpressionContext)
            {
                regex = @"^AND  (\:Const\d+) $";
            }
            if (Regex.IsMatch(result, regex))
            {
                var value = GetValue(result, regex);
                if (value is Expression)
                {
                    var p = this.Context.Parameters.First(it => it.ParameterName == Regex.Match(result, regex).Groups[1].Value);
                    result = "AND " + SubTools.GetMethodValue(Context, value as Expression, ResolveExpressType.WhereMultiple);
                    argExp = value as Expression;
                    this.Context.Parameters.Remove(p);
                }
                else
                {
                    result = "AND " + value;
                    return result;
                }
            }

            var selfParameterName = this.Context.GetTranslationColumnName((argExp as LambdaExpression).Parameters.First().Name) + UtilConstants.Dot;
            if (this.Context.JoinIndex == 0)
                result = result.Replace(selfParameterName, SubTools.GetSubReplace(this.Context));
            return result;
        }

        private object GetValue(string result, string regex)
        {
            return this.Context.Parameters.First(it => it.ParameterName == Regex.Match(result, regex).Groups[1].Value).Value;
        }
    }
}
