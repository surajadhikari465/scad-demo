Namespace WholeFoods.IRMA.Reporting.UserInterface

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmCompetitorMarginImpact
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
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
            Me.gbStoreFilter = New System.Windows.Forms.GroupBox
            Me.lsCompetitorStore = New WholeFoods.IRMA.CompetitorStore.UserInterface.CompetitorStoreListSelect
            Me.cmbFiscalWeek = New System.Windows.Forms.ComboBox
            Me.lblFiscalWeek = New System.Windows.Forms.Label
            Me.lblWFMStore = New System.Windows.Forms.Label
            Me.cmbWFMStore = New System.Windows.Forms.ComboBox
            Me.gbItemFilter = New System.Windows.Forms.GroupBox
            Me.cmbCompetitivePriceType = New System.Windows.Forms.ComboBox
            Me.lblCompetitivePriceType = New System.Windows.Forms.Label
            Me.iscItemSearch = New WholeFoods.IRMA.ItemChaining.UserInterface.ItemSearchControl
            Me.btnViewReport = New System.Windows.Forms.Button
            Me.btnExit = New System.Windows.Forms.Button
            Me.gbStoreFilter.SuspendLayout()
            Me.gbItemFilter.SuspendLayout()
            Me.SuspendLayout()
            '
            'gbStoreFilter
            '
            Me.gbStoreFilter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.gbStoreFilter.BackColor = System.Drawing.Color.Transparent
            Me.gbStoreFilter.Controls.Add(Me.lsCompetitorStore)
            Me.gbStoreFilter.Controls.Add(Me.cmbFiscalWeek)
            Me.gbStoreFilter.Controls.Add(Me.lblFiscalWeek)
            Me.gbStoreFilter.Controls.Add(Me.lblWFMStore)
            Me.gbStoreFilter.Controls.Add(Me.cmbWFMStore)
            Me.gbStoreFilter.Location = New System.Drawing.Point(13, 13)
            Me.gbStoreFilter.Name = "gbStoreFilter"
            Me.gbStoreFilter.Size = New System.Drawing.Size(717, 270)
            Me.gbStoreFilter.TabIndex = 0
            Me.gbStoreFilter.TabStop = False
            Me.gbStoreFilter.Text = "Store Filter"
            '
            'lsCompetitorStore
            '
            Me.lsCompetitorStore.BackColor = System.Drawing.Color.Transparent
            Me.lsCompetitorStore.Location = New System.Drawing.Point(10, 38)
            Me.lsCompetitorStore.MinimumSize = New System.Drawing.Size(700, 245)
            Me.lsCompetitorStore.Name = "lsCompetitorStore"
            Me.lsCompetitorStore.ShowFilter = True
            Me.lsCompetitorStore.Size = New System.Drawing.Size(700, 245)
            Me.lsCompetitorStore.TabIndex = 4
            '
            'cmbFiscalWeek
            '
            Me.cmbFiscalWeek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbFiscalWeek.FormattingEnabled = True
            Me.cmbFiscalWeek.Location = New System.Drawing.Point(429, 16)
            Me.cmbFiscalWeek.Name = "cmbFiscalWeek"
            Me.cmbFiscalWeek.Size = New System.Drawing.Size(150, 21)
            Me.cmbFiscalWeek.TabIndex = 3
            '
            'lblFiscalWeek
            '
            Me.lblFiscalWeek.AutoSize = True
            Me.lblFiscalWeek.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblFiscalWeek.Location = New System.Drawing.Point(344, 20)
            Me.lblFiscalWeek.Name = "lblFiscalWeek"
            Me.lblFiscalWeek.Size = New System.Drawing.Size(78, 14)
            Me.lblFiscalWeek.TabIndex = 2
            Me.lblFiscalWeek.Text = "Fiscal Week :"
            '
            'lblWFMStore
            '
            Me.lblWFMStore.AutoSize = True
            Me.lblWFMStore.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblWFMStore.Location = New System.Drawing.Point(7, 20)
            Me.lblWFMStore.Name = "lblWFMStore"
            Me.lblWFMStore.Size = New System.Drawing.Size(72, 14)
            Me.lblWFMStore.TabIndex = 1
            Me.lblWFMStore.Text = "WFM Store :"
            '
            'cmbWFMStore
            '
            Me.cmbWFMStore.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbWFMStore.FormattingEnabled = True
            Me.cmbWFMStore.Location = New System.Drawing.Point(85, 17)
            Me.cmbWFMStore.Name = "cmbWFMStore"
            Me.cmbWFMStore.Size = New System.Drawing.Size(150, 21)
            Me.cmbWFMStore.TabIndex = 0
            '
            'gbItemFilter
            '
            Me.gbItemFilter.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                        Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.gbItemFilter.BackColor = System.Drawing.Color.Transparent
            Me.gbItemFilter.Controls.Add(Me.cmbCompetitivePriceType)
            Me.gbItemFilter.Controls.Add(Me.lblCompetitivePriceType)
            Me.gbItemFilter.Controls.Add(Me.iscItemSearch)
            Me.gbItemFilter.Location = New System.Drawing.Point(13, 289)
            Me.gbItemFilter.Name = "gbItemFilter"
            Me.gbItemFilter.Size = New System.Drawing.Size(717, 230)
            Me.gbItemFilter.TabIndex = 1
            Me.gbItemFilter.TabStop = False
            Me.gbItemFilter.Text = "Item Filter"
            '
            'cmbCompetitivePriceType
            '
            Me.cmbCompetitivePriceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbCompetitivePriceType.FormattingEnabled = True
            Me.cmbCompetitivePriceType.Location = New System.Drawing.Point(330, 157)
            Me.cmbCompetitivePriceType.Name = "cmbCompetitivePriceType"
            Me.cmbCompetitivePriceType.Size = New System.Drawing.Size(150, 21)
            Me.cmbCompetitivePriceType.TabIndex = 2
            '
            'lblCompetitivePriceType
            '
            Me.lblCompetitivePriceType.AutoSize = True
            Me.lblCompetitivePriceType.Font = New System.Drawing.Font("Arial", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblCompetitivePriceType.Location = New System.Drawing.Point(216, 159)
            Me.lblCompetitivePriceType.Name = "lblCompetitivePriceType"
            Me.lblCompetitivePriceType.Size = New System.Drawing.Size(110, 14)
            Me.lblCompetitivePriceType.TabIndex = 1
            Me.lblCompetitivePriceType.Text = "Comp. Price Type :"
            '
            'iscItemSearch
            '
            Me.iscItemSearch.BackColor = System.Drawing.Color.Transparent
            Me.iscItemSearch.Location = New System.Drawing.Point(10, 20)
            Me.iscItemSearch.Name = "iscItemSearch"
            Me.iscItemSearch.ShowClearButton = False
            Me.iscItemSearch.ShowHFM = False
            Me.iscItemSearch.ShowItemCheckBoxes = True
            Me.iscItemSearch.ShowSearchButton = False
            Me.iscItemSearch.ShowWFM = False
            Me.iscItemSearch.Size = New System.Drawing.Size(485, 204)
            Me.iscItemSearch.TabIndex = 0
            '
            'btnViewReport
            '
            Me.btnViewReport.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnViewReport.Location = New System.Drawing.Point(630, 531)
            Me.btnViewReport.Name = "btnViewReport"
            Me.btnViewReport.Size = New System.Drawing.Size(100, 23)
            Me.btnViewReport.TabIndex = 2
            Me.btnViewReport.Text = "View Report"
            Me.btnViewReport.UseVisualStyleBackColor = True
            '
            'btnExit
            '
            Me.btnExit.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
            Me.btnExit.Location = New System.Drawing.Point(12, 531)
            Me.btnExit.Name = "btnExit"
            Me.btnExit.Size = New System.Drawing.Size(100, 23)
            Me.btnExit.TabIndex = 3
            Me.btnExit.Text = "Exit"
            Me.btnExit.UseVisualStyleBackColor = True
            '
            'frmCompetitorMarginImpact
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(742, 566)
            Me.Controls.Add(Me.btnExit)
            Me.Controls.Add(Me.btnViewReport)
            Me.Controls.Add(Me.gbItemFilter)
            Me.Controls.Add(Me.gbStoreFilter)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmCompetitorMarginImpact"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.Text = "Competitor Margin Impact"
            Me.gbStoreFilter.ResumeLayout(False)
            Me.gbStoreFilter.PerformLayout()
            Me.gbItemFilter.ResumeLayout(False)
            Me.gbItemFilter.PerformLayout()
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents gbStoreFilter As System.Windows.Forms.GroupBox
        Friend WithEvents gbItemFilter As System.Windows.Forms.GroupBox
        Friend WithEvents btnViewReport As System.Windows.Forms.Button
        Friend WithEvents btnExit As System.Windows.Forms.Button
        Friend WithEvents cmbFiscalWeek As System.Windows.Forms.ComboBox
        Friend WithEvents lblFiscalWeek As System.Windows.Forms.Label
        Friend WithEvents lblWFMStore As System.Windows.Forms.Label
        Friend WithEvents cmbWFMStore As System.Windows.Forms.ComboBox
        Friend WithEvents iscItemSearch As WholeFoods.IRMA.ItemChaining.UserInterface.ItemSearchControl
        Friend WithEvents cmbCompetitivePriceType As System.Windows.Forms.ComboBox
        Friend WithEvents lblCompetitivePriceType As System.Windows.Forms.Label
        Friend WithEvents lsCompetitorStore As WholeFoods.IRMA.CompetitorStore.UserInterface.CompetitorStoreListSelect
    End Class

End Namespace