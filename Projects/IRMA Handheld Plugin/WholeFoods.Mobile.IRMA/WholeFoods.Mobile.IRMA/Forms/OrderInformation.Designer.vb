<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class OrderInformation
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(OrderInformation))
        Me.ButtonOk = New System.Windows.Forms.Button
        Me.LabelFormTitle = New System.Windows.Forms.Label
        Me.CheckBoxProduct = New System.Windows.Forms.CheckBox
        Me.CheckBoxCredit = New System.Windows.Forms.CheckBox
        Me.PanelOrderType = New System.Windows.Forms.Panel
        Me.LabelOrderNotesTitle = New System.Windows.Forms.Label
        Me.LabelOrderNotesField = New System.Windows.Forms.Label
        Me.LabelOrderDate = New System.Windows.Forms.Label
        Me.PanelOrderNotes = New System.Windows.Forms.Panel
        Me.PanelBuyer = New System.Windows.Forms.Panel
        Me.LabelBuyerField = New System.Windows.Forms.Label
        Me.LabelBuyerTitle = New System.Windows.Forms.Label
        Me.LabelOrderDateField = New System.Windows.Forms.Label
        Me.PanelOrderType.SuspendLayout()
        Me.PanelOrderNotes.SuspendLayout()
        Me.PanelBuyer.SuspendLayout()
        Me.SuspendLayout()
        '
        'ButtonOk
        '
        resources.ApplyResources(Me.ButtonOk, "ButtonOk")
        Me.ButtonOk.BackColor = System.Drawing.Color.Green
        Me.ButtonOk.ForeColor = System.Drawing.Color.White
        Me.ButtonOk.Name = "ButtonOk"
        '
        'LabelFormTitle
        '
        Me.LabelFormTitle.BackColor = System.Drawing.Color.Green
        resources.ApplyResources(Me.LabelFormTitle, "LabelFormTitle")
        Me.LabelFormTitle.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.LabelFormTitle.Name = "LabelFormTitle"
        '
        'CheckBoxProduct
        '
        resources.ApplyResources(Me.CheckBoxProduct, "CheckBoxProduct")
        Me.CheckBoxProduct.Name = "CheckBoxProduct"
        '
        'CheckBoxCredit
        '
        resources.ApplyResources(Me.CheckBoxCredit, "CheckBoxCredit")
        Me.CheckBoxCredit.Name = "CheckBoxCredit"
        '
        'PanelOrderType
        '
        Me.PanelOrderType.BackColor = System.Drawing.Color.White
        Me.PanelOrderType.Controls.Add(Me.CheckBoxProduct)
        Me.PanelOrderType.Controls.Add(Me.CheckBoxCredit)
        resources.ApplyResources(Me.PanelOrderType, "PanelOrderType")
        Me.PanelOrderType.Name = "PanelOrderType"
        '
        'LabelOrderNotesTitle
        '
        resources.ApplyResources(Me.LabelOrderNotesTitle, "LabelOrderNotesTitle")
        Me.LabelOrderNotesTitle.Name = "LabelOrderNotesTitle"
        '
        'LabelOrderNotesField
        '
        resources.ApplyResources(Me.LabelOrderNotesField, "LabelOrderNotesField")
        Me.LabelOrderNotesField.Name = "LabelOrderNotesField"
        '
        'LabelOrderDate
        '
        resources.ApplyResources(Me.LabelOrderDate, "LabelOrderDate")
        Me.LabelOrderDate.Name = "LabelOrderDate"
        '
        'PanelOrderNotes
        '
        Me.PanelOrderNotes.BackColor = System.Drawing.Color.WhiteSmoke
        Me.PanelOrderNotes.Controls.Add(Me.LabelOrderNotesField)
        Me.PanelOrderNotes.Controls.Add(Me.LabelOrderNotesTitle)
        resources.ApplyResources(Me.PanelOrderNotes, "PanelOrderNotes")
        Me.PanelOrderNotes.Name = "PanelOrderNotes"
        '
        'PanelBuyer
        '
        Me.PanelBuyer.BackColor = System.Drawing.Color.Gainsboro
        Me.PanelBuyer.Controls.Add(Me.LabelOrderDateField)
        Me.PanelBuyer.Controls.Add(Me.LabelBuyerField)
        Me.PanelBuyer.Controls.Add(Me.LabelBuyerTitle)
        Me.PanelBuyer.Controls.Add(Me.LabelOrderDate)
        resources.ApplyResources(Me.PanelBuyer, "PanelBuyer")
        Me.PanelBuyer.Name = "PanelBuyer"
        '
        'LabelBuyerField
        '
        resources.ApplyResources(Me.LabelBuyerField, "LabelBuyerField")
        Me.LabelBuyerField.Name = "LabelBuyerField"
        '
        'LabelBuyerTitle
        '
        resources.ApplyResources(Me.LabelBuyerTitle, "LabelBuyerTitle")
        Me.LabelBuyerTitle.Name = "LabelBuyerTitle"
        '
        'LabelOrderDateField
        '
        resources.ApplyResources(Me.LabelOrderDateField, "LabelOrderDateField")
        Me.LabelOrderDateField.Name = "LabelOrderDateField"
        '
        'OrderInformation
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        resources.ApplyResources(Me, "$this")
        Me.BackColor = System.Drawing.Color.White
        Me.ControlBox = False
        Me.Controls.Add(Me.PanelBuyer)
        Me.Controls.Add(Me.PanelOrderNotes)
        Me.Controls.Add(Me.PanelOrderType)
        Me.Controls.Add(Me.LabelFormTitle)
        Me.Controls.Add(Me.ButtonOk)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MinimizeBox = False
        Me.Name = "OrderInformation"
        Me.TopMost = True
        Me.PanelOrderType.ResumeLayout(False)
        Me.PanelOrderNotes.ResumeLayout(False)
        Me.PanelBuyer.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonOk As System.Windows.Forms.Button
    Friend WithEvents LabelFormTitle As System.Windows.Forms.Label
    Friend WithEvents CheckBoxProduct As System.Windows.Forms.CheckBox
    Friend WithEvents CheckBoxCredit As System.Windows.Forms.CheckBox
    Friend WithEvents PanelOrderType As System.Windows.Forms.Panel
    Friend WithEvents LabelOrderNotesTitle As System.Windows.Forms.Label
    Friend WithEvents LabelOrderNotesField As System.Windows.Forms.Label
    Friend WithEvents LabelOrderDate As System.Windows.Forms.Label
    Friend WithEvents PanelOrderNotes As System.Windows.Forms.Panel
    Friend WithEvents PanelBuyer As System.Windows.Forms.Panel
    Friend WithEvents LabelBuyerField As System.Windows.Forms.Label
    Friend WithEvents LabelBuyerTitle As System.Windows.Forms.Label
    Friend WithEvents LabelOrderDateField As System.Windows.Forms.Label
End Class
