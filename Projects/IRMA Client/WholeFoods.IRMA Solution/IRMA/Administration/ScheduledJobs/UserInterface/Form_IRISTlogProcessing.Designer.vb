<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_IRISTlogProcessing
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
        Me.ComboBox_Stores = New System.Windows.Forms.ComboBox
        Me.ProgressBar_Progress = New System.Windows.Forms.ProgressBar
        Me.Button_Process = New System.Windows.Forms.Button
        Me.Label_Msg = New System.Windows.Forms.Label
        Me.ListBox_Files = New System.Windows.Forms.ListBox
        Me.DateTime_FileDate = New System.Windows.Forms.DateTimePicker
        Me.ListView_Log = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.LogToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveLogMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ClearToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label3 = New System.Windows.Forms.Label
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ComboBox_Stores
        '
        Me.ComboBox_Stores.FormattingEnabled = True
        Me.ComboBox_Stores.Location = New System.Drawing.Point(7, 49)
        Me.ComboBox_Stores.Name = "ComboBox_Stores"
        Me.ComboBox_Stores.Size = New System.Drawing.Size(191, 21)
        Me.ComboBox_Stores.TabIndex = 4
        '
        'ProgressBar_Progress
        '
        Me.ProgressBar_Progress.Location = New System.Drawing.Point(7, 115)
        Me.ProgressBar_Progress.Name = "ProgressBar_Progress"
        Me.ProgressBar_Progress.Size = New System.Drawing.Size(191, 16)
        Me.ProgressBar_Progress.TabIndex = 8
        '
        'Button_Process
        '
        Me.Button_Process.AutoSize = True
        Me.Button_Process.Location = New System.Drawing.Point(7, 208)
        Me.Button_Process.Name = "Button_Process"
        Me.Button_Process.Size = New System.Drawing.Size(191, 23)
        Me.Button_Process.TabIndex = 9
        Me.Button_Process.Text = "Process Tlogs"
        Me.Button_Process.UseVisualStyleBackColor = True
        '
        'Label_Msg
        '
        Me.Label_Msg.Location = New System.Drawing.Point(4, 134)
        Me.Label_Msg.Name = "Label_Msg"
        Me.Label_Msg.Size = New System.Drawing.Size(194, 71)
        Me.Label_Msg.TabIndex = 10
        '
        'ListBox_Files
        '
        Me.ListBox_Files.FormattingEnabled = True
        Me.ListBox_Files.Location = New System.Drawing.Point(204, 45)
        Me.ListBox_Files.Name = "ListBox_Files"
        Me.ListBox_Files.SelectionMode = System.Windows.Forms.SelectionMode.MultiSimple
        Me.ListBox_Files.Size = New System.Drawing.Size(191, 186)
        Me.ListBox_Files.TabIndex = 11
        '
        'DateTime_FileDate
        '
        Me.DateTime_FileDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTime_FileDate.Location = New System.Drawing.Point(7, 89)
        Me.DateTime_FileDate.Name = "DateTime_FileDate"
        Me.DateTime_FileDate.Size = New System.Drawing.Size(191, 22)
        Me.DateTime_FileDate.TabIndex = 12
        '
        'ListView_Log
        '
        Me.ListView_Log.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.ListView_Log.GridLines = True
        Me.ListView_Log.Location = New System.Drawing.Point(7, 237)
        Me.ListView_Log.Name = "ListView_Log"
        Me.ListView_Log.Size = New System.Drawing.Size(388, 146)
        Me.ListView_Log.TabIndex = 13
        Me.ListView_Log.UseCompatibleStateImageBehavior = False
        Me.ListView_Log.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Log"
        Me.ColumnHeader1.Width = 384
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LogToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(401, 24)
        Me.MenuStrip1.TabIndex = 14
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'LogToolStripMenuItem
        '
        Me.LogToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveLogMenuItem, Me.ClearToolStripMenuItem})
        Me.LogToolStripMenuItem.Name = "LogToolStripMenuItem"
        Me.LogToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
        Me.LogToolStripMenuItem.Text = "Log"
        '
        'SaveLogMenuItem
        '
        Me.SaveLogMenuItem.Name = "SaveLogMenuItem"
        Me.SaveLogMenuItem.Size = New System.Drawing.Size(101, 22)
        Me.SaveLogMenuItem.Text = "Save"
        '
        'ClearToolStripMenuItem
        '
        Me.ClearToolStripMenuItem.Name = "ClearToolStripMenuItem"
        Me.ClearToolStripMenuItem.Size = New System.Drawing.Size(101, 22)
        Me.ClearToolStripMenuItem.Text = "Clear"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(4, 73)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 15
        Me.Label1.Text = "Date:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(4, 33)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(37, 13)
        Me.Label2.TabIndex = 16
        Me.Label2.Text = "Store:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(201, 33)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(48, 13)
        Me.Label3.TabIndex = 17
        Me.Label3.Text = "File List:"
        '
        'Form_IRISTlogProcessing
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(401, 388)
        Me.Controls.Add(Me.ListBox_Files)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ListView_Log)
        Me.Controls.Add(Me.DateTime_FileDate)
        Me.Controls.Add(Me.Label_Msg)
        Me.Controls.Add(Me.Button_Process)
        Me.Controls.Add(Me.ComboBox_Stores)
        Me.Controls.Add(Me.ProgressBar_Progress)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MinimizeBox = False
        Me.Name = "Form_IRISTlogProcessing"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Process Tlogs"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboBox_Stores As System.Windows.Forms.ComboBox
    Friend WithEvents ProgressBar_Progress As System.Windows.Forms.ProgressBar
    Friend WithEvents Button_Process As System.Windows.Forms.Button
    Friend WithEvents Label_Msg As System.Windows.Forms.Label
    Friend WithEvents ListBox_Files As System.Windows.Forms.ListBox
    Friend WithEvents DateTime_FileDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents ListView_Log As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents LogToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveLogMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ClearToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label3 As System.Windows.Forms.Label
End Class
