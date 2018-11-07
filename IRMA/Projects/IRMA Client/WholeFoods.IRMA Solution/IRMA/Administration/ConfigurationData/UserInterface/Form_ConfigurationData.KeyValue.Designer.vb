<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ConfigurationData_KeyValue
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
        Me._buttonSave = New System.Windows.Forms.Button
        Me._labelApplication = New System.Windows.Forms.Label
        Me._labelValue = New System.Windows.Forms.Label
        Me._textValue = New System.Windows.Forms.TextBox
        Me._labelApplicationName = New System.Windows.Forms.Label
        Me._labelEnvironmentName = New System.Windows.Forms.Label
        Me._labelEnvironment = New System.Windows.Forms.Label
        Me._labelKeyName = New System.Windows.Forms.Label
        Me._labelKey = New System.Windows.Forms.Label
        Me._groupContainer = New System.Windows.Forms.GroupBox
        Me._formErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me._checkEncrypt = New System.Windows.Forms.CheckBox
        Me._groupContainer.SuspendLayout()
        CType(Me._formErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_buttonSave
        '
        Me._buttonSave.Enabled = False
        Me._buttonSave.Location = New System.Drawing.Point(322, 131)
        Me._buttonSave.Name = "_buttonSave"
        Me._buttonSave.Size = New System.Drawing.Size(72, 28)
        Me._buttonSave.TabIndex = 2
        Me._buttonSave.Text = "Save"
        Me._buttonSave.UseVisualStyleBackColor = True
        '
        '_labelApplication
        '
        Me._labelApplication.AutoSize = True
        Me._labelApplication.Location = New System.Drawing.Point(24, 29)
        Me._labelApplication.Name = "_labelApplication"
        Me._labelApplication.Size = New System.Drawing.Size(69, 13)
        Me._labelApplication.TabIndex = 1
        Me._labelApplication.Text = "Application:"
        '
        '_labelValue
        '
        Me._labelValue.AutoSize = True
        Me._labelValue.Location = New System.Drawing.Point(54, 106)
        Me._labelValue.Name = "_labelValue"
        Me._labelValue.Size = New System.Drawing.Size(39, 13)
        Me._labelValue.TabIndex = 2
        Me._labelValue.Text = "Value:"
        '
        '_textValue
        '
        Me._textValue.Location = New System.Drawing.Point(102, 103)
        Me._textValue.Name = "_textValue"
        Me._textValue.Size = New System.Drawing.Size(292, 22)
        Me._textValue.TabIndex = 1
        '
        '_labelApplicationName
        '
        Me._labelApplicationName.AutoSize = True
        Me._labelApplicationName.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._labelApplicationName.Location = New System.Drawing.Point(99, 29)
        Me._labelApplicationName.Name = "_labelApplicationName"
        Me._labelApplicationName.Size = New System.Drawing.Size(21, 13)
        Me._labelApplicationName.TabIndex = 4
        Me._labelApplicationName.Text = "{0}"
        '
        '_labelEnvironmentName
        '
        Me._labelEnvironmentName.AutoSize = True
        Me._labelEnvironmentName.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._labelEnvironmentName.Location = New System.Drawing.Point(99, 51)
        Me._labelEnvironmentName.Name = "_labelEnvironmentName"
        Me._labelEnvironmentName.Size = New System.Drawing.Size(21, 13)
        Me._labelEnvironmentName.TabIndex = 6
        Me._labelEnvironmentName.Text = "{0}"
        '
        '_labelEnvironment
        '
        Me._labelEnvironment.AutoSize = True
        Me._labelEnvironment.Location = New System.Drawing.Point(18, 51)
        Me._labelEnvironment.Name = "_labelEnvironment"
        Me._labelEnvironment.Size = New System.Drawing.Size(75, 13)
        Me._labelEnvironment.TabIndex = 5
        Me._labelEnvironment.Text = "Environment:"
        '
        '_labelKeyName
        '
        Me._labelKeyName.AutoSize = True
        Me._labelKeyName.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me._labelKeyName.Location = New System.Drawing.Point(99, 84)
        Me._labelKeyName.Name = "_labelKeyName"
        Me._labelKeyName.Size = New System.Drawing.Size(21, 13)
        Me._labelKeyName.TabIndex = 8
        Me._labelKeyName.Text = "{0}"
        '
        '_labelKey
        '
        Me._labelKey.AutoSize = True
        Me._labelKey.Location = New System.Drawing.Point(66, 84)
        Me._labelKey.Name = "_labelKey"
        Me._labelKey.Size = New System.Drawing.Size(27, 13)
        Me._labelKey.TabIndex = 7
        Me._labelKey.Text = "Key:"
        '
        '_groupContainer
        '
        Me._groupContainer.Controls.Add(Me._checkEncrypt)
        Me._groupContainer.Controls.Add(Me._labelApplication)
        Me._groupContainer.Controls.Add(Me._labelKeyName)
        Me._groupContainer.Controls.Add(Me._buttonSave)
        Me._groupContainer.Controls.Add(Me._labelKey)
        Me._groupContainer.Controls.Add(Me._labelValue)
        Me._groupContainer.Controls.Add(Me._labelEnvironmentName)
        Me._groupContainer.Controls.Add(Me._textValue)
        Me._groupContainer.Controls.Add(Me._labelEnvironment)
        Me._groupContainer.Controls.Add(Me._labelApplicationName)
        Me._groupContainer.Location = New System.Drawing.Point(6, 2)
        Me._groupContainer.Name = "_groupContainer"
        Me._groupContainer.Size = New System.Drawing.Size(410, 177)
        Me._groupContainer.TabIndex = 0
        Me._groupContainer.TabStop = False
        '
        '_formErrorProvider
        '
        Me._formErrorProvider.ContainerControl = Me
        '
        '_checkEncrypt
        '
        Me._checkEncrypt.AutoSize = True
        Me._checkEncrypt.Location = New System.Drawing.Point(102, 131)
        Me._checkEncrypt.Name = "_checkEncrypt"
        Me._checkEncrypt.Size = New System.Drawing.Size(96, 17)
        Me._checkEncrypt.TabIndex = 9
        Me._checkEncrypt.Text = "Encrypt Value"
        Me._checkEncrypt.UseVisualStyleBackColor = True
        '
        'Form_ConfigurationData_KeyValue
        '
        Me.AcceptButton = Me._buttonSave
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(422, 186)
        Me.Controls.Add(Me._groupContainer)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_ConfigurationData_KeyValue"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Set Key/Value Pair"
        Me._groupContainer.ResumeLayout(False)
        Me._groupContainer.PerformLayout()
        CType(Me._formErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents _buttonSave As System.Windows.Forms.Button
    Friend WithEvents _labelApplication As System.Windows.Forms.Label
    Friend WithEvents _labelValue As System.Windows.Forms.Label
    Friend WithEvents _textValue As System.Windows.Forms.TextBox
    Friend WithEvents _labelApplicationName As System.Windows.Forms.Label
    Friend WithEvents _labelEnvironmentName As System.Windows.Forms.Label
    Friend WithEvents _labelEnvironment As System.Windows.Forms.Label
    Friend WithEvents _labelKeyName As System.Windows.Forms.Label
    Friend WithEvents _labelKey As System.Windows.Forms.Label
    Friend WithEvents _groupContainer As System.Windows.Forms.GroupBox
    Friend WithEvents _formErrorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents _checkEncrypt As System.Windows.Forms.CheckBox
End Class
