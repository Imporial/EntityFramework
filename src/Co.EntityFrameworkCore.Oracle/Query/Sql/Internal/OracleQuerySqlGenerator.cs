// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Co.EntityFrameworkCore.Query.Expressions.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using Remotion.Linq.Parsing;
using Microsoft.EntityFrameworkCore.Query.Sql;

namespace Co.EntityFrameworkCore.Query.Sql.Internal
{
    /// <summary>
    /// Oracle ��ѯ sql ������
    /// </summary>
    public class OracleQuerySqlGenerator : DefaultQuerySqlGenerator, IOracleExpressionVisitor
    {
        /// <summary>
        /// ��ʼ�� Oracle ��ѯ sql ������
        /// </summary>
        /// <param name="relationalCommandBuilderFactory">������������</param>
        /// <param name="sqlGenerationHelper">sql ���ɰ�����</param>
        /// <param name="parameterNameGeneratorFactory">����������������</param>
        /// <param name="relationalTypeMapper">�������ӳ��</param>
        /// <param name="selectExpression">select ��ѯ���ʽ</param>
        public OracleQuerySqlGenerator(
            [NotNull] IRelationalCommandBuilderFactory relationalCommandBuilderFactory,
            [NotNull] ISqlGenerationHelper sqlGenerationHelper,
            [NotNull] IParameterNameGeneratorFactory parameterNameGeneratorFactory,
            [NotNull] IRelationalTypeMapper relationalTypeMapper,
            [NotNull] SelectExpression selectExpression)
            : base(
                relationalCommandBuilderFactory,
                sqlGenerationHelper,
                parameterNameGeneratorFactory,
                relationalTypeMapper,
                selectExpression)
        {
        }
        /// <summary>
        /// ���ʺ�������
        /// </summary>
        /// <param name="lateralJoinExpression">�������ӱ��ʽ</param>
        /// <returns></returns>
        public override Expression VisitLateralJoin(LateralJoinExpression lateralJoinExpression)
        {
            Check.NotNull(lateralJoinExpression, nameof(lateralJoinExpression));

            Sql.Append("CROSS APPLY ");

            Visit(lateralJoinExpression.TableExpression);

            return lateralJoinExpression;
        }
        /// <summary>
        /// ����������
        /// </summary>
        /// <param name="countExpression">���������ʽ</param>
        /// <returns></returns>
        public override Expression VisitCount(CountExpression countExpression)
        {
            Check.NotNull(countExpression, nameof(countExpression));

            if (countExpression.Type == typeof(long))
            {
                Sql.Append("COUNT_BIG(*)");

                return countExpression;
            }

            return base.VisitCount(countExpression);
        }
        /// <summary>
        /// δ֪
        /// </summary>
        /// <param name="selectExpression">��ѯ���ʽ</param>
        protected override void GenerateLimitOffset(SelectExpression selectExpression)
        {
            if (selectExpression.Projection.OfType<RowNumberExpression>().Any())
            {
                return;
            }

            if (selectExpression.Offset != null
                && !selectExpression.OrderBy.Any())
            {
                Sql.AppendLine().Append("ORDER BY @@ROWCOUNT");
            }

            base.GenerateLimitOffset(selectExpression);
        }
        /// <summary>
        /// Ͷ�����
        /// </summary>
        /// <param name="projections">Ͷ�伯��</param>
        protected override void VisitProjection(IReadOnlyList<Expression> projections)
        {
            var comparisonTransformer = new ProjectionComparisonTransformingVisitor();
            var transformedProjections = projections.Select(comparisonTransformer.Visit).ToList();

            base.VisitProjection(transformedProjections);
        }
        /// <summary>
        /// �кŷ���
        /// </summary>
        /// <param name="rowNumberExpression">�кű��ʽ</param>
        /// <returns></returns>
        public virtual Expression VisitRowNumber(RowNumberExpression rowNumberExpression)
        {
            Check.NotNull(rowNumberExpression, nameof(rowNumberExpression));

            Sql.Append("ROW_NUMBER() OVER(");
            GenerateOrderBy(rowNumberExpression.Orderings);
            Sql.Append(") AS ").Append(SqlGenerator.DelimitIdentifier(rowNumberExpression.ColumnExpression.Name));

            return rowNumberExpression;
        }
        /// <summary>
        /// ���ڲ��ֱ��ʽ����
        /// </summary>
        /// <param name="datePartExpression">���ڲ��ֱ��ʽ</param>
        /// <returns></returns>
        public virtual Expression VisitDatePartExpression(DatePartExpression datePartExpression)
        {
            Check.NotNull(datePartExpression, nameof(datePartExpression));

            Sql.Append("DATEPART(")
                .Append(datePartExpression.DatePart)
                .Append(", ");
            Visit(datePartExpression.Argument);
            Sql.Append(")");
            return datePartExpression;
        }
        /// <summary>
        /// Sql ��������
        /// </summary>
        /// <param name="sqlFunctionExpression">SQL �������ʽ</param>
        /// <returns></returns>
        public override Expression VisitSqlFunction(SqlFunctionExpression sqlFunctionExpression)
        {
            if (sqlFunctionExpression.FunctionName.StartsWith("@@", StringComparison.Ordinal))
            {
                Sql.Append(sqlFunctionExpression.FunctionName);
                return sqlFunctionExpression;
            }
            return base.VisitSqlFunction(sqlFunctionExpression);
        }
        /// <summary>
        /// Ͷ��Ƚ�ת��������
        /// </summary>
        private class ProjectionComparisonTransformingVisitor : RelinqExpressionVisitor
        {
            private bool _insideConditionalTest;

            protected override Expression VisitUnary(UnaryExpression node)
            {
                if (!_insideConditionalTest
                    && node.NodeType == ExpressionType.Not
                    && node.Operand is AliasExpression)
                {
                    return Expression.Condition(
                        node,
                        Expression.Constant(true, typeof(bool)),
                        Expression.Constant(false, typeof(bool)));
                }

                if (!_insideConditionalTest
                    && node.NodeType == ExpressionType.Not
                    && node.Operand is IsNullExpression)
                {
                    return Expression.Condition(
                        node.Operand,
                        Expression.Constant(false, typeof(bool)),
                        Expression.Constant(true, typeof(bool)));
                }

                if (!_insideConditionalTest
                    && node.Operand is IsNullExpression)
                {
                    return Expression.Condition(
                        node,
                        Expression.Constant(true, typeof(bool)),
                        Expression.Constant(false, typeof(bool)));
                }

                return base.VisitUnary(node);
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                if (!_insideConditionalTest
                    && (node.IsComparisonOperation()
                        || node.IsLogicalOperation()))
                {
                    return Expression.Condition(
                        node,
                        Expression.Constant(true, typeof(bool)),
                        Expression.Constant(false, typeof(bool)));
                }

                return base.VisitBinary(node);
            }

            protected override Expression VisitConditional(ConditionalExpression node)
            {
                _insideConditionalTest = true;
                var test = Visit(node.Test);
                _insideConditionalTest = false;
                if (test is AliasExpression)
                {
                    return Expression.Condition(
                        Expression.Equal(test, Expression.Constant(true, typeof(bool))),
                        Visit(node.IfTrue),
                        Visit(node.IfFalse));
                }

                var condition = test as ConditionalExpression;
                if (condition != null)
                {
                    return Expression.Condition(
                        condition.Test,
                        Visit(node.IfTrue),
                        Visit(node.IfFalse));
                }
                return Expression.Condition(test,
                    Visit(node.IfTrue),
                    Visit(node.IfFalse));
            }
        }
    }
}
