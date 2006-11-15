using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Text;
using System.Collections.Specialized;

using Gtk;
using VersionControl.Service;

using MonoDevelop.Core.AddIns;
using MonoDevelop.Core;
using MonoDevelop.Core.Gui;
using MonoDevelop.Core.Gui.Dialogs;
using MonoDevelop.SourceEditor.Gui;
using MonoDevelop.Ide.Gui;

namespace VersionControl.AddIn.Views 
{
	public class StatusView : BaseView 
	{
		string filepath;
		Repository vc;
		
		Widget widget;
		Toolbar commandbar;
		VBox main;
		Label status;
		Gtk.ToolButton showRemoteStatus;
		Gtk.ToolButton buttonCommit;
		
		TreeView filelist;
		TreeViewColumn colCommit, colRemote;
		TreeStore filestore;
		ScrolledWindow scroller;
		
		Box commitBox;
		TextView commitText;
		Gtk.Label labelCommit;
		
		List<VersionInfo> statuses;
		
		bool remoteStatus = false;
		bool diffRequested = false;
		bool diffRunning = false;
		Exception diffException;
		DiffInfo[] difs;
		bool updatingComment;
		ChangeSet changeSet;
		bool firstLoad = true;
		
		const int ColIcon = 0;
		const int ColStatus = 1;
		const int ColPath = 2;
		const int ColRemoteStatus = 3;
		const int ColCommit = 4;
		const int ColFilled = 5;
		const int ColFullPath = 6;
		const int ColShowToggle = 7;
		const int ColShowComment = 8;
		const int ColIconFile = 9;
		
		public static bool Show (Repository vc, string path, bool test)
		{
			if (vc.IsVersioned(path)) {
				if (test) return true;
				StatusView d = new StatusView(path, vc);
				MonoDevelop.Ide.Gui.IdeApp.Workbench.OpenDocument (d, true);
				return true;
			}
			return false;
		}
		
