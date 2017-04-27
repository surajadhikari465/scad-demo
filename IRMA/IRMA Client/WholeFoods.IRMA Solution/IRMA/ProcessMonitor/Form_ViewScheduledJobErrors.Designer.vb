<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ViewScheduledJobErrors
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_ViewScheduledJobErrors))
        Me.GroupBox_Log = New System.Windows.Forms.GroupBox
        Me.DataGridView_Errors = New System.Windows.Forms.DataGridView
        Me.cmdCopy = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.GroupBox_Log.SuspendLayout()
        CType(Me.DataGridView_Errors, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox_Log
        '
        Me.GroupBox_Log.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox_Log.Controls.Add(Me.DataGridView_Errors)
        Me.GroupBox_Log.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_Log.Name = "GroupBox_Log"
        Me.GroupBox_Log.Size = New System.Drawing.Size(647, 333)
        Me.GroupBox_Log.TabIndex = 9
        Me.GroupBox_Log.TabStop = False
        Me.GroupBox_Log.Text = "Error Log for Job"
        '
        'DataGridView_Errors
        '
        Me.DataGridView_Errors.AllowUserToAddRows = False
        Me.DataGridView_Errors.AllowUserToDeleteRows = False
        Me.DataGridView_Errors.AllowUserToOrderColumns = True
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.DataGridView_Errors.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView_Errors.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView_Errors.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridView_Errors.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DataGridView_Errors.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView_Errors.Location = New System.Drawing.Point(6, 19)
        Me.DataGridView_Errors.MultiSelect = False
        Me.DataGridView_Errors.Name = "DataGridView_Errors"
        Me.DataGridView_Errors.ReadOnly = True
        Me.DataGridView_Errors.RowHeadersVisible = False
        Me.DataGridView_Errors.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView_Errors.Size = New System.Drawing.Size(635, 308)
        Me.DataGridView_Errors.TabIndex = 7
        '
        'cmdCopy
        '
        Me.cmdCopy.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCopy.Image = CType(resources.GetObject("cmdCopy.Image"), System.Drawing.Image)
        Me.cmdCopy.Location = New System.Drawing.Point(665, 31)
        Me.cmdCopy.Name = "cmdCopy"
        Me.cmdCopy.Size = New System.Drawing.Size(67, 38)
        Me.cmdCopy.TabIndex = 10
        Me.cmdCopy.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTip1.SetToolTip(Me.cmdCopy, "Copy Exception Text")
        Me.cmdCopy.UseVisualStyleBackColor = True
        '
        'Form_ViewScheduledJobErrors
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(744, 357)
        Me.Controls.Add(Me.cmdCopy)
        Me.Controls.Add(Me.GroupBox_Log)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(760, 395)
        Me.Name = "Form_ViewScheduledJobErrors"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Scheduled Job Error Log"
        Me.GroupBox_Log.ResumeLayout(False)
        CType(Me.DataGridView_Errors, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox_Log As System.Windows.Forms.GroupBox
    Public WithEvents DataGridView_Errors As System.Windows.Forms.DataGridView
    Friend WithEvents cmdCopy As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
End Class
