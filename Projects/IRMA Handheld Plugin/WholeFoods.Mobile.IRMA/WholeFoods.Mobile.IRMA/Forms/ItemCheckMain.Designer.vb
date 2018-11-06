<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ItemCheckMain
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
        Me.mnuCancel = New System.Windows.Forms.MenuItem
        Me.mnuNext = New System.Windows.Forms.MenuItem
        Me.lblUPC = New System.Windows.Forms.Label
        Me.txtUPC = New System.Windows.Forms.TextBox
        Me.lblHelp = New System.Windows.Forms.Label
        Me.lblStore = New System.Windows.Forms.Label
        Me.lblSubTeam = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.mnuCancel)
        Me.mainMenu1.MenuItems.Add(Me.mnuNext)
        '
        'mnuCancel
        '
        Me.mnuCancel.Text = "Cancel"
        '
        'mnuNext
        '
        Me.mnuNext.Text = "Next"
        '
        'lblUPC
        '
        Me.lblUPC.Location = New System.Drawing.Point(4, 131)
        Me.lblUPC.Name = "lblUPC"
        Me.lblUPC.Size = New System.Drawing.Size(59, 20)
        Me.lblUPC.Text = "UPC:"
        '
        'txtUPC
        '
        Me.txtUPC.Location = New System.Drawing.Point(62, 131)
        Me.txtUPC.Name = "txtUPC"
        Me.txtUPC.Size = New System.Drawing.Size(171, 21)
        Me.txtUPC.TabIndex = 11
        '
        'lblHelp
        '
        Me.lblHelp.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Regular)
        Me.lblHelp.Location = New System.Drawing.Point(4, 49)
        Me.lblHelp.Name = "lblHelp"
        Me.lblHelp.Size = New System.Drawing.Size(229, 64)
        Me.lblHelp.Text = "SCAN ITEM OR MANUALLY ENTER CODE"
        Me.lblHelp.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'lblStore
        '
        Me.lblStore.BackColor = System.Drawing.Color.Silver
        Me.lblStore.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblStore.Location = New System.Drawing.Point(0, 4)
        Me.lblStore.Name = "lblStore"
        Me.lblStore.Size = New System.Drawing.Size(118, 20)
        Me.lblStore.Text = "Store"
        '
        'lblSubTeam
        '
        Me.lblSubTeam.BackColor = System.Drawing.Color.Silver
        Me.lblSubTeam.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.lblSubTeam.Location = New System.Drawing.Point(122, 4)
        Me.lblSubTeam.Name = "lblSubTeam"
        Me.lblSubTeam.Size = New System.Drawing.Size(118, 20)
        Me.lblSubTeam.Text = "SubTeam"
        '
        'ItemCheckMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.lblSubTeam)
        Me.Controls.Add(Me.lblHelp)
        Me.Controls.Add(Me.txtUPC)
        Me.Controls.Add(Me.lblUPC)
        Me.Controls.Add(Me.lblStore)
        Me.Menu = Me.mainMenu1
        Me.Name = "ItemCheckMain"
        Me.Text = "Item Check"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents mnuCancel As System.Windows.Forms.MenuItem
    Friend WithEvents mnuNext As System.Windows.Forms.MenuItem
    Friend WithEvents lblUPC As System.Windows.Forms.Label
    Friend WithEvents txtUPC As System.Windows.Forms.TextBox
    Friend WithEvents lblHelp As System.Windows.Forms.Label
    Friend WithEvents lblStore As System.Windows.Forms.Label
    Friend WithEvents lblSubTeam As System.Windows.Forms.Label
End Class
