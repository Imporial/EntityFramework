// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Metadata;

namespace Co.EntityFrameworkCore.Metadata
{
    public class OracleAnnotationProvider : IRelationalAnnotationProvider
    {
        public virtual IRelationalEntityTypeAnnotations For(IEntityType entityType) => entityType.Oracle();
        public virtual IRelationalForeignKeyAnnotations For(IForeignKey foreignKey) => foreignKey.Oracle();
        public virtual IRelationalIndexAnnotations For(IIndex index) => index.Oracle();
        public virtual IRelationalKeyAnnotations For(IKey key) => key.Oracle();
        public virtual IRelationalPropertyAnnotations For(IProperty property) => property.Oracle();
        public virtual IRelationalModelAnnotations For(IModel model) => model.Oracle();
    }
}
