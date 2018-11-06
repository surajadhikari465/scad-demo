<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ConfigurationData_Build
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_ConfigurationData_Build))
        Me._groupContainer = New System.Windows.Forms.GroupBox
        Me._formProgressBar = New System.Windows.Forms.ProgressBar
        Me._groupBuildChoice = New System.Windows.Forms.GroupBox
        Me._comboFilterEnv = New System.Windows.Forms.ComboBox
        Me._comboFilterApp = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me._labelApplication = New System.Windows.Forms.Label
        Me._radioBuildChoose = New System.Windows.Forms.RadioButton
        Me._radioBuildAll = New System.Windows.Forms.RadioButton
        Me._buttonPreview = New System.Windows.Forms.Button
        Me._buttonBuild = New System.Windows.Forms.Button
        Me._formErrorProvider = New System.Windows.Forms.ErrorProvider(Me.components)
        Me._formBuildWorker = New System.ComponentModel.BackgroundWorker
        Me._groupContainer.SuspendLayout()
        Me._groupBuildChoice.SuspendLayout()
        CType(Me._formErrorProvider, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        '_groupContainer
        '
        Me._groupContainer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._groupContainer.Controls.Add(Me._formProgressBar)
        Me._groupContainer.Controls.Add(Me._groupBuildChoice)
        Me._groupContainer.Controls.Add(Me._radioBuildChoose)
        Me._groupContainer.Controls.Add(Me._radioBuildAll)
        Me._groupContainer.Controls.Add(Me._buttonPreview)
        Me._groupContainer.Controls.Add(Me._buttonBuild)
        Me._groupContainer.Location = New System.Drawing.Point(7, 2)
        Me._groupContainer.Name = "_groupContainer"
        Me._groupContainer.Size = New System.Drawing.Size(384, 227)
        Me._groupContainer.TabIndex = 0
        Me._groupContainer.TabStop = False
        '
        '_formProgressBar
        '
        Me._formProgressBar.Location = New System.Drawing.Point(18, 191)
        Me._formProgressBar.Name = "_formProgressBar"
        Me._formProgressBar.Size = New System.Drawing.Size(158, 17)
        Me._formProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee
        Me._formProgressBar.TabIndex = 5
        Me._formProgressBar.Visible = False
        '
        '_groupBuildChoice
        '
        Me._groupBuildChoice.Controls.Add(Me._comboFilterEnv)
        Me._groupBuildChoice.Controls.Add(Me._comboFilterApp)
        Me._groupBuildChoice.Controls.Add(Me.Label1)
        Me._groupBuildChoice.Controls.Add(Me._labelApplication)
        Me._groupBuildChoice.Enabled = False
        Me._groupBuildChoice.Location = New System.Drawing.Point(18, 72)
        Me._groupBuildChoice.Name = "_groupBuildChoice"
        Me._groupBuildChoice.Size = New System.Drawing.Size(351, 101)
        Me._groupBuildChoice.TabIndex = 4
        Me._groupBuildChoice.TabStop = False
        '
        '_comboFilterEnv
        '
        Me._comboFilterEnv.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me._comboFilterEnv.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._comboFilterEnv.FormattingEnabled = True
        Me._comboFilterEnv.Location = New System.Drawing.Point(100, 30)
        Me._comboFilterEnv.Name = "_comboFilterEnv"
        Me._comboFilterEnv.Size = New System.Drawing.Size(229, 21)
        Me._comboFilterEnv.TabIndex = 3
        '
        '_comboFilterApp
        '
        Me._comboFilterApp.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me._comboFilterApp.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me._comboFilterApp.Enabled = False
        Me._comboFilterApp.FormattingEnabled = True
        Me._comboFilterApp.Location = New System.Drawing.Point(100, 57)
        Me._comboFilterApp.Name = "_comboFilterApp"
        Me._comboFilterApp.Size = New System.Drawing.Size(229, 21)
        Me._comboFilterApp.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(24, 33)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(75, 13)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Environment:"
        '
        '_labelApplication
        '
        Me._labelApplication.AutoSize = True
        Me._labelApplication.Location = New System.Drawing.Point(30, 60)
        Me._labelApplication.Name = "_labelApplication"
        Me._labelApplication.Size = New System.Drawing.Size(69, 13)
        Me._labelApplication.TabIndex = 0
        Me._labelApplication.Text = "Application:"
        '
        '_radioBuildChoose
        '
        Me._radioBuildChoose.AutoSize = True
        Me._radioBuildChoose.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me._radioBuildChoose.Location = New System.Drawing.Point(18, 49)
        Me._radioBuildChoose.Name = "_radioBuildChoose"
        Me._radioBuildChoose.Size = New System.Drawing.Size(171, 17)
        Me._radioBuildChoose.TabIndex = 3
        Me._radioBuildChoose.Text = "Let me choose what to build"
        Me._radioBuildChoose.UseVisualStyleBackColor = True
        '
        '_radioBuildAll
        '
        Me._radioBuildAll.AutoSize = True
        Me._radioBuildAll.CheckAlign = System.Drawing.ContentAlignment.TopLeft
        Me._radioBuildAll.Checked = True
        Me._radioBuildAll.Location = New System.Drawing.Point(18, 26)
        Me._radioBuildAll.Name = "_radioBuildAll"
        Me._radioBuildAll.Size = New System.Drawing.Size(183, 17)
        Me._radioBuildAll.TabIndex = 2
        Me._radioBuildAll.TabStop = True
        Me._radioBuildAll.Text = "All Enviroments && Applications"
        Me._radioBuildAll.UseVisualStyleBackColor = True
        '
        '_buttonPreview
        '
        Me._buttonPreview.Enabled = False
        Me._buttonPreview.Image = CType(resources.GetObject("_buttonPreview.Image"), System.Drawing.Image)
        Me._buttonPreview.Location = New System.Drawing.Point(182, 181)
        Me._buttonPreview.Name = "_buttonPreview"
        Me._buttonPreview.Size = New System.Drawing.Size(76, 36)
        Me._buttonPreview.TabIndex = 1
        Me._buttonPreview.Text = "Preview"
        Me._buttonPreview.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me._buttonPreview.UseVisualStyleBackColor = True
        '
        '_buttonBuild
        '
        Me._buttonBuild.Image = CType(resources.GetObject("_buttonBuild.Image"), System.Drawing.Image)
        Me._buttonBuild.Location = New System.Drawing.Point(264, 181)
        Me._buttonBuild.Name = "_buttonBuild"
        Me._buttonBuild.Size = New System.Drawing.Size(105, 36)
        Me._buttonBuild.TabIndex = 0
        Me._buttonBuild.Text = "Build && Save"
        Me._buttonBuild.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me._buttonBuild.UseVisualStyleBackColor = True
        '
        '_formErrorProvider
        '
        Me._formErrorProvider.ContainerControl = Me
        '
        '_formBuildWorker
        '
        Me._formBuildWorker.WorkerSupportsCancellation = True
        '
        'Form_ConfigurationData_Build
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(399, 236)
        Me.Controls.Add(Me._groupContainer)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_ConfigurationData_Build"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Build Configuration"
        Me._groupContainer.ResumeLayout(False)
        Me._groupContainer.PerformLayout()
        Me._groupBuildChoice.ResumeLayout(False)
        Me._groupBuildChoice.PerformLayout()
        CType(Me._formErrorProvider, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents _groupContainer As System.Windows.Forms.GroupBox
    Friend WithEvents _buttonBuild As System.Windows.Forms.Button
    Friend WithEvents _buttonPreview As System.Windows.Forms.Button
    Friend WithEvents _groupBuildChoice As System.Windows.Forms.GroupBox
    Friend WithEvents _comboFilterEnv As System.Windows.Forms.ComboBox
    Friend WithEvents _comboFilterApp As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents _labelApplication As System.Windows.Forms.Label
    Friend WithEvents _radioBuildChoose As System.Windows.Forms.RadioButton
    Friend WithEvents _radioBuildAll As System.Windows.Forms.RadioButton
    Friend WithEvents _formErrorProvider As System.Windows.Forms.ErrorProvider
    Friend WithEvents _formProgressBar As System.Windows.Forms.ProgressBar
    Friend WithEvents _formBuildWorker As System.ComponentModel.BackgroundWorker
End Class