		public StatusView (string filepath, Repository vc) 
			: base(Path.GetFileName(filepath) + " Status") 
		{
			this.vc = vc;
			this.filepath = filepath;
			changeSet = vc.CreateChangeSet (filepath);
			
			main = new VBox(false, 6);
			widget = main;
			
			commandbar = new Toolbar ();
			commandbar.ToolbarStyle = Gtk.ToolbarStyle.BothHoriz;
			commandbar.IconSize = Gtk.IconSize.Menu;
			main.PackStart(commandbar, false, false, 0);
			
			buttonCommit = new Gtk.ToolButton (new Gtk.Image ("vc-commit", Gtk.IconSize.Menu), "Commit...");
			buttonCommit.IsImportant = true;
			buttonCommit.Clicked += new EventHandler(OnCommitClicked);
			commandbar.Insert (buttonCommit, -1);
			
			Gtk.ToolButton btnRevert = new Gtk.ToolButton (new Gtk.Image ("vc-revert-command", Gtk.IconSize.Menu), GettextCatalog.GetString ("Revert"));
			btnRevert.IsImportant = true;
			btnRevert.Clicked += new EventHandler (OnRevert);
			commandbar.Insert (btnRevert, -1);
			
			showRemoteStatus = new Gtk.ToolButton (new Gtk.Image ("vc-remote-status", Gtk.IconSize.Menu), "Show Remote Status");
			showRemoteStatus.IsImportant = true;
			showRemoteStatus.Clicked += new EventHandler(OnShowRemoteStatusClicked);
			commandbar.Insert (showRemoteStatus, -1);
			
			Gtk.ToolButton btnRefresh = new Gtk.ToolButton (Gtk.Stock.Refresh);
			btnRefresh.IsImportant = true;
			btnRefresh.Clicked += new EventHandler (OnRefresh);
			commandbar.Insert (btnRefresh, -1);
			
			commandbar.Insert (new Gtk.SeparatorToolItem (), -1);
			
			Gtk.ToolButton btnExpand = new Gtk.ToolButton (null, "Expand All");
			btnExpand.IsImportant = true;
			btnExpand.Clicked += new EventHandler (OnExpandAll);
			commandbar.Insert (btnExpand, -1);
			
			Gtk.ToolButton btnCollapse = new Gtk.ToolButton (null, "Collapse All");
			btnCollapse.IsImportant = true;
			btnCollapse.Clicked += new EventHandler (OnCollapseAll);
			commandbar.Insert (btnCollapse, -1);
			
			status = new Label("");
			main.PackStart(status, false, false, 0);
			
			scroller = new ScrolledWindow();
			scroller.ShadowType = Gtk.ShadowType.In;
			filelist = new TreeView();
			filelist.Selection.Mode = SelectionMode.Multiple;
			scroller.Add(filelist);
			scroller.HscrollbarPolicy = PolicyType.Automatic;
			scroller.VscrollbarPolicy = PolicyType.Automatic;
			filelist.RowActivated += new RowActivatedHandler(OnRowActivated);
			
			CellRendererToggle cellToggle = new CellRendererToggle();
			cellToggle.Toggled += new ToggledHandler(OnCommitToggledHandler);
			CellRendererPixbuf crc = new CellRendererPixbuf();
			crc.StockId = "vc-comment";
			colCommit = new TreeViewColumn ();
			colCommit.Spacing = 2;
			colCommit.Widget = new Gtk.Image ("vc-commit", Gtk.IconSize.Menu);
			colCommit.Widget.Show ();
			colCommit.PackStart (cellToggle, false);
			colCommit.PackStart (crc, false);
			colCommit.AddAttribute (cellToggle, "active", ColCommit);
			colCommit.AddAttribute (cellToggle, "visible", ColShowToggle);
			colCommit.AddAttribute (crc, "visible", ColShowComment);
			
			CellRendererText crt = new CellRendererText();
			CellRendererPixbuf crp = new CellRendererPixbuf();
			TreeViewColumn colStatus = new TreeViewColumn ();
			colStatus.Title = GettextCatalog.GetString ("Status");
			colStatus.PackStart (crp, false);
			colStatus.PackStart (crt, true);
			colStatus.AddAttribute (crp, "pixbuf", ColIcon);
			colStatus.AddAttribute (crt, "text", ColStatus);
			
			TreeViewColumn colFile = new TreeViewColumn ();
			colFile.Title = GettextCatalog.GetString ("File");
			colFile.Spacing = 2;
			crp = new CellRendererPixbuf();
			CellRendererDiff cdif = new CellRendererDiff ();
			colFile.PackStart (crp, false);
			colFile.PackStart (cdif, true);
			colFile.AddAttribute (crp, "stock-id", ColIconFile);
			colFile.AddAttribute (crp, "visible", ColShowToggle);
			colFile.SetCellDataFunc (cdif, new TreeCellDataFunc (SetDiffCellData));
			
			colRemote = new TreeViewColumn("Remote Status", new CellRendererText(), "text", ColRemoteStatus);
			
			filelist.AppendColumn(colStatus);
			filelist.AppendColumn(colRemote);
			filelist.AppendColumn(colCommit);
			filelist.AppendColumn(colFile);
			
			colRemote.Visible = false;

			filestore = new TreeStore (typeof (Gdk.Pixbuf), typeof (string), typeof (string), typeof (string), typeof(bool), typeof(bool), typeof(string), typeof(bool), typeof (bool), typeof(string));
			filelist.Model = filestore;
			filelist.TestExpandRow += new Gtk.TestExpandRowHandler (OnTestExpandRow);
			
			commitBox = new VBox ();
			labelCommit = new Gtk.Label (GettextCatalog.GetString ("Commit message:"));
			labelCommit.Xalign = 0;
			commitBox.PackStart (labelCommit, false, false, 0);
			Gtk.ScrolledWindow frame = new Gtk.ScrolledWindow ();
			frame.ShadowType = ShadowType.In;
			commitText = new TextView ();
			commitText.WrapMode = WrapMode.WordChar;
			commitText.Buffer.Changed += OnCommitTextChanged;
			frame.Add (commitText);
			commitBox.PackStart (frame, true, true, 6);
			
			VPaned paned = new VPaned ();
			paned.Pack1 (scroller, true, true);
			paned.Pack2 (commitBox, false, false);
			main.PackStart (paned, true, true, 0);
			
			main.ShowAll();
			status.Visible = false;
			
			filelist.Selection.Changed += new EventHandler(OnCursorChanged);
			VersionControlProjectService.FileStatusChanged += OnFileStatusChanged;
			
			StartUpdate();
		}
		
		public override void Dispose ()
		{
			base.Dispose ();
		}
		
		public override Gtk.Widget Control { 
			get {
				return widget;
			}
		}
		
