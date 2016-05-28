// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Co.EntityFrameworkCore.Metadata.Internal;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Utilities;

namespace Co.EntityFrameworkCore.Metadata
{
    public class OracleModelAnnotations : RelationalModelAnnotations, IOracleModelAnnotations
    {
        public const string DefaultHiLoSequenceName = "EntityFrameworkHiLoSequence";

        public OracleModelAnnotations([NotNull] IModel model)
            : base(model, OracleFullAnnotationNames.Instance)
        {
        }

        protected OracleModelAnnotations([NotNull] RelationalAnnotations annotations)
            : base(annotations, OracleFullAnnotationNames.Instance)
        {
        }

        public virtual string HiLoSequenceName
        {
            get { return (string)Annotations.GetAnnotation(OracleFullAnnotationNames.Instance.HiLoSequenceName, null); }
            [param: CanBeNull] set { SetHiLoSequenceName(value); }
        }

        protected virtual bool SetHiLoSequenceName([CanBeNull] string value)
            => Annotations.SetAnnotation(
                OracleFullAnnotationNames.Instance.HiLoSequenceName,
                null,
                Check.NullButNotEmpty(value, nameof(value)));

        public virtual string HiLoSequenceSchema
        {
            get { return (string)Annotations.GetAnnotation(OracleFullAnnotationNames.Instance.HiLoSequenceSchema, null); }
            [param: CanBeNull] set { SetHiLoSequenceSchema(value); }
        }

        protected virtual bool SetHiLoSequenceSchema([CanBeNull] string value)
            => Annotations.SetAnnotation(
                OracleFullAnnotationNames.Instance.HiLoSequenceSchema,
                null,
                Check.NullButNotEmpty(value, nameof(value)));

        public virtual OracleValueGenerationStrategy? ValueGenerationStrategy
        {
            get
            {
                return (OracleValueGenerationStrategy?)Annotations.GetAnnotation(
                    OracleFullAnnotationNames.Instance.ValueGenerationStrategy,
                    null);
            }
            set { SetValueGenerationStrategy(value); }
        }

        protected virtual bool SetValueGenerationStrategy(OracleValueGenerationStrategy? value)
            => Annotations.SetAnnotation(OracleFullAnnotationNames.Instance.ValueGenerationStrategy,
                null,
                value);
    }
}
