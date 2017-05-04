Option Strict Off
Option Explicit On

Imports System.Data.DataTable
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common.DataAccess
Imports Infragistics.Win.UltraWinGrid
Imports Infragistics.Win.UltraWinDataSource

Imports WholeFoods.IRMA.Ordering.DataAccess
Imports WholeFoods.IRMA.Ordering.BusinessLogic
Imports WholeFoods.IRMA.Administration.ReasonCodes.BusinessLogic

Imports log4net
Imports System.Windows.Forms

Friend Class frmOrderItemsRefused

    Dim daItemCatalog As New System.Data.SqlClient.SqlDataAdapter

    Private m_OrderHeaderId As Integer = 0
    Private m_VendorName As String = String.Empty
    Private m_EInvoicingId As Integer = 0
    Private RListDAO As New ReceivingListDAO
    Private dtOrderItemsRefused As New DataTable
    Dim rsOrder As DAO.Recordset = Nothing
    Private pbUploaded As Boolean
    Private pbClosed As Boolean
    Private canEdit As Boolean = False
    Private dtInfo As New DataTable
    Private savedChanges As Boolean
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Property OrderHeaderId() As Integer
        Get
            OrderHeaderId = m_OrderHeaderId
        End Get
        Set(ByVal value As Integer)
            m_OrderHeaderId = value
        End Set
    End Property

    Public Property VendorName() As String
        Get
            VendorName = m_VendorName
        End Get
        Set(ByVal value As String)
            m_VendorName = value
        End Set
    End Property

    Public Property EInvoicingId() As Integer
        Get
            EInvoicingId = m_EInvoicingId
        End Get
        Set(ByVal value As Integer)
            m_EInvoicingId = value
        End Set
    End Property

    Private Sub DisplayHeaderInfo()
        txtOrderHeader_ID.Text = m_OrderHeaderId
        txtVendorName.Text = m_VendorName
    End Sub

    Private Sub LoadPOInformation()

        '-- Retreive Order Info
        logger.Info("Calling GetOrderStatus to populate the OrderStatus.vb form: OrderHeader_ID=" + glOrderHeaderID.ToString)
        rsOrder = SQLOpenRecordSet("GetOrderStatus " & glOrderHeaderID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        pbClosed = Not IsDBNull(rsOrder.Fields("CloseDate").Value)
        pbUploaded = Not (IsDBNull(rsOrder.Fields("UploadedDate").Value) And IsDBNull(rsOrder.Fields("AccountingUploadDate").Value))

    End Sub

    Private Sub SetPermissions()
        'Determine the flag whether the user can edit the values in the grid
        canEdit = Not pbClosed And Not pbUploaded And (gbDistributor Or gbSuperUser)

        'Disable buttons if PO is not uploaded or the user is not buyer or superuser
        SetActive(cmdAdd, canEdit)
        SetActive(cmdDelete, canEdit)
        SetActive(cmdExit, canEdit)
    End Sub

    Private Sub frmOrderRefused_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmOrderRefused_Load Entry")

        CenterForm(Me)
        LoadPOInformation()

        SetPermissions()

        DisplayHeaderInfo()

        initGrid()

        PopGrid()

        LoadReceivingDiscrepanyReasonCodes()

        logger.Debug("frmOrderRefused_Load Entry")
    End Sub

    Private Sub initGrid()
        gridRefused.DisplayLayout.Bands(0).Columns("OrderItemRefusedID").Hidden = True
        gridRefused.DisplayLayout.Bands(0).Columns("eInvoiceException").Hidden = True
        gridRefused.DisplayLayout.Bands(0).Columns("UserAddedEntry").Hidden = True
    End Sub

    Private Sub PopGrid(Optional ByRef bRefreshData As Boolean = True)
        logger.Debug("PopGrid Entry")

        Dim iLoop As Integer

        If bRefreshData Then
            Me.dtOrderItemsRefused.Rows.Clear()
            dtOrderItemsRefused = RListDAO.GetRefusedItemList(glOrderHeaderID)
        End If

        udsRefusedList.Rows.Clear()
        Dim totalrows As Integer = dtOrderItemsRefused.Rows.Count

        If totalrows > 0 Then
            udsRefusedList.Rows.SetCount(totalrows)
        End If

        Dim dr As DataRow
        'Populate excess quantity rows
        For Each dr In dtOrderItemsRefused.Rows
            iLoop = iLoop + 1
            PopulateRefusedGridRow(dr, iLoop)
        Next dr

        CalculateTotals()

        logger.Debug("PopGrid Exit")
    End Sub

    Private Sub PopulateRefusedGridRow(ByVal dr As DataRow, ByVal index As Integer)

        logger.Debug("PopulateRefusedGridRow Entry")
        Dim row As UltraDataRow

        'Get the first row.
        row = Me.udsRefusedList.Rows(index - 1)

        row("OrderItemRefusedID") = dr.Item("OrderItemRefusedID")

        'If IsDBNull(dr.Item("eInvoice_Id")) Then
        '    row("eInvoiceException") = False
        'Else
        '    row("eInvoiceException") = IIf(CInt(dr.Item("eInvoice_Id")) = 0, False, True)
        'End If

        row("eInvoiceException") = IIf(CInt(dr.Item("eInvoiceException")) = 0, False, True)
        row("eInvoiceItem") = IIf(CInt(dr.Item("eInvoiceItem")) = 0, False, True)
        row("UserAddedEntry") = dr.Item("UserAddedEntry")

        ' If the item is an eInvoice exception, then GetOrderItemsRefused will not be able to properly populate eInvoiceQuantity and eInvoiceCost fields.
        ' Use the standard Invoice values for these two in that case.  Also, for these kinds of orders, the reason code should be set to II.
        If row("eInvoiceException") Then
            row("eInvoiceQuantity") = dr.Item("InvoiceQuantity")
            row("eInvoiceCost") = dr.Item("InvoiceCost")
            row("InvoiceQuantity") = Nothing
            row("InvoiceCost") = Nothing

        ElseIf row("eInvoiceItem") Then
            ' Otherwise, if the item is an eInvoice item but not an exception, populate the eInvoice fields of the grid.
            row("eInvoiceQuantity") = dr.Item("eInvoiceQuantity")
            row("eInvoiceCost") = dr.Item("eInvoiceCost")
            row("InvoiceQuantity") = Nothing
            row("InvoiceCost") = Nothing

        Else
            ' The item is not an eInvoice exception or a normal eInvoice item.  Populate the regular invoice fields.
            row("eInvoiceQuantity") = Nothing
            row("eInvoiceCost") = Nothing
            row("InvoiceQuantity") = dr.Item("InvoiceQuantity")
            row("InvoiceCost") = dr.Item("InvoiceCost")
        End If

        row("Identifier") = dr.Item("Identifier")
        row("VendorItemNumber") = dr.Item("VendorItemNumber")
        row("Description") = dr.Item("Description")
        row("Unit") = Trim(dr.Item("Unit").ToString.ToUpper)
        row("QuantityReceived") = dr.Item("QuantityReceived")
        row("QuantityOrdered") = dr.Item("QuantityOrdered")
        row("RefusedQuantity") = dr.Item("RefusedQuantity")
        row("Code") = dr.Item("DiscrepancyCodeID")
        row("DataChanged") = False

        logger.Debug("PopulateRefusedGridRow Exit")
    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub gridRefused_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles gridRefused.InitializeLayout
        e.Layout.Bands(0).Columns("Code").ValueList = Me.uddReasonCode
        e.Layout.Bands(0).Columns("Code").Style = Infragistics.Win.UltraWinGrid.ColumnStyle.DropDownList
    End Sub

    Private Sub gridRefused_BeforeCellActivate(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.CancelableCellEventArgs) Handles gridRefused.BeforeCellActivate

        If Not canEdit Then
            e.Cancel = True
            Exit Sub
        End If

        If e.Cell.Band.Index = 1 Then Exit Sub

        If e.Cell.Row.Cells("UserAddedEntry").Value = False Then
            If (e.Cell.Row.Cells("eInvoiceException").Value = True Or e.Cell.Row.Cells("eInvoiceItem").Value = True) And e.Cell.Column.Key <> "RefusedQuantity" Then
                e.Cancel = True
                Exit Sub
            End If
        Else
            If (e.Cell.Column.Key <> "RefusedQuantity" And e.Cell.Column.Key <> "Code" And e.Cell.Column.Key <> "InvoiceCost" And e.Cell.Column.Key <> "InvoiceQuantity") Then
                e.Cancel = True
                Exit Sub
            End If
        End If
    End Sub

    Private Sub gridRefused_AfterSelectChange(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.AfterSelectChangeEventArgs) Handles gridRefused.AfterSelectChange

        If (e.Type.Name <> "UltraGridRow") Then Exit Sub

        If gridRefused.Selected.Rows.Count < 1 Then
            Exit Sub
        End If

        If gridRefused.Selected.Rows(0).Band.Index = 1 Then Exit Sub
    End Sub

    Private Sub LoadReceivingDiscrepanyReasonCodes()
        Dim bo As ReasonCodeMaintenanceBO = New ReasonCodeMaintenanceBO
        Dim dt As DataTable = bo.getReasonCodeDetailsForType(enumReasonCodeType.RI.ToString)

        uddReasonCode.DataSource = dt
        uddReasonCode.DisplayMember = "ReasonCode"
        uddReasonCode.ValueMember = "ReasonCodeDetailID"

        Dim dr As DataRow = dt.NewRow()
        dr("ReasonCodeDetailID") = -1
        dr("ReasonCode") = ""
        dr("ReasonCodeDesc") = ""
        dr("ReasonCodeExtDesc") = ""
        dt.Rows.InsertAt(dr, 0)

        uddReasonCode.DisplayLayout.Bands(0).Columns("ReasonCode").Header.Caption = "Code"
        uddReasonCode.DisplayLayout.Bands(0).Columns("ReasonCodeDesc").Header.Caption = "Description"
        uddReasonCode.DisplayLayout.Bands(0).Columns("ReasonCode").Width = 50
        uddReasonCode.DisplayLayout.Bands(0).Columns("ReasonCodeDesc").Width = 200
        uddReasonCode.DisplayLayout.Bands(0).Columns("ReasonCodeDetailID").Hidden = True
        uddReasonCode.DisplayLayout.Bands(0).Columns("ReasonCodeExtDesc").Hidden = True
    End Sub

    Private Sub CalculateTotals()
        Dim total As Double = 0.0
        Dim cost As Double = 0.0

        If gridRefused.Rows.Count < 1 Then
            lblTotalRefusedAmount.Text = "0"
            Exit Sub
        End If

        For i As Integer = 0 To gridRefused.Rows.Count - 1
            cost = 0.0
            If Not (gridRefused.Rows(i).Cells("InvoiceCost").Value Is DBNull.Value) Then
                cost = CDbl(gridRefused.Rows(i).Cells("InvoiceCost").Value)
            ElseIf Not (gridRefused.Rows(i).Cells("eInvoiceCost").Value Is DBNull.Value) Then
                cost = CDbl(gridRefused.Rows(i).Cells("eInvoiceCost").Value)
            End If

            total = total + cost * CDbl(gridRefused.Rows(i).Cells("RefusedQuantity").Value)
        Next i

        lblTotalRefusedAmount.Text = VB6.Format((total), "##,###0.00##")
    End Sub

    Private Sub cmdSave_Click(sender As System.Object, e As System.EventArgs) Handles cmdSave.Click
        logger.Debug("cmdSave_Click Entry")

        Dim upc As String
        Dim rowDataChanged As Boolean

        If gridRefused.Selected.Rows.Count < 1 Then
            MsgBox("Please choose a line to save by selecting one of the rows.", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        Dim i As Integer = gridRefused.Selected.Rows(0).Index
        If i < 0 Then Exit Sub

        rowDataChanged = gridRefused.Rows(i).Cells("DataChanged").Value.ToString
        If Not rowDataChanged Then
            MsgBox("There are no pending changes to be saved for this item.", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        upc = gridRefused.Rows(i).Cells("Identifier").Value.ToString

        Dim refusedQuantity As Decimal = CDec(IIf(gridRefused.Rows(i).Cells("RefusedQuantity").Value Is System.DBNull.Value, 0, gridRefused.Rows(i).Cells("RefusedQuantity").Value))
        If refusedQuantity <= 0 Then
            MsgBox("Refused Quantity must be greater than 0.0 for UPC [" + upc + "]", MsgBoxStyle.Information, "Warning")
            Exit Sub
        End If

        Dim invoiceCost As Decimal = CDec(IIf(gridRefused.Rows(i).Cells("InvoiceCost").Value Is System.DBNull.Value, 0, gridRefused.Rows(i).Cells("InvoiceCost").Value))
        If invoiceCost <= 0 Then
            MsgBox("Invoice Cost must be greater than 0.0 for UPC [" + upc + "]", MsgBoxStyle.Information, "Warning")
            Exit Sub
        End If

        Dim orderItemRefusedID As String = gridRefused.Rows(i).Cells("OrderItemRefusedID").Value.ToString
        Dim updateSQLString As String = "EXEC UpdateOrderItemRefused " _
                                        & orderItemRefusedID & ", '" _
                                        & gridRefused.Rows(i).Cells("Identifier").Value.ToString & "', '" _
                                        & gridRefused.Rows(i).Cells("VendorItemNumber").Value.ToString & "', '" _
                                        & gridRefused.Rows(i).Cells("Description").Value.ToString & "', '" _
                                        & gridRefused.Rows(i).Cells("Unit").Value.ToString & "', " _
                                        & IIf(gridRefused.Rows(i).Cells("InvoiceCost").Value Is System.DBNull.Value, "null", gridRefused.Rows(i).Cells("InvoiceCost").Value.ToString) & ", " _
                                        & IIf(gridRefused.Rows(i).Cells("RefusedQuantity").Value Is System.DBNull.Value, "null", gridRefused.Rows(i).Cells("RefusedQuantity").Value.ToString) & ", " _
                                        & IIf(gridRefused.Rows(i).Cells("InvoiceQuantity").Value Is System.DBNull.Value, "null", gridRefused.Rows(i).Cells("InvoiceQuantity").Value.ToString) & ", " _
                                        & IIf(gridRefused.Rows(i).Cells("Code").Value Is System.DBNull.Value, "null", gridRefused.Rows(i).Cells("Code").Value) & ", " _
                                        & IIf(gridRefused.Rows(i).Cells("UserAddedEntry").Value, "1", "0")

        SQLExecute(updateSQLString, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        gridRefused.Rows(i).Cells("DataChanged").Value = False

        'Populate the OrderedCost for the newly created PO
        SQLExecute(String.Format("UpdateOrderHeaderCosts {0}", m_OrderHeaderId), DAO.RecordsetOptionEnum.dbSQLPassThrough)

        PopGrid()

        MsgBox("Changes have been saved.", MsgBoxStyle.Information, Me.Text)
        savedChanges = True

        logger.Debug("cmdSave_Click Exit")
    End Sub

    Private Sub cmdDelete_Click(sender As System.Object, e As System.EventArgs) Handles cmdDelete.Click
        logger.Debug("cmdDelete_Click Entry")

        Dim msg As String = String.Empty

        If gridRefused.Selected.Rows.Count < 1 Then
            Exit Sub
        End If

        Dim i As Integer = gridRefused.Selected.Rows(0).Index

        If gridRefused.Rows(i).Cells("UserAddedEntry").Value = True Then
            If MsgBox("Delete this item?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, "Refused Items") = MsgBoxResult.No Then
                Exit Sub
            End If

            Dim orderItemRefusedID As String = gridRefused.Rows(i).Cells("OrderItemRefusedID").Value.ToString
            Dim deleteSQLString As String = "EXEC DeleteOrderItemRefused " + orderItemRefusedID

            SQLExecute(deleteSQLString, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        Else
            MsgBox("This is not a user-added item and cannot be deleted.", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        'Populate the OrderedCost for the newly created PO
        SQLExecute(String.Format("UpdateOrderHeaderCosts {0}", m_OrderHeaderId), DAO.RecordsetOptionEnum.dbSQLPassThrough)

        PopGrid()

        logger.Debug("cmdDelete_Click Exit")
    End Sub

    Private Sub cmdAdd_Click(sender As System.Object, e As System.EventArgs) Handles cmdAdd.Click
        logger.Debug("cmdAdd_Click Entry")

        Dim frm As New frmAddRefusedItem

        frm.OrderHeaderId = m_OrderHeaderId

        frm.ShowDialog()

        'Populate the OrderedCost for the newly created PO
        SQLExecute(String.Format("UpdateOrderHeaderCosts {0}", m_OrderHeaderId), DAO.RecordsetOptionEnum.dbSQLPassThrough)

        PopGrid()

        logger.Debug("cmdAdd_Click Exit")
    End Sub

    Private Sub gridRefused_AfterCellUpdate(sender As System.Object, e As Infragistics.Win.UltraWinGrid.CellEventArgs) Handles gridRefused.AfterCellUpdate

        CalculateTotals()

        If e.Cell.Row.Cells("UserAddedEntry").Value = True Then
            e.Cell.Row.Appearance.BackColor = Color.LightGreen
            e.Cell.Row.Cells("RefusedQuantity").Appearance.BackColor = Color.LightGreen
            e.Cell.Row.Cells("InvoiceCost").Appearance.BackColor = Color.LightGreen
        ElseIf e.Cell.Row.Cells("UserAddedEntry").Value = False And e.Cell.Row.Cells("eInvoiceException").Value = True Then
            e.Cell.Row.Appearance.BackColor = Color.LightCoral
            e.Cell.Row.Cells("RefusedQuantity").Appearance.BackColor = Color.LightCoral
            e.Cell.Row.Cells("InvoiceCost").Appearance.BackColor = Color.LightCoral
        Else
            e.Cell.Row.Appearance.BackColor = Color.LightGray
            e.Cell.Row.Cells("RefusedQuantity").Appearance.BackColor = Color.LightGray
            e.Cell.Row.Cells("InvoiceCost").Appearance.BackColor = Color.LightGray
        End If

        If e.Cell.Column.Key = "RefusedQuantity" Then
            If CInt(e.Cell.Value) <= 0 Then
                e.Cell.Row.Cells("RefusedQuantity").Appearance.BackColor = Color.Red
            End If
        End If

        If e.Cell.Column.Key = "InvoiceCost" And e.Cell.Row.Cells("eInvoiceException").Value = False And e.Cell.Row.Cells("eInvoiceItem").Value = False Then
            If CInt(e.Cell.Value) <= 0 Then
                e.Cell.Row.Cells("InvoiceCost").Appearance.BackColor = Color.Red
            End If
        End If

        e.Cell.Row.Cells("DataChanged").Value = True

    End Sub

    Private Sub gridRefused_InitializeRow(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles gridRefused.InitializeRow
        If e.Row.Band.Index = 1 Then Exit Sub

        If e.Row.Cells("UserAddedEntry").Value = True Then
            e.Row.Appearance.BackColor = Color.LightGreen
        ElseIf e.Row.Cells("UserAddedEntry").Value = False And e.Row.Cells("eInvoiceException").Value = True Then
            e.Row.Appearance.BackColor = Color.LightCoral
        Else
            e.Row.Appearance.BackColor = Color.LightGray
        End If

        If CInt(IIf(IsDBNull(e.Row.Cells("RefusedQuantity").Value), 0, e.Row.Cells("RefusedQuantity").Value)) <= 0 Then
            e.Row.Cells("RefusedQuantity").Appearance.BackColor = Color.Red
        End If

        If CInt(IIf(IsDBNull(e.Row.Cells("InvoiceCost").Value), 0, e.Row.Cells("InvoiceCost").Value)) <= 0 _
            And e.Row.Cells("eInvoiceException").Value = False And e.Row.Cells("eInvoiceItem").Value = False Then

            e.Row.Cells("InvoiceCost").Appearance.BackColor = Color.Red
        End If

        ' If the row is an eInvoice item or an eInvoice exception, then all of the information is pre-populated, and none of the cells should be editable.
        If e.Row.Cells("eInvoiceException").Value = True Or e.Row.Cells("eInvoiceItem").Value = True Then
            e.Row.Activation = Activation.NoEdit
        End If

    End Sub

    Private Sub frmOrderItemsRefused_FormClosing(sender As System.Object, e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        logger.Debug("frmOrderItemsRefused_FormClosing Entry")

        Dim upc As String
        Dim rowDataChanged As Boolean

        If gridRefused.Rows.Count > 0 Then
            For i As Integer = 0 To gridRefused.Rows.Count - 1
                upc = gridRefused.Rows(i).Cells("Identifier").Value.ToString

                Dim refusedQuantity As Decimal = CDec(IIf(gridRefused.Rows(i).Cells("RefusedQuantity").Value Is System.DBNull.Value, 0, gridRefused.Rows(i).Cells("RefusedQuantity").Value))
                If refusedQuantity <= 0 Then
                    MsgBox("Refused Quantity must be greater than 0 for UPC [" + upc + "]", MsgBoxStyle.Information, "Warning")
                    e.Cancel = True
                    Exit Sub
                End If

                Dim invoiceCost As Decimal = CDec(IIf(gridRefused.Rows(i).Cells("InvoiceCost").Value Is System.DBNull.Value, 0, gridRefused.Rows(i).Cells("InvoiceCost").Value))
                If invoiceCost <= 0 And gridRefused.Rows(i).Cells("eInvoiceException").Value = False And gridRefused.Rows(i).Cells("eInvoiceItem").Value = False Then
                    MsgBox("Invoice Cost must be greater than 0 for UPC [" + upc + "]", MsgBoxStyle.Information, "Warning")
                    e.Cancel = True
                    Exit Sub
                End If

                rowDataChanged = gridRefused.Rows(i).Cells("DataChanged").Value.ToString
                If (rowDataChanged = True) Then
                    If (MsgBox("Exit without saving changes for UPC [" + upc + "]", MsgBoxStyle.YesNo + MsgBoxStyle.Exclamation, Me.Text) = MsgBoxResult.No) Then
                        e.Cancel = True
                        Exit Sub
                    End If
                End If
            Next i
        End If

        If savedChanges Then
            ' Changes were saved during this session.  This helps OrderStatus.vb to know if it needs to refresh its data.
            Me.DialogResult = Windows.Forms.DialogResult.OK
        Else
            ' No changes were made.
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
        End If
        logger.Debug("frmOrderItemsRefused_FormClosing Exit")
    End Sub

End Class