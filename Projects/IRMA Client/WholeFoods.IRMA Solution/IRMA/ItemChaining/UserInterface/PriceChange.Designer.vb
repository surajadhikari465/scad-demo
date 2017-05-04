Namespace WholeFoods.IRMA.ItemChaining.UserInterface
    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
        Partial Class frmPriceChange2
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
            Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmPriceChange2))
            Dim ListViewGroup1 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("ANNA BANANA", System.Windows.Forms.HorizontalAlignment.Left)
            Dim ListViewGroup2 As System.Windows.Forms.ListViewGroup = New System.Windows.Forms.ListViewGroup("JALALA BEANS", System.Windows.Forms.HorizontalAlignment.Left)
            Dim ListViewItem1 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem(New String() {"Boulder", "15.00", "17.00", "10%", "12%"}, -1)
            Dim ListViewItem2 As System.Windows.Forms.ListViewItem = New System.Windows.Forms.ListViewItem("Test Store")
            Me.pnlHeader = New System.Windows.Forms.Panel
            Me.lblHeader = New System.Windows.Forms.Label
            Me.lblTitle = New System.Windows.Forms.Label
            Me.picHEader = New System.Windows.Forms.PictureBox
            Me.pnlWorkArea = New System.Windows.Forms.Panel
            Me.TabControl1 = New System.Windows.Forms.TabControl
            Me.wpSearchItems = New System.Windows.Forms.TabPage
            Me.lblWait = New System.Windows.Forms.Label
            Me.ItemSearchControl1 = New WholeFoods.IRMA.ItemChaining.UserInterface.ItemSearchControl
            Me.wpSelectItems = New System.Windows.Forms.TabPage
            Me.lblMessage = New System.Windows.Forms.Label
            Me.ItemSelectingControl1 = New WholeFoods.IRMA.ItemChaining.UserInterface.ItemSelectingControl
            Me.wpSelectStores = New System.Windows.Forms.TabPage
            Me.cmbZone = New System.Windows.Forms.ComboBox
            Me.Label10 = New System.Windows.Forms.Label
            Me.cmbState = New System.Windows.Forms.ComboBox
            Me.Label11 = New System.Windows.Forms.Label
            Me.Label9 = New System.Windows.Forms.Label
            Me.isSelectStores = New WholeFoods.IRMA.ItemChaining.UserInterface.ItemSelectingControl
            Me.wpPriceType = New System.Windows.Forms.TabPage
            Me.lstPriceType = New System.Windows.Forms.ListView
            Me.PriceType = New System.Windows.Forms.ColumnHeader
            Me.wpReg = New System.Windows.Forms.TabPage
            Me.txtChangePOSpricetoPrice = New System.Windows.Forms.TextBox
            Me.Label12 = New System.Windows.Forms.Label
            Me.mclReg = New System.Windows.Forms.MonthCalendar
            Me.radChangePOSpriceby = New System.Windows.Forms.RadioButton
            Me.radChangePOSpriceto = New System.Windows.Forms.RadioButton
            Me.Label18 = New System.Windows.Forms.Label
            Me.txtChangePOSpricebyPercent = New System.Windows.Forms.TextBox
            Me.Label20 = New System.Windows.Forms.Label
            Me.txtChangePOSpricetoQty = New System.Windows.Forms.TextBox
            Me.wpPromo = New System.Windows.Forms.TabPage
            Me.Label6 = New System.Windows.Forms.Label
            Me.cmbPromoType = New System.Windows.Forms.ComboBox
            Me.mclPromoEnd = New System.Windows.Forms.MonthCalendar
            Me.mclPromoStart = New System.Windows.Forms.MonthCalendar
            Me.radChangePOSpromopricebyREG = New System.Windows.Forms.RadioButton
            Me.Label28 = New System.Windows.Forms.Label
            Me.txtChangePOSpromopricebyREG = New System.Windows.Forms.TextBox
            Me.Label23 = New System.Windows.Forms.Label
            Me.txtChangePOSpromopricetoPriceMSRP = New System.Windows.Forms.TextBox
            Me.Label22 = New System.Windows.Forms.Label
            Me.txtChangePOSpromopricetoQtyMSRP = New System.Windows.Forms.TextBox
            Me.radChangePOSpromopricebyMSRP = New System.Windows.Forms.RadioButton
            Me.radChangePOSpromopriceto = New System.Windows.Forms.RadioButton
            Me.Label14 = New System.Windows.Forms.Label
            Me.txtChangePOSpromopricebyMSRP = New System.Windows.Forms.TextBox
            Me.txtChangePOSpromopricetoPrice = New System.Windows.Forms.TextBox
            Me.Label21 = New System.Windows.Forms.Label
            Me.txtChangePOSpromopricetoQty = New System.Windows.Forms.TextBox
            Me.Label13 = New System.Windows.Forms.Label
            Me.Label19 = New System.Windows.Forms.Label
            Me.wpEarnedDiscount = New System.Windows.Forms.TabPage
            Me.TextBox11 = New System.Windows.Forms.TextBox
            Me.TextBox10 = New System.Windows.Forms.TextBox
            Me.TextBox9 = New System.Windows.Forms.TextBox
            Me.Label26 = New System.Windows.Forms.Label
            Me.Label25 = New System.Windows.Forms.Label
            Me.Label24 = New System.Windows.Forms.Label
            Me.wpPreview = New System.Windows.Forms.TabPage
            Me.lstPreview = New System.Windows.Forms.ListView
            Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
            Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
            Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
            Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
            Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
            Me.wpFinal = New System.Windows.Forms.TabPage
            Me.grpWarning = New System.Windows.Forms.GroupBox
            Me.PictureBox2 = New System.Windows.Forms.PictureBox
            Me.lblWarning = New System.Windows.Forms.Label
            Me.lblValidating = New System.Windows.Forms.Label
            Me.btnPreview = New System.Windows.Forms.Button
            Me.lstSelectedStores = New System.Windows.Forms.ListView
            Me.SelectedItem = New System.Windows.Forms.ColumnHeader
            Me.lstSelectedItems = New System.Windows.Forms.ListView
            Me.FoundItem = New System.Windows.Forms.ColumnHeader
            Me.chkIAggree = New System.Windows.Forms.CheckBox
            Me.lblFinal = New System.Windows.Forms.Label
            Me.Label16 = New System.Windows.Forms.Label
            Me.Label15 = New System.Windows.Forms.Label
            Me.pnlButtons = New System.Windows.Forms.Panel
            Me.btnCancel = New System.Windows.Forms.Button
            Me.btnBack = New System.Windows.Forms.Button
            Me.btnNext = New System.Windows.Forms.Button
            Me.btnFinish = New System.Windows.Forms.Button
            Me.pnlHeader.SuspendLayout()
            CType(Me.picHEader, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlWorkArea.SuspendLayout()
            Me.TabControl1.SuspendLayout()
            Me.wpSearchItems.SuspendLayout()
            Me.wpSelectItems.SuspendLayout()
            Me.wpSelectStores.SuspendLayout()
            Me.wpPriceType.SuspendLayout()
            Me.wpReg.SuspendLayout()
            Me.wpPromo.SuspendLayout()
            Me.wpEarnedDiscount.SuspendLayout()
            Me.wpPreview.SuspendLayout()
            Me.wpFinal.SuspendLayout()
            Me.grpWarning.SuspendLayout()
            CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlButtons.SuspendLayout()
            Me.SuspendLayout()
            '
            'pnlHeader
            '
            Me.pnlHeader.BackColor = System.Drawing.Color.Khaki
            Me.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlHeader.Controls.Add(Me.lblHeader)
            Me.pnlHeader.Controls.Add(Me.lblTitle)
            Me.pnlHeader.Controls.Add(Me.picHEader)
            Me.pnlHeader.Location = New System.Drawing.Point(-2, 0)
            Me.pnlHeader.Name = "pnlHeader"
            Me.pnlHeader.Size = New System.Drawing.Size(521, 63)
            Me.pnlHeader.TabIndex = 2
            '
            'lblHeader
            '
            Me.lblHeader.BackColor = System.Drawing.Color.Transparent
            Me.lblHeader.Location = New System.Drawing.Point(79, 25)
            Me.lblHeader.Name = "lblHeader"
            Me.lblHeader.Size = New System.Drawing.Size(439, 38)
            Me.lblHeader.TabIndex = 4
            Me.lblHeader.Text = "Hello"
            '
            'lblTitle
            '
            Me.lblTitle.AutoSize = True
            Me.lblTitle.BackColor = System.Drawing.Color.Transparent
            Me.lblTitle.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTitle.Location = New System.Drawing.Point(79, 9)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(36, 13)
            Me.lblTitle.TabIndex = 3
            Me.lblTitle.Text = "Hello"
            '
            'picHEader
            '
            Me.picHEader.Image = CType(resources.GetObject("picHEader.Image"), System.Drawing.Image)
            Me.picHEader.Location = New System.Drawing.Point(0, 0)
            Me.picHEader.Name = "picHEader"
            Me.picHEader.Size = New System.Drawing.Size(521, 63)
            Me.picHEader.TabIndex = 2
            Me.picHEader.TabStop = False
            '
            'pnlWorkArea
            '
            Me.pnlWorkArea.BackColor = System.Drawing.Color.Khaki
            Me.pnlWorkArea.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None
            Me.pnlWorkArea.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlWorkArea.Controls.Add(Me.TabControl1)
            Me.pnlWorkArea.Location = New System.Drawing.Point(-2, 34)
            Me.pnlWorkArea.Name = "pnlWorkArea"
            Me.pnlWorkArea.Size = New System.Drawing.Size(518, 317)
            Me.pnlWorkArea.TabIndex = 3
            '
            'TabControl1
            '
            Me.TabControl1.Controls.Add(Me.wpSearchItems)
            Me.TabControl1.Controls.Add(Me.wpSelectItems)
            Me.TabControl1.Controls.Add(Me.wpSelectStores)
            Me.TabControl1.Controls.Add(Me.wpPriceType)
            Me.TabControl1.Controls.Add(Me.wpReg)
            Me.TabControl1.Controls.Add(Me.wpPromo)
            Me.TabControl1.Controls.Add(Me.wpEarnedDiscount)
            Me.TabControl1.Controls.Add(Me.wpPreview)
            Me.TabControl1.Controls.Add(Me.wpFinal)
            Me.TabControl1.Location = New System.Drawing.Point(0, 3)
            Me.TabControl1.Name = "TabControl1"
            Me.TabControl1.SelectedIndex = 0
            Me.TabControl1.Size = New System.Drawing.Size(515, 315)
            Me.TabControl1.TabIndex = 0
            '
            'wpSearchItems
            '
            Me.wpSearchItems.Controls.Add(Me.lblWait)
            Me.wpSearchItems.Controls.Add(Me.ItemSearchControl1)
            Me.wpSearchItems.Location = New System.Drawing.Point(4, 22)
            Me.wpSearchItems.Name = "wpSearchItems"
            Me.wpSearchItems.Padding = New System.Windows.Forms.Padding(3)
            Me.wpSearchItems.Size = New System.Drawing.Size(507, 289)
            Me.wpSearchItems.TabIndex = 2
            Me.wpSearchItems.Tag = "This page allows you to search for products for which you would like to change th" & _
                "e price."
            Me.wpSearchItems.Text = "Search Items to Change Price"
            Me.wpSearchItems.UseVisualStyleBackColor = True
            '
            'lblWait
            '
            Me.lblWait.AutoSize = True
            Me.lblWait.BackColor = System.Drawing.Color.Transparent
            Me.lblWait.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblWait.ForeColor = System.Drawing.SystemColors.ControlDark
            Me.lblWait.Location = New System.Drawing.Point(160, 234)
            Me.lblWait.Name = "lblWait"
            Me.lblWait.Size = New System.Drawing.Size(200, 37)
            Me.lblWait.TabIndex = 3
            Me.lblWait.Text = "Searching..."
            Me.lblWait.Visible = False
            '
            'ItemSearchControl1
            '
            Me.ItemSearchControl1.BackColor = System.Drawing.Color.Transparent
            Me.ItemSearchControl1.Location = New System.Drawing.Point(11, 26)
            Me.ItemSearchControl1.Name = "ItemSearchControl1"
            Me.ItemSearchControl1.ShowClearButton = True
            Me.ItemSearchControl1.ShowHFM = False
            Me.ItemSearchControl1.ShowItemCheckBoxes = False
            Me.ItemSearchControl1.ShowSearchButton = False
            Me.ItemSearchControl1.ShowWFM = False
            Me.ItemSearchControl1.Size = New System.Drawing.Size(485, 205)
            Me.ItemSearchControl1.TabIndex = 2
            '
            'wpSelectItems
            '
            Me.wpSelectItems.Controls.Add(Me.lblMessage)
            Me.wpSelectItems.Controls.Add(Me.ItemSelectingControl1)
            Me.wpSelectItems.Location = New System.Drawing.Point(4, 22)
            Me.wpSelectItems.Name = "wpSelectItems"
            Me.wpSelectItems.Padding = New System.Windows.Forms.Padding(3)
            Me.wpSelectItems.Size = New System.Drawing.Size(507, 289)
            Me.wpSelectItems.TabIndex = 3
            Me.wpSelectItems.Tag = "Select from the list on the left and press the arrows to move the selected items " & _
                "to the list on the right. You can press ""Back"" to modify your search."
            Me.wpSelectItems.Text = "Select Items to Change Price"
            Me.wpSelectItems.UseVisualStyleBackColor = True
            '
            'lblMessage
            '
            Me.lblMessage.AutoSize = True
            Me.lblMessage.BackColor = System.Drawing.Color.Transparent
            Me.lblMessage.Location = New System.Drawing.Point(15, 259)
            Me.lblMessage.Name = "lblMessage"
            Me.lblMessage.Size = New System.Drawing.Size(0, 13)
            Me.lblMessage.TabIndex = 3
            '
            'ItemSelectingControl1
            '
            Me.ItemSelectingControl1.BackColor = System.Drawing.Color.Transparent
            Me.ItemSelectingControl1.Field_ID = "Item_Key"
            Me.ItemSelectingControl1.Field_Image = "Chain_ID"
            Me.ItemSelectingControl1.Field_Text = "Item_Description"
            Me.ItemSelectingControl1.Field_Text2 = "Identifier"
            Me.ItemSelectingControl1.Icon = WholeFoods.IRMA.ItemChaining.UserInterface.ItemSelectingControl.IconType.[Boolean]
            Me.ItemSelectingControl1.ItemText = "Item"
            Me.ItemSelectingControl1.ListHeight = 210
            Me.ItemSelectingControl1.Location = New System.Drawing.Point(11, 18)
            Me.ItemSelectingControl1.Name = "ItemSelectingControl1"
            Me.ItemSelectingControl1.ShowClearButton = True
            Me.ItemSelectingControl1.ShowExportButton = False
            Me.ItemSelectingControl1.Size = New System.Drawing.Size(485, 261)
            Me.ItemSelectingControl1.TabIndex = 2
            '
            'wpSelectStores
            '
            Me.wpSelectStores.Controls.Add(Me.cmbZone)
            Me.wpSelectStores.Controls.Add(Me.Label10)
            Me.wpSelectStores.Controls.Add(Me.cmbState)
            Me.wpSelectStores.Controls.Add(Me.Label11)
            Me.wpSelectStores.Controls.Add(Me.Label9)
            Me.wpSelectStores.Controls.Add(Me.isSelectStores)
            Me.wpSelectStores.Location = New System.Drawing.Point(4, 22)
            Me.wpSelectStores.Name = "wpSelectStores"
            Me.wpSelectStores.Padding = New System.Windows.Forms.Padding(3)
            Me.wpSelectStores.Size = New System.Drawing.Size(507, 289)
            Me.wpSelectStores.TabIndex = 4
            Me.wpSelectStores.Tag = "Select the stores that the price change will affect"
            Me.wpSelectStores.Text = "Select Stores"
            Me.wpSelectStores.UseVisualStyleBackColor = True
            '
            'cmbZone
            '
            Me.cmbZone.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbZone.FormattingEnabled = True
            Me.cmbZone.Location = New System.Drawing.Point(100, 224)
            Me.cmbZone.Name = "cmbZone"
            Me.cmbZone.Size = New System.Drawing.Size(129, 21)
            Me.cmbZone.TabIndex = 32
            '
            'Label10
            '
            Me.Label10.AutoSize = True
            Me.Label10.BackColor = System.Drawing.Color.Transparent
            Me.Label10.Location = New System.Drawing.Point(61, 227)
            Me.Label10.Name = "Label10"
            Me.Label10.Size = New System.Drawing.Size(35, 13)
            Me.Label10.TabIndex = 31
            Me.Label10.Text = "Zone:"
            '
            'cmbState
            '
            Me.cmbState.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbState.FormattingEnabled = True
            Me.cmbState.Location = New System.Drawing.Point(100, 250)
            Me.cmbState.Name = "cmbState"
            Me.cmbState.Size = New System.Drawing.Size(129, 21)
            Me.cmbState.TabIndex = 29
            '
            'Label11
            '
            Me.Label11.AutoSize = True
            Me.Label11.BackColor = System.Drawing.Color.Transparent
            Me.Label11.Location = New System.Drawing.Point(62, 253)
            Me.Label11.Name = "Label11"
            Me.Label11.Size = New System.Drawing.Size(35, 13)
            Me.Label11.TabIndex = 28
            Me.Label11.Text = "State:"
            '
            'Label9
            '
            Me.Label9.AutoSize = True
            Me.Label9.BackColor = System.Drawing.Color.Transparent
            Me.Label9.Location = New System.Drawing.Point(14, 227)
            Me.Label9.Name = "Label9"
            Me.Label9.Size = New System.Drawing.Size(46, 13)
            Me.Label9.TabIndex = 27
            Me.Label9.Text = "Filter by:"
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
            Me.isSelectStores.Location = New System.Drawing.Point(11, 17)
            Me.isSelectStores.Name = "isSelectStores"
            Me.isSelectStores.ShowClearButton = False
            Me.isSelectStores.ShowExportButton = False
            Me.isSelectStores.Size = New System.Drawing.Size(485, 233)
            Me.isSelectStores.TabIndex = 30
            '
            'wpPriceType
            '
            Me.wpPriceType.Controls.Add(Me.lstPriceType)
            Me.wpPriceType.Location = New System.Drawing.Point(4, 22)
            Me.wpPriceType.Name = "wpPriceType"
            Me.wpPriceType.Padding = New System.Windows.Forms.Padding(3)
            Me.wpPriceType.Size = New System.Drawing.Size(507, 289)
            Me.wpPriceType.TabIndex = 5
            Me.wpPriceType.Tag = "Please select if this is a regular or promotional price change "
            Me.wpPriceType.Text = "Select Price Change Type"
            Me.wpPriceType.UseVisualStyleBackColor = True
            '
            'lstPriceType
            '
            Me.lstPriceType.AllowDrop = True
            Me.lstPriceType.BackColor = System.Drawing.Color.LemonChiffon
            Me.lstPriceType.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.PriceType})
            Me.lstPriceType.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lstPriceType.FullRowSelect = True
            Me.lstPriceType.GridLines = True
            Me.lstPriceType.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            Me.lstPriceType.HideSelection = False
            Me.lstPriceType.Location = New System.Drawing.Point(193, 39)
            Me.lstPriceType.Name = "lstPriceType"
            Me.lstPriceType.Size = New System.Drawing.Size(121, 210)
            Me.lstPriceType.TabIndex = 23
            Me.lstPriceType.UseCompatibleStateImageBehavior = False
            Me.lstPriceType.View = System.Windows.Forms.View.Details
            '
            'PriceType
            '
            Me.PriceType.Text = "Type"
            Me.PriceType.Width = 117
            '
            'wpReg
            '
            Me.wpReg.Controls.Add(Me.txtChangePOSpricetoPrice)
            Me.wpReg.Controls.Add(Me.Label12)
            Me.wpReg.Controls.Add(Me.mclReg)
            Me.wpReg.Controls.Add(Me.radChangePOSpriceby)
            Me.wpReg.Controls.Add(Me.radChangePOSpriceto)
            Me.wpReg.Controls.Add(Me.Label18)
            Me.wpReg.Controls.Add(Me.txtChangePOSpricebyPercent)
            Me.wpReg.Controls.Add(Me.Label20)
            Me.wpReg.Controls.Add(Me.txtChangePOSpricetoQty)
            Me.wpReg.Location = New System.Drawing.Point(4, 22)
            Me.wpReg.Name = "wpReg"
            Me.wpReg.Padding = New System.Windows.Forms.Padding(3)
            Me.wpReg.Size = New System.Drawing.Size(507, 289)
            Me.wpReg.TabIndex = 6
            Me.wpReg.Tag = "Please enter the information for a REG price change"
            Me.wpReg.Text = "REG Price Change"
            Me.wpReg.UseVisualStyleBackColor = True
            '
            'txtChangePOSpricetoPrice
            '
            Me.txtChangePOSpricetoPrice.BackColor = System.Drawing.Color.MistyRose
            Me.txtChangePOSpricetoPrice.Location = New System.Drawing.Point(340, 22)
            Me.txtChangePOSpricetoPrice.Name = "txtChangePOSpricetoPrice"
            Me.txtChangePOSpricetoPrice.Size = New System.Drawing.Size(75, 20)
            Me.txtChangePOSpricetoPrice.TabIndex = 1
            '
            'Label12
            '
            Me.Label12.AutoSize = True
            Me.Label12.BackColor = System.Drawing.Color.Transparent
            Me.Label12.Location = New System.Drawing.Point(165, 93)
            Me.Label12.Name = "Label12"
            Me.Label12.Size = New System.Drawing.Size(58, 13)
            Me.Label12.TabIndex = 24
            Me.Label12.Text = "Start Date:"
            '
            'mclReg
            '
            Me.mclReg.BackColor = System.Drawing.Color.LemonChiffon
            Me.mclReg.Location = New System.Drawing.Point(165, 111)
            Me.mclReg.MaxSelectionCount = 1
            Me.mclReg.Name = "mclReg"
            Me.mclReg.ShowToday = False
            Me.mclReg.TabIndex = 23
            Me.mclReg.TitleBackColor = System.Drawing.Color.DarkKhaki
            '
            'radChangePOSpriceby
            '
            Me.radChangePOSpriceby.AutoSize = True
            Me.radChangePOSpriceby.BackColor = System.Drawing.Color.Transparent
            Me.radChangePOSpriceby.Location = New System.Drawing.Point(134, 50)
            Me.radChangePOSpriceby.Name = "radChangePOSpriceby"
            Me.radChangePOSpriceby.Size = New System.Drawing.Size(130, 17)
            Me.radChangePOSpriceby.TabIndex = 22
            Me.radChangePOSpriceby.Text = "Change POS price by "
            Me.radChangePOSpriceby.UseVisualStyleBackColor = False
            '
            'radChangePOSpriceto
            '
            Me.radChangePOSpriceto.AutoSize = True
            Me.radChangePOSpriceto.BackColor = System.Drawing.Color.Transparent
            Me.radChangePOSpriceto.Checked = True
            Me.radChangePOSpriceto.Location = New System.Drawing.Point(134, 22)
            Me.radChangePOSpriceto.Name = "radChangePOSpriceto"
            Me.radChangePOSpriceto.Size = New System.Drawing.Size(131, 17)
            Me.radChangePOSpriceto.TabIndex = 21
            Me.radChangePOSpriceto.TabStop = True
            Me.radChangePOSpriceto.Text = "Change POS price to: "
            Me.radChangePOSpriceto.UseVisualStyleBackColor = False
            '
            'Label18
            '
            Me.Label18.AutoSize = True
            Me.Label18.BackColor = System.Drawing.Color.Transparent
            Me.Label18.Location = New System.Drawing.Point(319, 52)
            Me.Label18.Name = "Label18"
            Me.Label18.Size = New System.Drawing.Size(15, 13)
            Me.Label18.TabIndex = 20
            Me.Label18.Text = "%"
            '
            'txtChangePOSpricebyPercent
            '
            Me.txtChangePOSpricebyPercent.Location = New System.Drawing.Point(271, 49)
            Me.txtChangePOSpricebyPercent.Name = "txtChangePOSpricebyPercent"
            Me.txtChangePOSpricebyPercent.Size = New System.Drawing.Size(42, 20)
            Me.txtChangePOSpricebyPercent.TabIndex = 19
            Me.txtChangePOSpricebyPercent.TabStop = False
            '
            'Label20
            '
            Me.Label20.AutoSize = True
            Me.Label20.BackColor = System.Drawing.Color.Transparent
            Me.Label20.Location = New System.Drawing.Point(316, 26)
            Me.Label20.Name = "Label20"
            Me.Label20.Size = New System.Drawing.Size(27, 13)
            Me.Label20.TabIndex = 18
            Me.Label20.Text = "@ $"
            '
            'txtChangePOSpricetoQty
            '
            Me.txtChangePOSpricetoQty.BackColor = System.Drawing.Color.MistyRose
            Me.txtChangePOSpricetoQty.Location = New System.Drawing.Point(271, 22)
            Me.txtChangePOSpricetoQty.Name = "txtChangePOSpricetoQty"
            Me.txtChangePOSpricetoQty.Size = New System.Drawing.Size(42, 20)
            Me.txtChangePOSpricetoQty.TabIndex = 0
            '
            'wpPromo
            '
            Me.wpPromo.Controls.Add(Me.Label6)
            Me.wpPromo.Controls.Add(Me.cmbPromoType)
            Me.wpPromo.Controls.Add(Me.mclPromoEnd)
            Me.wpPromo.Controls.Add(Me.mclPromoStart)
            Me.wpPromo.Controls.Add(Me.radChangePOSpromopricebyREG)
            Me.wpPromo.Controls.Add(Me.Label28)
            Me.wpPromo.Controls.Add(Me.txtChangePOSpromopricebyREG)
            Me.wpPromo.Controls.Add(Me.Label23)
            Me.wpPromo.Controls.Add(Me.txtChangePOSpromopricetoPriceMSRP)
            Me.wpPromo.Controls.Add(Me.Label22)
            Me.wpPromo.Controls.Add(Me.txtChangePOSpromopricetoQtyMSRP)
            Me.wpPromo.Controls.Add(Me.radChangePOSpromopricebyMSRP)
            Me.wpPromo.Controls.Add(Me.radChangePOSpromopriceto)
            Me.wpPromo.Controls.Add(Me.Label14)
            Me.wpPromo.Controls.Add(Me.txtChangePOSpromopricebyMSRP)
            Me.wpPromo.Controls.Add(Me.txtChangePOSpromopricetoPrice)
            Me.wpPromo.Controls.Add(Me.Label21)
            Me.wpPromo.Controls.Add(Me.txtChangePOSpromopricetoQty)
            Me.wpPromo.Controls.Add(Me.Label13)
            Me.wpPromo.Controls.Add(Me.Label19)
            Me.wpPromo.Location = New System.Drawing.Point(4, 22)
            Me.wpPromo.Name = "wpPromo"
            Me.wpPromo.Padding = New System.Windows.Forms.Padding(3)
            Me.wpPromo.Size = New System.Drawing.Size(507, 289)
            Me.wpPromo.TabIndex = 7
            Me.wpPromo.Tag = "Please enter the information for a ISS promo price change"
            Me.wpPromo.Text = "Promo Price Change"
            Me.wpPromo.UseVisualStyleBackColor = True
            '
            'Label6
            '
            Me.Label6.AutoSize = True
            Me.Label6.BackColor = System.Drawing.Color.Transparent
            Me.Label6.Location = New System.Drawing.Point(376, 73)
            Me.Label6.Name = "Label6"
            Me.Label6.Size = New System.Drawing.Size(73, 13)
            Me.Label6.TabIndex = 54
            Me.Label6.Text = "Price Method:"
            Me.Label6.Visible = False
            '
            'cmbPromoType
            '
            Me.cmbPromoType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbPromoType.FormattingEnabled = True
            Me.cmbPromoType.Items.AddRange(New Object() {"REGULAR PROMO", "EARNED DISCOUNT"})
            Me.cmbPromoType.Location = New System.Drawing.Point(375, 88)
            Me.cmbPromoType.Name = "cmbPromoType"
            Me.cmbPromoType.Size = New System.Drawing.Size(121, 21)
            Me.cmbPromoType.TabIndex = 53
            Me.cmbPromoType.Visible = False
            '
            'mclPromoEnd
            '
            Me.mclPromoEnd.BackColor = System.Drawing.Color.LemonChiffon
            Me.mclPromoEnd.Location = New System.Drawing.Point(319, 114)
            Me.mclPromoEnd.Name = "mclPromoEnd"
            Me.mclPromoEnd.ShowToday = False
            Me.mclPromoEnd.TabIndex = 52
            Me.mclPromoEnd.TitleBackColor = System.Drawing.Color.DarkKhaki
            '
            'mclPromoStart
            '
            Me.mclPromoStart.BackColor = System.Drawing.Color.LemonChiffon
            Me.mclPromoStart.Location = New System.Drawing.Point(74, 114)
            Me.mclPromoStart.Name = "mclPromoStart"
            Me.mclPromoStart.ShowToday = False
            Me.mclPromoStart.TabIndex = 51
            Me.mclPromoStart.TitleBackColor = System.Drawing.Color.DarkKhaki
            '
            'radChangePOSpromopricebyREG
            '
            Me.radChangePOSpromopricebyREG.AutoSize = True
            Me.radChangePOSpromopricebyREG.BackColor = System.Drawing.Color.Transparent
            Me.radChangePOSpromopricebyREG.Location = New System.Drawing.Point(17, 92)
            Me.radChangePOSpromopricebyREG.Name = "radChangePOSpromopricebyREG"
            Me.radChangePOSpromopricebyREG.Size = New System.Drawing.Size(162, 17)
            Me.radChangePOSpromopricebyREG.TabIndex = 50
            Me.radChangePOSpromopricebyREG.Text = "Change POS promo price by "
            Me.radChangePOSpromopricebyREG.UseVisualStyleBackColor = False
            '
            'Label28
            '
            Me.Label28.AutoSize = True
            Me.Label28.BackColor = System.Drawing.Color.Transparent
            Me.Label28.Location = New System.Drawing.Point(234, 94)
            Me.Label28.Name = "Label28"
            Me.Label28.Size = New System.Drawing.Size(102, 13)
            Me.Label28.TabIndex = 49
            Me.Label28.Text = "% based on Regular"
            '
            'txtChangePOSpromopricebyREG
            '
            Me.txtChangePOSpromopricebyREG.Location = New System.Drawing.Point(186, 91)
            Me.txtChangePOSpromopricebyREG.Name = "txtChangePOSpromopricebyREG"
            Me.txtChangePOSpromopricebyREG.Size = New System.Drawing.Size(42, 20)
            Me.txtChangePOSpromopricebyREG.TabIndex = 40
            Me.txtChangePOSpromopricebyREG.TabStop = False
            '
            'Label23
            '
            Me.Label23.AutoSize = True
            Me.Label23.BackColor = System.Drawing.Color.Transparent
            Me.Label23.Location = New System.Drawing.Point(82, 44)
            Me.Label23.Name = "Label23"
            Me.Label23.Size = New System.Drawing.Size(93, 13)
            Me.Label23.TabIndex = 48
            Me.Label23.Text = "Change MSRP to:"
            '
            'txtChangePOSpromopricetoPriceMSRP
            '
            Me.txtChangePOSpromopricetoPriceMSRP.Location = New System.Drawing.Point(256, 41)
            Me.txtChangePOSpromopricetoPriceMSRP.Name = "txtChangePOSpromopricetoPriceMSRP"
            Me.txtChangePOSpromopricetoPriceMSRP.Size = New System.Drawing.Size(75, 20)
            Me.txtChangePOSpromopricetoPriceMSRP.TabIndex = 38
            '
            'Label22
            '
            Me.Label22.AutoSize = True
            Me.Label22.BackColor = System.Drawing.Color.Transparent
            Me.Label22.Location = New System.Drawing.Point(231, 45)
            Me.Label22.Name = "Label22"
            Me.Label22.Size = New System.Drawing.Size(27, 13)
            Me.Label22.TabIndex = 47
            Me.Label22.Text = "@ $"
            '
            'txtChangePOSpromopricetoQtyMSRP
            '
            Me.txtChangePOSpromopricetoQtyMSRP.Location = New System.Drawing.Point(186, 41)
            Me.txtChangePOSpromopricetoQtyMSRP.Name = "txtChangePOSpromopricetoQtyMSRP"
            Me.txtChangePOSpromopricetoQtyMSRP.Size = New System.Drawing.Size(42, 20)
            Me.txtChangePOSpromopricetoQtyMSRP.TabIndex = 37
            '
            'radChangePOSpromopricebyMSRP
            '
            Me.radChangePOSpromopricebyMSRP.AutoSize = True
            Me.radChangePOSpromopricebyMSRP.BackColor = System.Drawing.Color.Transparent
            Me.radChangePOSpromopricebyMSRP.Location = New System.Drawing.Point(17, 69)
            Me.radChangePOSpromopricebyMSRP.Name = "radChangePOSpromopricebyMSRP"
            Me.radChangePOSpromopricebyMSRP.Size = New System.Drawing.Size(162, 17)
            Me.radChangePOSpromopricebyMSRP.TabIndex = 46
            Me.radChangePOSpromopricebyMSRP.Text = "Change POS promo price by "
            Me.radChangePOSpromopricebyMSRP.UseVisualStyleBackColor = False
            '
            'radChangePOSpromopriceto
            '
            Me.radChangePOSpromopriceto.AutoSize = True
            Me.radChangePOSpromopriceto.BackColor = System.Drawing.Color.Transparent
            Me.radChangePOSpromopriceto.Checked = True
            Me.radChangePOSpromopriceto.Location = New System.Drawing.Point(17, 19)
            Me.radChangePOSpromopriceto.Name = "radChangePOSpromopriceto"
            Me.radChangePOSpromopriceto.Size = New System.Drawing.Size(163, 17)
            Me.radChangePOSpromopriceto.TabIndex = 45
            Me.radChangePOSpromopriceto.TabStop = True
            Me.radChangePOSpromopriceto.Text = "Change POS promo price to: "
            Me.radChangePOSpromopriceto.UseVisualStyleBackColor = False
            '
            'Label14
            '
            Me.Label14.AutoSize = True
            Me.Label14.BackColor = System.Drawing.Color.Transparent
            Me.Label14.Location = New System.Drawing.Point(234, 71)
            Me.Label14.Name = "Label14"
            Me.Label14.Size = New System.Drawing.Size(96, 13)
            Me.Label14.TabIndex = 44
            Me.Label14.Text = "% based on MSRP"
            '
            'txtChangePOSpromopricebyMSRP
            '
            Me.txtChangePOSpromopricebyMSRP.Location = New System.Drawing.Point(186, 68)
            Me.txtChangePOSpromopricebyMSRP.Name = "txtChangePOSpromopricebyMSRP"
            Me.txtChangePOSpromopricebyMSRP.Size = New System.Drawing.Size(42, 20)
            Me.txtChangePOSpromopricebyMSRP.TabIndex = 39
            Me.txtChangePOSpromopricebyMSRP.TabStop = False
            '
            'txtChangePOSpromopricetoPrice
            '
            Me.txtChangePOSpromopricetoPrice.BackColor = System.Drawing.Color.MistyRose
            Me.txtChangePOSpromopricetoPrice.Location = New System.Drawing.Point(256, 19)
            Me.txtChangePOSpromopricetoPrice.Name = "txtChangePOSpromopricetoPrice"
            Me.txtChangePOSpromopricetoPrice.Size = New System.Drawing.Size(75, 20)
            Me.txtChangePOSpromopricetoPrice.TabIndex = 36
            '
            'Label21
            '
            Me.Label21.AutoSize = True
            Me.Label21.BackColor = System.Drawing.Color.Transparent
            Me.Label21.Location = New System.Drawing.Point(231, 23)
            Me.Label21.Name = "Label21"
            Me.Label21.Size = New System.Drawing.Size(27, 13)
            Me.Label21.TabIndex = 43
            Me.Label21.Text = "@ $"
            '
            'txtChangePOSpromopricetoQty
            '
            Me.txtChangePOSpromopricetoQty.BackColor = System.Drawing.Color.MistyRose
            Me.txtChangePOSpromopricetoQty.Location = New System.Drawing.Point(186, 19)
            Me.txtChangePOSpromopricetoQty.Name = "txtChangePOSpromopricetoQty"
            Me.txtChangePOSpromopricetoQty.Size = New System.Drawing.Size(42, 20)
            Me.txtChangePOSpromopricetoQty.TabIndex = 35
            '
            'Label13
            '
            Me.Label13.AutoSize = True
            Me.Label13.BackColor = System.Drawing.Color.Transparent
            Me.Label13.Location = New System.Drawing.Point(264, 114)
            Me.Label13.Name = "Label13"
            Me.Label13.Size = New System.Drawing.Size(55, 13)
            Me.Label13.TabIndex = 42
            Me.Label13.Text = "End Date:"
            '
            'Label19
            '
            Me.Label19.AutoSize = True
            Me.Label19.BackColor = System.Drawing.Color.Transparent
            Me.Label19.Location = New System.Drawing.Point(10, 114)
            Me.Label19.Name = "Label19"
            Me.Label19.Size = New System.Drawing.Size(58, 13)
            Me.Label19.TabIndex = 41
            Me.Label19.Text = "Start Date:"
            '
            'wpEarnedDiscount
            '
            Me.wpEarnedDiscount.Controls.Add(Me.TextBox11)
            Me.wpEarnedDiscount.Controls.Add(Me.TextBox10)
            Me.wpEarnedDiscount.Controls.Add(Me.TextBox9)
            Me.wpEarnedDiscount.Controls.Add(Me.Label26)
            Me.wpEarnedDiscount.Controls.Add(Me.Label25)
            Me.wpEarnedDiscount.Controls.Add(Me.Label24)
            Me.wpEarnedDiscount.Location = New System.Drawing.Point(4, 22)
            Me.wpEarnedDiscount.Name = "wpEarnedDiscount"
            Me.wpEarnedDiscount.Padding = New System.Windows.Forms.Padding(3)
            Me.wpEarnedDiscount.Size = New System.Drawing.Size(507, 289)
            Me.wpEarnedDiscount.TabIndex = 8
            Me.wpEarnedDiscount.Tag = "Please enter earned discount information"
            Me.wpEarnedDiscount.Text = "Earned Discount"
            Me.wpEarnedDiscount.UseVisualStyleBackColor = True
            '
            'TextBox11
            '
            Me.TextBox11.Location = New System.Drawing.Point(305, 165)
            Me.TextBox11.Name = "TextBox11"
            Me.TextBox11.Size = New System.Drawing.Size(59, 20)
            Me.TextBox11.TabIndex = 11
            '
            'TextBox10
            '
            Me.TextBox10.Location = New System.Drawing.Point(305, 135)
            Me.TextBox10.Name = "TextBox10"
            Me.TextBox10.Size = New System.Drawing.Size(59, 20)
            Me.TextBox10.TabIndex = 10
            '
            'TextBox9
            '
            Me.TextBox9.Location = New System.Drawing.Point(305, 103)
            Me.TextBox9.Name = "TextBox9"
            Me.TextBox9.Size = New System.Drawing.Size(59, 20)
            Me.TextBox9.TabIndex = 9
            '
            'Label26
            '
            Me.Label26.AutoSize = True
            Me.Label26.BackColor = System.Drawing.Color.Transparent
            Me.Label26.Location = New System.Drawing.Point(142, 168)
            Me.Label26.Name = "Label26"
            Me.Label26.Size = New System.Drawing.Size(147, 13)
            Me.Label26.TabIndex = 8
            Me.Label26.Text = "Limit this many at a sale price:"
            '
            'Label25
            '
            Me.Label25.AutoSize = True
            Me.Label25.BackColor = System.Drawing.Color.Transparent
            Me.Label25.Location = New System.Drawing.Point(142, 135)
            Me.Label25.Name = "Label25"
            Me.Label25.Size = New System.Drawing.Size(144, 13)
            Me.Label25.TabIndex = 7
            Me.Label25.Text = "Buy this many at a sale price:"
            '
            'Label24
            '
            Me.Label24.AutoSize = True
            Me.Label24.BackColor = System.Drawing.Color.Transparent
            Me.Label24.Location = New System.Drawing.Point(142, 103)
            Me.Label24.Name = "Label24"
            Me.Label24.Size = New System.Drawing.Size(157, 13)
            Me.Label24.TabIndex = 6
            Me.Label24.Text = "Buy this many at a regular price:"
            '
            'wpPreview
            '
            Me.wpPreview.Controls.Add(Me.lstPreview)
            Me.wpPreview.Location = New System.Drawing.Point(4, 22)
            Me.wpPreview.Name = "wpPreview"
            Me.wpPreview.Padding = New System.Windows.Forms.Padding(3)
            Me.wpPreview.Size = New System.Drawing.Size(507, 289)
            Me.wpPreview.TabIndex = 9
            Me.wpPreview.Tag = "Compare old and new prices before changing"
            Me.wpPreview.Text = "Price Change Preview"
            Me.wpPreview.UseVisualStyleBackColor = True
            '
            'lstPreview
            '
            Me.lstPreview.AllowDrop = True
            Me.lstPreview.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5})
            Me.lstPreview.FullRowSelect = True
            Me.lstPreview.GridLines = True
            ListViewGroup1.Header = "ANNA BANANA"
            ListViewGroup1.Name = "ListViewGroup1"
            ListViewGroup2.Header = "JALALA BEANS"
            ListViewGroup2.Name = "ListViewGroup2"
            Me.lstPreview.Groups.AddRange(New System.Windows.Forms.ListViewGroup() {ListViewGroup1, ListViewGroup2})
            Me.lstPreview.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            Me.lstPreview.HideSelection = False
            ListViewItem1.Group = ListViewGroup1
            ListViewItem2.Group = ListViewGroup2
            Me.lstPreview.Items.AddRange(New System.Windows.Forms.ListViewItem() {ListViewItem1, ListViewItem2})
            Me.lstPreview.Location = New System.Drawing.Point(7, 10)
            Me.lstPreview.Name = "lstPreview"
            Me.lstPreview.Size = New System.Drawing.Size(494, 273)
            Me.lstPreview.TabIndex = 25
            Me.lstPreview.UseCompatibleStateImageBehavior = False
            Me.lstPreview.View = System.Windows.Forms.View.Details
            '
            'ColumnHeader1
            '
            Me.ColumnHeader1.Text = "Store"
            Me.ColumnHeader1.Width = 157
            '
            'ColumnHeader2
            '
            Me.ColumnHeader2.Text = "POS Price"
            Me.ColumnHeader2.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.ColumnHeader2.Width = 70
            '
            'ColumnHeader3
            '
            Me.ColumnHeader3.Text = "New POS Price"
            Me.ColumnHeader3.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.ColumnHeader3.Width = 92
            '
            'ColumnHeader4
            '
            Me.ColumnHeader4.Text = "Margin"
            Me.ColumnHeader4.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.ColumnHeader4.Width = 73
            '
            'ColumnHeader5
            '
            Me.ColumnHeader5.Text = "New Margin"
            Me.ColumnHeader5.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            Me.ColumnHeader5.Width = 77
            '
            'wpFinal
            '
            Me.wpFinal.Controls.Add(Me.grpWarning)
            Me.wpFinal.Controls.Add(Me.lblValidating)
            Me.wpFinal.Controls.Add(Me.btnPreview)
            Me.wpFinal.Controls.Add(Me.lstSelectedStores)
            Me.wpFinal.Controls.Add(Me.lstSelectedItems)
            Me.wpFinal.Controls.Add(Me.chkIAggree)
            Me.wpFinal.Controls.Add(Me.lblFinal)
            Me.wpFinal.Controls.Add(Me.Label16)
            Me.wpFinal.Controls.Add(Me.Label15)
            Me.wpFinal.Location = New System.Drawing.Point(4, 22)
            Me.wpFinal.Name = "wpFinal"
            Me.wpFinal.Padding = New System.Windows.Forms.Padding(3)
            Me.wpFinal.Size = New System.Drawing.Size(507, 289)
            Me.wpFinal.TabIndex = 10
            Me.wpFinal.Tag = "Please review your selection and press ""Finish"" for the price change to take affe" & _
                "ct."
            Me.wpFinal.Text = "Please Review"
            Me.wpFinal.UseVisualStyleBackColor = True
            '
            'grpWarning
            '
            Me.grpWarning.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer))
            Me.grpWarning.Controls.Add(Me.PictureBox2)
            Me.grpWarning.Controls.Add(Me.lblWarning)
            Me.grpWarning.Cursor = System.Windows.Forms.Cursors.Hand
            Me.grpWarning.ForeColor = System.Drawing.Color.Red
            Me.grpWarning.Location = New System.Drawing.Point(17, 176)
            Me.grpWarning.Name = "grpWarning"
            Me.grpWarning.Size = New System.Drawing.Size(476, 94)
            Me.grpWarning.TabIndex = 34
            Me.grpWarning.TabStop = False
            Me.grpWarning.Text = "Warning"
            Me.grpWarning.Visible = False
            '
            'PictureBox2
            '
            Me.PictureBox2.BackColor = System.Drawing.Color.Transparent
            Me.PictureBox2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
            Me.PictureBox2.Location = New System.Drawing.Point(6, 19)
            Me.PictureBox2.Name = "PictureBox2"
            Me.PictureBox2.Size = New System.Drawing.Size(29, 23)
            Me.PictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
            Me.PictureBox2.TabIndex = 1
            Me.PictureBox2.TabStop = False
            '
            'lblWarning
            '
            Me.lblWarning.BackColor = System.Drawing.Color.Transparent
            Me.lblWarning.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblWarning.ForeColor = System.Drawing.Color.Black
            Me.lblWarning.Location = New System.Drawing.Point(41, 16)
            Me.lblWarning.Name = "lblWarning"
            Me.lblWarning.Size = New System.Drawing.Size(429, 71)
            Me.lblWarning.TabIndex = 0
            Me.lblWarning.Text = "Warning"
            '
            'lblValidating
            '
            Me.lblValidating.AutoSize = True
            Me.lblValidating.BackColor = System.Drawing.Color.Transparent
            Me.lblValidating.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblValidating.Font = New System.Drawing.Font("Microsoft Sans Serif", 24.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblValidating.ForeColor = System.Drawing.SystemColors.ControlDark
            Me.lblValidating.Location = New System.Drawing.Point(148, 85)
            Me.lblValidating.Name = "lblValidating"
            Me.lblValidating.Size = New System.Drawing.Size(201, 39)
            Me.lblValidating.TabIndex = 36
            Me.lblValidating.Text = "Validating..."
            Me.lblValidating.Visible = False
            '
            'btnPreview
            '
            Me.btnPreview.Location = New System.Drawing.Point(331, 151)
            Me.btnPreview.Name = "btnPreview"
            Me.btnPreview.Size = New System.Drawing.Size(152, 23)
            Me.btnPreview.TabIndex = 35
            Me.btnPreview.Text = "Price Changes Preview..."
            Me.btnPreview.UseVisualStyleBackColor = True
            '
            'lstSelectedStores
            '
            Me.lstSelectedStores.AllowDrop = True
            Me.lstSelectedStores.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.SelectedItem})
            Me.lstSelectedStores.FullRowSelect = True
            Me.lstSelectedStores.GridLines = True
            Me.lstSelectedStores.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            Me.lstSelectedStores.HideSelection = False
            Me.lstSelectedStores.Location = New System.Drawing.Point(262, 39)
            Me.lstSelectedStores.Name = "lstSelectedStores"
            Me.lstSelectedStores.Size = New System.Drawing.Size(221, 110)
            Me.lstSelectedStores.TabIndex = 33
            Me.lstSelectedStores.UseCompatibleStateImageBehavior = False
            Me.lstSelectedStores.View = System.Windows.Forms.View.Details
            '
            'SelectedItem
            '
            Me.SelectedItem.Text = "Store"
            Me.SelectedItem.Width = 217
            '
            'lstSelectedItems
            '
            Me.lstSelectedItems.AllowDrop = True
            Me.lstSelectedItems.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.FoundItem})
            Me.lstSelectedItems.FullRowSelect = True
            Me.lstSelectedItems.GridLines = True
            Me.lstSelectedItems.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable
            Me.lstSelectedItems.HideSelection = False
            Me.lstSelectedItems.Location = New System.Drawing.Point(17, 39)
            Me.lstSelectedItems.Name = "lstSelectedItems"
            Me.lstSelectedItems.Size = New System.Drawing.Size(216, 131)
            Me.lstSelectedItems.TabIndex = 32
            Me.lstSelectedItems.UseCompatibleStateImageBehavior = False
            Me.lstSelectedItems.View = System.Windows.Forms.View.Details
            '
            'FoundItem
            '
            Me.FoundItem.Text = "Item"
            Me.FoundItem.Width = 211
            '
            'chkIAggree
            '
            Me.chkIAggree.BackColor = System.Drawing.Color.Transparent
            Me.chkIAggree.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.chkIAggree.ForeColor = System.Drawing.Color.Red
            Me.chkIAggree.Location = New System.Drawing.Point(77, 222)
            Me.chkIAggree.Name = "chkIAggree"
            Me.chkIAggree.Size = New System.Drawing.Size(372, 48)
            Me.chkIAggree.TabIndex = 31
            Me.chkIAggree.Text = "I understand that price items batch details will be automatically generated for t" & _
                "hese items."
            Me.chkIAggree.UseVisualStyleBackColor = False
            Me.chkIAggree.Visible = False
            '
            'lblFinal
            '
            Me.lblFinal.BackColor = System.Drawing.Color.Transparent
            Me.lblFinal.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblFinal.Location = New System.Drawing.Point(14, 173)
            Me.lblFinal.Name = "lblFinal"
            Me.lblFinal.Size = New System.Drawing.Size(479, 57)
            Me.lblFinal.TabIndex = 30
            '
            'Label16
            '
            Me.Label16.AutoSize = True
            Me.Label16.BackColor = System.Drawing.Color.Transparent
            Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label16.Location = New System.Drawing.Point(259, 19)
            Me.Label16.Name = "Label16"
            Me.Label16.Size = New System.Drawing.Size(105, 17)
            Me.Label16.TabIndex = 29
            Me.Label16.Text = "In these stores:"
            '
            'Label15
            '
            Me.Label15.AutoSize = True
            Me.Label15.BackColor = System.Drawing.Color.Transparent
            Me.Label15.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.Label15.Location = New System.Drawing.Point(14, 19)
            Me.Label15.Name = "Label15"
            Me.Label15.Size = New System.Drawing.Size(89, 17)
            Me.Label15.TabIndex = 28
            Me.Label15.Text = "These items:"
            '
            'pnlButtons
            '
            Me.pnlButtons.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlButtons.Controls.Add(Me.btnCancel)
            Me.pnlButtons.Controls.Add(Me.btnBack)
            Me.pnlButtons.Controls.Add(Me.btnNext)
            Me.pnlButtons.Controls.Add(Me.btnFinish)
            Me.pnlButtons.Location = New System.Drawing.Point(-1, 351)
            Me.pnlButtons.Name = "pnlButtons"
            Me.pnlButtons.Size = New System.Drawing.Size(517, 37)
            Me.pnlButtons.TabIndex = 4
            '
            'btnCancel
            '
            Me.btnCancel.Location = New System.Drawing.Point(10, 7)
            Me.btnCancel.Name = "btnCancel"
            Me.btnCancel.Size = New System.Drawing.Size(75, 23)
            Me.btnCancel.TabIndex = 9
            Me.btnCancel.Text = "Cancel"
            Me.btnCancel.UseVisualStyleBackColor = True
            '
            'btnBack
            '
            Me.btnBack.Enabled = False
            Me.btnBack.Location = New System.Drawing.Point(352, 7)
            Me.btnBack.Name = "btnBack"
            Me.btnBack.Size = New System.Drawing.Size(75, 23)
            Me.btnBack.TabIndex = 8
            Me.btnBack.Text = "< Back"
            Me.btnBack.UseVisualStyleBackColor = True
            '
            'btnNext
            '
            Me.btnNext.Location = New System.Drawing.Point(433, 7)
            Me.btnNext.Name = "btnNext"
            Me.btnNext.Size = New System.Drawing.Size(75, 23)
            Me.btnNext.TabIndex = 2
            Me.btnNext.Text = "Next >"
            Me.btnNext.UseVisualStyleBackColor = True
            '
            'btnFinish
            '
            Me.btnFinish.Location = New System.Drawing.Point(433, 7)
            Me.btnFinish.Name = "btnFinish"
            Me.btnFinish.Size = New System.Drawing.Size(75, 23)
            Me.btnFinish.TabIndex = 10
            Me.btnFinish.Text = "Finish"
            Me.btnFinish.UseVisualStyleBackColor = True
            Me.btnFinish.Visible = False
            '
            'frmPriceChange2
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(512, 388)
            Me.Controls.Add(Me.pnlWorkArea)
            Me.Controls.Add(Me.pnlButtons)
            Me.Controls.Add(Me.pnlHeader)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "frmPriceChange2"
            Me.ShowIcon = False
            Me.Text = "Wizard"
            Me.pnlHeader.ResumeLayout(False)
            Me.pnlHeader.PerformLayout()
            CType(Me.picHEader, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlWorkArea.ResumeLayout(False)
            Me.TabControl1.ResumeLayout(False)
            Me.wpSearchItems.ResumeLayout(False)
            Me.wpSearchItems.PerformLayout()
            Me.wpSelectItems.ResumeLayout(False)
            Me.wpSelectItems.PerformLayout()
            Me.wpSelectStores.ResumeLayout(False)
            Me.wpSelectStores.PerformLayout()
            Me.wpPriceType.ResumeLayout(False)
            Me.wpReg.ResumeLayout(False)
            Me.wpReg.PerformLayout()
            Me.wpPromo.ResumeLayout(False)
            Me.wpPromo.PerformLayout()
            Me.wpEarnedDiscount.ResumeLayout(False)
            Me.wpEarnedDiscount.PerformLayout()
            Me.wpPreview.ResumeLayout(False)
            Me.wpFinal.ResumeLayout(False)
            Me.wpFinal.PerformLayout()
            Me.grpWarning.ResumeLayout(False)
            CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlButtons.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub
        Friend WithEvents pnlHeader As System.Windows.Forms.Panel
        Friend WithEvents lblTitle As System.Windows.Forms.Label
        Friend WithEvents picHEader As System.Windows.Forms.PictureBox
        Friend WithEvents lblHeader As System.Windows.Forms.Label
        Friend WithEvents pnlWorkArea As System.Windows.Forms.Panel
        Friend WithEvents pnlButtons As System.Windows.Forms.Panel
        Friend WithEvents btnCancel As System.Windows.Forms.Button
        Friend WithEvents btnBack As System.Windows.Forms.Button
        Friend WithEvents btnNext As System.Windows.Forms.Button
        Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
        Friend WithEvents wpSearchItems As System.Windows.Forms.TabPage
        Friend WithEvents lblWait As System.Windows.Forms.Label
        Friend WithEvents ItemSearchControl1 As ItemSearchControl
        Friend WithEvents wpSelectItems As System.Windows.Forms.TabPage
        Friend WithEvents lblMessage As System.Windows.Forms.Label
        Friend WithEvents ItemSelectingControl1 As ItemSelectingControl
        Friend WithEvents btnFinish As System.Windows.Forms.Button
        Friend WithEvents wpSelectStores As System.Windows.Forms.TabPage
        Friend WithEvents cmbZone As System.Windows.Forms.ComboBox
        Friend WithEvents Label10 As System.Windows.Forms.Label
        Friend WithEvents cmbState As System.Windows.Forms.ComboBox
        Friend WithEvents Label11 As System.Windows.Forms.Label
        Friend WithEvents Label9 As System.Windows.Forms.Label
        Friend WithEvents isSelectStores As ItemSelectingControl
        Friend WithEvents wpPriceType As System.Windows.Forms.TabPage
        Friend WithEvents lstPriceType As System.Windows.Forms.ListView
        Friend WithEvents PriceType As System.Windows.Forms.ColumnHeader
        Friend WithEvents wpReg As System.Windows.Forms.TabPage
        Friend WithEvents Label12 As System.Windows.Forms.Label
        Friend WithEvents mclReg As System.Windows.Forms.MonthCalendar
        Friend WithEvents radChangePOSpriceby As System.Windows.Forms.RadioButton
        Friend WithEvents radChangePOSpriceto As System.Windows.Forms.RadioButton
        Friend WithEvents Label18 As System.Windows.Forms.Label
        Friend WithEvents txtChangePOSpricebyPercent As System.Windows.Forms.TextBox
        Friend WithEvents Label20 As System.Windows.Forms.Label
        Friend WithEvents txtChangePOSpricetoQty As System.Windows.Forms.TextBox
        Friend WithEvents txtChangePOSpricetoPrice As System.Windows.Forms.TextBox
        Friend WithEvents wpPromo As System.Windows.Forms.TabPage
        Friend WithEvents Label6 As System.Windows.Forms.Label
        Friend WithEvents cmbPromoType As System.Windows.Forms.ComboBox
        Friend WithEvents mclPromoEnd As System.Windows.Forms.MonthCalendar
        Friend WithEvents mclPromoStart As System.Windows.Forms.MonthCalendar
        Friend WithEvents radChangePOSpromopricebyREG As System.Windows.Forms.RadioButton
        Friend WithEvents Label28 As System.Windows.Forms.Label
        Friend WithEvents txtChangePOSpromopricebyREG As System.Windows.Forms.TextBox
        Friend WithEvents Label23 As System.Windows.Forms.Label
        Friend WithEvents txtChangePOSpromopricetoPriceMSRP As System.Windows.Forms.TextBox
        Friend WithEvents Label22 As System.Windows.Forms.Label
        Friend WithEvents txtChangePOSpromopricetoQtyMSRP As System.Windows.Forms.TextBox
        Friend WithEvents radChangePOSpromopricebyMSRP As System.Windows.Forms.RadioButton
        Friend WithEvents radChangePOSpromopriceto As System.Windows.Forms.RadioButton
        Friend WithEvents Label14 As System.Windows.Forms.Label
        Friend WithEvents txtChangePOSpromopricebyMSRP As System.Windows.Forms.TextBox
        Friend WithEvents txtChangePOSpromopricetoPrice As System.Windows.Forms.TextBox
        Friend WithEvents Label21 As System.Windows.Forms.Label
        Friend WithEvents txtChangePOSpromopricetoQty As System.Windows.Forms.TextBox
        Friend WithEvents Label13 As System.Windows.Forms.Label
        Friend WithEvents Label19 As System.Windows.Forms.Label
        Friend WithEvents wpEarnedDiscount As System.Windows.Forms.TabPage
        Friend WithEvents TextBox11 As System.Windows.Forms.TextBox
        Friend WithEvents TextBox10 As System.Windows.Forms.TextBox
        Friend WithEvents TextBox9 As System.Windows.Forms.TextBox
        Friend WithEvents Label26 As System.Windows.Forms.Label
        Friend WithEvents Label25 As System.Windows.Forms.Label
        Friend WithEvents Label24 As System.Windows.Forms.Label
        Friend WithEvents wpPreview As System.Windows.Forms.TabPage
        Friend WithEvents lstPreview As System.Windows.Forms.ListView
        Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
        Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
        Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
        Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
        Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
        Friend WithEvents wpFinal As System.Windows.Forms.TabPage
        Friend WithEvents grpWarning As System.Windows.Forms.GroupBox
        Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
        Friend WithEvents lblWarning As System.Windows.Forms.Label
        Friend WithEvents lblValidating As System.Windows.Forms.Label
        Friend WithEvents btnPreview As System.Windows.Forms.Button
        Friend WithEvents lstSelectedStores As System.Windows.Forms.ListView
        Friend WithEvents SelectedItem As System.Windows.Forms.ColumnHeader
        Friend WithEvents lstSelectedItems As System.Windows.Forms.ListView
        Friend WithEvents FoundItem As System.Windows.Forms.ColumnHeader
        Friend WithEvents chkIAggree As System.Windows.Forms.CheckBox
        Friend WithEvents lblFinal As System.Windows.Forms.Label
        Friend WithEvents Label16 As System.Windows.Forms.Label
        Friend WithEvents Label15 As System.Windows.Forms.Label
    End Class
End Namespace