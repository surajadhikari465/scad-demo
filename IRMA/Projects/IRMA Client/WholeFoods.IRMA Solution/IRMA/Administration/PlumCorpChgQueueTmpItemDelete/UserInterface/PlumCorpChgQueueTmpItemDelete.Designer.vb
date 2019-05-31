<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PlumCorpChgQueueTmpItemDelete
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.DeleteItemsBtn = New System.Windows.Forms.Button()
        Me.ItemKeysTextBox = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'DeleteItemsBtn
        '
        Me.DeleteItemsBtn.Location = New System.Drawing.Point(333, 533)
        Me.DeleteItemsBtn.Name = "DeleteItemsBtn"
        Me.DeleteItemsBtn.Size = New System.Drawing.Size(160, 28)
        Me.DeleteItemsBtn.TabIndex = 7
        Me.DeleteItemsBtn.Text = "Delete ItemKeys"
        Me.DeleteItemsBtn.UseVisualStyleBackColor = True
        '
        'ItemKeysTextBox
        '
        Me.ItemKeysTextBox.Location = New System.Drawing.Point(4, 37)
        Me.ItemKeysTextBox.Multiline = True
        Me.ItemKeysTextBox.Name = "ItemKeysTextBox"
        Me.ItemKeysTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.ItemKeysTextBox.Size = New System.Drawing.Size(485, 485)
        Me.ItemKeysTextBox.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(2, 539)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(291, 17)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Enter Item Key (one per line, maximum 1000)"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(4, 16)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(80, 16)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Item Keys:"
        '
        'PlumCorpChgQueueTmpItemDelete
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(497, 568)
        Me.Controls.Add(Me.DeleteItemsBtn)
        Me.Controls.Add(Me.ItemKeysTextBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "PlumCorpChgQueueTmpItemDelete"
        Me.Text = "PlumCorpChgQueueTmp Item Delete"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents DeleteItemsBtn As Button
    Friend WithEvents ItemKeysTextBox As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
End Class
