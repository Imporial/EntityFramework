// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore.Query.ExpressionTranslators;
using System;

namespace Co.EntityFrameworkCore.Query.ExpressionTranslators.Internal
{
    public class OracleMathFloorTranslator : MultipleOverloadStaticMethodCallTranslator
    {
        public OracleMathFloorTranslator()
            : base(typeof(Math), nameof(Math.Floor), "FLOOR")
        {
        }
    }
}
