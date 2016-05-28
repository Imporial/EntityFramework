// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;

namespace Co.EntityFrameworkCore.Query.ExpressionTranslators.Internal
{
    public class OracleStringLengthTranslator : IMemberTranslator
    {
        public virtual Expression Translate(MemberExpression memberExpression)
            => (memberExpression.Expression != null)
               && (memberExpression.Expression.Type == typeof(string))
               && (memberExpression.Member.Name == nameof(string.Length))
                ? new SqlFunctionExpression("LEN", memberExpression.Type, new[] { memberExpression.Expression })
                : null;
    }
}
