<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class SlawPriceRefresh
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
        Me.RefreshItemsBtn = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ItemIdentifiersTextBox = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'RefreshItemsBtn
        '
        Me.RefreshItemsBtn.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RefreshItemsBtn.Location = New System.Drawing.Point(262, 426)
        Me.RefreshItemsBtn.Name = "RefreshItemsBtn"
        Me.RefreshItemsBtn.Size = New System.Drawing.Size(120, 23)
        Me.RefreshItemsBtn.TabIndex = 2
        Me.RefreshItemsBtn.Text = "Refresh Items"
        Me.RefreshItemsBtn.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(95, 14)
        Me.Label1.TabIndex = 3
        Me.Label1.Text = "Item Identifiers:"
        '
        'ItemIdentifiersTextBox
        '
        Me.ItemIdentifiersTextBox.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ItemIdentifiersTextBox.Location = New System.Drawing.Point(15, 26)
        Me.ItemIdentifiersTextBox.Multiline = True
        Me.ItemIdentifiersTextBox.Name = "ItemIdentifiersTextBox"
        Me.ItemIdentifiersTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical
        Me.ItemIdentifiersTextBox.Size = New System.Drawing.Size(365, 395)
        Me.ItemIdentifiersTextBox.TabIndex = 6
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 431)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(243, 13)
        Me.Label2.TabIndex = 7
        Me.Label2.Text = "Enter Item Identifiers (one per line, maximum 1000)"
        '
        'SlawPriceRefresh
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(394, 453)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.ItemIdentifiersTextBox)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.RefreshItemsBtn)
        Me.MinimumSize = New System.Drawing.Size(410, 491)
        Me.Name = "SlawPriceRefresh"
        Me.Text = "Slaw Price Refresh"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents RefreshItemsBtn As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ItemIdentifiersTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
End Class
