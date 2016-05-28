// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;

namespace Co.EntityFrameworkCore.Query.ExpressionTranslators.Internal
{
    public class OracleStringIsNullOrWhiteSpaceTranslator : IMethodCallTranslator
    {
        private static readonly MethodInfo _methodInfo
            = typeof(string).GetRuntimeMethod(nameof(string.IsNullOrWhiteSpace), new[] { typeof(string) });

        public virtual Expression Translate(MethodCallExpression methodCallExpression)
        {
            if (_methodInfo == methodCallExpression.Method)
            {
                var argument = methodCallExpression.Arguments[0];

                return Expression.MakeBinary(
                    ExpressionType.OrElse,
                    new IsNullExpression(argument),
                    Expression.Equal(
                        new SqlFunctionExpression(
                            "LTRIM",
                            typeof(string),
                            new[]
                            {
                                new SqlFunctionExpression(
                                    "RTRIM",
                                    typeof(string),
                                    new [] { argument})
                            }),
                        Expression.Constant("", typeof(string))));
            }

            return null;
        }
    }
}
