<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ReasonCodeDescription
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ReasonCodeDescription))
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.LabelHeader = New System.Windows.Forms.Label
        Me.ButtonOK = New System.Windows.Forms.Button
        Me.DataGridReasonCode = New System.Windows.Forms.DataGrid
        Me.DataGridTableStyle1 = New System.Windows.Forms.DataGridTableStyle
        Me.DataGridTextBoxColumnCode = New System.Windows.Forms.DataGridTextBoxColumn
        Me.DataGridTextBoxColumnDescription = New System.Windows.Forms.DataGridTextBoxColumn
        Me.SuspendLayout()
        '
        'LabelHeader
        '
        Me.LabelHeader.BackColor = System.Drawing.Color.Green
        resources.ApplyResources(Me.LabelHeader, "LabelHeader")
        Me.LabelHeader.ForeColor = System.Drawing.Color.White
        Me.LabelHeader.Name = "LabelHeader"
        '
        'ButtonOK
        '
        resources.ApplyResources(Me.ButtonOK, "ButtonOK")
        Me.ButtonOK.BackColor = System.Drawing.Color.YellowGreen
        Me.ButtonOK.Name = "ButtonOK"
        '
        'DataGridReasonCode
        '
        Me.DataGridReasonCode.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        resources.ApplyResources(Me.DataGridReasonCode, "DataGridReasonCode")
        Me.DataGridReasonCode.Name = "DataGridReasonCode"
        Me.DataGridReasonCode.RowHeadersVisible = False
        Me.DataGridReasonCode.TableStyles.Add(Me.DataGridTableStyle1)
        '
        'DataGridTableStyle1
        '
        Me.DataGridTableStyle1.GridColumnStyles.Add(Me.DataGridTextBoxColumnCode)
        Me.DataGridTableStyle1.GridColumnStyles.Add(Me.DataGridTextBoxColumnDescription)
        Me.DataGridTableStyle1.MappingName = "ReasonCodes"
        '
        'DataGridTextBoxColumnCode
        '
        Me.DataGridTextBoxColumnCode.Format = ""
        Me.DataGridTextBoxColumnCode.FormatInfo = Nothing
        resources.ApplyResources(Me.DataGridTextBoxColumnCode, "DataGridTextBoxColumnCode")
        '
        'DataGridTextBoxColumnDescription
        '
        Me.DataGridTextBoxColumnDescription.Format = ""
        Me.DataGridTextBoxColumnDescription.FormatInfo = Nothing
        resources.ApplyResources(Me.DataGridTextBoxColumnDescription, "DataGridTextBoxColumnDescription")
        '
        'ReasonCodeDescription
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        resources.ApplyResources(Me, "$this")
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.ControlBox = False
        Me.Controls.Add(Me.DataGridReasonCode)
        Me.Controls.Add(Me.ButtonOK)
        Me.Controls.Add(Me.LabelHeader)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MinimizeBox = False
        Me.Name = "ReasonCodeDescription"
        Me.TopMost = True
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LabelHeader As System.Windows.Forms.Label
    Friend WithEvents ButtonOK As System.Windows.Forms.Button
    Friend WithEvents DataGridReasonCode As System.Windows.Forms.DataGrid
    Friend WithEvents DataGridTableStyle1 As System.Windows.Forms.DataGridTableStyle
    Friend WithEvents DataGridTextBoxColumnCode As System.Windows.Forms.DataGridTextBoxColumn
    Friend WithEvents DataGridTextBoxColumnDescription As System.Windows.Forms.DataGridTextBoxColumn
End Class
