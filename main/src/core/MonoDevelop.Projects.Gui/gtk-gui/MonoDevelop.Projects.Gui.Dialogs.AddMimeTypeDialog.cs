// ------------------------------------------------------------------------------
//  <autogenerated>
//      This code was generated by a tool.
//      
// 
//      Changes to this file may cause incorrect behavior and will be lost if 
//      the code is regenerated.
//  </autogenerated>
// ------------------------------------------------------------------------------

namespace MonoDevelop.Projects.Gui.Dialogs {
    
    
    internal partial class AddMimeTypeDialog {
        
        private Gtk.VBox vbox2;
        
        private Gtk.Label label1;
        
        private Gtk.Entry entry;
        
        private Gtk.HBox hbox1;
        
        private Gtk.Image image;
        
        private Gtk.Label labelDesc;
        
        private Gtk.Button buttonCancel;
        
        private Gtk.Button buttonOk;
        
        protected virtual void Build() {
            Stetic.Gui.Initialize(this);
            // Widget MonoDevelop.Projects.Gui.Dialogs.AddMimeTypeDialog
            this.Name = "MonoDevelop.Projects.Gui.Dialogs.AddMimeTypeDialog";
            this.Title = MonoDevelop.Core.GettextCatalog.GetString("Add File Type");
            this.WindowPosition = ((Gtk.WindowPosition)(4));
            // Internal child MonoDevelop.Projects.Gui.Dialogs.AddMimeTypeDialog.VBox
            Gtk.VBox w1 = this.VBox;
            w1.Name = "dialog1_VBox";
            w1.BorderWidth = ((uint)(2));
            // Container child dialog1_VBox.Gtk.Box+BoxChild
            this.vbox2 = new Gtk.VBox();
            this.vbox2.Name = "vbox2";
            this.vbox2.Spacing = 6;
            this.vbox2.BorderWidth = ((uint)(9));
            // Container child vbox2.Gtk.Box+BoxChild
            this.label1 = new Gtk.Label();
            this.label1.Name = "label1";
            this.label1.Xalign = 0F;
            this.label1.LabelProp = MonoDevelop.Core.GettextCatalog.GetString("Enter a file extension or a mime type name:");
            this.vbox2.Add(this.label1);
            Gtk.Box.BoxChild w2 = ((Gtk.Box.BoxChild)(this.vbox2[this.label1]));
            w2.Position = 0;
            w2.Expand = false;
            w2.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.entry = new Gtk.Entry();
            this.entry.CanFocus = true;
            this.entry.Name = "entry";
            this.entry.IsEditable = true;
            this.entry.InvisibleChar = '●';
            this.vbox2.Add(this.entry);
            Gtk.Box.BoxChild w3 = ((Gtk.Box.BoxChild)(this.vbox2[this.entry]));
            w3.Position = 1;
            w3.Expand = false;
            w3.Fill = false;
            // Container child vbox2.Gtk.Box+BoxChild
            this.hbox1 = new Gtk.HBox();
            this.hbox1.Name = "hbox1";
            this.hbox1.Spacing = 6;
            // Container child hbox1.Gtk.Box+BoxChild
            this.image = new Gtk.Image();
            this.image.Name = "image";
            this.image.Pixbuf = Stetic.IconLoader.LoadIcon(this, "gtk-dialog-error", Gtk.IconSize.Menu, 16);
            this.hbox1.Add(this.image);
            Gtk.Box.BoxChild w4 = ((Gtk.Box.BoxChild)(this.hbox1[this.image]));
            w4.Position = 0;
            w4.Expand = false;
            w4.Fill = false;
            // Container child hbox1.Gtk.Box+BoxChild
            this.labelDesc = new Gtk.Label();
            this.labelDesc.Name = "labelDesc";
            this.labelDesc.Xalign = 0F;
            this.hbox1.Add(this.labelDesc);
            Gtk.Box.BoxChild w5 = ((Gtk.Box.BoxChild)(this.hbox1[this.labelDesc]));
            w5.Position = 1;
            this.vbox2.Add(this.hbox1);
            Gtk.Box.BoxChild w6 = ((Gtk.Box.BoxChild)(this.vbox2[this.hbox1]));
            w6.Position = 2;
            w6.Expand = false;
            w6.Fill = false;
            w1.Add(this.vbox2);
            Gtk.Box.BoxChild w7 = ((Gtk.Box.BoxChild)(w1[this.vbox2]));
            w7.Position = 0;
            w7.Expand = false;
            w7.Fill = false;
            // Internal child MonoDevelop.Projects.Gui.Dialogs.AddMimeTypeDialog.ActionArea
            Gtk.HButtonBox w8 = this.ActionArea;
            w8.Name = "dialog1_ActionArea";
            w8.Spacing = 10;
            w8.BorderWidth = ((uint)(5));
            w8.LayoutStyle = ((Gtk.ButtonBoxStyle)(4));
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonCancel = new Gtk.Button();
            this.buttonCancel.CanDefault = true;
            this.buttonCancel.CanFocus = true;
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.UseStock = true;
            this.buttonCancel.UseUnderline = true;
            this.buttonCancel.Label = "gtk-cancel";
            this.AddActionWidget(this.buttonCancel, -6);
            Gtk.ButtonBox.ButtonBoxChild w9 = ((Gtk.ButtonBox.ButtonBoxChild)(w8[this.buttonCancel]));
            w9.Expand = false;
            w9.Fill = false;
            // Container child dialog1_ActionArea.Gtk.ButtonBox+ButtonBoxChild
            this.buttonOk = new Gtk.Button();
            this.buttonOk.CanDefault = true;
            this.buttonOk.CanFocus = true;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseStock = true;
            this.buttonOk.UseUnderline = true;
            this.buttonOk.Label = "gtk-add";
            this.AddActionWidget(this.buttonOk, -5);
            Gtk.ButtonBox.ButtonBoxChild w10 = ((Gtk.ButtonBox.ButtonBoxChild)(w8[this.buttonOk]));
            w10.Position = 1;
            w10.Expand = false;
            w10.Fill = false;
            if ((this.Child != null)) {
                this.Child.ShowAll();
            }
            this.DefaultWidth = 400;
            this.DefaultHeight = 164;
            this.image.Hide();
            this.Show();
            this.entry.Changed += new System.EventHandler(this.OnEntryChanged);
        }
    }
}
