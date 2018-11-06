<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmRetentionPolicyList
#Region "Windows Form Designer generated code "
    <System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
        MyBase.New()

        IsInitializing = True

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        IsInitializing = False

    End Sub
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
        If Disposing Then
            If Not components Is Nothing Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(Disposing)
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents Frame1 As System.Windows.Forms.GroupBox
    Public WithEvents cmdDeleteRetentionPolicy As System.Windows.Forms.Button
    Public WithEvents cmdAddRetentionPolicy As System.Windows.Forms.Button
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdEditRetentionPolicy As System.Windows.Forms.Button
    Public WithEvents Label2 As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
    Public WithEvents lblLocDesc As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
    Public WithEvents lblSubTeam As Microsoft.VisualBasic.Compatibility.VB6.LabelArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmRetentionPolicyList))
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("RetentionPolicyId")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Schema", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Descending, False)
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Table")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("ReferenceColumn")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("DaysToKeep")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("TimeToStart")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("TimeToEnd")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("IncludedInDailyPurge")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("DailyPurgeCompleted")
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("PurgeJobName")
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("LastPurgedDateTime")
        Dim Appearance17 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance18 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance19 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance20 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance21 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance22 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance23 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance24 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance25 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance26 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance27 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance28 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance29 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance30 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdDeleteRetentionPolicy = New System.Windows.Forms.Button()
        Me.cmdAddRetentionPolicy = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdEditRetentionPolicy = New System.Windows.Forms.Button()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.cmdReset = New System.Windows.Forms.Button()
        Me.Frame1 = New System.Windows.Forms.GroupBox()
        Me.Label_IsSecureTransfer = New System.Windows.Forms.Label()
        Me.CheckBox_IncludedInDailyPurge = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cboTable = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label2 = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.lblLocDesc = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.lblSubTeam = New Microsoft.VisualBasic.Compatibility.VB6.LabelArray(Me.components)
        Me.ugrdRetentionPolicy = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.Frame1.SuspendLayout()
        CType(Me.Label2, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblLocDesc, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.lblSubTeam, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ugrdRetentionPolicy, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdDeleteRetentionPolicy
        '
        Me.cmdDeleteRetentionPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDeleteRetentionPolicy.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdDeleteRetentionPolicy, "cmdDeleteRetentionPolicy")
        Me.cmdDeleteRetentionPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDeleteRetentionPolicy.Name = "cmdDeleteRetentionPolicy"
        Me.ToolTip1.SetToolTip(Me.cmdDeleteRetentionPolicy, resources.GetString("cmdDeleteRetentionPolicy.ToolTip"))
        Me.cmdDeleteRetentionPolicy.UseVisualStyleBackColor = False
        '
        'cmdAddRetentionPolicy
        '
        Me.cmdAddRetentionPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAddRetentionPolicy.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdAddRetentionPolicy, "cmdAddRetentionPolicy")
        Me.cmdAddRetentionPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAddRetentionPolicy.Name = "cmdAddRetentionPolicy"
        Me.ToolTip1.SetToolTip(Me.cmdAddRetentionPolicy, resources.GetString("cmdAddRetentionPolicy.ToolTip"))
        Me.cmdAddRetentionPolicy.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdEditRetentionPolicy
        '
        Me.cmdEditRetentionPolicy.BackColor = System.Drawing.SystemColors.Control
        Me.cmdEditRetentionPolicy.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdEditRetentionPolicy, "cmdEditRetentionPolicy")
        Me.cmdEditRetentionPolicy.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdEditRetentionPolicy.Name = "cmdEditRetentionPolicy"
        Me.cmdEditRetentionPolicy.Tag = "B"
        Me.ToolTip1.SetToolTip(Me.cmdEditRetentionPolicy, resources.GetString("cmdEditRetentionPolicy.ToolTip"))
        Me.cmdEditRetentionPolicy.UseVisualStyleBackColor = False
        '
        'cmdSearch
        '
        Me.cmdSearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdSearch.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdSearch, "cmdSearch")
        Me.cmdSearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdSearch.Name = "cmdSearch"
        Me.ToolTip1.SetToolTip(Me.cmdSearch, resources.GetString("cmdSearch.ToolTip"))
        Me.cmdSearch.UseVisualStyleBackColor = False
        '
        'cmdReset
        '
        Me.cmdReset.BackColor = System.Drawing.SystemColors.Control
        Me.cmdReset.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdReset, "cmdReset")
        Me.cmdReset.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdReset.Name = "cmdReset"
        Me.ToolTip1.SetToolTip(Me.cmdReset, resources.GetString("cmdReset.ToolTip"))
        Me.cmdReset.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.cmdReset)
        Me.Frame1.Controls.Add(Me.cmdSearch)
        Me.Frame1.Controls.Add(Me.Label_IsSecureTransfer)
        Me.Frame1.Controls.Add(Me.CheckBox_IncludedInDailyPurge)
        Me.Frame1.Controls.Add(Me.Label1)
        Me.Frame1.Controls.Add(Me.cboTable)
        Me.Frame1.Controls.Add(Me.Label5)
        resources.ApplyResources(Me.Frame1, "Frame1")
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Name = "Frame1"
        Me.Frame1.TabStop = False
        '
        'Label_IsSecureTransfer
        '
        resources.ApplyResources(Me.Label_IsSecureTransfer, "Label_IsSecureTransfer")
        Me.Label_IsSecureTransfer.Name = "Label_IsSecureTransfer"
        '
        'CheckBox_IncludedInDailyPurge
        '
        resources.ApplyResources(Me.CheckBox_IncludedInDailyPurge, "CheckBox_IncludedInDailyPurge")
        Me.CheckBox_IncludedInDailyPurge.Name = "CheckBox_IncludedInDailyPurge"
        Me.CheckBox_IncludedInDailyPurge.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.SystemColors.Control
        Me.Label1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label1, "Label1")
        Me.Label1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label1.Name = "Label1"
        '
        'cboTable
        '
        Me.cboTable.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append
        Me.cboTable.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.cboTable.BackColor = System.Drawing.SystemColors.Window
        Me.cboTable.Cursor = System.Windows.Forms.Cursors.Default
        Me.cboTable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        resources.ApplyResources(Me.cboTable, "cboTable")
        Me.cboTable.ForeColor = System.Drawing.SystemColors.WindowText
        Me.cboTable.Name = "cboTable"
        Me.cboTable.Sorted = True
        '
        'Label5
        '
        Me.Label5.BackColor = System.Drawing.SystemColors.Control
        Me.Label5.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.Label5, "Label5")
        Me.Label5.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Label5.Name = "Label5"
        '
        'ugrdRetentionPolicy
        '
        Appearance16.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance16.BorderColor = System.Drawing.SystemColors.InactiveCaption
        resources.ApplyResources(Appearance16.FontData, "Appearance16.FontData")
        resources.ApplyResources(Appearance16, "Appearance16")
        Appearance16.ForceApplyResources = "FontData|"
        Me.ugrdRetentionPolicy.DisplayLayout.Appearance = Appearance16
        Me.ugrdRetentionPolicy.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.Hidden = True
        UltraGridColumn1.Width = 57
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.Width = 64
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.Width = 109
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.Header.Caption = resources.GetString("resource.Caption")
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.Width = 137
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn5.Header.Caption = resources.GetString("resource.Caption1")
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.Width = 56
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn6.Header.Caption = resources.GetString("resource.Caption2")
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Width = 57
        UltraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn7.Header.Caption = resources.GetString("resource.Caption3")
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.Width = 52
        UltraGridColumn8.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn8.Header.Caption = resources.GetString("resource.Caption4")
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.Width = 79
        UltraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn9.Header.Caption = resources.GetString("resource.Caption5")
        UltraGridColumn9.Header.VisiblePosition = 8
        UltraGridColumn9.Width = 83
        UltraGridColumn10.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn10.Header.Caption = resources.GetString("resource.Caption6")
        UltraGridColumn10.Header.VisiblePosition = 9
        UltraGridColumn10.Width = 127
        UltraGridColumn11.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn11.Header.Caption = resources.GetString("resource.Caption7")
        UltraGridColumn11.Header.VisiblePosition = 10
        UltraGridColumn11.Width = 114
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11})
        Me.ugrdRetentionPolicy.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdRetentionPolicy.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        resources.ApplyResources(Appearance17.FontData, "Appearance17.FontData")
        resources.ApplyResources(Appearance17, "Appearance17")
        Appearance17.ForceApplyResources = "FontData|"
        Me.ugrdRetentionPolicy.DisplayLayout.CaptionAppearance = Appearance17
        Me.ugrdRetentionPolicy.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance18.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance18.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance18.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance18.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance18.FontData, "Appearance18.FontData")
        resources.ApplyResources(Appearance18, "Appearance18")
        Appearance18.ForceApplyResources = "FontData|"
        Me.ugrdRetentionPolicy.DisplayLayout.GroupByBox.Appearance = Appearance18
        Appearance19.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance19.FontData, "Appearance19.FontData")
        resources.ApplyResources(Appearance19, "Appearance19")
        Appearance19.ForceApplyResources = "FontData|"
        Me.ugrdRetentionPolicy.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance19
        Me.ugrdRetentionPolicy.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdRetentionPolicy.DisplayLayout.GroupByBox.Hidden = True
        Appearance20.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance20.BackColor2 = System.Drawing.SystemColors.Control
        Appearance20.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance20.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance20.FontData, "Appearance20.FontData")
        resources.ApplyResources(Appearance20, "Appearance20")
        Appearance20.ForceApplyResources = "FontData|"
        Me.ugrdRetentionPolicy.DisplayLayout.GroupByBox.PromptAppearance = Appearance20
        Me.ugrdRetentionPolicy.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdRetentionPolicy.DisplayLayout.MaxRowScrollRegions = 1
        Appearance21.BackColor = System.Drawing.SystemColors.Window
        Appearance21.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Appearance21.FontData, "Appearance21.FontData")
        resources.ApplyResources(Appearance21, "Appearance21")
        Appearance21.ForceApplyResources = "FontData|"
        Me.ugrdRetentionPolicy.DisplayLayout.Override.ActiveCellAppearance = Appearance21
        Appearance22.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance22.FontData, "Appearance22.FontData")
        resources.ApplyResources(Appearance22, "Appearance22")
        Appearance22.ForceApplyResources = "FontData|"
        Me.ugrdRetentionPolicy.DisplayLayout.Override.ActiveRowAppearance = Appearance22
        Me.ugrdRetentionPolicy.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdRetentionPolicy.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance23.BackColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance23.FontData, "Appearance23.FontData")
        resources.ApplyResources(Appearance23, "Appearance23")
        Appearance23.ForceApplyResources = "FontData|"
        Me.ugrdRetentionPolicy.DisplayLayout.Override.CardAreaAppearance = Appearance23
        Appearance24.BorderColor = System.Drawing.Color.Silver
        resources.ApplyResources(Appearance24.FontData, "Appearance24.FontData")
        Appearance24.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        resources.ApplyResources(Appearance24, "Appearance24")
        Appearance24.ForceApplyResources = "FontData|"
        Me.ugrdRetentionPolicy.DisplayLayout.Override.CellAppearance = Appearance24
        Me.ugrdRetentionPolicy.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
        Me.ugrdRetentionPolicy.DisplayLayout.Override.CellPadding = 0
        resources.ApplyResources(Appearance25.FontData, "Appearance25.FontData")
        resources.ApplyResources(Appearance25, "Appearance25")
        Appearance25.ForceApplyResources = "FontData|"
        Me.ugrdRetentionPolicy.DisplayLayout.Override.FixedHeaderAppearance = Appearance25
        Appearance26.BackColor = System.Drawing.SystemColors.Control
        Appearance26.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance26.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance26.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance26.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance26.FontData, "Appearance26.FontData")
        resources.ApplyResources(Appearance26, "Appearance26")
        Appearance26.ForceApplyResources = "FontData|"
        Me.ugrdRetentionPolicy.DisplayLayout.Override.GroupByRowAppearance = Appearance26
        resources.ApplyResources(Appearance27.FontData, "Appearance27.FontData")
        resources.ApplyResources(Appearance27, "Appearance27")
        Appearance27.ForceApplyResources = "FontData|"
        Me.ugrdRetentionPolicy.DisplayLayout.Override.HeaderAppearance = Appearance27
        Me.ugrdRetentionPolicy.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdRetentionPolicy.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance28.BackColor = System.Drawing.SystemColors.Control
        resources.ApplyResources(Appearance28.FontData, "Appearance28.FontData")
        resources.ApplyResources(Appearance28, "Appearance28")
        Appearance28.ForceApplyResources = "FontData|"
        Me.ugrdRetentionPolicy.DisplayLayout.Override.RowAlternateAppearance = Appearance28
        Appearance29.BackColor = System.Drawing.SystemColors.Window
        Appearance29.BorderColor = System.Drawing.Color.Silver
        resources.ApplyResources(Appearance29.FontData, "Appearance29.FontData")
        resources.ApplyResources(Appearance29, "Appearance29")
        Appearance29.ForceApplyResources = "FontData|"
        Me.ugrdRetentionPolicy.DisplayLayout.Override.RowAppearance = Appearance29
        Me.ugrdRetentionPolicy.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdRetentionPolicy.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdRetentionPolicy.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdRetentionPolicy.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdRetentionPolicy.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.Extended
        Appearance30.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance30.FontData, "Appearance30.FontData")
        resources.ApplyResources(Appearance30, "Appearance30")
        Appearance30.ForceApplyResources = "FontData|"
        Me.ugrdRetentionPolicy.DisplayLayout.Override.TemplateAddRowAppearance = Appearance30
        Me.ugrdRetentionPolicy.DisplayLayout.Override.WrapHeaderText = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdRetentionPolicy.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdRetentionPolicy.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdRetentionPolicy.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdRetentionPolicy.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        resources.ApplyResources(Me.ugrdRetentionPolicy, "ugrdRetentionPolicy")
        Me.ugrdRetentionPolicy.Name = "ugrdRetentionPolicy"
        '
        'frmRetentionPolicyList
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.ugrdRetentionPolicy)
        Me.Controls.Add(Me.cmdDeleteRetentionPolicy)
        Me.Controls.Add(Me.cmdAddRetentionPolicy)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdEditRetentionPolicy)
        Me.Controls.Add(Me.Frame1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.KeyPreview = True
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmRetentionPolicyList"
        Me.ShowInTaskbar = False
        Me.Frame1.ResumeLayout(False)
        Me.Frame1.PerformLayout()
        CType(Me.Label2, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblLocDesc, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.lblSubTeam, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ugrdRetentionPolicy, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ugrdRetentionPolicy As Infragistics.Win.UltraWinGrid.UltraGrid
    Public WithEvents Label1 As Label
    Public WithEvents Label5 As Label
    Public WithEvents cboTable As ComboBox
    Friend WithEvents Label_IsSecureTransfer As Label
    Friend WithEvents CheckBox_IncludedInDailyPurge As CheckBox
    Public WithEvents cmdSearch As Button
    Public WithEvents cmdReset As Button
#End Region
End Class