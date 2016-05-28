// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Data.Common;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Co.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore;
using Co.EntityFrameworkCore.Infrastructure;

// ReSharper disable once CheckNamespace
namespace Co.EntityFrameworkCore
{
    public static class OracleDbContextOptionsExtensions
    {
        /// <summary>
        ///     Configures the context to connect to a Microsoft SQL Server database.
        /// </summary>
        /// <param name="optionsBuilder"> A builder for setting options on the context. </param>
        /// <param name="connectionString"> The connection string of the database to connect to. </param>
        /// <param name="oracleOptionsAction">An optional action to allow additional SQL Server specific configuration.</param>
        /// <returns> The options builder so that further configuration can be chained. </returns>
        public static DbContextOptionsBuilder UseOracle(
            [NotNull] this DbContextOptionsBuilder optionsBuilder,
            [NotNull] string connectionString,
            [CanBeNull] Action<OracleDbContextOptionsBuilder> oracleOptionsAction = null)
        {
            Check.NotNull(optionsBuilder, nameof(optionsBuilder));
            Check.NotEmpty(connectionString, nameof(connectionString));

            var extension = GetOrCreateExtension(optionsBuilder);
            extension.ConnectionString = connectionString;
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

            oracleOptionsAction?.Invoke(new OracleDbContextOptionsBuilder(optionsBuilder));

            return optionsBuilder;
        }

        // Note: Decision made to use DbConnection not SqlConnection: Issue #772
        /// <summary>
        ///     Configures the context to connect to a Microsoft SQL Server database.
        /// </summary>
        /// <param name="optionsBuilder"> A builder for setting options on the context. </param>
        /// <param name="connection">
        ///     An existing <see cref="DbConnection" /> to be used to connect to the database. If the connection is
        ///     in the open state then EF will not open or close the connection. If the connection is in the closed
        ///     state then EF will open and close the connection as needed.
        /// </param>
        /// <param name="oracleOptionsAction">An optional action to allow additional SQL Server specific configuration.</param>
        /// <returns> The options builder so that further configuration can be chained. </returns>
        public static DbContextOptionsBuilder UseOracle(
            [NotNull] this DbContextOptionsBuilder optionsBuilder,
            [NotNull] DbConnection connection,
            [CanBeNull] Action<OracleDbContextOptionsBuilder> oracleOptionsAction = null)
        {
            Check.NotNull(optionsBuilder, nameof(optionsBuilder));
            Check.NotNull(connection, nameof(connection));

            var extension = GetOrCreateExtension(optionsBuilder);
            extension.Connection = connection;
            ((IDbContextOptionsBuilderInfrastructure)optionsBuilder).AddOrUpdateExtension(extension);

            oracleOptionsAction?.Invoke(new OracleDbContextOptionsBuilder(optionsBuilder));

            return optionsBuilder;
        }

        /// <summary>
        ///     Configures the context to connect to a Microsoft SQL Server database.
        /// </summary>
        /// <param name="optionsBuilder"> A builder for setting options on the context. </param>
        /// <param name="connectionString"> The connection string of the database to connect to. </param>
        /// <param name="oracleOptionsAction">An optional action to allow additional SQL Server specific configuration.</param>
        /// <returns> The options builder so that further configuration can be chained. </returns>
        public static DbContextOptionsBuilder<TContext> UseOracle<TContext>(
            [NotNull] this DbContextOptionsBuilder<TContext> optionsBuilder,
            [NotNull] string connectionString,
            [CanBeNull] Action<OracleDbContextOptionsBuilder> oracleOptionsAction = null)
            where TContext : DbContext
            => (DbContextOptionsBuilder<TContext>)UseOracle(
                (DbContextOptionsBuilder)optionsBuilder, connectionString, oracleOptionsAction);

        // Note: Decision made to use DbConnection not SqlConnection: Issue #772
        /// <summary>
        ///     Configures the context to connect to a Microsoft SQL Server database.
        /// </summary>
        /// <param name="optionsBuilder"> A builder for setting options on the context. </param>
        /// <param name="connection">
        ///     An existing <see cref="DbConnection" /> to be used to connect to the database. If the connection is
        ///     in the open state then EF will not open or close the connection. If the connection is in the closed
        ///     state then EF will open and close the connection as needed.
        /// </param>
        /// <param name="oracleOptionsAction">An optional action to allow additional SQL Server specific configuration.</param>
        /// <returns> The options builder so that further configuration can be chained. </returns>
        public static DbContextOptionsBuilder<TContext> UseOracle<TContext>(
            [NotNull] this DbContextOptionsBuilder<TContext> optionsBuilder,
            [NotNull] DbConnection connection,
            [CanBeNull] Action<OracleDbContextOptionsBuilder> oracleOptionsAction = null)
            where TContext : DbContext
            => (DbContextOptionsBuilder<TContext>)UseOracle(
                (DbContextOptionsBuilder)optionsBuilder, connection, oracleOptionsAction);

        private static OracleOptionsExtension GetOrCreateExtension(DbContextOptionsBuilder optionsBuilder)
        {
            var existing = optionsBuilder.Options.FindExtension<OracleOptionsExtension>();
            return existing != null
                ? new OracleOptionsExtension(existing)
                : new OracleOptionsExtension();
        }
    }
}
