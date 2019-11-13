Option Strict Off
Option Explicit On

Imports System.ComponentModel
Imports System.Linq
Imports System.Security.SecurityElement
Imports System.Text.RegularExpressions
Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid
Imports log4net
Imports WholeFoods.IRMA.Administration.Common.DataAccess
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.Ordering.BusinessLogic
Imports WholeFoods.IRMA.Ordering.BusinessLogic.OrderingFunctions
Imports WholeFoods.IRMA.Ordering.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports VB = Microsoft.VisualBasic


Friend Class frmOrders
    Inherits System.Windows.Forms.Form

#Region "Private Members"

    Private resources As ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmOrders))
    Private m_3rdParty As Decimal
    Private IsInitializing As Boolean
    Private m_bLoading As Boolean
    Private m_rsOrder As DAO.Recordset
    Private m_bDataChanged As Boolean
    Private m_lReturnOrder_ID As Integer
    Private m_bOrderApproved As Boolean
    Private m_bOrderClosed As Boolean
    Private m_bOrderUploaded As Boolean
    Private m_bMultipleSubTeams As Boolean
    Private m_dtOriginalCloseDate As Date
    Private m_bSeafoodMissingCountryInfo As Boolean
    Private m_bGridCtrlKey As Boolean
    Private m_bEXEWarehouseDistOrder As Boolean
    Private m_dtOrderEnd As Date
    Private m_dtCurrSysTime As Date
    Private m_rsOrderItems As DAO.Recordset
    Private m_iInsertOrder() As Short
    Private m_lStartOrderItem_ID As Integer
    Private m_bPre_Order As Boolean
    Private m_bFrom_SubTeam_Unrestricted As Boolean
    Private m_bTo_SubTeam_Unrestricted As Boolean
    Private m_lSubTeamLimit As Integer
    Private m_ReceiveLocationIsDistribution As Boolean
    Private m_IsVendorExternal As Boolean
    Private m_rsIsEinvoice As DAO.Recordset
    Private m_rsOrderSend As DAO.Recordset
    Private bUserLock As Boolean
    Private m_bAllowPOEdit As Boolean
    Private m_bSOGOrder As Boolean
    Private displayTemperatureInCelsius As Boolean
    Private m_bIsDeleted As Boolean
    Private m_bIsIntraStoreTransfer As Boolean
    Private m_bIsOrderWFM As Boolean
    Private m_bIsOrderHFM As Boolean
    Private m_lOrderHeader_ID As Integer
    Private m_lOriginalOrder_ID As Integer
    Private m_lStoreNo As Integer
    Private m_lVendorID As Integer
    Private m_lVendorStoreNo As Integer
    Private m_lPurchaseLocationID As Integer
    Private m_lReceiveLocationID As Integer
    Private m_sVendorName As String
    Private m_bIsStore_Vend As Boolean
    Private m_iVendorType As enumVendorType
    Private m_bSent As Boolean
    Private m_lTransferFromSubTeam As Integer
    Private m_lTransferToSubTeam As Integer
    Private m_bReceivingAllowed As Boolean
    Private m_bItemsOrdered As Boolean
    Private m_bEXEWarehouse As Boolean
    Private m_bEXEWarehousePurchaseOrder As Boolean
    Private m_lCreatedBy As Integer
    Private m_bIsEXEDistributed As Boolean
    Private m_OrderDiscountExists As Boolean
    Private m_OrderDiscountType As Integer
    Private m_OrderDiscountAmt As Decimal
    Private m_DiscountAmtChanged As Boolean
    Private m_IsEinvoice As Boolean
    Private m_iCopyPO_OrderHeaderID As Integer
    Private m_iInvalidCopyPOItems_Id As Integer
    Private m_sFax As String
    Private m_sEmail As String
    Private m_iEInvoiceId As Integer
    Private m_sSubTeamName As String
    Private m_bAllowReceiveAll As Boolean
    Private m_bOverrideDefaultPOTransmissionMethod As Boolean
    Private iDefaultPOTransmission As Integer
    Private m_iTransmissionMethod As Integer
    Private m_bTransmissionMethodOverriden As Boolean
    Private m_sTransmissionTarget As String
    Private m_WasReceived As Boolean
    Private m_WasClosed As Boolean
    Private m_bOrderAccountingUploaded As Boolean
    Private m_bPayAgreedCost As Boolean
    Private m_IsDSDOrder As Boolean
    Private bOpenAndSend As Boolean
    Private JumpToOrderStatus As Boolean

    Private Shared m_bItemsReceived As Boolean
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public AutoSearch_OrderHeaderId As Integer?
    Public AutoSearch_OrderHeaderId_OneKnownMatch As Integer?

    Dim mdt As DataTable

#End Region

#Region "Properties"

    Public ReadOnly Property OrderDiscountExists() As Boolean
        Get
            OrderDiscountExists = m_OrderDiscountExists
        End Get
    End Property
    Public ReadOnly Property OrderDiscountType() As Integer
        Get
            OrderDiscountType = m_OrderDiscountType
        End Get
    End Property
    Public ReadOnly Property OrderDiscountAmt() As Decimal
        Get
            OrderDiscountAmt = m_OrderDiscountAmt
        End Get
    End Property
    Public ReadOnly Property AllowQuantityUnitSelection() As Boolean
        Get
            Return Not gbClosedOrder And (IsStore_Vend Or IsCredit) And (Not EXEWarehouse Or Not Sent Or (gbWarehouse And Me.cmdWarehouseSend.Enabled) Or ItemsReceived)
        End Get
    End Property
    Public ReadOnly Property EXEOrder() As Boolean
        Get
            EXEOrder = m_bEXEWarehouse And m_bIsEXEDistributed And (Not geOrderType = enumOrderType.Transfer)
        End Get
    End Property
    Public Property IsEXEDistributed() As Boolean
        Get
            IsEXEDistributed = m_bIsEXEDistributed
        End Get
        Set(ByVal Value As Boolean)
            m_bIsEXEDistributed = Value
        End Set
    End Property
    Public Property IsEInvoice() As Boolean
        Get
            IsEInvoice = m_IsEinvoice
        End Get
        Set(ByVal Value As Boolean)
            m_IsEinvoice = Value
        End Set
    End Property
    Public ReadOnly Property IsExternalVendor() As Boolean
        Get
            If m_lTransferFromSubTeam = -1 Then
                IsExternalVendor = True
            Else
                IsExternalVendor = False
            End If
        End Get
    End Property
    Public ReadOnly Property IsIntraStoreTransfer() As Boolean
        Get
            IsIntraStoreTransfer = m_bIsIntraStoreTransfer
        End Get
    End Property
    Public ReadOnly Property IsOrderWFM() As Boolean
        Get
            IsOrderWFM = m_bIsOrderWFM
        End Get
    End Property
    Public ReadOnly Property IsOrderHFM() As Boolean
        Get
            IsOrderHFM = m_bIsOrderHFM
        End Get
    End Property
    Public ReadOnly Property IsCredit() As Boolean
        Get
            If chkField(iOrderHeaderReturn_Order).CheckState = System.Windows.Forms.CheckState.Checked Then
                IsCredit = True
            Else
                IsCredit = False
            End If
        End Get
    End Property
    Public Property PreOrder() As Boolean
        Get
            PreOrder = m_bPre_Order
        End Get
        Set(ByVal Value As Boolean)
            m_bPre_Order = Value
        End Set
    End Property
    Public Property InvalidCopyPOItems_ID() As Integer
        Get
            InvalidCopyPOItems_ID = m_iInvalidCopyPOItems_Id
        End Get
        Set(ByVal Value As Integer)
            m_iInvalidCopyPOItems_Id = Value
        End Set
    End Property
    Public Property CopyPO_OrderHeaderID() As Integer
        Get
            CopyPO_OrderHeaderID = m_iCopyPO_OrderHeaderID
        End Get
        Set(ByVal Value As Integer)
            m_iCopyPO_OrderHeaderID = Value
        End Set
    End Property
    Public ReadOnly Property VendorType() As enumVendorType
        Get
            VendorType = m_iVendorType
        End Get
    End Property
    Public ReadOnly Property OrderHeader_ID() As Integer
        Get
            OrderHeader_ID = m_lOrderHeader_ID
        End Get
    End Property
    Public ReadOnly Property OriginalOrder_ID() As Integer
        Get
            OriginalOrder_ID = m_lOriginalOrder_ID
        End Get
    End Property
    Public ReadOnly Property StoreNo() As Integer
        Get
            StoreNo = m_lStoreNo
        End Get
    End Property
    Public ReadOnly Property EInvoiceId() As Integer
        Get
            EInvoiceId = m_iEInvoiceId
        End Get
    End Property
    Public ReadOnly Property VendorID() As Integer
        Get
            VendorID = m_lVendorID
        End Get
    End Property
    Public ReadOnly Property ReceiveLocationID() As Integer
        Get
            ReceiveLocationID = m_lReceiveLocationID
        End Get
    End Property
    Public ReadOnly Property IsStore_Vend() As Boolean
        Get
            IsStore_Vend = m_bIsStore_Vend
        End Get
    End Property
    Public ReadOnly Property Sent() As Boolean
        Get
            Sent = m_bSent
        End Get
    End Property
    Public ReadOnly Property TransferFromSubTeam() As Integer
        Get
            TransferFromSubTeam = m_lTransferFromSubTeam
        End Get
    End Property
    Public ReadOnly Property TransferToSubTeam() As Integer
        Get
            TransferToSubTeam = m_lTransferToSubTeam
        End Get
    End Property
    Public ReadOnly Property SubTeamLimit() As Integer
        Get
            SubTeamLimit = m_lSubTeamLimit
        End Get
    End Property
    Public ReadOnly Property ReceivingAllowed() As Boolean
        Get
            ReceivingAllowed = m_bReceivingAllowed
        End Get
    End Property
    Public Shared Property ItemsReceived() As Boolean
        Get
            ItemsReceived = m_bItemsReceived
        End Get
        Set(ByVal value As Boolean)
            m_bItemsReceived = value
        End Set
    End Property
    Public ReadOnly Property EXEWarehouse() As Boolean
        Get
            EXEWarehouse = m_bEXEWarehouse
        End Get
    End Property
    Public ReadOnly Property EXEWarehousePurchaseOrder() As Boolean
        Get
            EXEWarehousePurchaseOrder = m_bEXEWarehousePurchaseOrder And m_bIsEXEDistributed
        End Get
    End Property
    Public ReadOnly Property CreatedBy() As Integer
        Get
            CreatedBy = m_lCreatedBy
        End Get
    End Property
    Public ReadOnly Property VendorName() As String
        Get
            VendorName = m_sVendorName
        End Get
    End Property
    Public ReadOnly Property Fax() As String
        Get
            Fax = m_sFax
        End Get
    End Property
    Public ReadOnly Property Email() As String
        Get
            Email = m_sEmail
        End Get
    End Property
    Public Property OpenAndSend() As Boolean
        Get
            Return bOpenAndSend
        End Get
        Set(ByVal value As Boolean)
            bOpenAndSend = value
        End Set
    End Property
    Public ReadOnly Property IsOrderPayAgreedCost() As Boolean
        Get
            IsOrderPayAgreedCost = m_bPayAgreedCost
        End Get
    End Property
    Public ReadOnly Property IsDSDOrder() As Boolean
        Get
            IsDSDOrder = m_IsDSDOrder
        End Get
    End Property
    Public Property AllowReceiveAll() As Boolean
        Get
            Return m_bAllowReceiveAll
        End Get
        Set(ByVal value As Boolean)
            m_bAllowReceiveAll = value
        End Set
    End Property
    Public Property IsDeleted() As Boolean
        Get
            Return m_bIsDeleted
        End Get
        Set(ByVal value As Boolean)
            m_bIsDeleted = value
        End Set
    End Property

