<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Promotions_TestForm
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

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Button_TestItemGroupEditor = New System.Windows.Forms.Button
        Me.Button_TestPromotionOfferEditor = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'Button_TestItemGroupEditor
        '
        Me.Button_TestItemGroupEditor.Location = New System.Drawing.Point(62, 56)
        Me.Button_TestItemGroupEditor.Name = "Button_TestItemGroupEditor"
        Me.Button_TestItemGroupEditor.Size = New System.Drawing.Size(207, 27)
        Me.Button_TestItemGroupEditor.TabIndex = 0
        Me.Button_TestItemGroupEditor.Text = "Test Item Group Editor"
        Me.Button_TestItemGroupEditor.UseVisualStyleBackColor = True
        '
        'Button_TestPromotionOfferEditor
        '
        Me.Button_TestPromotionOfferEditor.Location = New System.Drawing.Point(62, 12)
        Me.Button_TestPromotionOfferEditor.Name = "Button_TestPromotionOfferEditor"
        Me.Button_TestPromotionOfferEditor.Size = New System.Drawing.Size(207, 29)
        Me.Button_TestPromotionOfferEditor.TabIndex = 1
        Me.Button_TestPromotionOfferEditor.Text = "Test Pomotion Offer Editor"
        Me.Button_TestPromotionOfferEditor.UseVisualStyleBackColor = True
        '
        'TestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(336, 95)
        Me.Controls.Add(Me.Button_TestPromotionOfferEditor)
        Me.Controls.Add(Me.Button_TestItemGroupEditor)
        Me.Name = "TestForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "TestForm"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button_TestItemGroupEditor As System.Windows.Forms.Button
    Friend WithEvents Button_TestPromotionOfferEditor As System.Windows.Forms.Button
End Class
