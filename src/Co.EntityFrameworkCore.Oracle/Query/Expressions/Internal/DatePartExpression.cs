// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Co.EntityFrameworkCore.Query.Sql;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Co.EntityFrameworkCore.Query.Expressions.Internal
{
    public class DatePartExpression : Expression
    {
        /// <summary>
        /// 指定日期部分整数表达式
        /// </summary>
        /// <param name="datePart">日期部分</param>
        /// <param name="type">类型</param>
        /// <param name="argument">表达式</param>
        public DatePartExpression(
            [NotNull] string datePart,
            [NotNull] Type type,
            [NotNull] Expression argument)
        {
            DatePart = datePart;
            Type = type;
            Argument = argument;
        }

        public override Type Type { get; }
        public override ExpressionType NodeType => ExpressionType.Extension;
        
        public virtual Expression Argument { get; }
        public virtual string DatePart { get; }

        protected override Expression Accept(ExpressionVisitor visitor)
        {
            Check.NotNull(visitor, nameof(visitor));

            var specificVisitor = visitor as IOracleExpressionVisitor;

            return specificVisitor != null
                ? specificVisitor.VisitDatePartExpression(this)
                : base.Accept(visitor);
        }

        protected override Expression VisitChildren(ExpressionVisitor visitor) => this;
    }
}