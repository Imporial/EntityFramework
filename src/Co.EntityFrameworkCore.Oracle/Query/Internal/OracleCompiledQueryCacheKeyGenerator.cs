// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq.Expressions;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Co.EntityFrameworkCore.Infrastructure.Internal;

namespace Co.EntityFrameworkCore.Query.Internal
{
    /// <summary>
    /// Oracle ±‡“Î≤È—Øª∫¥Êº¸…˙≥…∆˜
    /// </summary>
    public class OracleCompiledQueryCacheKeyGenerator : RelationalCompiledQueryCacheKeyGenerator
    {
        private readonly IDbContextOptions _contextOptions;
        /// <summary>
        /// ≥ı ºªØ Oracle ±‡“Î≤È—Øª∫¥Êº¸…˙≥…∆˜
        /// </summary>
        /// <param name="model"></param>
        /// <param name="currentContext"></param>
        /// <param name="contextOptions"></param>
        public OracleCompiledQueryCacheKeyGenerator(
            [NotNull] IModel model,
            [NotNull] ICurrentDbContext currentContext,
            [NotNull] IDbContextOptions contextOptions)
            : base(model, currentContext, contextOptions)
        {
            Check.NotNull(contextOptions, nameof(contextOptions));

            _contextOptions = contextOptions;
        }

        public override object GenerateCacheKey(Expression query, bool async)
            => new OracleCompiledQueryCacheKey(
                GenerateCacheKeyCore(query, async),
                _contextOptions.FindExtension<OracleOptionsExtension>()?.RowNumberPaging ?? false);
        /// <summary>
        /// Oracle ±‡“Î≤È—Øª∫¥Êº¸
        /// </summary>
        private struct OracleCompiledQueryCacheKey
        {
            private readonly RelationalCompiledQueryCacheKey _relationalCompiledQueryCacheKey;
            private readonly bool _useRowNumberOffset;
            /// <summary>
            /// ≥ı ºªØ Oracle ±‡“Î≤È—Øª∫¥Êº¸
            /// </summary>
            /// <param name="relationalCompiledQueryCacheKey"></param>
            /// <param name="useRowNumberOffset"></param>
            public OracleCompiledQueryCacheKey(
                RelationalCompiledQueryCacheKey relationalCompiledQueryCacheKey, bool useRowNumberOffset)
            {
                _relationalCompiledQueryCacheKey = relationalCompiledQueryCacheKey;
                _useRowNumberOffset = useRowNumberOffset;
            }

            public override bool Equals(object obj)
                => !ReferenceEquals(null, obj) && obj is OracleCompiledQueryCacheKey && Equals((OracleCompiledQueryCacheKey)obj);

            private bool Equals(OracleCompiledQueryCacheKey other)
                => _relationalCompiledQueryCacheKey.Equals(other._relationalCompiledQueryCacheKey)
                   && (_useRowNumberOffset == other._useRowNumberOffset);

            public override int GetHashCode()
            {
                unchecked
                {
                    return (_relationalCompiledQueryCacheKey.GetHashCode() * 397) ^ _useRowNumberOffset.GetHashCode();
                }
            }
        }
    }
}
