// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using System.Collections.Generic;

namespace Co.EntityFrameworkCore.Query.ExpressionTranslators.Internal
{
    public class OracleCompositeMemberTranslator : RelationalCompositeMemberTranslator
    {
        public OracleCompositeMemberTranslator()
        {
            var sqlServerTranslators = new List<IMemberTranslator>
            {
                new OracleStringLengthTranslator(),
                new OracleDateTimeNowTranslator(),
                new OracleDateTimeDateComponentTranslator(),
                new OracleDateTimeDatePartComponentTranslator(),
            };

            AddTranslators(sqlServerTranslators);
        }
    }
}