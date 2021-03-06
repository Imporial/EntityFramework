// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Co.EntityFrameworkCore.Metadata.Internal;
using Co.EntityFrameworkCore.Migrations.Operations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Co.EntityFrameworkCore.Metadata;

namespace Co.EntityFrameworkCore.Migrations
{
    /// <summary>
    /// Oracle 迁移 Sql 生成器
    /// </summary>
    public class OracleMigrationsSqlGenerator : MigrationsSqlGenerator
    {
        private int _variableCounter;
        /// <summary>
        /// 初始化 Oracle 迁移 Sql 生成器
        /// </summary>
        /// <param name="commandBuilderFactory"></param>
        /// <param name="sqlGenerationHelper"></param>
        /// <param name="typeMapper"></param>
        /// <param name="annotations"></param>
        public OracleMigrationsSqlGenerator(
            [NotNull] IRelationalCommandBuilderFactory commandBuilderFactory,
            [NotNull] ISqlGenerationHelper sqlGenerationHelper,
            [NotNull] IRelationalTypeMapper typeMapper,
            [NotNull] IRelationalAnnotationProvider annotations)
            : base(commandBuilderFactory, sqlGenerationHelper, typeMapper, annotations)
        {
        }
        /// <summary>
        /// 生成迁移
        /// </summary>
        /// <param name="operation">迁移操作对象</param>
        /// <param name="model">模型</param>
        /// <param name="builder">迁移命令列表构建器</param>
        protected override void Generate(MigrationOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            Check.NotNull(operation, nameof(operation));
            Check.NotNull(builder, nameof(builder));

            var createDatabaseOperation = operation as OracleCreateDatabaseOperation;
            var dropDatabaseOperation = operation as OracleDropDatabaseOperation;
            if (createDatabaseOperation != null)
            {
                Generate(createDatabaseOperation, model, builder);
            }
            else if (dropDatabaseOperation != null)
            {
                Generate(dropDatabaseOperation, model, builder);
            }
            else
            {
                base.Generate(operation, model, builder);
            }
        }
        /// <summary>
        /// 生成修改列
        /// </summary>
        /// <param name="operation">修改列操作对象</param>
        /// <param name="model">模型</param>
        /// <param name="builder">迁移命令列表构建器</param>
        protected override void Generate(
            AlterColumnOperation operation,
            IModel model,
            MigrationCommandListBuilder builder)
        {
            Check.NotNull(operation, nameof(operation));
            Check.NotNull(builder, nameof(builder));

            DropDefaultConstraint(operation.Schema, operation.Table, operation.Name, builder);

            builder
                .Append("ALTER TABLE ")
                .Append(SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema))
                .Append(" ALTER COLUMN ");

            ColumnDefinition(
                operation.Schema,
                operation.Table,
                operation.Name,
                operation.ClrType,
                operation.ColumnType,
                operation.IsNullable,
                /*defaultValue:*/ null,
                /*defaultValueSql:*/ null,
                operation.ComputedColumnSql,
                /*identity:*/ false,
                operation,
                model,
                builder);

            if ((operation.DefaultValue != null)
                || (operation.DefaultValueSql != null))
            {
                builder
                    .AppendLine(";")
                    .Append("ALTER TABLE ")
                    .Append(SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema))
                    .Append(" ADD");
                DefaultValue(operation.DefaultValue, operation.DefaultValueSql, builder);
                builder
                    .Append(" FOR ")
                    .Append(SqlGenerationHelper.DelimitIdentifier(operation.Name));
            }

