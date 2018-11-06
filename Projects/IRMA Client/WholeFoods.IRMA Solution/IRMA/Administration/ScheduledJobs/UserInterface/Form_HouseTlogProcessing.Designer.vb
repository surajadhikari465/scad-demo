<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_HouseTlogProcessing
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
        Me.DateTime_ParseDate = New System.Windows.Forms.DateTimePicker
        Me.Button_Parse = New System.Windows.Forms.Button
        Me.ListView_logs = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip
        Me.LogsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.SaveToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ClearToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem
        Me.ProgressBar_Tlog = New System.Windows.Forms.ProgressBar
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ComboBox_Stores
        '
        Me.ComboBox_Stores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Stores.FormattingEnabled = True
        Me.ComboBox_Stores.Location = New System.Drawing.Point(50, 36)
        Me.ComboBox_Stores.Name = "ComboBox_Stores"
        Me.ComboBox_Stores.Size = New System.Drawing.Size(165, 21)
        Me.ComboBox_Stores.TabIndex = 0
        '
        'DateTime_ParseDate
        '
        Me.DateTime_ParseDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTime_ParseDate.Location = New System.Drawing.Point(286, 35)
        Me.DateTime_ParseDate.Name = "DateTime_ParseDate"
        Me.DateTime_ParseDate.Size = New System.Drawing.Size(165, 22)
        Me.DateTime_ParseDate.TabIndex = 1
        '
        'Button_Parse
        '
        Me.Button_Parse.Location = New System.Drawing.Point(484, 35)
        Me.Button_Parse.Name = "Button_Parse"
        Me.Button_Parse.Size = New System.Drawing.Size(164, 21)
        Me.Button_Parse.TabIndex = 2
        Me.Button_Parse.Text = "Parse Tlogs"
        Me.Button_Parse.UseVisualStyleBackColor = True
        '
        'ListView_logs
        '
        Me.ListView_logs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView_logs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.ListView_logs.GridLines = True
        Me.ListView_logs.Location = New System.Drawing.Point(12, 76)
        Me.ListView_logs.Name = "ListView_logs"
        Me.ListView_logs.ShowGroups = False
        Me.ListView_logs.Size = New System.Drawing.Size(636, 131)
        Me.ListView_logs.TabIndex = 3
        Me.ListView_logs.UseCompatibleStateImageBehavior = False
        Me.ListView_logs.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Log"
        Me.ColumnHeader1.Width = 600
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(9, 39)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(37, 13)
        Me.Label1.TabIndex = 4
        Me.Label1.Text = "Store:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(247, 39)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Date:"
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.LogsToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(661, 24)
        Me.MenuStrip1.TabIndex = 6
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'LogsToolStripMenuItem
        '
        Me.LogsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SaveToolStripMenuItem, Me.ClearToolStripMenuItem})
        Me.LogsToolStripMenuItem.Name = "LogsToolStripMenuItem"
        Me.LogsToolStripMenuItem.Size = New System.Drawing.Size(44, 20)
        Me.LogsToolStripMenuItem.Text = "Logs"
        '
        'SaveToolStripMenuItem
        '
        Me.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem"
        Me.SaveToolStripMenuItem.Size = New System.Drawing.Size(101, 22)
        Me.SaveToolStripMenuItem.Text = "Save"
        '
        'ClearToolStripMenuItem
        '
        Me.ClearToolStripMenuItem.Name = "ClearToolStripMenuItem"
        Me.ClearToolStripMenuItem.Size = New System.Drawing.Size(101, 22)
        Me.ClearToolStripMenuItem.Text = "Clear"
        '
        'ProgressBar_Tlog
        '
        Me.ProgressBar_Tlog.Location = New System.Drawing.Point(13, 61)
        Me.ProgressBar_Tlog.Name = "ProgressBar_Tlog"
        Me.ProgressBar_Tlog.Size = New System.Drawing.Size(636, 10)
        Me.ProgressBar_Tlog.TabIndex = 7
        '
        'Form_HouseTlogProcessing
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(661, 219)
        Me.Controls.Add(Me.ProgressBar_Tlog)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ListView_logs)
        Me.Controls.Add(Me.Button_Parse)
        Me.Controls.Add(Me.DateTime_ParseDate)
        Me.Controls.Add(Me.ComboBox_Stores)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MinimizeBox = False
        Me.Name = "Form_HouseTlogProcessing"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Tlog Parsing"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents ComboBox_Stores As System.Windows.Forms.ComboBox
    Friend WithEvents DateTime_ParseDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents Button_Parse As System.Windows.Forms.Button
    Friend WithEvents ListView_logs As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents MenuStrip1 As System.Windows.Forms.MenuStrip
    Friend WithEvents LogsToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents SaveToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents ClearToolStripMenuItem As System.Windows.Forms.ToolStripMenuItem

    Private Sub Form_HouseTlogProcessing_Disposed(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Disposed
        If Not _BackgroundWorker Is Nothing Then
            _BackgroundWorker.WorkerSupportsCancellation = True
            _BackgroundWorker.CancelAsync()
            _BackgroundWorker.Dispose()
        End If
    End Sub
    Friend WithEvents ProgressBar_Tlog As System.Windows.Forms.ProgressBar
End Class
