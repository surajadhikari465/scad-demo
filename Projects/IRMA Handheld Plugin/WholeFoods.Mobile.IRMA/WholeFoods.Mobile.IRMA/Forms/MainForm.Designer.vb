<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class MainForm
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
        Me.cmdClose = New System.Windows.Forms.MenuItem
        Me.MenuInfo = New System.Windows.Forms.MenuItem
        Me.cmdShrink = New System.Windows.Forms.Button
        Me.StoreComboBox = New System.Windows.Forms.ComboBox
        Me.SubteamComboBox = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.cmdOrder = New System.Windows.Forms.Button
        Me.cmdTransfer = New System.Windows.Forms.Button
        Me.cmdCredit = New System.Windows.Forms.Button
        Me.cmdReceive = New System.Windows.Forms.Button
        Me.cmdItemCheck = New System.Windows.Forms.Button
        Me.cmdPrintSign = New System.Windows.Forms.Button
        Me.cmdCycleTeam = New System.Windows.Forms.Button
        Me.cmdCycleLocation = New System.Windows.Forms.Button
        Me.cmdInventory = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.cmdClose)
        Me.mainMenu1.MenuItems.Add(Me.MenuInfo)
        '
        'cmdClose
        '
        Me.cmdClose.Text = "Exit"
        '
        'MenuInfo
        '
        Me.MenuInfo.Text = "Info"
        '
        'cmdShrink
        '
        Me.cmdShrink.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.cmdShrink.Location = New System.Drawing.Point(3, 107)
        Me.cmdShrink.Name = "cmdShrink"
        Me.cmdShrink.Size = New System.Drawing.Size(105, 25)
        Me.cmdShrink.TabIndex = 0
        Me.cmdShrink.Text = "Shrink"
        '
        'StoreComboBox
        '
        Me.StoreComboBox.Location = New System.Drawing.Point(3, 51)
        Me.StoreComboBox.Name = "StoreComboBox"
        Me.StoreComboBox.Size = New System.Drawing.Size(234, 22)
        Me.StoreComboBox.TabIndex = 1
        '
        'SubteamComboBox
        '
        Me.SubteamComboBox.Location = New System.Drawing.Point(3, 79)
        Me.SubteamComboBox.Name = "SubteamComboBox"
        Me.SubteamComboBox.Size = New System.Drawing.Size(234, 22)
        Me.SubteamComboBox.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.BackColor = System.Drawing.Color.LightGray
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(0, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(240, 35)
        Me.Label1.Text = "Set your store and subteam, then select a function"
        '
        'cmdOrder
        '
        Me.cmdOrder.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.cmdOrder.Location = New System.Drawing.Point(128, 107)
        Me.cmdOrder.Name = "cmdOrder"
        Me.cmdOrder.Size = New System.Drawing.Size(109, 25)
        Me.cmdOrder.TabIndex = 3
        Me.cmdOrder.Text = "Order"
        '
        'cmdTransfer
        '
        Me.cmdTransfer.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.cmdTransfer.Location = New System.Drawing.Point(128, 138)
        Me.cmdTransfer.Name = "cmdTransfer"
        Me.cmdTransfer.Size = New System.Drawing.Size(109, 25)
        Me.cmdTransfer.TabIndex = 5
        Me.cmdTransfer.Text = "Transfer"
        '
        'cmdCredit
        '
        Me.cmdCredit.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.cmdCredit.Location = New System.Drawing.Point(128, 169)
        Me.cmdCredit.Name = "cmdCredit"
        Me.cmdCredit.Size = New System.Drawing.Size(109, 25)
        Me.cmdCredit.TabIndex = 6
        Me.cmdCredit.Text = "Credit"
        '
        'cmdReceive
        '
        Me.cmdReceive.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.cmdReceive.Location = New System.Drawing.Point(3, 138)
        Me.cmdReceive.Name = "cmdReceive"
        Me.cmdReceive.Size = New System.Drawing.Size(105, 25)
        Me.cmdReceive.TabIndex = 7
        Me.cmdReceive.Text = "Receive"
        '
        'cmdItemCheck
        '
        Me.cmdItemCheck.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.cmdItemCheck.Location = New System.Drawing.Point(128, 200)
        Me.cmdItemCheck.Name = "cmdItemCheck"
        Me.cmdItemCheck.Size = New System.Drawing.Size(109, 25)
        Me.cmdItemCheck.TabIndex = 8
        Me.cmdItemCheck.Text = "Item Check"
        '
        'cmdPrintSign
        '
        Me.cmdPrintSign.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.cmdPrintSign.Location = New System.Drawing.Point(3, 169)
        Me.cmdPrintSign.Name = "cmdPrintSign"
        Me.cmdPrintSign.Size = New System.Drawing.Size(105, 25)
        Me.cmdPrintSign.TabIndex = 9
        Me.cmdPrintSign.Text = "Print Sign"
        Me.cmdPrintSign.Visible = False
        '
        'cmdCycleTeam
        '
        Me.cmdCycleTeam.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold)
        Me.cmdCycleTeam.Location = New System.Drawing.Point(128, 231)
        Me.cmdCycleTeam.Name = "cmdCycleTeam"
        Me.cmdCycleTeam.Size = New System.Drawing.Size(109, 25)
        Me.cmdCycleTeam.TabIndex = 10
        Me.cmdCycleTeam.Text = "CycleCount SubTeam"
        '
        'cmdCycleLocation
        '
        Me.cmdCycleLocation.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold)
        Me.cmdCycleLocation.Location = New System.Drawing.Point(3, 231)
        Me.cmdCycleLocation.Name = "cmdCycleLocation"
        Me.cmdCycleLocation.Size = New System.Drawing.Size(105, 25)
        Me.cmdCycleLocation.TabIndex = 11
        Me.cmdCycleLocation.Text = "CycleCount Location"
        '
        'cmdInventory
        '
        Me.cmdInventory.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.cmdInventory.Location = New System.Drawing.Point(3, 200)
        Me.cmdInventory.Name = "cmdInventory"
        Me.cmdInventory.Size = New System.Drawing.Size(105, 25)
        Me.cmdInventory.TabIndex = 12
        Me.cmdInventory.Text = "Inventory"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.cmdInventory)
        Me.Controls.Add(Me.cmdCycleLocation)
        Me.Controls.Add(Me.cmdCycleTeam)
        Me.Controls.Add(Me.cmdPrintSign)
        Me.Controls.Add(Me.cmdItemCheck)
        Me.Controls.Add(Me.cmdReceive)
        Me.Controls.Add(Me.cmdCredit)
        Me.Controls.Add(Me.cmdTransfer)
        Me.Controls.Add(Me.cmdOrder)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.SubteamComboBox)
        Me.Controls.Add(Me.StoreComboBox)
        Me.Controls.Add(Me.cmdShrink)
        Me.Menu = Me.mainMenu1
        Me.Name = "MainForm"
        Me.Text = "IRMA Main Menu"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents cmdShrink As System.Windows.Forms.Button
    Friend WithEvents StoreComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents SubteamComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents cmdClose As System.Windows.Forms.MenuItem
    Friend WithEvents cmdOrder As System.Windows.Forms.Button
    Friend WithEvents cmdTransfer As System.Windows.Forms.Button
    Friend WithEvents cmdCredit As System.Windows.Forms.Button
    Friend WithEvents cmdReceive As System.Windows.Forms.Button
    Friend WithEvents cmdItemCheck As System.Windows.Forms.Button
    Friend WithEvents cmdPrintSign As System.Windows.Forms.Button
    Friend WithEvents cmdCycleTeam As System.Windows.Forms.Button
    Friend WithEvents cmdCycleLocation As System.Windows.Forms.Button
    Friend WithEvents cmdInventory As System.Windows.Forms.Button
    Friend WithEvents MenuInfo As System.Windows.Forms.MenuItem
End Class
