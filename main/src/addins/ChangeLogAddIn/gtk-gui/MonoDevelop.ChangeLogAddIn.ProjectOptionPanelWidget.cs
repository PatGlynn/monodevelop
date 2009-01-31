// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace MonoDevelop.ChangeLogAddIn {
    
    
    internal partial class ProjectOptionPanelWidget {
        
        private Gtk.Notebook notebook1;
        
        private Gtk.VBox vbox2;
        
        private Gtk.RadioButton noneRadioButton;
        
        private Gtk.Label label3;
        
        private Gtk.RadioButton nearestRadioButton;
        
        private Gtk.Label label5;
        
        private Gtk.RadioButton oneChangeLogInProjectRootDirectoryRadioButton;
        
        private Gtk.Label label6;
        
        private Gtk.RadioButton oneChangeLogInEachDirectoryRadioButton;
        
        private Gtk.Label label7;
        
        private Gtk.Alignment alignment1;
        
        private Gtk.Alignment alignment2;
        
        private Gtk.CheckButton checkVersionControl;
        
        private Gtk.Alignment alignment3;
        
        private Gtk.CheckButton checkRequireOnCommit;
        
        private Gtk.Label label1;
        
        private Gtk.VBox vbox1;
        
        private MonoDevelop.VersionControl.CommitMessageStylePanelWidget messageWidget;
        
        private Gtk.Label label2;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget MonoDevelop.ChangeLogAddIn.ProjectOptionPanelWidget
            Stetic.BinContainer.Attach(this);
            this.Name = "MonoDevelop.ChangeLogAddIn.ProjectOptionPanelWidget";
            // Container child MonoDevelop.ChangeLogAddIn.ProjectOptionPanelWidget.Gtk.Container+ContainerChild
            this.notebook1 = new Gtk.Notebook();
            this.notebook1.CanFocus = true;
            this.notebook1.Name = "notebook1";
            this.notebook1.CurrentPage = 1;
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            this.vbox2.BorderWidth = ((uint)(6));
            // Container child vbox2.Gtk.Box+BoxChild
            this.noneRadioButton = new Gtk.RadioButton(Mono.Unix.Catalog.GetString("radiobutton1"));
            this.noneRadioButton.CanFocus = true;
            this.noneRadioButton.Name = "noneRadioButton";
            this.noneRadioButton.DrawIndicator = true;
            this.noneRadioButton.UseUnderline = true;
            this.noneRadioButton.Group = new GLib.SList(System.IntPtr.Zero);
            this.noneRadioButton.Remove(this.noneRadioButton.Child);
            // Container child noneRadioButton.Gtk.Container+ContainerChild
            this.label3 = new Gtk.Label();
            this.label3.Name = "label3";
            this.label3.LabelProp = Mono.Unix.Catalog.GetString("<b>Disable ChangeLog support</b>\nNo ChangeLog entries will be generated for this project.");
            this.label3.UseMarkup = true;
            this.noneRadioButton.Add(this.label3);
            this.vbox2.Add(this.noneRadioButton);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.vbox2[this.noneRadioButton]));
            w2.Position = 0;
            w2.Expand = false;
            w2.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.nearestRadioButton = new Gtk.RadioButton(Mono.Unix.Catalog.GetString("Custom policy"));
            this.nearestRadioButton.CanFocus = true;
            this.nearestRadioButton.Name = "nearestRadioButton";
            this.nearestRadioButton.DrawIndicator = true;
            this.nearestRadioButton.UseUnderline = true;
            this.nearestRadioButton.Group = this.noneRadioButton.Group;
            this.nearestRadioButton.Remove(this.nearestRadioButton.Child);
            // Container child nearestRadioButton.Gtk.Container+ContainerChild
            this.label5 = new Gtk.Label();
            this.label5.WidthRequest = 500;
            this.label5.Name = "label5";
            this.label5.LabelProp = Mono.Unix.Catalog.GetString("<b>Update nearest ChangeLog</b>\nThe nearest ChangeLog file in the directory hierarchy will be updated (below the commit directory). If none is found, a warning message will be shown. ChangeLog files will never be automatically created.");
            this.label5.UseMarkup = true;
            this.label5.Wrap = true;
            this.nearestRadioButton.Add(this.label5);
            this.vbox2.Add(this.nearestRadioButton);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.vbox2[this.nearestRadioButton]));
            w4.Position = 1;
            w4.Expand = false;
            w4.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.oneChangeLogInProjectRootDirectoryRadioButton = new Gtk.RadioButton(Mono.Unix.Catalog.GetString("One ChangeLog in the project root directory"));
            this.oneChangeLogInProjectRootDirectoryRadioButton.CanFocus = true;
            this.oneChangeLogInProjectRootDirectoryRadioButton.Name = "oneChangeLogInProjectRootDirectoryRadioButton";
            this.oneChangeLogInProjectRootDirectoryRadioButton.DrawIndicator = true;
            this.oneChangeLogInProjectRootDirectoryRadioButton.UseUnderline = true;
            this.oneChangeLogInProjectRootDirectoryRadioButton.Group = this.noneRadioButton.Group;
            this.oneChangeLogInProjectRootDirectoryRadioButton.Remove(this.oneChangeLogInProjectRootDirectoryRadioButton.Child);
            // Container child oneChangeLogInProjectRootDirectoryRadioButton.Gtk.Container+ContainerChild
            this.label6 = new Gtk.Label();
            this.label6.WidthRequest = 500;
            this.label6.Name = "label6";
            this.label6.LabelProp = Mono.Unix.Catalog.GetString("<b>Single project ChangeLog</b>\nAll changes done in the project files will be logged in a single ChangeLog file, located at the project root directory. The ChangeLog file will be created if it doesn't exist.");
            this.label6.UseMarkup = true;
            this.label6.Wrap = true;
            this.oneChangeLogInProjectRootDirectoryRadioButton.Add(this.label6);
            this.vbox2.Add(this.oneChangeLogInProjectRootDirectoryRadioButton);
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(this.vbox2[this.oneChangeLogInProjectRootDirectoryRadioButton]));
            w6.Position = 2;
            w6.Expand = false;
            w6.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.oneChangeLogInEachDirectoryRadioButton = new Gtk.RadioButton(Mono.Unix.Catalog.GetString("One ChangeLog in each directory"));
            this.oneChangeLogInEachDirectoryRadioButton.CanFocus = true;
            this.oneChangeLogInEachDirectoryRadioButton.Name = "oneChangeLogInEachDirectoryRadioButton";
            this.oneChangeLogInEachDirectoryRadioButton.DrawIndicator = true;
            this.oneChangeLogInEachDirectoryRadioButton.UseUnderline = true;
            this.oneChangeLogInEachDirectoryRadioButton.Group = this.noneRadioButton.Group;
            this.oneChangeLogInEachDirectoryRadioButton.Remove(this.oneChangeLogInEachDirectoryRadioButton.Child);
            // Container child oneChangeLogInEachDirectoryRadioButton.Gtk.Container+ContainerChild
            this.label7 = new Gtk.Label();
            this.label7.WidthRequest = 500;
            this.label7.Name = "label7";
            this.label7.LabelProp = Mono.Unix.Catalog.GetString("<b>One ChangeLog in each directory</b>\nFile changes will be logged in a ChangeLog located at the file's directory. The ChangeLog file will be created if it doesn't exist.");
            this.label7.UseMarkup = true;
            this.label7.Wrap = true;
            this.oneChangeLogInEachDirectoryRadioButton.Add(this.label7);
            this.vbox2.Add(this.oneChangeLogInEachDirectoryRadioButton);
            Gtk.Box.BoxChild w8 = ((Gtk.Box.BoxChild)(this.vbox2[this.oneChangeLogInEachDirectoryRadioButton]));
            w8.Position = 3;
            w8.Expand = false;
            w8.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.alignment1 = new Gtk.Alignment(0.5F, 0.5F, 1F, 1F);
            this.alignment1.Name = "alignment1";
            // Container child alignment1.Gtk.Container+ContainerChild
            this.alignment2 = new Gtk.Alignment(0F, 0F, 1F, 1F);
            this.alignment2.Name = "alignment2";
            this.alignment2.TopPadding = ((uint)(18));
            // Container child alignment2.Gtk.Container+ContainerChild
            this.checkVersionControl = new Gtk.CheckButton();
            this.checkVersionControl.CanFocus = true;
            this.checkVersionControl.Name = "checkVersionControl";
            this.checkVersionControl.Label = Mono.Unix.Catalog.GetString("Integrate with _version control");
            this.checkVersionControl.DrawIndicator = true;
            this.checkVersionControl.UseUnderline = true;
            this.alignment2.Add(this.checkVersionControl);
            this.alignment1.Add(this.alignment2);
            this.vbox2.Add(this.alignment1);
            Gtk.Box.BoxChild w11 = ((Gtk.Box.BoxChild)(this.vbox2[this.alignment1]));
            w11.Position = 4;
            w11.Expand = false;
            w11.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.alignment3 = new Gtk.Alignment(0F, 0F, 1F, 1F);
            this.alignment3.Name = "alignment3";
            this.alignment3.LeftPadding = ((uint)(24));
            // Container child alignment3.Gtk.Container+ContainerChild
            this.checkRequireOnCommit = new Gtk.CheckButton();
            this.checkRequireOnCommit.CanFocus = true;
            this.checkRequireOnCommit.Name = "checkRequireOnCommit";
            this.checkRequireOnCommit.Label = Mono.Unix.Catalog.GetString("_Require ChangeLog entries for all files when committing");
            this.checkRequireOnCommit.DrawIndicator = true;
            this.checkRequireOnCommit.UseUnderline = true;
            this.alignment3.Add(this.checkRequireOnCommit);
            this.vbox2.Add(this.alignment3);
            Gtk.Box.BoxChild w13 = ((Gtk.Box.BoxChild)(this.vbox2[this.alignment3]));
            w13.PackType = ((Gtk.PackType)(1));
            w13.Position = 6;
            w13.Expand = false;
            w13.Fill = false;
            this.notebook1.Add(this.vbox2);
            // Notebook tab
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.LabelProp = Mono.Unix.Catalog.GetString("ChangLog Generation");
            this.notebook1.SetTabLabel(this.vbox2, this.label1);
            this.label1.ShowAll();
            // Container child notebook1.Gtk.Notebook+NotebookChild
            this.vbox1 = new Gtk.VBox();
            this.vbox1.Name = "vbox1";
            this.vbox1.Spacing = 6;
            this.vbox1.BorderWidth = ((uint)(9));
            // Container child vbox1.Gtk.Box+BoxChild
            this.messageWidget = new MonoDevelop.VersionControl.CommitMessageStylePanelWidget();
            this.messageWidget.Events = ((Gdk.EventMask)(256));
            this.messageWidget.Name = "messageWidget";
            this.vbox1.Add(this.messageWidget);
            Gtk.Box.BoxChild w15 = ((Gtk.Box.BoxChild)(this.vbox1[this.messageWidget]));
            w15.Position = 0;
            this.notebook1.Add(this.vbox1);
            Gtk.Notebook.NotebookChild w16 = ((Gtk.Notebook.NotebookChild)(this.notebook1[this.vbox1]));
            w16.Position = 1;
            // Notebook tab
            this.label2 = new Gtk.Label();
            this.label2.Name = "label2";
            this.label2.LabelProp = Mono.Unix.Catalog.GetString("Message Style");
            this.notebook1.SetTabLabel(this.vbox1, this.label2);
            this.label2.ShowAll();
            this.Add(this.notebook1);
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.Show();
            this.noneRadioButton.Toggled += new System.EventHandler(this.ValueChanged);
            this.nearestRadioButton.Toggled += new System.EventHandler(this.ValueChanged);
            this.oneChangeLogInProjectRootDirectoryRadioButton.Toggled += new System.EventHandler(this.ValueChanged);
            this.oneChangeLogInEachDirectoryRadioButton.Toggled += new System.EventHandler(this.ValueChanged);
            this.checkVersionControl.Toggled += new System.EventHandler(this.ValueChanged);
            this.checkRequireOnCommit.Toggled += new System.EventHandler(this.ValueChanged);
            this.messageWidget.Changed += new System.EventHandler(this.OnMessageWidgetChanged);
        }
    }
}
