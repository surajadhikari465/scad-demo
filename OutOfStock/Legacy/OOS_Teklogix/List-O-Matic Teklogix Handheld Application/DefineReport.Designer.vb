<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class DefineReport
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
    Private mainMenu1 As System.Windows.Forms.MainMenu

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.MenuItem_Menu = New System.Windows.Forms.MenuItem
        Me.Label3 = New System.Windows.Forms.Label
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.ComboBox1 = New System.Windows.Forms.ComboBox
        Me.Button_Continue = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Button_GetStores = New System.Windows.Forms.Button
        Me.ComboBox_Region = New System.Windows.Forms.ComboBox
        Me.Label4 = New System.Windows.Forms.Label
        Me.MainMenu2 = New System.Windows.Forms.MainMenu
        Me.MenuItem1 = New System.Windows.Forms.MenuItem
        Me.MenuItem2 = New System.Windows.Forms.MenuItem
        Me.MenuItem3 = New System.Windows.Forms.MenuItem
        Me.Label_Version = New System.Windows.Forms.Label
        Me.MenuItem_About = New System.Windows.Forms.MenuItem
        Me.MenuItem_Exit = New System.Windows.Forms.MenuItem
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.MenuItem_Menu)
        '
        'MenuItem_Menu
        '
        Me.MenuItem_Menu.MenuItems.Add(Me.MenuItem_Exit)
        Me.MenuItem_Menu.MenuItems.Add(Me.MenuItem_About)
        Me.MenuItem_Menu.Text = "Exit"
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Bold)
        Me.Label3.Location = New System.Drawing.Point(66, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(112, 20)
        Me.Label3.Text = "Out Of Stock"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.LightYellow
        Me.Panel1.Controls.Add(Me.ComboBox1)
        Me.Panel1.Controls.Add(Me.Button_Continue)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.Button_GetStores)
        Me.Panel1.Controls.Add(Me.ComboBox_Region)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Location = New System.Drawing.Point(10, 34)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(219, 216)
        '
        'ComboBox1
        '
        Me.ComboBox1.Font = New System.Drawing.Font("Tahoma", 12.0!, System.Drawing.FontStyle.Bold)
        Me.ComboBox1.Location = New System.Drawing.Point(15, 106)
        Me.ComboBox1.Name = "ComboBox1"
        Me.ComboBox1.Size = New System.Drawing.Size(171, 27)
        Me.ComboBox1.TabIndex = 7
        '
        'Button_Continue
        '
        Me.Button_Continue.BackColor = System.Drawing.Color.DarkBlue
        Me.Button_Continue.Font = New System.Drawing.Font("Tahoma", 11.0!, System.Drawing.FontStyle.Bold)
        Me.Button_Continue.ForeColor = System.Drawing.Color.White
        Me.Button_Continue.Location = New System.Drawing.Point(15, 148)
        Me.Button_Continue.Name = "Button_Continue"
        Me.Button_Continue.Size = New System.Drawing.Size(171, 47)
        Me.Button_Continue.TabIndex = 6
        Me.Button_Continue.Text = "Continue"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.ForeColor = System.Drawing.Color.Black
        Me.Label1.Location = New System.Drawing.Point(15, 83)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 20)
        Me.Label1.Text = "Store :"
        '
        'Button_GetStores
        '
        Me.Button_GetStores.BackColor = System.Drawing.Color.DarkBlue
        Me.Button_GetStores.ForeColor = System.Drawing.Color.White
        Me.Button_GetStores.Location = New System.Drawing.Point(121, 32)
        Me.Button_GetStores.Name = "Button_GetStores"
        Me.Button_GetStores.Size = New System.Drawing.Size(82, 31)
        Me.Button_GetStores.TabIndex = 3
        Me.Button_GetStores.Text = "Get Stores"
        '
        'ComboBox_Region
        '
        Me.ComboBox_Region.Font = New System.Drawing.Font("Tahoma", 8.0!, System.Drawing.FontStyle.Bold)
        Me.ComboBox_Region.Location = New System.Drawing.Point(3, 43)
        Me.ComboBox_Region.Name = "ComboBox_Region"
        Me.ComboBox_Region.Size = New System.Drawing.Size(112, 20)
        Me.ComboBox_Region.TabIndex = 1
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label4.ForeColor = System.Drawing.Color.Black
        Me.Label4.Location = New System.Drawing.Point(15, 20)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(100, 20)
        Me.Label4.Text = "Region :"
        '
        'MainMenu2
        '
        Me.MainMenu2.MenuItems.Add(Me.MenuItem1)
        '
        'MenuItem1
        '
        Me.MenuItem1.MenuItems.Add(Me.MenuItem2)
        Me.MenuItem1.MenuItems.Add(Me.MenuItem3)
        Me.MenuItem1.Text = "Menu"
        '
        'MenuItem2
        '
        Me.MenuItem2.Text = "Exit"
        '
        'MenuItem3
        '
        Me.MenuItem3.Text = "About"
        '
        'Label_Version
        '
        Me.Label_Version.Font = New System.Drawing.Font("Tahoma", 7.0!, System.Drawing.FontStyle.Regular)
        Me.Label_Version.ForeColor = System.Drawing.SystemColors.GrayText
        Me.Label_Version.Location = New System.Drawing.Point(10, 253)
        Me.Label_Version.Name = "Label_Version"
        Me.Label_Version.Size = New System.Drawing.Size(224, 14)
        Me.Label_Version.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'MenuItem_About
        '
        Me.MenuItem_About.Text = "About"
        '
        'MenuItem_Exit
        '
        Me.MenuItem_Exit.Text = "Exit"
        '
        'DefineReport
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.Label_Version)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Label3)
        Me.Menu = Me.mainMenu1
        Me.Name = "DefineReport"
        Me.Text = "Select Store"
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents ComboBox_Region As System.Windows.Forms.ComboBox
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Button_GetStores As System.Windows.Forms.Button
    Friend WithEvents ComboBox1 As System.Windows.Forms.ComboBox
    Friend WithEvents Button_Continue As System.Windows.Forms.Button
    Friend WithEvents MainMenu2 As System.Windows.Forms.MainMenu
    Friend WithEvents MenuItem1 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem2 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem3 As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem_Menu As System.Windows.Forms.MenuItem
    Friend WithEvents Label_Version As System.Windows.Forms.Label
    Friend WithEvents MenuItem_Exit As System.Windows.Forms.MenuItem
    Friend WithEvents MenuItem_About As System.Windows.Forms.MenuItem
End Class