		private void StartUpdate ()
		{
			if (!remoteStatus)
				status.Text = "Scanning for changes...";
			else
				status.Text = "Scanning for local and remote changes...";
			
			status.Visible = true;
			scroller.Visible = false;
			commitBox.Visible = false;
			
			showRemoteStatus.Sensitive = false;
			buttonCommit.Sensitive = false;
			
			new Worker(vc, filepath, remoteStatus, this).Start();
		}
		
		void UpdateControlStatus ()
		{
			// Set controls to the correct state according to the changes found
			showRemoteStatus.Sensitive = !remoteStatus;
			
			if (statuses.Count == 0) {
				commitBox.Visible = false;
				buttonCommit.Sensitive = false;
				if (!remoteStatus)
					status.Text = "No files have local modifications.";
				else
					status.Text = "No files have local or remote modifications.";
				return;
			} else {
				status.Visible = false;
				scroller.Visible = true;
				commitBox.Visible = true;
				colRemote.Visible = remoteStatus;
				Console.WriteLine ("CC: " + vc.CanCommit(filepath) + " " + filepath);
				
				if (vc.CanCommit(filepath))
					buttonCommit.Sensitive = true;
			}
		}
		
		private void Update ()
		{
			UpdateControlStatus ();

			filestore.Clear();
			
			if (statuses.Count > 0) {
				foreach (VersionInfo n in statuses) {
					if (FileVisible (n)) {
						if (firstLoad)
							changeSet.AddFile (n);
						AppendFileInfo (n);
					}
				}
			}
			firstLoad = false;
		}
		
		TreeIter AppendFileInfo (VersionInfo n)
		{
			Gdk.Pixbuf statusicon = VersionControlProjectService.LoadIconForStatus(n.Status);
			string lstatus = VersionControlProjectService.GetStatusLabel (n.Status);
			
			string localpath = n.LocalPath.Substring(filepath.Length);
			if (localpath.Length > 0 && localpath[0] == Path.DirectorySeparatorChar) localpath = localpath.Substring(1);
			if (localpath == "") { localpath = "."; } // not sure if this happens
			
			string rstatus = n.RemoteStatus.ToString();
			bool hasComment = GetCommitMessage (n.LocalPath).Length > 0;
			bool commit = changeSet.ContainsFile (n.LocalPath);
			
			string fileIcon;
			if (n.IsDirectory)
				fileIcon = MonoDevelop.Core.Gui.Stock.ClosedFolder;
			else
				fileIcon = IdeApp.Services.Icons.GetImageForFile (n.LocalPath);

			TreeIter it = filestore.AppendValues (statusicon, lstatus, GLib.Markup.EscapeText (localpath), rstatus, commit, false, n.LocalPath, true, hasComment, fileIcon);
			if (!n.IsDirectory)
				filestore.AppendValues (it, null, "", "", "", false, true, n.LocalPath, false, hasComment, "");
			return it;
		}
		
		string[] GetCurrentFiles ()
		{
			TreePath[] paths = filelist.Selection.GetSelectedRows ();
			string[] files = new string [paths.Length];
			
			for (int n=0; n<paths.Length; n++) {
				TreeIter iter;
				filestore.GetIter (out iter, paths [n]);
				files [n] = (string) filestore.GetValue (iter, ColFullPath);
			}
			return files;
		}
		
		void OnCursorChanged (object o, EventArgs args)
		{
			string[] files = GetCurrentFiles ();
			if (files.Length > 0) {
				commitBox.Visible = true;
				updatingComment = true;
				if (files.Length == 1)
					labelCommit.Text = GettextCatalog.GetString ("Commit message:");
				else
					labelCommit.Text = GettextCatalog.GetString ("Commit message (multiple selection):");
				
				// If all selected files have the same message,
				// then show it so it can be modified. If not, show
				// a blank message
				string msg = GetCommitMessage (files[0]);
				foreach (string file in files) {
					if (msg != GetCommitMessage (file)) {
						commitText.Buffer.Text = "";
						updatingComment = false;
						return;
					}
				}
				commitText.Buffer.Text = msg;
				updatingComment = false;
			} else {
				updatingComment = true;
				commitText.Buffer.Text = "";
				updatingComment = false;
				commitBox.Visible = false;
			}
		}
		
		void OnCommitTextChanged (object o, EventArgs args)
		{
			if (updatingComment)
				return;
				
			string msg = commitText.Buffer.Text;
			
			string[] files = GetCurrentFiles ();
			foreach (string file in files)
				SetCommitMessage (file, msg);

			TreePath[] paths = filelist.Selection.GetSelectedRows ();
			foreach (TreePath path in paths) {
				TreeIter iter;
				filestore.GetIter (out iter, path);
				if (msg.Length > 0)
					filestore.SetValue (iter, ColShowComment, true);
				else
					filestore.SetValue (iter, ColShowComment, false);
			}
		}
		
