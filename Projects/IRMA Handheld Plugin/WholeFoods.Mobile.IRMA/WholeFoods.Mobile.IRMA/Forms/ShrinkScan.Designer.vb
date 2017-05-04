<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ShrinkScan
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
        Me.mnuMenu_ClearSession = New System.Windows.Forms.MenuItem
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.mnuMenu_ExitIRMA = New System.Windows.Forms.MenuItem
        Me.mnuMenu_ExitShrink = New System.Windows.Forms.MenuItem
        Me.mnuReview = New System.Windows.Forms.MenuItem
        Me.lblDescription = New System.Windows.Forms.Label
        Me.cmdClear = New System.Windows.Forms.Button
        Me.StoreTeamLabel = New System.Windows.Forms.Label
        Me.IrmaShrinkLabel = New System.Windows.Forms.Label
        Me.frmStatus = New System.Windows.Forms.StatusBar
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmdSave = New System.Windows.Forms.Button
        Me.txtQty = New System.Windows.Forms.TextBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtUpc = New System.Windows.Forms.TextBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.lblIQueuedQty = New System.Windows.Forms.Label
        Me.lblUOM = New System.Windows.Forms.Label
        Me.cmdSearch = New System.Windows.Forms.Button
        Me.lblRetailUnit = New System.Windows.Forms.Label
        Me.chkSkipConfirm = New System.Windows.Forms.CheckBox
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.mnuMenu)
        Me.mainMenu1.MenuItems.Add(Me.mnuReview)
        '
        'mnuMenu
        '
        Me.mnuMenu.MenuItems.Add(Me.mnuMenu_ClearSession)
        Me.mnuMenu.MenuItems.Add(Me.MenuItem1)
        Me.mnuMenu.MenuItems.Add(Me.mnuMenu_ExitIRMA)
        Me.mnuMenu.MenuItems.Add(Me.mnuMenu_ExitShrink)
        Me.mnuMenu.Text = "Menu"
        '
        'mnuMenu_ClearSession
        '
        Me.mnuMenu_ClearSession.Text = "Clear Session"
        '
        'MenuItem1
        '
        Me.MenuItem1.Text = "-"
        '
        'mnuMenu_ExitIRMA
        '
        Me.mnuMenu_ExitIRMA.Text = "Exit IRMA"
        '
        'mnuMenu_ExitShrink
        '
        Me.mnuMenu_ExitShrink.Text = "Exit Shrink"
        '
        'mnuReview
        '
        Me.mnuReview.Text = "Review"
        '
        'lblDescription
        '
        Me.lblDescription.Location = New System.Drawing.Point(61, 79)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(171, 43)
        '
        'cmdClear
        '
        Me.cmdClear.Location = New System.Drawing.Point(3, 215)
        Me.cmdClear.Name = "cmdClear"
        Me.cmdClear.Size = New System.Drawing.Size(109, 27)
        Me.cmdClear.TabIndex = 4
        Me.cmdClear.Text = "Clear"
        '
        'StoreTeamLabel
        '
        Me.StoreTeamLabel.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.StoreTeamLabel.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.StoreTeamLabel.Location = New System.Drawing.Point(0, 18)
        Me.StoreTeamLabel.Name = "StoreTeamLabel"
        Me.StoreTeamLabel.Size = New System.Drawing.Size(240, 29)
        '
        'IrmaShrinkLabel
        '
        Me.IrmaShrinkLabel.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.IrmaShrinkLabel.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.IrmaShrinkLabel.Location = New System.Drawing.Point(0, 0)
        Me.IrmaShrinkLabel.Name = "IrmaShrinkLabel"
        Me.IrmaShrinkLabel.Size = New System.Drawing.Size(240, 18)
        '
        'frmStatus
        '
        Me.frmStatus.Location = New System.Drawing.Point(0, 246)
        Me.frmStatus.Name = "frmStatus"
        Me.frmStatus.Size = New System.Drawing.Size(240, 22)
        Me.frmStatus.Text = "StatusBar1"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(3, 78)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(52, 20)
        Me.Label2.Text = "Desc:"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(3, 150)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(52, 20)
        Me.Label1.Text = "UOM:"
        '
        'cmdSave
        '
        Me.cmdSave.Location = New System.Drawing.Point(123, 215)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(109, 27)
        Me.cmdSave.TabIndex = 3
        Me.cmdSave.Text = "Save"
        '
        'txtQty
        '
        Me.txtQty.Location = New System.Drawing.Point(61, 122)
        Me.txtQty.Name = "txtQty"
        Me.txtQty.Size = New System.Drawing.Size(51, 21)
        Me.txtQty.TabIndex = 1
        Me.txtQty.Text = "1"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(3, 122)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(52, 20)
        Me.Label3.Text = "Qty:"
        '
        'txtUpc
        '
        Me.txtUpc.Location = New System.Drawing.Point(61, 54)
        Me.txtUpc.Name = "txtUpc"
        Me.txtUpc.Size = New System.Drawing.Size(133, 21)
        Me.txtUpc.TabIndex = 5
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(3, 55)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(52, 20)
        Me.Label4.Text = "UPC:"
        '
        'lblIQueuedQty
        '
        Me.lblIQueuedQty.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblIQueuedQty.Location = New System.Drawing.Point(3, 173)
        Me.lblIQueuedQty.Name = "lblIQueuedQty"
        Me.lblIQueuedQty.Size = New System.Drawing.Size(109, 20)
        Me.lblIQueuedQty.Text = "Queued:"
        '
        'lblUOM
        '
        Me.lblUOM.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblUOM.Location = New System.Drawing.Point(61, 150)
        Me.lblUOM.Name = "lblUOM"
        Me.lblUOM.Size = New System.Drawing.Size(162, 20)
        '
        'cmdSearch
        '
        Me.cmdSearch.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.cmdSearch.Location = New System.Drawing.Point(200, 54)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(32, 21)
        Me.cmdSearch.TabIndex = 12
        Me.cmdSearch.TabStop = False
        Me.cmdSearch.Text = ">>"
        '
        'lblRetailUnit
        '
        Me.lblRetailUnit.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblRetailUnit.Location = New System.Drawing.Point(123, 122)
        Me.lblRetailUnit.Name = "lblRetailUnit"
        Me.lblRetailUnit.Size = New System.Drawing.Size(109, 20)
        '
        'chkSkipConfirm
        '
        Me.chkSkipConfirm.Checked = True
        Me.chkSkipConfirm.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSkipConfirm.Location = New System.Drawing.Point(3, 195)
        Me.chkSkipConfirm.Name = "chkSkipConfirm"
        Me.chkSkipConfirm.Size = New System.Drawing.Size(109, 20)
        Me.chkSkipConfirm.TabIndex = 23
        Me.chkSkipConfirm.Text = "Skip Confirm"
        '
        'ShrinkScan
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.ControlBox = False
        Me.Controls.Add(Me.chkSkipConfirm)
        Me.Controls.Add(Me.lblRetailUnit)
        Me.Controls.Add(Me.cmdSearch)
        Me.Controls.Add(Me.lblUOM)
        Me.Controls.Add(Me.lblIQueuedQty)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.cmdClear)
        Me.Controls.Add(Me.StoreTeamLabel)
        Me.Controls.Add(Me.IrmaShrinkLabel)
        Me.Controls.Add(Me.frmStatus)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.txtQty)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtUpc)
        Me.Controls.Add(Me.Label4)
        Me.Menu = Me.mainMenu1
        Me.Name = "ShrinkScan"
        Me.Text = "Scan Shrink"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents mnuMenu As System.Windows.Forms.MenuItem
    Friend WithEvents mnuReview As System.Windows.Forms.MenuItem
    Friend WithEvents mnuMenu_ClearSession As System.Windows.Forms.MenuItem
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents cmdClear As System.Windows.Forms.Button
    Friend WithEvents StoreTeamLabel As System.Windows.Forms.Label
    Friend WithEvents IrmaShrinkLabel As System.Windows.Forms.Label
    Friend WithEvents frmStatus As System.Windows.Forms.StatusBar
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents txtQty As System.Windows.Forms.TextBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtUpc As System.Windows.Forms.TextBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents lblIQueuedQty As System.Windows.Forms.Label
    Friend WithEvents lblUOM As System.Windows.Forms.Label
    Friend WithEvents cmdSearch As System.Windows.Forms.Button
    Friend WithEvents mnuMenu_ExitShrink As System.Windows.Forms.MenuItem
    Friend WithEvents mnuMenu_ExitIRMA As System.Windows.Forms.MenuItem
    Friend WithEvents lblRetailUnit As System.Windows.Forms.Label
    Friend WithEvents chkSkipConfirm As System.Windows.Forms.CheckBox
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
End Class
