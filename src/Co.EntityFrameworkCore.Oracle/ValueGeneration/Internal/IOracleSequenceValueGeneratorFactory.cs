// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Co.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Co.EntityFrameworkCore.ValueGeneration.Internal
{
    public interface IOracleSequenceValueGeneratorFactory
    {
        ValueGenerator Create(
            [NotNull] IProperty property,
            [NotNull] OracleSequenceValueGeneratorState generatorState,
            [NotNull] IOracleConnection connection);
    }
}
