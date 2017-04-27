<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Form_TlogParsing
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
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.ListView_logs = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.Button_Parse = New System.Windows.Forms.Button
        Me.DateTime_ParseDate = New System.Windows.Forms.DateTimePicker
        Me.ComboBox_Stores = New System.Windows.Forms.ComboBox
        Me.GroupBox1 = New System.Windows.Forms.GroupBox
        Me.Label_ConfiguredParser = New System.Windows.Forms.Label
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(233, 25)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(34, 13)
        Me.Label2.TabIndex = 11
        Me.Label2.Text = "Date:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(49, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(37, 13)
        Me.Label1.TabIndex = 10
        Me.Label1.Text = "Store:"
        '
        'ListView_logs
        '
        Me.ListView_logs.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView_logs.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.ListView_logs.GridLines = True
        Me.ListView_logs.Location = New System.Drawing.Point(49, 112)
        Me.ListView_logs.Name = "ListView_logs"
        Me.ListView_logs.ShowGroups = False
        Me.ListView_logs.Size = New System.Drawing.Size(352, 198)
        Me.ListView_logs.TabIndex = 9
        Me.ListView_logs.UseCompatibleStateImageBehavior = False
        Me.ListView_logs.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Log"
        Me.ColumnHeader1.Width = 600
        '
        'Button_Parse
        '
        Me.Button_Parse.Location = New System.Drawing.Point(49, 67)
        Me.Button_Parse.Name = "Button_Parse"
        Me.Button_Parse.Size = New System.Drawing.Size(164, 39)
        Me.Button_Parse.TabIndex = 8
        Me.Button_Parse.Text = "Parse Tlogs"
        Me.Button_Parse.UseVisualStyleBackColor = True
        '
        'DateTime_ParseDate
        '
        Me.DateTime_ParseDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.DateTime_ParseDate.Location = New System.Drawing.Point(236, 41)
        Me.DateTime_ParseDate.Name = "DateTime_ParseDate"
        Me.DateTime_ParseDate.Size = New System.Drawing.Size(165, 22)
        Me.DateTime_ParseDate.TabIndex = 7
        '
        'ComboBox_Stores
        '
        Me.ComboBox_Stores.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ComboBox_Stores.FormattingEnabled = True
        Me.ComboBox_Stores.Location = New System.Drawing.Point(49, 40)
        Me.ComboBox_Stores.Name = "ComboBox_Stores"
        Me.ComboBox_Stores.Size = New System.Drawing.Size(165, 21)
        Me.ComboBox_Stores.TabIndex = 6
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Label_ConfiguredParser)
        Me.GroupBox1.Location = New System.Drawing.Point(236, 67)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(165, 39)
        Me.GroupBox1.TabIndex = 12
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Configured Parser Type:"
        '
        'Label_ConfiguredParser
        '
        Me.Label_ConfiguredParser.AutoSize = True
        Me.Label_ConfiguredParser.Location = New System.Drawing.Point(6, 16)
        Me.Label_ConfiguredParser.Name = "Label_ConfiguredParser"
        Me.Label_ConfiguredParser.Size = New System.Drawing.Size(0, 13)
        Me.Label_ConfiguredParser.TabIndex = 13
        '
        'Form_TlogParsing
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(451, 335)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.ListView_logs)
        Me.Controls.Add(Me.Button_Parse)
        Me.Controls.Add(Me.DateTime_ParseDate)
        Me.Controls.Add(Me.ComboBox_Stores)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MinimizeBox = False
        Me.Name = "Form_TlogParsing"
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Form_TlogParsing"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ListView_logs As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Button_Parse As System.Windows.Forms.Button
    Friend WithEvents DateTime_ParseDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents ComboBox_Stores As System.Windows.Forms.ComboBox
    Friend WithEvents GroupBox1 As System.Windows.Forms.GroupBox
    Friend WithEvents Label_ConfiguredParser As System.Windows.Forms.Label
End Class
