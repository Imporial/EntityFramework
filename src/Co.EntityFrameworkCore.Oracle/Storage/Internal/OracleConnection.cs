// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Data.Common;
using System.Data.SqlClient;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore;

namespace Co.EntityFrameworkCore.Storage.Internal
{
    public class OracleConnection : RelationalConnection, IOracleConnection
    {
        private bool? _multipleActiveResultSetsEnabled;

        // Compensate for slow SQL Server database creation
        internal const int DefaultMasterConnectionCommandTimeout = 60;

        public OracleConnection(
            [NotNull] IDbContextOptions options,
            // ReSharper disable once SuggestBaseTypeForParameter
            [NotNull] ILogger<OracleConnection> logger)
            : base(options, logger)
        {
        }

        private OracleConnection(
            [NotNull] IDbContextOptions options, [NotNull] ILogger logger)
            : base(options, logger)
        {
        }

        protected override DbConnection CreateDbConnection() => new SqlConnection(ConnectionString);

        // TODO use clone connection method once implemented see #1406
        public virtual IOracleConnection CreateMasterConnection()
            => new OracleConnection(new DbContextOptionsBuilder()
                .UseOracle(
                    new SqlConnectionStringBuilder { ConnectionString = ConnectionString, InitialCatalog = "master" }.ConnectionString,
                    b => b.CommandTimeout(CommandTimeout ?? DefaultMasterConnectionCommandTimeout)).Options, Logger);

        public override bool IsMultipleActiveResultSetsEnabled
            => (bool)(_multipleActiveResultSetsEnabled
                      ?? (_multipleActiveResultSetsEnabled
                          = new SqlConnectionStringBuilder(ConnectionString).MultipleActiveResultSets));
    }
}
