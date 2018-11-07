<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class TransferMain
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
        Me.mmuMenu = New System.Windows.Forms.MenuItem
        Me.mmuNewOrder = New System.Windows.Forms.MenuItem
        Me.mmuExitTransfer = New System.Windows.Forms.MenuItem
        Me.mmuExitIRMA = New System.Windows.Forms.MenuItem
        Me.mmuCreate = New System.Windows.Forms.MenuItem
        Me.lblFrom = New System.Windows.Forms.Label
        Me.cmbToStore = New System.Windows.Forms.ComboBox
        Me.cmbToSubTeam = New System.Windows.Forms.ComboBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.lblExpectedDate = New System.Windows.Forms.Label
        Me.dtpExpectedDate = New System.Windows.Forms.DateTimePicker
        Me.cmbFromSubTeam = New System.Windows.Forms.ComboBox
        Me.cmbFromStore = New System.Windows.Forms.ComboBox
        Me.lblProductType = New System.Windows.Forms.Label
        Me.cmbProductType = New System.Windows.Forms.ComboBox
        Me.lblSupplyType = New System.Windows.Forms.Label
        Me.cmbSupplyType = New System.Windows.Forms.ComboBox
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.mmuMenu)
        Me.mainMenu1.MenuItems.Add(Me.mmuCreate)
        '
        'mmuMenu
        '
        Me.mmuMenu.MenuItems.Add(Me.mmuNewOrder)
        Me.mmuMenu.MenuItems.Add(Me.mmuExitTransfer)
        Me.mmuMenu.MenuItems.Add(Me.mmuExitIRMA)
        Me.mmuMenu.Text = "Menu"
        '
        'mmuNewOrder
        '
        Me.mmuNewOrder.Text = "New Order"
        '
        'mmuExitTransfer
        '
        Me.mmuExitTransfer.Text = "Exit Transfer"
        '
        'mmuExitIRMA
        '
        Me.mmuExitIRMA.Text = "Exit IRMA"
        '
        'mmuCreate
        '
        Me.mmuCreate.Text = "Create PO"
        '
        'lblFrom
        '
        Me.lblFrom.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblFrom.Location = New System.Drawing.Point(3, 3)
        Me.lblFrom.Name = "lblFrom"
        Me.lblFrom.Size = New System.Drawing.Size(100, 20)
        Me.lblFrom.Text = "From:"
        '
        'cmbToStore
        '
        Me.cmbToStore.Location = New System.Drawing.Point(3, 108)
        Me.cmbToStore.Name = "cmbToStore"
        Me.cmbToStore.Size = New System.Drawing.Size(234, 22)
        Me.cmbToStore.TabIndex = 3
        '
        'cmbToSubTeam
        '
        Me.cmbToSubTeam.Location = New System.Drawing.Point(3, 136)
        Me.cmbToSubTeam.Name = "cmbToSubTeam"
        Me.cmbToSubTeam.Size = New System.Drawing.Size(234, 22)
        Me.cmbToSubTeam.TabIndex = 4
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.Label1.Location = New System.Drawing.Point(3, 86)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(100, 20)
        Me.Label1.Text = "To:"
        '
        'lblExpectedDate
        '
        Me.lblExpectedDate.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblExpectedDate.Location = New System.Drawing.Point(3, 224)
        Me.lblExpectedDate.Name = "lblExpectedDate"
        Me.lblExpectedDate.Size = New System.Drawing.Size(100, 20)
        Me.lblExpectedDate.Text = "Expected Date:"
        '
        'dtpExpectedDate
        '
        Me.dtpExpectedDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtpExpectedDate.Location = New System.Drawing.Point(109, 222)
        Me.dtpExpectedDate.MinDate = New Date(2012, 9, 1, 0, 0, 0, 0)
        Me.dtpExpectedDate.Name = "dtpExpectedDate"
        Me.dtpExpectedDate.Size = New System.Drawing.Size(128, 22)
        Me.dtpExpectedDate.TabIndex = 7
        Me.dtpExpectedDate.Value = New Date(2013, 1, 1, 0, 0, 0, 0)
        '
        'cmbFromSubTeam
        '
        Me.cmbFromSubTeam.Location = New System.Drawing.Point(4, 53)
        Me.cmbFromSubTeam.Name = "cmbFromSubTeam"
        Me.cmbFromSubTeam.Size = New System.Drawing.Size(234, 22)
        Me.cmbFromSubTeam.TabIndex = 2
        '
        'cmbFromStore
        '
        Me.cmbFromStore.Location = New System.Drawing.Point(4, 25)
        Me.cmbFromStore.Name = "cmbFromStore"
        Me.cmbFromStore.Size = New System.Drawing.Size(234, 22)
        Me.cmbFromStore.TabIndex = 1
        '
        'lblProductType
        '
        Me.lblProductType.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblProductType.Location = New System.Drawing.Point(3, 167)
        Me.lblProductType.Name = "lblProductType"
        Me.lblProductType.Size = New System.Drawing.Size(100, 20)
        Me.lblProductType.Text = "Product Type:"
        '
        'cmbProductType
        '
        Me.cmbProductType.Location = New System.Drawing.Point(109, 165)
        Me.cmbProductType.Name = "cmbProductType"
        Me.cmbProductType.Size = New System.Drawing.Size(128, 22)
        Me.cmbProductType.TabIndex = 10
        '
        'lblSupplyType
        '
        Me.lblSupplyType.Font = New System.Drawing.Font("Tahoma", 9.0!, System.Drawing.FontStyle.Bold)
        Me.lblSupplyType.Location = New System.Drawing.Point(3, 196)
        Me.lblSupplyType.Name = "lblSupplyType"
        Me.lblSupplyType.Size = New System.Drawing.Size(100, 20)
        Me.lblSupplyType.Text = "Supply Type:"
        '
        'cmbSupplyType
        '
        Me.cmbSupplyType.Location = New System.Drawing.Point(109, 194)
        Me.cmbSupplyType.Name = "cmbSupplyType"
        Me.cmbSupplyType.Size = New System.Drawing.Size(128, 22)
        Me.cmbSupplyType.TabIndex = 6
        '
        'TransferMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(240, 268)
        Me.Controls.Add(Me.cmbSupplyType)
        Me.Controls.Add(Me.lblSupplyType)
        Me.Controls.Add(Me.cmbProductType)
        Me.Controls.Add(Me.lblProductType)
        Me.Controls.Add(Me.cmbFromSubTeam)
        Me.Controls.Add(Me.cmbFromStore)
        Me.Controls.Add(Me.dtpExpectedDate)
        Me.Controls.Add(Me.lblExpectedDate)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.cmbToSubTeam)
        Me.Controls.Add(Me.cmbToStore)
        Me.Controls.Add(Me.lblFrom)
        Me.Menu = Me.mainMenu1
        Me.Name = "TransferMain"
        Me.Text = "Transfer Main"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents lblFrom As System.Windows.Forms.Label
    Friend WithEvents cmbToStore As System.Windows.Forms.ComboBox
    Friend WithEvents cmbToSubTeam As System.Windows.Forms.ComboBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents mmuMenu As System.Windows.Forms.MenuItem
    Friend WithEvents mmuCreate As System.Windows.Forms.MenuItem
    Friend WithEvents lblExpectedDate As System.Windows.Forms.Label
    Friend WithEvents dtpExpectedDate As System.Windows.Forms.DateTimePicker
    Friend WithEvents cmbFromSubTeam As System.Windows.Forms.ComboBox
    Friend WithEvents cmbFromStore As System.Windows.Forms.ComboBox
    Friend WithEvents lblProductType As System.Windows.Forms.Label
    Friend WithEvents cmbProductType As System.Windows.Forms.ComboBox
    Friend WithEvents mmuNewOrder As System.Windows.Forms.MenuItem
    Friend WithEvents lblSupplyType As System.Windows.Forms.Label
    Friend WithEvents cmbSupplyType As System.Windows.Forms.ComboBox
    Friend WithEvents mmuExitTransfer As System.Windows.Forms.MenuItem
    Friend WithEvents mmuExitIRMA As System.Windows.Forms.MenuItem
End Class
