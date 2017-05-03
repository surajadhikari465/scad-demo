<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Public Class ReceiveOrder
    Inherits HandheldHardware.ScanForm

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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(ReceiveOrder))
        Me.mainMenu1 = New System.Windows.Forms.MainMenu
        Me.mnuMenu = New System.Windows.Forms.MenuItem
        Me.MenuItemRefusedList = New System.Windows.Forms.MenuItem
        Me.MenuItemInvoiceData = New System.Windows.Forms.MenuItem
        Me.MenuItemReceivingList = New System.Windows.Forms.MenuItem
        Me.MenuItemOrderInfo = New System.Windows.Forms.MenuItem
        Me.mnuMenu_Clear = New System.Windows.Forms.MenuItem
        Me.mnuMenu_ExitReceiveOrder = New System.Windows.Forms.MenuItem
        Me.mnuReceive = New System.Windows.Forms.MenuItem
        Me.frmStatus = New System.Windows.Forms.StatusBar
        Me.txtQtyReceived = New System.Windows.Forms.TextBox
        Me.lblQtyReceived = New System.Windows.Forms.Label
        Me.lblSubTeamVal = New System.Windows.Forms.Label
        Me.lblStoreVal = New System.Windows.Forms.Label
        Me.lblPkgVal = New System.Windows.Forms.Label
        Me.lblPkg = New System.Windows.Forms.Label
        Me.lblPrimaryVendorVal = New System.Windows.Forms.Label
        Me.lblPrimaryVendor = New System.Windows.Forms.Label
        Me.lblItemSubTeamVal = New System.Windows.Forms.Label
        Me.lblItemSubTeam = New System.Windows.Forms.Label
        Me.lblOrderedVal = New System.Windows.Forms.Label
        Me.lblOrdered = New System.Windows.Forms.Label
        Me.cmdSearchItem = New System.Windows.Forms.Button
        Me.txtUpc = New System.Windows.Forms.TextBox
        Me.lblUPC = New System.Windows.Forms.Label
        Me.txtWeightReceived = New System.Windows.Forms.TextBox
        Me.lblWeightReceived = New System.Windows.Forms.Label
        Me.lblReceivedVal = New System.Windows.Forms.Label
        Me.lblReceived = New System.Windows.Forms.Label
        Me.lblOrderedUOM = New System.Windows.Forms.Label
        Me.cmdSearchForExtPO = New System.Windows.Forms.Button
        Me.txtExternalPONumber = New System.Windows.Forms.TextBox
        Me.lblEinvoiceQty = New System.Windows.Forms.Label
        Me.lblEinvoiceQtyVal = New System.Windows.Forms.Label
        Me.LabelEinvoiceUOM = New System.Windows.Forms.Label
        Me.ComboBoxReasonCode = New System.Windows.Forms.ComboBox
        Me.lblEinvoiceQtyUOM = New System.Windows.Forms.Label
        Me.LinkLabelReasonCode = New System.Windows.Forms.LinkLabel
        Me.lblDescriptionVal = New System.Windows.Forms.Label
        Me.lblExternalID = New System.Windows.Forms.Label
        Me.cmbUOM = New System.Windows.Forms.ComboBox
        Me.lblUOM = New System.Windows.Forms.Label
        Me.SuspendLayout()
        '
        'mainMenu1
        '
        Me.mainMenu1.MenuItems.Add(Me.mnuMenu)
        Me.mainMenu1.MenuItems.Add(Me.mnuReceive)
        '
        'mnuMenu
        '
        Me.mnuMenu.MenuItems.Add(Me.MenuItemRefusedList)
        Me.mnuMenu.MenuItems.Add(Me.MenuItemInvoiceData)
        Me.mnuMenu.MenuItems.Add(Me.MenuItemReceivingList)
        Me.mnuMenu.MenuItems.Add(Me.MenuItemOrderInfo)
        Me.mnuMenu.MenuItems.Add(Me.mnuMenu_Clear)
        Me.mnuMenu.MenuItems.Add(Me.mnuMenu_ExitReceiveOrder)
        resources.ApplyResources(Me.mnuMenu, "mnuMenu")
        '
        'MenuItemRefusedList
        '
        resources.ApplyResources(Me.MenuItemRefusedList, "MenuItemRefusedList")
        '
        'MenuItemInvoiceData
        '
        resources.ApplyResources(Me.MenuItemInvoiceData, "MenuItemInvoiceData")
        '
        'MenuItemReceivingList
        '
        resources.ApplyResources(Me.MenuItemReceivingList, "MenuItemReceivingList")
        '
        'MenuItemOrderInfo
        '
        resources.ApplyResources(Me.MenuItemOrderInfo, "MenuItemOrderInfo")
        '
        'mnuMenu_Clear
        '
        resources.ApplyResources(Me.mnuMenu_Clear, "mnuMenu_Clear")
        '
        'mnuMenu_ExitReceiveOrder
        '
        resources.ApplyResources(Me.mnuMenu_ExitReceiveOrder, "mnuMenu_ExitReceiveOrder")
        '
        'mnuReceive
        '
        resources.ApplyResources(Me.mnuReceive, "mnuReceive")
        '
        'frmStatus
        '
        resources.ApplyResources(Me.frmStatus, "frmStatus")
        Me.frmStatus.Name = "frmStatus"
        '
        'txtQtyReceived
        '
        Me.txtQtyReceived.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        resources.ApplyResources(Me.txtQtyReceived, "txtQtyReceived")
        Me.txtQtyReceived.Name = "txtQtyReceived"
        '
        'lblQtyReceived
        '
        resources.ApplyResources(Me.lblQtyReceived, "lblQtyReceived")
        Me.lblQtyReceived.Name = "lblQtyReceived"
        '
        'lblSubTeamVal
        '
        Me.lblSubTeamVal.BackColor = System.Drawing.Color.Silver
        resources.ApplyResources(Me.lblSubTeamVal, "lblSubTeamVal")
        Me.lblSubTeamVal.Name = "lblSubTeamVal"
        '
        'lblStoreVal
        '
        Me.lblStoreVal.BackColor = System.Drawing.Color.Silver
        resources.ApplyResources(Me.lblStoreVal, "lblStoreVal")
        Me.lblStoreVal.Name = "lblStoreVal"
        '
        'lblPkgVal
        '
        resources.ApplyResources(Me.lblPkgVal, "lblPkgVal")
        Me.lblPkgVal.Name = "lblPkgVal"
        '
        'lblPkg
        '
        resources.ApplyResources(Me.lblPkg, "lblPkg")
        Me.lblPkg.Name = "lblPkg"
        '
        'lblPrimaryVendorVal
        '
        resources.ApplyResources(Me.lblPrimaryVendorVal, "lblPrimaryVendorVal")
        Me.lblPrimaryVendorVal.Name = "lblPrimaryVendorVal"
        '
        'lblPrimaryVendor
        '
        resources.ApplyResources(Me.lblPrimaryVendor, "lblPrimaryVendor")
        Me.lblPrimaryVendor.Name = "lblPrimaryVendor"
        '
        'lblItemSubTeamVal
        '
        resources.ApplyResources(Me.lblItemSubTeamVal, "lblItemSubTeamVal")
        Me.lblItemSubTeamVal.Name = "lblItemSubTeamVal"
        '
        'lblItemSubTeam
        '
        resources.ApplyResources(Me.lblItemSubTeam, "lblItemSubTeam")
        Me.lblItemSubTeam.Name = "lblItemSubTeam"
        '
        'lblOrderedVal
        '
        Me.lblOrderedVal.BackColor = System.Drawing.Color.Gainsboro
        resources.ApplyResources(Me.lblOrderedVal, "lblOrderedVal")
        Me.lblOrderedVal.Name = "lblOrderedVal"
        '
        'lblOrdered
        '
        Me.lblOrdered.BackColor = System.Drawing.Color.Gainsboro
        resources.ApplyResources(Me.lblOrdered, "lblOrdered")
        Me.lblOrdered.Name = "lblOrdered"
        '
        'cmdSearchItem
        '
        resources.ApplyResources(Me.cmdSearchItem, "cmdSearchItem")
        Me.cmdSearchItem.Name = "cmdSearchItem"
        Me.cmdSearchItem.TabStop = False
        '
        'txtUpc
        '
        Me.txtUpc.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.txtUpc, "txtUpc")
        Me.txtUpc.Name = "txtUpc"
        '
        'lblUPC
        '
        resources.ApplyResources(Me.lblUPC, "lblUPC")
        Me.lblUPC.Name = "lblUPC"
        '
        'txtWeightReceived
        '
        Me.txtWeightReceived.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        resources.ApplyResources(Me.txtWeightReceived, "txtWeightReceived")
        Me.txtWeightReceived.Name = "txtWeightReceived"
        '
        'lblWeightReceived
        '
        resources.ApplyResources(Me.lblWeightReceived, "lblWeightReceived")
        Me.lblWeightReceived.Name = "lblWeightReceived"
        '
        'lblReceivedVal
        '
        Me.lblReceivedVal.BackColor = System.Drawing.Color.Gainsboro
        resources.ApplyResources(Me.lblReceivedVal, "lblReceivedVal")
        Me.lblReceivedVal.Name = "lblReceivedVal"
        '
        'lblReceived
        '
        Me.lblReceived.BackColor = System.Drawing.Color.Gainsboro
        resources.ApplyResources(Me.lblReceived, "lblReceived")
        Me.lblReceived.Name = "lblReceived"
        '
        'lblOrderedUOM
        '
        Me.lblOrderedUOM.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.lblOrderedUOM, "lblOrderedUOM")
        Me.lblOrderedUOM.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblOrderedUOM.Name = "lblOrderedUOM"
        '
        'cmdSearchForExtPO
        '
        resources.ApplyResources(Me.cmdSearchForExtPO, "cmdSearchForExtPO")
        Me.cmdSearchForExtPO.Name = "cmdSearchForExtPO"
        Me.cmdSearchForExtPO.TabStop = False
        '
        'txtExternalPONumber
        '
        resources.ApplyResources(Me.txtExternalPONumber, "txtExternalPONumber")
        Me.txtExternalPONumber.Name = "txtExternalPONumber"
        '
        'lblEinvoiceQty
        '
        Me.lblEinvoiceQty.BackColor = System.Drawing.Color.Gainsboro
        resources.ApplyResources(Me.lblEinvoiceQty, "lblEinvoiceQty")
        Me.lblEinvoiceQty.Name = "lblEinvoiceQty"
        '
        'lblEinvoiceQtyVal
        '
        Me.lblEinvoiceQtyVal.BackColor = System.Drawing.Color.Gainsboro
        resources.ApplyResources(Me.lblEinvoiceQtyVal, "lblEinvoiceQtyVal")
        Me.lblEinvoiceQtyVal.Name = "lblEinvoiceQtyVal"
        '
        'LabelEinvoiceUOM
        '
        Me.LabelEinvoiceUOM.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.LabelEinvoiceUOM, "LabelEinvoiceUOM")
        Me.LabelEinvoiceUOM.ForeColor = System.Drawing.SystemColors.WindowText
        Me.LabelEinvoiceUOM.Name = "LabelEinvoiceUOM"
        '
        'ComboBoxReasonCode
        '
        resources.ApplyResources(Me.ComboBoxReasonCode, "ComboBoxReasonCode")
        Me.ComboBoxReasonCode.Name = "ComboBoxReasonCode"
        '
        'lblEinvoiceQtyUOM
        '
        Me.lblEinvoiceQtyUOM.BackColor = System.Drawing.Color.White
        resources.ApplyResources(Me.lblEinvoiceQtyUOM, "lblEinvoiceQtyUOM")
        Me.lblEinvoiceQtyUOM.ForeColor = System.Drawing.SystemColors.WindowText
        Me.lblEinvoiceQtyUOM.Name = "lblEinvoiceQtyUOM"
        '
        'LinkLabelReasonCode
        '
        resources.ApplyResources(Me.LinkLabelReasonCode, "LinkLabelReasonCode")
        Me.LinkLabelReasonCode.Name = "LinkLabelReasonCode"
        '
        'lblDescriptionVal
        '
        Me.lblDescriptionVal.BackColor = System.Drawing.Color.FromArgb(CType(CType(192, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(255, Byte), Integer))
        resources.ApplyResources(Me.lblDescriptionVal, "lblDescriptionVal")
        Me.lblDescriptionVal.Name = "lblDescriptionVal"
        '
        'lblExternalID
        '
        resources.ApplyResources(Me.lblExternalID, "lblExternalID")
        Me.lblExternalID.Name = "lblExternalID"
        '
        'cmbUOM
        '
        resources.ApplyResources(Me.cmbUOM, "cmbUOM")
        Me.cmbUOM.Items.Add(resources.GetString("cmbUOM.Items"))
        Me.cmbUOM.Items.Add(resources.GetString("cmbUOM.Items1"))
        Me.cmbUOM.Items.Add(resources.GetString("cmbUOM.Items2"))
        Me.cmbUOM.Items.Add(resources.GetString("cmbUOM.Items3"))
        Me.cmbUOM.Name = "cmbUOM"
        '
        'lblUOM
        '
        resources.ApplyResources(Me.lblUOM, "lblUOM")
        Me.lblUOM.Name = "lblUOM"
        '
        'ReceiveOrder
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        resources.ApplyResources(Me, "$this")
        Me.ControlBox = False
        Me.Controls.Add(Me.lblUOM)
        Me.Controls.Add(Me.cmbUOM)
        Me.Controls.Add(Me.lblExternalID)
        Me.Controls.Add(Me.lblDescriptionVal)
        Me.Controls.Add(Me.LinkLabelReasonCode)
        Me.Controls.Add(Me.lblEinvoiceQtyUOM)
        Me.Controls.Add(Me.ComboBoxReasonCode)
        Me.Controls.Add(Me.LabelEinvoiceUOM)
        Me.Controls.Add(Me.lblEinvoiceQtyVal)
        Me.Controls.Add(Me.lblEinvoiceQty)
        Me.Controls.Add(Me.cmdSearchForExtPO)
        Me.Controls.Add(Me.txtExternalPONumber)
        Me.Controls.Add(Me.lblOrderedUOM)
        Me.Controls.Add(Me.lblReceivedVal)
        Me.Controls.Add(Me.lblReceived)
        Me.Controls.Add(Me.txtWeightReceived)
        Me.Controls.Add(Me.lblWeightReceived)
        Me.Controls.Add(Me.cmdSearchItem)
        Me.Controls.Add(Me.txtUpc)
        Me.Controls.Add(Me.lblUPC)
        Me.Controls.Add(Me.lblOrderedVal)
        Me.Controls.Add(Me.lblOrdered)
        Me.Controls.Add(Me.lblItemSubTeamVal)
        Me.Controls.Add(Me.lblItemSubTeam)
        Me.Controls.Add(Me.lblPrimaryVendorVal)
        Me.Controls.Add(Me.lblPrimaryVendor)
        Me.Controls.Add(Me.lblPkgVal)
        Me.Controls.Add(Me.lblPkg)
        Me.Controls.Add(Me.lblSubTeamVal)
        Me.Controls.Add(Me.lblStoreVal)
        Me.Controls.Add(Me.frmStatus)
        Me.Controls.Add(Me.txtQtyReceived)
        Me.Controls.Add(Me.lblQtyReceived)
        Me.Menu = Me.mainMenu1
        Me.Name = "ReceiveOrder"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents mnuMenu As System.Windows.Forms.MenuItem
    Friend WithEvents mnuReceive As System.Windows.Forms.MenuItem
    Friend WithEvents mnuMenu_Clear As System.Windows.Forms.MenuItem
    Friend WithEvents frmStatus As System.Windows.Forms.StatusBar
    Friend WithEvents txtQtyReceived As System.Windows.Forms.TextBox
    Friend WithEvents mnuMenu_ExitReceiveOrder As System.Windows.Forms.MenuItem
    Friend WithEvents lblSubTeamVal As System.Windows.Forms.Label
    Friend WithEvents lblStoreVal As System.Windows.Forms.Label
    Friend WithEvents lblPkgVal As System.Windows.Forms.Label
    Friend WithEvents lblPkg As System.Windows.Forms.Label
    Friend WithEvents lblQtyReceived As System.Windows.Forms.Label
    Friend WithEvents lblPrimaryVendorVal As System.Windows.Forms.Label
    Friend WithEvents lblPrimaryVendor As System.Windows.Forms.Label
    Friend WithEvents lblItemSubTeamVal As System.Windows.Forms.Label
    Friend WithEvents lblItemSubTeam As System.Windows.Forms.Label
    Friend WithEvents lblOrderedVal As System.Windows.Forms.Label
    Friend WithEvents lblOrdered As System.Windows.Forms.Label
    Friend WithEvents cmdSearchItem As System.Windows.Forms.Button
    Friend WithEvents txtUpc As System.Windows.Forms.TextBox
    Friend WithEvents lblUPC As System.Windows.Forms.Label
    Friend WithEvents txtWeightReceived As System.Windows.Forms.TextBox
    Friend WithEvents lblWeightReceived As System.Windows.Forms.Label
    Friend WithEvents lblReceivedVal As System.Windows.Forms.Label
    Friend WithEvents lblReceived As System.Windows.Forms.Label
    Friend WithEvents lblOrderedUOM As System.Windows.Forms.Label
    Friend WithEvents cmdSearchForExtPO As System.Windows.Forms.Button
    Friend WithEvents txtExternalPONumber As System.Windows.Forms.TextBox
    Friend WithEvents MenuItemInvoiceData As System.Windows.Forms.MenuItem
    Friend WithEvents lblEinvoiceQty As System.Windows.Forms.Label
    Friend WithEvents lblEinvoiceQtyVal As System.Windows.Forms.Label
    Friend WithEvents LabelEinvoiceUOM As System.Windows.Forms.Label
    Friend WithEvents ComboBoxReasonCode As System.Windows.Forms.ComboBox
    Friend WithEvents lblEinvoiceQtyUOM As System.Windows.Forms.Label
    Friend WithEvents LinkLabelReasonCode As System.Windows.Forms.LinkLabel
    Friend WithEvents MenuItemReceivingList As System.Windows.Forms.MenuItem
    Friend WithEvents lblDescriptionVal As System.Windows.Forms.Label
    Friend WithEvents lblExternalID As System.Windows.Forms.Label
    Friend WithEvents MenuItemRefusedList As System.Windows.Forms.MenuItem
    Friend WithEvents cmbUOM As System.Windows.Forms.ComboBox
    Friend WithEvents lblUOM As System.Windows.Forms.Label
    Friend WithEvents MenuItemOrderInfo As System.Windows.Forms.MenuItem
End Class