		string GetCommitMessage (string file)
		{
			ChangeSetItem item = changeSet.GetFileItem (file);
			string txt = item != null ? item.Comment : null;
			return txt != null ? txt : "";
		}
		
		void SetCommitMessage (string file, string text)
		{
			ChangeSetItem item = changeSet.GetFileItem (file);
			if (item == null)
				item = changeSet.AddFile (file);
			item.Comment = text;
		}
		
		void OnRowActivated(object o, RowActivatedArgs args) {
			int index = args.Path.Indices[0];
			VersionInfo node = statuses[index];
			DiffView.Show (vc, node.LocalPath, false);
		}
		
		void OnCommitToggledHandler(object o, ToggledArgs args) {
			TreeIter pos;
			if (!filestore.GetIterFromString(out pos, args.Path))
				return;

			string localpath = (string) filestore.GetValue (pos, ColFullPath);
			
			if (changeSet.ContainsFile (localpath)) {
				changeSet.RemoveFile (localpath);
			} else {
				VersionInfo vi = GetVersionInfo (localpath);
				if (vi != null) {
					Console.WriteLine ("p1: " + vi.LocalPath);
					changeSet.AddFile (vi);
				}
			}
			filestore.SetValue (pos, ColCommit, changeSet.ContainsFile (localpath));
		}
		
		VersionInfo GetVersionInfo (string file)
		{
			foreach (VersionInfo vi in statuses)
				if (vi.LocalPath == file)
					return vi;
			return null;
		}
		
		private void OnShowRemoteStatusClicked(object src, EventArgs args) {
			remoteStatus = true;
			StartUpdate();
		}
		
		private void OnCommitClicked(object src, EventArgs args)
		{
			// Nothing to commit
			if (changeSet.IsEmpty)
				return;
				
			if (!CommitCommand.Commit (vc, changeSet, false))
				return;
		}

		
		private void OnTestExpandRow (object sender, Gtk.TestExpandRowArgs args)
		{
			bool filled = (bool) filestore.GetValue (args.Iter, ColFilled);
			if (!filled) {
				filestore.SetValue (args.Iter, ColFilled, true);
				TreeIter iter;
				filestore.IterChildren (out iter, args.Iter);
				SetFileDiff (iter, (string) filestore.GetValue (args.Iter, ColFullPath));
			}
		}
		
		void OnExpandAll (object s, EventArgs args)
		{
			filelist.ExpandAll ();
		}
		
		void OnCollapseAll (object s, EventArgs args)
		{
			filelist.CollapseAll ();
		}
		
		void OnRefresh (object s, EventArgs args)
		{
			StartUpdate ();
		}
		
		void OnRevert (object s, EventArgs args)
		{
			string[] files = GetCurrentFiles ();
			RevertCommand.Revert (vc, files, false);
		}
		
		void OnFileStatusChanged (object s, FileUpdateEventArgs args)
		{
			if (!args.FilePath.StartsWith (filepath))
				return;
				
			if (args.IsDirectory) {
				StartUpdate ();
				return;
			}

			TreeIter it;
			bool found = false;
			if (filestore.GetIterFirst (out it)) {
				do {
					if (args.FilePath == (string) filestore.GetValue (it, ColFullPath)) {
						found = true;
						break;
					}
				} while (filestore.IterNext (ref it));
			}
			
			VersionInfo newInfo = vc.GetVersionInfo (args.FilePath, remoteStatus);
			
			if (found) {
				int n;
				for (n=0; n<statuses.Count; n++) {
					if (statuses [n].LocalPath == args.FilePath)
						break;
				}
				
				if (newInfo == null ||
					newInfo.Status == VersionStatus.Unchanged ||
				    newInfo.Status == VersionStatus.Protected ||
				    newInfo.Status == VersionStatus.Unversioned
				    ) {
					// Just remove the file from the change set
					changeSet.RemoveFile (args.FilePath);
					statuses.RemoveAt (n);
					filestore.Remove (ref it);
					UpdateControlStatus ();
					return;
				}
				
				statuses [n] = newInfo;
				
				// Update the tree
				TreeIter newi = AppendFileInfo (newInfo);
				filestore.MoveAfter (newi, it);
				filestore.Remove (ref it);
			}
			else {
				if (FileVisible (newInfo)) {
					AppendFileInfo (newInfo);
					statuses.Add (newInfo);
					changeSet.AddFile (newInfo);
				}
			}
			UpdateControlStatus ();
		}
		
