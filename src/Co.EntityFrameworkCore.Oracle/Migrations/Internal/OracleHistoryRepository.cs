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
    /// Oracle ��ʷ���Ͽ�
    /// </summary>
    public class OracleHistoryRepository : HistoryRepository
    {
        /// <summary>
        /// ��ʼ�� Oracle ��ʷ���Ͽ�
        /// </summary>
        /// <param name="databaseCreator">���ݿ⹹����</param>
        /// <param name="rawSqlCommandBuilder">ԭʼ SQL �������</param>
        /// <param name="connection">Oracle ���ݿ�����</param>
        /// <param name="options">����ʵ�������Ŀ�ѡ��</param>
        /// <param name="modelDiffer">ģ�Ͳ���</param>
        /// <param name="migrationsSqlGenerator">Ǩ�� Sql ������</param>
        /// <param name="annotations">���ע���ṩ����</param>
        /// <param name="sqlGenerationHelper">sql ������������</param>
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
        /// ��ȡ�Ѵ��� sql
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
        /// �Ƿ���ڷ�����
        /// </summary>
        /// <param name="value">���������</param>
        /// <returns></returns>
        protected override bool InterpretExistsResult(object value) => value != DBNull.Value;
        /// <summary>
        /// ��ȡ����ű�
        /// </summary>
        /// <param name="row">��ʷ������</param>
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
        /// ��ȡɾ���ű�
        /// </summary>
        /// <param name="migrationId">Ǩ�Ʊ�ʾ</param>
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
        /// ��������ڻ�ȡ CREATE �ű�
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
        /// ��������ڻ�ȡ BEGIN �ű�
        /// </summary>
        /// <param name="migrationId">Ǩ�Ʊ�ʾ</param>
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
        /// ������ڻ�ȡ BEGIN �ű�
        /// </summary>
        /// <param name="migrationId">Ǩ�Ʊ�ʾ</param>
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
        /// ��ȡ EndIf �ű�
        /// </summary>
        /// <returns></returns>
        public override string GetEndIfScript() => "END;" + Environment.NewLine;
    }
}
