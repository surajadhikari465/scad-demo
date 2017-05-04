<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ConfigurationData_EnvironmentAdd
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
        Me._textShortname = New System.Windows.Forms.TextBox
        Me._textName = New System.Windows.Forms.TextBox
        Me._buttonAdd = New System.Windows.Forms.Button
        Me._labelShortname = New System.Windows.Forms.Label
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
        Me._groupContainer.Controls.Add(Me._textShortname)
        Me._groupContainer.Controls.Add(Me._textName)
        Me._groupContainer.Controls.Add(Me._buttonAdd)
        Me._groupContainer.Controls.Add(Me._labelShortname)
        Me._groupContainer.Controls.Add(Me.Label1)
        Me._groupContainer.Location = New System.Drawing.Point(3, 2)
        Me._groupContainer.Name = "_groupContainer"
        Me._groupContainer.Size = New System.Drawing.Size(331, 96)
        Me._groupContainer.TabIndex = 0
        Me._groupContainer.TabStop = False
        '
        '_textShortname
        '
        Me._textShortname.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me._textShortname.Location = New System.Drawing.Point(84, 56)
        Me._textShortname.MaxLength = 5
        Me._textShortname.Name = "_textShortname"
        Me._textShortname.Size = New System.Drawing.Size(59, 22)
        Me._textShortname.TabIndex = 2
        '
        '_textName
        '
        Me._textName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper
        Me._textName.Location = New System.Drawing.Point(84, 29)
        Me._textName.MaxLength = 50
        Me._textName.Name = "_textName"
        Me._textName.Size = New System.Drawing.Size(227, 22)
        Me._textName.TabIndex = 1
        '
        '_buttonAdd
        '
        Me._buttonAdd.Location = New System.Drawing.Point(236, 56)
        Me._buttonAdd.Name = "_buttonAdd"
        Me._buttonAdd.Size = New System.Drawing.Size(75, 23)
        Me._buttonAdd.TabIndex = 3
        Me._buttonAdd.Text = "Add"
        Me._buttonAdd.UseVisualStyleBackColor = True
        '
        '_labelShortname
        '
        Me._labelShortname.AutoSize = True
        Me._labelShortname.Location = New System.Drawing.Point(17, 61)
        Me._labelShortname.Name = "_labelShortname"
        Me._labelShortname.Size = New System.Drawing.Size(66, 13)
        Me._labelShortname.TabIndex = 1
        Me._labelShortname.Text = "Shortname:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(44, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(39, 13)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Name:"
        '
        '_formErrorProvider
        '
        Me._formErrorProvider.ContainerControl = Me
        '
        'Form_ConfigurationData_EnvironmentAdd
        '
        Me.AcceptButton = Me._buttonAdd
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(338, 104)
        Me.Controls.Add(Me._groupContainer)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_ConfigurationData_EnvironmentAdd"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Add Environment"
        Me._groupContainer.ResumeLayout(False)
        Me._groupContainer.PerformLayout()
        CType(Me._formErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents _groupContainer As System.Windows.Forms.GroupBox
    Friend WithEvents _textName As System.Windows.Forms.TextBox
    Friend WithEvents _buttonAdd As System.Windows.Forms.Button
    Friend WithEvents _labelShortname As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents _textShortname As System.Windows.Forms.TextBox
    Friend WithEvents _formErrorProvider As System.Windows.Forms.ErrorProvider
End Class
