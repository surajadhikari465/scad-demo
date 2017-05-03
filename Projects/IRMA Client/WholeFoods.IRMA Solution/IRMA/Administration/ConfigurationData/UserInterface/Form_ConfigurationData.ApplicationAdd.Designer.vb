<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ConfigurationData_ApplicationAdd
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
        Me.components = New System.ComponentModel.Container
        Me._groupContainer = New System.Windows.Forms.GroupBox
        Me._labelEnv = New System.Windows.Forms.Label
        Me._comboFilterType = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me._textName = New System.Windows.Forms.TextBox
        Me._buttonAdd = New System.Windows.Forms.Button
        Me._labelEnvironment = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me._formErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me._groupContainer.SuspendLayout()
        CType(Me._formErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_groupContainer
        '
        Me._groupContainer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._groupContainer.Controls.Add(Me._labelEnv)
        Me._groupContainer.Controls.Add(Me._comboFilterType)
        Me._groupContainer.Controls.Add(Me.Label3)
        Me._groupContainer.Controls.Add(Me._textName)
        Me._groupContainer.Controls.Add(Me._buttonAdd)
        Me._groupContainer.Controls.Add(Me._labelEnvironment)
        Me._groupContainer.Controls.Add(Me.Label1)
        Me._groupContainer.Location = New System.Drawing.Point(4, 2)
        Me._groupContainer.Name = "_groupContainer"
        Me._groupContainer.Size = New System.Drawing.Size(340, 152)
        Me._groupContainer.TabIndex = 0
        Me._groupContainer.TabStop = False
        '
        '_labelEnv
        '
        Me._labelEnv.AutoSize = True
        Me._labelEnv.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._labelEnv.Location = New System.Drawing.Point(96, 29)
        Me._labelEnv.Name = "_labelEnv"
        Me._labelEnv.Size = New System.Drawing.Size(21, 13)
        Me._labelEnv.TabIndex = 6
        Me._labelEnv.Text = "{0}"
        '
        '_comboFilterType
        '
        Me._comboFilterType.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me._comboFilterType.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._comboFilterType.FormattingEnabled = True
        Me._comboFilterType.Location = New System.Drawing.Point(96, 84)
        Me._comboFilterType.Name = "_comboFilterType"
        Me._comboFilterType.Size = New System.Drawing.Size(221, 21)
        Me._comboFilterType.TabIndex = 2
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(57, 87)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(33, 13)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Type:"
        '
        '_textName
        '
        Me._textName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me._textName.Location = New System.Drawing.Point(97, 56)
        Me._textName.Name = "_textName"
        Me._textName.Size = New System.Drawing.Size(220, 22)
        Me._textName.TabIndex = 1
        '
        '_buttonAdd
        '
        Me._buttonAdd.Location = New System.Drawing.Point(242, 111)
        Me._buttonAdd.Name = "_buttonAdd"
        Me._buttonAdd.Size = New System.Drawing.Size(75, 23)
        Me._buttonAdd.TabIndex = 4
        Me._buttonAdd.Text = "Add"
        Me._buttonAdd.UseVisualStyleBackColor = True
        '
        '_labelEnvironment
        '
        Me._labelEnvironment.AutoSize = True
        Me._labelEnvironment.Location = New System.Drawing.Point(15, 29)
        Me._labelEnvironment.Name = "_labelEnvironment"
        Me._labelEnvironment.Size = New System.Drawing.Size(75, 13)
        Me._labelEnvironment.TabIndex = 1
        Me._labelEnvironment.Text = "Environment:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(51, 59)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Name:"
        '
        '_formErrorProvider
        '
        Me._formErrorProvider.ContainerControl = Me
        '
        'Form_ConfigurationData_ApplicationAdd
        '
        Me.AcceptButton = Me._buttonAdd
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(349, 161)
        Me.Controls.Add(Me._groupContainer)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_ConfigurationData_ApplicationAdd"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add Application"
        Me._groupContainer.ResumeLayout(False)
        Me._groupContainer.PerformLayout()
        CType(Me._formErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents _groupContainer As System.Windows.Forms.GroupBox
    Friend WithEvents _textName As System.Windows.Forms.TextBox
    Friend WithEvents _buttonAdd As System.Windows.Forms.Button
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents _comboFilterType As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents _formErrorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents _labelEnv As System.Windows.Forms.Label
    Friend WithEvents _labelEnvironment As System.Windows.Forms.Label
End Class
