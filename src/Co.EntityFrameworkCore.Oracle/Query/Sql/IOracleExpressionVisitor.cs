// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq.Expressions;
using JetBrains.Annotations;
using Co.EntityFrameworkCore.Query.Expressions.Internal;

namespace Co.EntityFrameworkCore.Query.Sql
{
    /// <summary>
    /// Oracle 表达式访问器接口
    /// </summary>
    public interface IOracleExpressionVisitor
    {
        /// <summary>
        /// 行号访问
        /// </summary>
        /// <param name="rowNumberExpression">行号表达式</param>
        /// <returns></returns>
        Expression VisitRowNumber([NotNull] RowNumberExpression rowNumberExpression);
        /// <summary>
        /// 日期部分表达式访问
        /// </summary>
        /// <param name="datePartExpression">日期部分表达式</param>
        /// <returns></returns>
        Expression VisitDatePartExpression([NotNull] DatePartExpression datePartExpression);
    }
}
