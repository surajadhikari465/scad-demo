<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ManageStorePOSConfig
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
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.DataGridView_ConfigItems = New System.Windows.Forms.DataGridView
        Me.Button_Close = New System.Windows.Forms.Button
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.StoreToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.AddStoreToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.EditSelectedStoreToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.DeleteSelectedStoreToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Button_Add = New System.Windows.Forms.Button
        Me.Button_Edit = New System.Windows.Forms.Button
        Me.Button_Delete = New System.Windows.Forms.Button
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
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
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.DataGridView_ConfigItems.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle3
        Me.DataGridView_ConfigItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridView_ConfigItems.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DataGridView_ConfigItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView_ConfigItems.Location = New System.Drawing.Point(18, 19)
        Me.DataGridView_ConfigItems.MultiSelect = False
        Me.DataGridView_ConfigItems.Name = "DataGridView_ConfigItems"
        Me.DataGridView_ConfigItems.ReadOnly = True
        Me.DataGridView_ConfigItems.Size = New System.Drawing.Size(497, 410)
        Me.DataGridView_ConfigItems.TabIndex = 6
        '
        'Button_Close
        '
        Me.Button_Close.Location = New System.Drawing.Point(569, 474)
        Me.Button_Close.Name = "Button_Close"
        Me.Button_Close.Size = New System.Drawing.Size(75, 23)
        Me.Button_Close.TabIndex = 8
        Me.Button_Close.Text = "Close"
        Me.Button_Close.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StoreToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(656, 24)
        Me.MenuStrip1.TabIndex = 9
        Me.MenuStrip1.Text = "MenuStrip1"
        Me.MenuStrip1.Visible = False
        '
        'StoreToolStripMenuItem
        '
        Me.StoreToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddStoreToolStripMenuItem, Me.EditSelectedStoreToolStripMenuItem, Me.DeleteSelectedStoreToolStripMenuItem})
        Me.StoreToolStripMenuItem.Name = "StoreToolStripMenuItem"
        Me.StoreToolStripMenuItem.Size = New System.Drawing.Size(45, 20)
        Me.StoreToolStripMenuItem.Text = "Store"
        '
        'AddStoreToolStripMenuItem
        '
        Me.AddStoreToolStripMenuItem.Name = "AddStoreToolStripMenuItem"
        Me.AddStoreToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.AddStoreToolStripMenuItem.Text = "Add Store"
        '
        'EditSelectedStoreToolStripMenuItem
        '
        Me.EditSelectedStoreToolStripMenuItem.Name = "EditSelectedStoreToolStripMenuItem"
        Me.EditSelectedStoreToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.EditSelectedStoreToolStripMenuItem.Text = "Edit Selected Store"
        '
        'DeleteSelectedStoreToolStripMenuItem
        '
        Me.DeleteSelectedStoreToolStripMenuItem.Name = "DeleteSelectedStoreToolStripMenuItem"
        Me.DeleteSelectedStoreToolStripMenuItem.Size = New System.Drawing.Size(189, 22)
        Me.DeleteSelectedStoreToolStripMenuItem.Text = "Delete Selected Store"
        '
        'Button_Add
        '
        Me.Button_Add.Location = New System.Drawing.Point(535, 19)
        Me.Button_Add.Name = "Button_Add"
        Me.Button_Add.Size = New System.Drawing.Size(75, 23)
        Me.Button_Add.TabIndex = 10
        Me.Button_Add.Text = "Add"
        Me.Button_Add.UseVisualStyleBackColor = True
        '
        'Button_Edit
        '
        Me.Button_Edit.Location = New System.Drawing.Point(535, 48)
        Me.Button_Edit.Name = "Button_Edit"
        Me.Button_Edit.Size = New System.Drawing.Size(75, 23)
        Me.Button_Edit.TabIndex = 11
        Me.Button_Edit.Text = "Edit"
        Me.Button_Edit.UseVisualStyleBackColor = True
        '
        'Button_Delete
        '
        Me.Button_Delete.Location = New System.Drawing.Point(535, 77)
        Me.Button_Delete.Name = "Button_Delete"
        Me.Button_Delete.Size = New System.Drawing.Size(75, 23)
        Me.Button_Delete.TabIndex = 12
        Me.Button_Delete.Text = "Delete"
        Me.Button_Delete.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button_Delete)
        Me.GroupBox1.Controls.Add(Me.Button_Edit)
        Me.GroupBox1.Controls.Add(Me.Button_Add)
        Me.GroupBox1.Controls.Add(Me.DataGridView_ConfigItems)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(632, 455)
        Me.GroupBox1.TabIndex = 13
        Me.GroupBox1.TabStop = False
        '
        'Form_StorePOSConfig
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.ClientSize = New System.Drawing.Size(656, 521)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Button_Close)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form_StorePOSConfig"
        Me.Text = "View Stores"
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
    Friend WithEvents StoreToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents AddStoreToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents EditSelectedStoreToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents DeleteSelectedStoreToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Button_Add As System.Windows.Forms.Button
    Friend WithEvents Button_Edit As System.Windows.Forms.Button
    Friend WithEvents Button_Delete As System.Windows.Forms.Button
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox

End Class
