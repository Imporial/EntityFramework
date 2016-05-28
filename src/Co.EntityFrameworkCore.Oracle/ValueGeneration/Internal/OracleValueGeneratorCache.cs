// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using System.Collections.Concurrent;
using System.Diagnostics;

namespace Co.EntityFrameworkCore.ValueGeneration.Internal
{
    public class OracleValueGeneratorCache : ValueGeneratorCache, IOracleValueGeneratorCache
    {
        private readonly ConcurrentDictionary<string, OracleSequenceValueGeneratorState> _sequenceGeneratorCache
            = new ConcurrentDictionary<string, OracleSequenceValueGeneratorState>();

        public virtual OracleSequenceValueGeneratorState GetOrAddSequenceState(IProperty property)
        {
            Check.NotNull(property, nameof(property));

            var sequence = property.Oracle().FindHiLoSequence();

            Debug.Assert(sequence != null);

            return _sequenceGeneratorCache.GetOrAdd(
                GetSequenceName(sequence),
                sequenceName => new OracleSequenceValueGeneratorState(sequence));
        }

        private static string GetSequenceName(ISequence sequence)
            => (sequence.Schema == null ? "" : sequence.Schema + ".") + sequence.Name;
    }
}
