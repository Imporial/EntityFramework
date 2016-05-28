// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;

namespace Co.EntityFrameworkCore.Query.ExpressionTranslators.Internal
{
    public class OracleDateTimeNowTranslator : IMemberTranslator
    {
        public virtual Expression Translate(MemberExpression memberExpression)
        {
            if ((memberExpression.Expression == null)
                && (memberExpression.Member.DeclaringType == typeof(DateTime)))
            {
                switch (memberExpression.Member.Name)
                {
                    case nameof(DateTime.Now):
                        return new SqlFunctionExpression("GETDATE", memberExpression.Type);
                    case nameof(DateTime.UtcNow):
                        return new SqlFunctionExpression("GETUTCDATE", memberExpression.Type);
                }
            }

            return null;
        }
    }
}
