<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class AddInvoiceCharge
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AddInvoiceCharge))
        Me.LabelHeader = New System.Windows.Forms.Label
        Me.ButtonAddCharge = New System.Windows.Forms.Button
        Me.ButtonCancel = New System.Windows.Forms.Button
        Me.ComboBoxChargeDescription = New System.Windows.Forms.ComboBox
        Me.TextBoxChargeValue = New System.Windows.Forms.TextBox
        Me.LabelAmount = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.RadioButtonNonAllocated = New System.Windows.Forms.RadioButton
        Me.RadioButtonAllocatedCharge = New System.Windows.Forms.RadioButton
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'LabelHeader
        '
        Me.LabelHeader.BackColor = System.Drawing.Color.Green
        resources.ApplyResources(Me.LabelHeader, "LabelHeader")
        Me.LabelHeader.ForeColor = System.Drawing.Color.White
        Me.LabelHeader.Name = "LabelHeader"
        '
        'ButtonAddCharge
        '
        resources.ApplyResources(Me.ButtonAddCharge, "ButtonAddCharge")
        Me.ButtonAddCharge.BackColor = System.Drawing.Color.YellowGreen
        Me.ButtonAddCharge.Name = "ButtonAddCharge"
        '
        'ButtonCancel
        '
        resources.ApplyResources(Me.ButtonCancel, "ButtonCancel")
        Me.ButtonCancel.BackColor = System.Drawing.Color.YellowGreen
        Me.ButtonCancel.Name = "ButtonCancel"
        '
        'ComboBoxChargeDescription
        '
        resources.ApplyResources(Me.ComboBoxChargeDescription, "ComboBoxChargeDescription")
        Me.ComboBoxChargeDescription.Name = "ComboBoxChargeDescription"
        '
        'TextBoxChargeValue
        '
        resources.ApplyResources(Me.TextBoxChargeValue, "TextBoxChargeValue")
        Me.TextBoxChargeValue.Name = "TextBoxChargeValue"
        '
        'LabelAmount
        '
        resources.ApplyResources(Me.LabelAmount, "LabelAmount")
        Me.LabelAmount.Name = "LabelAmount"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.White
        Me.Panel1.Controls.Add(Me.RadioButtonNonAllocated)
        Me.Panel1.Controls.Add(Me.RadioButtonAllocatedCharge)
        resources.ApplyResources(Me.Panel1, "Panel1")
        Me.Panel1.Name = "Panel1"
        '
        'RadioButtonNonAllocated
        '
        resources.ApplyResources(Me.RadioButtonNonAllocated, "RadioButtonNonAllocated")
        Me.RadioButtonNonAllocated.Name = "RadioButtonNonAllocated"
        '
        'RadioButtonAllocatedCharge
        '
        resources.ApplyResources(Me.RadioButtonAllocatedCharge, "RadioButtonAllocatedCharge")
        Me.RadioButtonAllocatedCharge.Name = "RadioButtonAllocatedCharge"
        '
        'AddInvoiceCharge
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        resources.ApplyResources(Me, "$this")
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ControlBox = False
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.LabelAmount)
        Me.Controls.Add(Me.TextBoxChargeValue)
        Me.Controls.Add(Me.ComboBoxChargeDescription)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.ButtonAddCharge)
        Me.Controls.Add(Me.LabelHeader)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MinimizeBox = False
        Me.Name = "AddInvoiceCharge"
        Me.TopMost = True
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LabelHeader As System.Windows.Forms.Label
    Friend WithEvents ButtonAddCharge As System.Windows.Forms.Button
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents ComboBoxChargeDescription As System.Windows.Forms.ComboBox
    Friend WithEvents TextBoxChargeValue As System.Windows.Forms.TextBox
    Friend WithEvents LabelAmount As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents RadioButtonNonAllocated As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButtonAllocatedCharge As System.Windows.Forms.RadioButton
End Class
