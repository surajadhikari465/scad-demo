<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ConfigurationData_View
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_ConfigurationData_View))
        Me._formWebBrowser = New System.Windows.Forms.WebBrowser
        Me._buttonCopy = New System.Windows.Forms.Button
        Me._buttonExport = New System.Windows.Forms.Button
        Me._dialogSaveFile = New System.Windows.Forms.SaveFileDialog
        Me._formToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.SuspendLayout()
        '
        '_formWebBrowser
        '
        Me._formWebBrowser.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me._formWebBrowser.Location = New System.Drawing.Point(12, 40)
        Me._formWebBrowser.MinimumSize = New System.Drawing.Size(20, 20)
        Me._formWebBrowser.Name = "_formWebBrowser"
        Me._formWebBrowser.Size = New System.Drawing.Size(860, 412)
        Me._formWebBrowser.TabIndex = 0
        '
        '_buttonCopy
        '
        Me._buttonCopy.Image = CType(resources.GetObject("_buttonCopy.Image"), System.Drawing.Image)
        Me._buttonCopy.Location = New System.Drawing.Point(12, 7)
        Me._buttonCopy.Name = "_buttonCopy"
        Me._buttonCopy.Size = New System.Drawing.Size(50, 27)
        Me._buttonCopy.TabIndex = 1
        Me._buttonCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me._formToolTip.SetToolTip(Me._buttonCopy, "Copy this configuration to the Clipboard.")
        Me._buttonCopy.UseVisualStyleBackColor = True
        '
        '_buttonExport
        '
        Me._buttonExport.Image = CType(resources.GetObject("_buttonExport.Image"), System.Drawing.Image)
        Me._buttonExport.Location = New System.Drawing.Point(68, 7)
        Me._buttonExport.Name = "_buttonExport"
        Me._buttonExport.Size = New System.Drawing.Size(50, 27)
        Me._buttonExport.TabIndex = 2
        Me._buttonExport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me._formToolTip.SetToolTip(Me._buttonExport, "Export this configuration to a file.")
        Me._buttonExport.UseVisualStyleBackColor = True
        '
        'Form_ConfigurationData_View
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(884, 464)
        Me.Controls.Add(Me._buttonExport)
        Me.Controls.Add(Me._buttonCopy)
        Me.Controls.Add(Me._formWebBrowser)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form_ConfigurationData_View"
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "{0}"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents _formWebBrowser As System.Windows.Forms.WebBrowser
    Friend WithEvents _buttonCopy As System.Windows.Forms.Button
    Friend WithEvents _buttonExport As System.Windows.Forms.Button
    Friend WithEvents _dialogSaveFile As System.Windows.Forms.SaveFileDialog
    Friend WithEvents _formToolTip As System.Windows.Forms.ToolTip
End Class
