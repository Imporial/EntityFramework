// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Conventions.Internal;

namespace Co.EntityFrameworkCore.Infrastructure.Internal
{
    public class OracleModelSource : RelationalModelSource
    {
        public OracleModelSource(
            [NotNull] IDbSetFinder setFinder,
            [NotNull] ICoreConventionSetBuilder coreConventionSetBuilder,
            [NotNull] IModelCustomizer modelCustomizer,
            [NotNull] IModelCacheKeyFactory modelCacheKeyFactory)
            : base(setFinder, coreConventionSetBuilder, modelCustomizer, modelCacheKeyFactory)
        {
        }
    }
}