            EndStatement(builder);
        }
        /// <summary>
        /// 生成重命名索引
        /// </summary>
        /// <param name="operation">重命名索引操作对象</param>
        /// <param name="model">模型</param>
        /// <param name="builder">迁移命令列表构建器</param>
        protected override void Generate(
            RenameIndexOperation operation,
            IModel model,
            MigrationCommandListBuilder builder)
        {
            Check.NotNull(operation, nameof(operation));
            Check.NotNull(builder, nameof(builder));

            var qualifiedName = new StringBuilder();
            if (operation.Schema != null)
            {
                qualifiedName
                    .Append(operation.Schema)
                    .Append(".");
            }
            qualifiedName
                .Append(operation.Table)
                .Append(".")
                .Append(operation.Name);

            Rename(qualifiedName.ToString(), operation.NewName, "INDEX", builder);
            EndStatement(builder);
        }
        /// <summary>
        /// 生成重命名序列
        /// </summary>
        /// <param name="operation">重命名序列操作对象</param>
        /// <param name="model">模型</param>
        /// <param name="builder">迁移命令列表构建器</param>
        protected override void Generate(RenameSequenceOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            Check.NotNull(operation, nameof(operation));
            Check.NotNull(builder, nameof(builder));

            var separate = false;
            var name = operation.Name;
            if (operation.NewName != null)
            {
                var qualifiedName = new StringBuilder();
                if (operation.Schema != null)
                {
                    qualifiedName
                        .Append(operation.Schema)
                        .Append(".");
                }
                qualifiedName.Append(operation.Name);

                Rename(qualifiedName.ToString(), operation.NewName, builder);

                separate = true;
                name = operation.NewName;
            }

            if (operation.NewSchema != null)
            {
                if (separate)
                {
                    builder.AppendLine(SqlGenerationHelper.StatementTerminator);
                }

                Transfer(operation.NewSchema, operation.Schema, name, builder);
            }

            EndStatement(builder);
        }
        /// <summary>
        /// 生成重命名表
        /// </summary>
        /// <param name="operation">重命名表操作对象</param>
        /// <param name="model">模型</param>
        /// <param name="builder">迁移命令列表构建器</param>
        protected override void Generate(
            RenameTableOperation operation,
            IModel model,
            MigrationCommandListBuilder builder)
        {
            Check.NotNull(operation, nameof(operation));
            Check.NotNull(builder, nameof(builder));

            var separate = false;
            var name = operation.Name;
            if (operation.NewName != null)
            {
                var qualifiedName = new StringBuilder();
                if (operation.Schema != null)
                {
                    qualifiedName
                        .Append(operation.Schema)
                        .Append(".");
                }
                qualifiedName.Append(operation.Name);

                Rename(qualifiedName.ToString(), operation.NewName, builder);

                separate = true;
                name = operation.NewName;
            }

            if (operation.NewSchema != null)
            {
                if (separate)
                {
                    builder.AppendLine(SqlGenerationHelper.StatementTerminator);
                }

                Transfer(operation.NewSchema, operation.Schema, name, builder);
            }

            EndStatement(builder);
        }
        /// <summary>
        /// 生成创建索引
        /// </summary>
        /// <param name="operation">创建索引操作对象</param>
        /// <param name="model">模型</param>
        /// <param name="builder">迁移命令列表构建器</param>
        protected override void Generate(CreateIndexOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            Check.NotNull(operation, nameof(operation));
            Check.NotNull(builder, nameof(builder));

            base.Generate(operation, model, builder, terminate: false);

            var clustered = operation[OracleFullAnnotationNames.Instance.Clustered] as bool?;
            if (operation.IsUnique
                && (clustered != true))
            {
                builder.Append(" WHERE ");
                for (var i = 0; i < operation.Columns.Length; i++)
                {
                    if (i != 0)
                    {
                        builder.Append(" AND ");
                    }

                    builder
                        .Append(SqlGenerationHelper.DelimitIdentifier(operation.Columns[i]))
                        .Append(" IS NOT NULL");
                }
            }

            EndStatement(builder);
        }
        /// <summary>
        /// 生成确保架构
        /// </summary>
        /// <param name="operation">确保架构操作对象</param>
        /// <param name="model">模型</param>
        /// <param name="builder">迁移命令列表构建器</param>
        protected override void Generate(EnsureSchemaOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            Check.NotNull(operation, nameof(operation));
            Check.NotNull(builder, nameof(builder));

            if (string.Equals(operation.Name, "DBO", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            builder
                .Append("IF SCHEMA_ID(")
                .Append(SqlGenerationHelper.GenerateLiteral(operation.Name))
                .Append(") IS NULL EXEC(N'CREATE SCHEMA ")
                .Append(SqlGenerationHelper.DelimitIdentifier(operation.Name))
                .Append("')");

            EndStatement(builder);
        }
        /// <summary>
        /// 生成创建数据库（Oracle 无效）
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="model"></param>
        /// <param name="builder"></param>
        protected virtual void Generate(
            [NotNull] OracleCreateDatabaseOperation operation,
            [CanBeNull] IModel model,
            [NotNull] MigrationCommandListBuilder builder)
        {
            Check.NotNull(operation, nameof(operation));
            Check.NotNull(builder, nameof(builder));

            builder
                .EndCommand()
                .Append("CREATE DATABASE ")
                .Append(SqlGenerationHelper.DelimitIdentifier(operation.Name))
                .AppendLine(SqlGenerationHelper.StatementTerminator)
                .EndCommand(suppressTransaction: true)
                .Append("IF SERVERPROPERTY('EngineEdition') <> 5 EXEC(N'ALTER DATABASE ")
                .Append(SqlGenerationHelper.DelimitIdentifier(operation.Name))
                .Append(" SET READ_COMMITTED_SNAPSHOT ON')");

            EndStatement(builder, suppressTransaction: true);
        }
        /// <summary>
        /// 生成分离数据库（Oracle 无效）
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="model"></param>
        /// <param name="builder"></param>
        protected virtual void Generate(
            [NotNull] OracleDropDatabaseOperation operation,
            [CanBeNull] IModel model,
            [NotNull] MigrationCommandListBuilder builder)
        {
            Check.NotNull(operation, nameof(operation));
            Check.NotNull(builder, nameof(builder));

            builder
                .EndCommand()
                .Append("IF SERVERPROPERTY('EngineEdition') <> 5 EXEC(N'ALTER DATABASE ")
                .Append(SqlGenerationHelper.DelimitIdentifier(operation.Name))
                .Append(" SET SINGLE_USER WITH ROLLBACK IMMEDIATE')")
                .AppendLine(SqlGenerationHelper.StatementTerminator)
                .EndCommand(suppressTransaction: true)
                .Append("DROP DATABASE ")
                .Append(SqlGenerationHelper.DelimitIdentifier(operation.Name));

            EndStatement(builder, suppressTransaction: true);
        }
        /// <summary>
        /// 生成分离索引
        /// </summary>
        /// <param name="operation">分离索引操作对象</param>
        /// <param name="model">模型</param>
        /// <param name="builder">迁移命令列表构建器</param>
        protected override void Generate(
            DropIndexOperation operation,
            IModel model,
            MigrationCommandListBuilder builder)
        {
            Check.NotNull(operation, nameof(operation));
            Check.NotNull(builder, nameof(builder));

            builder
                .Append("DROP INDEX ")
                .Append(SqlGenerationHelper.DelimitIdentifier(operation.Name))
                .Append(" ON ")
                .Append(SqlGenerationHelper.DelimitIdentifier(operation.Table, operation.Schema));

            EndStatement(builder);
        }
        /// <summary>
        /// 生成分离列
        /// </summary>
        /// <param name="operation">分离列操作对象</param>
        /// <param name="model">模型</param>
        /// <param name="builder">迁移命令列表构建器</param>
        protected override void Generate(
            DropColumnOperation operation,
            IModel model,
            MigrationCommandListBuilder builder)
        {
            Check.NotNull(operation, nameof(operation));
            Check.NotNull(builder, nameof(builder));

            DropDefaultConstraint(operation.Schema, operation.Table, operation.Name, builder);
            base.Generate(operation, model, builder);
        }
        /// <summary>
        /// 生成重命名列
        /// </summary>
        /// <param name="operation">重命名列操作对象</param>
        /// <param name="model">模型</param>
        /// <param name="builder">迁移命令列表构建器</param>
        protected override void Generate(
            RenameColumnOperation operation,
            IModel model,
            MigrationCommandListBuilder builder)
        {
            Check.NotNull(operation, nameof(operation));
            Check.NotNull(builder, nameof(builder));

            var qualifiedName = new StringBuilder();
            if (operation.Schema != null)
            {
                qualifiedName
                    .Append(operation.Schema)
                    .Append(".");
            }
            qualifiedName
                .Append(operation.Table)
                .Append(".")
                .Append(operation.Name);

            Rename(qualifiedName.ToString(), operation.NewName, "COLUMN", builder);
            EndStatement(builder);
        }
        /// <summary>
        /// 列定义
        /// </summary>
        /// <param name="schema">架构</param>
        /// <param name="table">表</param>
        /// <param name="name">列名</param>
        /// <param name="clrType">clr 类型</param>
        /// <param name="type">数据类型</param>
        /// <param name="nullable">是否可为空</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="defaultValueSql">默认值 sql</param>
        /// <param name="computedColumnSql">计算列 sql</param>
        /// <param name="annotatable">注解</param>
        /// <param name="model">模型</param>
        /// <param name="builder">迁移命令列表构建器</param>
        protected override void ColumnDefinition(
            string schema,
            string table,
            string name,
            Type clrType,
            string type,
            bool nullable,
            object defaultValue,
            string defaultValueSql,
            string computedColumnSql,
            IAnnotatable annotatable,
            IModel model,
            MigrationCommandListBuilder builder)
        {
            var valueGenerationStrategy = annotatable[
                OracleFullAnnotationNames.Instance.ValueGenerationStrategy] as OracleValueGenerationStrategy?;

            ColumnDefinition(
                schema,
                table,
                name,
                clrType,
                type,
                nullable,
                defaultValue,
                defaultValueSql,
                computedColumnSql,
                valueGenerationStrategy == OracleValueGenerationStrategy.IdentityColumn,
                annotatable,
                model,
                builder);
        }
        /// <summary>
        /// 列定义
        /// </summary>
        /// <param name="schema">架构</param>
        /// <param name="table">表</param>
        /// <param name="name">列名</param>
        /// <param name="clrType">clr 类型</param>
        /// <param name="type">数据类型</param>
        /// <param name="nullable">是否可为空</param>
        /// <param name="defaultValue">默认值</param>
        /// <param name="defaultValueSql">默认值 sql</param>
        /// <param name="computedColumnSql">计算列 sql</param>
        /// <param name="identity">是否唯一标示</param>
        /// <param name="annotatable">注解</param>
        /// <param name="model">模型</param>
        /// <param name="builder">迁移命令列表构建器</param>
        protected virtual void ColumnDefinition(
            [CanBeNull] string schema,
            [NotNull] string table,
            [NotNull] string name,
            [NotNull] Type clrType,
            [CanBeNull] string type,
            bool nullable,
            [CanBeNull] object defaultValue,
            [CanBeNull] string defaultValueSql,
            [CanBeNull] string computedColumnSql,
            bool identity,
            [NotNull] IAnnotatable annotatable,
            [CanBeNull] IModel model,
            [NotNull] MigrationCommandListBuilder builder)
        {
            Check.NotEmpty(name, nameof(name));
            Check.NotNull(clrType, nameof(clrType));
            Check.NotNull(annotatable, nameof(annotatable));
            Check.NotNull(builder, nameof(builder));

            if (computedColumnSql != null)
            {
                builder
                    .Append(SqlGenerationHelper.DelimitIdentifier(name))
                    .Append(" AS ")
                    .Append(computedColumnSql);

                return;
            }

            base.ColumnDefinition(
                schema,
                table,
                name,
                clrType,
                type,
                nullable,
                defaultValue,
                defaultValueSql,
                computedColumnSql,
                annotatable,
                model,
                builder);

            if (identity)
            {
                builder.Append(" IDENTITY");
            }
        }

        protected virtual void Rename(
            [NotNull] string name,
            [NotNull] string newName,
            [NotNull] MigrationCommandListBuilder builder) => Rename(name, newName, /*type:*/ null, builder);

        protected virtual void Rename(
            [NotNull] string name,
            [NotNull] string newName,
            [CanBeNull] string type,
            [NotNull] MigrationCommandListBuilder builder)
        {
            Check.NotEmpty(name, nameof(name));
            Check.NotEmpty(newName, nameof(newName));
            Check.NotNull(builder, nameof(builder));

            builder
                .Append("EXEC sp_rename ")
                .Append(SqlGenerationHelper.GenerateLiteral(name))
                .Append(", ")
                .Append(SqlGenerationHelper.GenerateLiteral(newName));

            if (type != null)
            {
                builder
                    .Append(", ")
                    .Append(SqlGenerationHelper.GenerateLiteral(type));
            }
        }

        protected virtual void Transfer(
            [NotNull] string newSchema,
            [CanBeNull] string schema,
            [NotNull] string name,
            [NotNull] MigrationCommandListBuilder builder)
        {
            Check.NotEmpty(newSchema, nameof(newSchema));
            Check.NotEmpty(name, nameof(name));
            Check.NotNull(builder, nameof(builder));

            builder
                .Append("ALTER SCHEMA ")
                .Append(SqlGenerationHelper.DelimitIdentifier(newSchema))
                .Append(" TRANSFER ")
                .Append(SqlGenerationHelper.DelimitIdentifier(name, schema));
        }

        protected override void IndexTraits(MigrationOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            Check.NotNull(operation, nameof(operation));
            Check.NotNull(builder, nameof(builder));

            var clustered = operation[OracleFullAnnotationNames.Instance.Clustered] as bool?;
            if (clustered.HasValue)
            {
                builder.Append(clustered.Value ? "CLUSTERED " : "NONCLUSTERED ");
            }
        }

        protected override void ForeignKeyAction(ReferentialAction referentialAction, MigrationCommandListBuilder builder)
        {
            Check.NotNull(builder, nameof(builder));

            if (referentialAction == ReferentialAction.Restrict)
            {
                builder.Append("NO ACTION");
            }
            else
            {
                base.ForeignKeyAction(referentialAction, builder);
            }
        }

        protected virtual void DropDefaultConstraint(
            [CanBeNull] string schema,
            [NotNull] string tableName,
            [NotNull] string columnName,
            [NotNull] MigrationCommandListBuilder builder)
        {
            Check.NotEmpty(tableName, nameof(tableName));
            Check.NotEmpty(columnName, nameof(columnName));
            Check.NotNull(builder, nameof(builder));

            var variable = "@var" + _variableCounter++;

            builder
                .Append("DECLARE ")
                .Append(variable)
                .AppendLine(" sysname;")
                .Append("SELECT ")
                .Append(variable)
                .AppendLine(" = [d].[name]")
                .AppendLine("FROM [sys].[default_constraints] [d]")
                .AppendLine("INNER JOIN [sys].[columns] [c] ON [d].[parent_column_id] = [c].[column_id] AND [d].[parent_object_id] = [c].[object_id]")
                .Append("WHERE ([d].[parent_object_id] = OBJECT_ID(N'");

            if (schema != null)
            {
                builder
                    .Append(SqlGenerationHelper.EscapeLiteral(schema))
                    .Append(".");
            }

            builder
                .Append(SqlGenerationHelper.EscapeLiteral(tableName))
                .Append("') AND [c].[name] = N'")
                .Append(SqlGenerationHelper.EscapeLiteral(columnName))
                .AppendLine("');")
                .Append("IF ")
                .Append(variable)
                .Append(" IS NOT NULL EXEC(N'ALTER TABLE ")
                .Append(SqlGenerationHelper.DelimitIdentifier(tableName, schema))
                .Append(" DROP CONSTRAINT [' + ")
                .Append(variable)
                .AppendLine(" + ']');");
        }
    }
}
