// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Metadata;

namespace Co.EntityFrameworkCore.Metadata
{
    public interface IOracleModelAnnotations : IRelationalModelAnnotations
    {
        OracleValueGenerationStrategy? ValueGenerationStrategy { get; }
        string HiLoSequenceName { get; }
        string HiLoSequenceSchema { get; }
    }
}
