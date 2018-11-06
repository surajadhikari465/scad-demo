<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> Partial Class frmVendorAdd
#Region "Windows Form Designer generated code "
	<System.Diagnostics.DebuggerNonUserCode()> Public Sub New()
		MyBase.New()
		'This call is required by the Windows Form Designer.
		InitializeComponent()
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
	Public WithEvents txtPSVendor As System.Windows.Forms.TextBox
	Public WithEvents txtPSAddr As System.Windows.Forms.TextBox
	Public WithEvents cmdAdd As System.Windows.Forms.Button
	Public WithEvents cmdExit As System.Windows.Forms.Button
	Public WithEvents txtCompany_Name As System.Windows.Forms.TextBox
    Public WithEvents lblPSVendor As System.Windows.Forms.Label
    Public WithEvents lblPSAddress As System.Windows.Forms.Label
    Public WithEvents lblCompany As System.Windows.Forms.Label
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmVendorAdd))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.cmdAdd = New System.Windows.Forms.Button
        Me.cmdExit = New System.Windows.Forms.Button
        Me.txtPSVendor = New System.Windows.Forms.TextBox
        Me.txtPSAddr = New System.Windows.Forms.TextBox
        Me.txtCompany_Name = New System.Windows.Forms.TextBox
        Me.lblPSVendor = New System.Windows.Forms.Label
        Me.lblPSAddress = New System.Windows.Forms.Label
        Me.lblCompany = New System.Windows.Forms.Label
        Me.txtPSVendorExport = New System.Windows.Forms.TextBox
        Me.lblPSVendorExport = New System.Windows.Forms.Label
        Me._groupVendorType = New System.Windows.Forms.GroupBox
        Me.txtVendorKey = New System.Windows.Forms.TextBox
        Me.lblKey = New System.Windows.Forms.Label
        Me._checkExternal = New System.Windows.Forms.CheckBox
        Me._groupAccountingSetup = New System.Windows.Forms.GroupBox
        Me._groupVendorType.SuspendLayout()
        Me._groupAccountingSetup.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdAdd
        '
        resources.ApplyResources(Me.cmdAdd, "cmdAdd")
        Me.cmdAdd.BackColor = System.Drawing.SystemColors.Control
        Me.cmdAdd.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdAdd.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdAdd.Name = "cmdAdd"
        Me.ToolTip1.SetToolTip(Me.cmdAdd, resources.GetString("cmdAdd.ToolTip"))
        Me.cmdAdd.UseVisualStyleBackColor = False
        '
        'cmdExit
        '
        resources.ApplyResources(Me.cmdExit, "cmdExit")
        Me.cmdExit.BackColor = System.Drawing.SystemColors.Control
        Me.cmdExit.Cursor = System.Windows.Forms.Cursors.Default
        Me.cmdExit.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.cmdExit.ForeColor = System.Drawing.SystemColors.ControlText
        Me.cmdExit.Name = "cmdExit"
        Me.ToolTip1.SetToolTip(Me.cmdExit, resources.GetString("cmdExit.ToolTip"))
        Me.cmdExit.UseVisualStyleBackColor = False
        '
        'txtPSVendor
        '
        Me.txtPSVendor.AcceptsReturn = True
        Me.txtPSVendor.BackColor = System.Drawing.SystemColors.Window
        Me.txtPSVendor.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtPSVendor, "txtPSVendor")
        Me.txtPSVendor.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPSVendor.Name = "txtPSVendor"
        Me.txtPSVendor.Tag = "String"
        '
        'txtPSAddr
        '
        Me.txtPSAddr.AcceptsReturn = True
        Me.txtPSAddr.BackColor = System.Drawing.SystemColors.Window
        Me.txtPSAddr.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtPSAddr, "txtPSAddr")
        Me.txtPSAddr.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPSAddr.Name = "txtPSAddr"
        Me.txtPSAddr.Tag = "Number"
        '
        'txtCompany_Name
        '
        Me.txtCompany_Name.AcceptsReturn = True
        Me.txtCompany_Name.BackColor = System.Drawing.SystemColors.Window
        Me.txtCompany_Name.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtCompany_Name, "txtCompany_Name")
        Me.txtCompany_Name.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtCompany_Name.Name = "txtCompany_Name"
        '
        'lblPSVendor
        '
        Me.lblPSVendor.BackColor = System.Drawing.Color.Transparent
        Me.lblPSVendor.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblPSVendor, "lblPSVendor")
        Me.lblPSVendor.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPSVendor.Name = "lblPSVendor"
        '
        'lblPSAddress
        '
        Me.lblPSAddress.BackColor = System.Drawing.Color.Transparent
        Me.lblPSAddress.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblPSAddress, "lblPSAddress")
        Me.lblPSAddress.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPSAddress.Name = "lblPSAddress"
        '
        'lblCompany
        '
        resources.ApplyResources(Me.lblCompany, "lblCompany")
        Me.lblCompany.BackColor = System.Drawing.Color.Transparent
        Me.lblCompany.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblCompany.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblCompany.Name = "lblCompany"
        '
        'txtPSVendorExport
        '
        Me.txtPSVendorExport.AcceptsReturn = True
        Me.txtPSVendorExport.BackColor = System.Drawing.SystemColors.Window
        Me.txtPSVendorExport.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtPSVendorExport, "txtPSVendorExport")
        Me.txtPSVendorExport.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPSVendorExport.Name = "txtPSVendorExport"
        Me.txtPSVendorExport.Tag = "String"
        '
        'lblPSVendorExport
        '
        Me.lblPSVendorExport.BackColor = System.Drawing.Color.Transparent
        Me.lblPSVendorExport.Cursor = System.Windows.Forms.Cursors.Default
        resources.ApplyResources(Me.lblPSVendorExport, "lblPSVendorExport")
        Me.lblPSVendorExport.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblPSVendorExport.Name = "lblPSVendorExport"
        '
        '_groupVendorType
        '
        Me._groupVendorType.Controls.Add(Me.txtVendorKey)
        Me._groupVendorType.Controls.Add(Me.lblKey)
        Me._groupVendorType.Controls.Add(Me._checkExternal)
        Me._groupVendorType.Controls.Add(Me.txtCompany_Name)
        Me._groupVendorType.Controls.Add(Me.lblCompany)
        resources.ApplyResources(Me._groupVendorType, "_groupVendorType")
        Me._groupVendorType.Name = "_groupVendorType"
        Me._groupVendorType.TabStop = False
        '
        'txtVendorKey
        '
        Me.txtVendorKey.AcceptsReturn = True
        Me.txtVendorKey.BackColor = System.Drawing.SystemColors.Window
        Me.txtVendorKey.Cursor = System.Windows.Forms.Cursors.IBeam
        resources.ApplyResources(Me.txtVendorKey, "txtVendorKey")
        Me.txtVendorKey.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtVendorKey.Name = "txtVendorKey"
        '
        'lblKey
        '
        resources.ApplyResources(Me.lblKey, "lblKey")
        Me.lblKey.BackColor = System.Drawing.Color.Transparent
        Me.lblKey.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblKey.ForeColor = System.Drawing.SystemColors.ControlText
        Me.lblKey.Name = "lblKey"
        '
        '_checkExternal
        '
        resources.ApplyResources(Me._checkExternal, "_checkExternal")
        Me._checkExternal.Checked = True
        Me._checkExternal.CheckState = System.Windows.Forms.CheckState.Checked
        Me._checkExternal.Name = "_checkExternal"
        Me._checkExternal.UseVisualStyleBackColor = True
        '
        '_groupAccountingSetup
        '
        Me._groupAccountingSetup.Controls.Add(Me.txtPSVendor)
        Me._groupAccountingSetup.Controls.Add(Me.lblPSAddress)
        Me._groupAccountingSetup.Controls.Add(Me.txtPSVendorExport)
        Me._groupAccountingSetup.Controls.Add(Me.lblPSVendor)
        Me._groupAccountingSetup.Controls.Add(Me.lblPSVendorExport)
        Me._groupAccountingSetup.Controls.Add(Me.txtPSAddr)
        resources.ApplyResources(Me._groupAccountingSetup, "_groupAccountingSetup")
        Me._groupAccountingSetup.Name = "_groupAccountingSetup"
        Me._groupAccountingSetup.TabStop = False
        '
        'frmVendorAdd
        '
        Me.AcceptButton = Me.cmdAdd
        resources.ApplyResources(Me, "$this")
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.SystemColors.Control
        Me.CancelButton = Me.cmdExit
        Me.Controls.Add(Me._groupAccountingSetup)
        Me.Controls.Add(Me._groupVendorType)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.cmdExit)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "frmVendorAdd"
        Me.ShowInTaskbar = False
        Me._groupVendorType.ResumeLayout(False)
        Me._groupVendorType.PerformLayout()
        Me._groupAccountingSetup.ResumeLayout(False)
        Me._groupAccountingSetup.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Public WithEvents txtPSVendorExport As System.Windows.Forms.TextBox
    Public WithEvents lblPSVendorExport As System.Windows.Forms.Label
    Friend WithEvents _groupVendorType As System.Windows.Forms.GroupBox
    Friend WithEvents _groupAccountingSetup As System.Windows.Forms.GroupBox
    Friend WithEvents _checkExternal As System.Windows.Forms.CheckBox
    Public WithEvents txtVendorKey As System.Windows.Forms.TextBox
    Public WithEvents lblKey As System.Windows.Forms.Label
#End Region 
End Class