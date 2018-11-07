<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SupportRestoreDeletedItems
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.ItemIdentifiersTextBox = New System.Windows.Forms.TextBox()
        Me.RestoreItemsBtn = New System.Windows.Forms.Button()
        Me.ClearRestoreItemKeysTableBtn = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(118, 16)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Item Identifiers:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 532)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(328, 17)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Enter Item Identifiers (one per line, maximum 1000)"
        '
        'ItemIdentifiersTextBox
        '
        Me.ItemIdentifiersTextBox.Location = New System.Drawing.Point(15, 30)
        Me.ItemIdentifiersTextBox.Multiline = True
        Me.ItemIdentifiersTextBox.Name = "ItemIdentifiersTextBox"
        Me.ItemIdentifiersTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.ItemIdentifiersTextBox.Size = New System.Drawing.Size(485, 485)
        Me.ItemIdentifiersTextBox.TabIndex = 2
        '
        'RestoreItemsBtn
        '
        Me.RestoreItemsBtn.Location = New System.Drawing.Point(340, 526)
        Me.RestoreItemsBtn.Name = "RestoreItemsBtn"
        Me.RestoreItemsBtn.Size = New System.Drawing.Size(160, 28)
        Me.RestoreItemsBtn.TabIndex = 3
        Me.RestoreItemsBtn.Text = "Restore Items"
        Me.RestoreItemsBtn.UseVisualStyleBackColor = True
        '
        'ClearRestoreItemKeysTableBtn
        '
        Me.ClearRestoreItemKeysTableBtn.Location = New System.Drawing.Point(15, 552)
        Me.ClearRestoreItemKeysTableBtn.Name = "ClearRestoreItemKeysTableBtn"
        Me.ClearRestoreItemKeysTableBtn.Size = New System.Drawing.Size(215, 23)
        Me.ClearRestoreItemKeysTableBtn.TabIndex = 4
        Me.ClearRestoreItemKeysTableBtn.Text = "Clear Restore Item Keys Table"
        Me.ClearRestoreItemKeysTableBtn.UseVisualStyleBackColor = True
        '
        'SupportRestoreDeletedItems
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(525, 580)
        Me.Controls.Add(Me.ClearRestoreItemKeysTableBtn)
        Me.Controls.Add(Me.RestoreItemsBtn)
        Me.Controls.Add(Me.ItemIdentifiersTextBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Name = "SupportRestoreDeletedItems"
        Me.Text = "Restore Deleted Items"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents ItemIdentifiersTextBox As TextBox
    Friend WithEvents RestoreItemsBtn As Button
    Friend WithEvents ClearRestoreItemKeysTableBtn As Button
End Class
