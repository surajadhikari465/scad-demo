<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ReceiveDocumentScan_Old
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
        Me.mnuMenu_Back = New System.Windows.Forms.MenuItem
        Me.mnuMenu_ClearSession = New System.Windows.Forms.MenuItem
        Me.mnuMenu_SaveSession = New System.Windows.Forms.MenuItem
        Me.mnuMenu_ViewSavedSessions = New System.Windows.Forms.MenuItem
        Me.mnuMenu_ExitIRMA = New System.Windows.Forms.MenuItem
        Me.mnuMenu_ExitReceiveDocument = New System.Windows.Forms.MenuItem
        Me.mnuReview = New System.Windows.Forms.MenuItem
        Me.IrmaReceiveDocuLabel = New System.Windows.Forms.Label
        Me.StoreTeamLabel = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.txtUpc = New System.Windows.Forms.TextBox
        Me.cmdSearch = New System.Windows.Forms.Button
        Me.Label2 = New System.Windows.Forms.Label
        Me.lblDescription = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.txtQty = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblUOM = New System.Windows.Forms.Label
        Me.lblRetailUnit = New System.Windows.Forms.Label
        Me.chkSkipConfirm = New System.Windows.Forms.CheckBox
        Me.cmdClear = New System.Windows.Forms.Button
        Me.cmdSave = New System.Windows.Forms.Button
        Me.frmStatus = New System.Windows.Forms.StatusBar
        Me.lblIQueuedQty = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.mnuMenu)
        Me.mainMenu1.MenuItems.Add(Me.mnuReview)
        '
        'mnuMenu
        '
        Me.mnuMenu.MenuItems.Add(Me.mnuMenu_Back)
        Me.mnuMenu.MenuItems.Add(Me.mnuMenu_ClearSession)
        Me.mnuMenu.MenuItems.Add(Me.mnuMenu_SaveSession)
        Me.mnuMenu.MenuItems.Add(Me.mnuMenu_ViewSavedSessions)
        Me.mnuMenu.MenuItems.Add(Me.mnuMenu_ExitIRMA)
        Me.mnuMenu.MenuItems.Add(Me.mnuMenu_ExitReceiveDocument)
        Me.mnuMenu.Text = "Menu"
        '
        'mnuMenu_Back
        '
        Me.mnuMenu_Back.Text = "New Session"
        '
        'mnuMenu_ClearSession
        '
        Me.mnuMenu_ClearSession.Text = "Clear Session"
        '
        'mnuMenu_SaveSession
        '
        Me.mnuMenu_SaveSession.Checked = True
        Me.mnuMenu_SaveSession.Text = "Save Session"
        '
        'mnuMenu_ViewSavedSessions
        '
        Me.mnuMenu_ViewSavedSessions.Text = "View Saved Sessions"
        '
        'mnuMenu_ExitIRMA
        '
        Me.mnuMenu_ExitIRMA.Text = "Exit IRMA"
        '
        'mnuMenu_ExitReceiveDocument
        '
        Me.mnuMenu_ExitReceiveDocument.Text = "Exit Receive Document"
        '
        'mnuReview
        '
        Me.mnuReview.Text = "Review"
        '
        'IrmaReceiveDocuLabel
        '
        Me.IrmaReceiveDocuLabel.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.IrmaReceiveDocuLabel.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.IrmaReceiveDocuLabel.Location = New System.Drawing.Point(0, 0)
        Me.IrmaReceiveDocuLabel.Name = "IrmaReceiveDocuLabel"
        Me.IrmaReceiveDocuLabel.Size = New System.Drawing.Size(240, 18)
        '
        'StoreTeamLabel
        '
        Me.StoreTeamLabel.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.StoreTeamLabel.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.StoreTeamLabel.Location = New System.Drawing.Point(0, 18)
        Me.StoreTeamLabel.Name = "StoreTeamLabel"
        Me.StoreTeamLabel.Size = New System.Drawing.Size(240, 29)
        '
        'Label4
        '
        Me.Label4.Location = New System.Drawing.Point(6, 60)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(41, 20)
        Me.Label4.Text = "UPC:"
        '
        'txtUpc
        '
        Me.txtUpc.Location = New System.Drawing.Point(60, 55)
        Me.txtUpc.Name = "txtUpc"
        Me.txtUpc.Size = New System.Drawing.Size(133, 21)
        Me.txtUpc.TabIndex = 6
        '
        'cmdSearch
        '
        Me.cmdSearch.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.cmdSearch.Location = New System.Drawing.Point(201, 51)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(32, 29)
        Me.cmdSearch.TabIndex = 13
        Me.cmdSearch.TabStop = False
        Me.cmdSearch.Text = ">>"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(6, 87)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(45, 20)
        Me.Label2.Text = "Desc:"
        '
        'lblDescription
        '
        Me.lblDescription.Location = New System.Drawing.Point(60, 85)
        Me.lblDescription.Name = "lblDescription"
        Me.lblDescription.Size = New System.Drawing.Size(171, 43)
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(6, 132)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(61, 20)
        Me.Label3.Text = "Quantity:"
        '
        'txtQty
        '
        Me.txtQty.Location = New System.Drawing.Point(73, 131)
        Me.txtQty.Name = "txtQty"
        Me.txtQty.Size = New System.Drawing.Size(51, 21)
        Me.txtQty.TabIndex = 20
        Me.txtQty.Text = "1"
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(7, 159)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 20)
        Me.Label1.Text = "UOM:"
        '
        'lblUOM
        '
        Me.lblUOM.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblUOM.Location = New System.Drawing.Point(73, 157)
        Me.lblUOM.Name = "lblUOM"
        Me.lblUOM.Size = New System.Drawing.Size(162, 20)
        '
        'lblRetailUnit
        '
        Me.lblRetailUnit.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblRetailUnit.Location = New System.Drawing.Point(130, 132)
        Me.lblRetailUnit.Name = "lblRetailUnit"
        Me.lblRetailUnit.Size = New System.Drawing.Size(109, 20)
        '
        'chkSkipConfirm
        '
        Me.chkSkipConfirm.Checked = True
        Me.chkSkipConfirm.CheckState = System.Windows.Forms.CheckState.Checked
        Me.chkSkipConfirm.Location = New System.Drawing.Point(6, 196)
        Me.chkSkipConfirm.Name = "chkSkipConfirm"
        Me.chkSkipConfirm.Size = New System.Drawing.Size(109, 20)
        Me.chkSkipConfirm.TabIndex = 27
        Me.chkSkipConfirm.Text = "Skip Confirm"
        '
        'cmdClear
        '
        Me.cmdClear.Location = New System.Drawing.Point(7, 216)
        Me.cmdClear.Name = "cmdClear"
        Me.cmdClear.Size = New System.Drawing.Size(109, 27)
        Me.cmdClear.TabIndex = 28
        Me.cmdClear.Text = "Clear"
        '
        'cmdSave
        '
        Me.cmdSave.Location = New System.Drawing.Point(128, 216)
        Me.cmdSave.Name = "cmdSave"
        Me.cmdSave.Size = New System.Drawing.Size(109, 27)
        Me.cmdSave.TabIndex = 29
        Me.cmdSave.Text = "Save"
        '
        'frmStatus
        '
        Me.frmStatus.Location = New System.Drawing.Point(0, 246)
        Me.frmStatus.Name = "frmStatus"
        Me.frmStatus.Size = New System.Drawing.Size(240, 22)
        Me.frmStatus.Text = "StatusBar1"
        '
        'lblIQueuedQty
        '
        Me.lblIQueuedQty.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.lblIQueuedQty.Location = New System.Drawing.Point(8, 176)
        Me.lblIQueuedQty.Name = "lblIQueuedQty"
        Me.lblIQueuedQty.Size = New System.Drawing.Size(109, 20)
        Me.lblIQueuedQty.Text = "Queued:"
        '
        'ReceiveDocumentScan
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.lblIQueuedQty)
        Me.Controls.Add(Me.frmStatus)
        Me.Controls.Add(Me.cmdSave)
        Me.Controls.Add(Me.cmdClear)
        Me.Controls.Add(Me.chkSkipConfirm)
        Me.Controls.Add(Me.lblRetailUnit)
        Me.Controls.Add(Me.lblUOM)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtQty)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.lblDescription)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.cmdSearch)
        Me.Controls.Add(Me.txtUpc)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.StoreTeamLabel)
        Me.Controls.Add(Me.IrmaReceiveDocuLabel)
        Me.Menu = Me.mainMenu1
        Me.Name = "ReceiveDocumentScan"
        Me.Text = "Scan Receive Document"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents IrmaReceiveDocuLabel As System.Windows.Forms.Label
    Friend WithEvents StoreTeamLabel As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents txtUpc As System.Windows.Forms.TextBox
    Friend WithEvents cmdSearch As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents lblDescription As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents txtQty As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents lblUOM As System.Windows.Forms.Label
    Friend WithEvents lblRetailUnit As System.Windows.Forms.Label
    Friend WithEvents chkSkipConfirm As System.Windows.Forms.CheckBox
    Friend WithEvents cmdClear As System.Windows.Forms.Button
    Friend WithEvents cmdSave As System.Windows.Forms.Button
    Friend WithEvents frmStatus As System.Windows.Forms.StatusBar
    Friend WithEvents mnuMenu As System.Windows.Forms.MenuItem
    Friend WithEvents mnuReview As System.Windows.Forms.MenuItem
    Friend WithEvents mnuMenu_Back As System.Windows.Forms.MenuItem
    Friend WithEvents mnuMenu_ClearSession As System.Windows.Forms.MenuItem
    Friend WithEvents mnuMenu_SaveSession As System.Windows.Forms.MenuItem
    Friend WithEvents mnuMenu_ViewSavedSessions As System.Windows.Forms.MenuItem
    Friend WithEvents mnuMenu_ExitIRMA As System.Windows.Forms.MenuItem
    Friend WithEvents mnuMenu_ExitReceiveDocument As System.Windows.Forms.MenuItem
    Friend WithEvents lblIQueuedQty As System.Windows.Forms.Label
End Class
