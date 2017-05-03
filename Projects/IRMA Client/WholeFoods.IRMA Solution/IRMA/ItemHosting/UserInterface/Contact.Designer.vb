<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmContact
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
        Me.IsInitializing = True
        InitializeComponent()
        Me.IsInitializing = False
	End Sub
	'Form overrides dispose to clean up the component list.
	<System.Diagnostics.DebuggerNonUserCode()> Protected Overloads Overrides Sub Dispose(ByVal Disposing As Boolean)
		If Disposing Then
			If Not components Is Nothing Then
				components.Dispose()
			End If
		End If
		MyBase.Dispose(Disposing)
	End Sub
	'Required by the Windows Form Designer
	Private components As System.ComponentModel.IContainer
	Public ToolTip1 As System.Windows.Forms.ToolTip
    Public WithEvents txtExt As System.Windows.Forms.TextBox
    Public WithEvents cmdCompanySearch As System.Windows.Forms.Button
    Public WithEvents txtFax As System.Windows.Forms.TextBox
    Public WithEvents txtPhone As System.Windows.Forms.TextBox
    Public WithEvents txtContactName As System.Windows.Forms.TextBox
    Public WithEvents cmdExit As System.Windows.Forms.Button
    Public WithEvents cmdAdd As System.Windows.Forms.Button
    Public WithEvents cmdDelete As System.Windows.Forms.Button
    Public WithEvents lblExt As System.Windows.Forms.Label
    Public WithEvents lblFax As System.Windows.Forms.Label
    Public WithEvents lblPhone As System.Windows.Forms.Label
    Public WithEvents lblContact As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmContact))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdCompanySearch = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdDelete = New System.Windows.Forms.Button
        Me.txtExt = New System.Windows.Forms.TextBox
        Me.txtFax = New System.Windows.Forms.TextBox
        Me.txtPhone = New System.Windows.Forms.TextBox
        Me.txtContactName = New System.Windows.Forms.TextBox
        Me.lblExt = New System.Windows.Forms.Label
        Me.lblFax = New System.Windows.Forms.Label
        Me.lblPhone = New System.Windows.Forms.Label
        Me.lblContact = New System.Windows.Forms.Label
        Me.txtEmail = New System.Windows.Forms.TextBox
        Me.lblEmail = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'cmdCompanySearch
        '
        Me.cmdCompanySearch.BackColor = System.Drawing.SystemColors.Control
        Me.cmdCompanySearch.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdCompanySearch, "cmdCompanySearch")
        Me.cmdCompanySearch.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdCompanySearch.Name = "cmdCompanySearch"
        Me.cmdCompanySearch.TabStop = False
        Me.ToolTip1.SetToolTip(Me.cmdCompanySearch, resources.GetString("cmdCompanySearch.ToolTip"))
        Me.cmdCompanySearch.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'cmdAdd
        '
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdAdd, "cmdAdd")
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Name = "cmdAdd"
        Me.ToolTip1.SetToolTip(Me.cmdAdd, resources.GetString("cmdAdd.ToolTip"))
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdDelete
        '
        Me.cmdDelete.BackColor = System.Drawing.SystemColors.Control
        Me.cmdDelete.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.cmdDelete, "cmdDelete")
        Me.cmdDelete.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdDelete.Name = "cmdDelete"
        Me.ToolTip1.SetToolTip(Me.cmdDelete, resources.GetString("cmdDelete.ToolTip"))
        Me.cmdDelete.UseVisualStyleBackColor = False
        '
        'txtExt
        '
        Me.txtExt.AcceptsReturn = True
        Me.txtExt.BackColor = System.Drawing.SystemColors.Window
        Me.txtExt.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtExt, "txtExt")
        Me.txtExt.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtExt.Name = "txtExt"
        Me.txtExt.Tag = "String"
        '
        'txtFax
        '
        Me.txtFax.AcceptsReturn = True
        Me.txtFax.BackColor = System.Drawing.SystemColors.Window
        Me.txtFax.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtFax, "txtFax")
        Me.txtFax.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtFax.Name = "txtFax"
        Me.txtFax.Tag = "String"
        '
        'txtPhone
        '
        Me.txtPhone.AcceptsReturn = True
        Me.txtPhone.BackColor = System.Drawing.SystemColors.Window
        Me.txtPhone.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtPhone, "txtPhone")
        Me.txtPhone.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPhone.Name = "txtPhone"
        Me.txtPhone.Tag = "String"
        '
        'txtContactName
        '
        Me.txtContactName.AcceptsReturn = True
        Me.txtContactName.BackColor = System.Drawing.SystemColors.Window
        Me.txtContactName.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtContactName, "txtContactName")
        Me.txtContactName.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtContactName.Name = "txtContactName"
        Me.txtContactName.Tag = "String"
        '
        'lblExt
        '
        Me.lblExt.BackColor = System.Drawing.Color.Transparent
        Me.lblExt.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblExt, "lblExt")
        Me.lblExt.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblExt.Name = "lblExt"
        '
        'lblFax
        '
        Me.lblFax.BackColor = System.Drawing.Color.Transparent
        Me.lblFax.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblFax, "lblFax")
        Me.lblFax.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblFax.Name = "lblFax"
        '
        'lblPhone
        '
        Me.lblPhone.BackColor = System.Drawing.Color.Transparent
        Me.lblPhone.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblPhone, "lblPhone")
        Me.lblPhone.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPhone.Name = "lblPhone"
        '
        'lblContact
        '
        Me.lblContact.BackColor = System.Drawing.Color.Transparent
        Me.lblContact.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblContact, "lblContact")
        Me.lblContact.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblContact.Name = "lblContact"
        '
        'txtEmail
        '
        Me.txtEmail.AcceptsReturn = True
        Me.txtEmail.BackColor = System.Drawing.SystemColors.Window
        Me.txtEmail.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtEmail, "txtEmail")
        Me.txtEmail.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtEmail.Name = "txtEmail"
        Me.txtEmail.Tag = "String"
        '
        'lblEmail
        '
        Me.lblEmail.BackColor = System.Drawing.Color.Transparent
        Me.lblEmail.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblEmail, "lblEmail")
        Me.lblEmail.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblEmail.Name = "lblEmail"
        '
        'frmContact
        '
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.Controls.Add(Me.lblEmail)
        Me.Controls.Add(Me.txtEmail)
        Me.Controls.Add(Me.txtExt)
        Me.Controls.Add(Me.cmdCompanySearch)
        Me.Controls.Add(Me.txtFax)
        Me.Controls.Add(Me.txtPhone)
        Me.Controls.Add(Me.txtContactName)
        Me.Controls.Add(Me.cmdExit)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cmdDelete)
        Me.Controls.Add(Me.lblExt)
        Me.Controls.Add(Me.lblFax)
        Me.Controls.Add(Me.lblPhone)
        Me.Controls.Add(Me.lblContact)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmContact"
        Me.ShowInTaskbar = False
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Public WithEvents txtEmail As System.Windows.Forms.TextBox
    Public WithEvents lblEmail As System.Windows.Forms.Label
#End Region 
End Class