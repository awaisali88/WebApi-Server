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

//[assembly: Elmah.Scc("$Id: DelegatedContextExpression.cs 623 2009-05-30 00:46:46Z azizatif $")]

using System;

namespace ElmahCore.Assertions
{
	internal sealed class DelegatedContextExpression : IContextExpression
    {
        private readonly Func<object, object> _handler;

        public DelegatedContextExpression(Func<object, object> handler)
        {
            if (handler == null) 
                throw new ArgumentNullException("handler");
            
            _handler = handler;
        }

        public Func<object, object> Handler
        {
            get { return _handler; }
        }

        public object Evaluate(object context)
        {
            return _handler(context);
        }

        public override string ToString()
        {
            return Handler.ToString();
        }
    }
}