ADDIN_BUILD = $(top_builddir)/build/AddIns/MonoDevelop.Moonlight
ASSEMBLY = $(ADDIN_BUILD)/MonoDevelop.Moonlight.dll

DEPS =  \
	$(top_builddir)/build/AddIns/MonoDevelop.AspNet/MonoDevelop.AspNet.dll \
	$(top_builddir)/build/AddIns/MonoDevelop.Deployment/MonoDevelop.Deployment.dll \
	$(top_builddir)/build/AddIns/MonoDevelop.DesignerSupport/MonoDevelop.DesignerSupport.dll \
	$(top_builddir)/build/AddIns/MonoDevelop.XmlEditor/MonoDevelop.XmlEditor.dll \
	$(top_builddir)/build/bin/MonoDevelop.Core.dll \
	$(top_builddir)/build/bin/MonoDevelop.Ide.dll

REFS =  \
	$(GLIB_SHARP_LIBS) \
	$(GTK_SHARP_LIBS) \
	-r:ICSharpCode.SharpZipLib \
	-r:Mono.Posix \
	-r:System \
	-r:System.Core \
	-r:System.Drawing \
	-r:System.Xml

FILES =  \
	AssemblyInfo.cs \
	gtk-gui/generated.cs \
	gtk-gui/MonoDevelop.Moonlight.Gui.MoonlightOptionsPanelWidget.cs \
	MonoDevelop.Moonlight.Gui/MoonlightOptionsPanel.cs \
	MonoDevelop.Moonlight.Gui/MoonlightOptionsPanelWidget.cs \
	MonoDevelop.Moonlight/MoonlightBuildExtension.cs \
	MonoDevelop.Moonlight/MoonlightEditorExtension.cs \
	MonoDevelop.Moonlight/MoonlightExecutionCommand.cs \
	MonoDevelop.Moonlight/MoonlightExecutionHandler.cs \
	MonoDevelop.Moonlight/MoonlightFrameworkBackend.cs \
	MonoDevelop.Moonlight/MoonlightParser.cs \
	MonoDevelop.Moonlight/MoonlightProject.cs \
	MonoDevelop.Moonlight/MoonlightProjectBinding.cs \
	MonoDevelop.Moonlight/MoonlightProjectConfiguration.cs \
	MonoDevelop.Moonlight/XamlG.cs

RES =  \
	gtk-gui/gui.stetic \
	MonoDevelop.Moonlight.addin.xml \
	Templates/Moonlight-ApplicationProject.xpt.xml \
	Templates/Moonlight-ApplicationXaml.xft.xml \
	Templates/Moonlight-LibraryProject.xpt.xml \
	Templates/Moonlight-UserControlXaml.xft.xml \
	Templates/PreviewTemplate.html

DATA_FILES =  \
	Templates/framework_SL_2_0.xml \
	Templates/framework_SL_3_0.xml \
	Templates/framework_SL_4_0.xml \
	Templates/Silverlight.js

DATA_FILE_BUILD = $(addprefix $(ADDIN_BUILD)/Templates/, $(notdir $(DATA_FILES)))

all: $(ASSEMBLY) $(ASSEMBLY).mdb $(DATA_FILE_BUILD)

$(DATA_FILE_BUILD): $(srcdir)$(subst $(ADDIN_BUILD),, $@)
	mkdir -p $(ADDIN_BUILD)/Templates
	cp $(srcdir)/$(subst $(ADDIN_BUILD),,$@) $@

$(ASSEMBLY): $(build_sources) $(build_resources) $(DEPS)
	mkdir -p $(ADDIN_BUILD)
	$(CSC) $(CSC_FLAGS) -debug -out:$@ -target:library $(REFS) $(build_deps) \
		$(build_resources:%=/resource:%) $(build_sources)

$(ASSEMBLY).mdb: $(ASSEMBLY)

check: all

mdmoonlightlibdir = $(MD_ADDIN_DIR)/MonoDevelop.Moonlight
mdmoonlightlib_DATA = $(ASSEMBLY) $(ASSEMBLY).mdb

mdmoonlighttemplatelibdir = $(MD_ADDIN_DIR)/MonoDevelop.Moonlight/Templates
mdmoonlighttemplatelib_DATA = $(DATA_FILE_BUILD)

CLEANFILES = $(ASSEMBLY) $(ASSEMBLY).mdb $(DATA_FILE_BUILD)

EXTRA_DIST = $(FILES) $(RES) $(DATA_FILES)

include $(top_srcdir)/Makefile.include

