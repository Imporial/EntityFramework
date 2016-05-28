// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Co.EntityFrameworkCore.Infrastructure.Internal;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Co.EntityFrameworkCore.Infrastructure
{
    public class OracleDbContextOptionsBuilder
        : RelationalDbContextOptionsBuilder<OracleDbContextOptionsBuilder, OracleOptionsExtension>
    {
        public OracleDbContextOptionsBuilder([NotNull] DbContextOptionsBuilder optionsBuilder)
            : base(optionsBuilder)
        {
        }

        protected override OracleOptionsExtension CloneExtension()
            => new OracleOptionsExtension(OptionsBuilder.Options.GetExtension<OracleOptionsExtension>());

        /// <summary>
        ///     Use a ROW_NUMBER() in queries instead of OFFSET/FETCH. This method is backwards-compatible to SQL Server 2005.
        /// </summary>
        public virtual void UseRowNumberForPaging() => SetOption(e => e.RowNumberPaging = true);
    }
}
