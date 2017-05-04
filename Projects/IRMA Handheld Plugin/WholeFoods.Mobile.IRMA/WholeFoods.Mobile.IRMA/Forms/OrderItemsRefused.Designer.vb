<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class OrderItemsRefused
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
    Private mainMenu1 As System.Windows.Forms.MainMenu

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.mnuMain = New System.Windows.Forms.MenuItem
        Me.mnuAddRefusedItem = New System.Windows.Forms.MenuItem
        Me.mnuExit = New System.Windows.Forms.MenuItem
        Me.mnuUpdate = New System.Windows.Forms.MenuItem
        Me.tabRefusedList = New System.Windows.Forms.TabControl
        Me.tabEInvMismatch = New System.Windows.Forms.TabPage
        Me.lblPONumber = New System.Windows.Forms.Label
        Me.lblPONumberValue = New System.Windows.Forms.Label
        Me.lblTotalRefuse = New System.Windows.Forms.Label
        Me.txtTotalRefused = New System.Windows.Forms.Label
        Me.tabRefusedList.SuspendLayout()
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.mnuMain)
        Me.mainMenu1.MenuItems.Add(Me.mnuUpdate)
        '
        'mnuMain
        '
        Me.mnuMain.MenuItems.Add(Me.mnuAddRefusedItem)
        Me.mnuMain.MenuItems.Add(Me.mnuExit)
        Me.mnuMain.Text = "Menu"
        '
        'mnuAddRefusedItem
        '
        Me.mnuAddRefusedItem.Text = "Add Refused Item"
        '
        'mnuExit
        '
        Me.mnuExit.Text = "Exit"
        '
        'mnuUpdate
        '
        Me.mnuUpdate.Text = "Update"
        '
        'tabRefusedList
        '
        Me.tabRefusedList.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tabRefusedList.Controls.Add(Me.tabEInvMismatch)
        Me.tabRefusedList.Dock = System.Windows.Forms.DockStyle.None
        Me.tabRefusedList.Location = New System.Drawing.Point(0, 35)
        Me.tabRefusedList.Name = "tabRefusedList"
        Me.tabRefusedList.SelectedIndex = 0
        Me.tabRefusedList.Size = New System.Drawing.Size(240, 232)
        Me.tabRefusedList.TabIndex = 0
        '
        'tabEInvMismatch
        '
        Me.tabEInvMismatch.AutoScroll = True
        Me.tabEInvMismatch.Location = New System.Drawing.Point(0, 0)
        Me.tabEInvMismatch.Name = "tabEInvMismatch"
        Me.tabEInvMismatch.Size = New System.Drawing.Size(240, 209)
        Me.tabEInvMismatch.Text = "Click on Update button to Save"
        '
        'lblPONumber
        '
        Me.lblPONumber.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblPONumber.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.lblPONumber.Location = New System.Drawing.Point(4, 2)
        Me.lblPONumber.Name = "lblPONumber"
        Me.lblPONumber.Size = New System.Drawing.Size(30, 12)
        Me.lblPONumber.Text = "PO#"
        '
        'lblPONumberValue
        '
        Me.lblPONumberValue.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblPONumberValue.Location = New System.Drawing.Point(33, 2)
        Me.lblPONumberValue.Name = "lblPONumberValue"
        Me.lblPONumberValue.Size = New System.Drawing.Size(65, 12)
        '
        'lblTotalRefuse
        '
        Me.lblTotalRefuse.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.lblTotalRefuse.ForeColor = System.Drawing.SystemColors.ActiveCaption
        Me.lblTotalRefuse.Location = New System.Drawing.Point(101, 2)
        Me.lblTotalRefuse.Name = "lblTotalRefuse"
        Me.lblTotalRefuse.Size = New System.Drawing.Size(78, 12)
        Me.lblTotalRefuse.Text = "Tot. Refused:"
        '
        'txtTotalRefused
        '
        Me.txtTotalRefused.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.txtTotalRefused.Location = New System.Drawing.Point(180, 2)
        Me.txtTotalRefused.Name = "txtTotalRefused"
        Me.txtTotalRefused.Size = New System.Drawing.Size(55, 12)
        '
        'OrderItemsRefused
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.ControlBox = False
        Me.Controls.Add(Me.txtTotalRefused)
        Me.Controls.Add(Me.lblTotalRefuse)
        Me.Controls.Add(Me.lblPONumberValue)
        Me.Controls.Add(Me.lblPONumber)
        Me.Controls.Add(Me.tabRefusedList)
        Me.Menu = Me.mainMenu1
        Me.Name = "OrderItemsRefused"
        Me.Text = "Refused Items"
        Me.tabRefusedList.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents mnuMain As System.Windows.Forms.MenuItem
    Friend WithEvents tabRefusedList As System.Windows.Forms.TabControl
    Friend WithEvents tabEInvMismatch As System.Windows.Forms.TabPage
    Friend WithEvents lblPONumber As System.Windows.Forms.Label
    Friend WithEvents mnuExit As System.Windows.Forms.MenuItem
    Friend WithEvents lblPONumberValue As System.Windows.Forms.Label
    Friend WithEvents mnuUpdate As System.Windows.Forms.MenuItem
    Friend WithEvents lblTotalRefuse As System.Windows.Forms.Label
    Friend WithEvents txtTotalRefused As System.Windows.Forms.Label
    Friend WithEvents mnuAddRefusedItem As System.Windows.Forms.MenuItem
End Class
