<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_PlanogramImport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_PlanogramImport))
        Me.CloseButton = New System.Windows.Forms.Button
        Me.LoadGroupBox = New System.Windows.Forms.GroupBox
        Me.FileGroupBox = New System.Windows.Forms.GroupBox
        Me.btnSelectFile = New System.Windows.Forms.Button
        Me.txtFile = New System.Windows.Forms.TextBox
        Me.ImportButton = New System.Windows.Forms.Button
        Me.CreateGroupBox = New System.Windows.Forms.GroupBox
        Me.NonRegularRadioButton = New System.Windows.Forms.RadioButton
        Me.RegularRadioButton = New System.Windows.Forms.RadioButton
        Me.SetListBox = New System.Windows.Forms.ListBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.SubteamComboBox = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.StoreComboBox = New System.Windows.Forms.ComboBox
        Me.DateLabel = New System.Windows.Forms.Label
        Me.dtpStartDate = New Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
        Me.StoreLabel = New System.Windows.Forms.Label
        Me.CreateButton = New System.Windows.Forms.Button
        Me.selectFileDialog = New System.Windows.Forms.OpenFileDialog
        Me.LoadGroupBox.SuspendLayout()
        Me.FileGroupBox.SuspendLayout()
        Me.CreateGroupBox.SuspendLayout()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'CloseButton
        '
        resources.ApplyResources(Me.CloseButton, "CloseButton")
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'LoadGroupBox
        '
        Me.LoadGroupBox.Controls.Add(Me.FileGroupBox)
        Me.LoadGroupBox.Controls.Add(Me.ImportButton)
        resources.ApplyResources(Me.LoadGroupBox, "LoadGroupBox")
        Me.LoadGroupBox.Name = "LoadGroupBox"
        Me.LoadGroupBox.TabStop = False
        '
        'FileGroupBox
        '
        Me.FileGroupBox.Controls.Add(Me.btnSelectFile)
        Me.FileGroupBox.Controls.Add(Me.txtFile)
        resources.ApplyResources(Me.FileGroupBox, "FileGroupBox")
        Me.FileGroupBox.Name = "FileGroupBox"
        Me.FileGroupBox.TabStop = False
        '
        'btnSelectFile
        '
        resources.ApplyResources(Me.btnSelectFile, "btnSelectFile")
        Me.btnSelectFile.Name = "btnSelectFile"
        Me.btnSelectFile.UseVisualStyleBackColor = True
        '
        'txtFile
        '
        resources.ApplyResources(Me.txtFile, "txtFile")
        Me.txtFile.Name = "txtFile"
        '
        'ImportButton
        '
        resources.ApplyResources(Me.ImportButton, "ImportButton")
        Me.ImportButton.Name = "ImportButton"
        Me.ImportButton.UseVisualStyleBackColor = True
        '
        'CreateGroupBox
        '
        Me.CreateGroupBox.Controls.Add(Me.NonRegularRadioButton)
        Me.CreateGroupBox.Controls.Add(Me.RegularRadioButton)
        Me.CreateGroupBox.Controls.Add(Me.SetListBox)
        Me.CreateGroupBox.Controls.Add(Me.Label3)
        Me.CreateGroupBox.Controls.Add(Me.SubteamComboBox)
        Me.CreateGroupBox.Controls.Add(Me.Label2)
        Me.CreateGroupBox.Controls.Add(Me.StoreComboBox)
        Me.CreateGroupBox.Controls.Add(Me.DateLabel)
        Me.CreateGroupBox.Controls.Add(Me.dtpStartDate)
        Me.CreateGroupBox.Controls.Add(Me.StoreLabel)
        Me.CreateGroupBox.Controls.Add(Me.CreateButton)
        resources.ApplyResources(Me.CreateGroupBox, "CreateGroupBox")
        Me.CreateGroupBox.Name = "CreateGroupBox"
        Me.CreateGroupBox.TabStop = False
        '
        'NonRegularRadioButton
        '
        resources.ApplyResources(Me.NonRegularRadioButton, "NonRegularRadioButton")
        Me.NonRegularRadioButton.Name = "NonRegularRadioButton"
        Me.NonRegularRadioButton.UseVisualStyleBackColor = True
        '
        'RegularRadioButton
        '
        resources.ApplyResources(Me.RegularRadioButton, "RegularRadioButton")
        Me.RegularRadioButton.Checked = True
        Me.RegularRadioButton.Name = "RegularRadioButton"
        Me.RegularRadioButton.TabStop = True
        Me.RegularRadioButton.UseVisualStyleBackColor = True
        '
        'SetListBox
        '
        Me.SetListBox.FormattingEnabled = True
        resources.ApplyResources(Me.SetListBox, "SetListBox")
        Me.SetListBox.Name = "SetListBox"
        Me.SetListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        '
        'Label3
        '
        resources.ApplyResources(Me.Label3, "Label3")
        Me.Label3.Name = "Label3"
        '
        'SubteamComboBox
        '
        Me.SubteamComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.SubteamComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.SubteamComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.SubteamComboBox.FormattingEnabled = True
        resources.ApplyResources(Me.SubteamComboBox, "SubteamComboBox")
        Me.SubteamComboBox.Name = "SubteamComboBox"
        '
        'Label2
        '
        resources.ApplyResources(Me.Label2, "Label2")
        Me.Label2.Name = "Label2"
        '
        'StoreComboBox
        '
        Me.StoreComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.StoreComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.StoreComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.StoreComboBox.FormattingEnabled = True
        resources.ApplyResources(Me.StoreComboBox, "StoreComboBox")
        Me.StoreComboBox.Name = "StoreComboBox"
        '
        'DateLabel
        '
        resources.ApplyResources(Me.DateLabel, "DateLabel")
        Me.DateLabel.Name = "DateLabel"
        '
        'dtpStartDate
        '
        Me.dtpStartDate.DateTime = New Date(1980, 1, 1, 0, 0, 0, 0)
        resources.ApplyResources(Me.dtpStartDate, "dtpStartDate")
        Me.dtpStartDate.MaskInput = ""
        Me.dtpStartDate.MaxDate = New Date(2099, 12, 31, 0, 0, 0, 0)
        Me.dtpStartDate.MinDate = New Date(1980, 1, 1, 0, 0, 0, 0)
        Me.dtpStartDate.Name = "dtpStartDate"
        Me.dtpStartDate.Value = Nothing
        '
        'StoreLabel
        '
        resources.ApplyResources(Me.StoreLabel, "StoreLabel")
        Me.StoreLabel.Name = "StoreLabel"
        '
        'CreateButton
        '
        resources.ApplyResources(Me.CreateButton, "CreateButton")
        Me.CreateButton.Name = "CreateButton"
        Me.CreateButton.UseVisualStyleBackColor = True
        '
        'Form_PlanogramImport
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ControlBox = False
        Me.Controls.Add(Me.CreateGroupBox)
        Me.Controls.Add(Me.LoadGroupBox)
        Me.Controls.Add(Me.CloseButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_PlanogramImport"
        Me.ShowInTaskbar = False
        Me.LoadGroupBox.ResumeLayout(False)
        Me.FileGroupBox.ResumeLayout(False)
        Me.FileGroupBox.PerformLayout()
        Me.CreateGroupBox.ResumeLayout(False)
        Me.CreateGroupBox.PerformLayout()
        CType(Me.dtpStartDate, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents CloseButton As System.Windows.Forms.Button
    Friend WithEvents LoadGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents CreateGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents ImportButton As System.Windows.Forms.Button
    Friend WithEvents CreateButton As System.Windows.Forms.Button
    Friend WithEvents FileGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents btnSelectFile As System.Windows.Forms.Button
    Friend WithEvents txtFile As System.Windows.Forms.TextBox
    Friend WithEvents StoreLabel As System.Windows.Forms.Label
    Friend WithEvents DateLabel As System.Windows.Forms.Label
    Friend WithEvents dtpStartDate As Infragistics.Win.UltraWinEditors.UltraDateTimeEditor
    Friend WithEvents selectFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents StoreComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents SubteamComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents SetListBox As System.Windows.Forms.ListBox
    Friend WithEvents NonRegularRadioButton As System.Windows.Forms.RadioButton
    Friend WithEvents RegularRadioButton As System.Windows.Forms.RadioButton
End Class
