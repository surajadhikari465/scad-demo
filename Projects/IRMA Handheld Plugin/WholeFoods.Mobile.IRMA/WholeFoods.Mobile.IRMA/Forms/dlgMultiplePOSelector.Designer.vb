<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class dlgMultiplePOSelector
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(dlgMultiplePOSelector))
        Me.DataGridPOSelector = New System.Windows.Forms.DataGrid
        Me.DataGridTableStylePOSelector = New System.Windows.Forms.DataGridTableStyle
        Me.DataGridTextBoxColumnIRMAPO = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumnSource = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumnVendor = New System.Windows.Forms.DataGridTextBoxColumn
        Me.ButtonCancel = New System.Windows.Forms.Button
        Me.LabelHeader = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'DataGridPOSelector
        '
        Me.DataGridPOSelector.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        resources.ApplyResources(Me.DataGridPOSelector, "DataGridPOSelector")
        Me.DataGridPOSelector.Name = "DataGridPOSelector"
        Me.DataGridPOSelector.RowHeadersVisible = False
        Me.DataGridPOSelector.TableStyles.Add(Me.DataGridTableStylePOSelector)
        '
        'DataGridTableStylePOSelector
        '
        Me.DataGridTableStylePOSelector.GridColumnStyles.Add(Me.DataGridTextBoxColumnIRMAPO)
        Me.DataGridTableStylePOSelector.GridColumnStyles.Add(Me.DataGridTextBoxColumnSource)
        Me.DataGridTableStylePOSelector.GridColumnStyles.Add(Me.DataGridTextBoxColumnVendor)
        Me.DataGridTableStylePOSelector.MappingName = "POSelector"
        '
        'DataGridTextBoxColumnIRMAPO
        '
        Me.DataGridTextBoxColumnIRMAPO.Format = ""
        Me.DataGridTextBoxColumnIRMAPO.FormatInfo = Nothing
        resources.ApplyResources(Me.DataGridTextBoxColumnIRMAPO, "DataGridTextBoxColumnIRMAPO")
        '
        'DataGridTextBoxColumnSource
        '
        Me.DataGridTextBoxColumnSource.Format = ""
        Me.DataGridTextBoxColumnSource.FormatInfo = Nothing
        resources.ApplyResources(Me.DataGridTextBoxColumnSource, "DataGridTextBoxColumnSource")
        '
        'DataGridTextBoxColumnVendor
        '
        Me.DataGridTextBoxColumnVendor.Format = ""
        Me.DataGridTextBoxColumnVendor.FormatInfo = Nothing
        resources.ApplyResources(Me.DataGridTextBoxColumnVendor, "DataGridTextBoxColumnVendor")
        '
        'ButtonCancel
        '
        resources.ApplyResources(Me.ButtonCancel, "ButtonCancel")
        Me.ButtonCancel.BackColor = System.Drawing.Color.YellowGreen
        Me.ButtonCancel.Name = "ButtonCancel"
        '
        'LabelHeader
        '
        Me.LabelHeader.BackColor = System.Drawing.Color.Green
        resources.ApplyResources(Me.LabelHeader, "LabelHeader")
        Me.LabelHeader.ForeColor = System.Drawing.Color.White
        Me.LabelHeader.Name = "LabelHeader"
        '
        'dlgMultiplePOSelector
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        resources.ApplyResources(Me, "$this")
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ControlBox = False
        Me.Controls.Add(Me.LabelHeader)
        Me.Controls.Add(Me.ButtonCancel)
        Me.Controls.Add(Me.DataGridPOSelector)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MinimizeBox = False
        Me.Name = "dlgMultiplePOSelector"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents ButtonCancel As System.Windows.Forms.Button
    Friend WithEvents DataGridTableStylePOSelector As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumnIRMAPO As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumnSource As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumnVendor As System.Windows.Forms.DataGridTextBoxColumn
    Public WithEvents DataGridPOSelector As System.Windows.Forms.DataGrid
    Friend WithEvents LabelHeader As System.Windows.Forms.Label
End Class
