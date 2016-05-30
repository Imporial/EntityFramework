// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Co.EntityFrameworkCore.Metadata;
using Co.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System.Collections.Generic;

namespace Co.EntityFrameworkCore.Migrations.Internal
{
    /// <summary>
    /// Oracle 迁移注解提供程序
    /// </summary>
    public class OracleMigrationsAnnotationProvider : MigrationsAnnotationProvider
    {
        public override IEnumerable<IAnnotation> For(IKey key)
        {
            var isClustered = key.Oracle().IsClustered;
            if (isClustered.HasValue)
            {
                yield return new Annotation(
                    OracleFullAnnotationNames.Instance.Clustered,
                    isClustered.Value);
            }
        }

        public override IEnumerable<IAnnotation> For(IIndex index)
        {
            var isClustered = index.Oracle().IsClustered;
            if (isClustered.HasValue)
            {
                yield return new Annotation(
                    OracleFullAnnotationNames.Instance.Clustered,
                    isClustered.Value);
            }
        }

        public override IEnumerable<IAnnotation> For(IProperty property)
        {
            if (property.Oracle().ValueGenerationStrategy == OracleValueGenerationStrategy.IdentityColumn)
            {
                yield return new Annotation(
                    OracleFullAnnotationNames.Instance.ValueGenerationStrategy,
                    OracleValueGenerationStrategy.IdentityColumn);
            }
        }
    }
}
