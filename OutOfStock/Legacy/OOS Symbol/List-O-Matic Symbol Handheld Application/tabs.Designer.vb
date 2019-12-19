<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class tabs
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(tabs))
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.LocationTab = New System.Windows.Forms.TabPage
        Me.TabControl1 = New System.Windows.Forms.TabControl
        Me.ContextMenu1 = New System.Windows.Forms.ContextMenu
        Me.Button1 = New System.Windows.Forms.Button
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.ListBox1 = New System.Windows.Forms.ListBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.Label2 = New System.Windows.Forms.Label
        Me.Button3 = New System.Windows.Forms.Button
        Me.Button2 = New System.Windows.Forms.Button
        Me.ScanTab = New System.Windows.Forms.TabPage
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.ComboBox2 = New System.Windows.Forms.ComboBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.LocationTab.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.ScanTab.SuspendLayout()
        Me.SuspendLayout()
        '
        'LocationTab
        '
        Me.LocationTab.BackColor = System.Drawing.SystemColors.Info
        Me.LocationTab.Controls.Add(Me.Label3)
        Me.LocationTab.Controls.Add(Me.ComboBox1)
        Me.LocationTab.Controls.Add(Me.Label4)
        Me.LocationTab.Controls.Add(Me.ComboBox2)
        Me.LocationTab.Location = New System.Drawing.Point(0, 0)
        Me.LocationTab.Name = "LocationTab"
        Me.LocationTab.Size = New System.Drawing.Size(240, 242)
        Me.LocationTab.Text = "Location"
        '
        'TabControl1
        '
        Me.TabControl1.Controls.Add(Me.LocationTab)
        Me.TabControl1.Controls.Add(Me.ScanTab)
        Me.TabControl1.Location = New System.Drawing.Point(0, 0)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(240, 265)
        Me.TabControl1.TabIndex = 7
        '
        'Button1
        '
        Me.Button1.BackColor = System.Drawing.Color.DarkBlue
        Me.Button1.ForeColor = System.Drawing.Color.White
        Me.Button1.Location = New System.Drawing.Point(156, 26)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(77, 45)
        Me.Button1.TabIndex = 1
        Me.Button1.Text = "Enter"
        '
        'TextBox1
        '
        Me.TextBox1.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Bold)
        Me.TextBox1.Location = New System.Drawing.Point(11, 47)
        Me.TextBox1.MaxLength = 13
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(139, 24)
        Me.TextBox1.TabIndex = 0
        '
        'ListBox1
        '
        Me.ListBox1.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Bold)
        Me.ListBox1.Location = New System.Drawing.Point(13, 87)
        Me.ListBox1.Name = "ListBox1"
        Me.ListBox1.Size = New System.Drawing.Size(211, 92)
        Me.ListBox1.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(11, 24)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 20)
        Me.Label1.Text = "UPC:"
        Me.Label1.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label2.ForeColor = System.Drawing.Color.Black
        Me.Label2.Location = New System.Drawing.Point(65, 3)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(142, 20)
        Me.Label2.Text = "Label2"
        '
        'Button3
        '
        Me.Button3.BackColor = System.Drawing.Color.DarkBlue
        Me.Button3.ForeColor = System.Drawing.Color.White
        Me.Button3.Location = New System.Drawing.Point(7, 185)
        Me.Button3.Name = "Button3"
        Me.Button3.Size = New System.Drawing.Size(109, 44)
        Me.Button3.TabIndex = 5
        Me.Button3.Text = "Remove UPC"
        '
        'Button2
        '
        Me.Button2.BackColor = System.Drawing.Color.DarkBlue
        Me.Button2.ForeColor = System.Drawing.Color.White
        Me.Button2.Location = New System.Drawing.Point(138, 185)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(86, 44)
        Me.Button2.TabIndex = 4
        Me.Button2.Text = "Upload"
        '
        'ScanTab
        '
        Me.ScanTab.BackColor = System.Drawing.SystemColors.Info
        Me.ScanTab.Controls.Add(Me.Label6)
        Me.ScanTab.Controls.Add(Me.Button2)
        Me.ScanTab.Controls.Add(Me.Button3)
        Me.ScanTab.Controls.Add(Me.Label2)
        Me.ScanTab.Controls.Add(Me.Label1)
        Me.ScanTab.Controls.Add(Me.ListBox1)
        Me.ScanTab.Controls.Add(Me.TextBox1)
        Me.ScanTab.Controls.Add(Me.Button1)
        Me.ScanTab.Location = New System.Drawing.Point(0, 0)
        Me.ScanTab.Name = "ScanTab"
        Me.ScanTab.Size = New System.Drawing.Size(240, 242)
        Me.ScanTab.Text = "Scan"
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Tahoma", 10.0!, System.Drawing.FontStyle.Bold)
        Me.Label6.ForeColor = System.Drawing.Color.Black
        Me.Label6.Location = New System.Drawing.Point(7, 4)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(52, 20)
        Me.Label6.Text = "Store:"
        Me.Label6.TextAlign = System.Drawing.ContentAlignment.TopRight
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(19, 29)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(69, 20)
        Me.Label4.Text = "Region:"
        '
        'ComboBox2
        '
        Me.ComboBox2.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold)
        Me.ComboBox2.Location = New System.Drawing.Point(105, 29)
        Me.ComboBox2.Name = "ComboBox2"
        Me.ComboBox2.Size = New System.Drawing.Size(71, 27)
        Me.ComboBox2.TabIndex = 5
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label3.ForeColor = System.Drawing.Color.Black
        Me.Label3.Location = New System.Drawing.Point(19, 83)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(100, 20)
        Me.Label3.Text = "Store:"
        '
        'ComboBox1
        '
        Me.ComboBox1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold)
        Me.ComboBox1.Location = New System.Drawing.Point(16, 106)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(173, 27)
        Me.ComboBox1.TabIndex = 7
        '
        'tabs
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.TabControl1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Menu = Me.mainMenu1
        Me.Name = "tabs"
        Me.Text = "Out of Stock"
        Me.LocationTab.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.ScanTab.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Private WithEvents mainMenu1 As System.Windows.Forms.MainMenu
    Friend WithEvents LocationTab As System.Windows.Forms.TabPage
    Friend WithEvents TabControl1 As System.Windows.Forms.TabControl
    Friend WithEvents ScanTab As System.Windows.Forms.TabPage
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Button2 As System.Windows.Forms.Button
    Friend WithEvents Button3 As System.Windows.Forms.Button
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents ListBox1 As System.Windows.Forms.ListBox
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents ContextMenu1 As System.Windows.Forms.ContextMenu
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents ComboBox2 As System.Windows.Forms.ComboBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox

End Class
