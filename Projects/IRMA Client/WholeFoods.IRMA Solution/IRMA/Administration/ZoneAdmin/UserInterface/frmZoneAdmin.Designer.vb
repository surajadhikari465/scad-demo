<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class ZoneAdmin
    Inherits Form_IRMABase

    'UserControl overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing AndAlso components IsNot Nothing Then
            components.Dispose()
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim Appearance1 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance2 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance3 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance4 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance5 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance6 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance7 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance8 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance9 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance10 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance11 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance12 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance13 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Dim Appearance14 As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.btnOK = New System.Windows.Forms.Button()
        Me.TaxJurisdictionGroupBox = New System.Windows.Forms.GroupBox()
        Me.ugrdZoneList = New Infragistics.Win.UltraWinGrid.UltraGrid()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnAdd = New System.Windows.Forms.Button()
        Me.TaxJurisdictionGroupBox.SuspendLayout()
        CType(Me.ugrdZoneList, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCancel
        '
        Me.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.btnCancel.Location = New System.Drawing.Point(417, 322)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(73, 24)
        Me.btnCancel.TabIndex = 22
        Me.btnCancel.Text = "Cancel"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'btnOK
        '
        Me.btnOK.Location = New System.Drawing.Point(338, 322)
        Me.btnOK.Name = "btnOK"
        Me.btnOK.Size = New System.Drawing.Size(73, 24)
        Me.btnOK.TabIndex = 21
        Me.btnOK.Text = "Ok"
        Me.btnOK.UseVisualStyleBackColor = True
        '
        'TaxJurisdictionGroupBox
        '
        Me.TaxJurisdictionGroupBox.Controls.Add(Me.ugrdZoneList)
        Me.TaxJurisdictionGroupBox.Controls.Add(Me.btnDelete)
        Me.TaxJurisdictionGroupBox.Controls.Add(Me.btnAdd)
        Me.TaxJurisdictionGroupBox.Location = New System.Drawing.Point(8, 10)
        Me.TaxJurisdictionGroupBox.Name = "TaxJurisdictionGroupBox"
        Me.TaxJurisdictionGroupBox.Size = New System.Drawing.Size(488, 306)
        Me.TaxJurisdictionGroupBox.TabIndex = 19
        Me.TaxJurisdictionGroupBox.TabStop = False
        '
        'ugrdZoneList
        '
        Me.ugrdZoneList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Appearance1.BackColor = System.Drawing.SystemColors.ControlLight
        Appearance1.BorderColor = System.Drawing.SystemColors.InactiveCaption
        Me.ugrdZoneList.DisplayLayout.Appearance = Appearance1
        Me.ugrdZoneList.DisplayLayout.AutoFitStyle = Infragistics.Win.UltraWinGrid.AutoFitStyle.ResizeAllColumns
        Me.ugrdZoneList.DisplayLayout.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Appearance2.FontData.BoldAsString = "True"
        Me.ugrdZoneList.DisplayLayout.CaptionAppearance = Appearance2
        Me.ugrdZoneList.DisplayLayout.CaptionVisible = Infragistics.Win.DefaultableBoolean.[True]
        Appearance3.BackColor = System.Drawing.SystemColors.ActiveBorder
        Appearance3.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance3.BackGradientStyle = Infragistics.Win.GradientStyle.Vertical
        Appearance3.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdZoneList.DisplayLayout.GroupByBox.Appearance = Appearance3
        Appearance4.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdZoneList.DisplayLayout.GroupByBox.BandLabelAppearance = Appearance4
        Me.ugrdZoneList.DisplayLayout.GroupByBox.BorderStyle = Infragistics.Win.UIElementBorderStyle.Solid
        Me.ugrdZoneList.DisplayLayout.GroupByBox.Hidden = True
        Appearance5.BackColor = System.Drawing.SystemColors.ControlLightLight
        Appearance5.BackColor2 = System.Drawing.SystemColors.Control
        Appearance5.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance5.ForeColor = System.Drawing.SystemColors.GrayText
        Me.ugrdZoneList.DisplayLayout.GroupByBox.PromptAppearance = Appearance5
        Me.ugrdZoneList.DisplayLayout.MaxColScrollRegions = 1
        Me.ugrdZoneList.DisplayLayout.MaxRowScrollRegions = 1
        Appearance6.BackColor = System.Drawing.SystemColors.Window
        Appearance6.ForeColor = System.Drawing.SystemColors.ControlText
        Me.ugrdZoneList.DisplayLayout.Override.ActiveCellAppearance = Appearance6
        Me.ugrdZoneList.DisplayLayout.Override.AllowAddNew = Infragistics.Win.UltraWinGrid.AllowAddNew.No
        Me.ugrdZoneList.DisplayLayout.Override.AllowColMoving = Infragistics.Win.UltraWinGrid.AllowColMoving.WithinGroup
        Me.ugrdZoneList.DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.[False]
        Me.ugrdZoneList.DisplayLayout.Override.AllowRowSummaries = Infragistics.Win.UltraWinGrid.AllowRowSummaries.[False]
        Me.ugrdZoneList.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.[True]
        Me.ugrdZoneList.DisplayLayout.Override.BorderStyleCell = Infragistics.Win.UIElementBorderStyle.Dotted
        Me.ugrdZoneList.DisplayLayout.Override.BorderStyleRow = Infragistics.Win.UIElementBorderStyle.Dotted
        Appearance7.BackColor = System.Drawing.SystemColors.Window
        Me.ugrdZoneList.DisplayLayout.Override.CardAreaAppearance = Appearance7
        Appearance8.BorderColor = System.Drawing.Color.Silver
        Appearance8.FontData.BoldAsString = "True"
        Appearance8.TextTrimming = Infragistics.Win.TextTrimming.EllipsisCharacter
        Me.ugrdZoneList.DisplayLayout.Override.CellAppearance = Appearance8
        Me.ugrdZoneList.DisplayLayout.Override.CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.EditAndSelectText
        Me.ugrdZoneList.DisplayLayout.Override.CellPadding = 0
        Appearance9.FontData.BoldAsString = "True"
        Me.ugrdZoneList.DisplayLayout.Override.FixedHeaderAppearance = Appearance9
        Appearance10.BackColor = System.Drawing.SystemColors.Control
        Appearance10.BackColor2 = System.Drawing.SystemColors.ControlDark
        Appearance10.BackGradientAlignment = Infragistics.Win.GradientAlignment.Element
        Appearance10.BackGradientStyle = Infragistics.Win.GradientStyle.Horizontal
        Appearance10.BorderColor = System.Drawing.SystemColors.Window
        Me.ugrdZoneList.DisplayLayout.Override.GroupByRowAppearance = Appearance10
        Appearance11.FontData.BoldAsString = "True"
        Appearance11.TextHAlignAsString = "Left"
        Me.ugrdZoneList.DisplayLayout.Override.HeaderAppearance = Appearance11
        Me.ugrdZoneList.DisplayLayout.Override.HeaderStyle = Infragistics.Win.HeaderStyle.WindowsXPCommand
        Appearance12.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdZoneList.DisplayLayout.Override.RowAlternateAppearance = Appearance12
        Appearance13.BackColor = System.Drawing.SystemColors.Window
        Appearance13.BorderColor = System.Drawing.Color.Silver
        Me.ugrdZoneList.DisplayLayout.Override.RowAppearance = Appearance13
        Me.ugrdZoneList.DisplayLayout.Override.RowSizing = Infragistics.Win.UltraWinGrid.RowSizing.Fixed
        Me.ugrdZoneList.DisplayLayout.Override.SelectTypeCell = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Me.ugrdZoneList.DisplayLayout.Override.SelectTypeCol = Infragistics.Win.UltraWinGrid.SelectType.None
        Me.ugrdZoneList.DisplayLayout.Override.SelectTypeRow = Infragistics.Win.UltraWinGrid.SelectType.[Single]
        Appearance14.BackColor = System.Drawing.SystemColors.ControlLight
        Me.ugrdZoneList.DisplayLayout.Override.TemplateAddRowAppearance = Appearance14
        Me.ugrdZoneList.DisplayLayout.ScrollBounds = Infragistics.Win.UltraWinGrid.ScrollBounds.ScrollToFill
        Me.ugrdZoneList.DisplayLayout.ScrollStyle = Infragistics.Win.UltraWinGrid.ScrollStyle.Immediate
        Me.ugrdZoneList.DisplayLayout.ViewStyle = Infragistics.Win.UltraWinGrid.ViewStyle.SingleBand
        Me.ugrdZoneList.DisplayLayout.ViewStyleBand = Infragistics.Win.UltraWinGrid.ViewStyleBand.OutlookGroupBy
        Me.ugrdZoneList.Font = New System.Drawing.Font("Arial", 8.0!)
        Me.ugrdZoneList.Location = New System.Drawing.Point(3, 19)
        Me.ugrdZoneList.Name = "ugrdZoneList"
        Me.ugrdZoneList.Size = New System.Drawing.Size(398, 263)
        Me.ugrdZoneList.TabIndex = 102
        Me.ugrdZoneList.Text = "Manage Zones"
        '
        'btnDelete
        '
        Me.btnDelete.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnDelete.Location = New System.Drawing.Point(407, 48)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(75, 23)
        Me.btnDelete.TabIndex = 4
        Me.btnDelete.Text = "Delete"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnAdd
        '
        Me.btnAdd.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnAdd.Location = New System.Drawing.Point(407, 19)
        Me.btnAdd.Name = "btnAdd"
        Me.btnAdd.Size = New System.Drawing.Size(75, 23)
        Me.btnAdd.TabIndex = 2
        Me.btnAdd.Text = "Add"
        Me.btnAdd.UseVisualStyleBackColor = True
        '
        'ZoneAdmin
        '
        Me.AcceptButton = Me.btnOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(504, 354)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnOK)
        Me.Controls.Add(Me.TaxJurisdictionGroupBox)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "ZoneAdmin"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "View Zones"
        Me.TaxJurisdictionGroupBox.ResumeLayout(False)
        CType(Me.ugrdZoneList, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents btnCancel As System.Windows.Forms.Button
    Friend WithEvents btnOK As System.Windows.Forms.Button
    Friend WithEvents TaxJurisdictionGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents ugrdZoneList As Infragistics.Win.UltraWinGrid.UltraGrid
    Friend WithEvents btnDelete As System.Windows.Forms.Button
    Friend WithEvents btnAdd As System.Windows.Forms.Button

End Class
