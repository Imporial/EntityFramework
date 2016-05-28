// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Co.EntityFrameworkCore.Metadata;
using Co.EntityFrameworkCore.Metadata.Internal;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

// ReSharper disable once CheckNamespace
namespace Co.EntityFrameworkCore
{
    public static class OraclePropertyBuilderExtensions
    {
        public static PropertyBuilder ForSqlServerHasColumnName(
            [NotNull] this PropertyBuilder propertyBuilder,
            [CanBeNull] string name)
        {
            Check.NotNull(propertyBuilder, nameof(propertyBuilder));
            Check.NullButNotEmpty(name, nameof(name));

            propertyBuilder.Metadata.Oracle().ColumnName = name;

            return propertyBuilder;
        }

        public static PropertyBuilder<TProperty> ForSqlServerHasColumnName<TProperty>(
            [NotNull] this PropertyBuilder<TProperty> propertyBuilder,
            [CanBeNull] string name)
            => (PropertyBuilder<TProperty>)ForSqlServerHasColumnName((PropertyBuilder)propertyBuilder, name);

        public static PropertyBuilder ForSqlServerHasColumnType(
            [NotNull] this PropertyBuilder propertyBuilder,
            [CanBeNull] string typeName)
        {
            Check.NotNull(propertyBuilder, nameof(propertyBuilder));
            Check.NullButNotEmpty(typeName, nameof(typeName));

            propertyBuilder.Metadata.Oracle().ColumnType = typeName;

            return propertyBuilder;
        }

        public static PropertyBuilder<TProperty> ForSqlServerHasColumnType<TProperty>(
            [NotNull] this PropertyBuilder<TProperty> propertyBuilder,
            [CanBeNull] string typeName)
            => (PropertyBuilder<TProperty>)ForSqlServerHasColumnType((PropertyBuilder)propertyBuilder, typeName);

        public static PropertyBuilder ForSqlServerHasDefaultValueSql(
            [NotNull] this PropertyBuilder propertyBuilder,
            [CanBeNull] string sql)
        {
            Check.NotNull(propertyBuilder, nameof(propertyBuilder));
            Check.NullButNotEmpty(sql, nameof(sql));

            var internalPropertyBuilder = propertyBuilder.GetInfrastructure<InternalPropertyBuilder>();
            internalPropertyBuilder.Oracle(ConfigurationSource.Explicit).DefaultValueSql(sql);

            return propertyBuilder;
        }

        public static PropertyBuilder<TProperty> ForSqlServerHasDefaultValueSql<TProperty>(
            [NotNull] this PropertyBuilder<TProperty> propertyBuilder,
            [CanBeNull] string sql)
            => (PropertyBuilder<TProperty>)ForSqlServerHasDefaultValueSql((PropertyBuilder)propertyBuilder, sql);

        public static PropertyBuilder ForSqlServerHasDefaultValue(
            [NotNull] this PropertyBuilder propertyBuilder,
            [CanBeNull] object value)
        {
            Check.NotNull(propertyBuilder, nameof(propertyBuilder));

            var internalPropertyBuilder = propertyBuilder.GetInfrastructure<InternalPropertyBuilder>();
            internalPropertyBuilder.Oracle(ConfigurationSource.Explicit).DefaultValue(value);

            return propertyBuilder;
        }

        public static PropertyBuilder<TProperty> ForSqlServerHasDefaultValue<TProperty>(
            [NotNull] this PropertyBuilder<TProperty> propertyBuilder,
            [CanBeNull] object value)
            => (PropertyBuilder<TProperty>)ForSqlServerHasDefaultValue((PropertyBuilder)propertyBuilder, value);

        public static PropertyBuilder ForSqlServerHasComputedColumnSql(
            [NotNull] this PropertyBuilder propertyBuilder,
            [CanBeNull] string sql)
        {
            Check.NotNull(propertyBuilder, nameof(propertyBuilder));
            Check.NullButNotEmpty(sql, nameof(sql));

            var internalPropertyBuilder = propertyBuilder.GetInfrastructure<InternalPropertyBuilder>();
            internalPropertyBuilder.Oracle(ConfigurationSource.Explicit).ComputedColumnSql(sql);

            return propertyBuilder;
        }

        public static PropertyBuilder<TProperty> ForSqlServerHasComputedColumnSql<TProperty>(
            [NotNull] this PropertyBuilder<TProperty> propertyBuilder,
            [CanBeNull] string sql)
            => (PropertyBuilder<TProperty>)ForSqlServerHasComputedColumnSql((PropertyBuilder)propertyBuilder, sql);

        public static PropertyBuilder ForSqlServerUseSequenceHiLo(
            [NotNull] this PropertyBuilder propertyBuilder,
            [CanBeNull] string name = null,
            [CanBeNull] string schema = null)
        {
            Check.NotNull(propertyBuilder, nameof(propertyBuilder));
            Check.NullButNotEmpty(name, nameof(name));
            Check.NullButNotEmpty(schema, nameof(schema));

            var property = propertyBuilder.Metadata;

            name = name ?? OracleModelAnnotations.DefaultHiLoSequenceName;

            var model = property.DeclaringEntityType.Model;

            if (model.Oracle().FindSequence(name, schema) == null)
            {
                model.Oracle().GetOrAddSequence(name, schema).IncrementBy = 10;
            }

            property.Oracle().ValueGenerationStrategy = OracleValueGenerationStrategy.SequenceHiLo;
            property.ValueGenerated = ValueGenerated.OnAdd;
            property.RequiresValueGenerator = true;
            property.Oracle().HiLoSequenceName = name;
            property.Oracle().HiLoSequenceSchema = schema;

            return propertyBuilder;
        }

        public static PropertyBuilder<TProperty> ForSqlServerUseSequenceHiLo<TProperty>(
            [NotNull] this PropertyBuilder<TProperty> propertyBuilder,
            [CanBeNull] string name = null,
            [CanBeNull] string schema = null)
            => (PropertyBuilder<TProperty>)ForSqlServerUseSequenceHiLo((PropertyBuilder)propertyBuilder, name, schema);

        public static PropertyBuilder UseSqlServerIdentityColumn(
            [NotNull] this PropertyBuilder propertyBuilder)
        {
            Check.NotNull(propertyBuilder, nameof(propertyBuilder));

            var property = propertyBuilder.Metadata;

            property.Oracle().ValueGenerationStrategy = OracleValueGenerationStrategy.IdentityColumn;
            property.ValueGenerated = ValueGenerated.OnAdd;
            property.RequiresValueGenerator = true;
            property.Oracle().HiLoSequenceName = null;
            property.Oracle().HiLoSequenceSchema = null;

            return propertyBuilder;
        }

        public static PropertyBuilder<TProperty> UseSqlServerIdentityColumn<TProperty>(
            [NotNull] this PropertyBuilder<TProperty> propertyBuilder)
            => (PropertyBuilder<TProperty>)UseSqlServerIdentityColumn((PropertyBuilder)propertyBuilder);
    }
}
