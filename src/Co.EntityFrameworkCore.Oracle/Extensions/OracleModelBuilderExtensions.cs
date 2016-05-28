// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore;
using Co.EntityFrameworkCore.Metadata;

// ReSharper disable once CheckNamespace
namespace Co.EntityFrameworkCore
{
    public static class OracleModelBuilderExtensions
    {
        public static RelationalSequenceBuilder ForOracleHasSequence(
            [NotNull] this ModelBuilder modelBuilder,
            [NotNull] string name,
            [CanBeNull] string schema = null)
        {
            Check.NotNull(modelBuilder, nameof(modelBuilder));
            Check.NotEmpty(name, nameof(name));
            Check.NullButNotEmpty(schema, nameof(schema));

            return new RelationalSequenceBuilder(modelBuilder.Model.Oracle().GetOrAddSequence(name, schema));
        }

        public static ModelBuilder ForOracleHasSequence(
            [NotNull] this ModelBuilder modelBuilder,
            [NotNull] string name,
            [NotNull] Action<RelationalSequenceBuilder> builderAction)
            => modelBuilder.ForOracleHasSequence(name, null, builderAction);

        public static ModelBuilder ForOracleHasSequence(
            [NotNull] this ModelBuilder modelBuilder,
            [NotNull] string name,
            [CanBeNull] string schema,
            [NotNull] Action<RelationalSequenceBuilder> builderAction)
        {
            Check.NotNull(modelBuilder, nameof(modelBuilder));
            Check.NotEmpty(name, nameof(name));
            Check.NullButNotEmpty(schema, nameof(schema));
            Check.NotNull(builderAction, nameof(builderAction));

            builderAction(ForOracleHasSequence(modelBuilder, name, schema));

            return modelBuilder;
        }

        public static RelationalSequenceBuilder ForOracleHasSequence<T>(
            [NotNull] this ModelBuilder modelBuilder,
            [NotNull] string name,
            [CanBeNull] string schema = null)
        {
            Check.NotNull(modelBuilder, nameof(modelBuilder));
            Check.NotEmpty(name, nameof(name));
            Check.NullButNotEmpty(schema, nameof(schema));

            var sequence = modelBuilder.Model.Oracle().GetOrAddSequence(name, schema);
            sequence.ClrType = typeof(T);

            return new RelationalSequenceBuilder(sequence);
        }

        public static ModelBuilder ForOracleHasSequence<T>(
            [NotNull] this ModelBuilder modelBuilder,
            [NotNull] string name,
            [NotNull] Action<RelationalSequenceBuilder> builderAction)
            => modelBuilder.ForOracleHasSequence<T>(name, null, builderAction);

        public static ModelBuilder ForOracleHasSequence<T>(
            [NotNull] this ModelBuilder modelBuilder,
            [NotNull] string name,
            [CanBeNull] string schema,
            [NotNull] Action<RelationalSequenceBuilder> builderAction)
        {
            Check.NotNull(modelBuilder, nameof(modelBuilder));
            Check.NotEmpty(name, nameof(name));
            Check.NullButNotEmpty(schema, nameof(schema));
            Check.NotNull(builderAction, nameof(builderAction));

            builderAction(ForOracleHasSequence<T>(modelBuilder, name, schema));

            return modelBuilder;
        }

        public static RelationalSequenceBuilder ForOracleHasSequence(
            [NotNull] this ModelBuilder modelBuilder,
            [NotNull] Type clrType,
            [NotNull] string name,
            [CanBeNull] string schema = null)
        {
            Check.NotNull(clrType, nameof(clrType));
            Check.NotNull(modelBuilder, nameof(modelBuilder));
            Check.NotEmpty(name, nameof(name));
            Check.NullButNotEmpty(schema, nameof(schema));

            var sequence = modelBuilder.Model.Oracle().GetOrAddSequence(name, schema);
            sequence.ClrType = clrType;

            return new RelationalSequenceBuilder(sequence);
        }

        public static ModelBuilder ForOracleHasSequence(
            [NotNull] this ModelBuilder modelBuilder,
            [NotNull] Type clrType,
            [NotNull] string name,
            [NotNull] Action<RelationalSequenceBuilder> builderAction)
            => modelBuilder.ForOracleHasSequence(clrType, name, null, builderAction);

        public static ModelBuilder ForOracleHasSequence(
            [NotNull] this ModelBuilder modelBuilder,
            [NotNull] Type clrType,
            [NotNull] string name,
            [CanBeNull] string schema,
            [NotNull] Action<RelationalSequenceBuilder> builderAction)
        {
            Check.NotNull(modelBuilder, nameof(modelBuilder));
            Check.NotNull(clrType, nameof(clrType));
            Check.NotEmpty(name, nameof(name));
            Check.NullButNotEmpty(schema, nameof(schema));
            Check.NotNull(builderAction, nameof(builderAction));

            builderAction(ForOracleHasSequence(modelBuilder, clrType, name, schema));

            return modelBuilder;
        }

        public static ModelBuilder ForOracleUseSequenceHiLo(
            [NotNull] this ModelBuilder modelBuilder,
            [CanBeNull] string name = null,
            [CanBeNull] string schema = null)
        {
            Check.NotNull(modelBuilder, nameof(modelBuilder));
            Check.NullButNotEmpty(name, nameof(name));
            Check.NullButNotEmpty(schema, nameof(schema));

            var model = modelBuilder.Model;

            name = name ?? OracleModelAnnotations.DefaultHiLoSequenceName;

            if (model.Oracle().FindSequence(name, schema) == null)
            {
                modelBuilder.ForOracleHasSequence(name, schema).IncrementsBy(10);
            }

            model.Oracle().ValueGenerationStrategy = OracleValueGenerationStrategy.SequenceHiLo;
            model.Oracle().HiLoSequenceName = name;
            model.Oracle().HiLoSequenceSchema = schema;

            return modelBuilder;
        }

        public static ModelBuilder ForOracleUseIdentityColumns(
            [NotNull] this ModelBuilder modelBuilder)
        {
            Check.NotNull(modelBuilder, nameof(modelBuilder));

            var property = modelBuilder.Model;

            property.Oracle().ValueGenerationStrategy = OracleValueGenerationStrategy.IdentityColumn;
            property.Oracle().HiLoSequenceName = null;
            property.Oracle().HiLoSequenceSchema = null;

            return modelBuilder;
        }
    }
}
