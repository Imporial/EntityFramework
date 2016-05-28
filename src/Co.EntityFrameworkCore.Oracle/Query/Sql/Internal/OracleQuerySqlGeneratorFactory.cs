// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query.Expressions;
using Microsoft.EntityFrameworkCore.Query.Sql;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Co.EntityFrameworkCore.Query.Sql.Internal
{
    /// <summary>
    /// Oracle ��ѯ SQL ����������
    /// </summary>
    public class OracleQuerySqlGeneratorFactory : QuerySqlGeneratorFactoryBase
    {
        /// <summary>
        /// ��ʼ�� Oracle ��ѯ SQL ����������
        /// </summary>
        /// <param name="commandBuilderFactory">���������</param>
        /// <param name="sqlGenerationHelper">SQL ������������</param>
        /// <param name="parameterNameGeneratorFactory">����������������</param>
        /// <param name="relationalTypeMapper">�������ӳ��</param>
        public OracleQuerySqlGeneratorFactory(
            [NotNull] IRelationalCommandBuilderFactory commandBuilderFactory,
            [NotNull] ISqlGenerationHelper sqlGenerationHelper,
            [NotNull] IParameterNameGeneratorFactory parameterNameGeneratorFactory,
            [NotNull] IRelationalTypeMapper relationalTypeMapper)
            : base(
                Check.NotNull(commandBuilderFactory, nameof(commandBuilderFactory)),
                Check.NotNull(sqlGenerationHelper, nameof(sqlGenerationHelper)),
                Check.NotNull(parameterNameGeneratorFactory, nameof(parameterNameGeneratorFactory)),
                Check.NotNull(relationalTypeMapper, nameof(relationalTypeMapper)))
        {
        }
        /// <summary>
        /// ��Ĭ�Ϸ�ʽ������ѯ
        /// </summary>
        /// <param name="selectExpression">select ��ѯ���ʽ</param>
        /// <returns></returns>
        public override IQuerySqlGenerator CreateDefault(SelectExpression selectExpression)
            => new OracleQuerySqlGenerator(
                CommandBuilderFactory,
                SqlGenerationHelper,
                ParameterNameGeneratorFactory,
                RelationalTypeMapper,
                Check.NotNull(selectExpression, nameof(selectExpression)));
    }
}
