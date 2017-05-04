<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ReceiveDocumentScan
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
        Me.MenuItemMenu = New System.Windows.Forms.MenuItem
        Me.MenuItemClearSession = New System.Windows.Forms.MenuItem
        Me.MenuItemViewSavedSession = New System.Windows.Forms.MenuItem
        Me.MenuItemBack = New System.Windows.Forms.MenuItem
        Me.mnuReview = New System.Windows.Forms.MenuItem
        Me.StoreTeamLabel2 = New System.Windows.Forms.Label
        Me.subTeamLabel = New System.Windows.Forms.Label
        Me.invoice1 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.cmdSearch = New System.Windows.Forms.Button
        Me.txtUPC = New System.Windows.Forms.TextBox
        Me.lblPkg = New System.Windows.Forms.Label
        Me.lblPkgVal = New System.Windows.Forms.Label
        Me.lblQty = New System.Windows.Forms.Label
        Me.txtQty = New System.Windows.Forms.TextBox
        Me.lblDescription = New System.Windows.Forms.Label
        Me.vendor = New System.Windows.Forms.Label
        Me.frmStatus = New System.Windows.Forms.StatusBar
        Me.cmdSave = New System.Windows.Forms.Button
        Me.cmdClear = New System.Windows.Forms.Button
        Me.Label3 = New System.Windows.Forms.Label
        Me.chkDiscount = New System.Windows.Forms.CheckBox
        Me.btnDiscount = New System.Windows.Forms.Button
        Me.invoiceNum = New System.Windows.Forms.TextBox
        Me.ComboBoxQuantityUnit = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.MenuItemMenu)
        Me.mainMenu1.MenuItems.Add(Me.mnuReview)
        '
        'MenuItemMenu
        '
        Me.MenuItemMenu.MenuItems.Add(Me.MenuItemClearSession)
        Me.MenuItemMenu.MenuItems.Add(Me.MenuItemViewSavedSession)
        Me.MenuItemMenu.MenuItems.Add(Me.MenuItemBack)
        Me.MenuItemMenu.Text = "Menu"
        '
        'MenuItemClearSession
        '
        Me.MenuItemClearSession.Text = "Clear Session"
        '
        'MenuItemViewSavedSession
        '
        Me.MenuItemViewSavedSession.Text = "View Saved Session"
        '
        'MenuItemBack
        '
        Me.MenuItemBack.Text = "Back"
        '
        'mnuReview
        '
        Me.mnuReview.Text = "Review"
        '
        'StoreTeamLabel2
        '
        Me.StoreTeamLabel2.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.StoreTeamLabel2.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold)
        Me.StoreTeamLabel2.Location = New System.Drawing.Point(0, 0)
        Me.StoreTeamLabel2.Name = "StoreTeamLabel2"
        Me.StoreTeamLabel2.Size = New System.Drawing.Size(120, 26)
        '
        'subTeamLabel
        '
        Me.subTeamLabel.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.subTeamLabel.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold)
        Me.subTeamLabel.Location = New System.Drawing.Point(126, 0)
        Me.subTeamLabel.Name = "subTeamLabel"
        Me.subTeamLabel.Size = New System.Drawing.Size(114, 26)
        '
        'invoice1
        '
        Me.invoice1.Location = New System.Drawing.Point(3, 32)
        Me.invoice1.Name = "invoice1"
        Me.invoice1.Size = New System.Drawing.Size(64, 18)
        Me.invoice1.Text = "Invoice#:"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(3, 54)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(64, 19)
        Me.Label1.Text = "Vendor:"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(3, 86)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(64, 20)
        Me.Label2.Text = "UPC:"
        '
        'cmdSearch
        '
        Me.cmdSearch.Enabled = False
        Me.cmdSearch.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.cmdSearch.Location = New System.Drawing.Point(195, 85)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(32, 19)
        Me.cmdSearch.TabIndex = 129
        Me.cmdSearch.TabStop = False
        Me.cmdSearch.Text = ">>"
        '
        'txtUPC
        '
        Me.txtUPC.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.txtUPC.Location = New System.Drawing.Point(73, 85)
        Me.txtUPC.Name = "txtUPC"
        Me.txtUPC.Size = New System.Drawing.Size(115, 19)
        Me.txtUPC.TabIndex = 131
        '
        'lblPkg
        '
        Me.lblPkg.Location = New System.Drawing.Point(3, 144)
        Me.lblPkg.Name = "lblPkg"
        Me.lblPkg.Size = New System.Drawing.Size(64, 20)
        Me.lblPkg.Text = "Pkg:"
        '
        'lblPkgVal
        '
        Me.lblPkgVal.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblPkgVal.Location = New System.Drawing.Point(73, 145)
        Me.lblPkgVal.Name = "lblPkgVal"
        Me.lblPkgVal.Size = New System.Drawing.Size(154, 20)
        '
        'lblQty
        '
        Me.lblQty.Location = New System.Drawing.Point(4, 166)
        Me.lblQty.Name = "lblQty"
        Me.lblQty.Size = New System.Drawing.Size(63, 19)
        Me.lblQty.Text = "Quantity:"
        '
        'txtQty
        '
        Me.txtQty.BackColor = System.Drawing.Color.White
        Me.txtQty.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.txtQty.Location = New System.Drawing.Point(73, 166)
        Me.txtQty.Name = "txtQty"
        Me.txtQty.Size = New System.Drawing.Size(47, 19)
        Me.txtQty.TabIndex = 1
        Me.txtQty.Text = "1"
        '
        'lblDescription
        '
        Me.lblDescription.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblDescription.Location = New System.Drawing.Point(73, 118)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(160, 27)
        '
        'vendor
        '
        Me.vendor.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.vendor.Location = New System.Drawing.Point(73, 54)
        Me.vendor.Name = "vendor"
        Me.vendor.Size = New System.Drawing.Size(160, 28)
        '
        'frmStatus
        '
        Me.frmStatus.Location = New System.Drawing.Point(0, 246)
        Me.frmStatus.Name = "frmStatus"
        Me.frmStatus.Size = New System.Drawing.Size(240, 22)
        '
        'cmdSave
        '
        Me.cmdSave.Location = New System.Drawing.Point(137, 220)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(90, 20)
        Me.cmdSave.TabIndex = 160
        Me.cmdSave.Text = "Save"
        '
        'cmdClear
        '
        Me.cmdClear.Location = New System.Drawing.Point(8, 220)
        Me.cmdClear.Name = "cmdClear"
        Me.cmdClear.Size = New System.Drawing.Size(90, 20)
        Me.cmdClear.TabIndex = 161
        Me.cmdClear.Text = "Clear"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(3, 115)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(64, 20)
        Me.Label3.Text = "Desc:"
        '
        'chkDiscount
        '
        Me.chkDiscount.Location = New System.Drawing.Point(4, 192)
        Me.chkDiscount.Name = "chkDiscount"
        Me.chkDiscount.Size = New System.Drawing.Size(22, 17)
        Me.chkDiscount.TabIndex = 174
        '
        'btnDiscount
        '
        Me.btnDiscount.Location = New System.Drawing.Point(28, 190)
        Me.btnDiscount.Name = "btnDiscount"
        Me.btnDiscount.Size = New System.Drawing.Size(66, 20)
        Me.btnDiscount.TabIndex = 175
        Me.btnDiscount.Text = "Discount"
        '
        'invoiceNum
        '
        Me.invoiceNum.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.invoiceNum.Location = New System.Drawing.Point(73, 32)
        Me.invoiceNum.Name = "invoiceNum"
        Me.invoiceNum.Size = New System.Drawing.Size(115, 19)
        Me.invoiceNum.TabIndex = 189
        '
        'ComboBoxQuantityUnit
        '
        Me.ComboBoxQuantityUnit.Enabled = False
        Me.ComboBoxQuantityUnit.Location = New System.Drawing.Point(126, 166)
        Me.ComboBoxQuantityUnit.Name = "ComboBoxQuantityUnit"
        Me.ComboBoxQuantityUnit.Size = New System.Drawing.Size(101, 22)
        Me.ComboBoxQuantityUnit.TabIndex = 202
        '
        'ReceiveDocumentScan
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.ComboBoxQuantityUnit)
        Me.Controls.Add(Me.invoiceNum)
        Me.Controls.Add(Me.btnDiscount)
        Me.Controls.Add(Me.chkDiscount)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.cmdClear)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.frmStatus)
        Me.Controls.Add(Me.vendor)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.txtQty)
        Me.Controls.Add(Me.lblQty)
        Me.Controls.Add(Me.lblPkgVal)
        Me.Controls.Add(Me.lblPkg)
        Me.Controls.Add(Me.txtUPC)
        Me.Controls.Add(Me.cmdSearch)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.invoice1)
        Me.Controls.Add(Me.subTeamLabel)
        Me.Controls.Add(Me.StoreTeamLabel2)
        Me.Menu = Me.mainMenu1
        Me.Name = "ReceiveDocumentScan"
        Me.Text = "RD Scan Item"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents StoreTeamLabel2 As System.Windows.Forms.Label
    Friend WithEvents subTeamLabel As System.Windows.Forms.Label
    Friend WithEvents MenuItemMenu As System.Windows.Forms.MenuItem
    Friend WithEvents mnuReview As System.Windows.Forms.MenuItem
    Friend WithEvents invoice1 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cmdSearch As System.Windows.Forms.Button
    Friend WithEvents txtUPC As System.Windows.Forms.TextBox
    Friend WithEvents lblPkg As System.Windows.Forms.Label
    Friend WithEvents lblPkgVal As System.Windows.Forms.Label
    Friend WithEvents lblQty As System.Windows.Forms.Label
    Friend WithEvents txtQty As System.Windows.Forms.TextBox
    Friend WithEvents MenuItemClearSession As System.Windows.Forms.MenuItem
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents vendor As System.Windows.Forms.Label
    Friend WithEvents frmStatus As System.Windows.Forms.StatusBar
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents cmdClear As System.Windows.Forms.Button
    Friend WithEvents MenuItemViewSavedSession As System.Windows.Forms.MenuItem
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents MenuItemBack As System.Windows.Forms.MenuItem
    Friend WithEvents chkDiscount As System.Windows.Forms.CheckBox
    Friend WithEvents btnDiscount As System.Windows.Forms.Button
    Friend WithEvents invoiceNum As System.Windows.Forms.TextBox
    Friend WithEvents ComboBoxQuantityUnit As System.Windows.Forms.ComboBox
End Class
