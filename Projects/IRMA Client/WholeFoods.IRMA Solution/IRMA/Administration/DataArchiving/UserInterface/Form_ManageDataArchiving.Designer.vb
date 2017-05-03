<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_ManageDataArchiving
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form_ManageDataArchiving))
        Dim Button_Add As System.Windows.Forms.Button
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle
        Me.Label_FilterStatus = New System.Windows.Forms.Label
        Me.Radio_FilterStatusEnabled = New System.Windows.Forms.RadioButton
        Me.Radio_FilterStatusDisabled = New System.Windows.Forms.RadioButton
        Me.Radio_FiterStatusAll = New System.Windows.Forms.RadioButton
        Me.ComboBox_FilterTable = New System.Windows.Forms.ComboBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Button_Delete = New System.Windows.Forms.Button
        Me.Button_Edit = New System.Windows.Forms.Button
        Me.DataGridView_ArchiveTables = New System.Windows.Forms.DataGridView
        Me.Label_FilterTable = New System.Windows.Forms.Label
        Me.Button_ClearFilter = New System.Windows.Forms.Button
        Me.Group_FilterOptions = New System.Windows.Forms.GroupBox
        Me.Button_Close = New System.Windows.Forms.Button
        Me.Button_ApplyFilter = New System.Windows.Forms.Button
        Me.Button_ManageTable = New System.Windows.Forms.Button
        Button_Add = New System.Windows.Forms.Button
        Me.GroupBox1.SuspendLayout()
        CType(Me.DataGridView_ArchiveTables, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Group_FilterOptions.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label_FilterStatus
        '
        Me.Label_FilterStatus.AutoSize = True
        Me.Label_FilterStatus.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_FilterStatus.Location = New System.Drawing.Point(569, 31)
        Me.Label_FilterStatus.Name = "Label_FilterStatus"
        Me.Label_FilterStatus.Size = New System.Drawing.Size(42, 13)
        Me.Label_FilterStatus.TabIndex = 13
        Me.Label_FilterStatus.Text = "Status:"
        '
        'Radio_FilterStatusEnabled
        '
        Me.Radio_FilterStatusEnabled.AutoSize = True
        Me.Radio_FilterStatusEnabled.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Radio_FilterStatusEnabled.Location = New System.Drawing.Point(628, 55)
        Me.Radio_FilterStatusEnabled.Name = "Radio_FilterStatusEnabled"
        Me.Radio_FilterStatusEnabled.Size = New System.Drawing.Size(67, 17)
        Me.Radio_FilterStatusEnabled.TabIndex = 4
        Me.Radio_FilterStatusEnabled.Text = "Enabled"
        Me.Radio_FilterStatusEnabled.UseVisualStyleBackColor = True
        '
        'Radio_FilterStatusDisabled
        '
        Me.Radio_FilterStatusDisabled.AutoSize = True
        Me.Radio_FilterStatusDisabled.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Radio_FilterStatusDisabled.Location = New System.Drawing.Point(628, 84)
        Me.Radio_FilterStatusDisabled.Name = "Radio_FilterStatusDisabled"
        Me.Radio_FilterStatusDisabled.Size = New System.Drawing.Size(70, 17)
        Me.Radio_FilterStatusDisabled.TabIndex = 5
        Me.Radio_FilterStatusDisabled.Text = "Disabled"
        Me.Radio_FilterStatusDisabled.UseVisualStyleBackColor = True
        '
        'Radio_FiterStatusAll
        '
        Me.Radio_FiterStatusAll.AutoSize = True
        Me.Radio_FiterStatusAll.Checked = True
        Me.Radio_FiterStatusAll.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Radio_FiterStatusAll.Location = New System.Drawing.Point(628, 26)
        Me.Radio_FiterStatusAll.Name = "Radio_FiterStatusAll"
        Me.Radio_FiterStatusAll.Size = New System.Drawing.Size(38, 17)
        Me.Radio_FiterStatusAll.TabIndex = 3
        Me.Radio_FiterStatusAll.TabStop = True
        Me.Radio_FiterStatusAll.Text = "All"
        Me.Radio_FiterStatusAll.UseVisualStyleBackColor = True
        '
        'ComboBox_FilterTable
        '
        Me.ComboBox_FilterTable.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend
        Me.ComboBox_FilterTable.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems
        Me.ComboBox_FilterTable.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ComboBox_FilterTable.FormattingEnabled = True
        Me.ComboBox_FilterTable.Location = New System.Drawing.Point(68, 22)
        Me.ComboBox_FilterTable.Name = "ComboBox_FilterTable"
        Me.ComboBox_FilterTable.Size = New System.Drawing.Size(316, 21)
        Me.ComboBox_FilterTable.TabIndex = 2
        Me.ComboBox_FilterTable.Text = "All Tables"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Button_Delete)
        Me.GroupBox1.Controls.Add(Me.Button_Edit)
        Me.GroupBox1.Controls.Add(Button_Add)
        Me.GroupBox1.Controls.Add(Me.DataGridView_ArchiveTables)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 144)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(886, 443)
        Me.GroupBox1.TabIndex = 18
        Me.GroupBox1.TabStop = False
        '
        'Button_Delete
        '
        Me.Button_Delete.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Delete.Enabled = False
        Me.Button_Delete.Image = CType(resources.GetObject("Button_Delete.Image"), System.Drawing.Image)
        Me.Button_Delete.Location = New System.Drawing.Point(783, 95)
        Me.Button_Delete.Name = "Button_Delete"
        Me.Button_Delete.Size = New System.Drawing.Size(97, 30)
        Me.Button_Delete.TabIndex = 11
        Me.Button_Delete.Text = "Disable"
        Me.Button_Delete.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button_Delete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_Delete.UseVisualStyleBackColor = True
        '
        'Button_Edit
        '
        Me.Button_Edit.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Edit.Enabled = False
        Me.Button_Edit.Image = CType(resources.GetObject("Button_Edit.Image"), System.Drawing.Image)
        Me.Button_Edit.Location = New System.Drawing.Point(783, 55)
        Me.Button_Edit.Name = "Button_Edit"
        Me.Button_Edit.Size = New System.Drawing.Size(97, 30)
        Me.Button_Edit.TabIndex = 9
        Me.Button_Edit.Text = "Edit"
        Me.Button_Edit.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button_Edit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_Edit.UseVisualStyleBackColor = True
        '
        'Button_Add
        '
        Button_Add.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Button_Add.Image = CType(resources.GetObject("Button_Add.Image"), System.Drawing.Image)
        Button_Add.Location = New System.Drawing.Point(783, 16)
        Button_Add.Name = "Button_Add"
        Button_Add.Size = New System.Drawing.Size(97, 30)
        Button_Add.TabIndex = 8
        Button_Add.Text = "Add"
        Button_Add.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Button_Add.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Button_Add.UseVisualStyleBackColor = True
        '
        'DataGridView_ArchiveTables
        '
        Me.DataGridView_ArchiveTables.AllowUserToAddRows = False
        Me.DataGridView_ArchiveTables.AllowUserToDeleteRows = False
        Me.DataGridView_ArchiveTables.AllowUserToOrderColumns = True
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.DataGridView_ArchiveTables.AlternatingRowsDefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridView_ArchiveTables.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridView_ArchiveTables.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridView_ArchiveTables.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells
        Me.DataGridView_ArchiveTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridView_ArchiveTables.Location = New System.Drawing.Point(6, 16)
        Me.DataGridView_ArchiveTables.MultiSelect = False
        Me.DataGridView_ArchiveTables.Name = "DataGridView_ArchiveTables"
        Me.DataGridView_ArchiveTables.ReadOnly = True
        Me.DataGridView_ArchiveTables.RowHeadersVisible = False
        Me.DataGridView_ArchiveTables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridView_ArchiveTables.Size = New System.Drawing.Size(771, 388)
        Me.DataGridView_ArchiveTables.TabIndex = 10
        '
        'Label_FilterTable
        '
        Me.Label_FilterTable.AutoSize = True
        Me.Label_FilterTable.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label_FilterTable.Location = New System.Drawing.Point(31, 26)
        Me.Label_FilterTable.Name = "Label_FilterTable"
        Me.Label_FilterTable.Size = New System.Drawing.Size(31, 13)
        Me.Label_FilterTable.TabIndex = 8
        Me.Label_FilterTable.Text = "Title:"
        '
        'Button_ClearFilter
        '
        Me.Button_ClearFilter.Image = CType(resources.GetObject("Button_ClearFilter.Image"), System.Drawing.Image)
        Me.Button_ClearFilter.Location = New System.Drawing.Point(783, 54)
        Me.Button_ClearFilter.Name = "Button_ClearFilter"
        Me.Button_ClearFilter.Size = New System.Drawing.Size(97, 30)
        Me.Button_ClearFilter.TabIndex = 7
        Me.Button_ClearFilter.Text = "Clear Filter"
        Me.Button_ClearFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button_ClearFilter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_ClearFilter.UseVisualStyleBackColor = True
        '
        'Group_FilterOptions
        '
        Me.Group_FilterOptions.Controls.Add(Me.Button_ManageTable)
        Me.Group_FilterOptions.Controls.Add(Me.Button_ApplyFilter)
        Me.Group_FilterOptions.Controls.Add(Me.Label_FilterStatus)
        Me.Group_FilterOptions.Controls.Add(Me.Radio_FilterStatusEnabled)
        Me.Group_FilterOptions.Controls.Add(Me.Radio_FilterStatusDisabled)
        Me.Group_FilterOptions.Controls.Add(Me.Radio_FiterStatusAll)
        Me.Group_FilterOptions.Controls.Add(Me.ComboBox_FilterTable)
        Me.Group_FilterOptions.Controls.Add(Me.Label_FilterTable)
        Me.Group_FilterOptions.Controls.Add(Me.Button_ClearFilter)
        Me.Group_FilterOptions.Location = New System.Drawing.Point(12, 12)
        Me.Group_FilterOptions.Name = "Group_FilterOptions"
        Me.Group_FilterOptions.Size = New System.Drawing.Size(886, 126)
        Me.Group_FilterOptions.TabIndex = 16
        Me.Group_FilterOptions.TabStop = False
        Me.Group_FilterOptions.Text = "User Filter"
        '
        'Button_Close
        '
        Me.Button_Close.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_Close.Location = New System.Drawing.Point(795, 593)
        Me.Button_Close.Name = "Button_Close"
        Me.Button_Close.Size = New System.Drawing.Size(97, 34)
        Me.Button_Close.TabIndex = 17
        Me.Button_Close.Text = "Close"
        Me.Button_Close.UseVisualStyleBackColor = True
        '
        'Button_ApplyFilter
        '
        Me.Button_ApplyFilter.Image = CType(resources.GetObject("Button_ApplyFilter.Image"), System.Drawing.Image)
        Me.Button_ApplyFilter.Location = New System.Drawing.Point(783, 17)
        Me.Button_ApplyFilter.Name = "Button_ApplyFilter"
        Me.Button_ApplyFilter.Size = New System.Drawing.Size(97, 30)
        Me.Button_ApplyFilter.TabIndex = 14
        Me.Button_ApplyFilter.Text = "Apply Filter"
        Me.Button_ApplyFilter.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.Button_ApplyFilter.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_ApplyFilter.UseVisualStyleBackColor = True
        '
        'Button_ManageTable
        '
        Me.Button_ManageTable.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button_ManageTable.Location = New System.Drawing.Point(399, 22)
        Me.Button_ManageTable.Name = "Button_ManageTable"
        Me.Button_ManageTable.Size = New System.Drawing.Size(110, 30)
        Me.Button_ManageTable.TabIndex = 15
        Me.Button_ManageTable.Text = "Manage"
        Me.Button_ManageTable.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText
        Me.Button_ManageTable.UseVisualStyleBackColor = True
        '
        'Form_ManageDataArchiving
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(921, 648)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Group_FilterOptions)
        Me.Controls.Add(Me.Button_Close)
        Me.Name = "Form_ManageDataArchiving"
        Me.Text = "Form_ManageDataArchiving"
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.DataGridView_ArchiveTables, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Group_FilterOptions.ResumeLayout(False)
        Me.Group_FilterOptions.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label_FilterStatus As System.Windows.Forms.Label
    Friend WithEvents Radio_FilterStatusEnabled As System.Windows.Forms.RadioButton
    Friend WithEvents Radio_FilterStatusDisabled As System.Windows.Forms.RadioButton
    Friend WithEvents Radio_FiterStatusAll As System.Windows.Forms.RadioButton
    Friend WithEvents ComboBox_FilterTable As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Button_Delete As System.Windows.Forms.Button
    Friend WithEvents Button_Edit As System.Windows.Forms.Button
    Public WithEvents DataGridView_ArchiveTables As System.Windows.Forms.DataGridView
    Friend WithEvents Label_FilterTable As System.Windows.Forms.Label
    Friend WithEvents Button_ClearFilter As System.Windows.Forms.Button
    Friend WithEvents Group_FilterOptions As System.Windows.Forms.GroupBox
    Friend WithEvents Button_Close As System.Windows.Forms.Button
    Friend WithEvents Button_ApplyFilter As System.Windows.Forms.Button
    Friend WithEvents Button_ManageTable As System.Windows.Forms.Button
End Class
