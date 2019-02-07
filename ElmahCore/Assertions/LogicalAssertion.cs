#region License, Terms and Author(s)
//
// ELMAH - Error Logging Modules and Handlers for ASP.NET
// Copyright (c) 2004-9 Atif Aziz. All rights reserved.
//
//  Author(s):
//
//      Atif Aziz, http://www.raboof.com
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

//[assembly: Elmah.Scc("$Id: LogicalAssertion.cs 623 2009-05-30 00:46:46Z azizatif $")]

using System;
using System.Collections.Generic;

namespace ElmahCore.Assertions
{
    #region Imports

	#endregion

    internal sealed class LogicalAssertion : CompositeAssertion
    {
        private readonly bool _not;
        private readonly bool _all;

        public static LogicalAssertion LogicalAnd(IAssertion[] operands)
        {
            return new LogicalAssertion(operands, false, true);
        }

        public static LogicalAssertion LogicalOr(IAssertion[] operands)
        {
            return new LogicalAssertion(operands, false, false);
        }

        public static LogicalAssertion LogicalNot(IAssertion[] operands)
        {
            return new LogicalAssertion(operands, true, true);
        }

        private LogicalAssertion(IEnumerable<IAssertion> assertions, bool not, bool all) : 
            base(assertions)
        {
            _not = not;
            _all = all;
        }

        public override bool Test(object context)
        {
            if (context == null)
                throw new ArgumentNullException("context");

            if (Count == 0)
                return false;

            //
            // Walk through all child assertions and determine the
            // outcome, OR-ing or AND-ing each as needed.
            //

            var result = false;

            foreach (var assertion in this)
            {
                if (assertion == null)
                    continue;

                var testResult = assertion.Test(context);
                
                if (_not) 
                    testResult = !testResult;
                
                if (testResult)
                {
                    if (!_all) return true;
                    result = true;
                }
                else
                {
                    if (_all) return false;
                }
            }

            return result;
        }
    }
}
