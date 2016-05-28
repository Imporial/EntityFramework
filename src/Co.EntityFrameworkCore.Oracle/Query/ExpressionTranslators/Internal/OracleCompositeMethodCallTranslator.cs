// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using Microsoft.Extensions.Logging;

namespace Co.EntityFrameworkCore.Query.ExpressionTranslators.Internal
{
    public class OracleCompositeMethodCallTranslator : RelationalCompositeMethodCallTranslator
    {
        private static readonly IMethodCallTranslator[] _methodCallTranslators =
        {
            new OracleMathAbsTranslator(),
            new OracleMathCeilingTranslator(),
            new OracleMathFloorTranslator(),
            new OracleMathPowerTranslator(),
            new OracleMathRoundTranslator(),
            new OracleMathTruncateTranslator(),
            new OracleNewGuidTranslator(),
            new OracleStringIsNullOrWhiteSpaceTranslator(),
            new OracleStringReplaceTranslator(),
            new OracleStringSubstringTranslator(),
            new OracleStringToLowerTranslator(),
            new OracleStringToUpperTranslator(),
            new OracleStringTrimEndTranslator(),
            new OracleStringTrimStartTranslator(),
            new OracleStringTrimTranslator(),
            new OracleConvertTranslator()
        };

        // ReSharper disable once SuggestBaseTypeForParameter
        public OracleCompositeMethodCallTranslator([NotNull] ILogger<OracleCompositeMethodCallTranslator> logger)
            : base(logger)
        {
            // ReSharper disable once DoNotCallOverridableMethodsInConstructor
            AddTranslators(_methodCallTranslators);
        }
    }
}
