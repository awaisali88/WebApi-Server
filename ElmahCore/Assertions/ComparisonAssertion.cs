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

//[assembly: Elmah.Scc("$Id: ComparisonAssertion.cs 622 2009-05-30 00:35:37Z azizatif $")]

using System;
using System.Globalization;

namespace ElmahCore.Assertions
{
    #region Imports

	#endregion

    /// <summary>
    /// An assertion implementation whose test is based on whether
    /// the result of an input expression evaluated against a context
    /// matches a regular expression pattern or not.
    /// </summary>

    internal class ComparisonAssertion : DataBoundAssertion
    {
        private readonly object _expectedValue;
        private readonly Predicate<int> _predicate;

        public ComparisonAssertion(Predicate<int> predicate, IContextExpression source, TypeCode type, string value) :
            base(source)
        {
            if (predicate == null) 
                throw new ArgumentNullException("predicate");

            _predicate = predicate;

            if (type == TypeCode.DBNull 
                || type == TypeCode.Empty 
                || type == TypeCode.Object)
            {
                var message = string.Format(
                    "The {0} value type is invalid for a comparison.", type.ToString());
                throw new ArgumentException(message, "type");
            }

            //
            // Convert the expected value to the comparison type and 
            // save it as a field.
            //

            _expectedValue = Convert.ChangeType(value, type/*, FIXME CultureInfo.InvariantCulture */);
        }

        public IContextExpression Source
        {
            get { return Expression; }
        }

        public object ExpectedValue
        {
            get { return _expectedValue; }
        }

        public override bool Test(object context)
        {
            if (context == null) throw new ArgumentNullException("context");
            return ExpectedValue != null && base.Test(context);
        }

        protected override bool TestResult(object result)
        {
            if (result == null)
                return false;

            var right = ExpectedValue as IComparable;
            
            if (right == null)
                return false;

            var left = Convert.ChangeType(result, right.GetType(), CultureInfo.InvariantCulture) as IComparable;
            
            return left != null && TestComparison(left, right);
        }

        protected bool TestComparison(IComparable left, IComparable right)
        {
            if (left == null) throw new ArgumentNullException("left");            
            return _predicate(left.CompareTo(right));
        }
    }
}