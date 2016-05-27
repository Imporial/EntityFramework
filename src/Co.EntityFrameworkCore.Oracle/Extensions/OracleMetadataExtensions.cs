// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

// ReSharper disable once CheckNamespace
namespace Co.EntityFrameworkCore
{
    public static class OracleMetadataExtensions
    {
        public static OraclePropertyAnnotations Oracle([NotNull] this IMutableProperty property)
            => (OraclePropertyAnnotations)Oracle((IProperty)property);

        public static IOraclePropertyAnnotations Oracle([NotNull] this IProperty property)
            => new OraclePropertyAnnotations(Check.NotNull(property, nameof(property)));

        public static RelationalEntityTypeAnnotations Oracle([NotNull] this IMutableEntityType entityType)
            => (RelationalEntityTypeAnnotations)Oracle((IEntityType)entityType);

        public static IRelationalEntityTypeAnnotations Oracle([NotNull] this IEntityType entityType)
            => new RelationalEntityTypeAnnotations(Check.NotNull(entityType, nameof(entityType)), OracleFullAnnotationNames.Instance);

        public static OracleKeyAnnotations Oracle([NotNull] this IMutableKey key)
            => (OracleKeyAnnotations)Oracle((IKey)key);

        public static IOracleKeyAnnotations Oracle([NotNull] this IKey key)
            => new OracleKeyAnnotations(Check.NotNull(key, nameof(key)));

        public static OracleIndexAnnotations Oracle([NotNull] this IMutableIndex index)
            => (OracleIndexAnnotations)Oracle((IIndex)index);

        public static IOracleIndexAnnotations Oracle([NotNull] this IIndex index)
            => new OracleIndexAnnotations(Check.NotNull(index, nameof(index)));

        public static RelationalForeignKeyAnnotations Oracle([NotNull] this IMutableForeignKey foreignKey)
            => (RelationalForeignKeyAnnotations)Oracle((IForeignKey)foreignKey);

        public static IRelationalForeignKeyAnnotations Oracle([NotNull] this IForeignKey foreignKey)
            => new RelationalForeignKeyAnnotations(Check.NotNull(foreignKey, nameof(foreignKey)), OracleFullAnnotationNames.Instance);

        public static OracleModelAnnotations Oracle([NotNull] this IMutableModel model)
            => (OracleModelAnnotations)Oracle((IModel)model);

        public static IOracleModelAnnotations Oracle([NotNull] this IModel model)
            => new OracleModelAnnotations(Check.NotNull(model, nameof(model)));
    }
}
