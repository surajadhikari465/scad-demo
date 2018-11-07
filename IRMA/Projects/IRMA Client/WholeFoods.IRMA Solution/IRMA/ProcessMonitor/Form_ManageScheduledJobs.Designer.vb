<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ManageScheduledJobs
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_ManageScheduledJobs))
        Me.DataGridView_Jobs = New System.Windows.Forms.DataGridView
        Me.GroupBox_Jobs = New System.Windows.Forms.GroupBox
        Me.cmdRefresh = New System.Windows.Forms.Button
        Me.Button_ErrorLog = New System.Windows.Forms.Button
        Me.Button_Reset = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        CType(Me.DataGridView_Jobs, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox_Jobs.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridView_Jobs
        '
        Me.DataGridView_Jobs.AllowUserToAddRows = False
        Me.DataGridView_Jobs.AllowUserToDeleteRows = False
        Me.DataGridView_Jobs.AllowUserToOrderColumns = True
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.DataGridView_Jobs.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView_Jobs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView_Jobs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridView_Jobs.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DataGridView_Jobs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView_Jobs.Location = New System.Drawing.Point(6, 19)
        Me.DataGridView_Jobs.MultiSelect = False
        Me.DataGridView_Jobs.Name = "DataGridView_Jobs"
        Me.DataGridView_Jobs.ReadOnly = True
        Me.DataGridView_Jobs.RowHeadersVisible = False
        Me.DataGridView_Jobs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView_Jobs.Size = New System.Drawing.Size(611, 308)
        Me.DataGridView_Jobs.TabIndex = 7
        '
        'GroupBox_Jobs
        '
        Me.GroupBox_Jobs.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox_Jobs.Controls.Add(Me.cmdRefresh)
        Me.GroupBox_Jobs.Controls.Add(Me.Button_ErrorLog)
        Me.GroupBox_Jobs.Controls.Add(Me.Button_Reset)
        Me.GroupBox_Jobs.Controls.Add(Me.DataGridView_Jobs)
        Me.GroupBox_Jobs.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox_Jobs.Name = "GroupBox_Jobs"
        Me.GroupBox_Jobs.Size = New System.Drawing.Size(722, 333)
        Me.GroupBox_Jobs.TabIndex = 8
        Me.GroupBox_Jobs.TabStop = False
        Me.GroupBox_Jobs.Text = "Current Job Status"
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdRefresh.Image = CType(resources.GetObject("cmdRefresh.Image"), System.Drawing.Image)
        Me.cmdRefresh.Location = New System.Drawing.Point(623, 19)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.Size = New System.Drawing.Size(93, 37)
        Me.cmdRefresh.TabIndex = 10
        Me.cmdRefresh.Text = "Refresh"
        Me.cmdRefresh.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.cmdRefresh.UseVisualStyleBackColor = True
        '
        'Button_ErrorLog
        '
        Me.Button_ErrorLog.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_ErrorLog.Image = CType(resources.GetObject("Button_ErrorLog.Image"), System.Drawing.Image)
        Me.Button_ErrorLog.Location = New System.Drawing.Point(623, 62)
        Me.Button_ErrorLog.Name = "Button_ErrorLog"
        Me.Button_ErrorLog.Size = New System.Drawing.Size(93, 37)
        Me.Button_ErrorLog.TabIndex = 9
        Me.Button_ErrorLog.Text = "Error Log"
        Me.Button_ErrorLog.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTip1.SetToolTip(Me.Button_ErrorLog, "View Error Log History for Selected Job")
        Me.Button_ErrorLog.UseVisualStyleBackColor = True
        '
        'Button_Reset
        '
        Me.Button_Reset.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Reset.Enabled = False
        Me.Button_Reset.Image = CType(resources.GetObject("Button_Reset.Image"), System.Drawing.Image)
        Me.Button_Reset.Location = New System.Drawing.Point(623, 105)
        Me.Button_Reset.Name = "Button_Reset"
        Me.Button_Reset.Size = New System.Drawing.Size(93, 37)
        Me.Button_Reset.TabIndex = 8
        Me.Button_Reset.Text = "Reset Job"
        Me.Button_Reset.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.ToolTip1.SetToolTip(Me.Button_Reset, "Reset Failed Job to Allow Execution")
        Me.Button_Reset.UseVisualStyleBackColor = True
        '
        'Form_ManageScheduledJobs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(746, 357)
        Me.Controls.Add(Me.GroupBox_Jobs)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(760, 395)
        Me.Name = "Form_ManageScheduledJobs"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Process Monitor"
        CType(Me.DataGridView_Jobs, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox_Jobs.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents DataGridView_Jobs As System.Windows.Forms.DataGridView
    Friend WithEvents GroupBox_Jobs As System.Windows.Forms.GroupBox
    Friend WithEvents Button_Reset As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents Button_ErrorLog As System.Windows.Forms.Button
    Friend WithEvents cmdRefresh As System.Windows.Forms.Button
End Class
