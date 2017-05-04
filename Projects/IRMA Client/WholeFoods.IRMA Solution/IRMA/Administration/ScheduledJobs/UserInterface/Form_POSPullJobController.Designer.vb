<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_POSPullJobController
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
        Me.Label1 = New System.Windows.Forms.Label
        Me.ComboBox_Stores = New System.Windows.Forms.ComboBox
        Me.PullCatalogsButton = New System.Windows.Forms.Button
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip
        Me.StatusStrip = New System.Windows.Forms.ToolStripStatusLabel
        Me.ExceptionLabel = New System.Windows.Forms.Label
        Me.ExitButton = New System.Windows.Forms.Button
        Me.IBMFileNameTextBox = New System.Windows.Forms.TextBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.NCRFileNameTextBox = New System.Windows.Forms.TextBox
        Me.DefaultFileNamesButton = New System.Windows.Forms.Button
        Me.btnReport = New System.Windows.Forms.Button
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 10)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(37, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Store:"
        '
        'ComboBox_Stores
        '
        Me.ComboBox_Stores.FormattingEnabled = True
        Me.ComboBox_Stores.Location = New System.Drawing.Point(12, 26)
        Me.ComboBox_Stores.Name = "ComboBox_Stores"
        Me.ComboBox_Stores.Size = New System.Drawing.Size(200, 21)
        Me.ComboBox_Stores.TabIndex = 5
        '
        'PullCatalogsButton
        '
        Me.PullCatalogsButton.Location = New System.Drawing.Point(239, 120)
        Me.PullCatalogsButton.Name = "PullCatalogsButton"
        Me.PullCatalogsButton.Size = New System.Drawing.Size(165, 23)
        Me.PullCatalogsButton.TabIndex = 7
        Me.PullCatalogsButton.Text = "Pull Item Catalogs from POS"
        Me.PullCatalogsButton.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusStrip})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 191)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(438, 22)
        Me.StatusStrip1.TabIndex = 8
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'StatusStrip
        '
        Me.StatusStrip.Name = "StatusStrip"
        Me.StatusStrip.Size = New System.Drawing.Size(179, 17)
        Me.StatusStrip.Text = "Pull All Stores or select a store ... "
        '
        'ExceptionLabel
        '
        Me.ExceptionLabel.AutoSize = True
        Me.ExceptionLabel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.ExceptionLabel.Location = New System.Drawing.Point(9, 195)
        Me.ExceptionLabel.MaximumSize = New System.Drawing.Size(400, 0)
        Me.ExceptionLabel.Name = "ExceptionLabel"
        Me.ExceptionLabel.Size = New System.Drawing.Size(135, 15)
        Me.ExceptionLabel.TabIndex = 9
        Me.ExceptionLabel.Text = "Exception text goes here"
        Me.ExceptionLabel.Visible = False
        '
        'ExitButton
        '
        Me.ExitButton.Location = New System.Drawing.Point(329, 160)
        Me.ExitButton.Name = "ExitButton"
        Me.ExitButton.Size = New System.Drawing.Size(75, 23)
        Me.ExitButton.TabIndex = 10
        Me.ExitButton.Text = "Exit"
        Me.ExitButton.UseVisualStyleBackColor = True
        '
        'IBMFileNameTextBox
        '
        Me.IBMFileNameTextBox.AcceptsReturn = True
        Me.IBMFileNameTextBox.Location = New System.Drawing.Point(12, 71)
        Me.IBMFileNameTextBox.Name = "IBMFileNameTextBox"
        Me.IBMFileNameTextBox.Size = New System.Drawing.Size(200, 22)
        Me.IBMFileNameTextBox.TabIndex = 12
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 55)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(167, 13)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "IBM Filename to pull from POS:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(12, 104)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(169, 13)
        Me.Label3.TabIndex = 15
        Me.Label3.Text = "NCR Filename to pull from POS:"
        '
        'NCRFileNameTextBox
        '
        Me.NCRFileNameTextBox.Location = New System.Drawing.Point(12, 120)
        Me.NCRFileNameTextBox.Name = "NCRFileNameTextBox"
        Me.NCRFileNameTextBox.Size = New System.Drawing.Size(200, 22)
        Me.NCRFileNameTextBox.TabIndex = 14
        '
        'DefaultFileNamesButton
        '
        Me.DefaultFileNamesButton.Location = New System.Drawing.Point(239, 69)
        Me.DefaultFileNamesButton.Name = "DefaultFileNamesButton"
        Me.DefaultFileNamesButton.Size = New System.Drawing.Size(165, 23)
        Me.DefaultFileNamesButton.TabIndex = 16
        Me.DefaultFileNamesButton.Text = "Set Filenames to Default"
        Me.DefaultFileNamesButton.UseVisualStyleBackColor = True
        '
        'btnReport
        '
        Me.btnReport.Location = New System.Drawing.Point(239, 160)
        Me.btnReport.Name = "btnReport"
        Me.btnReport.Size = New System.Drawing.Size(75, 23)
        Me.btnReport.TabIndex = 17
        Me.btnReport.Text = "View Report"
        Me.btnReport.UseVisualStyleBackColor = True
        '
        'Form_POSPullJobController
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(438, 213)
        Me.Controls.Add(Me.btnReport)
        Me.Controls.Add(Me.DefaultFileNamesButton)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.NCRFileNameTextBox)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.IBMFileNameTextBox)
        Me.Controls.Add(Me.ExitButton)
        Me.Controls.Add(Me.ExceptionLabel)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.PullCatalogsButton)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ComboBox_Stores)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimizeBox = False
        Me.Name = "Form_POSPullJobController"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "POS Pull"
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ComboBox_Stores As System.Windows.Forms.ComboBox
    Friend WithEvents PullCatalogsButton As System.Windows.Forms.Button
    Friend WithEvents StatusStrip1 As System.Windows.Forms.StatusStrip
    Friend WithEvents StatusStrip As System.Windows.Forms.ToolStripStatusLabel
    Friend WithEvents ExceptionLabel As System.Windows.Forms.Label
    Friend WithEvents ExitButton As System.Windows.Forms.Button
    Friend WithEvents IBMFileNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents NCRFileNameTextBox As System.Windows.Forms.TextBox
    Friend WithEvents DefaultFileNamesButton As System.Windows.Forms.Button
    Friend WithEvents btnReport As System.Windows.Forms.Button
End Class
