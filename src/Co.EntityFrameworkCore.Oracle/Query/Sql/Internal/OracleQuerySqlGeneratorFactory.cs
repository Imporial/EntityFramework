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
    /// Oracle 查询 SQL 生成器工厂
    /// </summary>
    public class OracleQuerySqlGeneratorFactory : QuerySqlGeneratorFactoryBase
    {
        /// <summary>
        /// 初始化 Oracle 查询 SQL 生成器工厂
        /// </summary>
        /// <param name="commandBuilderFactory">命令构建工厂</param>
        /// <param name="sqlGenerationHelper">SQL 生成器帮助类</param>
        /// <param name="parameterNameGeneratorFactory">参数名生成器工厂</param>
        /// <param name="relationalTypeMapper">相关类型映射</param>
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
        /// 以默认方式创建查询
        /// </summary>
        /// <param name="selectExpression">select 查询表达式</param>
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
