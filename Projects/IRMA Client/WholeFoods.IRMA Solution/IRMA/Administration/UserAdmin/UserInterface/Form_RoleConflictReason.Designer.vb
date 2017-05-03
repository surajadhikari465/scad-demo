<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_RoleConflictReasons
    Inherits Form_IRMABase

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_RoleConflictReasons))
        Me.Button_OK = New System.Windows.Forms.Button
        Me.grdConflicts = New System.Windows.Forms.DataGridView
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label_Notification = New System.Windows.Forms.Label
        Me.Button_Cancel = New System.Windows.Forms.Button
        CType(Me.grdConflicts, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Button_OK
        '
        Me.Button_OK.Location = New System.Drawing.Point(421, 337)
        Me.Button_OK.Name = "Button_OK"
        Me.Button_OK.Size = New System.Drawing.Size(56, 31)
        Me.Button_OK.TabIndex = 7
        Me.Button_OK.Text = "OK"
        Me.Button_OK.UseVisualStyleBackColor = True
        '
        'grdConflicts
        '
        Me.grdConflicts.AllowUserToAddRows = False
        Me.grdConflicts.AllowUserToDeleteRows = False
        Me.grdConflicts.AllowUserToResizeColumns = False
        Me.grdConflicts.AllowUserToResizeRows = False
        Me.grdConflicts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.grdConflicts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.grdConflicts.Location = New System.Drawing.Point(10, 56)
        Me.grdConflicts.Name = "grdConflicts"
        Me.grdConflicts.RowHeadersVisible = False
        Me.grdConflicts.Size = New System.Drawing.Size(550, 257)
        Me.grdConflicts.TabIndex = 28
        '
        'Label1
        '
        Me.Label1.Location = New System.Drawing.Point(12, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(559, 35)
        Me.Label1.TabIndex = 29
        Me.Label1.Text = "The following SOX conflicts have been detected.  Please fill in the reason for th" & _
            "e conflict in the grid below."
        '
        'Label_Notification
        '
        Me.Label_Notification.Location = New System.Drawing.Point(12, 333)
        Me.Label_Notification.Name = "Label_Notification"
        Me.Label_Notification.Size = New System.Drawing.Size(400, 35)
        Me.Label_Notification.TabIndex = 30
        Me.Label_Notification.Text = "A notification e-mail will be sent to {0} upon acceptance of the risk."
        '
        'Button_Cancel
        '
        Me.Button_Cancel.Location = New System.Drawing.Point(494, 337)
        Me.Button_Cancel.Name = "Button_Cancel"
        Me.Button_Cancel.Size = New System.Drawing.Size(66, 31)
        Me.Button_Cancel.TabIndex = 31
        Me.Button_Cancel.Text = "Cancel"
        Me.Button_Cancel.UseVisualStyleBackColor = True
        '
        'Form_RoleConflictReasons
        '
        Me.AcceptButton = Me.Button_OK
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(569, 380)
        Me.Controls.Add(Me.Button_Cancel)
        Me.Controls.Add(Me.Label_Notification)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.grdConflicts)
        Me.Controls.Add(Me.Button_OK)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "Form_RoleConflictReasons"
        Me.ShowInTaskbar = False
        Me.Text = "Role Conflict Reasons"
        CType(Me.grdConflicts, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Button_OK As System.Windows.Forms.Button
    Friend WithEvents grdConflicts As System.Windows.Forms.DataGridView
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label_Notification As System.Windows.Forms.Label
    Friend WithEvents Button_Cancel As System.Windows.Forms.Button

End Class
