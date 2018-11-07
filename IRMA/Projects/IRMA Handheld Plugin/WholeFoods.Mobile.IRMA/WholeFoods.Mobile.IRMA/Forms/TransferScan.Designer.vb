<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class TransferScan
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
        Me.mnuMenu_DeleteOrder = New System.Windows.Forms.MenuItem
        Me.mnuMenu_SaveOrder = New System.Windows.Forms.MenuItem
        Me.mnuMenu_Back = New System.Windows.Forms.MenuItem
        Me.mmuReview = New System.Windows.Forms.MenuItem
        Me.lblRetailUnit = New System.Windows.Forms.Label
        Me.cmdSearch = New System.Windows.Forms.Button
        Me.lblIQueuedQty = New System.Windows.Forms.Label
        Me.lblDescription = New System.Windows.Forms.Label
        Me.cmdClear = New System.Windows.Forms.Button
        Me.frmStatus = New System.Windows.Forms.StatusBar
        Me.lblDesc = New System.Windows.Forms.Label
        Me.cmdSave = New System.Windows.Forms.Button
        Me.txtQty = New System.Windows.Forms.TextBox
        Me.lblQty = New System.Windows.Forms.Label
        Me.txtUpc = New System.Windows.Forms.TextBox
        Me.lblUPC = New System.Windows.Forms.Label
        Me.lblCost = New System.Windows.Forms.Label
        Me.lblItemCost = New System.Windows.Forms.Label
        Me.lblVendorPack = New System.Windows.Forms.Label
        Me.lblVendorPackValue = New System.Windows.Forms.Label
        Me.lblRetailUnitName = New System.Windows.Forms.Label
        Me.txtItemCost = New System.Windows.Forms.TextBox
        Me.lnlReason = New System.Windows.Forms.LinkLabel
        Me.cmbReason = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.mnuMenu)
        Me.mainMenu1.MenuItems.Add(Me.mmuReview)
        '
        'mnuMenu
        '
        Me.mnuMenu.MenuItems.Add(Me.mnuMenu_DeleteOrder)
        Me.mnuMenu.MenuItems.Add(Me.mnuMenu_SaveOrder)
        Me.mnuMenu.MenuItems.Add(Me.mnuMenu_Back)
        Me.mnuMenu.Text = "Menu"
        '
        'mnuMenu_DeleteOrder
        '
        Me.mnuMenu_DeleteOrder.Text = "Delete Order"
        '
        'mnuMenu_SaveOrder
        '
        Me.mnuMenu_SaveOrder.Checked = True
        Me.mnuMenu_SaveOrder.Text = "Save Order"
        '
        'mnuMenu_Back
        '
        Me.mnuMenu_Back.Text = "Back"
        '
        'mmuReview
        '
        Me.mmuReview.Text = "Review"
        '
        'lblRetailUnit
        '
        Me.lblRetailUnit.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lblRetailUnit.Location = New System.Drawing.Point(115, 101)
        Me.lblRetailUnit.Name = "lblRetailUnit"
        Me.lblRetailUnit.Size = New System.Drawing.Size(40, 20)
        Me.lblRetailUnit.Text = "Retail: "
        '
        'cmdSearch
        '
        Me.cmdSearch.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.cmdSearch.Location = New System.Drawing.Point(185, 13)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(32, 21)
        Me.cmdSearch.TabIndex = 2
        Me.cmdSearch.TabStop = False
        Me.cmdSearch.Text = ">>"
        '
        'lblIQueuedQty
        '
        Me.lblIQueuedQty.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblIQueuedQty.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblIQueuedQty.Location = New System.Drawing.Point(3, 199)
        Me.lblIQueuedQty.Name = "lblIQueuedQty"
        Me.lblIQueuedQty.Size = New System.Drawing.Size(106, 20)
        Me.lblIQueuedQty.Text = "Queued:"
        '
        'lblDescription
        '
        Me.lblDescription.Location = New System.Drawing.Point(50, 44)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(163, 43)
        '
        'cmdClear
        '
        Me.cmdClear.Location = New System.Drawing.Point(3, 221)
        Me.cmdClear.Name = "cmdClear"
        Me.cmdClear.Size = New System.Drawing.Size(106, 22)
        Me.cmdClear.TabIndex = 5
        Me.cmdClear.Text = "Clear"
        '
        'frmStatus
        '
        Me.frmStatus.Location = New System.Drawing.Point(0, 246)
        Me.frmStatus.Name = "frmStatus"
        Me.frmStatus.Size = New System.Drawing.Size(240, 22)
        Me.frmStatus.Text = "StatusBar1"
        '
        'lblDesc
        '
        Me.lblDesc.Location = New System.Drawing.Point(3, 44)
        Me.lblDesc.Name = "lblDesc"
        Me.lblDesc.Size = New System.Drawing.Size(41, 20)
        Me.lblDesc.Text = "Desc:"
        '
        'cmdSave
        '
        Me.cmdSave.Location = New System.Drawing.Point(136, 222)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(101, 21)
        Me.cmdSave.TabIndex = 6
        Me.cmdSave.Text = "Save"
        '
        'txtQty
        '
        Me.txtQty.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.txtQty.Location = New System.Drawing.Point(61, 100)
        Me.txtQty.Name = "txtQty"
        Me.txtQty.Size = New System.Drawing.Size(48, 19)
        Me.txtQty.TabIndex = 3
        Me.txtQty.Text = "1"
        '
        'lblQty
        '
        Me.lblQty.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lblQty.Location = New System.Drawing.Point(3, 100)
        Me.lblQty.Name = "lblQty"
        Me.lblQty.Size = New System.Drawing.Size(61, 20)
        Me.lblQty.Text = "Quantity:"
        '
        'txtUpc
        '
        Me.txtUpc.Location = New System.Drawing.Point(50, 13)
        Me.txtUpc.Name = "txtUpc"
        Me.txtUpc.Size = New System.Drawing.Size(129, 21)
        Me.txtUpc.TabIndex = 1
        '
        'lblUPC
        '
        Me.lblUPC.Location = New System.Drawing.Point(3, 14)
        Me.lblUPC.Name = "lblUPC"
        Me.lblUPC.Size = New System.Drawing.Size(41, 20)
        Me.lblUPC.Text = "UPC:"
        '
        'lblCost
        '
        Me.lblCost.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lblCost.Location = New System.Drawing.Point(3, 165)
        Me.lblCost.Name = "lblCost"
        Me.lblCost.Size = New System.Drawing.Size(76, 20)
        Me.lblCost.Text = "Vendor Cost:"
        '
        'lblItemCost
        '
        Me.lblItemCost.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblItemCost.Location = New System.Drawing.Point(80, 165)
        Me.lblItemCost.Name = "lblItemCost"
        Me.lblItemCost.Size = New System.Drawing.Size(99, 20)
        '
        'lblVendorPack
        '
        Me.lblVendorPack.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lblVendorPack.Location = New System.Drawing.Point(3, 122)
        Me.lblVendorPack.Name = "lblVendorPack"
        Me.lblVendorPack.Size = New System.Drawing.Size(80, 20)
        Me.lblVendorPack.Text = "Vendor Pack:"
        '
        'lblVendorPackValue
        '
        Me.lblVendorPackValue.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblVendorPackValue.Location = New System.Drawing.Point(80, 122)
        Me.lblVendorPackValue.Name = "lblVendorPackValue"
        Me.lblVendorPackValue.Size = New System.Drawing.Size(137, 40)
        '
        'lblRetailUnitName
        '
        Me.lblRetailUnitName.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblRetailUnitName.Location = New System.Drawing.Point(152, 100)
        Me.lblRetailUnitName.Name = "lblRetailUnitName"
        Me.lblRetailUnitName.Size = New System.Drawing.Size(65, 20)
        '
        'txtItemCost
        '
        Me.txtItemCost.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.txtItemCost.Location = New System.Drawing.Point(80, 165)
        Me.txtItemCost.Name = "txtItemCost"
        Me.txtItemCost.Size = New System.Drawing.Size(50, 19)
        Me.txtItemCost.TabIndex = 12
        '
        'lnlReason
        '
        Me.lnlReason.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Underline)
        Me.lnlReason.Location = New System.Drawing.Point(136, 165)
        Me.lnlReason.Name = "lnlReason"
        Me.lnlReason.Size = New System.Drawing.Size(43, 20)
        Me.lnlReason.TabIndex = 25
        Me.lnlReason.Text = "Reason"
        '
        'cmbReason
        '
        Me.cmbReason.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.cmbReason.Location = New System.Drawing.Point(185, 164)
        Me.cmbReason.Name = "cmbReason"
        Me.cmbReason.Size = New System.Drawing.Size(46, 20)
        Me.cmbReason.TabIndex = 26
        '
        'TransferScan
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.cmbReason)
        Me.Controls.Add(Me.lnlReason)
        Me.Controls.Add(Me.txtItemCost)
        Me.Controls.Add(Me.lblRetailUnitName)
        Me.Controls.Add(Me.lblVendorPackValue)
        Me.Controls.Add(Me.lblVendorPack)
        Me.Controls.Add(Me.lblItemCost)
        Me.Controls.Add(Me.lblCost)
        Me.Controls.Add(Me.lblRetailUnit)
        Me.Controls.Add(Me.cmdSearch)
        Me.Controls.Add(Me.lblIQueuedQty)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.cmdClear)
        Me.Controls.Add(Me.frmStatus)
        Me.Controls.Add(Me.lblDesc)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.txtQty)
        Me.Controls.Add(Me.lblQty)
        Me.Controls.Add(Me.txtUpc)
        Me.Controls.Add(Me.lblUPC)
        Me.Menu = Me.mainMenu1
        Me.Name = "TransferScan"
        Me.Text = "Transfer Scan"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents mnuMenu As System.Windows.Forms.MenuItem
    Friend WithEvents mnuMenu_DeleteOrder As System.Windows.Forms.MenuItem
    Friend WithEvents mnuMenu_Back As System.Windows.Forms.MenuItem
    Friend WithEvents mmuReview As System.Windows.Forms.MenuItem
    Friend WithEvents lblRetailUnit As System.Windows.Forms.Label
    Friend WithEvents cmdSearch As System.Windows.Forms.Button
    Friend WithEvents lblIQueuedQty As System.Windows.Forms.Label
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents cmdClear As System.Windows.Forms.Button
    Friend WithEvents frmStatus As System.Windows.Forms.StatusBar
    Friend WithEvents lblDesc As System.Windows.Forms.Label
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents txtQty As System.Windows.Forms.TextBox
    Friend WithEvents lblQty As System.Windows.Forms.Label
    Friend WithEvents txtUpc As System.Windows.Forms.TextBox
    Friend WithEvents lblUPC As System.Windows.Forms.Label
    Friend WithEvents lblCost As System.Windows.Forms.Label
    Friend WithEvents lblItemCost As System.Windows.Forms.Label
    Friend WithEvents mnuMenu_SaveOrder As System.Windows.Forms.MenuItem
    Friend WithEvents lblVendorPack As System.Windows.Forms.Label
    Friend WithEvents lblVendorPackValue As System.Windows.Forms.Label
    Friend WithEvents lblRetailUnitName As System.Windows.Forms.Label
    Friend WithEvents txtItemCost As System.Windows.Forms.TextBox
    Friend WithEvents lnlReason As System.Windows.Forms.LinkLabel
    Friend WithEvents cmbReason As System.Windows.Forms.ComboBox
End Class
