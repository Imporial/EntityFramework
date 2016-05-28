// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Co.EntityFrameworkCore.Metadata.Internal
{
    public static class OracleInternalMetadataBuilderExtensions
    {
        public static OracleModelBuilderAnnotations Oracle(
            [NotNull] this InternalModelBuilder builder,
            ConfigurationSource configurationSource)
            => new OracleModelBuilderAnnotations(builder, configurationSource);

        public static OraclePropertyBuilderAnnotations Oracle(
            [NotNull] this InternalPropertyBuilder builder,
            ConfigurationSource configurationSource)
            => new OraclePropertyBuilderAnnotations(builder, configurationSource);

        public static RelationalEntityTypeBuilderAnnotations Oracle(
            [NotNull] this InternalEntityTypeBuilder builder,
            ConfigurationSource configurationSource)
            => new RelationalEntityTypeBuilderAnnotations(builder, configurationSource, OracleFullAnnotationNames.Instance);

        public static OracleKeyBuilderAnnotations Oracle(
            [NotNull] this InternalKeyBuilder builder,
            ConfigurationSource configurationSource)
            => new OracleKeyBuilderAnnotations(builder, configurationSource);

        public static OracleIndexBuilderAnnotations Oracle(
            [NotNull] this InternalIndexBuilder builder,
            ConfigurationSource configurationSource)
            => new OracleIndexBuilderAnnotations(builder, configurationSource);

        public static RelationalForeignKeyBuilderAnnotations Oracle(
            [NotNull] this InternalRelationshipBuilder builder,
            ConfigurationSource configurationSource)
            => new RelationalForeignKeyBuilderAnnotations(builder, configurationSource, OracleFullAnnotationNames.Instance);
    }
}
