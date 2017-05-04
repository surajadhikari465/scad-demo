<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ManagePOSWriter
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.DataGridView_ConfigItems = New System.Windows.Forms.DataGridView
        Me.Button_Close = New System.Windows.Forms.Button
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.FileWriterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AddFileWriterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EditSelectedFileWriterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DisableSelectedFileWriterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Button_Delete = New System.Windows.Forms.Button
        Me.Button_Edit = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Button_Add = New System.Windows.Forms.Button
        CType(Me.DataGridView_ConfigItems, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridView_ConfigItems
        '
        Me.DataGridView_ConfigItems.AllowUserToAddRows = False
        Me.DataGridView_ConfigItems.AllowUserToDeleteRows = False
        Me.DataGridView_ConfigItems.AllowUserToOrderColumns = True
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.DataGridView_ConfigItems.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView_ConfigItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridView_ConfigItems.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DataGridView_ConfigItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView_ConfigItems.Location = New System.Drawing.Point(20, 19)
        Me.DataGridView_ConfigItems.MultiSelect = False
        Me.DataGridView_ConfigItems.Name = "DataGridView_ConfigItems"
        Me.DataGridView_ConfigItems.ReadOnly = True
        Me.DataGridView_ConfigItems.Size = New System.Drawing.Size(632, 410)
        Me.DataGridView_ConfigItems.TabIndex = 7
        '
        'Button_Close
        '
        Me.Button_Close.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Close.Location = New System.Drawing.Point(670, 470)
        Me.Button_Close.Name = "Button_Close"
        Me.Button_Close.Size = New System.Drawing.Size(75, 23)
        Me.Button_Close.TabIndex = 9
        Me.Button_Close.Text = "Close"
        Me.Button_Close.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.FileWriterToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(656, 24)
        Me.MenuStrip1.TabIndex = 10
        Me.MenuStrip1.Text = "MenuStrip1"
        Me.MenuStrip1.Visible = False
        '
        'FileWriterToolStripMenuItem
        '
        Me.FileWriterToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddFileWriterToolStripMenuItem, Me.EditSelectedFileWriterToolStripMenuItem, Me.DisableSelectedFileWriterToolStripMenuItem})
        Me.FileWriterToolStripMenuItem.Name = "FileWriterToolStripMenuItem"
        Me.FileWriterToolStripMenuItem.Size = New System.Drawing.Size(72, 20)
        Me.FileWriterToolStripMenuItem.Text = "File Writer"
        '
        'AddFileWriterToolStripMenuItem
        '
        Me.AddFileWriterToolStripMenuItem.Name = "AddFileWriterToolStripMenuItem"
        Me.AddFileWriterToolStripMenuItem.Size = New System.Drawing.Size(210, 22)
        Me.AddFileWriterToolStripMenuItem.Text = "Add File Writer"
        '
        'EditSelectedFileWriterToolStripMenuItem
        '
        Me.EditSelectedFileWriterToolStripMenuItem.Name = "EditSelectedFileWriterToolStripMenuItem"
        Me.EditSelectedFileWriterToolStripMenuItem.Size = New System.Drawing.Size(210, 22)
        Me.EditSelectedFileWriterToolStripMenuItem.Text = "Edit Selected File Writer"
        '
        'DisableSelectedFileWriterToolStripMenuItem
        '
        Me.DisableSelectedFileWriterToolStripMenuItem.Name = "DisableSelectedFileWriterToolStripMenuItem"
        Me.DisableSelectedFileWriterToolStripMenuItem.Size = New System.Drawing.Size(210, 22)
        Me.DisableSelectedFileWriterToolStripMenuItem.Text = "Delete Selected File Writer"
        '
        'Button_Delete
        '
        Me.Button_Delete.Location = New System.Drawing.Point(658, 77)
        Me.Button_Delete.Name = "Button_Delete"
        Me.Button_Delete.Size = New System.Drawing.Size(75, 23)
        Me.Button_Delete.TabIndex = 12
        Me.Button_Delete.Text = "Delete"
        Me.Button_Delete.UseVisualStyleBackColor = True
        '
        'Button_Edit
        '
        Me.Button_Edit.Location = New System.Drawing.Point(658, 48)
        Me.Button_Edit.Name = "Button_Edit"
        Me.Button_Edit.Size = New System.Drawing.Size(75, 23)
        Me.Button_Edit.TabIndex = 11
        Me.Button_Edit.Text = "Edit"
        Me.Button_Edit.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button_Delete)
        Me.GroupBox1.Controls.Add(Me.Button_Edit)
        Me.GroupBox1.Controls.Add(Me.DataGridView_ConfigItems)
        Me.GroupBox1.Controls.Add(Me.Button_Add)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(742, 455)
        Me.GroupBox1.TabIndex = 14
        Me.GroupBox1.TabStop = False
        '
        'Button_Add
        '
        Me.Button_Add.Location = New System.Drawing.Point(658, 19)
        Me.Button_Add.Name = "Button_Add"
        Me.Button_Add.Size = New System.Drawing.Size(75, 23)
        Me.Button_Add.TabIndex = 10
        Me.Button_Add.Text = "Add"
        Me.Button_Add.UseVisualStyleBackColor = True
        '
        'Form_ManagePOSWriter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(769, 505)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button_Close)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "Form_ManagePOSWriter"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "View File Writers"
        CType(Me.DataGridView_ConfigItems, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents DataGridView_ConfigItems As System.Windows.Forms.DataGridView
    Friend WithEvents Button_Close As System.Windows.Forms.Button
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents FileWriterToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddFileWriterToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditSelectedFileWriterToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DisableSelectedFileWriterToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Button_Delete As System.Windows.Forms.Button
    Friend WithEvents Button_Edit As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Button_Add As System.Windows.Forms.Button

End Class
