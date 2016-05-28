// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Co.EntityFrameworkCore.Metadata.Conventions.Internal;
using Co.EntityFrameworkCore.Storage.Internal;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Co.EntityFrameworkCore.Metadata.Conventions
{
    public class OracleConventionSetBuilder : RelationalConventionSetBuilder
    {
        public OracleConventionSetBuilder(
            [NotNull] IRelationalTypeMapper typeMapper,
            [CanBeNull] ICurrentDbContext currentContext,
            [CanBeNull] IDbSetFinder setFinder)
            : base(typeMapper, currentContext, setFinder)
        {
        }

        public override ConventionSet AddConventions(ConventionSet conventionSet)
        {
            Check.NotNull(conventionSet, nameof(conventionSet));

            base.AddConventions(conventionSet);

            conventionSet.ModelInitializedConventions.Add(new OracleValueGenerationStrategyConvention());

            return conventionSet;
        }

        public static ConventionSet Build()
            => new OracleConventionSetBuilder(new OracleTypeMapper(), null, null)
                .AddConventions(new CoreConventionSetBuilder().CreateConventionSet());
    }
}