#End Region

    Public Sub New(ByVal OrderHeaderId As Integer, Optional ByVal JumpToOrderStatus As Boolean = False)
        InitializeComponent()
        glOrderHeaderID = OrderHeaderId
        Me.JumpToOrderStatus = JumpToOrderStatus
    End Sub

    Private Sub AddStatusInfo(ByRef sIn As String)
        logger.Debug("AddStatusInfo Entry")

        If Len(lblStatus.Text) > 0 Then
            If VB.Right(lblStatus.Text, 1) = "]" Then
                lblStatus.Text = Mid(lblStatus.Text, 1, Len(lblStatus.Text) - 1) & ", " & sIn & "]"
            Else
                lblStatus.Text = lblStatus.Text & ", " & sIn
            End If
        Else
            lblStatus.Text = sIn
        End If

        logger.Debug("AddStatusInfo Exit")
    End Sub

    Private Sub chkField_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkField.CheckStateChanged
        logger.Debug("chkField_CheckStateChanged Entry")

        If Me.IsInitializing Then Exit Sub

        Dim Index As Short = chkField.GetIndex(eventSender)

        If Not m_bLoading Then
            m_bDataChanged = True
        End If

        logger.Debug("chkField_CheckStateChanged Exit")
    End Sub

    Private Sub cmbField_KeyPress(ByVal eventSender As Object, ByVal EventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbField.KeyPress
        logger.Debug("cmbField_KeyPress Entry")

        Dim KeyAscii As Short = Asc(EventArgs.KeyChar)
        Dim Index As Short = cmbField.GetIndex(eventSender)

        If KeyAscii = ASCII_BACKSPACE Then
            cmbField(Index).SelectedIndex = -1
        End If

        EventArgs.KeyChar = Chr(KeyAscii)

        If KeyAscii = ASCII_NULL Then
            EventArgs.Handled = True
        End If

        logger.Debug("cmbField_KeyPress Exit")
    End Sub

    Private Sub cmbField_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbField.SelectedIndexChanged
        logger.Debug("cmbField_SelectedIndexChanged Entry")

        If Me.IsInitializing Then Exit Sub

        If m_bLoading Then Exit Sub

        Dim Index As Short = cmbField.GetIndex(eventSender)

        If Index = iOrderHeaderDiscountType Then
            m_OrderDiscountType = cmbField(iOrderHeaderDiscountType).SelectedIndex

            'The following code is admittedly a stinky hack
            'but it was unavoidable.  Since order header discount
            'types are limited to percent discounts only,
            'the index of the percent discount type is now 1
            'instead of 2 as it used to be.  
            'If the user selects a percent discount type
            'we force the index to be 2 to keep all the IRMA
            'gears turning smoothly without complaint.
            If m_OrderDiscountType = 1 Then
                m_OrderDiscountType = 2
            End If

            m_OrderDiscountExists = IIf(m_OrderDiscountType = 0, False, True)
            SaveData()
            UpdateOrderItemsCost()
            RefreshOrderDetails(CInt(txtField(iOrderHeaderOrderHeader_ID).Text), True)
        End If

        m_bDataChanged = True

        logger.Debug("cmbField_SelectedIndexChanged Exit")
    End Sub

    Public Sub ConfigureOrderItemSearch(ByRef frmSearchInstance As frmOrderItemSearch)
        logger.Debug("ConfigureOrderItemSearch Entry")

        Dim bIsVendorFacility As Boolean
        Dim bIsVendorStoreSame As Boolean

        glOrderHeaderID = CInt(txtField(iOrderHeaderOrderHeader_ID).Text)
        glVendorID = m_lVendorID
        glSubTeamNo = m_lTransferFromSubTeam
        glTransfer_To_SubTeam = m_lTransferToSubTeam
        gbClosedOrder = m_bOrderClosed
        glOrderItemID = 0

        frmSearchInstance.StatusLabel = lblStatus.Text
        frmSearchInstance.OrderHeader_ID = Me.OrderHeader_ID

        If ((m_iVendorType = enumVendorType.RegionalFacilityManufacturer) Or (m_iVendorType = enumVendorType.RegionalFacilityDistCenter)) Then
            ' load subteams with different list
            bIsVendorFacility = True
        End If

        If geOrderType = enumOrderType.Transfer Then
            ' if vendor and store are the same, list of subteams is not limited by zone, but rather by store
            If (m_lStoreNo = m_lVendorStoreNo) Then
                ' can only happen for transfer orders
                bIsVendorStoreSame = True
            End If
        End If

        Call frmSearchInstance.LoadSubTeamCombo(bIsVendorFacility, bIsVendorStoreSame, m_lVendorStoreNo, m_lTransferFromSubTeam)

        Select Case geOrderType
            Case enumOrderType.Distribution
                frmSearchInstance.VendorName = txtField(iOrderHeaderCompanyName).Text
                frmSearchInstance.LimitStore_No = m_lStoreNo
                If (m_bFrom_SubTeam_Unrestricted = False) Then
                    'non-mfg subteam can only distribute its own items
                    frmSearchInstance.LimitSubTeam_No = m_lTransferFromSubTeam
                ElseIf (m_bTo_SubTeam_Unrestricted = False) Then
                    ' retail subteam must match the TO subteam
                    ' cause non-mfg subteam can only distribute into itself the same
                    frmSearchInstance.LimitSubTeam_No = m_lTransferToSubTeam
                Else
                    ' both subteams are mfg, so default to the receiving subteam
                    frmSearchInstance.SubTeam_No = 0
                End If
                frmSearchInstance.IncludeDiscontinued = enumChkBoxValues.UncheckedEnabled
            Case enumOrderType.Purchase, enumOrderType.Flowthru
                frmSearchInstance.VendorName = txtField(iOrderHeaderCompanyName).Text
                frmSearchInstance.LimitStore_No = m_lStoreNo
                If (geProductType = enumProductType.Product) Then
                    ' Purchase Orders never have a From-SubTeam
                    If (m_bTo_SubTeam_Unrestricted = False) Then
                        ' retail subteam must match the TO subteam
                        ' cause non-mfg subteam can only purchase into itself the same
                        frmSearchInstance.LimitSubTeam_No = m_lTransferToSubTeam
                    Else
                        ' To-subteam is mfg, so default to the receiving subteam
                        frmSearchInstance.SubTeam_No = 0
                    End If
                Else
                    frmSearchInstance.LimitSubTeam_No = 0
                End If
                If (DeterminePurchaserStore(m_lPurchaseLocationID) Or DeterminePurchaserFacility(m_lPurchaseLocationID)) Then
                    ' Don't let retail stores or distribution centers order discontinued items from external vendors
                    frmSearchInstance.IncludeDiscontinued = enumChkBoxValues.UncheckedDisabled
                Else
                    If (Not DeterminePurchaserStore(m_lPurchaseLocationID) And Not DeterminePurchaserFacility(m_lPurchaseLocationID)) Then
                        frmSearchInstance.IncludeDiscontinued = enumChkBoxValues.UncheckedDisabled
                    Else
                        frmSearchInstance.IncludeDiscontinued = enumChkBoxValues.UncheckedEnabled
                    End If
                End If
            Case enumOrderType.Transfer
                If (geProductType = enumProductType.Product) Then
                    'TFS 8880, Faisal Ahmed, 12/10/2012 - Open up all subteams for transfers
                    frmSearchInstance.LimitSubTeam_No = 0

                ElseIf (geProductType = enumProductType.OtherSupplies) Then
                    'TFS 9562, Faisal Ahmed, 12/19/2012 - Limiting to only selected supply type subteams
                    frmSearchInstance.LimitSubTeam_No = m_lTransferToSubTeam
                Else
                    frmSearchInstance.LimitSubTeam_No = 0
                End If
                frmSearchInstance.IncludeDiscontinued = enumChkBoxValues.UncheckedEnabled
        End Select

        If m_bItemsOrdered Then
            If geOrderType = enumOrderType.Distribution Then
                frmSearchInstance.IsPreOrderItem = m_bPre_Order
                If m_bEXEWarehouse Then frmSearchInstance.IsEXEDistributed = m_bIsEXEDistributed
            End If
        End If

        logger.Debug("ConfigureOrderItemSearch Exit")
    End Sub

    Private Sub cmdAddItem_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAddItem.Click
        logger.Debug("cmdAddItem_Click Entry")

        Dim frmSearchInstance As New frmOrderItemSearch

        Call ConfigureOrderItemSearch(frmSearchInstance)

        frmSearchInstance.ShowDialog()

        If glItemID <> 0 Then

            'get a list of (item, item_description) from the order that match glItemId
            Dim results As IEnumerable(Of KeyValuePair(Of Integer, String)) = (From row As UltraGridRow In ugrdItems.Rows Select New KeyValuePair(Of Integer, String)(row.Cells("item_key").Value, row.Cells("item_description").Value.ToString)).Where(Function(x) x.Key = glItemID)
            Dim dialogResult As DialogResult

            'if the order already containts the item key we are trying to add...
            If results.Count > 0 Then

                'display notification.
                dialogResult = MessageBox.Show(String.Format("[ {0} ] already exists on this order.", results(0).Value, "Notice", MessageBoxButtons.OK, MessageBoxIcon.Exclamation))

                'find the item in the grid. select it. activate it. open the item detail screen to edit it.
                Dim colrow As UltraGridRow = ugrdItems.Rows.OfType(Of UltraGridRow)().ToList().Find(Function(row) row.Cells("Item_Key").Value.Equals(glItemID))

                colrow.Selected = True
                colrow.Activate()
                glItemID = 0
                cmdEditItem.PerformClick()

                Exit Sub
            End If

            m_bItemsOrdered = True
            m_bPre_Order = frmSearchInstance.IsPreOrderItem
            m_bIsEXEDistributed = frmSearchInstance.IsEXEDistributed

            frmSearchInstance.Close()
            frmSearchInstance.Dispose()

            frmOrdersItem.ShowDialog()
            frmOrdersItem.Close()
            frmOrdersItem.Dispose()

            DistributeFreight(CInt(txtField(iOrderHeaderOrderHeader_ID).Text),
                              CDec(txtField(iOrderHeader3rdPartyFreight).Text))

            RefreshOrderDetails(CInt(txtField(iOrderHeaderOrderHeader_ID).Text), True)

        Else
            frmSearchInstance.Close()
            frmSearchInstance.Dispose()
        End If

        logger.Debug("cmdAddItem_Click Exit")
    End Sub

    Private Sub cmdAddOrder_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAddOrder.Click
        logger.Debug("cmdAddOrder_Click Entry")

        '-- if it's not the first then update the data
        If m_lOrderHeader_ID <> -1 Then
            If Not SaveData() Then Exit Sub

            If m_bItemsReceived And (Not m_bOrderClosed) And (Not lblReadOnly.Visible) Then
                ' Do not require the user to close the order if the Transfer Subteam (glSubTeamNo) is set because they cannot enter any data
                ' on the OrderStatus UI screen when the glSubTeamNo = -1.
                If glSubTeamNo = -1 Then
                    MsgBox("Items have been received.  The order must be closed." & vbCrLf & "Please enter Invoice/Document Data to close the order.", MsgBoxStyle.Information, Me.Text)
                    cmdCloseOrder.PerformClick()
                    logger.Info("cmdAddOrder_Click " & " Items received - order must be closed." & vbCrLf & "Please enter Invoice/Document Data and close...")
                    logger.Debug("cmdAddOrder_Click Exit")
                    Exit Sub
                Else
                    ' Prompt the user to close this order.  They are not allowed to enter invoice information on the
                    ' OrderStatus UI screen, so we can automatically do the close for them.
                    CloseOrder()
                End If
            End If
        End If

        '-- Set search criteria
        glVendorID = 0
        giSearchType = iSearchVendorCompany
        glOrderHeaderID = 0

        '-- Open the search form
        frmSearch.Text = "Add Order For Vendor"
        frmSearch.ShowDialog()
        frmSearch.Close()
        frmSearch.Dispose()

        Dim orderCreateOk As Boolean = True

        '-- If they added a vendor then add an order
        If glVendorID > 0 Then
            Dim StoreLimit As Integer = Global_Renamed.glStore_Limit
            If (StoreLimit <> 0) Then
                If (OrderingDAO.CheckVendorIsDSDForStore(glVendorID, StoreLimit)) Then
                    MsgBox("Cannot create a purchase order for a Guaranteed Sale Supplier.  Please use IRMA Mobile to create a Receiving Document for this vendor.", MsgBoxStyle.Information, "Cannot Create Order")
                    orderCreateOk = False
                End If
            End If

            If (orderCreateOk) Then
                frmOrderAdd.ShowDialog()
                frmOrderAdd.Close()
                frmOrderAdd.Dispose()
            End If
        End If

        If glOrderHeaderID <> 0 Then
            RefreshDataSource(glOrderHeaderID)
        ElseIf m_lOrderHeader_ID <> -1 Then
            RefreshDataSource(m_lOrderHeader_ID)
        End If

        logger.Debug("cmdAddOrder_Click Exit")
    End Sub

    Private Sub cmdChgVendor_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdChgVendor.Click
        logger.Debug("cmdChgVendor_Click Entry")

        '-- Set glvendorid to none found
        glVendorID = 0

        '-- Set the search type
        giSearchType = iSearchAllVendors

        '-- Open the search form
        frmSearch.Text = "Search for Vendor by Company Name"
        frmSearch.ShowDialog()
        frmSearch.Close()
        frmSearch.Dispose()

        If glVendorID > 0 Then
            If SaveData() Then
                SQLExecute("EXEC UpdateOrderVendor " & m_lOrderHeader_ID & "," & glVendorID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                RefreshDataSource(m_lOrderHeader_ID)
            End If
        End If

        logger.Debug("cmdChgVendor_Click Exit")
    End Sub

    Private Sub cmdCloseOrder_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCloseOrder.Click
        logger.Debug("cmdCloseOrder_Click Entry")

        m_WasClosed = False

        If Not SaveData() Then Exit Sub

        ' TFS 8325, 03/30/2013, Faisal Ahmed - Forces users to navigate to refused items screen if there is refused item with no invoice cost or refused quantity
        ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
        'If Not IsValidRefusedItemList(m_lOrderHeader_ID) Then
        '    If MsgBox("This PO contains refused items which need to be reviewed before the order is closed.  Review these items now?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Warning!") = MsgBoxResult.No Then
        '        Exit Sub
        '    Else
        '        cmdItemsRefused.PerformClick()
        '        Exit Sub
        '    End If
        'End If

        glOrderHeaderID = m_lOrderHeader_ID
        glSubTeamNo = m_lTransferFromSubTeam
        gsStoreName = CStr(VB6.GetItemString(cmbField(iOrderHeaderReceiveLocation_ID), cmbField(iOrderHeaderReceiveLocation_ID).SelectedIndex))
        gsPOCreatorUserName = txtField(iOrderHeaderCreatedBy).Text

        If m_lTransferFromSubTeam = -1 Then
            gsVendorName = txtField(iOrderHeaderCompanyName).Text
        Else
            gsVendorName = ""
        End If

        frmOrderStatus.OrderType = geOrderType
        frmOrderStatus.EInvoiceid = m_iEInvoiceId
        frmOrderStatus.ShowDialog()
        frmOrderStatus.Dispose()

        m_WasClosed = True
        RefreshDataSource(m_lOrderHeader_ID)

        logger.Debug("cmdCloseOrder_Click Exit")
    End Sub

    Private Sub cmdDeleteItem_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDeleteItem.Click
        logger.Debug("cmdDeleteItem_Click Entry")

        Dim lIgnoreErrNum(0) As Integer

        If ugrdItems.Selected.Rows.Count = 1 Then

            glOrderItemID = CInt(ugrdItems.Selected.Rows(0).Cells("OrderItem_ID").Value)

            '-- Make sure they really want to delete that OrderItem
            If MsgBox("Really delete this line item?", MsgBoxStyle.Question + MsgBoxStyle.DefaultButton2, "Confirm Line Item Delete") = MsgBoxResult.No Then
                Exit Sub
            End If

            '-- Delete the OrderItem from the database
            lIgnoreErrNum(0) = 50002

            On Error Resume Next
            logger.Info("cmdDeleteItem_Click - calling DeleteOrderItem to remove an item from the order: glOrderItemID=" + glOrderItemID.ToString + ", giUserID=" + giUserID.ToString)
            SQLExecute3("EXEC DeleteOrderItem " & glOrderItemID & "," & giUserID, DAO.RecordsetOptionEnum.dbSQLPassThrough, lIgnoreErrNum)
            If Err.Number <> 0 Then
                MsgBox(Err.Description, MsgBoxStyle.Exclamation, Me.Text)
                logger.Error("cmdDeleteItem_Click Error in " & "EXEC DeleteOrderItem " & glOrderItemID & "," & giUserID & Err.Description)
                logger.Debug("cmdDeleteItem_Click Exit")
                Exit Sub
            End If
            On Error GoTo 0

            DistributeFreight(CInt(txtField(iOrderHeaderOrderHeader_ID).Text), CDec(txtField(iOrderHeader3rdPartyFreight).Text))

            RefreshOrderDetails(CInt(txtField(iOrderHeaderOrderHeader_ID).Text), True)

        Else
            MsgBox("Please select a line item to delete.", MsgBoxStyle.Information, "Remove Item")
        End If

        logger.Debug("cmdDeleteItem_Click Exit")
    End Sub

    Private Sub cmdDeleteOrder_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDeleteOrder.Click
        logger.Debug("cmdDeleteOrder_Click Entry")

        'The following check was added as a stopgap measure to ensure that what is being deleted is correct.
        'It appears that m_lOrderHeader_ID is correct, but there were reports of PO's being deleted "mysteriously" and this measure is just to make absolutely sure.
        If CDbl(txtField(iOrderHeaderOrderHeader_ID).Text) <> m_lOrderHeader_ID Then
            MsgBox("Application error - internal PO Number, " & m_lOrderHeader_ID & ", does not match what is currently displayed." & vbCrLf & "The order has not been deleted.", MsgBoxStyle.Exclamation, Me.Text)
            Exit Sub
        End If

        If gbBuyer Or gbPOAccountant Then
            SQLOpenRS(m_rsOrder, "EXEC GetOrderStatus " & m_lOrderHeader_ID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            If Not IsDBNull(m_rsOrder.Fields("CloseDate").Value) Then
                MsgBox("This order is closed and cannot be deleted.", MsgBoxStyle.Information, "Delete Order")
                m_rsOrder.Close()
                logger.Info("cmdDeleteOrder_Click " & " This order is closed and cannot be deleted!")
                logger.Debug("cmdDeleteOrder_Click Exit")
                Exit Sub
            End If
            m_rsOrder.Close()
        End If

        If frmDeletePOReasons.ShowDialog() = Windows.Forms.DialogResult.Yes Then
            Dim ReasonCode As Integer = Convert.ToInt32(frmDeletePOReasons.ulDeleteReasonCode.Value)

            '-- Delete the OrderItem from the database
            Dim lIgnoreErrNum(0) As Integer
            lIgnoreErrNum(0) = 50002
            On Error Resume Next
            If (OrderingDAO.DeleteOrderHeader(m_lOrderHeader_ID, giUserID, ReasonCode) = False) Then Exit Sub
            On Error GoTo 0

            If bSpecificOrder Then
                Me.Close()
            Else
                RefreshDataSource(-1)
            End If
        End If

        frmDeletePOReasons.Close()

        logger.Debug("cmdDeleteOrder_Click Exit")
    End Sub

    Private Sub cmdEditItem_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEditItem.Click
        logger.Debug("cmdEditItem_Click Entry")

        If ugrdItems.Selected.Rows.Count = 1 Then
            glOrderHeaderID = CInt(txtField(iOrderHeaderOrderHeader_ID).Text)
            glOrderItemID = CInt(ugrdItems.Selected.Rows(0).Cells("OrderItem_ID").Value)
            glVendorID = m_lVendorID
            glItemID = 0
            glSubTeamNo = m_lTransferFromSubTeam
            gbClosedOrder = m_bOrderClosed
            If m_lVendorID = CDbl(ComboValue(cmbField(iOrderHeaderReceiveLocation_ID))) Then
                glTransfer_To_SubTeam = m_lTransferToSubTeam
            Else
                glTransfer_To_SubTeam = -1
            End If
            If m_lTransferFromSubTeam = -1 Then
                gsVendorName = txtField(iOrderHeaderCompanyName).Text
            Else
                gsVendorName = ""
            End If

            frmOrdersItem.ShowDialog()
            frmOrdersItem.Close()
            frmOrdersItem.Dispose()

            RefreshOrderDetails(CInt(txtField(iOrderHeaderOrderHeader_ID).Text), True)
        Else
            MsgBox("Please select a line item to edit.", MsgBoxStyle.Information, "Edit Item")
        End If

        logger.Debug("cmdEditItem_Click Exit")
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        Me.Close()
        logger.Debug("cmdExit_Click Exit")
    End Sub

    Private Sub cmdOrderList_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOrderList.Click
        logger.Debug("cmdOrderList_Click Entry")

        Dim bIsVendorFacility As Boolean
        Dim bIsVendorStoreSame As Boolean

        glSubTeamNo = m_lTransferFromSubTeam
        glTransfer_To_SubTeam = m_lTransferToSubTeam
        glOrderHeaderID = CInt(txtField(iOrderHeaderOrderHeader_ID).Text)

        frmOrderList.StatusLabel = lblStatus.Text
        frmOrderList.ReceiveLocationIsDistribution = m_ReceiveLocationIsDistribution
        frmOrderList.IsVendorExternal = m_IsVendorExternal

        If ((m_iVendorType = enumVendorType.RegionalFacilityManufacturer) Or (m_iVendorType = enumVendorType.RegionalFacilityDistCenter)) Then
            ' load subteams with different list
            bIsVendorFacility = True
        End If

        If geOrderType = enumOrderType.Transfer Then
            ' if vendor and store are the same, list of subteams is not limited by zone, but rather by store
            If (m_lStoreNo = m_lVendorStoreNo) Then
                ' can only happen for transfer orders
                bIsVendorStoreSame = True
            End If
        End If

        Call frmOrderList.LoadSubTeamCombo(bIsVendorFacility, bIsVendorStoreSame, m_lVendorStoreNo, m_lTransferFromSubTeam)

        Select Case geOrderType
            Case enumOrderType.Distribution
                If (m_bFrom_SubTeam_Unrestricted = False) Then
                    'non-mfg subteam can only distribute its own items
                    frmOrderList.LimitSubTeam_No = m_lTransferFromSubTeam
                ElseIf (m_bTo_SubTeam_Unrestricted = False) Then
                    ' retail subteam must match the TO subteam
                    ' cause non-mfg subteam can only distribute into itself the same
                    frmOrderList.LimitSubTeam_No = m_lTransferToSubTeam
                Else
                    ' both subteams are mfg, so default to the receiving subteam
                    frmOrderList.SubTeam_No = m_lTransferToSubTeam
                End If
                frmOrderList.IncludeDiscontinued = enumChkBoxValues.UncheckedEnabled

            Case enumOrderType.Purchase, enumOrderType.Flowthru
                If (geProductType = enumProductType.Product) Then
                    ' Purchase Orders never have a From-SubTeam
                    If (m_bTo_SubTeam_Unrestricted = False) Then
                        ' retail subteam must match the TO subteam
                        ' cause non-mfg subteam can only purchase into itself the same
                        frmOrderList.LimitSubTeam_No = m_lTransferToSubTeam
                    Else
                        ' both To-subteam is mfg, so default to the receiving subteam
                        frmOrderList.SubTeam_No = m_lTransferToSubTeam
                    End If
                Else
                    frmOrderList.LimitSubTeam_No = m_lTransferToSubTeam
                End If
                If (DeterminePurchaserStore(m_lPurchaseLocationID) Or DeterminePurchaserFacility(m_lPurchaseLocationID)) Then
                    ' Don't let retail stores or distribution centers order discontinued items from external vendors
                    frmOrderList.IncludeDiscontinued = enumChkBoxValues.UncheckedDisabled
                Else
                    If (Not DeterminePurchaserStore(m_lPurchaseLocationID) And Not DeterminePurchaserFacility(m_lPurchaseLocationID)) Then
                        frmOrderList.IncludeDiscontinued = enumChkBoxValues.UncheckedDisabled
                    Else
                        frmOrderList.IncludeDiscontinued = enumChkBoxValues.UncheckedEnabled
                    End If
                End If
            Case enumOrderType.Transfer
                If (geProductType = enumProductType.Product) Then
                    If (m_bFrom_SubTeam_Unrestricted = False) Then
                        'non-mfg/non-expense subteam can only transfer its own items
                        frmOrderList.LimitSubTeam_No = m_lTransferFromSubTeam
                    ElseIf (m_bTo_SubTeam_Unrestricted = False) Then
                        ' retail subteam must match the TO subteam
                        ' cause non-mfg/non-expense subteam can only transfer into itself the same
                        frmOrderList.LimitSubTeam_No = m_lTransferToSubTeam
                    Else
                        ' both subteams are mfg/expense, so default to the receiving subteam
                        frmOrderList.SubTeam_No = m_lTransferToSubTeam
                    End If
                Else
                    frmOrderList.LimitSubTeam_No = 0
                End If
                frmOrderList.IncludeDiscontinued = enumChkBoxValues.UncheckedEnabled
        End Select

        frmOrderList.ProductType_ID = geProductType

        Call frmOrderList.InitializeForm()
        Call frmOrderList.ShowDialog()

        frmOrderList.Dispose()

        DistributeFreight(CInt(txtField(iOrderHeaderOrderHeader_ID).Text), CDec(txtField(iOrderHeader3rdPartyFreight).Text))

        RefreshOrderDetails(CInt(txtField(iOrderHeaderOrderHeader_ID).Text), True)

        logger.Debug("cmdOrderList_Click Exit")
    End Sub

    Private Sub cmdOrderNotes_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdOrderNotes.Click
        logger.Debug("cmdOrderNotes_Click Entry")

        glOrderHeaderID = m_lOrderHeader_ID

        frmOrdersDesc.ShowDialog()
        frmOrdersDesc.Close()
        frmOrdersDesc.Dispose()

        logger.Debug("cmdOrderNotes_Click Exit")
    End Sub

    Private Sub cmdReceive_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReceive.Click
        logger.Debug("cmdReceive_Click Entry")

        Dim frm As New frmReceivingList

        m_WasReceived = False

        If Not SaveData() Then Exit Sub

        RefreshDataSource(m_lOrderHeader_ID)

        glSubTeamNo = m_lTransferFromSubTeam
        gsStoreName = CStr(VB6.GetItemString(cmbField(iOrderHeaderReceiveLocation_ID), cmbField(iOrderHeaderReceiveLocation_ID).SelectedIndex))

        gsPOCreatorUserName = txtField(iOrderHeaderCreatedBy).Text

        If m_lVendorID = CDbl(ComboValue(cmbField(iOrderHeaderReceiveLocation_ID))) Then
            glTransfer_To_SubTeam = m_lTransferToSubTeam
        Else
            glTransfer_To_SubTeam = -1
        End If

        If m_lTransferFromSubTeam = -1 Then
            gsVendorName = txtField(iOrderHeaderCompanyName).Text
        Else
            gsVendorName = ""
        End If

        glOrderHeaderID = m_lOrderHeader_ID

        frm.OrderHeaderId = m_lOrderHeader_ID
        frm.TransferFromSubTeamNo = m_lTransferFromSubTeam
        frm.EInvoicingId = m_iEInvoiceId
        frm.ShowDialog()
        frm.Close()
        frm.Dispose()


        m_WasReceived = True
        RefreshDataSource(m_lOrderHeader_ID)

        logger.Debug("cmdReceive_Click Exit")

me_exit:
        logger.Debug("cmdReceive_Click Exit")
        Exit Sub
    End Sub

    Private Sub cmdReports_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdReports.Click
        logger.Debug("cmdReports_Click Entry")

        '-- Save any data first
        If Not SaveData() Then Exit Sub
        RefreshDataSource(m_lOrderHeader_ID)

        glOrderHeaderID = m_lOrderHeader_ID
        frmReports.ShowDialog()

        logger.Debug("cmdReports_Click Exit")
    End Sub

    Private Sub cmdDistributionCreditOrder_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDistributionCreditOrder.Click
        logger.Debug("cmdDistributionCreditOrder_Click Entry")

        glOrderHeaderID = CInt(txtField(iOrderHeaderOrderHeader_ID).Text)
        glSubTeamNo = m_lTransferFromSubTeam

        frmDistributionCreditOrder.ShowDialog()

        If SaveData() Then
            RefreshDataSource(m_lOrderHeader_ID)
        End If

        logger.Debug("cmdDistributionCreditOrder_Click Exit")
    End Sub

    Private Sub cmdSearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSearch.Click
        logger.Debug("cmdSearch_Click Entry")

        '-- Save any data first
        If m_lOrderHeader_ID <> -1 Then
            If Not SaveData() Then Exit Sub

            If m_bItemsReceived And (Not m_bOrderClosed) And (Not lblReadOnly.Visible) Then
                ' Do not require the user to close the order if the Transfer Subteam (glSubTeamNo) is set because they cannot enter invoice data
                ' on the OrderStatus UI screen when the glSubTeamNo = -1.
                If glSubTeamNo = -1 Then
                    logger.Info("cmdSearch_Click " & "Items received - order must be closed." & vbCrLf & "Please enter Invoice/Document Data and close...")
                    MsgBox("Items have been received.  The order must be closed." & vbCrLf & "Please enter Invoice/Document Data to close the order.", MsgBoxStyle.Information, Me.Text)
                    cmdCloseOrder.PerformClick()
                    logger.Debug("cmdSearch_Click Exit")
                    Exit Sub
                Else
                    ' Prompt the user to close this order.  They are not allowed to enter invoice information on the
                    ' OrderStatus UI screen, so we can automatically do the close for them.
                    CloseOrder()
                End If
            End If
        End If

        '-- Set it to no order selected
        glOrderHeaderID = 0

        '-- Go to the search screen
        frmOrdersSearch.AutoSearch_OrderHeaderId = Me.AutoSearch_OrderHeaderId
        frmOrdersSearch.ShowDialog()
        frmOrdersSearch.Close()
        frmOrdersSearch.Dispose()

        '-- See if they choose anything
        If glOrderHeaderID <> 0 Then
            UnlockPO()
            RefreshDataSource(glOrderHeaderID)
        Else
            RefreshDataSource(m_lOrderHeader_ID)
        End If

        logger.Debug("cmdSearch_Click Exit")
    End Sub

    Private Sub cmdSendOrder_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSendOrder.Click
        logger.Debug("cmdSendOrder_Click Entry")

        Dim bSend As Boolean
        Dim bFax As Boolean
        Dim bEmail As Boolean
        Dim bManual As Boolean
        Dim bElectronic As Boolean
        Dim sFaxNbr As String = Nothing
        Dim sEmailAddr As String = Nothing
        Dim sendValidation As WholeFoods.IRMA.Ordering.BusinessLogic.OrderValidationBO = New WholeFoods.IRMA.Ordering.BusinessLogic.OrderValidationBO
        Dim sendMessage As String = Nothing
        Dim sendAddresseeMessage As String = Nothing

        ' If the order is an already existing order for a vendor which is now GSS, do not allow the user to send it.
        If (OrderingDAO.CheckVendorIsDSDForStore(m_lVendorID, m_lStoreNo)) Then
            MsgBox("Cannot send a purchase order for a Guaranteed Sale Supplier.  Please use IRMA Mobile to create a Receiving Document for this vendor.", MsgBoxStyle.Information, "Cannot Send Order")
            Exit Sub
        End If

        If geOrderType = enumOrderType.Distribution Then
            'tfs#13496:  adding buyer access. order window error was being thrown even when no windows were set up. buyers needed access rights. 
            If Not (gbSuperUser Or gbWarehouse Or gbDCAdmin Or gbBuyer) And Not IsOrderWindowOpen(GetStoreZone(m_lVendorStoreNo), m_lTransferToSubTeam, m_lVendorStoreNo) Then
                MsgBox("This order cannot be sent because the order window is not open.  Please contact the facility at " & GetVendorPhoneNumber(m_lVendorID) & " for assistance.", MsgBoxStyle.Information, Me.Text)
                logger.Debug("cmdSendOrder_Click Exit")
                Exit Sub
            End If
        End If

        If (DateDiff(DateInterval.Day, SystemDateTime(True), dtpExpectedDate.Value) < 0) And (Not m_bSent) Then
            MsgBox("Expected Date cannot be in the past.", MsgBoxStyle.Information, Me.Text)
            dtpExpectedDate.Focus()
            logger.Info("cmdSendOrder_Click Expected Date cannot be in the past.")
            logger.Debug("cmdSendOrder_Click Exit")
            Exit Sub
        End If

        If m_bMultipleSubTeams Then
            MsgBox("There are items for multiple subteams on this order." & vbCrLf & "The order cannot be sent unless all items are for the same subteam.", MsgBoxStyle.Information, Me.Text)
            logger.Info("cmdSendOrder_Click " & " There are items for multiple sub-teams on this order." & vbCrLf & " Send not allowed unless all items are for the same sub-team." & vbCrLf & " Delete items that are not for the order sub-team.")
            logger.Debug("cmdSendOrder_Click Exit")
            Exit Sub
        End If

        If ugrdItems.Rows.Count = 0 Then
            MsgBox("The order cannot be sent with no items.  Please add items to this order.", MsgBoxStyle.Information, Me.Text)
            logger.Info("cmdSendOrder_Click " & "There are no items on this order - send not allowed")
            logger.Debug("cmdSendOrder_Click Exit")
            Exit Sub
        End If

        If m_bSeafoodMissingCountryInfo Then
            MsgBox("A seafood item is missing country of origin/processing information." & vbCrLf & "The order cannot be sent without this information", MsgBoxStyle.Information, Me.Text)
            logger.Info("cmdSendOrder_Click " & " Seafood item missing country of origin/processing info." & vbCrLf & " Send not allowed without this information")
            logger.Debug("cmdSendOrder_Click Exit")
            Exit Sub
        End If

        If m_lTransferFromSubTeam = -1 Then

            Dim frmOrderSend As New frmOrderSend()

            'TFS Task 8316 - Allow user to select PO Transmission method
            frmOrderSend.Fax = CBool(m_iTransmissionMethod = 1)
            frmOrderSend.Email = CBool(m_iTransmissionMethod = 2)
            frmOrderSend.Manual = CBool(m_iTransmissionMethod = 3)
            frmOrderSend.Electronic = CBool(m_iTransmissionMethod = 4)

            'Set email and fax values on form based on stored vendor profile info
            'if we are not in a production environment, replace with user information and user fax number

            RefreshOrderSendDetails(Me.VendorID)

            frmOrderSend.FaxNbr = m_sFax

            If My.Application.IsProduction Then
                frmOrderSend.EmailAddr = m_sEmail
            Else
                frmOrderSend.EmailAddr = API.GetADUserInfo(Environment.UserName, "mail")
            End If

            'Only enable radio buttons and text fields if instance data flag is true
            m_bOverrideDefaultPOTransmissionMethod = InstanceDataDAO.IsFlagActive("OverrideDefaultPOTransmissionMethod")

            If (m_bOverrideDefaultPOTransmissionMethod) Then
                If m_iTransmissionMethod = 4 And geOrderType = enumOrderType.Purchase And Not IsCredit Then
                    frmOrderSend._optPOTrans_0.Enabled = False
                    frmOrderSend._optPOTrans_1.Enabled = False
                    frmOrderSend._optPOTrans_2.Enabled = True
                    frmOrderSend._optPOTrans_3.Enabled = CBool(GetVendorTransmissionType(Me.VendorID) = 4)
                    frmOrderSend._txtField_0.Enabled = False
                    frmOrderSend._txtField_1.Enabled = False
                ElseIf m_iTransmissionMethod = 4 And geOrderType = enumOrderType.Purchase And IsCredit Then
                    'disable Electronic and default to Manual due to Electronic Ordering not working for Credit Orders
                    frmOrderSend._optPOTrans_0.Enabled = True
                    frmOrderSend._optPOTrans_1.Enabled = True
                    frmOrderSend._optPOTrans_2.Enabled = True
                    frmOrderSend._optPOTrans_3.Enabled = False
                    frmOrderSend._txtField_0.Enabled = True
                    frmOrderSend._txtField_1.Enabled = True
                    frmOrderSend._optPOTrans_2.Checked = True
                Else
                    frmOrderSend._optPOTrans_0.Enabled = True
                    frmOrderSend._optPOTrans_1.Enabled = True
                    frmOrderSend._optPOTrans_2.Enabled = True
                    frmOrderSend._optPOTrans_3.Enabled = CBool(GetVendorTransmissionType(Me.VendorID) = 4 And Not IsCredit)
                    frmOrderSend._txtField_0.Enabled = True
                    frmOrderSend._txtField_1.Enabled = True
                End If
            End If

            frmOrderSend.ShowDialog()

            If frmOrderSend.Send Then
                CheckBox_DropShip.Checked = frmOrderSend.IsDropShipment
                bSend = True
                Me.m_bTransmissionMethodOverriden = False

                If frmOrderSend.TransmissionMethod <> iDefaultPOTransmission Then
                    Me.m_bTransmissionMethodOverriden = True
                End If

                If (m_sEmail <> frmOrderSend.EmailAddr) Or (m_sFax <> frmOrderSend.FaxNbr) Then
                    Me.m_bTransmissionMethodOverriden = True
                End If

                bFax = frmOrderSend.Fax
                bEmail = frmOrderSend.Email
                bManual = frmOrderSend.Manual
                bElectronic = frmOrderSend.Electronic
                sFaxNbr = frmOrderSend.FaxNbr
                sEmailAddr = frmOrderSend.EmailAddr
            End If

            frmOrderSend.Close()

            If Not bSend Then
                Exit Sub
            Else
                'Set appropriate buttons and clear out previous selections
                If bFax Then
                    Me.lblTransmissionType.Text = "Transmission Type: Fax"
                ElseIf bEmail Then
                    Me.lblTransmissionType.Text = "Transmission Type: Email"
                ElseIf bElectronic Then
                    Me.lblTransmissionType.Text = "Transmission Type: Electronic"
                Else
                    Me.lblTransmissionType.Text = "Transmission Type: Manual"
                End If

                m_bDataChanged = True
            End If

        Else
            If IsIntraStoreTransfer Then
                If MsgBox("This PO will be sent and closed.  Would you like to continue?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                    logger.Info("cmdSendOrder_Click " & " Send this order now? False ")
                    logger.Debug("cmdSendOrder_Click Exit")
                    Exit Sub
                End If
            Else
                If MsgBox("Send this order now?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                    logger.Info("cmdSendOrder_Click " & " Send this order now? False ")
                    logger.Debug("cmdSendOrder_Click Exit")
                    Exit Sub
                End If
            End If
        End If

        Me.Cursor = Cursors.WaitCursor

        If SaveData() Then
            'TFS 7548, Faisal Ahmed - Auto Close Intra-Store Transfers
            If IsIntraStoreTransfer Then
                AutoCloseIntraStoreTransfer()
                RefreshDataSource(m_lOrderHeader_ID)
            Else
                'Task 8316: If users are allowed to override default PO method 
                'and enter their own email and fax number
                'validate the email or fax number
                m_bOverrideDefaultPOTransmissionMethod = InstanceDataDAO.IsFlagActive("OverrideDefaultPOTransmissionMethod")

                If (m_bOverrideDefaultPOTransmissionMethod) Then
                    If bFax Then
                        'If fax selected and the fax number was changed by the user, validate new entry
                        sendAddresseeMessage = sendValidation.isFaxValid(sFaxNbr)

                    ElseIf bEmail Then
                        'If email selected and the email number was changed by the user, validate new entry
                        sendAddresseeMessage = sendValidation.isEmailValid(sEmailAddr)
                    End If
                Else
                    'Otherwise, go with old validation of the vendor configuration for
                    sendMessage = sendValidation.isVendorConfigComplete(m_lOrderHeader_ID)
                End If

                If (String.IsNullOrEmpty(sendMessage) And String.IsNullOrEmpty(sendAddresseeMessage)) Then
                    If (sendValidation.isElectronicTransfer(m_lOrderHeader_ID)) Then
                        If MsgBox("Please contact the vendor prior to sending order to validate the order format.  Continue to send the order?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                            logger.Info("cmdSendOrder_Click " & "Warning: please contact vendor prior to sending order to validate order format. Do you still want to send order? False")
                            logger.Debug("cmdSendOrder_Click Exit")
                            Exit Sub
                        End If
                    End If

                    If bFax And Len(sFaxNbr) > 0 Then
                        m_sTransmissionTarget = sFaxNbr
                    ElseIf bEmail And Len(sEmailAddr) > 0 Then
                        m_sTransmissionTarget = sEmailAddr
                    Else
                        m_sTransmissionTarget = String.Empty
                    End If

                    SQLExecute("EXEC UpdateOrderSend " & m_lOrderHeader_ID & ", " & bFax & ", " & bEmail & "," & bElectronic & ", " & CInt(m_bTransmissionMethodOverriden) & ",'" & m_sTransmissionTarget & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)

                    ' send now if email
                    If bEmail Then
                        Me.Cursor = Cursors.Default

                        Try
                            SendMailBO.EmailPO(m_lOrderHeader_ID)
                            MessageBox.Show("Your PO was sent successfully." & vbCrLf & vbCrLf &
                                            "If the delivery of the PO fails, you will receive a notification in your email inbox.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Catch ex As Exception
                            ' To avoid null-ref exception, we need to check InnerException.
                            Dim innerExcMsg As String = Nothing
                            If Not ex.InnerException Is Nothing Then
                                innerExcMsg = ex.InnerException.ToString
                            End If

                            MessageBox.Show("An error ocurred transmitting your PO: " &
                                            vbCrLf & vbCrLf &
                                            ex.Message & vbCrLf &
                                            innerExcMsg,
                                            Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End Try
                    ElseIf bElectronic Then
                        SendElectronicOrder()
                    End If

                    RefreshDataSource(m_lOrderHeader_ID)

                ElseIf Not (String.IsNullOrEmpty(sendAddresseeMessage)) Then
                    logger.Info("cmdSendOrder_Click " & sendAddresseeMessage)
                    MsgBox(sendAddresseeMessage, MsgBoxStyle.Critical, Me.Text)
                ElseIf Not (String.IsNullOrEmpty(sendMessage)) Then
                    logger.Info("cmdSendOrder_Click " & sendMessage)
                    MsgBox(sendMessage, MsgBoxStyle.Critical, Me.Text)
                End If
            End If
        End If

        If bOpenAndSend Then
            Me.Close()
        End If

        Me.Cursor = Cursors.Default

        logger.Debug("cmdSendOrder_Click Exit")
    End Sub

    Private Sub AutoCloseIntraStoreTransfer()
        logger.Debug("AutoCloseIntraStoreTransfer Entry")

        If OrderingDAO.AutoCloseIntraStoreTransfer(m_lOrderHeader_ID, giUserID) <> True Then
            MessageBox.Show("An error occurred while auto-closing this PO", "Auto-Close Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        logger.Debug("AutoCloseIntraStoreTransfer Exit")
    End Sub

    Private Sub cmdUnlock_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdUnlock.Click
        logger.Debug("cmdUnlock_Click Entry")

        If MsgBox("Really unlock this record?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Unlock PO") = MsgBoxResult.Yes Then
            ' This is the only place we should unconditionally call the unlock process.  All other cases should use the UnlockPO() method in this class.
            SQLExecute("EXEC UnlockOrderHeader " & m_lOrderHeader_ID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            RefreshDataSource(m_lOrderHeader_ID)
        End If

        logger.Debug("cmdUnlock_Click Exit")
    End Sub

    Function SaveData() As Boolean
        logger.Debug("SaveData Entry")

        Dim bFax As Boolean
        Dim bEmail As Boolean
        Dim bElectronic As Boolean
        Dim rs As DAO.Recordset = Nothing

        SaveData = True

        If m_lOrderHeader_ID = -1 Then Exit Function

        bFax = CBool(Me.lblTransmissionType.Text = "Transmission Type: Fax")
        bEmail = CBool(Me.lblTransmissionType.Text = "Transmission Type: Email")
        bElectronic = CBool(Me.lblTransmissionType.Text = "Transmission Type: Electronic")

        If m_bDataChanged Then
            VerifyPOLockedForCurrentUser()
            ' If we are here, the current user should have a lock on the PO.

            ' Check for an invalid Expected Date (cannot be a past date).
            If (DateDiff(DateInterval.Day, SystemDateTime, Me.dtpExpectedDate.Value) < 0) And Me.dtpExpectedDate.Enabled = True Then
                MsgBox("Expected Date cannot be in the past.", MsgBoxStyle.Information, Me.Text)
                SaveData = False
                Me.dtpExpectedDate.Focus()
                logger.Info("SaveData " & " Expected Date cannot be in the past(SaveData = False).")
                logger.Debug("SaveData Exit")
                SaveData = False
                Exit Function
            End If

            If Not txtField(iOrderHeaderOrgPO).ReadOnly And (Len(Trim(txtField(iOrderHeaderOrgPO).Text)) > 0) Then
                rs = SQLOpenRecordSet("TestOrgPO " & TextValue(txtField(iOrderHeaderOrgPO).Text) & ", " & m_lVendorID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                If rs.Fields(0).Value = 0 Then
                    MsgBox("Invalid Original PO Number", MsgBoxStyle.Critical, Me.Text)
                    rs.Close()
                    SaveData = False
                    logger.Info("SaveData " & " Invalid Original PO Number")
                    logger.Debug("SaveData Exit")
                    Exit Function
                End If
            End If
            If rs IsNot Nothing Then
                rs.Close()
                rs = Nothing
            End If

            If txtField(iOrderHeaderQuantityDiscount).Text = "" Then txtField(iOrderHeaderQuantityDiscount).Text = "0"
            m_OrderDiscountAmt = CDec(txtField(iOrderHeaderQuantityDiscount).Text)

            ' discount - Discount Type selection is required - TFS 2095
            If cmbField(iOrderHeaderDiscountType).Enabled AndAlso Val(txtField(iOrderHeaderQuantityDiscount).Text) <> 0 And cmbField(iOrderHeaderDiscountType).SelectedIndex = 0 Then
                FormValidator.SetError(cmbField(iOrderHeaderDiscountType), "A selection is required.  Please select a Discount Type.")
                SaveData = False
                logger.Info("SaveData " & "Discount Type not selected")
                logger.Debug("SaveData Exit")
                Exit Function
            Else
                FormValidator.SetError(cmbField(iOrderHeaderDiscountType), "")
            End If

            ' discount - Reason code selection is required - TFS 2095
            If ucCostAdjReasonCode.Enabled AndAlso Val(txtField(iOrderHeaderQuantityDiscount).Text) <> 0 And IsNothing(ucCostAdjReasonCode.Value) And cmbField(iOrderHeaderDiscountType).SelectedIndex <> 0 Then
                FormValidator.SetError(ucCostAdjReasonCode, "A selection is required.  Please select a Reason Code for the Discount.")
                SaveData = False
                logger.Info("SaveData " & " Reason Code not selected")
                logger.Debug("SaveData Exit")
                Exit Function
            Else
                FormValidator.SetError(ucCostAdjReasonCode, "")
            End If
        End If

        If cmbField(iOrderHeaderDiscountType).SelectedIndex = 0 Then
            ucCostAdjReasonCode.Value = -1
        End If

        '-- Everything is ok
        Dim lIgnoreErrNum(0) As Integer

        If m_bDataChanged Then
            lIgnoreErrNum(0) = 50002

            Try
                Dim Temperature As Integer? = Nothing
                Dim QuantityDiscount As Decimal
                Dim OriginalOrderHeader_ID As Integer
                Dim ReasonCode_ID As String

                ' I wanted to combine the following If block in a single IIf expression, but it kept calling the TextValue() method in the "false" part even when the expression was true.
                ' Then I found this at MSDN.  It might be helpful information:
                '
                ' Note: The expressions in the argument list can include function calls. As part of preparing the argument list for the call to IIf, the Visual 
                ' Basic compiler calls every function in every expression. This means that you cannot rely on a particular function not being called if the other argument is selected by Expression.

                If TextValue(txtField(iOrderHeaderTemperature).Text) = "NULL" Then
                    Temperature = Nothing
                Else
                    Temperature = CInt(TextValue(txtField(iOrderHeaderTemperature).Text))
                End If

                If Temperature.HasValue And displayTemperatureInCelsius Then
                    ' The display unit is Celsius, but we only store Fahrenheit in the database.  Convert the value before proceeding.
                    Temperature = ConvertTemperature(Temperature, TemperatureUnit.Fahrenheit)
                End If

                QuantityDiscount = IIf(TextValue(txtField(iOrderHeaderQuantityDiscount).Text) = "NULL", Nothing, TextValue(txtField(iOrderHeaderQuantityDiscount).Text))
                OriginalOrderHeader_ID = IIf(TextValue(txtField(iOrderHeaderOrgPO).Text) = "NULL", Nothing, TextValue(txtField(iOrderHeaderOrgPO).Text))
                ReasonCode_ID = IIf(IsNothing(ucCostAdjReasonCode.Value), "NULL", ucCostAdjReasonCode.Value)

                OrderingDAO.UpdateOrderInfo(m_lOrderHeader_ID,
                                            Temperature,
                                            QuantityDiscount,
                                            Me.dtpExpectedDate.Value,
                                            ComboValue(cmbField(iOrderHeaderDiscountType)),
                                            ReasonCode_ID,
                                            ComboValue(cmbField(iOrderHeaderPurchaseLocation_ID)),
                                            ComboValue(cmbField(iOrderHeaderReceiveLocation_ID)),
                                            IIf(bFax = True, 1, 0),
                                            IIf(bEmail = True, 1, 0),
                                            IIf(bElectronic = True, 1, 0),
                                            chkField(iOrderHeaderReturn_Order).CheckState,
                                            OriginalOrderHeader_ID,
                                            giUserID,
                                            IIf(CheckBox_DropShip.Checked, 1, 0))

            Catch ex As Exception
                logger.Error("SaveData " & "Error in EXEC UpdateOrderInfo " & Err.Description)
                MsgBox(Err.Description & vbLf & Err.Source & vbLf & Err.Number, MsgBoxStyle.Critical, Me.Text)
                SaveData = False
            End Try

            'Populate the OrderedCost for the newly created PO
            SQLExecute(String.Format("UpdateOrderHeaderCosts {0}", m_lOrderHeader_ID), DAO.RecordsetOptionEnum.dbSQLPassThrough)

        ElseIf Not lblReadOnly.Visible Then
            UnlockPO()
        End If

        logger.Debug("SaveData Exit")
    End Function

    Private Sub cmdWarehouseSend_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdWarehouseSend.Click
        logger.Debug("cmdWarehouseSend_Click Entry")

        If m_bMultipleSubTeams Then
            MsgBox("There are items for multiple subteams on this order." & vbCrLf & "The order cannot be sent unless all items are for the same subteam.", MsgBoxStyle.Information, Me.Text)
            logger.Info("cmdWarehouseSend_Click " & "There are items for multiple sub-teams on this order." & vbCrLf & "Send not allowed unless all items are for the same sub-team." & vbCrLf & "Delete items that are not for the order sub-team.")
            logger.Debug("cmdWarehouseSend_Click Exit")
            Exit Sub
        End If

        If (DateDiff(DateInterval.Day, SystemDateTime(True), dtpExpectedDate.Value) < 0) Then
            MsgBox("Expected Date cannot be in the past.", MsgBoxStyle.Information, Me.Text)
            dtpExpectedDate.Focus()
            logger.Info("cmdSendOrder_Click Expected Date cannot be in the past.")
            logger.Debug("cmdSendOrder_Click Exit")
            Exit Sub
        End If

        If MsgBox("Send this order to the warehouse now (it will be locked in IRMA)?", MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then Exit Sub

        If SaveData() Then
            SQLExecute("UpdateOrderWarehouseSend " & m_lOrderHeader_ID, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            'Unlock because no longer needs to be locked for current user - locked for all
            UnlockPO()
            RefreshDataSource(m_lOrderHeader_ID)
        End If

        logger.Debug("cmdWarehouseSend_Click Exit")
    End Sub

    Private Sub frmOrders_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated
        logger.Debug("frmOrders_Activated Entry")

        If cmbField(iOrderHeaderPurchaseLocation_ID).Items.Count = 0 Then
            MsgBox("Orders cannot be entered.  There are no vendors available for internal customers.", MsgBoxStyle.Information, Me.Text)
            Me.Close()
            logger.Info("frmOrders_Activated " & " Orders cannot be entered - no vendors marked for internal customers.")
            logger.Debug("frmOrders_Activated Exit")
            Exit Sub
        End If

        ' When there are no orders, the event causes an endless loop.
        RemoveHandler Me.Activated, AddressOf frmOrders_Activated

        ' Make sure there is data.
        CheckNoOrders()

        logger.Debug("frmOrders_Activated Exit")
    End Sub

    Private Sub frmOrders_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles MyBase.KeyPress
        logger.Debug("frmOrders_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = ASCII_ENTER Then
            KeyAscii = ASCII_NULL
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = ASCII_NULL Then
            eventArgs.Handled = True
        End If

        logger.Debug("frmOrders_KeyPress Exit")
    End Sub

    Private Sub frmOrders_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmOrders_Load Entry")

        CenterForm(Me)

        LoadInternalCustomer(cmbField(iOrderHeaderPurchaseLocation_ID))
        LoadDiscount(cmbField(iOrderHeaderDiscountType), False)
        LoadReasonCodesUltraCombo(ucCostAdjReasonCode, enumReasonCodeType.CA)

        sDiscountType(0) = "No Discount"
        sDiscountType(1) = "Cash Discount"
        sDiscountType(2) = "Percent Discount"
        sDiscountType(3) = "Free Items"
        cmbField(iOrderHeaderDiscountType).Items.RemoveAt(3)
        cmbField(iOrderHeaderDiscountType).Items.RemoveAt(1)
        cmbField(iOrderHeaderDiscountType).Enabled = True
        txtField(iOrderHeaderQuantityDiscount).Enabled = True
        ucCostAdjReasonCode.Enabled = True

        cmdCopyPO.Enabled = Not (gbAccountant Or gbPOAccountant Or gbDistributor)

        ' For the UK region, display the temperature in degrees Celsius.  I couldn't find a good way to do this using culture information, so I'm 
        ' just going to check for the UK culture specifically.  It would be better to have a more 'globalized' solution, but Temperature is not part
        ' of the standard array of Windows regional settings.
        If gsUG_Culture = "en-GB" Then
            _lblLabel_13.Text = "°C"
            displayTemperatureInCelsius = True
        End If

        SetupDataTable()

        '-- Get the current record
        If bSpecificOrder Then
            RefreshDataSource(glOrderHeaderID)

            ' this is for orders being created from the handheld ItemQueue.
            ' when orders are created there, user is given the option of "sending"
            ' if the option is chosen, this form is opened from that screen with the OpenAndSend property here = True
            If bOpenAndSend Then
                cmdSendOrder.PerformClick()
            ElseIf JumpToOrderStatus Then
                Me.Show()
                cmdCloseOrder.PerformClick()
            End If

        ElseIf Me.AutoSearch_OrderHeaderId_OneKnownMatch IsNot Nothing Then
            RefreshDataSource(AutoSearch_OrderHeaderId_OneKnownMatch.Value)
            Me.AutoSearch_OrderHeaderId_OneKnownMatch = Nothing
        ElseIf Me.AutoSearch_OrderHeaderId IsNot Nothing Then
            'm_bItemsReceived has shared scope and (for now) must be reset before running a quicksearch
            m_bItemsReceived = False
            cmdSearch.PerformClick()
            Me.AutoSearch_OrderHeaderId = Nothing
        Else
            RefreshDataSource(-1)
        End If

        Global.SetUltraGridSelectionStyle(ugrdItems)
        logger.Debug("frmOrders_Load Exit")
    End Sub

    Private Sub CheckNoOrders()
        logger.Debug("CheckNoOrders Entry")

        '-- Make sure there is data for them to be in this form
        If m_lOrderHeader_ID = -1 Then
            If MsgBox("No orders found in the database." & vbCrLf & "Would you like to add one?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "New Order") = MsgBoxResult.Yes Then
                cmdAddOrder_Click(cmdAddOrder, New System.EventArgs())
                If m_lOrderHeader_ID = -1 Then
                    Me.Close()
                End If
            Else
                Me.Close()
            End If
        End If

        logger.Debug("CheckNoOrders Exit")
    End Sub

    Private Sub Timer1_Tick(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles Timer1.Tick
        logger.Debug("Timer1_Tick Entry")

        Static siLoop As Short
        Dim lDiff As Integer
        Dim dStartTime As Date

        If (m_dtOrderEnd > System.DateTime.FromOADate(0)) Then

            m_dtCurrSysTime = DateAdd(DateInterval.Minute, 1, m_dtCurrSysTime)

            lDiff = DateDiff(DateInterval.Minute, m_dtCurrSysTime, m_dtOrderEnd)
            If lDiff > 0 Then
                Me.sbMsg.Items.Item(0).Text = "Ordering time remaining: " & lDiff & " min."
                If (lDiff <= 15) Then
                    'Display message box every 5 minutes up until window is closed
                    siLoop = siLoop + 1
                    If (siLoop Mod 4 = 0) Or (siLoop = 1) Then
                        dStartTime = TimeOfDay 'Record start time so we can add time message box was displayed
                        Timer1.Enabled = False
                        MsgBox("Ordering window is about to close", MsgBoxStyle.Exclamation, Me.Text)
                        m_dtCurrSysTime = DateAdd(DateInterval.Second, DateDiff(DateInterval.Second, dStartTime, TimeOfDay), m_dtCurrSysTime)

                        lDiff = DateDiff(DateInterval.Minute, m_dtCurrSysTime, m_dtOrderEnd)
                        If lDiff > 0 Then
                            Me.sbMsg.Items.Item(0).Text = "Ordering time remaining: " & lDiff & " min."
                            Timer1.Enabled = True
                        Else
                            Me.sbMsg.Items.Item(0).Text = "Ordering window is CLOSED."
                            siLoop = 0
                        End If
                    End If
                End If
            Else
                Me.sbMsg.Items.Item(0).Text = "Ordering window is CLOSED."
                Timer1.Enabled = False
                siLoop = 0
            End If
        Else
            Me.sbMsg.Items.Item(0).Text = ""
            siLoop = 0
            Timer1.Enabled = False
        End If

        logger.Debug("Timer1_Tick Exit")
    End Sub

    Private Sub txtField_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.TextChanged
        logger.Debug("=> txtField_TextChanged")

        If Me.IsInitializing Then Exit Sub

        Dim Index As Short = txtField.GetIndex(eventSender)

        If Not m_bLoading Then
            If Not txtField(Index).ReadOnly Then m_bDataChanged = True

            Select Case Index
                Case iOrderHeaderQuantityDiscount
                    m_DiscountAmtChanged = True
            End Select

        End If

        logger.Debug("<= txtField_TextChanged ")
    End Sub

    Private Sub ucCostAdjReasonCode_RowSelected(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles ucCostAdjReasonCode.RowSelected
        logger.Debug("=> ucCostAdjReasonCode_RowSelected")

        If Me.IsInitializing Then Exit Sub

        If Not m_bLoading Then
            If Not ucCostAdjReasonCode.ReadOnly And Val(txtField(iOrderHeaderQuantityDiscount).Text) <> 0 And cmbField(iOrderHeaderDiscountType).SelectedIndex <> 0 Then
                m_bDataChanged = True
                SaveData()
                RefreshDataSource(glOrderHeaderID)
            End If
        End If

        logger.Debug("<= ucCostAdjReasonCode_RowSelected")
    End Sub

    Private Sub txtField_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.Enter
        CType(eventSender, TextBox).SelectAll()
    End Sub

    Private Sub txtField_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtField.Leave
        logger.Debug("=> txtField_Leave")

        If m_DiscountAmtChanged And m_OrderDiscountType <> 0 Then
            txtField(iOrderHeaderQuantityDiscount).Text = txtField(iOrderHeaderQuantityDiscount).Text.Trim()
            If txtField(iOrderHeaderQuantityDiscount).Text.Length <> 0 Then
                m_OrderDiscountAmt = CDec(txtField(iOrderHeaderQuantityDiscount).Text)
            End If
            SaveData()
            UpdateOrderItemsCost()

            DistributeFreight(CInt(txtField(iOrderHeaderOrderHeader_ID).Text),
                                    CDec(txtField(iOrderHeader3rdPartyFreight).Text))

            RefreshOrderDetails(CInt(txtField(iOrderHeaderOrderHeader_ID).Text), True)
            m_DiscountAmtChanged = False
        End If

        logger.Debug("<= txtField_Leave ")
    End Sub

    Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtField.KeyPress
        logger.Debug("txtField_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        Dim Index As Short = txtField.GetIndex(eventSender)

        If Not txtField(Index).ReadOnly Then
            KeyAscii = ValidateKeyPressEvent(KeyAscii, txtField(Index).Tag, txtField(Index), 0, 0, 0)

            If Index = iOrderHeaderInvoiceNumber Then
                If KeyAscii >= ASCII_LOWERCASE_A And KeyAscii <= ASCII_LOWERCASE_Z Then
                    KeyAscii = KeyAscii - ASCII_LOWERCASE_A + ASCII_UPPERCASE_A
                End If
            End If
        End If

        eventArgs.KeyChar = Chr(KeyAscii)

        If KeyAscii = ASCII_NULL Then
            eventArgs.Handled = True
        End If

        logger.Debug("txtField_KeyPress Exit")
    End Sub

    Private Sub RefreshOrderDetails(ByRef lOrderNumber As Integer, ByRef bUpdateGrid As Boolean)
        logger.Debug("RefreshOrderDetails Entry")

        Dim rsTemp As DAO.Recordset = Nothing
        Dim isDeleted As Boolean = False

        'Populate the OrderedCost for the newly created PO
        SQLExecute(String.Format("UpdateOrderHeaderCosts {0}", lOrderNumber), DAO.RecordsetOptionEnum.dbSQLPassThrough)

        Try
            rsTemp = SQLOpenRecordSet("EXEC GetOrderInfo " & lOrderNumber & ", 0", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            If Not rsTemp.EOF Then
                Me.OrderedCostTextBox.Text = VB6.Format((rsTemp.Fields("OrderedCost").Value), "##,###0.00##") 'Discounts already accounted for
                Me.OriginalReceivedCostTextBox.Text = VB6.Format((rsTemp.Fields("OriginalReceivedCost").Value), "##,###0.00##")
                Me.AdjustedReceivedCostTextBox.Text = VB6.Format((rsTemp.Fields("AdjustedReceivedCost").Value), "##,###0.00##")
                Me.UploadedCostTextBox.Text = VB6.Format((rsTemp.Fields("UploadedCost").Value), "##,###0.00")
                txtField(iOrderHeaderTotalFreight).Text = VB6.Format(CDec(rsTemp.Fields("LineItemFreight").Value), "##,###0.00")
                isDeleted = Convert.ToBoolean(rsTemp.Fields("IsDeleted").Value)
            End If
        Finally
            If rsTemp IsNot Nothing Then
                rsTemp.Close()
                rsTemp = Nothing
            End If
        End Try

        Try
            If bUpdateGrid Then
                If Not (m_rsOrderItems Is Nothing) Then m_rsOrderItems.Close()

                If isDeleted Then
                    SQLOpenRS(m_rsOrderItems, "EXEC GetDeletedOrderItemList " & lOrderNumber & ", 0", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                Else
                    SQLOpenRS(m_rsOrderItems, "EXEC GetOrderItemList " & lOrderNumber & ", 0", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                End If

                PopGrid(True, Not isDeleted)
            End If
        Finally
            If m_rsOrderItems IsNot Nothing Then
                m_rsOrderItems.Close()
                m_rsOrderItems = Nothing
            End If
        End Try

        logger.Debug("RefreshOrderDetails Exit")
    End Sub

    Private Sub PopGrid(ByRef bUpdateGrid As Boolean, ByVal IsEnabled As Boolean)

        logger.Debug("PopGrid Entry")
        Dim iLoop As Short
        Dim sGridRowText As String
        Dim dMarkup As Decimal
        sGridRowText = String.Empty

        Dim row As DataRow

        mdt.Clear()

        m_bItemsReceived = False 'Set this again in this routine because it is called separately after the initial call during loading
        m_bItemsOrdered = False
        m_bMultipleSubTeams = False
        m_bSeafoodMissingCountryInfo = False
        m_bPre_Order = False
        m_bIsEXEDistributed = False
        m_IsEinvoice = IsEInvoice()

        If bUpdateGrid Then Erase m_iInsertOrder


        While Not m_rsOrderItems.EOF
            iLoop = iLoop + 1

            m_bItemsOrdered = True

            'If the grid's data was refreshed, then it is the original sort order - capture the order row numbers
            If bUpdateGrid Then
                If iLoop = 1 Then m_lStartOrderItem_ID = m_rsOrderItems.Fields("OrderItem_ID").Value
                ReDim Preserve m_iInsertOrder(m_rsOrderItems.Fields("OrderItem_ID").Value - m_lStartOrderItem_ID)
                m_iInsertOrder(UBound(m_iInsertOrder)) = iLoop
            End If

            row = mdt.NewRow
            row("Item_Key") = m_rsOrderItems.Fields("Item_Key").Value
            row("OrderItem_ID") = m_rsOrderItems.Fields("OrderItem_ID").Value
            row("Line") = IIf(bUpdateGrid, iLoop, m_iInsertOrder(m_rsOrderItems.Fields("OrderItem_ID").Value - m_lStartOrderItem_ID))
            row("Identifier") = m_rsOrderItems.Fields("Identifier").Value
            row("Brand_Name") = m_rsOrderItems.Fields("Brand_Name").Value
            row("Item_Description") = m_rsOrderItems.Fields("Item_Description").Value
            row("vendor_Item_Description") = m_rsOrderItems.Fields("VendorItemDescription").Value & ""

            row("QuantityOrdered") = Trim(Str(CDec(CDec(m_rsOrderItems.Fields("QuantityOrdered").Value) + CDec(IIf(m_rsOrderItems.Fields("DiscountType").Value = 3, 0, 0)).ToString("##0.#")))) & " " & Trim(m_rsOrderItems.Fields("Unit_Name").Value)
            row("QuantityReceived") = m_rsOrderItems.Fields("QuantityReceived").Value

            If m_rsOrderItems.Fields("QuantityOrdered").Value <> 0 Then
                ' TFS# 7696 = show the actual cost - markup
                dMarkup = (m_rsOrderItems.Fields("ActualCost").Value - (m_rsOrderItems.Fields("LineItemCost").Value / m_rsOrderItems.Fields("QuantityOrdered").Value))

                ' TFS# 9813 = include any handling charges from the DC.
                If geOrderType = enumOrderType.Distribution Or geOrderType = enumOrderType.Flowthru Then
                    row("ActualCost") = (m_rsOrderItems.Fields("ActualCost").Value)
                Else
                    row("ActualCost") = (m_rsOrderItems.Fields("ActualCost").Value - dMarkup)
                End If
            Else
                row("ActualCost") = 0
            End If

            row("Reg Cost") = String.Format("{0:######0.00##}", m_rsOrderItems.Fields("CurrentVendorCost").Value)
            row("eInvoice") = m_rsOrderItems.Fields("eInvoiceQuantity").Value
            mdt.Rows.Add(row)

            If m_rsOrderItems.Fields("quantityreceived").Value IsNot DBNull.Value Then

                If (m_rsOrderItems.Fields("QuantityReceived").Value > 0) Or (m_rsOrderItems.Fields("Total_Weight").Value > 0) Then m_bItemsReceived = True
            End If

            If m_rsOrderItems.Fields("SeafoodMissingCountryInfo").Value Then m_bSeafoodMissingCountryInfo = True

            If m_rsOrderItems.Fields("Pre_Order").Value Then m_bPre_Order = True

            If m_rsOrderItems.Fields("EXEDistributed").Value Then m_bIsEXEDistributed = True

            m_rsOrderItems.MoveNext()
        End While

        mdt.AcceptChanges()
        ugrdItems.DataSource = mdt

        ' TFS 11577 - Don't disable the grid completely if PO is deleted. Instead, allow the grid to be 'read-only' without allowing updates
        If IsEnabled Then
            ugrdItems.Enabled = IsEnabled
        Else
            ugrdItems.DisplayLayout.Override.AllowUpdate = DefaultableBoolean.False
        End If

        With ugrdItems.DisplayLayout.Bands(0)

            .Columns("Vendor_Item_Description").Hidden = Not chkShowVendorDescription.Checked
            .Columns("Vendor_Item_Description").Width = .Columns("Item_Description").Width
            .Columns("Item_Description").Hidden = chkShowVendorDescription.Checked

            'TFS 11989, Faisal Ahmed, 04/30/2013 - Hide the 'Reg Cost' column for transfers and distribution orders
            .Columns("Reg Cost").Hidden = False
            If geOrderType = enumOrderType.Transfer Or geOrderType = enumOrderType.Distribution Then
                .Columns("Reg Cost").Hidden = True
            End If

            .SortedColumns.Add("Line", False, False)
            .Columns("QuantityOrdered").SortComparer = New srtComparer
        End With

        logger.Debug("PopGrid Exit")

    End Sub

    Private Sub RefreshDataSource(ByRef lRecord As Integer)
        logger.Debug("RefreshDataSource Entry")

        Dim bBuyerDistributor As Boolean
        Dim dStartTime As Date
        Dim sProductType As String
        Dim lReceiveID As Long
        Dim iDiscountType As Integer
        Dim blnIsMyStore As Boolean
        Dim UserDAO As New UserDAO
        Dim IsDeleted As Boolean

        Timer1.Enabled = False
        m_dtOrderEnd = System.DateTime.FromOADate(0)
        m_bLoading = True
        bBuyerDistributor = gbBuyer Or gbDistributor
        sProductType = String.Empty
        dStartTime = SystemDateTime()
        iDefaultPOTransmission = ConfigurationServices.AppSettings("DefaultPOTransmission")

        Try
            If lRecord = -1 Then
                m_rsOrder = SQLOpenRecordSet("GetOrderInfo " & " 0," & giUserID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Else
                m_rsOrder = SQLOpenRecordSet("GetOrderInfo " & lRecord & ", 0", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            End If

            If m_rsOrder.EOF Then
                m_lOrderHeader_ID = -1
                If (gbQuickSearch = True) Then
                    MsgBox("No items found.", MsgBoxStyle.Information, Me.Text)
                    Me.Close()
                End If
            Else

                ' If user has a store limit and is viewing a PO from another store, then do not lock the PO since it's read only anyway.
                m_lStoreNo = IIf(IsDBNull(m_rsOrder.Fields("Store_No").Value), -1, m_rsOrder.Fields("Store_No").Value)

                If Not ((Not gbCoordinator) And (glStore_Limit > 0 And glStore_Limit <> m_lStoreNo)) Then
                    Try
                        ' Passing the user ID we just pulled from the PO record will save a trip to DB here, so pass it in.
                        OrderValidationBO.Lock(m_rsOrder.Fields("OrderHeader_ID").Value, giUserID, m_rsOrder.Fields("User_ID").Value)
                    Catch ex As Exception
                        ' If this PO is locked by another user, the lock attempt results in an exception, which we can discard.  It's okay if another user has PO locked
                        ' because we are just trying to open the PO and the default action is to try to lock it.  The read-only piece is handled later in this method if another user already has PO locked.
                    End Try
                End If

                If Not m_rsOrder.Fields("WarehouseCancelled").Value Is System.DBNull.Value Then
                    LabelCancelled.Text = String.Format("This order was cancelled in EXE on {0}", m_rsOrder.Fields("WarehouseCancelled").Value)
                Else
                    LabelCancelled.Text = String.Empty
                End If

                'Setting up contols for deleted PO
                IsDeleted = Convert.ToBoolean(m_rsOrder.Fields("IsDeleted").Value)
                Me.IsDeleted = IsDeleted

                If IsDeleted Then
                    txtDeletedDate.Text = CDate(m_rsOrder.Fields("DeletedDate").Value)
                    txtDeletedby.Text = m_rsOrder.Fields("UserName").Value.ToString()
                    txtDeletedReason.Text = m_rsOrder.Fields("ReasonCodeDesc").Value.ToString()
                End If

                ' Assign the field values to the variables
                m_3rdParty = m_rsOrder.Fields("Freight3Party_OrderCost").Value
                geOrderType = m_rsOrder.Fields("OrderType_ID").Value
                geProductType = m_rsOrder.Fields("ProductType_ID").Value
                If CStr(m_rsOrder.Fields("OrderEnd").Value) <> "0" Then
                    m_dtOrderEnd = m_rsOrder.Fields("OrderEnd").Value
                    m_dtCurrSysTime = m_rsOrder.Fields("CurrSysTime").Value
                End If
                m_lCreatedBy = m_rsOrder.Fields("CreatedBy").Value
                m_lOrderHeader_ID = m_rsOrder.Fields("OrderHeader_ID").Value
                glOrderHeaderID = m_rsOrder.Fields("OrderHeader_ID").Value
                m_bSent = m_rsOrder.Fields("Sent").Value
                m_bOrderClosed = Not IsDBNull(m_rsOrder.Fields("CloseDate").Value)
                m_bOrderApproved = Not IsDBNull(m_rsOrder.Fields("ApprovedDate").Value)
                m_bOrderUploaded = Not IsDBNull(m_rsOrder.Fields("UploadedDate").Value)
                m_bOrderAccountingUploaded = Not IsDBNull(m_rsOrder.Fields("AccountingUploadDate").Value)
                m_lReturnOrder_ID = IIf(IsDBNull(m_rsOrder.Fields("ReturnOrder_ID").Value), -1, m_rsOrder.Fields("ReturnOrder_ID").Value)
                m_lOriginalOrder_ID = IIf(IsDBNull(m_rsOrder.Fields("OriginalOrder_ID").Value), -1, m_rsOrder.Fields("OriginalOrder_ID").Value)
                m_lVendorID = m_rsOrder.Fields("Vendor_ID").Value
                m_lVendorStoreNo = m_rsOrder.Fields("Vendor_Store_No").Value
                m_lReceiveLocationID = m_rsOrder.Fields("ReceiveLocation_ID").Value
                m_bIsStore_Vend = m_rsOrder.Fields("Store_Vend").Value
                m_bFrom_SubTeam_Unrestricted = m_rsOrder.Fields("From_SubTeam_Unrestricted").Value
                m_bTo_SubTeam_Unrestricted = m_rsOrder.Fields("To_SubTeam_Unrestricted").Value
                m_ReceiveLocationIsDistribution = DirectCast(m_rsOrder.Fields("ReceivingStore_Distribution_Center").Value, Boolean)
                m_IsVendorExternal = m_rsOrder.Fields("IsVendorExternal").Value
                m_bPayAgreedCost = CBool(m_rsOrder.Fields("PayByAgreedCost").Value)
                m_bAllowPOEdit = True
                m_bAllowReceiveAll = m_rsOrder.Fields("AllowReceiveAll").Value
                glAllowBarcodePOReport = m_rsOrder.Fields("AllowBarcodePOReport").Value
                'TFS 7548, Faisal Ahmed, Feature: Auto Close Intra Store Transfer
                If m_lVendorID = m_lReceiveLocationID Then
                    m_bIsIntraStoreTransfer = True
                Else
                    m_bIsIntraStoreTransfer = False
                End If

                m_bSOGOrder = CBool(m_rsOrder.Fields("IsSOGOrder").Value)
                If OrderingDAO.IsOrderEinvoice(m_lOrderHeader_ID, m_iEInvoiceId) Then
                    cmdDisplayEInvoice.Enabled = True
                Else
                    cmdDisplayEInvoice.Enabled = False
                End If
                If m_rsOrder.Fields("Manufacturer").Value Then
                    m_iVendorType = enumVendorType.RegionalFacilityManufacturer
                ElseIf m_rsOrder.Fields("Distribution_Center").Value Then
                    m_iVendorType = enumVendorType.RegionalFacilityDistCenter
                ElseIf m_rsOrder.Fields("WFM_Store").Value Then
                    m_iVendorType = enumVendorType.RegionalRetailWFM
                ElseIf m_rsOrder.Fields("HFM_Store").Value Then
                    m_iVendorType = enumVendorType.RegionalRetailMega
                ElseIf m_rsOrder.Fields("WFM").Value Then
                    m_iVendorType = enumVendorType.OutSideRegionalWFM
                Else
                    m_iVendorType = -1
                End If
                If IsDBNull(m_rsOrder.Fields("WFM_Store").Value) Then
                    m_bIsOrderWFM = False
                Else
                    If m_rsOrder.Fields("WFM_Store").Value = False Then
                        m_bIsOrderWFM = False
                    Else
                        m_bIsOrderWFM = True
                    End If
                End If
                If IsDBNull(m_rsOrder.Fields("HFM_Store").Value) Then
                    m_bIsOrderHFM = False
                Else
                    If m_rsOrder.Fields("HFM_Store").Value = False Then
                        m_bIsOrderHFM = False
                    Else
                        m_bIsOrderHFM = True
                    End If
                End If
                If (Not IsDBNull(m_rsOrder.Fields("OriginalCloseDate").Value)) Then
                    m_dtOriginalCloseDate = CDate(m_rsOrder.Fields("OriginalCloseDate").Value)
                End If
                m_bItemsReceived = m_rsOrder.Fields("ItemsReceived").Value > 0
                If IsDBNull(m_rsOrder.Fields("Transfer_SubTeam").Value) Then
                    m_lTransferFromSubTeam = -1
                    gbExcludeNot_Available = False
                Else
                    ' Always show NA items for distribution orders.  The validation logic in the OrderList screen will
                    ' determine if the NA item can or cannot be added to the order.
                    m_lTransferFromSubTeam = m_rsOrder.Fields("Transfer_SubTeam").Value
                    gbExcludeNot_Available = False
                End If
                m_lTransferToSubTeam = m_rsOrder.Fields("Transfer_To_SubTeam").Value
                m_bEXEWarehouse = (Not IsDBNull(m_rsOrder.Fields("EXEWarehouse").Value))
                m_bEXEWarehouseDistOrder = (Not IsDBNull(m_rsOrder.Fields("EXEWarehouse").Value)) And (m_lTransferFromSubTeam > -1)
                m_bEXEWarehousePurchaseOrder = (Not IsDBNull(m_rsOrder.Fields("EXEWarehouse").Value)) And (m_lTransferFromSubTeam = -1)
                m_bIsEXEDistributed = (m_rsOrder.Fields("IsEXEDistributed").Value > 0)

                'If this is an order newly loaded, initialize the value of the controls.
                If txtField(iOrderHeaderOrderHeader_ID).Text <> CStr(m_lOrderHeader_ID) Then
                    lblCreditReasonCode.Text = ""
                    chkApplyAllCreditReason.Checked = False
                End If

                txtField(iOrderHeaderOrderHeader_ID).Text = CStr(m_lOrderHeader_ID)
                txtField(iOrderHeaderInvoiceNumber).Text = m_rsOrder.Fields("InvoiceNumber").Value & ""
                txtField(iOrderHeaderCreatedBy).Text = m_rsOrder.Fields("CreatedByName").Value & ""
                txtClosedBy.Text = m_rsOrder.Fields("ClosedByUserName").Value
                txtApprover.Text = m_rsOrder.Fields("ApprovedByUserName").Value
                txtApproved.Text = VB6.Format(m_rsOrder.Fields("ApprovedDate").Value, CStr(gsUG_DateMask) & " hh:mm AM/PM")
                txtField(iOrderHeaderCompanyName).Text = m_rsOrder.Fields("CompanyName").Value & ""
                m_sVendorName = m_rsOrder.Fields("CompanyName").Value & ""
                txtField(iOrderHeaderOrderDate).Text = VB6.Format(m_rsOrder.Fields("OrderDate").Value, CStr(gsUG_DateMask))
                Me.dtpExpectedDate.Value = m_rsOrder.Fields("Expected_Date").Value
                txtField(iOrderHeaderSentDate).Text = VB6.Format(m_rsOrder.Fields("SentDate").Value, CStr(gsUG_DateMask) & " hh:mm AM/PM")
                txtPOCostDate.Text = VB6.Format(m_rsOrder.Fields("POCostDate").Value, CStr(gsUG_DateMask))
                txtField(iOrderHeaderCloseDate).Text = VB6.Format(m_rsOrder.Fields("CloseDate").Value, CStr(gsUG_DateMask))
                If m_lOriginalOrder_ID <> -1 Then
                    txtField(iOrderHeaderOrgPO).Text = CStr(m_lOriginalOrder_ID)
                Else
                    txtField(iOrderHeaderOrgPO).Text = ""
                End If

                ' Show Invoice Amount if eInvoice exists
                If m_bOrderClosed Or (Not (m_bOrderClosed) And m_iEInvoiceId > 0) Then
                    txtField(15).Text = String.Format("{0:##,##0.00##}", m_rsOrder.Fields("InvoiceAmount").Value)
                Else
                    txtField(15).Text = String.Format("{0:##,##0.00##}", 0)
                End If
                txtField(iOrderHeaderRecvLog_No).Text = m_rsOrder.Fields("RecvLog_No").Value & ""
                m_lPurchaseLocationID = m_rsOrder.Fields("PurchaseLocation_ID").Value

                ' Load combo boxes
                LoadInternalCustomer(cmbField(iOrderHeaderPurchaseLocation_ID), m_lPurchaseLocationID)
                lReceiveID = m_rsOrder.Fields("ReceiveLocation_ID").Value
                LoadInternalCustomer(cmbField(iOrderHeaderReceiveLocation_ID), lReceiveID)
                SetCombo(cmbField(iOrderHeaderPurchaseLocation_ID), m_lPurchaseLocationID)
                SetCombo(cmbField(iOrderHeaderReceiveLocation_ID), lReceiveID)

                ' Load text fields
                txtField(iOrderHeader3rdPartyFreight).Text = m_rsOrder.Fields("Freight3Party_OrderCost").Value
                txtField(iOrderHeaderQuantityDiscount).Text = m_rsOrder.Fields("QuantityDiscount").Value
                m_OrderDiscountAmt = CDec(txtField(iOrderHeaderQuantityDiscount).Text)
                iDiscountType = m_rsOrder.Fields("DiscountType").Value
                m_OrderDiscountType = iDiscountType
                m_OrderDiscountExists = IIf(m_OrderDiscountType = 0, False, True)
                SetCombo(cmbField(iOrderHeaderDiscountType), iDiscountType)

                If iDiscountType = 0 Then
                    ucCostAdjReasonCode.Value = -1
                Else
                    ucCostAdjReasonCode.Value = m_rsOrder.Fields("ReasonCodeDetailID").Value
                End If

                If IsDBNull(m_rsOrder.Fields("Temperature").Value) Then
                    txtField(iOrderHeaderTemperature).Text = String.Empty
                Else
                    If displayTemperatureInCelsius Then
                        ' The temperature value is stored and retrived as degrees Fahrenheit, but the temperature display unit is Celsius.  Convert the value to degrees Celsius.
                        txtField(iOrderHeaderTemperature).Text = ConvertTemperature(m_rsOrder.Fields("Temperature").Value, TemperatureUnit.Celsius) & ""
                    Else
                        txtField(iOrderHeaderTemperature).Text = m_rsOrder.Fields("Temperature").Value & ""
                    End If
                End If

                chkField(iOrderHeaderReturn_Order).CheckState = IIf(m_rsOrder.Fields("Return_Order").Value = 0, 0, 1)

                ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
                'txtRefusedTotal.Text = String.Format("{0:##,##0.00##}", m_rsOrder.Fields("TotalRefused").Value)

                ' Task 8316 - If order was already sent, display the PO Transmission method with which it was sent
                If m_bSent Then
                    If (m_rsOrder.Fields("Fax_Order").Value = True) Then
                        Me.lblTransmissionType.Text = "Transmission Type: Fax"
                        m_iTransmissionMethod = 1
                    ElseIf (m_rsOrder.Fields("Email_Order").Value = True) Then
                        Me.lblTransmissionType.Text = "Transmission Type: Email"
                        m_iTransmissionMethod = 2
                    ElseIf (m_rsOrder.Fields("Electronic_Order").Value = True) Then
                        Me.lblTransmissionType.Text = "Transmission Type: Electronic"
                        m_iTransmissionMethod = 4
                    Else
                        Me.lblTransmissionType.Text = "Transmission Type: Manual"
                        m_iTransmissionMethod = 3
                    End If
                ElseIf Not IsDBNull(m_rsOrder.Fields("POTransmissionTypeID").Value) Then

                    ' Task 8316 - If order is not yet sent, display the default PO transmission method for the vendor
                    m_iTransmissionMethod = (m_rsOrder.Fields("POTransmissionTypeID")).Value

                    Select Case m_iTransmissionMethod
                        Case 1 'Fax
                            Me.lblTransmissionType.Text = "Transmission Type: Fax"
                        Case 2 'Email
                            Me.lblTransmissionType.Text = "Transmission Type: Email"
                        Case 4
                            Me.lblTransmissionType.Text = "Transmission Type: Electronic"
                        Case Else
                            Me.lblTransmissionType.Text = "Transmission Type: Manual"
                    End Select

                    ' If Vendor PO Transmission is not set and the order isn't sent yet, use the regional default set in the client config
                ElseIf Not IsDBNull(iDefaultPOTransmission) Then

                    m_iTransmissionMethod = iDefaultPOTransmission

                    Select Case iDefaultPOTransmission
                        Case 1 'Fax
                            Me.lblTransmissionType.Text = "Transmission Type: Fax"
                        Case 2 'Email
                            Me.lblTransmissionType.Text = "Transmission Type: Email"
                        Case 4
                            Me.lblTransmissionType.Text = "Transmission Type: Electronic"
                        Case Else
                            Me.lblTransmissionType.Text = "Transmission Type: Manual"
                    End Select
                Else
                    ' Clear all previously viewed order selections
                    Me.lblTransmissionType.Text = ""
                End If

                CheckBox_DropShip.CheckState = IIf(m_rsOrder.Fields("isDropShipment").Value = 0, 0, 1)

                ' Do file locking
                If IsDBNull(m_rsOrder.Fields("User_ID").Value) Then
                    lblReadOnly.Visible = False
                    cmdUnlock.Enabled = False
                Else
                    If m_rsOrder.Fields("User_ID").Value <> giUserID Then
                        bUserLock = True
                        lblReadOnly.Text = "Read Only (" & GetInvUserName(m_rsOrder.Fields("User_ID").Value) & ")"
                        lblReadOnly.Visible = True
                        cmdUnlock.Enabled = gbLockAdministrator
                    Else
                        lblReadOnly.Visible = False
                        cmdUnlock.Enabled = False
                    End If
                End If

                ' If the user has a store limit and this order is not for their store, then make it read-only
                If (Not lblReadOnly.Visible) And (Not gbCoordinator) And (glStore_Limit > 0 And glStore_Limit <> m_lStoreNo) Then
                    lblReadOnly.Visible = True
                    cmdUnlock.Enabled = False
                    lblReadOnly.Text = "Read Only"
                End If

                ' If the order has been sent to the warehouse system and has not been received in warehouse, make it read-only
                If (Not lblReadOnly.Visible) And m_rsOrder.Fields("WarehouseSent").Value And (Not m_bItemsReceived) Then
                    lblReadOnly.Visible = True
                    cmdUnlock.Enabled = False
                    lblReadOnly.Text = "Read Only"
                End If

                If (Not lblReadOnly.Visible) And m_bSent And (m_bEXEWarehouseDistOrder And m_bIsEXEDistributed And (Not gbWarehouse)) Then
                    lblReadOnly.Visible = True
                    cmdUnlock.Enabled = False
                    lblReadOnly.Text = "Read Only"
                End If

                ' If there is an ordering window and it is closed, set to read only
                If (Not lblReadOnly.Visible) And (m_dtOrderEnd > System.DateTime.FromOADate(0)) Then
                    If (DateDiff(DateInterval.Minute, m_dtCurrSysTime, m_dtOrderEnd) <= 0) And (Not gbWarehouse) Then
                        lblReadOnly.Visible = True
                        cmdUnlock.Enabled = False
                        lblReadOnly.Text = "Read Only"
                    End If
                End If

                If Not lblReadOnly.Visible And m_bItemsReceived And Not m_bOrderClosed And Not gbDistributor Then
                    lblReadOnly.Visible = True
                    cmdUnlock.Enabled = False
                    lblReadOnly.Text = "Read Only (Receiving Incomplete)"
                End If

                'Lock Store Order Guide (SOG) orders to store buyers. Only TMs in warehouse role can edit SOG orders.
                If (Not lblReadOnly.Visible) And (m_bSOGOrder And (Not gbWarehouse)) Then
                    lblReadOnly.Visible = True
                    cmdUnlock.Enabled = False
                    lblReadOnly.Text = "Read Only"
                End If

                ' Find the status of this order
                lblStatus.Text = String.Empty

                Dim sOtherSuppliesStatus As String = String.Empty

                Select Case geProductType
                    Case enumProductType.Product
                        sProductType = "Product"
                    Case enumProductType.PackagingSupplies
                        sProductType = "Packaging"
                    Case enumProductType.OtherSupplies
                        sProductType = "Supplies"
                        sOtherSuppliesStatus = "Supplies (" & m_rsOrder.Fields("Transfer_To_SubTeamName").Value & ")"
                End Select

                AddStatusInfo(sOtherSuppliesStatus)

                m_sSubTeamName = m_rsOrder.Fields("Transfer_To_SubTeamName").Value

                Select Case geOrderType
                    Case enumOrderType.Distribution
                        Me.Text = "Distribution " & sProductType & " Order Information"
                        If (m_rsOrder.Fields("SubTeam_Name").Value <> m_rsOrder.Fields("Transfer_To_SubTeamName").Value) Then
                            AddStatusInfo("Distribution (" & m_rsOrder.Fields("SubTeam_Name").Value & " to " & m_rsOrder.Fields("Transfer_To_SubTeamName").Value & ")")
                        Else
                            AddStatusInfo("Distribution (" & m_rsOrder.Fields("SubTeam_Name").Value & ")")
                        End If
                    Case enumOrderType.Purchase
                        Dim sSubTeamName As String
                        If IsDBNull(m_rsOrder.Fields("SupplyType_SubTeamName").Value) Then
                            sSubTeamName = m_rsOrder.Fields("Transfer_To_SubTeamName").Value.ToString().Trim()
                        Else
                            sSubTeamName = m_rsOrder.Fields("SupplyType_SubTeamName").Value.ToString().Trim()
                        End If
                        If m_rsOrder.Fields("Return_Order").Value = True Then
                            Me.Text = "Return Purchase " & sProductType & " Order Information"

                            AddStatusInfo("Credit, Purchase (" & sSubTeamName & ")")
                        Else
                            Me.Text = "Purchase " & sProductType & " Order Information"

                            AddStatusInfo("Purchase (" & sSubTeamName & ")")
                        End If
                    Case enumOrderType.Flowthru
                        If m_rsOrder.Fields("Return_Order").Value = True Then
                            Me.Text = "Return Flow Purchase " & sProductType & " Order Information"
                            AddStatusInfo("Credit, Flow Purchase (" & m_rsOrder.Fields("Transfer_To_SubTeamName").Value & ")")
                        Else
                            If m_rsOrder.Fields("Distribution_Center").Value Then
                                Me.Text = "Flow Distribution " & sProductType & " Order Information"
                                AddStatusInfo("Flow Distribution (" & m_rsOrder.Fields("Transfer_To_SubTeamName").Value & ")")
                            Else
                                Me.Text = "Flow Purchase " & sProductType & " Order Information"
                                AddStatusInfo("Flow Purchase (" & m_rsOrder.Fields("Transfer_To_SubTeamName").Value & ")")
                            End If
                        End If
                    Case enumOrderType.Transfer
                        Me.Text = "Transfer " & sProductType & " Order Information"
                        Dim sSuppliesToSubteam As String = Nothing
                        sSuppliesToSubteam = IIf(IsDBNull(m_rsOrder.Fields("SupplyType_SubTeamName").Value), "", m_rsOrder.Fields("SupplyType_SubTeamName").Value)
                        If sSuppliesToSubteam <> "" Then
                            AddStatusInfo("Transfer (" & m_rsOrder.Fields("SubTeam_Name").Value & " to " & m_rsOrder.Fields("SupplyType_SubTeamName").Value & ")")
                        Else
                            AddStatusInfo("Transfer (" & m_rsOrder.Fields("SubTeam_Name").Value & " to " & m_rsOrder.Fields("Transfer_To_SubTeamName").Value & ")")
                        End If
                End Select

                Me.Text += " " & GetInvoiceMethodDisplayText(glOrderHeaderID, m_bPayAgreedCost, m_iEInvoiceId)
                blnIsMyStore = CBool(Me.StoreNo = glStore_Limit) Or CBool(glStore_Limit = 0)

                If (Not IsDBNull(m_rsOrder.Fields("ReceivedDate").Value)) Then
                    txtRecvDate.Text = VB6.Format(m_rsOrder.Fields("ReceivedDate").Value, CStr(gsUG_DateMask))
                Else
                    txtRecvDate.Text = ""
                End If

                If (m_rsOrder.Fields("DSDOrder").Value) Then
                    m_IsDSDOrder = m_rsOrder.Fields("DSDOrder").Value
                Else
                    m_IsDSDOrder = False
                End If

                'TFS 10035, 02/11/2013, Faisal Ahmed, Make the quantity Ordered ccolumn visible 
                ugrdItems.DisplayLayout.Bands(0).Columns("QuantityOrdered").Hidden = False

                ' update form based on DSD value
                If (m_IsDSDOrder) Then
                    If m_rsOrder.Fields("Return_Order").Value = True Then
                        UpdateFormForDSDOrder(True)
                    Else
                        UpdateFormForDSDOrder()
                    End If
                Else
                    _lblLabel_2.Text = "Order Date :"
                End If

                If m_bSent Then AddStatusInfo("Sent")
                If m_rsOrder.Fields("WarehouseSent").Value Then AddStatusInfo("Warehouse")
                If m_bOrderClosed Then AddStatusInfo("Closed")

                ' The Suspended status is only applied to Purchase orders
                If (geOrderType = enumOrderType.Purchase Or geOrderType = enumOrderType.Flowthru) AndAlso m_bOrderClosed And Not m_bOrderApproved Then AddStatusInfo("Suspended")

                If (m_bOrderApproved And IsDBNull(m_rsOrder.Fields("RefuseReceivingReasonID").Value)) Then AddStatusInfo("Approved")
                If Not IsDBNull(m_rsOrder.Fields("RefuseReceivingReasonID").Value) Then AddStatusInfo("Refused")
                If m_bOrderUploaded Then AddStatusInfo("Uploaded: " & m_rsOrder.Fields("UploadedDate").Value)
                If m_bOrderAccountingUploaded Then AddStatusInfo("GL Uploaded")
                If lblStatus.Text <> String.Empty Then lblStatus.Text = "[" & Mid(lblStatus.Text, 1, Len(lblStatus.Text)) & "]"
                If InStr(1, lblStatus.Text, "Refused", CompareMethod.Text) > 0 Then
                    lblStatus.ForeColor = Color.Red
                Else
                    lblStatus.ForeColor = Color.Black
                End If

                ' Check access for following fields
                SetActive(txtField(iOrderHeaderTemperature), Not (IsDeleted Or lblReadOnly.Visible Or m_bOrderClosed Or Not ((gbDistributor And (glStore_Limit = 0 Or glStore_Limit = m_lStoreNo)) And m_bSent)))
                SetActive(txtField(iOrderHeaderOrderDate), False)
                SetActive(txtField(iOrderHeaderInvoiceNumber), False)
                SetActive(txtField(iOrderHeaderQuantityDiscount), Not (lblReadOnly.Visible Or m_bOrderClosed Or (gbDistributor And Not (gbSuperUser Or gbBuyer)) Or Not (gbBuyer And (m_lTransferFromSubTeam = -1 Or gbSuperUser))))

                If Not m_bOrderClosed And bBuyerDistributor And (Not lblReadOnly.Visible Or (m_bEXEWarehousePurchaseOrder And m_bIsEXEDistributed And gbWarehouse)) Then
                    dtpExpectedDate.Enabled = True
                Else
                    dtpExpectedDate.Enabled = False
                End If

                SetActive(txtField(iOrderHeaderTotalFreight), False)
                SetActive(txtField(iOrderHeader3rdPartyFreight), Not (IsDeleted Or lblReadOnly.Visible Or m_bOrderClosed))
                SetActive(txtField(iOrderHeaderOrgPO), chkField(iOrderHeaderReturn_Order).CheckState And m_bOrderClosed = False)
                SetActive(txtField(iOrderHeaderRecvLog_No), False)
                SetActive(cmbField(iOrderHeaderReceiveLocation_ID), False)
                SetActive(cmbField(iOrderHeaderPurchaseLocation_ID), False)
                SetActive(cmbField(iOrderHeaderDiscountType), Not (txtField(iOrderHeaderQuantityDiscount).ReadOnly))
                ucCostAdjReasonCode.Enabled = Not (txtField(iOrderHeaderQuantityDiscount).ReadOnly)
                SetActive(chkField(iOrderHeaderReturn_Order), False)
                SetActive(cmdAddOrder, gbBuyer And Not bSpecificOrder)
                SetActive(cmdCopyPO, gbBuyer And Not bSpecificOrder And Not m_IsDSDOrder)

                If gbDeletePO Then
                    SetActive(cmdDeleteOrder, (Not lblReadOnly.Visible) And m_bAllowPOEdit And Not m_bOrderUploaded And (((Not m_bItemsReceived)) Or gbPOAccountant) And blnIsMyStore)
                Else
                    ' Can delete if order is not received and user is Coordinator or if order belongs to logged on user or if it is the PO Accountant (who can delete any order)
                    SetActive(cmdDeleteOrder, (Not m_bOrderClosed) And (Not lblReadOnly.Visible) And m_bAllowPOEdit And Not m_bOrderUploaded And (((giUserID = m_lCreatedBy) And (Not m_bItemsReceived)) Or gbPOAccountant) And blnIsMyStore)
                End If

                SetActive(cmdSendOrder, (Not lblReadOnly.Visible) And m_bAllowPOEdit And (gbBuyer Or gbSuperUser Or gbPOAccountant) And (Not m_bOrderClosed) And (Not m_rsOrder.Fields("WarehouseSent").Value) And blnIsMyStore And (Not m_IsDSDOrder))

                SetActive(cmdWarehouseSend, (Not lblReadOnly.Visible) And m_bAllowPOEdit And gbWarehouse And (txtField(iOrderHeaderSentDate).Text <> "") And (Not m_bItemsReceived) And EXEOrder)
                SetActive(cmdUndoWarehouseSend, lblReadOnly.Visible And (gbDCAdmin Or gbSuperUser) And (txtField(iOrderHeaderSentDate).Text <> "") And (Not m_bOrderClosed) And EXEOrder)
                SetActive(cmdCloseOrder, ((Not lblReadOnly.Visible) And m_bAllowPOEdit And blnIsMyStore) And ((gbPOAccountant And IsDSDOrder) Or Not IsDSDOrder))
                m_bReceivingAllowed = (Not lblReadOnly.Visible) And m_bAllowPOEdit And (gbDistributor And (glRecvLog_Store_Limit = 0 Or glRecvLog_Store_Limit = m_lStoreNo)) And (txtField(iOrderHeaderSentDate).Text <> "") And (Not m_bOrderClosed) And (Not (EXEOrder) Or IsCredit Or m_bItemsReceived)

                If gbPOAccountant And (Not gbSuperUser And Not gbReceiver) Then
                    m_bReceivingAllowed = False
                End If

                SetActive(cmdReceive, m_bReceivingAllowed And m_bAllowPOEdit And blnIsMyStore)
                SetActive(cmdSearch, Not bSpecificOrder)
                SetActive(cmdChgVendor, False)
                SetActive(cmdAddItem, gbBuyer And m_bAllowPOEdit And Not (lblReadOnly.Visible Or m_bOrderClosed) And (Not m_rsOrder.Fields("WarehouseSent").Value))
                SetActive(chkApplyAllCreditReason, chkField(iOrderHeaderReturn_Order).Checked And cmdAddItem.Enabled, Not (chkField(iOrderHeaderReturn_Order).Checked And cmdAddItem.Enabled))
                SetActive(cmdReports, True)
                SetActive(cmdOrderNotes, (Not lblReadOnly.Visible))
                SetActive(cmdEditItem, (Not lblReadOnly.Visible) And m_bAllowPOEdit And (gbBuyer Or gbPOEditor Or ((gbDistributor And (glStore_Limit = 0 Or glStore_Limit = m_lStoreNo)) And txtField(iOrderHeaderSentDate).Text <> "")))
                SetActive(cmdDeleteItem, (Not lblReadOnly.Visible) And m_bAllowPOEdit And gbBuyer And (Not m_bOrderClosed) And (Not m_rsOrder.Fields("WarehouseSent").Value))
                If (geOrderType = enumOrderType.Transfer) Then
                    SetActive(cmdOrderList, False)
                Else
                    SetActive(cmdOrderList, (Not lblReadOnly.Visible) And m_bAllowPOEdit And (gbBuyer) And (Not m_bOrderClosed) And (Not m_rsOrder.Fields("WarehouseSent").Value))
                End If
                SetActive(cmdDistributionCreditOrder, geOrderType = enumOrderType.Distribution And (Not bUserLock) And m_bOrderClosed And m_lOriginalOrder_ID = -1 And gbFacilityCreditProcessor)
                SetActive(cmd3rdPartyInvoice, m_bAllowPOEdit)

                ' TFS 8325, 02/28/2013, Faisal Ahmed, Refusal functionality
                ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
                'SetActive(Me.cmdItemsRefused, IsRefusalAllowed(m_lOrderHeader_ID) And Not IsDSDOrder)

                SetActive(Me.OrderedCostTextBox, True, m_IsDSDOrder)
                SetActive(Me.OrderedCostLabel, True, m_IsDSDOrder)

                m_rsOrder.Close()
                m_rsOrder = Nothing

                If geOrderType = enumOrderType.Distribution Then
                    If (gbBuyer And gbWarehouse) OrElse (gbDistributor And gbWarehouse) And m_bAllowPOEdit Then
                        ugrdItems.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.True
                    Else
                        ugrdItems.DisplayLayout.Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False
                    End If
                End If

                ' TFS#10641 -  eInvoicing Column Editable on the PO Information Screen
                ugrdItems.DisplayLayout.Bands(0).Columns("eInvoice").CellActivation = Infragistics.Win.UltraWinGrid.Activation.Disabled

                ' TFS#9813 - Change label on Actual Cost column depending on order type
                If geOrderType = enumOrderType.Distribution Or geOrderType = enumOrderType.Flowthru Then
                    ' DC order, show as Delivered Cost
                    ugrdItems.DisplayLayout.Bands(0).Columns("ActualCost").Header.Caption = "Delivered Cost"
                Else
                    ' All other order types, show as Case Cost
                    ugrdItems.DisplayLayout.Bands(0).Columns("ActualCost").Header.Caption = "Case Cost"
                End If

                ugrdItems.DisplayLayout.Bands(0).Columns("ActualCost").CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit

                '################################################################################################################
                ' Bug #6735 (Robin Eudy) Disabled Editing for the Cost field for all users regardless of permissions if an order is closed.
                ' for now this overrides the distribution order type settings that are set a above.
                '################################################################################################################
                ugrdItems.DisplayLayout.Override.AllowUpdate = IIf(m_bOrderClosed, Infragistics.Win.DefaultableBoolean.False, Infragistics.Win.DefaultableBoolean.True)

                If chkField(iOrderHeaderReturn_Order).CheckState = 0 Then
                    Try
                        SQLOpenRS(m_rsOrder, "GetCreditOrderList " & m_lOrderHeader_ID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                        If m_rsOrder.EOF Then
                            lblCredit.Text = ""
                        Else
                            lblCredit.Text = "Credits: "
                            While Not m_rsOrder.EOF
                                lblCredit.Text = lblCredit.Text & m_rsOrder.Fields("ReturnOrderHeader_id").Value
                                m_rsOrder.MoveNext()
                                If Not m_rsOrder.EOF Then lblCredit.Text = lblCredit.Text & ", "
                            End While
                        End If

                    Finally
                        If m_rsOrder IsNot Nothing Then
                            m_rsOrder.Close()
                            m_rsOrder = Nothing
                        End If
                    End Try
                Else
                    lblCredit.Text = ""
                End If

            End If

            RefreshOrderDetails(m_lOrderHeader_ID, True)

            m_bLoading = False
            m_bDataChanged = False

            If lRecord <> -1 And m_lOrderHeader_ID = -1 Then
                RefreshDataSource(-1)
                CheckNoOrders()
            End If

            If m_dtOrderEnd > System.DateTime.FromOADate(0) Then
                m_dtCurrSysTime = DateAdd(DateInterval.Second, DateDiff(DateInterval.Second, dStartTime, SystemDateTime), m_dtCurrSysTime)
                Timer1.Enabled = True
            End If

            'Setting up contols for deleted PO
            If IsDeleted Then
                SetActive(cmdDeleteOrder, False)
                SetActive(cmdReports, False)
                SetActive(cmdOrderNotes, True)
                SetActive(cmdUnlock, False)
                SetActive(cmdOrderList, False)
                SetActive(cmdReceive, False)
                SetActive(cmdSendOrder, False)
                SetActive(cmdChgVendor, False)
                SetActive(cmdWarehouseSend, False)
                SetActive(cmdUndoWarehouseSend, False)
                SetActive(cmdDistributionCreditOrder, False)
                SetActive(cmdDisplayEInvoice, False)
                SetActive(cmdCloseOrder, False)
                SetActive(cmd3rdPartyInvoice, False)
                SetActive(cmdAddItem, False)
                SetActive(cmdEditItem, False)
                SetActive(cmdDeleteItem, False)
                SetActive(chkApplyAllCreditReason, False)
                AddStatusInfo("Deleted")
                lblReadOnly.Text = "Read Only (system)"
                lblReadOnly.Visible = True
                lblDeletedBy.Visible = True
                lblDeletedDate.Visible = True
                lblDeletedReason.Visible = True
                txtDeletedby.Visible = True
                txtDeletedDate.Visible = True
                txtDeletedReason.Visible = True
            Else
                lblDeletedBy.Visible = False
                lblDeletedDate.Visible = False
                lblDeletedReason.Visible = False
                txtDeletedby.Visible = False
                txtDeletedDate.Visible = False
                txtDeletedReason.Visible = False
            End If

        Catch ex As Exception
            Dim msg As String = "Unable to load order information.  Please contact support if the problem persists."
            logger.Error(msg, ex)
            ErrorDialog.HandleError(msg, ex, ErrorDialog.NotificationTypes.DialogAndEmail, "")
        Finally
            If m_rsOrder IsNot Nothing Then
                m_rsOrder.Close()
                m_rsOrder = Nothing
            End If
        End Try

        Timer1_Tick(Timer1, New System.EventArgs())

        logger.Debug("RefreshDataSource Exit")
    End Sub

    ''' <summary>
    ''' Close an order without entering invoice or document data.  This is a helper method that was added to this UI for
    ''' orders that do not allow the user to edit the invoice information on the OrderStatus.vb screen.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CloseOrder()
        logger.Debug("CloseOrder Entry")

        ' Prompt the user to close this order.  They are not allowed to enter invoice information on the
        ' OrderStatus UI screen, so we can automatically perform the close for them.
        If MsgBox(ResourcesOrdering.GetString("msg_ConfirmCloseOrder"), MsgBoxStyle.Question + MsgBoxStyle.YesNo, "Verify Close Order") = MsgBoxResult.Yes Then
            If OrderingDAO.IsOrderReceivingComplete(glOrderHeaderID) Then
                ' All line items have a received quantity; close the order.
                logger.Info("CloseOrder - user confirmed to close the order, all items have been received: OrderHeader_ID=" + glOrderHeaderID.ToString)
                OrderingDAO.CloseOrder(glOrderHeaderID)
            Else
                ' Prompt the user to enter a received quantity for all items.
                MsgBox("Not all line items have been received.  Please open the Receiving List and enter a received quantity for each line.", MsgBoxStyle.Exclamation, Me.Text)
                logger.Info("CloseOrder - user confirmed to close the order, all items have not been received: OrderHeader_ID=" + glOrderHeaderID.ToString)
            End If
        End If

        logger.Debug("CloseOrder Exit")
    End Sub

    Private Sub frmOrders_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        logger.Debug("frmOrders_FormClosing Entry")

        If m_lOrderHeader_ID > -1 Then
            e.Cancel = Not SaveData()
            If (e.Cancel = 0) And m_bItemsReceived And (Not m_bOrderClosed) And (Not lblReadOnly.Visible) And (Not gbEInvoicing) Then
                ' Do not require the user to close the order if the Transfer Subteam (glSubTeamNo) is set because they cannot enter any data
                ' on the OrderStatus UI screen when the glSubTeamNo = -1.
                If glSubTeamNo = -1 Then
                    e.Cancel = True
                    MsgBox("Items have been received, and the order must be closed." & vbCrLf & "Please enter Invoice/Document Data to close the order.", MsgBoxStyle.Information, Me.Text)
                    logger.Info("frmOrders_FormClosing " & " Items received - order must be closed." & vbCrLf & "Please enter Invoice/Document Data and close...")
                    logger.Debug("frmOrders_FormClosing Exit")
                    cmdCloseOrder.PerformClick()
                    Exit Sub
                Else
                    ' Prompt the user to close this order.  They are not allowed to enter invoice information on the
                    ' OrderStatus UI screen, so we can automatically do the close for them.
                    CloseOrder()
                End If
            ElseIf (e.Cancel = 0) And (m_bItemsReceived And (Not m_bOrderClosed) And (Not lblReadOnly.Visible) And gbEInvoicing) Then
                MsgBox("Items have been received, but the order is not closed.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
            ElseIf e.Cancel <> 0 Then
                MsgBox("The form will be closed without saving the order.", MsgBoxStyle.Information + MsgBoxStyle.OkOnly, Me.Text)
                e.Cancel = 0

                'If 3rdPartyFreight value has been changed
                If CDec(txtField(iOrderHeader3rdPartyFreight).Text) <> m_3rdParty Then
                    ' restore old value
                    txtField(iOrderHeader3rdPartyFreight).Text = m_3rdParty.ToString
                    ' Run update to the line items
                    DistributeFreight(CInt(txtField(iOrderHeaderOrderHeader_ID).Text),
                          CDec(txtField(iOrderHeader3rdPartyFreight).Text))
                End If
            End If
        End If

        If m_rsOrderItems IsNot Nothing Then
            m_rsOrderItems.Close()
            m_rsOrderItems = Nothing
        End If

        UnlockPO()

        'Clear globals used by Item Search window
        glSubTeamNo = 0
        gbExcludeNot_Available = False
        gsVendorName = ""
        gsPOCreatorUserName = ""
        gsStoreName = ""
        geOrderType = 0
        geProductType = 0
        glStoreNo = 0
        gbDistribution_Center = False
        gbManufacturer = False

        Timer1.Enabled = False

        logger.Debug("frmOrders_FormClosing Exit")
    End Sub

    Private Sub SetupDataTable()
        logger.Debug("SetupDataTable Entry")

        mdt = New DataTable("OrderItemList")
        mdt.Columns.Add(New DataColumn("Item_Key", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("OrderItem_ID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Line", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("Identifier", GetType(String)))
        mdt.Columns.Add(New DataColumn("Brand_Name", GetType(String)))
        mdt.Columns.Add(New DataColumn("Item_Description", GetType(String)))
        mdt.Columns.Add(New DataColumn("Vendor_Item_Description", GetType(String)))
        mdt.Columns.Add(New DataColumn("QuantityOrdered", GetType(String)))
        mdt.Columns.Add(New DataColumn("QuantityReceived", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("ActualCost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("Reg Cost", GetType(Decimal)))
        mdt.Columns.Add(New DataColumn("eInvoice", GetType(Decimal)))

        logger.Debug("SetupDataTable Exit")
    End Sub

    Private Sub ugrdItems_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles ugrdItems.DoubleClickRow
        logger.Debug("ugrdItems_DoubleClickRow Entry")

        '-- Make the edit button go
        If cmdEditItem.Enabled Then
            cmdEditItem.PerformClick()
        End If

        logger.Debug("ugrdItems_DoubleClickRow Exit")
    End Sub

    Private Sub UpdateOrderItemsCost()
        logger.Debug("UpdateOrderItemsCost Entry")

        Dim Freight3rdParty As Decimal
        Dim LineItemFreight3rdParty As Decimal
        Dim LandingCost As Decimal
        Dim Markupcost As Decimal
        Dim LineItemCost As Decimal
        Dim LineItemHandling As Decimal
        Dim LineItemFreight As Decimal
        Dim ReceivedItemCost As Decimal
        Dim ReceivedItemHandling As Decimal
        Dim ReceivedItemFreight As Decimal
        Dim OrderItemID As Integer
        Dim lIgnoreErrNum(0) As Integer
        Dim sSQL As String

        If Not (m_rsOrderItems Is Nothing) Then
            m_rsOrderItems.Close()
        End If

        SQLOpenRS(m_rsOrderItems, "EXEC GetOrderItemsCostData " & CInt(txtField(iOrderHeaderOrderHeader_ID).Text), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        While Not m_rsOrderItems.EOF
            OrderItemID = CInt(m_rsOrderItems.Fields("OrderItem_ID").Value)

            If IsDBNull(m_rsOrderItems.Fields("Freight3Party").Value) Then
                Freight3rdParty = 0
            Else
                Freight3rdParty = CDec(m_rsOrderItems.Fields("Freight3Party").Value)
            End If

            If IsDBNull(m_rsOrderItems.Fields("LineItemFreight3Party").Value) Then
                LineItemFreight3rdParty = "0.00"
            Else
                LineItemFreight3rdParty = CDec(m_rsOrderItems.Fields("LineItemFreight3Party").Value)
            End If

            LandingCost = CalculateCost(iOrderItemLandingCost) + Freight3rdParty

            Markupcost = CalculateCost(iOrderItemMarkupCost)
            LineItemCost = CalculateCost(iOrderItemLineItemCost)
            LineItemHandling = CalculateCost(iOrderItemLineItemHandling)
            LineItemFreight = CalculateCost(iOrderItemLineItemFreight)
            ReceivedItemCost = CalculateCost(iOrderItemReceivedItemCost)
            ReceivedItemHandling = CalculateCost(iOrderItemReceivedItemHandling)
            ReceivedItemFreight = CalculateCost(iOrderItemReceivedItemFreight)

            lIgnoreErrNum(0) = 50002
            sSQL = "EXEC UpdateOrderItemCostData " & OrderItemID & ", " &
                         LandingCost & ", " &
                         Markupcost & ", " &
                         LineItemCost & ", " &
                         LineItemHandling & ", " &
                         LineItemFreight & ", " &
                         ReceivedItemCost & ", -" &
                         ReceivedItemHandling & ", -" &
                         ReceivedItemFreight & ", " &
                         Freight3rdParty & ", " &
                         LineItemFreight3rdParty
            SQLExecute3(sSQL, DAO.RecordsetOptionEnum.dbSQLPassThrough,
                         lIgnoreErrNum)

            If Err.Number <> 0 Then
                logger.Error("UpdateOrderItemsCost " & " Error in EXEC UpdateOrderItemCostData " & Err.Description)
                MsgBox(Err.Description, MsgBoxStyle.Critical, Me.Text)
            End If

            m_rsOrderItems.MoveNext()
        End While

        If m_rsOrderItems IsNot Nothing Then
            m_rsOrderItems.Close()
            m_rsOrderItems = Nothing
        End If

        RefreshOrderDetails(m_lOrderHeader_ID, False)

        logger.Debug("UpdateOrderItemsCost Exit")
    End Sub

    Private Function CalculateCost(ByVal iCostField As Short) As Decimal
        logger.Debug("CalculateCost Entry")

        Dim cCost As Decimal
        Dim dFreeUnits As Decimal
        Dim ItemDiscountType As Integer
        Dim ItemQuantityDiscount As Decimal
        Dim NetVendorItemDiscount As Decimal
        Dim CostUnit As Integer
        Dim QuantityUnit As Integer
        Dim Package_Desc1 As Decimal
        Dim Package_Desc2 As Decimal
        Dim Package_Unit_ID As Integer
        Dim Total_Weight As Decimal
        Dim QuantityReceived As Decimal
        Dim MarkupPercent As Decimal
        Dim ItemFreight As Decimal
        Dim FreightUnit As Integer
        Dim ItemHandling As Decimal
        Dim HandlingUnit As Integer
        Dim QuantityOrdered As Decimal
        Dim AdjustedCost As Decimal
        Dim pcUnitCost As Decimal
        Dim pcUnitExtCost As Decimal
        Dim Retail_Unit_ID As Integer
        Dim FloatingUnitValue As Integer

        Dim cu As tItemUnit
        Dim fu As tItemUnit

        ItemDiscountType = CInt(m_rsOrderItems.Fields("DiscountType").Value)
        ItemQuantityDiscount = CDec(m_rsOrderItems.Fields("QuantityDiscount").Value)
        NetVendorItemDiscount = CDec(m_rsOrderItems.Fields("NetVendorItemDiscount").Value)
        CostUnit = CInt(m_rsOrderItems.Fields("CostUnit").Value)
        QuantityUnit = CInt(m_rsOrderItems.Fields("QuantityUnit").Value)
        Package_Desc1 = CDec(m_rsOrderItems.Fields("Package_Desc1").Value)
        Package_Desc2 = CDec(m_rsOrderItems.Fields("Package_Desc2").Value)
        Package_Unit_ID = CInt(m_rsOrderItems.Fields("Package_Unit_ID").Value)
        Total_Weight = CDec(m_rsOrderItems.Fields("Total_Weight").Value)
        QuantityReceived = CDec(IIf(IsDBNull(m_rsOrderItems.Fields("QuantityReceived").Value), 0, m_rsOrderItems.Fields("QuantityReceived").Value))
        MarkupPercent = CDec(m_rsOrderItems.Fields("MarkupPercent").Value)
        ItemFreight = CDec(m_rsOrderItems.Fields("Freight").Value)
        FreightUnit = CInt(m_rsOrderItems.Fields("FreightUnit").Value)
        ItemHandling = CDec(m_rsOrderItems.Fields("Handling").Value)
        HandlingUnit = CInt(m_rsOrderItems.Fields("HandlingUnit").Value)
        QuantityOrdered = CDec(m_rsOrderItems.Fields("QuantityOrdered").Value)
        AdjustedCost = CDec(m_rsOrderItems.Fields("AdjustedCost").Value)
        Retail_Unit_ID = CInt(m_rsOrderItems.Fields("Retail_Unit_ID").Value)


        ' Use the Current VendorCost (RegCost) for orders with Discount Type = Percent Discount
        If ((geOrderType = enumOrderType.Flowthru Or geOrderType = enumOrderType.Purchase) And (OrderDiscountType = 2 Or ItemDiscountType = 2)) Then
            cCost = CDec(m_rsOrderItems.Fields("CurrentVendorCost").Value)
        Else
            cCost = CDec(m_rsOrderItems.Fields("Cost").Value)
        End If

        Select Case iCostField
            Case iOrderItemLandingCost
                FloatingUnitValue = HandlingUnit

            Case iOrderItemMarkupCost, iOrderItemLineItemCost
                FloatingUnitValue = QuantityUnit

            Case iOrderItemLineItemHandling
                FloatingUnitValue = Retail_Unit_ID

            Case iOrderItemLineItemFreight
                FloatingUnitValue = Package_Unit_ID
        End Select


        dFreeUnits = IIf(ItemDiscountType = 3, ItemQuantityDiscount, 0)

        '-- Convert Cost, Handling, Freight to their respective units

        If iCostField >= iOrderItemLineItemCost Then
            cCost = IIf(AdjustedCost = 0, cCost, AdjustedCost)

            Select Case iCostField
                Case iOrderItemLineItemCost, iOrderItemReceivedItemCost

                    'This function should only deal with line item records that 
                    'do not have a vendor discount or manually entered line item discount.
                    'The stored procedure that populates the recordset excludes these records.
                    'The extra code is here because the business requirments are still in flux.

                    Select Case ItemDiscountType
                        Case 0

                            If OrderDiscountExists Then
                                Select Case OrderDiscountType
                                    Case 1 : cCost = cCost - OrderDiscountAmt
                                    Case 2 : cCost = cCost - (cCost * (OrderDiscountAmt / 100))
                                    Case 4 : cCost = cCost - (cCost * (OrderDiscountAmt / 100))
                                End Select
                            End If

                        Case 1 : cCost = cCost - ItemQuantityDiscount
                            'Case 2 : cCost = cCost * (100 - ItemQuantityDiscount) / 100
                        Case 2 : cCost = cCost - (cCost * (ItemQuantityDiscount / 100))
                    End Select

                    cCost = CostConversion(cCost,
                                          CostUnit,
                                          QuantityUnit,
                                          Package_Desc1,
                                          Package_Desc2,
                                          Package_Unit_ID,
                                          Total_Weight,
                                          (QuantityReceived * (100 + MarkupPercent) / 100))

                Case iOrderItemLineItemFreight, iOrderItemReceivedItemFreight

                    cCost = CostConversion(ItemFreight,
                                           FreightUnit,
                                           QuantityUnit,
                                           Package_Desc1,
                                           Package_Desc2,
                                           Package_Unit_ID,
                                           Total_Weight,
                                           (QuantityReceived * (100 + MarkupPercent) / 100))

                Case iOrderItemLineItemHandling, iOrderItemReceivedItemHandling

                    cCost = CostConversion(ItemHandling,
                                           HandlingUnit,
                                           QuantityUnit,
                                           Package_Desc1,
                                           Package_Desc2,
                                           Package_Unit_ID,
                                           Total_Weight,
                                           QuantityReceived)
            End Select

            If iCostField >= iOrderItemReceivedItemCost Then

                If QuantityReceived = 0 Then
                    cCost = 0
                Else
                    If ItemDiscountType = 3 And iCostField = iOrderItemReceivedItemCost Then
                        If QuantityReceived < ItemQuantityDiscount Then
                            cCost = 0
                        Else
                            cCost = cCost * (QuantityReceived - ItemQuantityDiscount)
                        End If
                    Else
                        cCost = cCost * QuantityReceived
                    End If
                End If
            Else
                cCost = cCost * (QuantityOrdered + IIf(iCostField = iOrderItemLineItemCost, 0, dFreeUnits))
            End If

        Else
            If AdjustedCost = 0 Then

                'This function should only deal with line item records that 
                'do not have a vendor discount or manually entered line item discount.
                'The stored procedure that populates the recordset excludes these records.
                'The extra code is here because the business requirments are still in flux.

                Select Case ItemDiscountType
                    Case 0
                        If NetVendorItemDiscount > 0 Then
                            'cCost = cCost - NetVendorItemDiscount
                        End If

                        If OrderDiscountExists Then
                            Select Case OrderDiscountType
                                Case 1 : cCost = cCost - OrderDiscountAmt
                                Case 2 : cCost = cCost - (cCost * (OrderDiscountAmt / 100))
                                Case 4 : cCost = cCost - (cCost * (OrderDiscountAmt / 100))
                            End Select
                        End If

                    Case 1 : cCost = cCost - ItemQuantityDiscount
                        'Case 2 : cCost = cCost * ((100 - ItemQuantityDiscount) / 100)
                    Case 2 : cCost = cCost - (cCost * (ItemQuantityDiscount / 100))

                    Case 3
                        If QuantityOrdered <> 0 Then
                            cCost = (cCost * QuantityOrdered) / (ItemQuantityDiscount + QuantityOrdered)
                        Else
                            cCost = 0
                        End If
                End Select

                cu = GetItemUnit(CostUnit)
                fu = GetItemUnit(FreightUnit)

                cCost = CostConversion(cCost,
                                        CostUnit,
                                        QuantityUnit,
                                        Package_Desc1,
                                        Package_Desc2,
                                        Package_Unit_ID,
                                        Total_Weight,
                                        QuantityReceived)

                pcUnitCost = CostConversion(cCost,
                                            QuantityUnit,
                                            IIf(cu.IsPackageUnit, giUnit, CostUnit),
                                            Package_Desc1,
                                            Package_Desc2,
                                            Package_Unit_ID,
                                            Total_Weight,
                                            QuantityReceived)

                cCost = cCost + CostConversion(ItemFreight,
                                               FreightUnit,
                                               QuantityUnit,
                                               Package_Desc1,
                                               Package_Desc2,
                                               Package_Unit_ID,
                                               Total_Weight,
                                               QuantityReceived)

                pcUnitExtCost = CostConversion(cCost,
                                               QuantityUnit,
                                               IIf(fu.IsPackageUnit, giUnit, FreightUnit),
                                               Package_Desc1,
                                               Package_Desc2,
                                               Package_Unit_ID,
                                               Total_Weight,
                                               QuantityReceived)

                If iCostField >= iOrderItemMarkupCost Then
                    pcUnitCost = pcUnitCost * (100 + MarkupPercent) / 100
                    pcUnitExtCost = pcUnitExtCost * (100 + MarkupPercent) / 100
                    cCost = cCost * (100 + MarkupPercent) / 100
                End If

                cCost = cCost + CostConversion(ItemHandling,
                                               HandlingUnit,
                                               FloatingUnitValue,
                                               Package_Desc1,
                                               Package_Desc2,
                                               Package_Unit_ID,
                                               Total_Weight,
                                               QuantityReceived)
            Else
                cCost = CostConversion(AdjustedCost,
                                       QuantityUnit,
                                       FloatingUnitValue,
                                       Package_Desc1,
                                       Package_Desc2,
                                       Package_Unit_ID,
                                       Total_Weight,
                                       QuantityReceived)
            End If
        End If

        CalculateCost = cCost

        logger.Debug("CalculateCost Exit")

    End Function

    Private Sub ugrdItems_BeforeExitEditMode(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeExitEditModeEventArgs) Handles ugrdItems.BeforeExitEditMode
        logger.Debug("ugrdItems_BeforeExitEditMode Entry")
        If Not e.CancellingEditOperation Then
            Dim activeCell As Infragistics.Win.UltraWinGrid.UltraGridCell = ugrdItems.ActiveCell
            If activeCell.Column.Key = "ActualCost" Then
                If activeCell.DataChanged Then
                    Dim costValue As Decimal
                    Dim validCost As Boolean = False
                    Try
                        costValue = CDec(activeCell.Text)
                        validCost = True
                    Catch ex As Exception
                        logger.Error("Order item cost value conversion failed for value '" & activeCell.Text & "', error message: " & ex.Message)
                        MessageBox.Show("The cost value could not be converted to a valid price/decimal value.", "Invalid Cost Value", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        activeCell.CancelUpdate()
                    End Try
                    If validCost Then
                        SQLExecute(String.Format("EXEC UpdateOrderItemAdjustedCost {0}, {1}",
                                CInt(activeCell.Row.GetCellValue(ugrdItems.DisplayLayout.Bands(0).Columns("OrderItem_ID"))),
                                costValue),
                            DAO.RecordsetOptionEnum.dbSQLPassThrough)
                        UpdateOrderItemsCost()
                    End If
                End If
            End If
        End If
        logger.Debug("ugrdItems_BeforeExitEditMode Exit")
    End Sub

    Private Sub _txtField_16_Leave(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _txtField_16.Leave
        logger.Debug("=> _txtField_16_Leave")

        If Validate3rdPartyFreight() Then

            DistributeFreight(CInt(txtField(iOrderHeaderOrderHeader_ID).Text),
                              CDec(txtField(iOrderHeader3rdPartyFreight).Text))


            ' Refresh the grid
            RefreshOrderDetails(CInt(txtField(iOrderHeaderOrderHeader_ID).Text), True)
        End If

        logger.Debug("_txtField_16_Leave Exit")
    End Sub

    Private Function Validate3rdPartyFreight() As Boolean
        logger.Debug("=> Validate3rdPartyFreight")

        Dim dTotFreight As Decimal

        ' Check value of txtField(iOrderHeader3rdPartyFreight) for validity
        If IsNumeric(txtField(iOrderHeader3rdPartyFreight).Text) Then
            dTotFreight = CDec(txtField(iOrderHeader3rdPartyFreight).Text)
        Else
            MsgBox("3rd Party Freight amount must be numeric in order to distribute it.")
            logger.Debug("<= Validate3rdPartyFreight(False)")
            Return False
        End If

        'if the value has more than 3 decimal places, reject it.
        If Decimal.Floor(10000 * dTotFreight) <> 10000 * dTotFreight Then
            MsgBox("3rd Party Freight amount must be 2 decimal places or less in order to distribute.")
            logger.Info("Validate3rdPartyFreight " & " 3rd Party Freight amount must be 2 decimal places or less in order to distribute.")
            logger.Debug("<= Validate3rdPartyFreight(False)")
            Return False
        End If

        'if the value is less than zero, reject it.
        If dTotFreight < 0 Then
            MsgBox("3rd Party Freight amount must be greater than or equal to zero in order to save and distribute it.")
            logger.Info("Validate3rdPartyFreight " & " 3rd Party Freight amount must be greater than or equal to zero in order to save and distribute it.")
            logger.Debug("<= Validate3rdPartyFreight(False)")
            Return False
        End If

        Return True

        logger.Debug("<= Validate3rdPartyFreight(True)")
    End Function

    Private Sub cmd3rdPartyInvoice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd3rdPartyInvoice.Click
        logger.Debug("cmd3rdPartyInvoice_Click Entry")

        If Not SaveData() Then Exit Sub
        If m_lOrderHeader_ID > 0 Then
            glOrderHeaderID = m_lOrderHeader_ID

            Dim frm As New frmThirdPartyFreightInvoice()
            With frmThirdPartyFreightInvoice
                .SetInvoice(m_lOrderHeader_ID)
                .ShowDialog()
                .Close()
                .Dispose()
            End With

            RefreshDataSource(m_lOrderHeader_ID)
        End If

        logger.Debug("cmd3rdPartyInvoice_Click Exit")
    End Sub

    Private Sub cmdDisplayEInvoice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDisplayEInvoice.Click
        EInvoiceHTMLDisplay.EinvoiceId = Me.EInvoiceId
        EInvoiceHTMLDisplay.StoreNo = Me.StoreNo
        EInvoiceHTMLDisplay.ShowDialog()
    End Sub

    Private Sub cmdCopyPO_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCopyPO.Click

        ' If the order is an old order for a vendor which is now GSS, do not allow the user to copy it.
        If (OrderingDAO.CheckVendorIsDSDForStore(m_lVendorID, m_lStoreNo)) Then
            MsgBox("Cannot copy a purchase order for a Guaranteed Sale Supplier.  Please use IRMA Mobile to create a Receiving Document for this vendor.", MsgBoxStyle.Information, "Cannot Copy Order")
            Exit Sub
        End If

        If lblReadOnly.Visible And InStr(lblReadOnly.Text, "(") > 0 Then
            Dim sUserName As String
            sUserName = Mid(lblReadOnly.Text, InStr(lblReadOnly.Text, "(") + 1)
            sUserName = Mid(sUserName, 1, Len(sUserName) - 1)

            If MsgBox("The PO you are copying is currently locked by user " & sUserName & ".  Any changes " & sUserName & " might make to this PO after you copy will not be reflected in the new PO.  Do you want to continue?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "IRMA Copy PO") = MsgBoxResult.Yes Then
                DoPOCopy()
            Else
                Exit Sub
            End If
        Else
            DoPOCopy()
        End If

    End Sub

    Public Shared Function VendorExists(ByVal sVendorId As String) As Boolean
        logger.Debug("VendorExists Entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim returnVal As Boolean = False

        ' Execute the function
        returnVal = CType(factory.ExecuteScalar("SELECT dbo.fn_VendorExists(" & sVendorId & ")"), Boolean)

        logger.Debug("VendorExists Exit")
        Return returnVal
    End Function

    Public Shared Function GetVendorTransmissionType(ByVal sVendorId As String) As Integer
        logger.Debug("GetVendorTransmissionType Entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        ' Execute the function
        GetVendorTransmissionType = CType(factory.ExecuteScalar("SELECT dbo.fn_GetVendorTransmissionType(" & sVendorId & ")"), Integer)

        If GetVendorTransmissionType = -1 Then GetVendorTransmissionType = ConfigurationServices.AppSettings("DefaultPOTransmission")

        logger.Debug("GetVendorTransmissionType Exit")
    End Function

    Public Shared Function GetVendorPSVendorId(ByVal sVendorId As String) As String
        logger.Debug("GetVendorPSVendor_Id Entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        ' Execute the function
        GetVendorPSVendorId = CType(factory.ExecuteScalar("SELECT dbo.fn_GetVendorPSVendorId(" & sVendorId & ")"), String)

        logger.Debug("GetVendorPSVendor_Id Exit")
    End Function

    Public Shared Function IsOrderWindowOpen(ByVal sZoneId As String, ByVal sSubTeamNo As String, ByVal sStoreNo As String) As Boolean
        logger.Debug("IsOrderWindowOpen Entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim returnVal As String = ""

        ' Execute the function
        returnVal = CType(factory.ExecuteScalar("SELECT dbo.fn_IsOrderWindowOpen(" & sZoneId & ", " & sSubTeamNo & ", " & sStoreNo & ")"), String)

        If returnVal = "" Then
            Return False
        Else
            Return True
        End If

        logger.Debug("IsOrderWindowOpen Exit")
    End Function

    '**********************************************************************************
    Public Shared Function GetVendorPhoneNumber(ByVal iVendorId As Integer) As String
        logger.Debug("GetVendorPhoneNumber Entry")

        Dim rsVendor As DAO.Recordset = Nothing

        GetVendorPhoneNumber = ""

        Try
            SQLOpenRS(rsVendor, "SELECT Phone FROM dbo.Vendor WHERE Vendor_ID = " & iVendorId, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            While Not rsVendor.EOF
                GetVendorPhoneNumber = IIf(IsDBNull(rsVendor.Fields("Phone").Value), "", rsVendor.Fields("Phone").Value.ToString)
                rsVendor.MoveNext()
            End While
        Catch ex As Exception
            logger.Error("GetVendorPhoneNumber failed" & ex.Message)
        End Try

        logger.Debug("GetVendorPhoneNumber Exit")
    End Function

    Public Shared Function GetStoreZone(ByVal iStoreNo As Integer) As String
        logger.Debug("GetStoreZone Entry")

        Dim rsStore As DAO.Recordset = Nothing

        GetStoreZone = ""

        Try
            SQLOpenRS(rsStore, "SELECT Zone_ID FROM dbo.Store WHERE Store_No = " & iStoreNo, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            While Not rsStore.EOF
                GetStoreZone = IIf(IsDBNull(rsStore.Fields("Zone_ID").Value), "", rsStore.Fields("Zone_ID").Value.ToString)
                rsStore.MoveNext()
            End While
        Catch ex As Exception
            logger.Error("GetStoreZone failed" & ex.Message)
        End Try

        logger.Debug("GetStoreZone Exit")
    End Function

    Private Function ValidateCopyPOItems(ByVal iOrderHeader_ID As Integer, ByVal iCopyToStoreNo As Integer, ByVal bIsDeleted As Boolean) As String
        Dim rsInvalidCopyPOItems As DAO.Recordset = Nothing
        Dim sError_ItemNotOnVendor As String
        Dim sError_ItemNotAuthorized As String
        Dim sError_ItemIsDiscontinued As String
        Dim sError_ItemIsNotAvailable As String
        Dim iCopyPODaysToKeep As Integer

        sError_ItemNotOnVendor = ""
        sError_ItemNotAuthorized = ""
        sError_ItemIsDiscontinued = ""
        sError_ItemIsNotAvailable = ""
        ValidateCopyPOItems = ""
        iCopyPODaysToKeep = 0

        If Not IsNumeric(ConfigurationServices.AppSettings("CopyPODaysToKeep")) Then
            MsgBox("The CopyPODaysToKeep configuration value is not numeric.  This value must be numeric.  Please contact your regional IRMA developer to have this resolved.", MsgBoxStyle.Critical, "IRMA Copy PO")
            ValidateCopyPOItems = "ERROR"
            Exit Function
        Else
            iCopyPODaysToKeep = ConfigurationServices.AppSettings("CopyPODaysToKeep") * -1
        End If

        SQLOpenRS(rsInvalidCopyPOItems, "EXEC ValidateCopyPOItems " & iOrderHeader_ID & ", " & iCopyPODaysToKeep & ", " & iCopyToStoreNo & ", " & IIf(bIsDeleted, "True", "False"), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        If Not rsInvalidCopyPOItems.EOF Then
            rsInvalidCopyPOItems.MoveFirst()

            Me.InvalidCopyPOItems_ID = rsInvalidCopyPOItems.Fields("InvalidCopyPOItems_ID").Value
            Me.CopyPO_OrderHeaderID = rsInvalidCopyPOItems.Fields("Copy_To_PO").Value

            Do While Not rsInvalidCopyPOItems.EOF
                If Not IsDBNull(rsInvalidCopyPOItems.Fields("ReasonType_ID").Value) Then
                    Select Case rsInvalidCopyPOItems.Fields("ReasonType_ID").Value
                        Case 1
                            If sError_ItemNotOnVendor = "" Then sError_ItemNotOnVendor = "The following Items are no longer associated to " & Me.VendorName & ":"
                            sError_ItemNotOnVendor = sError_ItemNotOnVendor & vbCrLf & "   " & rsInvalidCopyPOItems.Fields("Identifier").Value & ", " & rsInvalidCopyPOItems.Fields("Item_Description").Value & ", " & rsInvalidCopyPOItems.Fields("Brand_Name").Value
                        Case 2
                            If sError_ItemNotAuthorized = "" Then sError_ItemNotAuthorized = "The following items are no longer authorized for " & Me.StoreNo & ":" & vbCrLf
                            sError_ItemNotAuthorized = sError_ItemNotAuthorized & vbCrLf & "   " & rsInvalidCopyPOItems.Fields("Identifier").Value & ", " & rsInvalidCopyPOItems.Fields("Item_Description").Value & ", " & rsInvalidCopyPOItems.Fields("Brand_Name").Value
                        Case 3
                            If sError_ItemIsDiscontinued = "" Then sError_ItemIsDiscontinued = "The following items are marked as Discontinued for the selected store and cannot be added to a purchase order: " & vbCrLf
                            sError_ItemIsDiscontinued = sError_ItemIsDiscontinued & vbCrLf & "   " & rsInvalidCopyPOItems.Fields("Identifier").Value & ", " & rsInvalidCopyPOItems.Fields("Item_Description").Value & ", " & rsInvalidCopyPOItems.Fields("Brand_Name").Value
                        Case 4
                            If sError_ItemIsNotAvailable = "" Then sError_ItemIsNotAvailable = "The following items are marked as Not Available and cannot be added to a distribution order: " & vbCrLf
                            sError_ItemIsNotAvailable = sError_ItemIsNotAvailable & vbCrLf & "   " & rsInvalidCopyPOItems.Fields("Identifier").Value & ", " & rsInvalidCopyPOItems.Fields("Item_Description").Value & ", " & rsInvalidCopyPOItems.Fields("Brand_Name").Value
                    End Select
                End If

                rsInvalidCopyPOItems.MoveNext()
            Loop
        End If

        If sError_ItemNotOnVendor <> "" Then
            ValidateCopyPOItems = ValidateCopyPOItems & sError_ItemNotOnVendor
        End If

        If sError_ItemNotAuthorized <> "" Then
            ValidateCopyPOItems = ValidateCopyPOItems & vbCrLf & sError_ItemNotAuthorized
        End If

        If sError_ItemIsDiscontinued <> "" Then
            ValidateCopyPOItems = ValidateCopyPOItems & vbCrLf & sError_ItemIsDiscontinued
        End If

        If sError_ItemIsNotAvailable <> "" Then
            ValidateCopyPOItems = ValidateCopyPOItems & vbCrLf & sError_ItemIsNotAvailable
        End If
    End Function

    ''' <summary>
    '''  This method adds a new item to an order, saving the change to the database.
    ''' </summary>
    ''' <param name="sItemKey_QuantityOrdered"></param>
    ''' <param name="iOrderHeader_ID"></param>
    ''' <remarks></remarks>
    Private Sub AddNewItem(ByRef sItemKey_QuantityOrdered As String, ByVal iOrderHeader_ID As Integer)
        logger.Debug("AddNewItem Entry")

        Dim rsOrderItem As DAO.Recordset
        Dim sLandedCost As Decimal
        Dim sLineItemCost As Decimal
        Dim sLineItemFreight As Decimal
        Dim sUnitCost As Decimal
        Dim sUnitExtCost As Decimal
        Dim sDiscountCost As Decimal
        Dim sMarkupCost As Decimal
        Dim sMarkupPercent As Decimal
        Dim sCost As Decimal
        Dim sFreight As Decimal
        Dim sQuantityOrdered As Decimal
        Dim iCost_Unit As Integer
        Dim iFreight_Unit As Integer
        Dim iQuantityUnit As Integer
        Dim decVendorDiscountAmt As Decimal
        Dim iItem_Key As Integer
        Dim sCreditReason_ID As String
        Dim sData() As String

        sData = Split(sItemKey_QuantityOrdered, "^")
        iItem_Key = CInt(sData(0))
        sQuantityOrdered = sData(1)
        sCreditReason_ID = sData(2)

        '-- Get the last occurances information
        rsOrderItem = SQLOpenRecordSet("EXEC AutomaticOrderItemInfo " & iItem_Key & ", " & iOrderHeader_ID & ", NULL", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        Dim cu As tItemUnit
        Dim fu As tItemUnit
        Dim lIgnoreErrNum(0) As Integer

        With rsOrderItem


            iQuantityUnit = .Fields("QuantityUnit").Value
            If Not IsDBNull(.Fields("VendorNetDiscount").Value) Then
                decVendorDiscountAmt = .Fields("VendorNetDiscount").Value
            Else
                decVendorDiscountAmt = 0
            End If

            '-- Pre markup Cost
            '20100427 - Dave Stacey - TFS 12604 - set this variable to the original cost instead of the calculated cost from the recordset
            sCost = IIf(IsDBNull(.Fields("OriginalCost").Value), 0, .Fields("OriginalCost").Value)

            If IsDBNull(.Fields("CostUnit").Value) Then
                logger.Warn("AddNewItem - Unable to add the selected item to the order because it does not have a Cost Unit ID assigned: iItem_Key=" + iItem_Key.ToString + ", OrderHeader_ID=" + iOrderHeader_ID.ToString)
                MsgBox("BAD DATA: This item has no Cost Unit ID assigned to it. This must be assigned in order to add it to the order." &
                        vbCrLf & vbCrLf & "Please close the empty Line Item Information screen that follows.", MsgBoxStyle.Critical)
                rsOrderItem.Close()
                logger.Debug("AddNewItem Exit")
                Exit Sub
            Else
                iCost_Unit = .Fields("CostUnit").Value
            End If

            Select Case .Fields("DiscountType").Value
                Case 0
                    If decVendorDiscountAmt > 0 Then
                        sDiscountCost = ((sCost * sQuantityOrdered) - (decVendorDiscountAmt * sQuantityOrdered)) / sQuantityOrdered
                    ElseIf Me.OrderDiscountExists And CopyPO_OrderHeaderID = 0 Then
                        Select Case Me.OrderDiscountType
                            Case 1 : sDiscountCost = sCost - Me.OrderDiscountAmt
                            Case 2 : sDiscountCost = sCost - (sCost * (Me.OrderDiscountAmt / 100))
                            Case 4 : sDiscountCost = sCost - (sCost * (Me.OrderDiscountAmt / 100))
                        End Select
                    Else
                        sDiscountCost = sCost
                    End If
                Case 1 : sDiscountCost = sCost - .Fields("QuantityDiscount").Value
                    'Case 2 : sDiscountCost = sCost * ((100 - .Fields("QuantityDiscount").Value) / 100)
                Case 2 : sDiscountCost = sCost - (sCost * (.Fields("QuantityDiscount").Value / 100))


                Case Else : sDiscountCost = sCost
            End Select
            sLineItemCost = CostConversion(sDiscountCost, iCost_Unit, iQuantityUnit, .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0) * sQuantityOrdered

            '-- Pre markup Freight
            sFreight = .Fields("Freight").Value
            iFreight_Unit = .Fields("FreightUnit").Value
            sLineItemFreight = CostConversion(sFreight, iFreight_Unit, iQuantityUnit, .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0) * sQuantityOrdered

            sLandedCost = (sLineItemCost + sLineItemFreight) / sQuantityOrdered

            '-- Markup
            cu = GetItemUnit(iCost_Unit)
            fu = GetItemUnit(iFreight_Unit)

            sLineItemCost = CostConversion(sDiscountCost * (.Fields("MarkupPercent").Value + 100) / 100, iCost_Unit, iQuantityUnit, .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0) * sQuantityOrdered
            sLineItemFreight = CostConversion(.Fields("Freight").Value * (.Fields("MarkupPercent").Value + 100) / 100, iFreight_Unit, iQuantityUnit, .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0) * sQuantityOrdered
            sUnitCost = CostConversion(sLineItemCost / sQuantityOrdered, iQuantityUnit, IIf(cu.IsPackageUnit, giUnit, iCost_Unit), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0)
            sUnitExtCost = CostConversion((sLineItemCost + sLineItemFreight) / sQuantityOrdered, iQuantityUnit, IIf(fu.IsPackageUnit, giUnit, iFreight_Unit), .Fields("Package_Desc1").Value, .Fields("Package_Desc2").Value, .Fields("Package_Unit_ID").Value, 0, 0)
            ' TFS 11405, 2/25/2010, v3.5.9, Tom Lux: Added handling charge to markupCost value; matches order-list screen logic.
            sMarkupCost = ((sLineItemCost + sLineItemFreight) / sQuantityOrdered) + .Fields("HandlingCharge").Value

            lIgnoreErrNum(0) = 50002

            On Error Resume Next
            SQLExecute3("EXEC InsertOrderItemCredit " &
                         iOrderHeader_ID & ", " &
                         iItem_Key & ", " &
                         .Fields("Units_Per_Pallet").Value & ", " &
                         iQuantityUnit & ", " &
                         sQuantityOrdered & ", " &
                         sDiscountCost & ", " &
                         iCost_Unit & ", " &
                         0 & ", " &
                         .Fields("HandlingUnit").Value & ", " &
                         sFreight & ", " &
                         iFreight_Unit & ", " &
                         .Fields("AdjustedCost").Value & ", " &
                         .Fields("QuantityDiscount").Value & ", " &
                         .Fields("DiscountType").Value & ", " &
                         sLandedCost & ", " &
                         sLineItemCost & ", " &
                         sLineItemFreight & ", " &
                         0 & ", " & sUnitCost & ", " &
                         sUnitExtCost & ", " &
                         .Fields("Package_Desc1").Value & ", " &
                         .Fields("Package_Desc2").Value & ", " &
                         .Fields("Package_Unit_ID").Value & ", " &
                         .Fields("MarkupPercent").Value & ", " &
                         sMarkupCost & ", " &
                         .Fields("Retail_Unit_ID").Value & ", " &
                         IIf(sCreditReason_ID = "NONE", "NULL", sCreditReason_ID) & ", " &
                         giUserID & ", " &
                         decVendorDiscountAmt & ", " & IIf(IsDBNull(.Fields("HandlingCharge").Value), 0, .Fields("HandlingCharge").Value),
                         DAO.RecordsetOptionEnum.dbSQLPassThrough,
                         lIgnoreErrNum)

            If Err.Number <> 0 Then
                logger.Error("AddNewItem " & "Error in EXEC InsertOrderItemCredit " & Err.Description)
                MsgBox(Err.Description, MsgBoxStyle.Exclamation, Me.Text)
            End If
            On Error GoTo 0
        End With

        rsOrderItem.Close()

        logger.Debug("AddNewItem Exit")
    End Sub

    Private Sub CopyPOItems(ByVal iOrderHeader_ID As Integer, ByVal iInvalidCopyPOItems_Id As Integer, ByVal dExpectedDate As Date, ByVal iUser_ID As Integer, ByVal iCopyToStoreNo As Integer)
        Dim rsCopyPOItems As DAO.Recordset = Nothing
        Dim sPOItems() As String
        Dim x As Integer = 0

        sPOItems = OrderingDAO.CopyExistingPO(iOrderHeader_ID, iInvalidCopyPOItems_Id, dExpectedDate, iUser_ID, iCopyToStoreNo, IIf(Me.IsDeleted, True, False))

        For x = 0 To UBound(sPOItems)
            AddNewItem(sPOItems(x), Me.CopyPO_OrderHeaderID)
        Next

        'Populate the OrderedCost for the newly created PO
        SQLExecute(String.Format("UpdateOrderHeaderCosts {0}", Me.CopyPO_OrderHeaderID), DAO.RecordsetOptionEnum.dbSQLPassThrough)
    End Sub

    Private Sub DoPOCopy()
        Dim frmExpectedDate As New frmCopyPO_ExpectedDate
        Dim dExpectedDate As Date
        Dim iCopyToStoreNo As Integer
        Dim sMessage As String

        Me.InvalidCopyPOItems_ID = 0

        frmExpectedDate.CopyFromStoreNo = Me.StoreNo
        frmExpectedDate.LimitStoreNo = glRecvLog_Store_Limit
        If Not frmExpectedDate.ShowDialog() = Windows.Forms.DialogResult.OK Then
            ' The user canceled the copy PO operation.  Exit this method.
            Exit Sub
        End If

        dExpectedDate = frmExpectedDate.ExpectedDate
        iCopyToStoreNo = frmExpectedDate.CopyToStoreNo
        frmExpectedDate.Dispose()

        If VendorExists(CStr(Me.VendorID)) Then
            sMessage = ValidateCopyPOItems(Me.OrderHeader_ID, iCopyToStoreNo, IIf(Me.IsDeleted, True, False))

            If sMessage = "ERROR" Then
                Exit Sub
            ElseIf sMessage <> String.Empty Then
                If MsgBox(sMessage & vbCrLf & vbCrLf & "Do you wish to continue?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "IRMA Copy PO") = MsgBoxResult.Yes Then
                    CopyPOItems(Me.OrderHeader_ID, Me.InvalidCopyPOItems_ID, dExpectedDate, giUserID, iCopyToStoreNo)
                Else
                    Exit Sub
                End If
            Else
                CopyPOItems(Me.OrderHeader_ID, Me.InvalidCopyPOItems_ID, dExpectedDate, giUserID, iCopyToStoreNo)
            End If
        Else
            MsgBox("Vendor " & m_sVendorName & " has been removed from IRMA.  This PO cannot be copied.", MsgBoxStyle.Critical, "IRMA Copy PO")
            Exit Sub
        End If

        ' [TFS 11474] Unlock copy-source order before reloading order screen with new copy of PO.
        UnlockPO()

        MsgBox("PO " & Me.CopyPO_OrderHeaderID & " has been created.", MsgBoxStyle.Information, "Copy PO")
        RefreshDataSource(Me.CopyPO_OrderHeaderID)

    End Sub

    Private Sub RefreshOrderSendDetails(ByVal iVendorID As Integer)
        logger.Debug("RefreshOrderSendDetails Entry")

        m_rsOrderSend = SQLOpenRecordSet("GetOrderSendInfo " & iVendorID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        logger.Debug("Manual = " & frmOrderSend.Manual.ToString)

        If Not IsDBNull(m_rsOrderSend.Fields("Fax").Value) Then
            m_sFax = m_rsOrderSend.Fields("Fax").Value
        Else
            m_sFax = String.Empty
        End If

        If Not IsDBNull(m_rsOrderSend.Fields("Email").Value) Then
            m_sEmail = m_rsOrderSend.Fields("Email").Value
        Else
            m_sEmail = String.Empty
        End If

        m_rsOrderSend.Close()

        logger.Debug("RefreshOrderSendDetails Exit")
    End Sub

    Private Sub cmdUndoWarehouseSend_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUndoWarehouseSend.Click
        Dim CancelOrder As Int16 = 0
        SQLExecute(String.Format("UpdateOrderResetWarehouseSent {0},{1}", m_lOrderHeader_ID, CancelOrder), DAO.RecordsetOptionEnum.dbSQLPassThrough)
        RefreshDataSource(m_lOrderHeader_ID)
    End Sub

    Private Sub SendElectronicOrder()
        Dim svc As New ElectronicOrderWebService
        Dim sResult As String
        Dim aryResult() As String
        Dim sRegionalIRMASupport, sDVOSupportEmail, sNotificationEmailAddress As String
        Dim sPOItemData As String

        Cursor = Cursors.WaitCursor

        Try
            sPOItemData = GetPOItemData()

            If sPOItemData <> String.Empty Then
                sResult = svc.saveItemData(sPOItemData) 'result comes back as ErrorCodeNumber|ErrorDescription|DVOPONumber
            Else
                SQLExecute("EXEC RollbackElectronicOrder " & m_lOrderHeader_ID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                Cursor = Cursors.Arrow
                Exit Sub
            End If
        Catch ex As Exception
            sResult = String.Empty
        End Try

        aryResult = Split(sResult, "|")
        sRegionalIRMASupport = ConfigurationServices.AppSettings("Error_ToEmailAddress")
        sDVOSupportEmail = ConfigurationServices.AppSettings("DVOSupportEmailAddress")

        If sDVOSupportEmail <> String.Empty Then
            sNotificationEmailAddress = sDVOSupportEmail
        Else
            sNotificationEmailAddress = sRegionalIRMASupport
        End If

        Select Case aryResult(0)
            Case 0
                MsgBox("Your PO has been successfully sent to DVO (DVO PO# " & aryResult(2) & ").  DVO will notify you with an e-mail when it has been delivered to the vendor.  This PO will remain locked until that time.", MsgBoxStyle.Information, "IRMA Electronic Ordering")
            Case -1
                MsgBox("There was an issue sending the PO to DVO.  This vendor requires an account set up in DVO and the account is missing.  Please contact your regional DVO support team at " & sNotificationEmailAddress & " to get this resolved.", MsgBoxStyle.Exclamation, "IRMA Electronic Ordering")
                SQLExecute("EXEC RollbackElectronicOrder " & m_lOrderHeader_ID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Case -2
                MsgBox("There was an issue sending the PO to DVO due to a data error with one of the items on your PO.  Please check the items and resend the PO.  If the problem continues please contact your regional DVO support team at " & sNotificationEmailAddress & " to get this resolved.", MsgBoxStyle.Exclamation, "IRMA Electronic Ordering")
                SQLExecute("EXEC RollbackElectronicOrder " & m_lOrderHeader_ID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Case -3
                MsgBox("This PO has already been received and processed by DVO. Please do not resend it. If the problem continues please contact your regional DVO support team at " & sNotificationEmailAddress & " to get this resolved.", MsgBoxStyle.Exclamation, "IRMA Electronic Ordering")
                ' RDE TFS 3002 09182011
                ' if the order has already been sent and processed by dvo we dont want to mark it as NOT sent. 
                'SQLExecute("EXEC RollbackElectronicOrder " & m_lOrderHeader_ID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Case 999
                MsgBox("There was an issue sending the PO to DVO.  There is no matching vendor for the PeopleSoft Vendor ID " & GetVendorPSVendorId(Me.VendorID) & ".  Please contact your regional DVO support team at " & sNotificationEmailAddress & " to get this resolved.", MsgBoxStyle.Exclamation, "IRMA Electronic Ordering")
                SQLExecute("EXEC RollbackElectronicOrder " & m_lOrderHeader_ID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Case Else
                MsgBox("Unexpected error code sent by DVO web service.  Please try again later and if the problem continues contact your regional DVO support team at " & sNotificationEmailAddress & " to get this resolved.", MsgBoxStyle.Exclamation, "IRMA Electronic Ordering")
                SQLExecute("EXEC RollbackElectronicOrder " & m_lOrderHeader_ID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End Select

        Cursor = Cursors.Arrow
    End Sub

    Private Function GetPOItemData() As String
        Dim rsHeaderInfo As DAO.Recordset
        Dim rsItemInfo As DAO.Recordset
        Dim sItemInfo As String
        Dim sXML As String

        ' Create a regex to remove any characters that might cause problems with the XML
        Dim textCleaner As Regex = New Regex("[^a-zA-Z0-9 ]")

        If Not StoreSubTeamRelationshipExists(m_lStoreNo, m_lTransferToSubTeam) Then
            MsgBox("The Store/Subteam relationship is not set up for store " & cmbField(iOrderHeaderPurchaseLocation_ID).Text & " and subteam " & m_sSubTeamName & ".  Please contact your regional IRMA support team by emailing " & ConfigurationServices.AppSettings("Error_ToEmailAddress"), MsgBoxStyle.Information, "IRMA Electronic Ordering")
            GetPOItemData = String.Empty
            Return GetPOItemData
        End If

        sItemInfo = String.Empty

        rsHeaderInfo = SQLOpenRecordSet("GetElectronicOrderHeaderInfo " & Me.OrderHeader_ID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        rsItemInfo = SQLOpenRecordSet("GetElectronicOrderItemInfo " & Me.OrderHeader_ID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        'TFS 9157 AB added  System.Security.SecurityElement.Escape to fix possible error in XML
        'Same changes needs to be done in service library.
        ' Create XML order header information - clean the notes field
        sXML = "<order>" &
                  "<irma_po>" & Me.OrderHeader_ID & "</irma_po>" &
                  "<error_email>" & Escape(rsHeaderInfo.Fields("Email").Value.ToString()) & "</error_email>" &
                  "<success_email>" & Escape(rsHeaderInfo.Fields("Email").Value.ToString()) & "</success_email>" &
                  "<ps_number>" & rsHeaderInfo.Fields("PS_Vendor_ID").Value & "</ps_number>" &
                  "<req_receive_date>" & rsHeaderInfo.Fields("Expected_Date").Value & "</req_receive_date>" &
                  "<po_notes>" & Escape(rsHeaderInfo.Fields("OrderHeaderDesc").Value.ToString()) & "</po_notes>" &
                  "<business_unit>" & rsHeaderInfo.Fields("BusinessUnit_ID").Value & "</business_unit>" &
                  "<sub_team>" & rsHeaderInfo.Fields("SubTeam_No").Value & "</sub_team>" &
                  "<pos_dept>" & rsHeaderInfo.Fields("POS_Dept").Value & "</pos_dept>" &
                  "<external_system>" & Escape(rsHeaderInfo.Fields("description").Value.ToString()) & "</external_system>" &
                  "<external_order_id>" & rsHeaderInfo.Fields("OrderExternalSourceOrderId").Value & "</external_order_id>" &
                  "<iscredit>" & rsHeaderInfo.Fields("isCredit").Value & "</iscredit>" &
                  "<invoice_number>" & If(IsDBNull(rsHeaderInfo.Fields("InvoiceNumber")), String.Empty, rsHeaderInfo.Fields("InvoiceNumber").Value) & "</invoice_number>" &
                  "<items>"

        ' -- RE:  7/21  DropShip functionality not implemented yet.
        '"<dropship>" & IIf(rsHeaderInfo.Fields("DropShip").Value = 1, "1", "0") & "</dropship>" & _

        ' Loop through the items on the order and create the XML item records - clean the description and brand fields
        While Not rsItemInfo.EOF
            sItemInfo = sItemInfo & "<item upc='" & rsItemInfo.Fields("UPC").Value & "' " &
                           "vid='" & rsItemInfo.Fields("VID").Value & "' " &
                           "case_pack='" & CInt(rsItemInfo.Fields("Case_Pack").Value) & "' " &
                           "qty='" & CInt(rsItemInfo.Fields("Qty").Value) & "' " &
                           "unit_cost='" & rsItemInfo.Fields("Unit_Cost").Value & "' " &
                           "pack_size='" & rsItemInfo.Fields("Pack_Size").Value & "' " &
                           "item_uom='" & Trim(rsItemInfo.Fields("Item_UOM").Value) & "' " &
                           "description='" & Escape(textCleaner.Replace(Trim(rsItemInfo.Fields("Description").Value.ToString()), "")) & "' " &
                           "pos_dept='" & rsItemInfo.Fields("POS_Dept").Value & "' " &
                           "brand='" & textCleaner.Replace(Trim(rsItemInfo.Fields("Brand").Value), "") & "' " &
                           "case_uom='" & Trim(rsItemInfo.Fields("Case_UOM").Value) & "' " &
                           "item_uom_iscase='" & Trim(rsItemInfo.Fields("UOM_IsCase").Value) & "' " &
                           "credit_reason_id='" & rsItemInfo.Fields("CreditReason_Id").Value & "' " &
                           "credit_lot_no='" & rsItemInfo.Fields("Lot_No").Value & "' " &
                           "credit_expire_date='' credit_notes=''/>" ' credit_expire_date and credit_notes left blank. for future use?

            rsItemInfo.MoveNext()
        End While

        GetPOItemData = sXML & sItemInfo & "</items></order>"

        rsItemInfo.Close()
        rsItemInfo = Nothing

        rsHeaderInfo.Close()
        rsHeaderInfo = Nothing
    End Function

    Public Shared Function StoreSubTeamRelationshipExists(ByVal sStoreNo As String, ByVal sSubTeamNo As String) As Boolean
        logger.Debug("StoreSubTeamRelationshipExists Entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim returnVal As Boolean = False

        ' Execute the function
        returnVal = CType(factory.ExecuteScalar("SELECT dbo.fn_StoreSubTeamExists(" & sStoreNo & ", " & sSubTeamNo & ")"), Boolean)

        logger.Debug("StoreSubTeamRelationshipExists Exit")
        Return returnVal
    End Function

    Public Function GetInvoiceMethodDisplayText(ByVal iOrderHeaderId As Integer, ByVal bPayAgreedCost As Boolean, ByVal iEInvoiceId As Integer) As String
        ' There are currently two pieces of information supplied herein:
        ' 1) P2P or "Agreed Cost Vendor" status.
        ' 2) eInvoice status.

        ' NOTE: This function depends on the status of the e-Invoicing button and should only be called
        ' after that button is setup in the form load function.
        Dim payDisplayText As String = "Pay by Invoice"
        If bPayAgreedCost Then
            payDisplayText = "Pay by Agreed Cost"
        End If

        Dim eInvDisplayText As String = "eInvoice"
        If Not OrderingDAO.IsOrderEinvoice(iOrderHeaderId, iEInvoiceId) Then
            eInvDisplayText = "Paper Invoicing"
        End If

        If OrderingDAO.IsVendorEinvoice(iOrderHeaderId) Then
            eInvDisplayText = "EInvoice Vendor"
        End If

        Return "(" & payDisplayText & ", " & eInvDisplayText & ")"
    End Function

    Private Sub UnlockPO()
        Try
            ' ## Bug 9276  if not locked by another user, make sure its not locked by you before you leave.
            OrderValidationBO.Unlock(Me.OrderHeader_ID, giUserID)
        Catch ex As Exception
            Dim msg As String = String.Format("Unable to determine what user had edit privilege for PO '{0}', so the unlock process was skipped.", Me.OrderHeader_ID)
            logger.Error(msg, ex)
            ErrorDialog.HandleError(msg, ex, ErrorDialog.NotificationTypes.DialogAndEmail, "")
        End Try
    End Sub

    Private Function VerifyPOLockedForCurrentUserOnLoad() As Boolean
        ' Make sure the current user owns the lock on the PO.  Most of the time, the current user will have a lock.  If not, there's a little
        ' overhead because the biz object verifies the lock as well.
        Dim lockStatus As OrderValidationBO.POLockStatus = Nothing
        Try
            lockStatus = OrderValidationBO.GetPOLockStatus(OrderHeader_ID, giUserID)
        Catch ex As Exception
            Dim msg As String = String.Format("Unable to determine if PO '{0}' is available for editing.", OrderHeader_ID)
            logger.Error(msg, ex)
            ErrorDialog.HandleError(msg, ex, ErrorDialog.NotificationTypes.DialogAndEmail, "")
            Return False
        End Try
        ' If not locked, try to lock.
        If lockStatus = OrderValidationBO.POLockStatus.NoLock Then
            logger.WarnFormat("No PO lock found, PO={0}, Current user={1}.  Trying PO lock...", OrderHeader_ID, giUserID)
            ' Current user does not have a lock on this order, so try to establish one.  *Passing 3rd arg here saves a trip to DB, since we.
            Try
                OrderValidationBO.Lock(OrderHeader_ID, giUserID, -1)
            Catch ex As Exception
                Dim msg As String = "Unable to establish edit permission for this PO, so edits will not be allowed."
                logger.Error(msg, ex)
                ErrorDialog.HandleError(msg, ex, ErrorDialog.NotificationTypes.DialogAndEmail, "")
                Return False
            End Try
        ElseIf lockStatus = OrderValidationBO.POLockStatus.DifferentUserLock Then
            Dim msg As String = String.Format("PO '{0}' is already locked by another user, so edits will not be allowed.", OrderHeader_ID)
            logger.Error(msg)
            MessageBox.Show(msg, "Save PO", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Return False
        End If
        Return True
    End Function

    Private Function VerifyPOLockedForCurrentUser() As Boolean
        ' Make sure the current user owns the lock on the PO.  Most of the time, the current user will have a lock.  If not, there's a little
        ' overhead because the biz object verifies the lock as well.
        Dim lockStatus As OrderValidationBO.POLockStatus = Nothing
        Try
            lockStatus = OrderValidationBO.GetPOLockStatus(OrderHeader_ID, giUserID)
        Catch ex As Exception
            Dim msg As String = String.Format("Unable to determine if PO '{0}' is available for editing, so the save process must be aborted.", OrderHeader_ID)
            logger.Error(msg, ex)
            ErrorDialog.HandleError(msg, ex, ErrorDialog.NotificationTypes.DialogAndEmail, "")
            Return False
        End Try
        ' If not locked, try to lock.
        If lockStatus = OrderValidationBO.POLockStatus.NoLock Then
            logger.WarnFormat("No PO lock found, PO={0}, Current user={1}.  Trying PO lock...", OrderHeader_ID, giUserID)
            ' Current user does not have a lock on this order, so try to establish one.  *Passing 3rd arg here saves a trip to DB, since we.
            Try
                OrderValidationBO.Lock(OrderHeader_ID, giUserID, -1)
            Catch ex As Exception
                Dim msg As String = "Unable to establish edit permission for this PO, so the save process must be aborted."
                logger.Error(msg, ex)
                ErrorDialog.HandleError(msg, ex, ErrorDialog.NotificationTypes.DialogAndEmail, "")
                Return False
            End Try
        ElseIf lockStatus = OrderValidationBO.POLockStatus.DifferentUserLock Then
            Dim msg As String = String.Format("PO '{0}' is already locked by another user, so the save process must be aborted.", OrderHeader_ID)
            logger.Error(msg)
            MessageBox.Show(msg, "Save PO", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            Return False
        End If
        Return True
    End Function

    Private Sub chkShowVendorDescription_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkShowVendorDescription.CheckedChanged
        With ugrdItems.DisplayLayout.Bands(0)
            .Columns("Vendor_Item_Description").Hidden = Not chkShowVendorDescription.Checked
            .Columns("Item_Description").Hidden = chkShowVendorDescription.Checked
        End With
    End Sub

    Private Sub UpdateFormForDSDOrder(Optional ByVal credit As Boolean = False)
        Me.Text = "Receiving Document Information " + GetInvoiceMethodDisplayText(glOrderHeaderID, m_bPayAgreedCost, m_iEInvoiceId)
        _lblLabel_2.Text = "Recv Date :"

        ugrdItems.DisplayLayout.Bands(0).Columns("QuantityOrdered").Hidden = True
        ugrdItems.Text = "Received Items"
        lblStatus.Text = String.Empty
        AddStatusInfo(If(credit, "Credit, ", "") & "Receiving Document (" & m_rsOrder.Fields("Transfer_To_SubTeamName").Value & ")")
    End Sub

    Private Sub dtpExpectedDate_ValueChanged(sender As System.Object, e As System.EventArgs) Handles dtpExpectedDate.ValueChanged
        If IsInitializing Then
            Exit Sub
        End If

        If Not m_bLoading Then
            If dtpExpectedDate.Enabled Then
                m_bDataChanged = True
            End If

        End If
    End Sub

    Private Sub cmdItemsRefused_Click(sender As System.Object, e As System.EventArgs) Handles cmdItemsRefused.Click
        logger.Debug("cmdItemsRefused__Click Entry")

        Dim frm As New frmOrderItemsRefused

        If Not SaveData() Then Exit Sub
        RefreshDataSource(m_lOrderHeader_ID)

        glOrderHeaderID = m_lOrderHeader_ID

        frm.OrderHeaderId = m_lOrderHeader_ID
        frm.VendorName = txtField(iOrderHeaderCompanyName).Text

        frm.EInvoicingId = m_iEInvoiceId

        frm.ShowDialog()
        frm.Close()
        frm.Dispose()

        m_WasReceived = True
        RefreshDataSource(m_lOrderHeader_ID)

        logger.Debug("cmdItemsRefused_Click Exit")
    End Sub
End Class

Public Class srtComparer
    Implements IComparer

    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

        Dim xCell As UltraGridCell = DirectCast(x, UltraGridCell)
        Dim yCell As UltraGridCell = DirectCast(y, UltraGridCell)

        Dim xArray() As String = Split(Trim(xCell.Row.Cells("QuantityOrdered").Value))
        Dim yArray() As String = Split(Trim(yCell.Row.Cells("QuantityOrdered").Value))

        Dim d As Double = IIf(String.IsNullOrEmpty(xArray(0)), 0, CDbl(xArray(0)))
        Return d.CompareTo(IIf(String.IsNullOrEmpty(yArray(0)), 0, CDbl(yArray(0))))

    End Function

End Class
