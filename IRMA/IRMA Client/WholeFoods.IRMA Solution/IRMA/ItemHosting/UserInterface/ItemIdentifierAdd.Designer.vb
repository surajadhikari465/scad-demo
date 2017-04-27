<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmItemIdentifierAdd
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
	Public WithEvents _optType_0 As System.Windows.Forms.RadioButton
	Public WithEvents _optType_1 As System.Windows.Forms.RadioButton
	Public WithEvents _optType_2 As System.Windows.Forms.RadioButton
	Public WithEvents _optType_3 As System.Windows.Forms.RadioButton
	Public WithEvents fraType As System.Windows.Forms.GroupBox
	Public WithEvents txtCheckDigit As System.Windows.Forms.TextBox
	Public WithEvents txtIdentifier As System.Windows.Forms.TextBox
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents lblCheckDigit As System.Windows.Forms.Label
    Public WithEvents lblIdentifier As System.Windows.Forms.Label
    Public WithEvents optType As Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmItemIdentifierAdd))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.cmdExit = New System.Windows.Forms.Button()
        Me.fraType = New System.Windows.Forms.GroupBox()
        Me._optType_0 = New System.Windows.Forms.RadioButton()
        Me._optType_1 = New System.Windows.Forms.RadioButton()
        Me._optType_2 = New System.Windows.Forms.RadioButton()
        Me._optType_3 = New System.Windows.Forms.RadioButton()
        Me.txtCheckDigit = New System.Windows.Forms.TextBox()
        Me.txtIdentifier = New System.Windows.Forms.TextBox()
        Me.lblCheckDigit = New System.Windows.Forms.Label()
        Me.lblIdentifier = New System.Windows.Forms.Label()
        Me.optType = New Microsoft.VisualBasic.Compatibility.VB6.RadioButtonArray(Me.components)
        Me.CheckBox_National = New System.Windows.Forms.CheckBox()
        Me.GroupBox_SendToScale = New System.Windows.Forms.GroupBox()
        Me.RadioButton_SendToScale_No = New System.Windows.Forms.RadioButton()
        Me.RadioButton_SendToScale_Yes = New System.Windows.Forms.RadioButton()
        Me.lblNonType2Message = New System.Windows.Forms.Label()
        Me.GroupBox_NumScaleDigits = New System.Windows.Forms.GroupBox()
        Me.RadioButton_NumScaleDigits_5 = New System.Windows.Forms.RadioButton()
        Me.RadioButton_NumScaleDigits_4 = New System.Windows.Forms.RadioButton()
        Me.fraType.SuspendLayout()
        CType(Me.optType, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_SendToScale.SuspendLayout()
        Me.GroupBox_NumScaleDigits.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdAdd, "cmdAdd")
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Name = "cmdAdd"
        Me.ToolTip1.SetToolTip(Me.cmdAdd, resources.GetString("cmdAdd.ToolTip"))
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'fraType
        '
        Me.fraType.BackColor = System.Drawing.SystemColors.Control
        Me.fraType.Controls.Add(Me._optType_0)
        Me.fraType.Controls.Add(Me._optType_1)
        Me.fraType.Controls.Add(Me._optType_2)
        Me.fraType.Controls.Add(Me._optType_3)
        resources.ApplyResources(Me.fraType, "fraType")
        Me.fraType.ForeColor = System.Drawing.SystemColors.ControlText
        Me.fraType.Name = "fraType"
        Me.fraType.TabStop = False
        '
        '_optType_0
        '
        Me._optType_0.BackColor = System.Drawing.SystemColors.Control
        Me._optType_0.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optType_0, "_optType_0")
        Me._optType_0.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optType.SetIndex(Me._optType_0, CType(0, Short))
        Me._optType_0.Name = "_optType_0"
        Me._optType_0.TabStop = True
        Me._optType_0.UseVisualStyleBackColor = False
        '
        '_optType_1
        '
        Me._optType_1.BackColor = System.Drawing.SystemColors.Control
        Me._optType_1.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optType_1, "_optType_1")
        Me._optType_1.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optType.SetIndex(Me._optType_1, CType(1, Short))
        Me._optType_1.Name = "_optType_1"
        Me._optType_1.TabStop = True
        Me._optType_1.UseVisualStyleBackColor = False
        '
        '_optType_2
        '
        Me._optType_2.BackColor = System.Drawing.SystemColors.Control
        Me._optType_2.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optType_2, "_optType_2")
        Me._optType_2.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optType.SetIndex(Me._optType_2, CType(2, Short))
        Me._optType_2.Name = "_optType_2"
        Me._optType_2.TabStop = True
        Me._optType_2.UseVisualStyleBackColor = False
        '
        '_optType_3
        '
        Me._optType_3.BackColor = System.Drawing.SystemColors.Control
        Me._optType_3.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me._optType_3, "_optType_3")
        Me._optType_3.ForeColor = System.Drawing.SystemColors.ControlText
        Me.optType.SetIndex(Me._optType_3, CType(3, Short))
        Me._optType_3.Name = "_optType_3"
        Me._optType_3.TabStop = True
        Me._optType_3.UseVisualStyleBackColor = False
        '
        'txtCheckDigit
        '
        Me.txtCheckDigit.AcceptsReturn = True
        Me.txtCheckDigit.BackColor = System.Drawing.SystemColors.Window
        Me.txtCheckDigit.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtCheckDigit, "txtCheckDigit")
        Me.txtCheckDigit.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCheckDigit.Name = "txtCheckDigit"
        Me.txtCheckDigit.Tag = "Number"
        '
        'txtIdentifier
        '
        Me.txtIdentifier.AcceptsReturn = True
        Me.txtIdentifier.BackColor = System.Drawing.SystemColors.Window
        Me.txtIdentifier.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtIdentifier, "txtIdentifier")
        Me.txtIdentifier.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtIdentifier.Name = "txtIdentifier"
        Me.txtIdentifier.Tag = "Number"
        '
        'lblCheckDigit
        '
        Me.lblCheckDigit.BackColor = System.Drawing.Color.Transparent
        Me.lblCheckDigit.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblCheckDigit, "lblCheckDigit")
        Me.lblCheckDigit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCheckDigit.Name = "lblCheckDigit"
        '
        'lblIdentifier
        '
        Me.lblIdentifier.BackColor = System.Drawing.Color.Transparent
        Me.lblIdentifier.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblIdentifier, "lblIdentifier")
        Me.lblIdentifier.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblIdentifier.Name = "lblIdentifier"
        '
        'optType
        '
        '
        'CheckBox_National
        '
        resources.ApplyResources(Me.CheckBox_National, "CheckBox_National")
        Me.CheckBox_National.Name = "CheckBox_National"
        Me.CheckBox_National.UseVisualStyleBackColor = True
        '
        'GroupBox_SendToScale
        '
        Me.GroupBox_SendToScale.Controls.Add(Me.RadioButton_SendToScale_No)
        Me.GroupBox_SendToScale.Controls.Add(Me.RadioButton_SendToScale_Yes)
        resources.ApplyResources(Me.GroupBox_SendToScale, "GroupBox_SendToScale")
        Me.GroupBox_SendToScale.Name = "GroupBox_SendToScale"
        Me.GroupBox_SendToScale.TabStop = False
        '
        'RadioButton_SendToScale_No
        '
        resources.ApplyResources(Me.RadioButton_SendToScale_No, "RadioButton_SendToScale_No")
        Me.RadioButton_SendToScale_No.Checked = True
        Me.RadioButton_SendToScale_No.Name = "RadioButton_SendToScale_No"
        Me.RadioButton_SendToScale_No.TabStop = True
        Me.RadioButton_SendToScale_No.UseVisualStyleBackColor = True
        '
        'RadioButton_SendToScale_Yes
        '
        resources.ApplyResources(Me.RadioButton_SendToScale_Yes, "RadioButton_SendToScale_Yes")
        Me.RadioButton_SendToScale_Yes.Name = "RadioButton_SendToScale_Yes"
        Me.RadioButton_SendToScale_Yes.UseVisualStyleBackColor = True
        '
        'lblNonType2Message
        '
        resources.ApplyResources(Me.lblNonType2Message, "lblNonType2Message")
        Me.lblNonType2Message.ForeColor = System.Drawing.Color.Red
        Me.lblNonType2Message.Name = "lblNonType2Message"
        '
        'GroupBox_NumScaleDigits
        '
        Me.GroupBox_NumScaleDigits.Controls.Add(Me.RadioButton_NumScaleDigits_5)
        Me.GroupBox_NumScaleDigits.Controls.Add(Me.RadioButton_NumScaleDigits_4)
        resources.ApplyResources(Me.GroupBox_NumScaleDigits, "GroupBox_NumScaleDigits")
        Me.GroupBox_NumScaleDigits.Name = "GroupBox_NumScaleDigits"
        Me.GroupBox_NumScaleDigits.TabStop = False
        '
        'RadioButton_NumScaleDigits_5
        '
        resources.ApplyResources(Me.RadioButton_NumScaleDigits_5, "RadioButton_NumScaleDigits_5")
        Me.RadioButton_NumScaleDigits_5.Name = "RadioButton_NumScaleDigits_5"
        Me.RadioButton_NumScaleDigits_5.UseVisualStyleBackColor = True
        '
        'RadioButton_NumScaleDigits_4
        '
        resources.ApplyResources(Me.RadioButton_NumScaleDigits_4, "RadioButton_NumScaleDigits_4")
        Me.RadioButton_NumScaleDigits_4.Checked = True
        Me.RadioButton_NumScaleDigits_4.Name = "RadioButton_NumScaleDigits_4"
        Me.RadioButton_NumScaleDigits_4.TabStop = True
        Me.RadioButton_NumScaleDigits_4.UseVisualStyleBackColor = True
        '
        'frmItemIdentifierAdd
        '
        Me.AcceptButton = Me.cmdAdd
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.Controls.Add(Me.GroupBox_NumScaleDigits)
        Me.Controls.Add(Me.lblNonType2Message)
        Me.Controls.Add(Me.GroupBox_SendToScale)
        Me.Controls.Add(Me.CheckBox_National)
        Me.Controls.Add(Me.fraType)
        Me.Controls.Add(Me.txtCheckDigit)
        Me.Controls.Add(Me.txtIdentifier)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.lblCheckDigit)
        Me.Controls.Add(Me.lblIdentifier)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmItemIdentifierAdd"
        Me.ShowInTaskbar = False
        Me.fraType.ResumeLayout(False)
        CType(Me.optType, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_SendToScale.ResumeLayout(False)
        Me.GroupBox_SendToScale.PerformLayout()
        Me.GroupBox_NumScaleDigits.ResumeLayout(False)
        Me.GroupBox_NumScaleDigits.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents CheckBox_National As System.Windows.Forms.CheckBox
    Friend WithEvents GroupBox_SendToScale As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton_SendToScale_No As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_SendToScale_Yes As System.Windows.Forms.RadioButton
    Friend WithEvents lblNonType2Message As System.Windows.Forms.Label
    Friend WithEvents GroupBox_NumScaleDigits As System.Windows.Forms.GroupBox
    Friend WithEvents RadioButton_NumScaleDigits_5 As System.Windows.Forms.RadioButton
    Friend WithEvents RadioButton_NumScaleDigits_4 As System.Windows.Forms.RadioButton
#End Region
End Class