<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SetHierarchyPositionForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SetHierarchyPositionForm))
        Me.Label1 = New System.Windows.Forms.Label
        Me.LabelItemIdentifier = New System.Windows.Forms.Label
        Me.HierarchySelectorForItem = New HierarchySelector
        Me.ButtonCancel = New System.Windows.Forms.Button
        Me.ButtonOK = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(22, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(35, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Item:"
        '
        'LabelItemIdentifier
        '
        Me.LabelItemIdentifier.Location = New System.Drawing.Point(58, 23)
        Me.LabelItemIdentifier.Name = "LabelItemIdentifier"
        Me.LabelItemIdentifier.Size = New System.Drawing.Size(175, 20)
        Me.LabelItemIdentifier.TabIndex = 1
        '
        'HierarchySelectorForItem
        '
        Me.HierarchySelectorForItem.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold)
        Me.HierarchySelectorForItem.ItemIdentifier = Nothing
        Me.HierarchySelectorForItem.Location = New System.Drawing.Point(12, 46)
        Me.HierarchySelectorForItem.Name = "HierarchySelectorForItem"
        Me.HierarchySelectorForItem.SelectedCategoryId = 0
        Me.HierarchySelectorForItem.SelectedLevel3Id = 0
        Me.HierarchySelectorForItem.SelectedLevel4Id = 0
        Me.HierarchySelectorForItem.SelectedSubTeamId = 0
        Me.HierarchySelectorForItem.Size = New System.Drawing.Size(399, 122)
        Me.HierarchySelectorForItem.TabIndex = 5
        '
        'ButtonCancel
        '
        Me.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.ButtonCancel.Location = New System.Drawing.Point(331, 174)
        Me.ButtonCancel.Name = "ButtonCancel"
        Me.ButtonCancel.Size = New System.Drawing.Size(75, 23)
        Me.ButtonCancel.TabIndex = 9
        Me.ButtonCancel.Text = "Cancel"
        Me.ButtonCancel.UseVisualStyleBackColor = True
        '
        'ButtonOK
        '
        Me.ButtonOK.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.ButtonOK.Location = New System.Drawing.Point(250, 174)
        Me.ButtonOK.Name = "ButtonOK"
        Me.ButtonOK.Size = New System.Drawing.Size(75, 23)
        Me.ButtonOK.TabIndex = 8
        Me.ButtonOK.Text = "OK"
        Me.ButtonOK.UseVisualStyleBackColor = True
        '
        'SetHierarchyPositionForm
        '
        Me.AcceptButton = Me.ButtonOK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.ButtonCancel
        Me.ClientSize = New System.Drawing.Size(415, 209)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.ButtonOK)
        Me.Controls.Add(Me.HierarchySelectorForItem)
        Me.Controls.Add(Me.LabelItemIdentifier)
        Me.Controls.Add(Me.Label1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "SetHierarchyPositionForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "EIM - Set Hierarchy Position for Item"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents LabelItemIdentifier As System.Windows.Forms.Label
    Friend WithEvents HierarchySelectorForItem As HierarchySelector
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents ButtonOK As System.Windows.Forms.Button
End Class
