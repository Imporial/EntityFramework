// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Co.EntityFrameworkCore.Storage.Internal;
using Co.EntityFrameworkCore.Update.Internal;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System;
using System.Globalization;

namespace Co.EntityFrameworkCore.ValueGeneration.Internal
{
    public class OracleSequenceHiLoValueGenerator<TValue> : HiLoValueGenerator<TValue>
    {
        private readonly IRawSqlCommandBuilder _rawSqlCommandBuilder;
        private readonly IOracleUpdateSqlGenerator _sqlGenerator;
        private readonly IOracleConnection _connection;
        private readonly ISequence _sequence;

        public OracleSequenceHiLoValueGenerator(
            [NotNull] IRawSqlCommandBuilder rawSqlCommandBuilder,
            [NotNull] IOracleUpdateSqlGenerator sqlGenerator,
            [NotNull] OracleSequenceValueGeneratorState generatorState,
            [NotNull] IOracleConnection connection)
            : base(generatorState)
        {
            Check.NotNull(rawSqlCommandBuilder, nameof(rawSqlCommandBuilder));
            Check.NotNull(sqlGenerator, nameof(sqlGenerator));
            Check.NotNull(connection, nameof(connection));

            _sequence = generatorState.Sequence;
            _rawSqlCommandBuilder = rawSqlCommandBuilder;
            _sqlGenerator = sqlGenerator;
            _connection = connection;
        }

        protected override long GetNewLowValue()
            => (long)Convert.ChangeType(
                _rawSqlCommandBuilder
                    .Build(_sqlGenerator.GenerateNextSequenceValueOperation(_sequence.Name, _sequence.Schema))
                    .ExecuteScalar(_connection),
                typeof(long),
                CultureInfo.InvariantCulture);

        public override bool GeneratesTemporaryValues => false;
    }
}
