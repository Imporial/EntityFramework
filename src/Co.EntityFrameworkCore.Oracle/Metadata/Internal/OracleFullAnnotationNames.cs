// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Co.EntityFrameworkCore.Metadata.Internal
{
    public class OracleFullAnnotationNames : RelationalFullAnnotationNames
    {
        protected OracleFullAnnotationNames(string prefix)
            : base(prefix)
        {
            Clustered = prefix + OracleAnnotationNames.Clustered;
            ValueGenerationStrategy = prefix + OracleAnnotationNames.ValueGenerationStrategy;
            HiLoSequenceName = prefix + OracleAnnotationNames.HiLoSequenceName;
            HiLoSequenceSchema = prefix + OracleAnnotationNames.HiLoSequenceSchema;
        }

        public new static OracleFullAnnotationNames Instance { get; } = new OracleFullAnnotationNames(OracleAnnotationNames.Prefix);

        public readonly string Clustered;
        public readonly string ValueGenerationStrategy;
        public readonly string HiLoSequenceName;
        public readonly string HiLoSequenceSchema;
    }
}
