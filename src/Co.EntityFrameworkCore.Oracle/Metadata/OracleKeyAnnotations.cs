// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Co.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Co.EntityFrameworkCore.Metadata
{
    public class OracleKeyAnnotations : RelationalKeyAnnotations, IOracleKeyAnnotations
    {
        public OracleKeyAnnotations([NotNull] IKey key)
            : base(key, OracleFullAnnotationNames.Instance)
        {
        }

        protected OracleKeyAnnotations([NotNull] RelationalAnnotations annotations)
            : base(annotations, OracleFullAnnotationNames.Instance)
        {
        }

        public virtual bool? IsClustered
        {
            get { return (bool?)Annotations.GetAnnotation(OracleFullAnnotationNames.Instance.Clustered, null); }
            [param: CanBeNull] set { SetIsClustered(value); }
        }

        protected virtual bool SetIsClustered(bool? value)
            => Annotations.SetAnnotation(OracleFullAnnotationNames.Instance.Clustered, null, value);
    }
}
