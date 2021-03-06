// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace Co.EntityFrameworkCore.Migrations.Operations
{
    /// <summary>
    /// Oracle 创建数据库操作
    /// </summary>
    public class OracleCreateDatabaseOperation : MigrationOperation
    {
        public virtual string Name { get; [param: NotNull] set; }
    }
}
