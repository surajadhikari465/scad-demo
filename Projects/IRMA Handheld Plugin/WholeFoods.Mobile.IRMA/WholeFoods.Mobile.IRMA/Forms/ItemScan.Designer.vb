<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ItemScan
    Inherits HandheldHardware.ScanForm

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
    Private mainMenu1 As System.Windows.Forms.MainMenu

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.mnuMenu = New System.Windows.Forms.MenuItem
        Me.mnuMenu_Clear = New System.Windows.Forms.MenuItem
        Me.mnuMenu_ExitItem = New System.Windows.Forms.MenuItem
        Me.mnuAction = New System.Windows.Forms.MenuItem
        Me.frmStatus = New System.Windows.Forms.StatusBar
        Me.txtQty = New System.Windows.Forms.TextBox
        Me.lblQty = New System.Windows.Forms.Label
        Me.txtUpc = New System.Windows.Forms.TextBox
        Me.lblUpc = New System.Windows.Forms.Label
        Me.cmdSearch = New System.Windows.Forms.Button
        Me.lblSubTeamVal = New System.Windows.Forms.Label
        Me.lblStoreVal = New System.Windows.Forms.Label
        Me.lblPriceType = New System.Windows.Forms.Label
        Me.lblPkgVal = New System.Windows.Forms.Label
        Me.lblPkg = New System.Windows.Forms.Label
        Me.lblDescriptionVal = New System.Windows.Forms.Label
        Me.lblReceivedVal = New System.Windows.Forms.Label
        Me.lblReceived = New System.Windows.Forms.Label
        Me.lblQueuedVal = New System.Windows.Forms.Label
        Me.lblOrderedVal = New System.Windows.Forms.Label
        Me.lblPriceVal = New System.Windows.Forms.Label
        Me.lblPrice = New System.Windows.Forms.Label
        Me.lblSaleVal = New System.Windows.Forms.Label
        Me.lblSale = New System.Windows.Forms.Label
        Me.lblSaleDatesVal = New System.Windows.Forms.Label
        Me.lblSaleDates = New System.Windows.Forms.Label
        Me.lblPrimaryVendorVal = New System.Windows.Forms.Label
        Me.lblPrimaryVendor = New System.Windows.Forms.Label
        Me.lblItemSubTeamVal = New System.Windows.Forms.Label
        Me.lblItemSubTeam = New System.Windows.Forms.Label
        Me.lblUomVal = New System.Windows.Forms.Label
        Me.lblPrintedVal = New System.Windows.Forms.Label
        Me.lblPrinted = New System.Windows.Forms.Label
        Me.lblItemKeyVal = New System.Windows.Forms.Label
        Me.lblOrdered = New System.Windows.Forms.Label
        Me.lblQueued = New System.Windows.Forms.Label
        Me.lblPriceTypeVal = New System.Windows.Forms.Label
        Me.chkSkipConfirm = New System.Windows.Forms.CheckBox
        Me.lblLastReceivedDate = New System.Windows.Forms.Label
        Me.lblLastReceivedDateVal = New System.Windows.Forms.Label
        Me.DisplayItemMov = New System.Windows.Forms.Button
        Me.DisplayEInvoiceQty = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.mnuMenu)
        Me.mainMenu1.MenuItems.Add(Me.mnuAction)
        '
        'mnuMenu
        '
        Me.mnuMenu.MenuItems.Add(Me.mnuMenu_Clear)
        Me.mnuMenu.MenuItems.Add(Me.mnuMenu_ExitItem)
        Me.mnuMenu.Text = "Menu"
        '
        'mnuMenu_Clear
        '
        Me.mnuMenu_Clear.Text = "Clear Screen"
        '
        'mnuMenu_ExitItem
        '
        Me.mnuMenu_ExitItem.Text = "Exit Item Scan"
        '
        'mnuAction
        '
        Me.mnuAction.Enabled = False
        Me.mnuAction.Text = "Action"
        '
        'frmStatus
        '
        Me.frmStatus.Location = New System.Drawing.Point(0, 537)
        Me.frmStatus.Name = "frmStatus"
        Me.frmStatus.Size = New System.Drawing.Size(480, 46)
        Me.frmStatus.Text = "StatusBar1"
        '
        'txtQty
        '
        Me.txtQty.Location = New System.Drawing.Point(112, 440)
        Me.txtQty.Name = "txtQty"
        Me.txtQty.Size = New System.Drawing.Size(100, 49)
        Me.txtQty.TabIndex = 1
        Me.txtQty.Visible = False
        '
        'lblQty
        '
        Me.lblQty.Location = New System.Drawing.Point(6, 440)
        Me.lblQty.Name = "lblQty"
        Me.lblQty.Size = New System.Drawing.Size(94, 42)
        Me.lblQty.Text = "Qty:"
        Me.lblQty.Visible = False
        '
        'txtUpc
        '
        Me.txtUpc.Location = New System.Drawing.Point(112, 488)
        Me.txtUpc.Name = "txtUpc"
        Me.txtUpc.Size = New System.Drawing.Size(286, 49)
        Me.txtUpc.TabIndex = 5
        '
        'lblUpc
        '
        Me.lblUpc.Location = New System.Drawing.Point(6, 488)
        Me.lblUpc.Name = "lblUpc"
        Me.lblUpc.Size = New System.Drawing.Size(94, 40)
        Me.lblUpc.Text = "UPC:"
        '
        'cmdSearch
        '
        Me.cmdSearch.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.cmdSearch.Location = New System.Drawing.Point(410, 488)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(64, 42)
        Me.cmdSearch.TabIndex = 12
        Me.cmdSearch.TabStop = False
        Me.cmdSearch.Text = ">>"
        '
        'lblSubTeamVal
        '
        Me.lblSubTeamVal.BackColor = System.Drawing.Color.Silver
        Me.lblSubTeamVal.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblSubTeamVal.Location = New System.Drawing.Point(244, 0)
        Me.lblSubTeamVal.Name = "lblSubTeamVal"
        Me.lblSubTeamVal.Size = New System.Drawing.Size(236, 40)
        Me.lblSubTeamVal.Text = "SubTeam"
        '
        'lblStoreVal
        '
        Me.lblStoreVal.BackColor = System.Drawing.Color.Silver
        Me.lblStoreVal.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold)
        Me.lblStoreVal.Location = New System.Drawing.Point(0, 0)
        Me.lblStoreVal.Name = "lblStoreVal"
        Me.lblStoreVal.Size = New System.Drawing.Size(236, 40)
        Me.lblStoreVal.Text = "Store"
        '
        'lblPriceType
        '
        Me.lblPriceType.Location = New System.Drawing.Point(6, 112)
        Me.lblPriceType.Name = "lblPriceType"
        Me.lblPriceType.Size = New System.Drawing.Size(136, 30)
        Me.lblPriceType.Text = "Price Type:"
        Me.lblPriceType.Visible = False
        '
        'lblPkgVal
        '
        Me.lblPkgVal.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblPkgVal.Location = New System.Drawing.Point(148, 150)
        Me.lblPkgVal.Name = "lblPkgVal"
        Me.lblPkgVal.Size = New System.Drawing.Size(326, 30)
        Me.lblPkgVal.Text = "..."
        Me.lblPkgVal.Visible = False
        '
        'lblPkg
        '
        Me.lblPkg.Location = New System.Drawing.Point(6, 150)
        Me.lblPkg.Name = "lblPkg"
        Me.lblPkg.Size = New System.Drawing.Size(136, 30)
        Me.lblPkg.Text = "Pkg:"
        Me.lblPkg.Visible = False
        '
        'lblDescriptionVal
        '
        Me.lblDescriptionVal.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblDescriptionVal.Location = New System.Drawing.Point(12, 31)
        Me.lblDescriptionVal.Name = "lblDescriptionVal"
        Me.lblDescriptionVal.Size = New System.Drawing.Size(468, 72)
        Me.lblDescriptionVal.Text = "..."
        Me.lblDescriptionVal.Visible = False
        '
        'lblReceivedVal
        '
        Me.lblReceivedVal.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblReceivedVal.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblReceivedVal.Location = New System.Drawing.Point(390, 414)
        Me.lblReceivedVal.Name = "lblReceivedVal"
        Me.lblReceivedVal.Size = New System.Drawing.Size(78, 26)
        Me.lblReceivedVal.Text = "0"
        Me.lblReceivedVal.Visible = False
        '
        'lblReceived
        '
        Me.lblReceived.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblReceived.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lblReceived.Location = New System.Drawing.Point(278, 414)
        Me.lblReceived.Name = "lblReceived"
        Me.lblReceived.Size = New System.Drawing.Size(112, 26)
        Me.lblReceived.Text = "Received:"
        Me.lblReceived.Visible = False
        '
        'lblQueuedVal
        '
        Me.lblQueuedVal.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblQueuedVal.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblQueuedVal.Location = New System.Drawing.Point(390, 376)
        Me.lblQueuedVal.Name = "lblQueuedVal"
        Me.lblQueuedVal.Size = New System.Drawing.Size(78, 26)
        Me.lblQueuedVal.Text = "0"
        Me.lblQueuedVal.Visible = False
        '
        'lblOrderedVal
        '
        Me.lblOrderedVal.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblOrderedVal.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblOrderedVal.Location = New System.Drawing.Point(390, 342)
        Me.lblOrderedVal.Name = "lblOrderedVal"
        Me.lblOrderedVal.Size = New System.Drawing.Size(78, 26)
        Me.lblOrderedVal.Text = "0"
        Me.lblOrderedVal.Visible = False
        '
        'lblPriceVal
        '
        Me.lblPriceVal.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblPriceVal.Location = New System.Drawing.Point(148, 226)
        Me.lblPriceVal.Name = "lblPriceVal"
        Me.lblPriceVal.Size = New System.Drawing.Size(326, 30)
        Me.lblPriceVal.Text = "..."
        Me.lblPriceVal.Visible = False
        '
        'lblPrice
        '
        Me.lblPrice.Location = New System.Drawing.Point(6, 226)
        Me.lblPrice.Name = "lblPrice"
        Me.lblPrice.Size = New System.Drawing.Size(136, 30)
        Me.lblPrice.Text = "Price:"
        Me.lblPrice.Visible = False
        '
        'lblSaleVal
        '
        Me.lblSaleVal.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblSaleVal.Location = New System.Drawing.Point(148, 264)
        Me.lblSaleVal.Name = "lblSaleVal"
        Me.lblSaleVal.Size = New System.Drawing.Size(326, 30)
        Me.lblSaleVal.Text = "..."
        Me.lblSaleVal.Visible = False
        '
        'lblSale
        '
        Me.lblSale.Location = New System.Drawing.Point(6, 264)
        Me.lblSale.Name = "lblSale"
        Me.lblSale.Size = New System.Drawing.Size(136, 30)
        Me.lblSale.Text = "Sale:"
        Me.lblSale.Visible = False
        '
        'lblSaleDatesVal
        '
        Me.lblSaleDatesVal.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblSaleDatesVal.Location = New System.Drawing.Point(148, 302)
        Me.lblSaleDatesVal.Name = "lblSaleDatesVal"
        Me.lblSaleDatesVal.Size = New System.Drawing.Size(324, 30)
        Me.lblSaleDatesVal.Text = "..."
        Me.lblSaleDatesVal.Visible = False
        '
        'lblSaleDates
        '
        Me.lblSaleDates.Location = New System.Drawing.Point(6, 302)
        Me.lblSaleDates.Name = "lblSaleDates"
        Me.lblSaleDates.Size = New System.Drawing.Size(136, 30)
        Me.lblSaleDates.Text = "Sale Dates:"
        Me.lblSaleDates.Visible = False
        '
        'lblPrimaryVendorVal
        '
        Me.lblPrimaryVendorVal.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblPrimaryVendorVal.Location = New System.Drawing.Point(6, 376)
        Me.lblPrimaryVendorVal.Name = "lblPrimaryVendorVal"
        Me.lblPrimaryVendorVal.Size = New System.Drawing.Size(270, 62)
        Me.lblPrimaryVendorVal.Text = "..."
        Me.lblPrimaryVendorVal.Visible = False
        '
        'lblPrimaryVendor
        '
        Me.lblPrimaryVendor.Location = New System.Drawing.Point(6, 340)
        Me.lblPrimaryVendor.Name = "lblPrimaryVendor"
        Me.lblPrimaryVendor.Size = New System.Drawing.Size(230, 32)
        Me.lblPrimaryVendor.Text = "Primary Vendor:"
        Me.lblPrimaryVendor.Visible = False
        '
        'lblItemSubTeamVal
        '
        Me.lblItemSubTeamVal.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblItemSubTeamVal.Location = New System.Drawing.Point(148, 188)
        Me.lblItemSubTeamVal.Name = "lblItemSubTeamVal"
        Me.lblItemSubTeamVal.Size = New System.Drawing.Size(326, 30)
        Me.lblItemSubTeamVal.Text = "..."
        Me.lblItemSubTeamVal.Visible = False
        '
        'lblItemSubTeam
        '
        Me.lblItemSubTeam.Location = New System.Drawing.Point(6, 188)
        Me.lblItemSubTeam.Name = "lblItemSubTeam"
        Me.lblItemSubTeam.Size = New System.Drawing.Size(136, 30)
        Me.lblItemSubTeam.Text = "Subteam:"
        Me.lblItemSubTeam.Visible = False
        '
        'lblUomVal
        '
        Me.lblUomVal.BackColor = System.Drawing.Color.White
        Me.lblUomVal.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblUomVal.Location = New System.Drawing.Point(224, 440)
        Me.lblUomVal.Name = "lblUomVal"
        Me.lblUomVal.Size = New System.Drawing.Size(48, 44)
        '
        'lblPrintedVal
        '
        Me.lblPrintedVal.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblPrintedVal.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblPrintedVal.Location = New System.Drawing.Point(410, 452)
        Me.lblPrintedVal.Name = "lblPrintedVal"
        Me.lblPrintedVal.Size = New System.Drawing.Size(58, 26)
        Me.lblPrintedVal.Text = "0"
        Me.lblPrintedVal.Visible = False
        '
        'lblPrinted
        '
        Me.lblPrinted.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblPrinted.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lblPrinted.Location = New System.Drawing.Point(284, 452)
        Me.lblPrinted.Name = "lblPrinted"
        Me.lblPrinted.Size = New System.Drawing.Size(106, 26)
        Me.lblPrinted.Text = "Printed:"
        Me.lblPrinted.Visible = False
        '
        'lblItemKeyVal
        '
        Me.lblItemKeyVal.Location = New System.Drawing.Point(284, 80)
        Me.lblItemKeyVal.Name = "lblItemKeyVal"
        Me.lblItemKeyVal.Size = New System.Drawing.Size(82, 40)
        Me.lblItemKeyVal.Visible = False
        '
        'lblOrdered
        '
        Me.lblOrdered.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblOrdered.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lblOrdered.Location = New System.Drawing.Point(278, 342)
        Me.lblOrdered.Name = "lblOrdered"
        Me.lblOrdered.Size = New System.Drawing.Size(112, 26)
        Me.lblOrdered.Text = "Ordered:"
        Me.lblOrdered.Visible = False
        '
        'lblQueued
        '
        Me.lblQueued.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblQueued.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lblQueued.Location = New System.Drawing.Point(278, 376)
        Me.lblQueued.Name = "lblQueued"
        Me.lblQueued.Size = New System.Drawing.Size(112, 26)
        Me.lblQueued.Text = "Queued:"
        Me.lblQueued.Visible = False
        '
        'lblPriceTypeVal
        '
        Me.lblPriceTypeVal.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblPriceTypeVal.Location = New System.Drawing.Point(148, 112)
        Me.lblPriceTypeVal.Name = "lblPriceTypeVal"
        Me.lblPriceTypeVal.Size = New System.Drawing.Size(128, 30)
        Me.lblPriceTypeVal.Text = "..."
        Me.lblPriceTypeVal.Visible = False
        '
        'chkSkipConfirm
        '
        Me.chkSkipConfirm.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.chkSkipConfirm.Location = New System.Drawing.Point(6, 442)
        Me.chkSkipConfirm.Name = "chkSkipConfirm"
        Me.chkSkipConfirm.Size = New System.Drawing.Size(200, 40)
        Me.chkSkipConfirm.TabIndex = 31
        Me.chkSkipConfirm.Text = "Skip Confirm"
        '
        'lblLastReceivedDate
        '
        Me.lblLastReceivedDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblLastReceivedDate.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lblLastReceivedDate.Location = New System.Drawing.Point(278, 452)
        Me.lblLastReceivedDate.Name = "lblLastReceivedDate"
        Me.lblLastReceivedDate.Size = New System.Drawing.Size(112, 26)
        Me.lblLastReceivedDate.Text = "Last Rcvd:"
        Me.lblLastReceivedDate.Visible = False
        '
        'lblLastReceivedDateVal
        '
        Me.lblLastReceivedDateVal.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblLastReceivedDateVal.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Regular)
        Me.lblLastReceivedDateVal.Location = New System.Drawing.Point(390, 452)
        Me.lblLastReceivedDateVal.Name = "lblLastReceivedDateVal"
        Me.lblLastReceivedDateVal.Size = New System.Drawing.Size(78, 26)
        Me.lblLastReceivedDateVal.Visible = False
        '
        'DisplayItemMov
        '
        Me.DisplayItemMov.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.DisplayItemMov.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.DisplayItemMov.Location = New System.Drawing.Point(24, 226)
        Me.DisplayItemMov.Name = "DisplayItemMov"
        Me.DisplayItemMov.Size = New System.Drawing.Size(164, 35)
        Me.DisplayItemMov.TabIndex = 61
        Me.DisplayItemMov.Text = "View Movement"
        Me.DisplayItemMov.Visible = False
        '
        'DisplayEInvoiceQty
        '
        Me.DisplayEInvoiceQty.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.DisplayEInvoiceQty.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.DisplayEInvoiceQty.Location = New System.Drawing.Point(208, 226)
        Me.DisplayEInvoiceQty.Name = "DisplayEInvoiceQty"
        Me.DisplayEInvoiceQty.Size = New System.Drawing.Size(161, 35)
        Me.DisplayEInvoiceQty.TabIndex = 62
        Me.DisplayEInvoiceQty.Text = "Confirmed QTY"
        Me.DisplayEInvoiceQty.Visible = False
        '
        'ItemScan
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(192.0!, 192.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(506, 536)
        Me.ControlBox = False
        Me.Controls.Add(Me.DisplayEInvoiceQty)
        Me.Controls.Add(Me.DisplayItemMov)
        Me.Controls.Add(Me.lblItemSubTeamVal)
        Me.Controls.Add(Me.lblLastReceivedDateVal)
        Me.Controls.Add(Me.lblLastReceivedDate)
        Me.Controls.Add(Me.chkSkipConfirm)
        Me.Controls.Add(Me.lblItemKeyVal)
        Me.Controls.Add(Me.lblOrdered)
        Me.Controls.Add(Me.lblQueued)
        Me.Controls.Add(Me.lblPriceTypeVal)
        Me.Controls.Add(Me.lblPrintedVal)
        Me.Controls.Add(Me.lblPrinted)
        Me.Controls.Add(Me.lblUomVal)
        Me.Controls.Add(Me.lblItemSubTeam)
        Me.Controls.Add(Me.lblPrimaryVendorVal)
        Me.Controls.Add(Me.lblPrimaryVendor)
        Me.Controls.Add(Me.lblSaleDatesVal)
        Me.Controls.Add(Me.lblSaleDates)
        Me.Controls.Add(Me.lblSaleVal)
        Me.Controls.Add(Me.lblSale)
        Me.Controls.Add(Me.lblPriceVal)
        Me.Controls.Add(Me.lblPrice)
        Me.Controls.Add(Me.lblOrderedVal)
        Me.Controls.Add(Me.lblQueuedVal)
        Me.Controls.Add(Me.lblReceivedVal)
        Me.Controls.Add(Me.lblReceived)
        Me.Controls.Add(Me.lblDescriptionVal)
        Me.Controls.Add(Me.lblPkgVal)
        Me.Controls.Add(Me.lblPkg)
        Me.Controls.Add(Me.lblPriceType)
        Me.Controls.Add(Me.lblSubTeamVal)
        Me.Controls.Add(Me.lblStoreVal)
        Me.Controls.Add(Me.cmdSearch)
        Me.Controls.Add(Me.frmStatus)
        Me.Controls.Add(Me.txtQty)
        Me.Controls.Add(Me.lblQty)
        Me.Controls.Add(Me.txtUpc)
        Me.Controls.Add(Me.lblUpc)
        Me.Location = New System.Drawing.Point(0, 52)
        Me.Menu = Me.mainMenu1
        Me.Name = "ItemScan"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents mnuMenu As System.Windows.Forms.MenuItem
    Friend WithEvents mnuAction As System.Windows.Forms.MenuItem
    Friend WithEvents mnuMenu_Clear As System.Windows.Forms.MenuItem
    Friend WithEvents frmStatus As System.Windows.Forms.StatusBar
    Friend WithEvents txtQty As System.Windows.Forms.TextBox
    Friend WithEvents lblQty As System.Windows.Forms.Label
    Friend WithEvents txtUpc As System.Windows.Forms.TextBox
    Friend WithEvents lblUpc As System.Windows.Forms.Label
    Friend WithEvents cmdSearch As System.Windows.Forms.Button
    Friend WithEvents mnuMenu_ExitItem As System.Windows.Forms.MenuItem
    Friend WithEvents lblSubTeamVal As System.Windows.Forms.Label
    Friend WithEvents lblStoreVal As System.Windows.Forms.Label
    Friend WithEvents lblPriceType As System.Windows.Forms.Label
    Friend WithEvents lblPkgVal As System.Windows.Forms.Label
    Friend WithEvents lblPkg As System.Windows.Forms.Label
    Friend WithEvents lblDescriptionVal As System.Windows.Forms.Label
    Friend WithEvents lblReceivedVal As System.Windows.Forms.Label
    Friend WithEvents lblReceived As System.Windows.Forms.Label
    Friend WithEvents lblQueuedVal As System.Windows.Forms.Label
    Friend WithEvents lblOrderedVal As System.Windows.Forms.Label
    Friend WithEvents lblPriceVal As System.Windows.Forms.Label
    Friend WithEvents lblPrice As System.Windows.Forms.Label
    Friend WithEvents lblSaleVal As System.Windows.Forms.Label
    Friend WithEvents lblSale As System.Windows.Forms.Label
    Friend WithEvents lblSaleDatesVal As System.Windows.Forms.Label
    Friend WithEvents lblSaleDates As System.Windows.Forms.Label
    Friend WithEvents lblPrimaryVendorVal As System.Windows.Forms.Label
    Friend WithEvents lblPrimaryVendor As System.Windows.Forms.Label
    Friend WithEvents lblItemSubTeamVal As System.Windows.Forms.Label
    Friend WithEvents lblItemSubTeam As System.Windows.Forms.Label
    Friend WithEvents lblUomVal As System.Windows.Forms.Label
    Friend WithEvents lblPrintedVal As System.Windows.Forms.Label
    Friend WithEvents lblPrinted As System.Windows.Forms.Label
    Friend WithEvents lblItemKeyVal As System.Windows.Forms.Label
    Friend WithEvents lblOrdered As System.Windows.Forms.Label
    Friend WithEvents lblQueued As System.Windows.Forms.Label
    Friend WithEvents lblPriceTypeVal As System.Windows.Forms.Label
    Friend WithEvents chkSkipConfirm As System.Windows.Forms.CheckBox
    Friend WithEvents lblLastReceivedDate As System.Windows.Forms.Label
    Friend WithEvents lblLastReceivedDateVal As System.Windows.Forms.Label
    Friend WithEvents DisplayItemMov As System.Windows.Forms.Button
    Friend WithEvents DisplayEInvoiceQty As System.Windows.Forms.Button
End Class
