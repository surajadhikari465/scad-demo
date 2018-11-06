<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ReceiveDocumentReview
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
        Me.StoreTeamLabel = New System.Windows.Forms.Label
        Me.DataGrid1 = New System.Windows.Forms.DataGrid
        Me.UpdateButton = New System.Windows.Forms.Button
        Me.DeleteButton = New System.Windows.Forms.Button
        Me.totalCount = New System.Windows.Forms.Label
        Me.Count = New System.Windows.Forms.Label
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
        Me.UploadMenuItem.Text = "Finish"
        '
        'StoreTeamLabel
        '
        Me.StoreTeamLabel.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.StoreTeamLabel.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Bold)
        Me.StoreTeamLabel.Location = New System.Drawing.Point(0, 0)
        Me.StoreTeamLabel.Name = "StoreTeamLabel"
        Me.StoreTeamLabel.Size = New System.Drawing.Size(240, 20)
        '
        'DataGrid1
        '
        Me.DataGrid1.BackgroundColor = System.Drawing.Color.Silver
        Me.DataGrid1.Location = New System.Drawing.Point(4, 43)
        Me.DataGrid1.Name = "DataGrid1"
        Me.DataGrid1.Size = New System.Drawing.Size(234, 185)
        Me.DataGrid1.TabIndex = 4
        '
        'UpdateButton
        '
        Me.UpdateButton.Location = New System.Drawing.Point(18, 234)
        Me.UpdateButton.Name = "UpdateButton"
        Me.UpdateButton.Size = New System.Drawing.Size(80, 31)
        Me.UpdateButton.TabIndex = 5
        Me.UpdateButton.Text = "Update"
        '
        'DeleteButton
        '
        Me.DeleteButton.Location = New System.Drawing.Point(139, 234)
        Me.DeleteButton.Name = "DeleteButton"
        Me.DeleteButton.Size = New System.Drawing.Size(80, 30)
        Me.DeleteButton.TabIndex = 6
        Me.DeleteButton.Text = "Remove"
        '
        'totalCount
        '
        Me.totalCount.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.totalCount.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.totalCount.Location = New System.Drawing.Point(0, 20)
        Me.totalCount.Name = "totalCount"
        Me.totalCount.Size = New System.Drawing.Size(70, 20)
        Me.totalCount.Text = "Total Count:"
        '
        'Count
        '
        Me.Count.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Count.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Regular)
        Me.Count.Location = New System.Drawing.Point(69, 20)
        Me.Count.Name = "Count"
        Me.Count.Size = New System.Drawing.Size(171, 20)
        '
        'ReceiveDocumentReview
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.Count)
        Me.Controls.Add(Me.totalCount)
        Me.Controls.Add(Me.DeleteButton)
        Me.Controls.Add(Me.UpdateButton)
        Me.Controls.Add(Me.DataGrid1)
        Me.Controls.Add(Me.StoreTeamLabel)
        Me.KeyPreview = True
        Me.Menu = Me.mainMenu1
        Me.Name = "ReceiveDocumentReview"
        Me.Text = "RD Review"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents StoreTeamLabel As System.Windows.Forms.Label
    Friend WithEvents DataGrid1 As System.Windows.Forms.DataGrid
    Friend WithEvents UpdateButton As System.Windows.Forms.Button
    Friend WithEvents DeleteButton As System.Windows.Forms.Button
    Friend WithEvents BackMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents UploadMenuItem As System.Windows.Forms.MenuItem
    Friend WithEvents totalCount As System.Windows.Forms.Label
    Friend WithEvents Count As System.Windows.Forms.Label
End Class
