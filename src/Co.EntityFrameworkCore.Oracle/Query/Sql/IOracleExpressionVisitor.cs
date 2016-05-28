// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq.Expressions;
using JetBrains.Annotations;
using Co.EntityFrameworkCore.Query.Expressions.Internal;

namespace Co.EntityFrameworkCore.Query.Sql
{
    /// <summary>
    /// Oracle ���ʽ�������ӿ�
    /// </summary>
    public interface IOracleExpressionVisitor
    {
        /// <summary>
        /// �кŷ���
        /// </summary>
        /// <param name="rowNumberExpression">�кű��ʽ</param>
        /// <returns></returns>
        Expression VisitRowNumber([NotNull] RowNumberExpression rowNumberExpression);
        /// <summary>
        /// ���ڲ��ֱ��ʽ����
        /// </summary>
        /// <param name="datePartExpression">���ڲ��ֱ��ʽ</param>
        /// <returns></returns>
        Expression VisitDatePartExpression([NotNull] DatePartExpression datePartExpression);
    }
}
