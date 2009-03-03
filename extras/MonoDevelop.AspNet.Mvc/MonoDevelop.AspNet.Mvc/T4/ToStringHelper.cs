// 
// ToStringHelper.cs
//  
// Author:
//       Michael Hutchinson <mhutchinson@novell.com>
// 
// Copyright (c) 2009 Novell, Inc. (http://www.novell.com)
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;
using System.Collections.Generic;
using System.Reflection;

namespace MonoDevelop.AspNet.Mvc.T4
{
	
	
	public static class ToStringHelper
	{
		static Dictionary<Type, Func<object, IFormatProvider, string>> cache
			= new Dictionary<Type, Func<object, IFormatProvider, string>> ();
		static IFormatProvider formatProvider = System.Globalization.CultureInfo.InvariantCulture;
		
		public static string ToStringWithCulture (object objectToConvert)
		{
			Type type = objectToConvert.GetType ();
			Func<object, IFormatProvider, string> action = null;
			if (!cache.TryGetValue (type, out action)) {
				MethodInfo mi = type.GetMethod ("ToString", new Type[] { typeof (IFormatProvider) });
				if (mi != null)
					action = (Func<object, IFormatProvider, string>)
						Delegate.CreateDelegate (typeof(Func<object, IFormatProvider, string>), mi);
				else
					action = InvokeToString;
				cache.Add (type, action);
			}
			return action (objectToConvert, FormatProvider);
		}
		
		public static IFormatProvider FormatProvider {
			get { return formatProvider; }
			set { formatProvider = value; }
		}
		
		static string InvokeToString (object obj, IFormatProvider dummy)
		{
			return obj.ToString ();
		}
	}
}
