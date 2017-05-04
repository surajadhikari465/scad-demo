<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_PlanogramExport
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_PlanogramExport))
        Me.CloseButton = New System.Windows.Forms.Button
        Me.CreateGroupBox = New System.Windows.Forms.GroupBox
        Me.StoreLabel = New System.Windows.Forms.Label
        Me.StoreComboBox = New System.Windows.Forms.ComboBox
        Me.ExportButton = New System.Windows.Forms.Button
        Me.CreateGroupBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'CloseButton
        '
        resources.ApplyResources(Me.CloseButton, "CloseButton")
        Me.CloseButton.Name = "CloseButton"
        Me.CloseButton.UseVisualStyleBackColor = True
        '
        'CreateGroupBox
        '
        Me.CreateGroupBox.Controls.Add(Me.StoreLabel)
        Me.CreateGroupBox.Controls.Add(Me.StoreComboBox)
        Me.CreateGroupBox.Controls.Add(Me.ExportButton)
        resources.ApplyResources(Me.CreateGroupBox, "CreateGroupBox")
        Me.CreateGroupBox.Name = "CreateGroupBox"
        Me.CreateGroupBox.TabStop = False
        '
        'StoreLabel
        '
        resources.ApplyResources(Me.StoreLabel, "StoreLabel")
        Me.StoreLabel.Name = "StoreLabel"
        '
        'StoreComboBox
        '
        Me.StoreComboBox.FormattingEnabled = True
        resources.ApplyResources(Me.StoreComboBox, "StoreComboBox")
        Me.StoreComboBox.Name = "StoreComboBox"
        '
        'ExportButton
        '
        resources.ApplyResources(Me.ExportButton, "ExportButton")
        Me.ExportButton.Name = "ExportButton"
        Me.ExportButton.UseVisualStyleBackColor = True
        '
        'Form_PlanogramExport
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ControlBox = False
        Me.Controls.Add(Me.CreateGroupBox)
        Me.Controls.Add(Me.CloseButton)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_PlanogramExport"
        Me.ShowInTaskbar = False
        Me.CreateGroupBox.ResumeLayout(False)
        Me.CreateGroupBox.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents CloseButton As System.Windows.Forms.Button
    Friend WithEvents CreateGroupBox As System.Windows.Forms.GroupBox
    Friend WithEvents StoreLabel As System.Windows.Forms.Label
    Friend WithEvents StoreComboBox As System.Windows.Forms.ComboBox
    Friend WithEvents ExportButton As System.Windows.Forms.Button
End Class
