<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class CycleCountTeam
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
        Me.mnuMenu_ExitCycleCount = New System.Windows.Forms.MenuItem
        Me.mnuCount = New System.Windows.Forms.MenuItem
        Me.frmStatus = New System.Windows.Forms.StatusBar
        Me.lblSubTeamVal = New System.Windows.Forms.Label
        Me.lblStoreVal = New System.Windows.Forms.Label
        Me.lblItemSubTeamVal = New System.Windows.Forms.Label
        Me.lblItemSubTeam = New System.Windows.Forms.Label
        Me.lblPrimaryVendorVal = New System.Windows.Forms.Label
        Me.lblPrimaryVendor = New System.Windows.Forms.Label
        Me.lblPkgVal = New System.Windows.Forms.Label
        Me.lblPkg = New System.Windows.Forms.Label
        Me.lblDescriptionVal = New System.Windows.Forms.Label
        Me.cmdSearch = New System.Windows.Forms.Button
        Me.txtUpc = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.lblItemKeyVal = New System.Windows.Forms.Label
        Me.cmbLocation = New System.Windows.Forms.ComboBox
        Me.lblLocation = New System.Windows.Forms.Label
        Me.lblTotalVal = New System.Windows.Forms.Label
        Me.lblTotal = New System.Windows.Forms.Label
        Me.lblQty = New System.Windows.Forms.Label
        Me.txtQty = New System.Windows.Forms.TextBox
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.mnuMenu)
        Me.mainMenu1.MenuItems.Add(Me.mnuCount)
        '
        'mnuMenu
        '
        Me.mnuMenu.MenuItems.Add(Me.mnuMenu_Clear)
        Me.mnuMenu.MenuItems.Add(Me.mnuMenu_ExitCycleCount)
        Me.mnuMenu.Text = "Menu"
        '
        'mnuMenu_Clear
        '
        Me.mnuMenu_Clear.Text = "Clear Screen"
        '
        'mnuMenu_ExitCycleCount
        '
        Me.mnuMenu_ExitCycleCount.Text = "Exit Cycle Count"
        '
        'mnuCount
        '
        Me.mnuCount.Text = "Count"
        '
        'frmStatus
        '
        Me.frmStatus.Location = New System.Drawing.Point(0, 246)
        Me.frmStatus.Name = "frmStatus"
        Me.frmStatus.Size = New System.Drawing.Size(253, 22)
        Me.frmStatus.Text = "StatusBar1"
        '
        'lblSubTeamVal
        '
        Me.lblSubTeamVal.BackColor = System.Drawing.Color.Silver
        Me.lblSubTeamVal.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblSubTeamVal.Location = New System.Drawing.Point(122, 0)
        Me.lblSubTeamVal.Name = "lblSubTeamVal"
        Me.lblSubTeamVal.Size = New System.Drawing.Size(118, 20)
        Me.lblSubTeamVal.Text = "SubTeam"
        '
        'lblStoreVal
        '
        Me.lblStoreVal.BackColor = System.Drawing.Color.Silver
        Me.lblStoreVal.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblStoreVal.Location = New System.Drawing.Point(0, 0)
        Me.lblStoreVal.Name = "lblStoreVal"
        Me.lblStoreVal.Size = New System.Drawing.Size(118, 20)
        Me.lblStoreVal.Text = "Store"
        '
        'lblItemSubTeamVal
        '
        Me.lblItemSubTeamVal.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblItemSubTeamVal.Location = New System.Drawing.Point(88, 78)
        Me.lblItemSubTeamVal.Name = "lblItemSubTeamVal"
        Me.lblItemSubTeamVal.Size = New System.Drawing.Size(149, 20)
        Me.lblItemSubTeamVal.Text = "..."
        Me.lblItemSubTeamVal.Visible = False
        '
        'lblItemSubTeam
        '
        Me.lblItemSubTeam.Location = New System.Drawing.Point(3, 78)
        Me.lblItemSubTeam.Name = "lblItemSubTeam"
        Me.lblItemSubTeam.Size = New System.Drawing.Size(75, 20)
        Me.lblItemSubTeam.Text = "SubTeam:"
        Me.lblItemSubTeam.Visible = False
        '
        'lblPrimaryVendorVal
        '
        Me.lblPrimaryVendorVal.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblPrimaryVendorVal.Location = New System.Drawing.Point(3, 118)
        Me.lblPrimaryVendorVal.Name = "lblPrimaryVendorVal"
        Me.lblPrimaryVendorVal.Size = New System.Drawing.Size(219, 39)
        Me.lblPrimaryVendorVal.Text = "..."
        Me.lblPrimaryVendorVal.Visible = False
        '
        'lblPrimaryVendor
        '
        Me.lblPrimaryVendor.Location = New System.Drawing.Point(3, 98)
        Me.lblPrimaryVendor.Name = "lblPrimaryVendor"
        Me.lblPrimaryVendor.Size = New System.Drawing.Size(199, 20)
        Me.lblPrimaryVendor.Text = "Primary Vendor:"
        Me.lblPrimaryVendor.Visible = False
        '
        'lblPkgVal
        '
        Me.lblPkgVal.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblPkgVal.Location = New System.Drawing.Point(88, 58)
        Me.lblPkgVal.Name = "lblPkgVal"
        Me.lblPkgVal.Size = New System.Drawing.Size(149, 20)
        Me.lblPkgVal.Text = "..."
        Me.lblPkgVal.Visible = False
        '
        'lblPkg
        '
        Me.lblPkg.Location = New System.Drawing.Point(3, 58)
        Me.lblPkg.Name = "lblPkg"
        Me.lblPkg.Size = New System.Drawing.Size(75, 20)
        Me.lblPkg.Text = "Pkg:"
        Me.lblPkg.Visible = False
        '
        'lblDescriptionVal
        '
        Me.lblDescriptionVal.Enabled = False
        Me.lblDescriptionVal.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblDescriptionVal.Location = New System.Drawing.Point(3, 20)
        Me.lblDescriptionVal.Name = "lblDescriptionVal"
        Me.lblDescriptionVal.Size = New System.Drawing.Size(234, 38)
        Me.lblDescriptionVal.Text = "..."
        Me.lblDescriptionVal.Visible = False
        '
        'cmdSearch
        '
        Me.cmdSearch.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.cmdSearch.Location = New System.Drawing.Point(205, 219)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(32, 21)
        Me.cmdSearch.TabIndex = 106
        Me.cmdSearch.TabStop = False
        Me.cmdSearch.Text = ">>"
        '
        'txtUpc
        '
        Me.txtUpc.Location = New System.Drawing.Point(56, 219)
        Me.txtUpc.Name = "txtUpc"
        Me.txtUpc.Size = New System.Drawing.Size(143, 21)
        Me.txtUpc.TabIndex = 105
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(3, 219)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(41, 20)
        Me.Label4.Text = "UPC:"
        '
        'lblItemKeyVal
        '
        Me.lblItemKeyVal.Location = New System.Drawing.Point(199, 40)
        Me.lblItemKeyVal.Name = "lblItemKeyVal"
        Me.lblItemKeyVal.Size = New System.Drawing.Size(41, 20)
        Me.lblItemKeyVal.Visible = False
        '
        'cmbLocation
        '
        Me.cmbLocation.Location = New System.Drawing.Point(56, 191)
        Me.cmbLocation.Name = "cmbLocation"
        Me.cmbLocation.Size = New System.Drawing.Size(181, 22)
        Me.cmbLocation.TabIndex = 121
        Me.cmbLocation.Visible = False
        '
        'lblLocation
        '
        Me.lblLocation.BackColor = System.Drawing.Color.White
        Me.lblLocation.Location = New System.Drawing.Point(3, 193)
        Me.lblLocation.Name = "lblLocation"
        Me.lblLocation.Size = New System.Drawing.Size(41, 20)
        Me.lblLocation.Text = "Loc:"
        Me.lblLocation.Visible = False
        '
        'lblTotalVal
        '
        Me.lblTotalVal.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTotalVal.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotalVal.Location = New System.Drawing.Point(165, 163)
        Me.lblTotalVal.Name = "lblTotalVal"
        Me.lblTotalVal.Size = New System.Drawing.Size(32, 21)
        Me.lblTotalVal.Text = "0"
        Me.lblTotalVal.Visible = False
        '
        'lblTotal
        '
        Me.lblTotal.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblTotal.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.lblTotal.Location = New System.Drawing.Point(102, 163)
        Me.lblTotal.Name = "lblTotal"
        Me.lblTotal.Size = New System.Drawing.Size(63, 21)
        Me.lblTotal.Text = "Total:"
        Me.lblTotal.Visible = False
        '
        'lblQty
        '
        Me.lblQty.BackColor = System.Drawing.Color.White
        Me.lblQty.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblQty.Location = New System.Drawing.Point(3, 163)
        Me.lblQty.Name = "lblQty"
        Me.lblQty.Size = New System.Drawing.Size(53, 21)
        Me.lblQty.Visible = False
        '
        'txtQty
        '
        Me.txtQty.Location = New System.Drawing.Point(56, 163)
        Me.txtQty.Name = "txtQty"
        Me.txtQty.Size = New System.Drawing.Size(40, 21)
        Me.txtQty.TabIndex = 127
        Me.txtQty.Visible = False
        '
        'CycleCountTeam
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(253, 268)
        Me.ControlBox = False
        Me.Controls.Add(Me.lblTotalVal)
        Me.Controls.Add(Me.lblTotal)
        Me.Controls.Add(Me.lblQty)
        Me.Controls.Add(Me.txtQty)
        Me.Controls.Add(Me.lblLocation)
        Me.Controls.Add(Me.cmbLocation)
        Me.Controls.Add(Me.lblItemKeyVal)
        Me.Controls.Add(Me.cmdSearch)
        Me.Controls.Add(Me.txtUpc)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.lblItemSubTeamVal)
        Me.Controls.Add(Me.lblItemSubTeam)
        Me.Controls.Add(Me.lblPrimaryVendorVal)
        Me.Controls.Add(Me.lblPrimaryVendor)
        Me.Controls.Add(Me.lblPkgVal)
        Me.Controls.Add(Me.lblPkg)
        Me.Controls.Add(Me.lblDescriptionVal)
        Me.Controls.Add(Me.lblSubTeamVal)
        Me.Controls.Add(Me.lblStoreVal)
        Me.Controls.Add(Me.frmStatus)
        Me.Menu = Me.mainMenu1
        Me.Name = "CycleCountTeam"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents mnuMenu As System.Windows.Forms.MenuItem
    Friend WithEvents mnuCount As System.Windows.Forms.MenuItem
    Friend WithEvents mnuMenu_Clear As System.Windows.Forms.MenuItem
    Friend WithEvents frmStatus As System.Windows.Forms.StatusBar
    Friend WithEvents mnuMenu_ExitCycleCount As System.Windows.Forms.MenuItem
    Friend WithEvents lblSubTeamVal As System.Windows.Forms.Label
    Friend WithEvents lblStoreVal As System.Windows.Forms.Label
    Friend WithEvents lblItemSubTeamVal As System.Windows.Forms.Label
    Friend WithEvents lblItemSubTeam As System.Windows.Forms.Label
    Friend WithEvents lblPrimaryVendorVal As System.Windows.Forms.Label
    Friend WithEvents lblPrimaryVendor As System.Windows.Forms.Label
    Friend WithEvents lblPkgVal As System.Windows.Forms.Label
    Friend WithEvents lblPkg As System.Windows.Forms.Label
    Friend WithEvents lblDescriptionVal As System.Windows.Forms.Label
    Friend WithEvents cmdSearch As System.Windows.Forms.Button
    Friend WithEvents txtUpc As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblItemKeyVal As System.Windows.Forms.Label
    Friend WithEvents cmbLocation As System.Windows.Forms.ComboBox
    Friend WithEvents lblLocation As System.Windows.Forms.Label
    Friend WithEvents lblTotalVal As System.Windows.Forms.Label
    Friend WithEvents lblTotal As System.Windows.Forms.Label
    Friend WithEvents lblQty As System.Windows.Forms.Label
    Friend WithEvents txtQty As System.Windows.Forms.TextBox
End Class
