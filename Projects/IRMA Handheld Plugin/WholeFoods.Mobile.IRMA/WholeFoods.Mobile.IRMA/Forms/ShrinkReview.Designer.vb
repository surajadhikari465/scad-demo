<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ShrinkReview
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
        Me.BackMenuItem = New System.Windows.Forms.MenuItem
        Me.UploadMenuItem = New System.Windows.Forms.MenuItem
        Me.DataGrid1 = New System.Windows.Forms.DataGrid
        Me.DeleteButton = New System.Windows.Forms.Button
        Me.StoreTeamLabel = New System.Windows.Forms.Label
        Me.UpdateButton = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.BackMenuItem)
        Me.mainMenu1.MenuItems.Add(Me.UploadMenuItem)
        '
        'BackMenuItem
        '
        Me.BackMenuItem.Text = "Back"
        '
        'UploadMenuItem
        '
        Me.UploadMenuItem.Text = "Upload"
        '
        'DataGrid1
        '
        Me.DataGrid1.BackgroundColor = System.Drawing.Color.Silver
        Me.DataGrid1.Location = New System.Drawing.Point(4, 30)
        Me.DataGrid1.Name = "DataGrid1"
        Me.DataGrid1.Size = New System.Drawing.Size(233, 185)
        Me.DataGrid1.TabIndex = 0
        '
        'DeleteButton
        '
        Me.DeleteButton.Location = New System.Drawing.Point(129, 235)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(80, 30)
        Me.DeleteButton.TabIndex = 1
        Me.DeleteButton.Text = "Remove"
        '
        'StoreTeamLabel
        '
        Me.StoreTeamLabel.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.StoreTeamLabel.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.StoreTeamLabel.Location = New System.Drawing.Point(0, 0)
        Me.StoreTeamLabel.Name = "StoreTeamLabel"
        Me.StoreTeamLabel.Size = New System.Drawing.Size(240, 26)
        '
        'UpdateButton
        '
        Me.UpdateButton.Location = New System.Drawing.Point(21, 235)
        Me.UpdateButton.Name = "UpdateButton"
        Me.UpdateButton.Size = New System.Drawing.Size(80, 30)
        Me.UpdateButton.TabIndex = 2
        Me.UpdateButton.Text = "Update"
        '
        'ShrinkReview
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.ControlBox = False
        Me.Controls.Add(Me.UpdateButton)
        Me.Controls.Add(Me.StoreTeamLabel)
        Me.Controls.Add(Me.DeleteButton)
        Me.Controls.Add(Me.DataGrid1)
        Me.Menu = Me.mainMenu1
        Me.Name = "ShrinkReview"
        Me.Text = "Review Shrink"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents DataGrid1 As System.Windows.Forms.DataGrid
    Friend WithEvents BackMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents UploadMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents DeleteButton As System.Windows.Forms.Button
    Friend WithEvents StoreTeamLabel As System.Windows.Forms.Label
    Friend WithEvents UpdateButton As System.Windows.Forms.Button
End Class
