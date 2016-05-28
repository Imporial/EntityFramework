// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Co.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Co.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Migrations;
using Co.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using Co.EntityFrameworkCore.Query.ExpressionTranslators.Internal;
using Co.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query.Sql;
using Co.EntityFrameworkCore.Query.Sql.Internal;
using Microsoft.EntityFrameworkCore.Update;
using Co.EntityFrameworkCore.Update.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using Co.EntityFrameworkCore.ValueGeneration.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Co.EntityFrameworkCore.Migrations;
using Co.EntityFrameworkCore.Metadata;

namespace Co.EntityFrameworkCore.Storage.Internal
{
    public class OracleDatabaseProviderServices : RelationalDatabaseProviderServices
    {
        public OracleDatabaseProviderServices([NotNull] IServiceProvider services)
            : base(services)
        {
        }

        public override string InvariantName => GetType().GetTypeInfo().Assembly.GetName().Name;
        public override IDatabaseCreator Creator => GetService<OracleDatabaseCreator>();
        public override IRelationalConnection RelationalConnection => GetService<IOracleConnection>();
        public override ISqlGenerationHelper SqlGenerationHelper => GetService<OracleSqlGenerationHelper>();
        public override IValueGeneratorSelector ValueGeneratorSelector => GetService<OracleValueGeneratorSelector>();
        public override IRelationalDatabaseCreator RelationalDatabaseCreator => GetService<OracleDatabaseCreator>();
        public override IConventionSetBuilder ConventionSetBuilder => GetService<OracleConventionSetBuilder>();
        public override IMigrationsAnnotationProvider MigrationsAnnotationProvider => GetService<OracleMigrationsAnnotationProvider>();
        public override IHistoryRepository HistoryRepository => GetService<OracleHistoryRepository>();
        public override IMigrationsSqlGenerator MigrationsSqlGenerator => GetService<OracleMigrationsSqlGenerator>();
        public override IModelSource ModelSource => GetService<OracleModelSource>();
        public override IUpdateSqlGenerator UpdateSqlGenerator => GetService<IOracleUpdateSqlGenerator>();
        public override IValueGeneratorCache ValueGeneratorCache => GetService<IOracleValueGeneratorCache>();
        public override IRelationalTypeMapper TypeMapper => GetService<OracleTypeMapper>();
        public override IModificationCommandBatchFactory ModificationCommandBatchFactory => GetService<OracleModificationCommandBatchFactory>();
        public override IRelationalValueBufferFactoryFactory ValueBufferFactoryFactory => GetService<UntypedRelationalValueBufferFactoryFactory>();
        public override IRelationalAnnotationProvider AnnotationProvider => GetService<OracleAnnotationProvider>();
        public override IMethodCallTranslator CompositeMethodCallTranslator => GetService<OracleCompositeMethodCallTranslator>();
        public override IMemberTranslator CompositeMemberTranslator => GetService<OracleCompositeMemberTranslator>();
        public override IQueryCompilationContextFactory QueryCompilationContextFactory => GetService<OracleQueryCompilationContextFactory>();
        public override IQuerySqlGeneratorFactory QuerySqlGeneratorFactory => GetService<OracleQuerySqlGeneratorFactory>();
        public override IEntityQueryModelVisitorFactory EntityQueryModelVisitorFactory => GetService<OracleQueryModelVisitorFactory>();
        public override ICompiledQueryCacheKeyGenerator CompiledQueryCacheKeyGenerator => GetService<OracleCompiledQueryCacheKeyGenerator>();
    }
}
