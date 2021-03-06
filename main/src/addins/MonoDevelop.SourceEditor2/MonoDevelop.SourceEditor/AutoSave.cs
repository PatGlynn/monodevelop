//
// AutoSave.cs
//
// Author:
//       Mike Krüger <mkrueger@novell.com>
//
// Copyright (c) 2009 Novell, Inc (http://www.novell.com)
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
using System.IO;
using System.Threading;
using MonoDevelop.Core;

namespace MonoDevelop.SourceEditor
{
	static class AutoSave
	{
		//FIXME: is this path a good one? wouldn't it be better to put autosaves beside the files anyway?
		static string autoSavePath = UserProfile.Current.CacheDir.Combine ("AutoSave");
		static bool autoSaveEnabled;
		
		static AutoSave ()
		{
			try {
				if (!Directory.Exists (autoSavePath))
					Directory.CreateDirectory (autoSavePath);
			} catch (Exception e) {
				LoggingService.LogError ("Can't create auto save path:" + autoSavePath +". Auto save is disabled.", e);
				autoSaveEnabled = false;
				return;
			}
			autoSaveEnabled = true;
			StartAutoSaveThread ();
		}

		static string GetAutoSaveFileName (string fileName)
		{
			if (fileName == null)
				return null;
			string newFileName = Path.Combine (Path.GetDirectoryName (fileName), Path.GetFileNameWithoutExtension (fileName) + Path.GetExtension (fileName) + "~");
			newFileName = Path.Combine (autoSavePath, newFileName.Replace(',','_').Replace(" ","").Replace (":","").Replace (Path.DirectorySeparatorChar, '_').Replace (Path.AltDirectorySeparatorChar, '_'));
			return newFileName;
		}

		public static bool AutoSaveExists (string fileName)
		{
			if (!autoSaveEnabled)
				return false;
			try {
				return File.Exists (GetAutoSaveFileName (fileName));
			} catch (Exception e) {
				LoggingService.LogError ("Error in auto save - disabling.", e);
				DisableAutoSave ();
				return false;
			}
		}

		static void CreateAutoSave (string fileName, string content)
		{
			if (!autoSaveEnabled)
				return;
			try {
				// Directory may have removed/unmounted. Therefore this operation is not guaranteed to work.
				File.WriteAllText (GetAutoSaveFileName (fileName), content);
				Counters.AutoSavedFiles++;
			} catch (Exception e) {
				LoggingService.LogError ("Error in auto save while creating: " + fileName +". Disabling auto save.", e);
				DisableAutoSave ();
			}
		}

#region AutoSave
		class FileContent
		{
			public string FileName;
			public Mono.TextEditor.Document Content;

			public FileContent (string fileName, Mono.TextEditor.Document content)
			{
				this.FileName = fileName;
				this.Content = content;
			}
		}

		public static bool Running {
			get {
				return autoSaveThreadRunning;
			}
		}

		static AutoResetEvent resetEvent = new AutoResetEvent (false);
		static bool autoSaveThreadRunning = false;
		static Thread autoSaveThread;
		static Queue<FileContent> queue = new Queue<FileContent> ();
		static object contentLock = new object ();

		static void StartAutoSaveThread ()
		{
			autoSaveThreadRunning = true;
			if (autoSaveThread == null) {
				autoSaveThread = new Thread (AutoSaveThread);
				autoSaveThread.Name = "Autosave";
				autoSaveThread.IsBackground = true;
				autoSaveThread.Start ();
			}
		}

		static void AutoSaveThread ()
		{
			while (autoSaveThreadRunning) {
				resetEvent.WaitOne ();
				while (queue.Count > 0) {
					var content = queue.Dequeue ();
					lock (contentLock) {
						string text;
						try {
							text = content.Content.Text;
						} catch (Exception e) {
							LoggingService.LogError ("Exception in auto save thread.", e);
							continue;
						}
						CreateAutoSave (content.FileName, text);
					}
				}
			}
		}

		public static string LoadAutoSave (string fileName)
		{
			string autoSaveFileName = GetAutoSaveFileName (fileName);
			lock (contentLock)
			{
				try {
					return File.ReadAllText (autoSaveFileName);
				} catch (Exception e) {
					LoggingService.LogError ("Error loading autosave from " + autoSaveFileName, e);
				}
			}

			return string.Empty;
		}

		public static void RemoveAutoSaveFile (string fileName)
		{
			if (!autoSaveEnabled)
				return;
			
			lock (contentLock) {
				if (AutoSaveExists (fileName)) {
					string autoSaveFileName = GetAutoSaveFileName (fileName);
					try {
							File.Delete (autoSaveFileName);
					} catch (Exception e) {
						LoggingService.LogError ("Can't delete auto save file: " + autoSaveFileName +". Disabling auto save.", e);
						DisableAutoSave ();
					}
				}
			}
		}

		public static void InformAutoSaveThread (Mono.TextEditor.Document content)
		{
			if (content == null || !autoSaveEnabled)
				return;
			if (content.IsDirty) {
				queue.Enqueue (new FileContent (content.FileName, content));
				resetEvent.Set ();
			} else {
				RemoveAutoSaveFile (content.FileName);
			}
		}

		public static void DisableAutoSave ()
		{
			autoSaveThreadRunning = false;
			autoSaveEnabled = false;
			if (autoSaveThread != null) {
				resetEvent.Set ();
				autoSaveThread.Join ();
				autoSaveThread = null;
			}
		}
#endregion
	}
}
