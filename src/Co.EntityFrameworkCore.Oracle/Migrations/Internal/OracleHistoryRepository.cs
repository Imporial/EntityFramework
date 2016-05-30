// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Co.EntityFrameworkCore.Storage.Internal;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using System;
using System.Text;

namespace Co.EntityFrameworkCore.Migrations.Internal
{
    /// <summary>
    /// Oracle 历史资料库
    /// </summary>
    public class OracleHistoryRepository : HistoryRepository
    {
        /// <summary>
        /// 初始化 Oracle 历史资料库
        /// </summary>
        /// <param name="databaseCreator">数据库构建器</param>
        /// <param name="rawSqlCommandBuilder">原始 SQL 命令构建器</param>
        /// <param name="connection">Oracle 数据库连接</param>
        /// <param name="options">数据实体上下文可选项</param>
        /// <param name="modelDiffer">模型差异</param>
        /// <param name="migrationsSqlGenerator">迁移 Sql 生成器</param>
        /// <param name="annotations">相关注解提供程序</param>
        /// <param name="sqlGenerationHelper">sql 生成器帮助类</param>
        public OracleHistoryRepository(
            [NotNull] IDatabaseCreator databaseCreator,
            [NotNull] IRawSqlCommandBuilder rawSqlCommandBuilder,
            [NotNull] IOracleConnection connection,
            [NotNull] IDbContextOptions options,
            [NotNull] IMigrationsModelDiffer modelDiffer,
            [NotNull] IMigrationsSqlGenerator migrationsSqlGenerator,
            [NotNull] IRelationalAnnotationProvider annotations,
            [NotNull] ISqlGenerationHelper sqlGenerationHelper)
            : base(
                databaseCreator,
                rawSqlCommandBuilder,
                connection,
                options,
                modelDiffer,
                migrationsSqlGenerator,
                annotations,
                sqlGenerationHelper)
        {
        }
        /// <summary>
        /// 获取已存在 sql
        /// </summary>
        protected override string ExistsSql
        {
            get
            {
                var builder = new StringBuilder();

                builder.Append("SELECT OBJECT_ID(N'");

                if (TableSchema != null)
                {
                    builder
                        .Append(SqlGenerationHelper.EscapeLiteral(TableSchema))
                        .Append(".");
                }

                builder
                    .Append(SqlGenerationHelper.EscapeLiteral(TableName))
                    .Append("');");

                return builder.ToString();
            }
        }
        /// <summary>
        /// 是否存在翻译结果
        /// </summary>
        /// <param name="value">待翻译对象</param>
        /// <returns></returns>
        protected override bool InterpretExistsResult(object value) => value != DBNull.Value;
        /// <summary>
        /// 获取插入脚本
        /// </summary>
        /// <param name="row">历史数据行</param>
        /// <returns></returns>
        public override string GetInsertScript(HistoryRow row)
        {
            Check.NotNull(row, nameof(row));

            return new StringBuilder().Append("INSERT INTO ")
                .Append(SqlGenerationHelper.DelimitIdentifier(TableName, TableSchema))
                .Append(" (")
                .Append(SqlGenerationHelper.DelimitIdentifier(MigrationIdColumnName))
                .Append(", ")
                .Append(SqlGenerationHelper.DelimitIdentifier(ProductVersionColumnName))
                .AppendLine(")")
                .Append("VALUES (N'")
                .Append(SqlGenerationHelper.EscapeLiteral(row.MigrationId))
                .Append("', N'")
                .Append(SqlGenerationHelper.EscapeLiteral(row.ProductVersion))
                .AppendLine("');")
                .ToString();
        }
        /// <summary>
        /// 获取删除脚本
        /// </summary>
        /// <param name="migrationId">迁移标示</param>
        /// <returns></returns>
        public override string GetDeleteScript(string migrationId)
        {
            Check.NotEmpty(migrationId, nameof(migrationId));

            return new StringBuilder().Append("DELETE FROM ")
                .AppendLine(SqlGenerationHelper.DelimitIdentifier(TableName, TableSchema))
                .Append("WHERE ")
                .Append(SqlGenerationHelper.DelimitIdentifier(MigrationIdColumnName))
                .Append(" = N'")
                .Append(SqlGenerationHelper.EscapeLiteral(migrationId))
                .AppendLine("';")
                .ToString();
        }
        /// <summary>
        /// 如果不存在获取 CREATE 脚本
        /// </summary>
        /// <returns></returns>
        public override string GetCreateIfNotExistsScript()
        {
            var builder = new IndentedStringBuilder();

            builder.Append("IF OBJECT_ID(N'");

            if (TableSchema != null)
            {
                builder
                    .Append(SqlGenerationHelper.EscapeLiteral(TableSchema))
                    .Append(".");
            }

            builder
                .Append(SqlGenerationHelper.EscapeLiteral(TableName))
                .AppendLine("') IS NULL")
                .AppendLine("BEGIN");
            using (builder.Indent())
            {
                builder.AppendLines(GetCreateScript());
            }
            builder.AppendLine("END;");

            return builder.ToString();
        }
        /// <summary>
        /// 如果不存在获取 BEGIN 脚本
        /// </summary>
        /// <param name="migrationId">迁移标示</param>
        /// <returns></returns>
        public override string GetBeginIfNotExistsScript(string migrationId)
        {
            Check.NotEmpty(migrationId, nameof(migrationId));

            return new StringBuilder()
                .Append("IF NOT EXISTS(SELECT * FROM ")
                .Append(SqlGenerationHelper.DelimitIdentifier(TableName, TableSchema))
                .Append(" WHERE ")
                .Append(SqlGenerationHelper.DelimitIdentifier(MigrationIdColumnName))
                .Append(" = N'")
                .Append(SqlGenerationHelper.EscapeLiteral(migrationId))
                .AppendLine("')")
                .Append("BEGIN")
                .ToString();
        }
        /// <summary>
        /// 如果存在获取 BEGIN 脚本
        /// </summary>
        /// <param name="migrationId">迁移标示</param>
        /// <returns></returns>
        public override string GetBeginIfExistsScript(string migrationId)
        {
            Check.NotEmpty(migrationId, nameof(migrationId));

            return new StringBuilder()
                .Append("IF EXISTS(SELECT * FROM ")
                .Append(SqlGenerationHelper.DelimitIdentifier(TableName, TableSchema))
                .Append(" WHERE ")
                .Append(SqlGenerationHelper.DelimitIdentifier(MigrationIdColumnName))
                .Append(" = N'")
                .Append(SqlGenerationHelper.EscapeLiteral(migrationId))
                .AppendLine("')")
                .Append("BEGIN")
                .ToString();
        }
        /// <summary>
        /// 获取 EndIf 脚本
        /// </summary>
        /// <returns></returns>
        public override string GetEndIfScript() => "END;" + Environment.NewLine;
    }
}
