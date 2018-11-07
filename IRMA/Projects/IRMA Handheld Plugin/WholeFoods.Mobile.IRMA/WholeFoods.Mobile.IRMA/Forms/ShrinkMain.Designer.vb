<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ShrinkMain
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
        Me.StoreTeamLabel = New System.Windows.Forms.Label
        Me.ShrinkComboBox = New System.Windows.Forms.ComboBox
        Me.StoreTeamLabel2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.ShrinkLabel = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
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
        'StoreTeamLabel
        '
        Me.StoreTeamLabel.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.StoreTeamLabel.Location = New System.Drawing.Point(86, 94)
        Me.StoreTeamLabel.Name = "StoreTeamLabel"
        Me.StoreTeamLabel.Size = New System.Drawing.Size(147, 33)
        Me.StoreTeamLabel.Text = "SubTeamLabel"
        '
        'ShrinkComboBox
        '
        Me.ShrinkComboBox.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.ShrinkComboBox.Location = New System.Drawing.Point(86, 135)
        Me.ShrinkComboBox.Name = "ShrinkComboBox"
        Me.ShrinkComboBox.Size = New System.Drawing.Size(147, 22)
        Me.ShrinkComboBox.TabIndex = 3
        '
        'StoreTeamLabel2
        '
        Me.StoreTeamLabel2.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.StoreTeamLabel2.Location = New System.Drawing.Point(86, 50)
        Me.StoreTeamLabel2.Name = "StoreTeamLabel2"
        Me.StoreTeamLabel2.Size = New System.Drawing.Size(147, 33)
        Me.StoreTeamLabel2.Text = "StoreLabel"
        '
        'Label3
        '
        Me.Label3.Location = New System.Drawing.Point(3, 137)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(77, 20)
        Me.Label3.Text = "Shrink Type:"
        '
        'ShrinkLabel
        '
        Me.ShrinkLabel.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.ShrinkLabel.Location = New System.Drawing.Point(36, 234)
        Me.ShrinkLabel.Name = "ShrinkLabel"
        Me.ShrinkLabel.Size = New System.Drawing.Size(167, 20)
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(3, 50)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(77, 20)
        Me.Label1.Text = "Store:"
        '
        'Label2
        '
        Me.Label2.Location = New System.Drawing.Point(3, 94)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(77, 20)
        Me.Label2.Text = "Subteam:"
        '
        'Label4
        '
        Me.Label4.BackColor = System.Drawing.Color.Silver
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.Location = New System.Drawing.Point(0, 4)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(240, 20)
        Me.Label4.Text = "IRMA Shrink"
        '
        'ShrinkMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ShrinkLabel)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.StoreTeamLabel2)
        Me.Controls.Add(Me.ShrinkComboBox)
        Me.Controls.Add(Me.StoreTeamLabel)
        Me.Menu = Me.mainMenu1
        Me.Name = "ShrinkMain"
        Me.Text = "Shrink Main"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents StoreTeamLabel As System.Windows.Forms.Label
    Friend WithEvents ShrinkComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents StoreTeamLabel2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents mnuCancel As System.Windows.Forms.MenuItem
    Friend WithEvents mnuNext As System.Windows.Forms.MenuItem
    Friend WithEvents ShrinkLabel As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
End Class
