// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Co.Extensions.DependencyInjection;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Utilities;
using Microsoft.Extensions.DependencyInjection;

namespace Co.EntityFrameworkCore.Infrastructure.Internal
{
    public class OracleOptionsExtension : RelationalOptionsExtension
    {
        private bool? _rowNumberPaging;

        public OracleOptionsExtension()
        {
        }

        // NB: When adding new options, make sure to update the copy ctor below.

        public OracleOptionsExtension([NotNull] OracleOptionsExtension copyFrom)
            : base(copyFrom)
        {
            _rowNumberPaging = copyFrom._rowNumberPaging;
        }

        public virtual bool? RowNumberPaging
        {
            get { return _rowNumberPaging; }
            set { _rowNumberPaging = value; }
        }

        public override void ApplyServices(IServiceCollection services)
            => Check.NotNull(services, nameof(services)).AddEntityFrameworkOracle();
    }
}
