<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmBigText
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
        'This call is required by the Windows Form Designer.
        Me.IsInitializing = True
        InitializeComponent()
        Me.IsInitializing = False
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
	Public WithEvents txtBigText As System.Windows.Forms.TextBox
	Public WithEvents CancelButton_Renamed As System.Windows.Forms.Button
	Public WithEvents OKButton As System.Windows.Forms.Button
	'NOTE: The following procedure is required by the Windows Form Designer
	'It can be modified using the Windows Form Designer.
	'Do not modify it using the code editor.
	<System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmBigText))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtBigText = New System.Windows.Forms.TextBox
        Me.CancelButton_Renamed = New System.Windows.Forms.Button
        Me.OKButton = New System.Windows.Forms.Button
        Me.SuspendLayout()
        '
        'txtBigText
        '
        Me.txtBigText.AcceptsReturn = True
        Me.txtBigText.BackColor = System.Drawing.SystemColors.Window
        Me.txtBigText.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtBigText, "txtBigText")
        Me.txtBigText.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtBigText.Name = "txtBigText"
        '
        'CancelButton_Renamed
        '
        Me.CancelButton_Renamed.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton_Renamed.Cursor = System.Windows.Forms.Cursors.Default
        Me.CancelButton_Renamed.DialogResult = System.Windows.Forms.DialogResult.Cancel
        resources.ApplyResources(Me.CancelButton_Renamed, "CancelButton_Renamed")
        Me.CancelButton_Renamed.ForeColor = System.Drawing.SystemColors.ControlText
        Me.CancelButton_Renamed.Name = "CancelButton_Renamed"
        Me.CancelButton_Renamed.UseVisualStyleBackColor = False
        '
        'OKButton
        '
        Me.OKButton.BackColor = System.Drawing.SystemColors.Control
        Me.OKButton.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.OKButton, "OKButton")
        Me.OKButton.ForeColor = System.Drawing.SystemColors.ControlText
        Me.OKButton.Name = "OKButton"
        Me.OKButton.UseVisualStyleBackColor = False
        '
        'frmBigText
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.CancelButton_Renamed
        Me.Controls.Add(Me.txtBigText)
        Me.Controls.Add(Me.CancelButton_Renamed)
        Me.Controls.Add(Me.OKButton)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmBigText"
        Me.ShowInTaskbar = False
        Me.ResumeLayout(False)

    End Sub
#End Region 
End Class