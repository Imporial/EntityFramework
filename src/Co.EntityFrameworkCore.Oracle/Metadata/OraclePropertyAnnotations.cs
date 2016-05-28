// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Co.EntityFrameworkCore.Internal;
using Co.EntityFrameworkCore.Metadata.Internal;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Utilities;
using System;

namespace Co.EntityFrameworkCore.Metadata
{
    public class OraclePropertyAnnotations : RelationalPropertyAnnotations, IOraclePropertyAnnotations
    {
        public OraclePropertyAnnotations([NotNull] IProperty property)
            : base(property, OracleFullAnnotationNames.Instance)
        {
        }

        protected OraclePropertyAnnotations([NotNull] RelationalAnnotations annotations)
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

        public virtual ISequence FindHiLoSequence()
        {
            var modelExtensions = Property.DeclaringEntityType.Model.Oracle();

            if (ValueGenerationStrategy != OracleValueGenerationStrategy.SequenceHiLo)
            {
                return null;
            }

            var sequenceName = HiLoSequenceName
                               ?? modelExtensions.HiLoSequenceName
                               ?? OracleModelAnnotations.DefaultHiLoSequenceName;

            var sequenceSchema = HiLoSequenceSchema
                                 ?? modelExtensions.HiLoSequenceSchema;

            return modelExtensions.FindSequence(sequenceName, sequenceSchema);
        }

        public virtual OracleValueGenerationStrategy? ValueGenerationStrategy
        {
            get { return GetOracleValueGenerationStrategy(fallbackToModel: true); }
            [param: CanBeNull] set { SetValueGenerationStrategy(value); }
        }

        private OracleValueGenerationStrategy? GetOracleValueGenerationStrategy(bool fallbackToModel)
        {
            if (GetDefaultValue(false) != null
                || GetDefaultValueSql(false) != null
                || GetComputedColumnSql(false) != null)
            {
                return null;
            }

            var value = (OracleValueGenerationStrategy?)Annotations.GetAnnotation(
                OracleFullAnnotationNames.Instance.ValueGenerationStrategy,
                null);

            var relationalProperty = Property.Relational();
            return value ??
                   (fallbackToModel
                    && Property.ValueGenerated == ValueGenerated.OnAdd
                    && Property.ClrType.UnwrapNullableType().IsInteger()
                    && relationalProperty.DefaultValue == null
                    && relationalProperty.DefaultValueSql == null
                    && relationalProperty.ComputedColumnSql == null
                       ? Property.DeclaringEntityType.Model.Oracle().ValueGenerationStrategy
                       : null);
        }

        protected virtual bool SetValueGenerationStrategy(OracleValueGenerationStrategy? value)
        {
            if (value != null)
            {
                var propertyType = Property.ClrType;

                if (value == OracleValueGenerationStrategy.IdentityColumn
                    && (!propertyType.IsInteger()
                        || propertyType == typeof(byte)
                        || propertyType == typeof(byte?)))
                {
                    throw new ArgumentException(OracleStrings.IdentityBadType(
                        Property.Name, Property.DeclaringEntityType.Name, propertyType.Name));
                }

                if ((value == OracleValueGenerationStrategy.SequenceHiLo)
                    && !propertyType.IsInteger())
                {
                    throw new ArgumentException(OracleStrings.SequenceBadType(
                        Property.Name, Property.DeclaringEntityType.Name, propertyType.Name));
                }
            }

            if (!CanSetValueGenerationStrategy(value))
            {
                return false;
            }

            if (!ShouldThrowOnConflict
                && ValueGenerationStrategy != value
                && value != null)
            {
                ClearAllServerGeneratedValues();
            }

            return Annotations.SetAnnotation(OracleFullAnnotationNames.Instance.ValueGenerationStrategy, null, value);
        }

        protected virtual bool CanSetValueGenerationStrategy(OracleValueGenerationStrategy? value)
        {
            if (GetOracleValueGenerationStrategy(fallbackToModel: false) == value)
            {
                return true;
            }

            if (!Annotations.CanSetAnnotation(OracleFullAnnotationNames.Instance.ValueGenerationStrategy, null, value))
            {
                return false;
            }

            if (ShouldThrowOnConflict)
            {
                if (GetDefaultValue(false) != null)
                {
                    throw new InvalidOperationException(
                        RelationalStrings.ConflictingColumnServerGeneration(nameof(ValueGenerationStrategy), Property.Name, nameof(DefaultValue)));
                }
                if (GetDefaultValueSql(false) != null)
                {
                    throw new InvalidOperationException(
                        RelationalStrings.ConflictingColumnServerGeneration(nameof(ValueGenerationStrategy), Property.Name, nameof(DefaultValueSql)));
                }
                if (GetComputedColumnSql(false) != null)
                {
                    throw new InvalidOperationException(
                        RelationalStrings.ConflictingColumnServerGeneration(nameof(ValueGenerationStrategy), Property.Name, nameof(ComputedColumnSql)));
                }
            }
            else if (value != null
                     && (!CanSetDefaultValue(null)
                         || !CanSetDefaultValueSql(null)
                         || !CanSetComputedColumnSql(null)))
            {
                return false;
            }

            return true;
        }

        protected override object GetDefaultValue(bool fallback)
        {
            if (fallback
                && ValueGenerationStrategy != null)
            {
                return null;
            }

            return base.GetDefaultValue(fallback);
        }

        protected override bool CanSetDefaultValue(object value)
        {
            if (ShouldThrowOnConflict)
            {
                if (ValueGenerationStrategy != null)
                {
                    throw new InvalidOperationException(
                        RelationalStrings.ConflictingColumnServerGeneration(nameof(DefaultValue), Property.Name, nameof(ValueGenerationStrategy)));
                }
            }
            else if (value != null
                     && !CanSetValueGenerationStrategy(null))
            {
                return false;
            }

            return base.CanSetDefaultValue(value);
        }

        protected override string GetDefaultValueSql(bool fallback)
        {
            if (fallback
                && ValueGenerationStrategy != null)
            {
                return null;
            }

            return base.GetDefaultValueSql(fallback);
        }

        protected override bool CanSetDefaultValueSql(string value)
        {
            if (ShouldThrowOnConflict)
            {
                if (ValueGenerationStrategy != null)
                {
                    throw new InvalidOperationException(
                        RelationalStrings.ConflictingColumnServerGeneration(nameof(DefaultValueSql), Property.Name, nameof(ValueGenerationStrategy)));
                }
            }
            else if (value != null
                     && !CanSetValueGenerationStrategy(null))
            {
                return false;
            }

            return base.CanSetDefaultValueSql(value);
        }

        protected override string GetComputedColumnSql(bool fallback)
        {
            if (fallback
                && ValueGenerationStrategy != null)
            {
                return null;
            }

            return base.GetComputedColumnSql(fallback);
        }

        protected override bool CanSetComputedColumnSql(string value)
        {
            if (ShouldThrowOnConflict)
            {
                if (ValueGenerationStrategy != null)
                {
                    throw new InvalidOperationException(
                        RelationalStrings.ConflictingColumnServerGeneration(nameof(ComputedColumnSql), Property.Name, nameof(ValueGenerationStrategy)));
                }
            }
            else if (value != null
                     && !CanSetValueGenerationStrategy(null))
            {
                return false;
            }

            return base.CanSetComputedColumnSql(value);
        }

        protected override void ClearAllServerGeneratedValues()
        {
            SetValueGenerationStrategy(null);

            base.ClearAllServerGeneratedValues();
        }
    }
}