		bool FileVisible (VersionInfo vinfo)
		{
			return vinfo != null && 
					vinfo.Status != VersionStatus.Protected &&
					vinfo.Status != VersionStatus.Unversioned &&
					vinfo.Status != VersionStatus.UnversionedIgnored &&
					vinfo.Status != VersionStatus.Unchanged;
		}
		
		void SetFileDiff (TreeIter iter, string file)
		{
			// If diff information is already loaded, just look for the
			// diff chunk of the file and fill the tree
			if (diffRequested) {
				FillDiffInfo (iter, file);
				return;
			}
			
			filestore.SetValue (iter, ColPath, GLib.Markup.EscapeText (GettextCatalog.GetString ("Loading data...")));
			
			if (diffRunning)
				return;

			// Diff not yet requested. Do it now.
			diffRunning = true;
			
			// Run the diff in a separate thread and update the tree when done
			
			Thread t = new Thread (
				delegate () {
					diffException = null;
					try {
						difs = vc.PathDiff (filepath, null);
					} catch (Exception ex) {
						diffException = ex;
					} finally {
						Gtk.Application.Invoke (OnFillDifs);
					}
				}
			);
			t.IsBackground = true;
			t.Start ();
		}
		
		void FillDiffInfo (TreeIter iter, string file)
		{
			if (difs != null) {
				foreach (DiffInfo di in difs) {
					if (di.FileName == file) {
						filestore.SetValue (iter, ColPath, Colorize (di.Content));
						return;
					}
				}
			}
			filestore.SetValue (iter, ColPath, GLib.Markup.EscapeText (GettextCatalog.GetString ("No differences found")));
		}
		
		string Colorize (string txt)
		{
			txt = GLib.Markup.EscapeText (txt);
			StringReader sr = new StringReader (txt);
			StringBuilder sb = new StringBuilder ();
			string line;
			while ((line = sr.ReadLine ()) != null) {
				if (line.Length > 0) {
					char c = line [0];
					if (c == '-') {
						line = "<span foreground='red'>" + line + "</span>";
					} else if (c == '+')
						line = "<span foreground='blue'>" + line + "</span>";
				}
				sb.Append (line).Append ('\n');
			}
			return sb.ToString ();
		}
		
		void OnFillDifs (object s, EventArgs a)
		{
			diffRequested = true;
			diffRunning = false;
			
			if (diffException != null) {
				IdeApp.Services.MessageService.ShowError (diffException, GettextCatalog.GetString ("Could not get diff information. ") + diffException.Message);
			}
			
			TreeIter it;
			if (!filestore.GetIterFirst (out it))
				return;
				
			do {
				bool filled = (bool) filestore.GetValue (it, ColFilled);
				if (filled) {
					string fileName = (string) filestore.GetValue (it, ColFullPath);
					TreeIter citer;
					filestore.IterChildren (out citer, it);
					FillDiffInfo (citer, fileName);
				}
			}
			while (filestore.IterNext (ref it));
		}
		
		void SetDiffCellData (Gtk.TreeViewColumn tree_column, Gtk.CellRenderer cell, Gtk.TreeModel model, Gtk.TreeIter iter)
		{
			CellRendererDiff rc = (CellRendererDiff) cell;
			string text = (string) filestore.GetValue (iter, ColPath);
			if (filestore.IterDepth (iter) == 0) {
				rc.InitCell (filelist, false, text);
			} else {
				rc.InitCell (filelist, true, text);
			}
		}
		
		private class Worker : Task {
			StatusView view;
			Repository vc;
			string filepath;
			bool remoteStatus;
			List<VersionInfo> newList = new List<VersionInfo> ();

			public Worker(Repository vc, string filepath, bool remoteStatus, StatusView view) {
				this.vc = vc;
				this.filepath = filepath;
				this.view = view;
				this.remoteStatus = remoteStatus;
			}
			
			protected override string GetDescription() {
				return "Retrieving status for " + Path.GetFileName(filepath) + "...";
			}
			
			protected override void Run() {
				newList.AddRange (vc.GetDirectoryVersionInfo(filepath, remoteStatus, true));
			}
		
			protected override void Finished()
			{
				view.statuses = newList;
				view.Update();
			}
		}
	}

}
