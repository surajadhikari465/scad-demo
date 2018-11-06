Namespace WholeFoods.IRMA.Reporting.UserInterface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
    Partial Class frmCompetitiorTrend
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmCompetitiorTrend))
            Me.isSelectStores = New WholeFoods.IRMA.ItemChaining.UserInterface.ItemSelectingControl
            Me.gbWFMStores = New System.Windows.Forms.GroupBox
            Me.gbCompetitorStores = New System.Windows.Forms.GroupBox
            Me.lsCompetitorStores = New WholeFoods.IRMA.CompetitorStore.UserInterface.CompetitorStoreListSelect
            Me.btnViewReport = New System.Windows.Forms.Button
            Me.btnExit = New System.Windows.Forms.Button
            Me.cmbCompetitivePriceType = New System.Windows.Forms.ComboBox
            Me.lblCompetitivePriceType = New System.Windows.Forms.Label
            Me.rbSale = New System.Windows.Forms.RadioButton
            Me.rbRegular = New System.Windows.Forms.RadioButton
            Me.lblPriceType = New System.Windows.Forms.Label
            Me.cmbEndFiscalWeek = New System.Windows.Forms.ComboBox
            Me.cmbStartFiscalWeek = New System.Windows.Forms.ComboBox
            Me.lblEndFiscalWeek = New System.Windows.Forms.Label
            Me.lblStartFiscalWeek = New System.Windows.Forms.Label
            Me.gbPrice = New System.Windows.Forms.GroupBox
            Me.btnItemSearch = New System.Windows.Forms.Button
            Me.lblItemDescription = New System.Windows.Forms.Label
            Me.lblItem = New System.Windows.Forms.Label
            Me.pnlSearch = New System.Windows.Forms.Panel
            Me.gbWFMStores.SuspendLayout()
            Me.gbCompetitorStores.SuspendLayout()
            Me.gbPrice.SuspendLayout()
            Me.pnlSearch.SuspendLayout()
            Me.SuspendLayout()
            '
            'isSelectStores
            '
            Me.isSelectStores.BackColor = System.Drawing.Color.Transparent
            Me.isSelectStores.Field_ID = "Store_No"
            Me.isSelectStores.Field_Image = ""
            Me.isSelectStores.Field_Text = "Store_Name"
            Me.isSelectStores.Field_Text2 = ""
            Me.isSelectStores.Icon = WholeFoods.IRMA.ItemChaining.UserInterface.ItemSelectingControl.IconType.[Boolean]
            Me.isSelectStores.ItemText = "Store"
            Me.isSelectStores.ListHeight = 180
            Me.isSelectStores.Location = New System.Drawing.Point(88, 19)
            Me.isSelectStores.Name = "isSelectStores"
            Me.isSelectStores.ShowClearButton = False
            Me.isSelectStores.ShowExportButton = False
            Me.isSelectStores.Size = New System.Drawing.Size(485, 233)
            Me.isSelectStores.TabIndex = 31
            '
            'gbWFMStores
            '
            Me.gbWFMStores.BackColor = System.Drawing.Color.Transparent
            Me.gbWFMStores.Controls.Add(Me.isSelectStores)
            Me.gbWFMStores.Location = New System.Drawing.Point(10, 11)
            Me.gbWFMStores.Name = "gbWFMStores"
            Me.gbWFMStores.Size = New System.Drawing.Size(712, 235)
            Me.gbWFMStores.TabIndex = 32
            Me.gbWFMStores.TabStop = False
            Me.gbWFMStores.Text = "WFM Stores"
            '
            'gbCompetitorStores
            '
            Me.gbCompetitorStores.BackColor = System.Drawing.Color.Transparent
            Me.gbCompetitorStores.Controls.Add(Me.lsCompetitorStores)
            Me.gbCompetitorStores.Location = New System.Drawing.Point(10, 252)
            Me.gbCompetitorStores.Name = "gbCompetitorStores"
            Me.gbCompetitorStores.Size = New System.Drawing.Size(712, 263)
            Me.gbCompetitorStores.TabIndex = 33
            Me.gbCompetitorStores.TabStop = False
            Me.gbCompetitorStores.Text = "Competitor Stores"
            '
            'lsCompetitorStores
            '
            Me.lsCompetitorStores.BackColor = System.Drawing.Color.Transparent
            Me.lsCompetitorStores.Location = New System.Drawing.Point(7, 20)
            Me.lsCompetitorStores.MinimumSize = New System.Drawing.Size(700, 245)
            Me.lsCompetitorStores.Name = "lsCompetitorStores"
            Me.lsCompetitorStores.ShowFilter = True
            Me.lsCompetitorStores.Size = New System.Drawing.Size(700, 245)
            Me.lsCompetitorStores.TabIndex = 0
            '
            'btnViewReport
            '
            Me.btnViewReport.Location = New System.Drawing.Point(624, 651)
            Me.btnViewReport.Name = "btnViewReport"
            Me.btnViewReport.Size = New System.Drawing.Size(100, 23)
            Me.btnViewReport.TabIndex = 34
            Me.btnViewReport.Text = "View Report"
            Me.btnViewReport.UseVisualStyleBackColor = True
            '
            'btnExit
            '
            Me.btnExit.CausesValidation = False
            Me.btnExit.Location = New System.Drawing.Point(12, 651)
            Me.btnExit.Name = "btnExit"
            Me.btnExit.Size = New System.Drawing.Size(100, 23)
            Me.btnExit.TabIndex = 35
            Me.btnExit.Text = "Exit"
            Me.btnExit.UseVisualStyleBackColor = True
            '
            'cmbCompetitivePriceType
            '
            Me.cmbCompetitivePriceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbCompetitivePriceType.FormattingEnabled = True
            Me.cmbCompetitivePriceType.Location = New System.Drawing.Point(422, 13)
            Me.cmbCompetitivePriceType.Name = "cmbCompetitivePriceType"
            Me.cmbCompetitivePriceType.Size = New System.Drawing.Size(150, 21)
            Me.cmbCompetitivePriceType.TabIndex = 46
            '
            'lblCompetitivePriceType
            '
            Me.lblCompetitivePriceType.AutoSize = True
            Me.lblCompetitivePriceType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblCompetitivePriceType.Location = New System.Drawing.Point(275, 16)
            Me.lblCompetitivePriceType.Name = "lblCompetitivePriceType"
            Me.lblCompetitivePriceType.Size = New System.Drawing.Size(141, 14)
            Me.lblCompetitivePriceType.TabIndex = 45
            Me.lblCompetitivePriceType.Text = "Competitive Price Type :"
            Me.lblCompetitivePriceType.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'rbSale
            '
            Me.rbSale.AutoSize = True
            Me.rbSale.Location = New System.Drawing.Point(496, 41)
            Me.rbSale.Name = "rbSale"
            Me.rbSale.Size = New System.Drawing.Size(46, 17)
            Me.rbSale.TabIndex = 44
            Me.rbSale.Text = "Sale"
            Me.rbSale.UseVisualStyleBackColor = True
            '
            'rbRegular
            '
            Me.rbRegular.AutoSize = True
            Me.rbRegular.Checked = True
            Me.rbRegular.Location = New System.Drawing.Point(422, 41)
            Me.rbRegular.Name = "rbRegular"
            Me.rbRegular.Size = New System.Drawing.Size(62, 17)
            Me.rbRegular.TabIndex = 43
            Me.rbRegular.TabStop = True
            Me.rbRegular.Text = "Regular"
            Me.rbRegular.UseVisualStyleBackColor = True
            '
            'lblPriceType
            '
            Me.lblPriceType.AutoSize = True
            Me.lblPriceType.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPriceType.Location = New System.Drawing.Point(345, 43)
            Me.lblPriceType.Name = "lblPriceType"
            Me.lblPriceType.Size = New System.Drawing.Size(71, 14)
            Me.lblPriceType.TabIndex = 42
            Me.lblPriceType.Text = "Price Type :"
            Me.lblPriceType.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'cmbEndFiscalWeek
            '
            Me.cmbEndFiscalWeek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbEndFiscalWeek.FormattingEnabled = True
            Me.cmbEndFiscalWeek.Location = New System.Drawing.Point(119, 40)
            Me.cmbEndFiscalWeek.Name = "cmbEndFiscalWeek"
            Me.cmbEndFiscalWeek.Size = New System.Drawing.Size(150, 21)
            Me.cmbEndFiscalWeek.TabIndex = 41
            '
            'cmbStartFiscalWeek
            '
            Me.cmbStartFiscalWeek.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbStartFiscalWeek.FormattingEnabled = True
            Me.cmbStartFiscalWeek.Location = New System.Drawing.Point(119, 13)
            Me.cmbStartFiscalWeek.Name = "cmbStartFiscalWeek"
            Me.cmbStartFiscalWeek.Size = New System.Drawing.Size(150, 21)
            Me.cmbStartFiscalWeek.TabIndex = 40
            '
            'lblEndFiscalWeek
            '
            Me.lblEndFiscalWeek.AutoSize = True
            Me.lblEndFiscalWeek.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblEndFiscalWeek.Location = New System.Drawing.Point(12, 43)
            Me.lblEndFiscalWeek.Name = "lblEndFiscalWeek"
            Me.lblEndFiscalWeek.Size = New System.Drawing.Size(101, 14)
            Me.lblEndFiscalWeek.TabIndex = 39
            Me.lblEndFiscalWeek.Text = "End Fiscal Week :"
            Me.lblEndFiscalWeek.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'lblStartFiscalWeek
            '
            Me.lblStartFiscalWeek.AutoSize = True
            Me.lblStartFiscalWeek.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblStartFiscalWeek.Location = New System.Drawing.Point(6, 16)
            Me.lblStartFiscalWeek.Name = "lblStartFiscalWeek"
            Me.lblStartFiscalWeek.Size = New System.Drawing.Size(107, 14)
            Me.lblStartFiscalWeek.TabIndex = 38
            Me.lblStartFiscalWeek.Text = "Start Fiscal Week :"
            Me.lblStartFiscalWeek.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'gbPrice
            '
            Me.gbPrice.BackColor = System.Drawing.Color.Transparent
            Me.gbPrice.Controls.Add(Me.btnItemSearch)
            Me.gbPrice.Controls.Add(Me.lblItemDescription)
            Me.gbPrice.Controls.Add(Me.lblItem)
            Me.gbPrice.Controls.Add(Me.lblStartFiscalWeek)
            Me.gbPrice.Controls.Add(Me.lblEndFiscalWeek)
            Me.gbPrice.Controls.Add(Me.cmbCompetitivePriceType)
            Me.gbPrice.Controls.Add(Me.cmbStartFiscalWeek)
            Me.gbPrice.Controls.Add(Me.cmbEndFiscalWeek)
            Me.gbPrice.Controls.Add(Me.lblCompetitivePriceType)
            Me.gbPrice.Controls.Add(Me.lblPriceType)
            Me.gbPrice.Controls.Add(Me.rbSale)
            Me.gbPrice.Controls.Add(Me.rbRegular)
            Me.gbPrice.Location = New System.Drawing.Point(10, 524)
            Me.gbPrice.Name = "gbPrice"
            Me.gbPrice.Size = New System.Drawing.Size(712, 120)
            Me.gbPrice.TabIndex = 47
            Me.gbPrice.TabStop = False
            Me.gbPrice.Text = "Price"
            '
            'btnItemSearch
            '
            Me.btnItemSearch.CausesValidation = False
            Me.btnItemSearch.Location = New System.Drawing.Point(119, 85)
            Me.btnItemSearch.Name = "btnItemSearch"
            Me.btnItemSearch.Size = New System.Drawing.Size(100, 23)
            Me.btnItemSearch.TabIndex = 49
            Me.btnItemSearch.Text = "Item Search"
            Me.btnItemSearch.UseVisualStyleBackColor = True
            '
            'lblItemDescription
            '
            Me.lblItemDescription.AutoSize = True
            Me.lblItemDescription.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblItemDescription.Location = New System.Drawing.Point(119, 68)
            Me.lblItemDescription.Name = "lblItemDescription"
            Me.lblItemDescription.Size = New System.Drawing.Size(95, 14)
            Me.lblItemDescription.TabIndex = 48
            Me.lblItemDescription.Text = "< None Selected >"
            '
            'lblItem
            '
            Me.lblItem.AutoSize = True
            Me.lblItem.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblItem.Location = New System.Drawing.Point(75, 68)
            Me.lblItem.Name = "lblItem"
            Me.lblItem.Size = New System.Drawing.Size(38, 14)
            Me.lblItem.TabIndex = 47
            Me.lblItem.Text = "Item :"
            Me.lblItem.TextAlign = System.Drawing.ContentAlignment.TopRight
            '
            'pnlSearch
            '
            Me.pnlSearch.BackColor = System.Drawing.Color.Transparent
            Me.pnlSearch.Controls.Add(Me.gbWFMStores)
            Me.pnlSearch.Controls.Add(Me.gbPrice)
            Me.pnlSearch.Controls.Add(Me.gbCompetitorStores)
            Me.pnlSearch.Location = New System.Drawing.Point(2, 1)
            Me.pnlSearch.Name = "pnlSearch"
            Me.pnlSearch.Size = New System.Drawing.Size(731, 644)
            Me.pnlSearch.TabIndex = 48
            '
            'frmCompetitiorTrend
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(735, 678)
            Me.Controls.Add(Me.pnlSearch)
            Me.Controls.Add(Me.btnExit)
            Me.Controls.Add(Me.btnViewReport)
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmCompetitiorTrend"
            Me.ShowIcon = False
            Me.ShowInTaskbar = False
            Me.Text = "Competitior Trend"
            Me.gbWFMStores.ResumeLayout(False)
            Me.gbCompetitorStores.ResumeLayout(False)
            Me.gbPrice.ResumeLayout(False)
            Me.gbPrice.PerformLayout()
            Me.pnlSearch.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents isSelectStores As WholeFoods.IRMA.ItemChaining.UserInterface.ItemSelectingControl
        Friend WithEvents gbWFMStores As System.Windows.Forms.GroupBox
        Friend WithEvents gbCompetitorStores As System.Windows.Forms.GroupBox
        Friend WithEvents btnViewReport As System.Windows.Forms.Button
        Friend WithEvents lsCompetitorStores As WholeFoods.IRMA.CompetitorStore.UserInterface.CompetitorStoreListSelect
        Friend WithEvents btnExit As System.Windows.Forms.Button
        Friend WithEvents cmbCompetitivePriceType As System.Windows.Forms.ComboBox
        Friend WithEvents lblCompetitivePriceType As System.Windows.Forms.Label
        Friend WithEvents rbSale As System.Windows.Forms.RadioButton
        Friend WithEvents rbRegular As System.Windows.Forms.RadioButton
        Friend WithEvents lblPriceType As System.Windows.Forms.Label
        Friend WithEvents cmbEndFiscalWeek As System.Windows.Forms.ComboBox
        Friend WithEvents cmbStartFiscalWeek As System.Windows.Forms.ComboBox
        Friend WithEvents lblEndFiscalWeek As System.Windows.Forms.Label
        Friend WithEvents lblStartFiscalWeek As System.Windows.Forms.Label
        Friend WithEvents gbPrice As System.Windows.Forms.GroupBox
        Friend WithEvents btnItemSearch As System.Windows.Forms.Button
        Friend WithEvents lblItemDescription As System.Windows.Forms.Label
        Friend WithEvents lblItem As System.Windows.Forms.Label
        Friend WithEvents pnlSearch As System.Windows.Forms.Panel
        'Friend WithEvents ucCompetitorTrend As Infragistics.Win.UltraWinChart.UltraChart
    End Class
End Namespace