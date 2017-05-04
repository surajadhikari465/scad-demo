<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmItemIdentifierList
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents cmdApplyCheckBox As System.Windows.Forms.Button
	Public WithEvents chkNatID As System.Windows.Forms.CheckBox
	Public WithEvents chkDefaultID As System.Windows.Forms.CheckBox
	Public WithEvents Frame1 As System.Windows.Forms.GroupBox
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdDelete As System.Windows.Forms.Button
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmItemIdentifierList))
        Dim Appearance15 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim UltraGridBand1 As Infragistics.Win.UltraWinGrid.UltraGridBand = New Infragistics.Win.UltraWinGrid.UltraGridBand("Band 0", -1)
        Dim UltraGridColumn1 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier")
        Dim UltraGridColumn2 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Default_Identifier")
        Dim UltraGridColumn3 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Add_Identifier")
        Dim UltraGridColumn4 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Remove_Identifier")
        Dim UltraGridColumn5 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("National_Identifier")
        Dim UltraGridColumn6 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Identifier_ID")
        Dim UltraGridColumn7 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("NumPluDigitsSentToScale")
        Dim UltraGridColumn8 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("IsScaleIdentifier")
        Dim UltraGridColumn9 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("Scale_Identifier", -1, Nothing, 0, Infragistics.Win.UltraWinGrid.SortIndicator.Ascending, False)
        Dim UltraGridColumn10 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("SubTeam_No")
        Dim UltraGridColumn11 As Infragistics.Win.UltraWinGrid.UltraGridColumn = New Infragistics.Win.UltraWinGrid.UltraGridColumn("IdentifierType", 0)
        Dim Appearance16 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
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
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdApplyCheckBox = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.Frame1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox_SendToScale = New System.Windows.Forms.GroupBox()
        Me.RadioButton_SendToScale_No = New System.Windows.Forms.RadioButton()
        Me.RadioButton_SendToScale_Yes = New System.Windows.Forms.RadioButton()
        Me.GroupBox_NumScaleDigits = New System.Windows.Forms.GroupBox()
        Me.RadioButton_NumScaleDigits_5 = New System.Windows.Forms.RadioButton()
        Me.RadioButton_NumScaleDigits_4 = New System.Windows.Forms.RadioButton()
        Me.chkNatID = New System.Windows.Forms.CheckBox()
        Me.chkDefaultID = New System.Windows.Forms.CheckBox()
        Me.ugrdIdentifier = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.Frame1.SuspendLayout()
        Me.GroupBox_SendToScale.SuspendLayout()
        Me.GroupBox_NumScaleDigits.SuspendLayout()
        CType(Me.ugrdIdentifier, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'cmdApplyCheckBox
        '
        Me.cmdApplyCheckBox.BackColor = System.Drawing.SystemColors.Control
        Me.cmdApplyCheckBox.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdApplyCheckBox, "cmdApplyCheckBox")
        Me.cmdApplyCheckBox.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdApplyCheckBox.Name = "cmdApplyCheckBox"
        Me.ToolTip1.SetToolTip(Me.cmdApplyCheckBox, resources.GetString("cmdApplyCheckBox.ToolTip"))
        Me.cmdApplyCheckBox.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdAdd, "cmdAdd")
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Name = "cmdAdd"
        Me.ToolTip1.SetToolTip(Me.cmdAdd, resources.GetString("cmdAdd.ToolTip"))
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdDelete, "cmdDelete")
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Name = "cmdDelete"
        Me.ToolTip1.SetToolTip(Me.cmdDelete, resources.GetString("cmdDelete.ToolTip"))
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'Frame1
        '
        Me.Frame1.BackColor = System.Drawing.SystemColors.Control
        Me.Frame1.Controls.Add(Me.GroupBox_SendToScale)
        Me.Frame1.Controls.Add(Me.GroupBox_NumScaleDigits)
        Me.Frame1.Controls.Add(Me.chkNatID)
        Me.Frame1.Controls.Add(Me.chkDefaultID)
        resources.ApplyResources(Me.Frame1, "Frame1")
        Me.Frame1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.Frame1.Name = "Frame1"
        Me.Frame1.TabStop = False
        '
        'GroupBox_SendToScale
        '
        Me.GroupBox_SendToScale.Controls.Add(Me.RadioButton_SendToScale_No)
        Me.GroupBox_SendToScale.Controls.Add(Me.RadioButton_SendToScale_Yes)
        resources.ApplyResources(Me.GroupBox_SendToScale, "GroupBox_SendToScale")
        Me.GroupBox_SendToScale.Name = "GroupBox_SendToScale"
        Me.GroupBox_SendToScale.TabStop = False
        '
        'RadioButton_SendToScale_No
        '
        resources.ApplyResources(Me.RadioButton_SendToScale_No, "RadioButton_SendToScale_No")
        Me.RadioButton_SendToScale_No.Name = "RadioButton_SendToScale_No"
        Me.RadioButton_SendToScale_No.UseVisualStyleBackColor = True
        '
        'RadioButton_SendToScale_Yes
        '
        resources.ApplyResources(Me.RadioButton_SendToScale_Yes, "RadioButton_SendToScale_Yes")
        Me.RadioButton_SendToScale_Yes.Checked = True
        Me.RadioButton_SendToScale_Yes.Name = "RadioButton_SendToScale_Yes"
        Me.RadioButton_SendToScale_Yes.TabStop = True
        Me.RadioButton_SendToScale_Yes.UseVisualStyleBackColor = True
        '
        'GroupBox_NumScaleDigits
        '
        Me.GroupBox_NumScaleDigits.Controls.Add(Me.RadioButton_NumScaleDigits_5)
        Me.GroupBox_NumScaleDigits.Controls.Add(Me.RadioButton_NumScaleDigits_4)
        resources.ApplyResources(Me.GroupBox_NumScaleDigits, "GroupBox_NumScaleDigits")
        Me.GroupBox_NumScaleDigits.Name = "GroupBox_NumScaleDigits"
        Me.GroupBox_NumScaleDigits.TabStop = False
        '
        'RadioButton_NumScaleDigits_5
        '
        resources.ApplyResources(Me.RadioButton_NumScaleDigits_5, "RadioButton_NumScaleDigits_5")
        Me.RadioButton_NumScaleDigits_5.Name = "RadioButton_NumScaleDigits_5"
        Me.RadioButton_NumScaleDigits_5.UseVisualStyleBackColor = True
        '
        'RadioButton_NumScaleDigits_4
        '
        resources.ApplyResources(Me.RadioButton_NumScaleDigits_4, "RadioButton_NumScaleDigits_4")
        Me.RadioButton_NumScaleDigits_4.Checked = True
        Me.RadioButton_NumScaleDigits_4.Name = "RadioButton_NumScaleDigits_4"
        Me.RadioButton_NumScaleDigits_4.TabStop = True
        Me.RadioButton_NumScaleDigits_4.UseVisualStyleBackColor = True
        '
        'chkNatID
        '
        Me.chkNatID.BackColor = System.Drawing.SystemColors.Control
        Me.chkNatID.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkNatID, "chkNatID")
        Me.chkNatID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkNatID.Name = "chkNatID"
        Me.chkNatID.UseVisualStyleBackColor = False
        '
        'chkDefaultID
        '
        Me.chkDefaultID.BackColor = System.Drawing.SystemColors.Control
        Me.chkDefaultID.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.chkDefaultID, "chkDefaultID")
        Me.chkDefaultID.ForeColor = System.Drawing.SystemColors.ControlText
        Me.chkDefaultID.Name = "chkDefaultID"
        Me.chkDefaultID.UseVisualStyleBackColor = False
        '
        'ugrdIdentifier
        '
        Appearance15.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance15.BorderColor = System.Drawing.SystemColors.InactiveCaption
        resources.ApplyResources(Appearance15.FontData, "Appearance15.FontData")
        resources.ApplyResources(Appearance15, "Appearance15")
        Appearance15.ForceApplyResources = "FontData|"
        Me.ugrdIdentifier.DisplayLayout.Appearance = Appearance15
        Me.ugrdIdentifier.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        UltraGridColumn1.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn1.Header.VisiblePosition = 0
        UltraGridColumn1.RowLayoutColumnInfo.OriginX = 0
        UltraGridColumn1.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn1.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn1.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn2.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn2.Header.Caption = resources.GetString("resource.Caption")
        UltraGridColumn2.Header.VisiblePosition = 1
        UltraGridColumn2.MaxWidth = 20
        UltraGridColumn2.RowLayoutColumnInfo.OriginX = 2
        UltraGridColumn2.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn2.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn2.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn3.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn3.Header.Caption = resources.GetString("resource.Caption1")
        UltraGridColumn3.Header.VisiblePosition = 2
        UltraGridColumn3.MaxWidth = 20
        UltraGridColumn3.RowLayoutColumnInfo.OriginX = 4
        UltraGridColumn3.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn3.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn3.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn4.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn4.Header.Caption = resources.GetString("resource.Caption2")
        UltraGridColumn4.Header.VisiblePosition = 3
        UltraGridColumn4.MaxWidth = 20
        UltraGridColumn4.RowLayoutColumnInfo.OriginX = 6
        UltraGridColumn4.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn4.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn4.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn5.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn5.Header.Caption = resources.GetString("resource.Caption3")
        UltraGridColumn5.Header.VisiblePosition = 4
        UltraGridColumn5.MaxWidth = 20
        UltraGridColumn5.RowLayoutColumnInfo.OriginX = 8
        UltraGridColumn5.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn5.RowLayoutColumnInfo.SpanX = 2
        UltraGridColumn5.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn6.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn6.Header.VisiblePosition = 5
        UltraGridColumn6.Hidden = True
        UltraGridColumn7.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn7.Header.Caption = resources.GetString("resource.Caption4")
        UltraGridColumn7.Header.VisiblePosition = 6
        UltraGridColumn7.RowLayoutColumnInfo.OriginX = 10
        UltraGridColumn7.RowLayoutColumnInfo.OriginY = 0
        UltraGridColumn7.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(93, 0)
        UltraGridColumn7.RowLayoutColumnInfo.SpanX = 1
        UltraGridColumn7.RowLayoutColumnInfo.SpanY = 2
        UltraGridColumn8.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn8.Header.VisiblePosition = 7
        UltraGridColumn8.Hidden = True
        UltraGridColumn9.CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
        UltraGridColumn9.Header.Caption = resources.GetString("resource.Caption5")
        UltraGridColumn9.Header.VisiblePosition = 8
        UltraGridColumn9.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(137, 0)
        UltraGridColumn10.Header.VisiblePosition = 9
        UltraGridColumn10.Hidden = True
        UltraGridColumn11.Header.VisiblePosition = 10
        UltraGridColumn11.RowLayoutColumnInfo.PreferredCellSize = New System.Drawing.Size(92, 0)
        UltraGridBand1.Columns.AddRange(New Object() {UltraGridColumn1, UltraGridColumn2, UltraGridColumn3, UltraGridColumn4, UltraGridColumn5, UltraGridColumn6, UltraGridColumn7, UltraGridColumn8, UltraGridColumn9, UltraGridColumn10, UltraGridColumn11})
        UltraGridBand1.RowLayoutStyle = Infragistics.Win.UltraWinGrid.RowLayoutStyle.ColumnLayout
        Me.ugrdIdentifier.DisplayLayout.BandsSerializer.Add(UltraGridBand1)
        Me.ugrdIdentifier.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        resources.ApplyResources(Appearance16.FontData, "Appearance16.FontData")
        resources.ApplyResources(Appearance16, "Appearance16")
        Appearance16.ForceApplyResources = "FontData|"
        Me.ugrdIdentifier.DisplayLayout.CaptionAppearance = Appearance16
        Me.ugrdIdentifier.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[False]
        Appearance17.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance17.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance17.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance17.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance17.FontData, "Appearance17.FontData")
        resources.ApplyResources(Appearance17, "Appearance17")
        Appearance17.ForceApplyResources = "FontData|"
        Me.ugrdIdentifier.DisplayLayout.GroupByBox.Appearance = Appearance17
        Appearance18.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance18.FontData, "Appearance18.FontData")
        resources.ApplyResources(Appearance18, "Appearance18")
        Appearance18.ForceApplyResources = "FontData|"
        Me.ugrdIdentifier.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance18
        Me.ugrdIdentifier.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdIdentifier.DisplayLayout.GroupByBox.Hidden = True
        Appearance19.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance19.BackColor2 = System.Drawing.SystemColors.Control
        Appearance19.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance19.ForeColor = System.Drawing.SystemColors.GrayText
        resources.ApplyResources(Appearance19.FontData, "Appearance19.FontData")
        resources.ApplyResources(Appearance19, "Appearance19")
        Appearance19.ForceApplyResources = "FontData|"
        Me.ugrdIdentifier.DisplayLayout.GroupByBox.PromptAppearance = Appearance19
        Me.ugrdIdentifier.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdIdentifier.DisplayLayout.MaxRowScrollRegions = 1
        Appearance20.BackColor = System.Drawing.SystemColors.Window
        Appearance20.ForeColor = System.Drawing.SystemColors.ControlText
        resources.ApplyResources(Appearance20.FontData, "Appearance20.FontData")
        resources.ApplyResources(Appearance20, "Appearance20")
        Appearance20.ForceApplyResources = "FontData|"
        Me.ugrdIdentifier.DisplayLayout.Override.ActiveCellAppearance = Appearance20
        Me.ugrdIdentifier.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdIdentifier.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance21.BackColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance21.FontData, "Appearance21.FontData")
        resources.ApplyResources(Appearance21, "Appearance21")
        Appearance21.ForceApplyResources = "FontData|"
        Me.ugrdIdentifier.DisplayLayout.Override.CardAreaAppearance = Appearance21
        Appearance22.BorderColor = System.Drawing.Color.Silver
        resources.ApplyResources(Appearance22.FontData, "Appearance22.FontData")
        Appearance22.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        resources.ApplyResources(Appearance22, "Appearance22")
        Appearance22.ForceApplyResources = "FontData|"
        Me.ugrdIdentifier.DisplayLayout.Override.CellAppearance = Appearance22
        Me.ugrdIdentifier.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdIdentifier.DisplayLayout.Override.CellPadding = 0
        resources.ApplyResources(Appearance23.FontData, "Appearance23.FontData")
        resources.ApplyResources(Appearance23, "Appearance23")
        Appearance23.ForceApplyResources = "FontData|"
        Me.ugrdIdentifier.DisplayLayout.Override.FixedHeaderAppearance = Appearance23
        Appearance24.BackColor = System.Drawing.SystemColors.Control
        Appearance24.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance24.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance24.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance24.BorderColor = System.Drawing.SystemColors.Window
        resources.ApplyResources(Appearance24.FontData, "Appearance24.FontData")
        resources.ApplyResources(Appearance24, "Appearance24")
        Appearance24.ForceApplyResources = "FontData|"
        Me.ugrdIdentifier.DisplayLayout.Override.GroupByRowAppearance = Appearance24
        resources.ApplyResources(Appearance25.FontData, "Appearance25.FontData")
        resources.ApplyResources(Appearance25, "Appearance25")
        Appearance25.ForceApplyResources = "FontData|"
        Me.ugrdIdentifier.DisplayLayout.Override.HeaderAppearance = Appearance25
        Me.ugrdIdentifier.DisplayLayout.Override.HeaderClickAction = Infragistics.Win.UltraWinGrid.HeaderClickAction.SortSingle
        Me.ugrdIdentifier.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance26.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance26.FontData, "Appearance26.FontData")
        resources.ApplyResources(Appearance26, "Appearance26")
        Appearance26.ForceApplyResources = "FontData|"
        Me.ugrdIdentifier.DisplayLayout.Override.RowAlternateAppearance = Appearance26
        Appearance27.BackColor = System.Drawing.SystemColors.Window
        Appearance27.BorderColor = System.Drawing.Color.Silver
        resources.ApplyResources(Appearance27.FontData, "Appearance27.FontData")
        resources.ApplyResources(Appearance27, "Appearance27")
        Appearance27.ForceApplyResources = "FontData|"
        Me.ugrdIdentifier.DisplayLayout.Override.RowAppearance = Appearance27
        Me.ugrdIdentifier.DisplayLayout.Override.RowSelectors = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdIdentifier.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdIdentifier.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdIdentifier.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdIdentifier.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance28.BackColor = System.Drawing.SystemColors.ControlLight
        resources.ApplyResources(Appearance28.FontData, "Appearance28.FontData")
        resources.ApplyResources(Appearance28, "Appearance28")
        Appearance28.ForceApplyResources = "FontData|"
        Me.ugrdIdentifier.DisplayLayout.Override.TemplateAddRowAppearance = Appearance28
        Me.ugrdIdentifier.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdIdentifier.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdIdentifier.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdIdentifier.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        resources.ApplyResources(Me.ugrdIdentifier, "ugrdIdentifier")
        Me.ugrdIdentifier.Name = "ugrdIdentifier"
        '
        'frmItemIdentifierList
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.ugrdIdentifier)
        Me.Controls.Add(Me.Frame1)
        Me.Controls.Add(Me.cmdApplyCheckBox)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cmdDelete)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmItemIdentifierList"
        Me.ShowInTaskbar = False
        Me.Frame1.ResumeLayout(False)
        Me.GroupBox_SendToScale.ResumeLayout(False)
        Me.GroupBox_SendToScale.PerformLayout()
        Me.GroupBox_NumScaleDigits.ResumeLayout(False)
        Me.GroupBox_NumScaleDigits.PerformLayout()
        CType(Me.ugrdIdentifier, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox_NumScaleDigits As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton_NumScaleDigits_5 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_NumScaleDigits_4 As System.Windows.Forms.RadioButton
    Friend WithEvents GroupBox_SendToScale As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton_SendToScale_No As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_SendToScale_Yes As System.Windows.Forms.RadioButton
    Private WithEvents ugrdIdentifier As Infragistics.Win.UltraWinGrid.UltraGrid
#End Region 
End Class