// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Co.EntityFrameworkCore.Infrastructure.Internal;
using Co.EntityFrameworkCore.Metadata;
using Co.EntityFrameworkCore.Metadata.Conventions;
using Co.EntityFrameworkCore.Migrations;
using Co.EntityFrameworkCore.Migrations.Internal;
using Co.EntityFrameworkCore.Query.ExpressionTranslators.Internal;
using Co.EntityFrameworkCore.Query.Internal;
using Co.EntityFrameworkCore.Query.Sql.Internal;
using Co.EntityFrameworkCore.Storage.Internal;
using Co.EntityFrameworkCore.Update.Internal;
using Co.EntityFrameworkCore.ValueGeneration.Internal;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

// ReSharper disable once CheckNamespace
namespace Co.Extensions.DependencyInjection
{
    public static class OracleServiceCollectionExtensions
    {
        /// <summary>
        ///     <para>
        ///         Adds the services required by the Microsoft SQL Server database provider for Entity Framework
        ///         to an <see cref="IServiceCollection" />. You use this method when using dependency injection
        ///         in your application, such as with ASP.NET. For more information on setting up dependency
        ///         injection, see http://go.microsoft.com/fwlink/?LinkId=526890.
        ///     </para>
        ///     <para>
        ///         You only need to use this functionality when you want Entity Framework to resolve the services it uses
        ///         from an external <see cref="IServiceCollection" />. If you are not using an external
        ///         <see cref="IServiceCollection" /> Entity Framework will take care of creating the services it requires.
        ///     </para>
        /// </summary>
        /// <example>
        ///     <code>
        ///         public void ConfigureServices(IServiceCollection services) 
        ///         {
        ///             var connectionString = "connection string to database";
        /// 
        ///             services
        ///                 .AddEntityFrameworkSqlServer()
        ///                 .AddDbContext&lt;MyContext&gt;(options => options.UseSqlServer(connectionString)); 
        ///         }
        ///     </code>
        /// </example>
        /// <param name="services"> The <see cref="IServiceCollection" /> to add services to. </param>
        /// <returns>
        ///     A builder that allows further Entity Framework specific setup of the <see cref="IServiceCollection" />.
        /// </returns>
        public static IServiceCollection AddEntityFrameworkOracle([NotNull] this IServiceCollection services)
        {
            Check.NotNull(services, nameof(services));

            services.AddRelational();

            services.TryAddEnumerable(ServiceDescriptor
                .Singleton<IDatabaseProvider, DatabaseProvider<OracleDatabaseProviderServices, OracleOptionsExtension>>());

            services.TryAdd(new ServiceCollection()
                .AddSingleton<IOracleValueGeneratorCache, OracleValueGeneratorCache>()
                .AddSingleton<OracleTypeMapper>()
                .AddSingleton<OracleSqlGenerationHelper>()
                .AddSingleton<OracleModelSource>()
                .AddSingleton<OracleAnnotationProvider>()
                .AddSingleton<OracleMigrationsAnnotationProvider>()
                .AddScoped<OracleConventionSetBuilder>()
                .AddScoped<IOracleUpdateSqlGenerator, OracleUpdateSqlGenerator>()
                .AddScoped<IOracleSequenceValueGeneratorFactory, OracleSequenceValueGeneratorFactory>()
                .AddScoped<OracleModificationCommandBatchFactory>()
                .AddScoped<OracleValueGeneratorSelector>()
                .AddScoped<OracleDatabaseProviderServices>()
                .AddScoped<IOracleConnection, OracleConnection>()
                .AddScoped<OracleMigrationsSqlGenerator>()
                .AddScoped<OracleDatabaseCreator>()
                .AddScoped<OracleHistoryRepository>()
                .AddScoped<OracleQueryModelVisitorFactory>()
                .AddScoped<OracleCompiledQueryCacheKeyGenerator>()
                .AddQuery());

            return services;
        }

        private static IServiceCollection AddQuery(this IServiceCollection serviceCollection)
            => serviceCollection
                .AddScoped<OracleQueryCompilationContextFactory>()
                .AddScoped<OracleCompositeMemberTranslator>()
                .AddScoped<OracleCompositeMethodCallTranslator>()
                .AddScoped<OracleQuerySqlGeneratorFactory>();
    }
}
