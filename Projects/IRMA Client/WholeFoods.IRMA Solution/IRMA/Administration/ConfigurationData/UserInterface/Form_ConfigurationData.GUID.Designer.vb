<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ConfigurationData_GUID
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
        Me._textGUID = New System.Windows.Forms.TextBox
        Me._labelGUID = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        '_textGUID
        '
        Me._textGUID.Location = New System.Drawing.Point(13, 29)
        Me._textGUID.Name = "_textGUID"
        Me._textGUID.ReadOnly = True
        Me._textGUID.Size = New System.Drawing.Size(313, 22)
        Me._textGUID.TabIndex = 0
        '
        '_labelGUID
        '
        Me._labelGUID.AutoSize = True
        Me._labelGUID.Location = New System.Drawing.Point(13, 13)
        Me._labelGUID.Name = "_labelGUID"
        Me._labelGUID.Size = New System.Drawing.Size(19, 13)
        Me._labelGUID.TabIndex = 1
        Me._labelGUID.Text = "{0}"
        '
        'Form_ConfigurationData_GUID
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(338, 69)
        Me.Controls.Add(Me._labelGUID)
        Me.Controls.Add(Me._textGUID)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_ConfigurationData_GUID"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Identification GUID"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents _textGUID As System.Windows.Forms.TextBox
    Friend WithEvents _labelGUID As System.Windows.Forms.Label
End Class
