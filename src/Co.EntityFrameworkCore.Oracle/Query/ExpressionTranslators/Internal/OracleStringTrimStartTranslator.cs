// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;

namespace Co.EntityFrameworkCore.Query.ExpressionTranslators.Internal
{
    public class OracleStringTrimStartTranslator : IMethodCallTranslator
    {
        private static readonly MethodInfo _trimStart = typeof(string).GetTypeInfo()
            .GetDeclaredMethod(nameof(string.TrimStart));

        public virtual Expression Translate(MethodCallExpression methodCallExpression)
        {
            if ((_trimStart == methodCallExpression.Method)
                // SqlServer LTRIM does not take arguments
                && (((methodCallExpression.Arguments[0] as ConstantExpression)?.Value as Array)?.Length == 0))
            {
                var sqlArguments = new[] { methodCallExpression.Object };
                return new SqlFunctionExpression("LTRIM", methodCallExpression.Type, sqlArguments);
            }

            return null;
        }
    }
}
