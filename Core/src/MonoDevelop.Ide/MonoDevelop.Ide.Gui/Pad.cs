//
// Pad.cs
//
// Author:
//   Lluis Sanchez Gual
//
// Copyright (C) 2005 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//



using System;
using System.Drawing;
using MonoDevelop.Core.Gui;

namespace MonoDevelop.Ide.Gui
{
	public class Pad
	{
		IPadWindow window;
		IWorkbench workbench;
		
		internal Pad (IWorkbench workbench, IPadContent content)
		{
			this.window = workbench.WorkbenchLayout.GetPadWindow (content);
			this.workbench = workbench;
		}
		
		public object Content {
			get { return window.Content; }
		}
		
		public string Title {
			get { return window.Title; }
		}
		
		public string Id {
			get { return window.Content.Id; }
		}
		
		public void BringToFront ()
		{
			workbench.BringToFront (window.Content);
		}
		
		public bool Visible {
			get {
				return workbench.WorkbenchLayout.IsVisible (window.Content);
			}
			set {
				if (value)
					workbench.WorkbenchLayout.ShowPad (window.Content);
				else
					workbench.WorkbenchLayout.HidePad (window.Content);
			}
		}
	}
}
