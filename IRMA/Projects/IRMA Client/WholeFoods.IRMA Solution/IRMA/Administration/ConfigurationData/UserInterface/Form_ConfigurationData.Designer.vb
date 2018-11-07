<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ConfigurationData
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_ConfigurationData))
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance
        Me._groupFilters = New System.Windows.Forms.GroupBox
        Me._checkHideDeleted = New System.Windows.Forms.CheckBox
        Me._buttonFilterClear = New System.Windows.Forms.Button
        Me._buttonImport = New System.Windows.Forms.Button
        Me._groupApplicationFilter = New System.Windows.Forms.GroupBox
        Me._buttonRemoveKeyAll = New System.Windows.Forms.Button
        Me._buttonViewConfiguration = New System.Windows.Forms.Button
        Me._buttonRemoveApp = New System.Windows.Forms.Button
        Me._buttonShowAppGUID = New System.Windows.Forms.Button
        Me._buttonAppKeyAdd = New System.Windows.Forms.Button
        Me._buttonAddApp = New System.Windows.Forms.Button
        Me._comboFilterApp = New System.Windows.Forms.ComboBox
        Me._comboFilterKey = New System.Windows.Forms.ComboBox
        Me._comboFilterType = New System.Windows.Forms.ComboBox
        Me._labelFilterType = New System.Windows.Forms.Label
        Me._buttonRemoveKey = New System.Windows.Forms.Button
        Me._buttonAddType = New System.Windows.Forms.Button
        Me._textFilterValue = New System.Windows.Forms.TextBox
        Me._buttonRemoveType = New System.Windows.Forms.Button
        Me._buttonAddKey = New System.Windows.Forms.Button
        Me._labelFilterApp = New System.Windows.Forms.Label
        Me._labelFilterKey = New System.Windows.Forms.Label
        Me._labelFilterValue = New System.Windows.Forms.Label
        Me._buttonAddEnv = New System.Windows.Forms.Button
        Me._buttonFilterApply = New System.Windows.Forms.Button
        Me._buttonShowEnvGUID = New System.Windows.Forms.Button
        Me._labelFilterEnv = New System.Windows.Forms.Label
        Me._comboFilterEnv = New System.Windows.Forms.ComboBox
        Me._buttonRemoveEnv = New System.Windows.Forms.Button
        Me._contextMenuGrid = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.RemoveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me._groupConfigSettings = New System.Windows.Forms.GroupBox
        Me._gridConfigList = New Infragistics.Win.UltraWinGrid.UltraGrid
        Me._formToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me._formErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me._formOpenFileDialog = New System.Windows.Forms.OpenFileDialog
        Me._formStatusStrip = New System.Windows.Forms.StatusStrip
        Me._formProgressBar = New System.Windows.Forms.ToolStripProgressBar
        Me._labelImportStatus = New System.Windows.Forms.ToolStripStatusLabel
        Me.ToolStripContainer1 = New System.Windows.Forms.ToolStripContainer
        Me._formImportWorker = New System.ComponentModel.BackgroundWorker
        Me._groupFilters.SuspendLayout()
        Me._groupApplicationFilter.SuspendLayout()
        Me._contextMenuGrid.SuspendLayout()
        Me._groupConfigSettings.SuspendLayout()
        CType(Me._gridConfigList, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me._formErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me._formStatusStrip.SuspendLayout()
        Me.ToolStripContainer1.BottomToolStripPanel.SuspendLayout()
        Me.ToolStripContainer1.ContentPanel.SuspendLayout()
        Me.ToolStripContainer1.SuspendLayout()
        Me.SuspendLayout()
        '
        '_groupFilters
        '
        Me._groupFilters.Controls.Add(Me._checkHideDeleted)
        Me._groupFilters.Controls.Add(Me._buttonFilterClear)
        Me._groupFilters.Controls.Add(Me._buttonImport)
        Me._groupFilters.Controls.Add(Me._groupApplicationFilter)
        Me._groupFilters.Controls.Add(Me._buttonAddEnv)
        Me._groupFilters.Controls.Add(Me._buttonFilterApply)
        Me._groupFilters.Controls.Add(Me._buttonShowEnvGUID)
        Me._groupFilters.Controls.Add(Me._labelFilterEnv)
        Me._groupFilters.Controls.Add(Me._comboFilterEnv)
        Me._groupFilters.Controls.Add(Me._buttonRemoveEnv)
        Me._groupFilters.Location = New System.Drawing.Point(12, 12)
        Me._groupFilters.Name = "_groupFilters"
        Me._groupFilters.Size = New System.Drawing.Size(630, 217)
        Me._groupFilters.TabIndex = 0
        Me._groupFilters.TabStop = False
        '
        '_checkHideDeleted
        '
        Me._checkHideDeleted.AutoSize = True
        Me._checkHideDeleted.Location = New System.Drawing.Point(24, 195)
        Me._checkHideDeleted.Name = "_checkHideDeleted"
        Me._checkHideDeleted.Size = New System.Drawing.Size(173, 17)
        Me._checkHideDeleted.TabIndex = 27
        Me._checkHideDeleted.Text = "Hide Deleted Key/Value Pairs"
        Me._checkHideDeleted.UseVisualStyleBackColor = True
        '
        '_buttonFilterClear
        '
        Me._buttonFilterClear.Image = CType(resources.GetObject("_buttonFilterClear.Image"), System.Drawing.Image)
        Me._buttonFilterClear.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._buttonFilterClear.Location = New System.Drawing.Point(515, 112)
        Me._buttonFilterClear.Name = "_buttonFilterClear"
        Me._buttonFilterClear.Size = New System.Drawing.Size(99, 37)
        Me._buttonFilterClear.TabIndex = 9
        Me._buttonFilterClear.Text = "Clear Filter"
        Me._buttonFilterClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me._formToolTip.SetToolTip(Me._buttonFilterClear, "Clear Configuration Settings filter")
        Me._buttonFilterClear.UseVisualStyleBackColor = True
        '
        '_buttonImport
        '
        Me._buttonImport.Enabled = False
        Me._buttonImport.Image = CType(resources.GetObject("_buttonImport.Image"), System.Drawing.Image)
        Me._buttonImport.Location = New System.Drawing.Point(515, 19)
        Me._buttonImport.Name = "_buttonImport"
        Me._buttonImport.Size = New System.Drawing.Size(99, 39)
        Me._buttonImport.TabIndex = 10
        Me._buttonImport.Text = "Import"
        Me._buttonImport.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me._buttonImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me._formToolTip.SetToolTip(Me._buttonImport, "Import an application configuration file.")
        Me._buttonImport.UseVisualStyleBackColor = True
        '
        '_groupApplicationFilter
        '
        Me._groupApplicationFilter.Controls.Add(Me._buttonRemoveKeyAll)
        Me._groupApplicationFilter.Controls.Add(Me._buttonViewConfiguration)
        Me._groupApplicationFilter.Controls.Add(Me._buttonRemoveApp)
        Me._groupApplicationFilter.Controls.Add(Me._buttonShowAppGUID)
        Me._groupApplicationFilter.Controls.Add(Me._buttonAppKeyAdd)
        Me._groupApplicationFilter.Controls.Add(Me._buttonAddApp)
        Me._groupApplicationFilter.Controls.Add(Me._comboFilterApp)
        Me._groupApplicationFilter.Controls.Add(Me._comboFilterKey)
        Me._groupApplicationFilter.Controls.Add(Me._comboFilterType)
        Me._groupApplicationFilter.Controls.Add(Me._labelFilterType)
        Me._groupApplicationFilter.Controls.Add(Me._buttonRemoveKey)
        Me._groupApplicationFilter.Controls.Add(Me._buttonAddType)
        Me._groupApplicationFilter.Controls.Add(Me._textFilterValue)
        Me._groupApplicationFilter.Controls.Add(Me._buttonRemoveType)
        Me._groupApplicationFilter.Controls.Add(Me._buttonAddKey)
        Me._groupApplicationFilter.Controls.Add(Me._labelFilterApp)
        Me._groupApplicationFilter.Controls.Add(Me._labelFilterKey)
        Me._groupApplicationFilter.Controls.Add(Me._labelFilterValue)
        Me._groupApplicationFilter.Enabled = False
        Me._groupApplicationFilter.Location = New System.Drawing.Point(18, 53)
        Me._groupApplicationFilter.Name = "_groupApplicationFilter"
        Me._groupApplicationFilter.Size = New System.Drawing.Size(470, 136)
        Me._groupApplicationFilter.TabIndex = 2
        Me._groupApplicationFilter.TabStop = False
        '
        '_buttonRemoveKeyAll
        '
        Me._buttonRemoveKeyAll.Enabled = False
        Me._buttonRemoveKeyAll.Image = CType(resources.GetObject("_buttonRemoveKeyAll.Image"), System.Drawing.Image)
        Me._buttonRemoveKeyAll.Location = New System.Drawing.Point(377, 46)
        Me._buttonRemoveKeyAll.Name = "_buttonRemoveKeyAll"
        Me._buttonRemoveKeyAll.Size = New System.Drawing.Size(26, 23)
        Me._buttonRemoveKeyAll.TabIndex = 29
        Me._buttonRemoveKeyAll.TabStop = False
        Me._formToolTip.SetToolTip(Me._buttonRemoveKeyAll, "Remove an existing key from all applications and environments.")
        Me._buttonRemoveKeyAll.UseVisualStyleBackColor = True
        '
        '_buttonViewConfiguration
        '
        Me._buttonViewConfiguration.Enabled = False
        Me._buttonViewConfiguration.Image = CType(resources.GetObject("_buttonViewConfiguration.Image"), System.Drawing.Image)
        Me._buttonViewConfiguration.Location = New System.Drawing.Point(409, 19)
        Me._buttonViewConfiguration.Name = "_buttonViewConfiguration"
        Me._buttonViewConfiguration.Size = New System.Drawing.Size(47, 50)
        Me._buttonViewConfiguration.TabIndex = 28
        Me._buttonViewConfiguration.TabStop = False
        Me._formToolTip.SetToolTip(Me._buttonViewConfiguration, "Display the current active configuration.")
        Me._buttonViewConfiguration.UseVisualStyleBackColor = True
        '
        '_buttonRemoveApp
        '
        Me._buttonRemoveApp.Enabled = False
        Me._buttonRemoveApp.Image = CType(resources.GetObject("_buttonRemoveApp.Image"), System.Drawing.Image)
        Me._buttonRemoveApp.Location = New System.Drawing.Point(350, 19)
        Me._buttonRemoveApp.Name = "_buttonRemoveApp"
        Me._buttonRemoveApp.Size = New System.Drawing.Size(26, 23)
        Me._buttonRemoveApp.TabIndex = 13
        Me._buttonRemoveApp.TabStop = False
        Me._formToolTip.SetToolTip(Me._buttonRemoveApp, "Remove an existing application.")
        Me._buttonRemoveApp.UseVisualStyleBackColor = True
        '
        '_buttonShowAppGUID
        '
        Me._buttonShowAppGUID.Enabled = False
        Me._buttonShowAppGUID.Image = CType(resources.GetObject("_buttonShowAppGUID.Image"), System.Drawing.Image)
        Me._buttonShowAppGUID.Location = New System.Drawing.Point(377, 19)
        Me._buttonShowAppGUID.Name = "_buttonShowAppGUID"
        Me._buttonShowAppGUID.Size = New System.Drawing.Size(26, 23)
        Me._buttonShowAppGUID.TabIndex = 24
        Me._buttonShowAppGUID.TabStop = False
        Me._formToolTip.SetToolTip(Me._buttonShowAppGUID, "Show Environment GUID")
        Me._buttonShowAppGUID.UseVisualStyleBackColor = True
        '
        '_buttonAppKeyAdd
        '
        Me._buttonAppKeyAdd.Enabled = False
        Me._buttonAppKeyAdd.Image = CType(resources.GetObject("_buttonAppKeyAdd.Image"), System.Drawing.Image)
        Me._buttonAppKeyAdd.Location = New System.Drawing.Point(409, 73)
        Me._buttonAppKeyAdd.Name = "_buttonAppKeyAdd"
        Me._buttonAppKeyAdd.Size = New System.Drawing.Size(47, 23)
        Me._buttonAppKeyAdd.TabIndex = 6
        Me._formToolTip.SetToolTip(Me._buttonAppKeyAdd, "Assign a value to the selected Key")
        Me._buttonAppKeyAdd.UseVisualStyleBackColor = True
        '
        '_buttonAddApp
        '
        Me._buttonAddApp.Image = CType(resources.GetObject("_buttonAddApp.Image"), System.Drawing.Image)
        Me._buttonAddApp.Location = New System.Drawing.Point(322, 19)
        Me._buttonAddApp.Name = "_buttonAddApp"
        Me._buttonAddApp.Size = New System.Drawing.Size(26, 23)
        Me._buttonAddApp.TabIndex = 16
        Me._buttonAddApp.TabStop = False
        Me._formToolTip.SetToolTip(Me._buttonAddApp, "Add a new application")
        Me._buttonAddApp.UseVisualStyleBackColor = True
        '
        '_comboFilterApp
        '
        Me._comboFilterApp.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me._comboFilterApp.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._comboFilterApp.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._comboFilterApp.FormattingEnabled = True
        Me._comboFilterApp.Location = New System.Drawing.Point(84, 20)
        Me._comboFilterApp.Name = "_comboFilterApp"
        Me._comboFilterApp.Size = New System.Drawing.Size(217, 21)
        Me._comboFilterApp.TabIndex = 3
        '
        '_comboFilterKey
        '
        Me._comboFilterKey.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me._comboFilterKey.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._comboFilterKey.FormattingEnabled = True
        Me._comboFilterKey.Location = New System.Drawing.Point(84, 47)
        Me._comboFilterKey.Name = "_comboFilterKey"
        Me._comboFilterKey.Size = New System.Drawing.Size(217, 21)
        Me._comboFilterKey.TabIndex = 4
        '
        '_comboFilterType
        '
        Me._comboFilterType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me._comboFilterType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._comboFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._comboFilterType.FormattingEnabled = True
        Me._comboFilterType.Location = New System.Drawing.Point(84, 102)
        Me._comboFilterType.Name = "_comboFilterType"
        Me._comboFilterType.Size = New System.Drawing.Size(217, 21)
        Me._comboFilterType.TabIndex = 7
        '
        '_labelFilterType
        '
        Me._labelFilterType.AutoSize = True
        Me._labelFilterType.Location = New System.Drawing.Point(45, 105)
        Me._labelFilterType.Name = "_labelFilterType"
        Me._labelFilterType.Size = New System.Drawing.Size(33, 13)
        Me._labelFilterType.TabIndex = 11
        Me._labelFilterType.Text = "Type:"
        '
        '_buttonRemoveKey
        '
        Me._buttonRemoveKey.Enabled = False
        Me._buttonRemoveKey.Image = CType(resources.GetObject("_buttonRemoveKey.Image"), System.Drawing.Image)
        Me._buttonRemoveKey.Location = New System.Drawing.Point(350, 46)
        Me._buttonRemoveKey.Name = "_buttonRemoveKey"
        Me._buttonRemoveKey.Size = New System.Drawing.Size(26, 23)
        Me._buttonRemoveKey.TabIndex = 19
        Me._buttonRemoveKey.TabStop = False
        Me._formToolTip.SetToolTip(Me._buttonRemoveKey, "Remove an existing key")
        Me._buttonRemoveKey.UseVisualStyleBackColor = True
        '
        '_buttonAddType
        '
        Me._buttonAddType.Image = CType(resources.GetObject("_buttonAddType.Image"), System.Drawing.Image)
        Me._buttonAddType.Location = New System.Drawing.Point(322, 101)
        Me._buttonAddType.Name = "_buttonAddType"
        Me._buttonAddType.Size = New System.Drawing.Size(26, 23)
        Me._buttonAddType.TabIndex = 14
        Me._buttonAddType.TabStop = False
        Me._formToolTip.SetToolTip(Me._buttonAddType, "Add a new application type")
        Me._buttonAddType.UseVisualStyleBackColor = True
        Me._buttonAddType.Visible = False
        '
        '_textFilterValue
        '
        Me._textFilterValue.Location = New System.Drawing.Point(84, 74)
        Me._textFilterValue.Name = "_textFilterValue"
        Me._textFilterValue.Size = New System.Drawing.Size(319, 22)
        Me._textFilterValue.TabIndex = 5
        '
        '_buttonRemoveType
        '
        Me._buttonRemoveType.Enabled = False
        Me._buttonRemoveType.Image = CType(resources.GetObject("_buttonRemoveType.Image"), System.Drawing.Image)
        Me._buttonRemoveType.Location = New System.Drawing.Point(350, 101)
        Me._buttonRemoveType.Name = "_buttonRemoveType"
        Me._buttonRemoveType.Size = New System.Drawing.Size(26, 23)
        Me._buttonRemoveType.TabIndex = 15
        Me._buttonRemoveType.TabStop = False
        Me._formToolTip.SetToolTip(Me._buttonRemoveType, "Remove an existing application type")
        Me._buttonRemoveType.UseVisualStyleBackColor = True
        Me._buttonRemoveType.Visible = False
        '
        '_buttonAddKey
        '
        Me._buttonAddKey.Image = CType(resources.GetObject("_buttonAddKey.Image"), System.Drawing.Image)
        Me._buttonAddKey.Location = New System.Drawing.Point(322, 46)
        Me._buttonAddKey.Name = "_buttonAddKey"
        Me._buttonAddKey.Size = New System.Drawing.Size(26, 23)
        Me._buttonAddKey.TabIndex = 18
        Me._buttonAddKey.TabStop = False
        Me._formToolTip.SetToolTip(Me._buttonAddKey, "Add a new key")
        Me._buttonAddKey.UseVisualStyleBackColor = True
        '
        '_labelFilterApp
        '
        Me._labelFilterApp.AutoSize = True
        Me._labelFilterApp.Location = New System.Drawing.Point(9, 23)
        Me._labelFilterApp.Name = "_labelFilterApp"
        Me._labelFilterApp.Size = New System.Drawing.Size(69, 13)
        Me._labelFilterApp.TabIndex = 4
        Me._labelFilterApp.Text = "Application:"
        '
        '_labelFilterKey
        '
        Me._labelFilterKey.AutoSize = True
        Me._labelFilterKey.Location = New System.Drawing.Point(51, 50)
        Me._labelFilterKey.Name = "_labelFilterKey"
        Me._labelFilterKey.Size = New System.Drawing.Size(27, 13)
        Me._labelFilterKey.TabIndex = 6
        Me._labelFilterKey.Text = "Key:"
        '
        '_labelFilterValue
        '
        Me._labelFilterValue.AutoSize = True
        Me._labelFilterValue.Location = New System.Drawing.Point(39, 77)
        Me._labelFilterValue.Name = "_labelFilterValue"
        Me._labelFilterValue.Size = New System.Drawing.Size(39, 13)
        Me._labelFilterValue.TabIndex = 7
        Me._labelFilterValue.Text = "Value:"
        '
        '_buttonAddEnv
        '
        Me._buttonAddEnv.Image = CType(resources.GetObject("_buttonAddEnv.Image"), System.Drawing.Image)
        Me._buttonAddEnv.Location = New System.Drawing.Point(340, 28)
        Me._buttonAddEnv.Name = "_buttonAddEnv"
        Me._buttonAddEnv.Size = New System.Drawing.Size(26, 23)
        Me._buttonAddEnv.TabIndex = 12
        Me._buttonAddEnv.TabStop = False
        Me._formToolTip.SetToolTip(Me._buttonAddEnv, "Add a new environment")
        Me._buttonAddEnv.UseVisualStyleBackColor = True
        '
        '_buttonFilterApply
        '
        Me._buttonFilterApply.Image = CType(resources.GetObject("_buttonFilterApply.Image"), System.Drawing.Image)
        Me._buttonFilterApply.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me._buttonFilterApply.Location = New System.Drawing.Point(515, 72)
        Me._buttonFilterApply.Name = "_buttonFilterApply"
        Me._buttonFilterApply.Size = New System.Drawing.Size(99, 37)
        Me._buttonFilterApply.TabIndex = 8
        Me._buttonFilterApply.Text = "Apply Filter"
        Me._buttonFilterApply.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me._formToolTip.SetToolTip(Me._buttonFilterApply, "Apply Configuration Settings Filter")
        Me._buttonFilterApply.UseVisualStyleBackColor = True
        '
        '_buttonShowEnvGUID
        '
        Me._buttonShowEnvGUID.Enabled = False
        Me._buttonShowEnvGUID.Image = CType(resources.GetObject("_buttonShowEnvGUID.Image"), System.Drawing.Image)
        Me._buttonShowEnvGUID.Location = New System.Drawing.Point(395, 28)
        Me._buttonShowEnvGUID.Name = "_buttonShowEnvGUID"
        Me._buttonShowEnvGUID.Size = New System.Drawing.Size(26, 23)
        Me._buttonShowEnvGUID.TabIndex = 26
        Me._buttonShowEnvGUID.TabStop = False
        Me._formToolTip.SetToolTip(Me._buttonShowEnvGUID, "Show Application GUID")
        Me._buttonShowEnvGUID.UseVisualStyleBackColor = True
        '
        '_labelFilterEnv
        '
        Me._labelFilterEnv.AutoSize = True
        Me._labelFilterEnv.Location = New System.Drawing.Point(21, 32)
        Me._labelFilterEnv.Name = "_labelFilterEnv"
        Me._labelFilterEnv.Size = New System.Drawing.Size(75, 13)
        Me._labelFilterEnv.TabIndex = 5
        Me._labelFilterEnv.Text = "Environment:"
        '
        '_comboFilterEnv
        '
        Me._comboFilterEnv.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me._comboFilterEnv.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._comboFilterEnv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me._comboFilterEnv.FormattingEnabled = True
        Me._comboFilterEnv.Location = New System.Drawing.Point(102, 29)
        Me._comboFilterEnv.Name = "_comboFilterEnv"
        Me._comboFilterEnv.Size = New System.Drawing.Size(217, 21)
        Me._comboFilterEnv.TabIndex = 1
        '
        '_buttonRemoveEnv
        '
        Me._buttonRemoveEnv.Enabled = False
        Me._buttonRemoveEnv.Image = CType(resources.GetObject("_buttonRemoveEnv.Image"), System.Drawing.Image)
        Me._buttonRemoveEnv.Location = New System.Drawing.Point(368, 28)
        Me._buttonRemoveEnv.Name = "_buttonRemoveEnv"
        Me._buttonRemoveEnv.Size = New System.Drawing.Size(26, 23)
        Me._buttonRemoveEnv.TabIndex = 17
        Me._buttonRemoveEnv.TabStop = False
        Me._formToolTip.SetToolTip(Me._buttonRemoveEnv, "Remove an existing environment.")
        Me._buttonRemoveEnv.UseVisualStyleBackColor = True
        '
        '_contextMenuGrid
        '
        Me._contextMenuGrid.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditToolStripMenuItem, Me.RemoveToolStripMenuItem})
        Me._contextMenuGrid.Name = "_contextMenuGrid"
        Me._contextMenuGrid.Size = New System.Drawing.Size(125, 48)
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.Image = CType(resources.GetObject("EditToolStripMenuItem.Image"), System.Drawing.Image)
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(124, 22)
        Me.EditToolStripMenuItem.Text = "Edit"
        '
        'RemoveToolStripMenuItem
        '
        Me.RemoveToolStripMenuItem.Image = CType(resources.GetObject("RemoveToolStripMenuItem.Image"), System.Drawing.Image)
        Me.RemoveToolStripMenuItem.Name = "RemoveToolStripMenuItem"
        Me.RemoveToolStripMenuItem.Size = New System.Drawing.Size(124, 22)
        Me.RemoveToolStripMenuItem.Text = "Remove"
        '
        '_groupConfigSettings
        '
        Me._groupConfigSettings.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._groupConfigSettings.Controls.Add(Me._gridConfigList)
        Me._groupConfigSettings.Location = New System.Drawing.Point(12, 235)
        Me._groupConfigSettings.Name = "_groupConfigSettings"
        Me._groupConfigSettings.Size = New System.Drawing.Size(767, 429)
        Me._groupConfigSettings.TabIndex = 5
        Me._groupConfigSettings.TabStop = False
        Me._groupConfigSettings.Text = "Configuration Settings"
        '
        '_gridConfigList
        '
        Me._gridConfigList.ContextMenuStrip = Me._contextMenuGrid
        Appearance1.BackColor = System.Drawing.SystemColors.Window
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me._gridConfigList.DisplayLayout.Appearance = Appearance1
        Me._gridConfigList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        Me._gridConfigList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me._gridConfigList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance2.BackColor = System.Drawing.SystemColors.Control
        Appearance2.BackColor2 = System.Drawing.SystemColors.Control
        Appearance2.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance2.BorderColor = System.Drawing.SystemColors.Window
        Me._gridConfigList.DisplayLayout.GroupByBox.Appearance = Appearance2
        Appearance3.BackColor2 = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Appearance3.ForeColor = System.Drawing.SystemColors.GrayText
        Me._gridConfigList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance3
        Me._gridConfigList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance4.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance4.BackColor2 = System.Drawing.SystemColors.Control
        Appearance4.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me._gridConfigList.DisplayLayout.GroupByBox.PromptAppearance = Appearance4
        Me._gridConfigList.DisplayLayout.MaxColScrollRegions = 1
        Me._gridConfigList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance5.BackColor = System.Drawing.SystemColors.Window
        Appearance5.ForeColor = System.Drawing.SystemColors.ControlText
        Me._gridConfigList.DisplayLayout.Override.ActiveCellAppearance = Appearance5
        Appearance6.BackColor = System.Drawing.SystemColors.Highlight
        Appearance6.ForeColor = System.Drawing.SystemColors.HighlightText
        Me._gridConfigList.DisplayLayout.Override.ActiveRowAppearance = Appearance6
        Me._gridConfigList.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me._gridConfigList.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me._gridConfigList.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[False]
        Me._gridConfigList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me._gridConfigList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me._gridConfigList.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me._gridConfigList.DisplayLayout.Override.CellAppearance = Appearance8
        Me._gridConfigList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me._gridConfigList.DisplayLayout.Override.CellPadding = 0
        Appearance9.BackColor = System.Drawing.SystemColors.Control
        Appearance9.BackColor2 = System.Drawing.SystemColors.Control
        Appearance9.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance9.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance9.BorderColor = System.Drawing.SystemColors.Window
        Me._gridConfigList.DisplayLayout.Override.GroupByRowAppearance = Appearance9
        Me._gridConfigList.DisplayLayout.Override.GroupBySummaryDisplayStyle = Infragistics.Win.UltraWinGrid.GroupBySummaryDisplayStyle.Text
        Appearance10.TextHAlignAsString = "Left"
        Me._gridConfigList.DisplayLayout.Override.HeaderAppearance = Appearance10
        Me._gridConfigList.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortMulti
        Me._gridConfigList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance11.BackColor = System.Drawing.SystemColors.Window
        Appearance11.BorderColor = System.Drawing.Color.Silver
        Me._gridConfigList.DisplayLayout.Override.RowAppearance = Appearance11
        Me._gridConfigList.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[False]
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me._gridConfigList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance12
        Me._gridConfigList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me._gridConfigList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me._gridConfigList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me._gridConfigList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me._gridConfigList.Dock = System.Windows.Forms.DockStyle.Fill
        Me._gridConfigList.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._gridConfigList.Location = New System.Drawing.Point(3, 18)
        Me._gridConfigList.Name = "_gridConfigList"
        Me._gridConfigList.Size = New System.Drawing.Size(761, 408)
        Me._gridConfigList.TabIndex = 1
        Me._gridConfigList.Text = "Configuration Settings"
        '
        '_formErrorProvider
        '
        Me._formErrorProvider.ContainerControl = Me
        '
        '_formOpenFileDialog
        '
        Me._formOpenFileDialog.DefaultExt = "config"
        Me._formOpenFileDialog.Filter = "Configuration files (*.config)|*.config|XML Files (*.xml)|*.xml"
        Me._formOpenFileDialog.SupportMultiDottedExtensions = True
        '
        '_formStatusStrip
        '
        Me._formStatusStrip.Dock = System.Windows.Forms.DockStyle.None
        Me._formStatusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me._formProgressBar, Me._labelImportStatus})
        Me._formStatusStrip.Location = New System.Drawing.Point(0, 0)
        Me._formStatusStrip.Name = "_formStatusStrip"
        Me._formStatusStrip.Size = New System.Drawing.Size(307, 22)
        Me._formStatusStrip.TabIndex = 12
        Me._formStatusStrip.Visible = False
        '
        '_formProgressBar
        '
        Me._formProgressBar.Name = "_formProgressBar"
        Me._formProgressBar.Size = New System.Drawing.Size(100, 16)
        '
        '_labelImportStatus
        '
        Me._labelImportStatus.Name = "_labelImportStatus"
        Me._labelImportStatus.Size = New System.Drawing.Size(188, 17)
        Me._labelImportStatus.Text = "Importing Application Configuration..."
        '
        'ToolStripContainer1
        '
        '
        'ToolStripContainer1.BottomToolStripPanel
        '
        Me.ToolStripContainer1.BottomToolStripPanel.Controls.Add(Me._formStatusStrip)
        '
        'ToolStripContainer1.ContentPanel
        '
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me._groupFilters)
        Me.ToolStripContainer1.ContentPanel.Controls.Add(Me._groupConfigSettings)
        Me.ToolStripContainer1.ContentPanel.Size = New System.Drawing.Size(792, 673)
        Me.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.ToolStripContainer1.LeftToolStripPanelVisible = False
        Me.ToolStripContainer1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStripContainer1.Name = "ToolStripContainer1"
        Me.ToolStripContainer1.RightToolStripPanelVisible = False
        Me.ToolStripContainer1.Size = New System.Drawing.Size(792, 673)
        Me.ToolStripContainer1.TabIndex = 13
        Me.ToolStripContainer1.Text = "ToolStripContainer1"
        Me.ToolStripContainer1.TopToolStripPanelVisible = False
        '
        '_formImportWorker
        '
        Me._formImportWorker.WorkerReportsProgress = True
        '
        'Form_ConfigurationData
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(792, 673)
        Me.Controls.Add(Me.ToolStripContainer1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(800, 700)
        Me.Name = "Form_ConfigurationData"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Manage Configuration Data"
        Me._groupFilters.ResumeLayout(False)
        Me._groupFilters.PerformLayout()
        Me._groupApplicationFilter.ResumeLayout(False)
        Me._groupApplicationFilter.PerformLayout()
        Me._contextMenuGrid.ResumeLayout(False)
        Me._groupConfigSettings.ResumeLayout(False)
        CType(Me._gridConfigList, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me._formErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me._formStatusStrip.ResumeLayout(False)
        Me._formStatusStrip.PerformLayout()
        Me.ToolStripContainer1.BottomToolStripPanel.ResumeLayout(False)
        Me.ToolStripContainer1.BottomToolStripPanel.PerformLayout()
        Me.ToolStripContainer1.ContentPanel.ResumeLayout(False)
        Me.ToolStripContainer1.ResumeLayout(False)
        Me.ToolStripContainer1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents _groupFilters As System.Windows.Forms.GroupBox
    Friend WithEvents _groupConfigSettings As System.Windows.Forms.GroupBox
    Friend WithEvents _labelFilterValue As System.Windows.Forms.Label
    Friend WithEvents _labelFilterKey As System.Windows.Forms.Label
    Friend WithEvents _labelFilterEnv As System.Windows.Forms.Label
    Friend WithEvents _labelFilterApp As System.Windows.Forms.Label
    Friend WithEvents _textFilterValue As System.Windows.Forms.TextBox
    Friend WithEvents _comboFilterKey As System.Windows.Forms.ComboBox
    Friend WithEvents _comboFilterEnv As System.Windows.Forms.ComboBox
    Friend WithEvents _comboFilterApp As System.Windows.Forms.ComboBox
    Friend WithEvents _buttonFilterClear As System.Windows.Forms.Button
    Friend WithEvents _buttonFilterApply As System.Windows.Forms.Button
    Friend WithEvents _labelFilterType As System.Windows.Forms.Label
    Friend WithEvents _comboFilterType As System.Windows.Forms.ComboBox
    Friend WithEvents _buttonAddEnv As System.Windows.Forms.Button
    Friend WithEvents _buttonRemoveKey As System.Windows.Forms.Button
    Friend WithEvents _buttonAddKey As System.Windows.Forms.Button
    Friend WithEvents _buttonRemoveEnv As System.Windows.Forms.Button
    Friend WithEvents _buttonAddApp As System.Windows.Forms.Button
    Friend WithEvents _buttonRemoveType As System.Windows.Forms.Button
    Friend WithEvents _buttonAddType As System.Windows.Forms.Button
    Friend WithEvents _buttonRemoveApp As System.Windows.Forms.Button
    Friend WithEvents _buttonShowEnvGUID As System.Windows.Forms.Button
    Friend WithEvents _buttonShowAppGUID As System.Windows.Forms.Button
    Friend WithEvents _buttonAppKeyAdd As System.Windows.Forms.Button
    Friend WithEvents _groupApplicationFilter As System.Windows.Forms.GroupBox
    Friend WithEvents _buttonViewConfiguration As System.Windows.Forms.Button
    Friend WithEvents _formToolTip As System.Windows.Forms.ToolTip
    Friend WithEvents _formErrorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents _buttonRemoveKeyAll As System.Windows.Forms.Button
    Friend WithEvents _contextMenuGrid As System.Windows.Forms.ContextMenuStrip
    Friend WithEvents EditToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents RemoveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents _buttonImport As System.Windows.Forms.Button
    Friend WithEvents _formOpenFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents ToolStripContainer1 As System.Windows.Forms.ToolStripContainer
    Friend WithEvents _formStatusStrip As System.Windows.Forms.StatusStrip
    Friend WithEvents _formProgressBar As System.Windows.Forms.ToolStripProgressBar
    Friend WithEvents _labelImportStatus As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents _formImportWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents _gridConfigList As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents _checkHideDeleted As System.Windows.Forms.CheckBox
End Class
