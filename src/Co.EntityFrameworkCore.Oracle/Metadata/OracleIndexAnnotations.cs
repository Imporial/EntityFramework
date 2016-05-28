// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Co.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Co.EntityFrameworkCore.Metadata
{
    public class OracleIndexAnnotations : RelationalIndexAnnotations, IOracleIndexAnnotations
    {
        public OracleIndexAnnotations([NotNull] IIndex index)
            : base(index, OracleFullAnnotationNames.Instance)
        {
        }

        protected OracleIndexAnnotations([NotNull] RelationalAnnotations annotations)
            : base(annotations, OracleFullAnnotationNames.Instance)
        {
        }

        public virtual bool? IsClustered
        {
            get { return (bool?)Annotations.GetAnnotation(OracleFullAnnotationNames.Instance.Clustered, null); }
            [param: CanBeNull] set { SetIsClustered(value); }
        }

        protected virtual bool SetIsClustered(bool? value) => Annotations.SetAnnotation(
            OracleFullAnnotationNames.Instance.Clustered,
            null,
            value);
    }
}
