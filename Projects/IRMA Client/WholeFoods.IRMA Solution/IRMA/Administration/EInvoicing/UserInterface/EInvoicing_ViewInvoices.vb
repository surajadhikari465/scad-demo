Imports System.ComponentModel
Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid
Imports WholeFoods.IRMA.Administration.EInvoicing.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.Replenishment.EInvoicing.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.EInvoicing.DataAccess
Imports WholeFoods.Utility


Public Class EInvoicing_ViewInvoices

    Private _cell As UltraGridCell
    Private _element As Infragistics.Win.UIElement
    Private _mousepoint As Point
    Private _hElement As HeaderUIElement
    Private _rsElement As RowSelectorUIElement
    Private _redAppearance As Infragistics.Win.Appearance = New Infragistics.Win.Appearance()


    Private Class ReparseWorkitem
        Implements IEquatable(Of ReparseWorkitem)

        Private _einvoiceId As Integer
        Public Property EinvoiceId() As Integer
            Get
                Return _einvoiceId
            End Get
            Set(ByVal value As Integer)
                _einvoiceId = value
            End Set
        End Property

        Private _orderheaderid As String
        Public Property OrderHeaderId() As String
            Get
                Return _orderheaderid
            End Get
            Set(ByVal value As String)
                _orderheaderid = value
            End Set
        End Property

        Sub New(ByVal einvoiceid As Integer, ByVal orderheaderid As String)
            _orderheaderid = orderheaderid
            _einvoiceId = einvoiceid
        End Sub

        Public Overloads Function Equals(ByVal other As ReparseWorkitem) As Boolean Implements IEquatable(Of ReparseWorkitem).Equals
            If Me.EinvoiceId = other.EinvoiceId And Me.OrderHeaderId = other.OrderHeaderId Then
                Return True
            Else
                Return False
            End If
        End Function
    End Class


    Private Class ReparseProgressInfo

        Private _einvoiceid As Integer
        Public Property EinvoiceId() As Integer
            Get
                Return _einvoiceid
            End Get
            Set(ByVal value As Integer)
                _einvoiceid = value
            End Set
        End Property

        Private _OrderHeaderId As String
        Public Property OrderHeaderId() As String
            Get
                Return _OrderHeaderId
            End Get
            Set(ByVal value As String)
                _OrderHeaderId = value
            End Set
        End Property

        Private _TotalItems As Integer
        Public Property TotalItems() As Integer
            Get
                Return _TotalItems
            End Get
            Set(ByVal value As Integer)
                _TotalItems = value
            End Set
        End Property

        Private _CurrentItem As Integer
        Public Property CurrentItem() As Integer
            Get
                Return _CurrentItem
            End Get
            Set(ByVal value As Integer)
                _CurrentItem = value
            End Set
        End Property

        Private _State As String
        Public Property State() As String
            Get
                Return _State
            End Get
            Set(ByVal value As String)
                _State = value
            End Set
        End Property

        Private _ErrorInfo As Exception
        Public Property ErrorInfo As Exception
            Get
                Return _ErrorInfo
            End Get
            Set(ByVal value As Exception)
                _ErrorInfo = value
            End Set
        End Property

        Sub New(ByVal einvoiceid As Integer, ByVal OrderHeaderId As String, ByVal TotalItems As Integer, ByVal CurrentItem As Integer, ByVal State As String, ByVal ErrorInfo As Exception)
            _einvoiceid = einvoiceid
            _OrderHeaderId = OrderHeaderId
            _TotalItems = TotalItems
            _CurrentItem = CurrentItem
            _State = State
            _ErrorInfo = ErrorInfo
        End Sub

    End Class

    Private Sub ViewInvoices_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        _redAppearance.BackColor = Color.Crimson
        _redAppearance.FontData.Bold = Infragistics.Win.DefaultableBoolean.True

        LoadStores()
        LoadVendors()
        LoadEinvoiceErrorMessages()
        LoadEinvoiceStatuses()

        'set default Invoice Dates to past 30 days.
        UltraDateTimeEditor_InvoiceStart.DateTime = DateTime.Today.AddDays(-30)
        UltraDateTimeEditor_InvoiceEnd.DateTime = DateTime.Today.AddDays(1).AddTicks(-1)
    End Sub

    Private Sub formatData()
        With UltraGrid_EInvoices
            .DisplayLayout.AutoFitStyle = AutoFitStyle.ExtendLastColumn
            .DisplayLayout.Override.AllowDelete = Infragistics.Win.DefaultableBoolean.False
            .DisplayLayout.Bands(0).Override.CellClickAction = CellClickAction.RowSelect
            .Selected.Rows.Clear()
            .DisplayLayout.Override.ActiveRowAppearance.Reset()
            .DisplayLayout.Bands(0).Columns("Store_No").Hidden = True
            .DisplayLayout.Bands(0).Columns("ErrorCode_Id").Hidden = True
            .DisplayLayout.Bands(0).Columns("EInvoice_Id").Hidden = True
            .DisplayLayout.Bands(0).Columns("Archived").Hidden = True
            .DisplayLayout.Bands(0).Columns("ArchivedDate").Hidden = True
            .DisplayLayout.Appearance.FontData.SizeInPoints = 9
            .DisplayLayout.Appearance.FontData.Name = "Calibri"
        End With

        UltraGrid_EInvoices.DisplayLayout.AutoFitStyle = AutoFitStyle.ResizeAllColumns
    End Sub

    Private Sub refreshData(ByRef searchData As EinvoicingSearchBO)
        Dim da As EInvoicingDAO = New EInvoicingDAO

        UltraButton_Search.Enabled = False
        UltraButton_Search.Text = "Searching..."
        UltraButton_Search.Refresh()

        If validateSearchCriteria() Then

            With UltraGrid_EInvoices
                .DataSource = da.getEInvoices(searchData)
                .DataBind()
            End With
            UltraButton_Search.Text = "Formatting..."
            UltraButton_Search.Refresh()
            formatData()
        End If

        da.Dispose()
        UltraButton_Search.Text = "Search"
        UltraButton_Search.Enabled = True
        UltraButton_Search.Refresh()
    End Sub

    Private Sub EditSACCodesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim frm As EInvoicing_SAC_Edit = New EInvoicing_SAC_Edit
        frm.ShowDialog()
        frm.Dispose()
    End Sub

    Private Sub UltraGrid_MouseDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles UltraGrid_EInvoices.MouseDown
        _mousepoint = New Point(e.X, e.Y)
        _element = CType(sender, UltraGrid).DisplayLayout.UIElement.ElementFromPoint(_mousepoint)
        _cell = CType(_element.GetContext(GetType(UltraGridCell)), UltraGridCell)
        _hElement = CType(_element.GetAncestor(GetType(HeaderUIElement)), HeaderUIElement)
        _rsElement = CType(_element.GetAncestor(GetType(RowSelectorUIElement)), RowSelectorUIElement)
    End Sub

    Private Sub ContextMenu_Opening(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles ContextMenuStrip1.Opening
        Dim InvoiceNum As String = String.Empty

        If Not _hElement Is Nothing Then
            e.Cancel = True
        End If

        If Not _cell Is Nothing Then
            ArchiveSelectedToolStripMenuItem.Visible = Not _cell.Row.Cells("Status").Value.Equals("Archived")
            ArchiveSelectedToolStripMenuItem.Enabled = Not _cell.Row.Cells("Status").Value.Equals("Archived")
        End If

        If e.Cancel = False Then
            If Not _cell Is Nothing Then

                If UltraGrid_EInvoices.Selected.Rows.Count = 0 Then
                    _cell.Row.Selected = True
                End If

                ChangePOInformationToolStripMenuItem.Enabled = Not (UltraGrid_EInvoices.Selected.Rows.Count > 1)
                ReparseMenu_MenuItem.Enabled = (UltraGrid_EInvoices.Selected.Rows.Count > 0)

                Dim IsSuspended As Boolean
                Dim IsCorrectErrorCode As Boolean

                IsSuspended = _cell.Row.Cells("Status").Value.ToString.ToLower.Equals("suspended")
                IsCorrectErrorCode = IIf(IsDBNull(_cell.Row.Cells("ErrorCode_Id").Value), 0, _cell.Row.Cells("errorCode_Id").Value) = 102

                ' set access rules for new "Force Load" context menu item.
                ForceLoadToolStripMenuItem.Enabled = UltraGrid_EInvoices.Selected.Rows.Count = 1 _
                                                        And gbEInvoicing _
                                                        And IsSuspended _
                                                        And IsCorrectErrorCode

                ViewErrorInformationToolStripMenuItem.Enabled = (UltraGrid_EInvoices.Selected.Rows.Count = 1)

                If UltraGrid_EInvoices.Selected.Rows.Count <= 1 Then
                    UltraGrid_EInvoices.Selected.Rows.Clear()
                    _cell.Row.Selected = True
                    _cell.Row.Activate()
                    InvoiceNum = _cell.Row.Cells("Invoice").Value.ToString
                    InfoToolStripMenuItem.Text = String.Format("[ Invoice: {0} ]", InvoiceNum)
                    InfoToolStripMenuItem.Visible = True
                Else
                    InfoToolStripMenuItem.Text = String.Format("[ {0} invoice(s) selected. ]", UltraGrid_EInvoices.Selected.Rows.Count)
                End If

            Else
                InfoToolStripMenuItem.Visible = False
                ReparseMenu_MenuItem.Enabled = False
                ViewErrorInformationToolStripMenuItem.Enabled = False
                ChangePOInformationToolStripMenuItem.Enabled = False
                ArchiveSelectedToolStripMenuItem.Enabled = False
                ForceLoadToolStripMenuItem.Enabled = False
            End If
        End If
    End Sub

    Private Sub ReparseMenu_MenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ReparseMenu_MenuItem.Click

        Dim item As ListViewItem
        Dim rwi As ReparseWorkitem
        Dim ReparseWorkItemList As List(Of ReparseWorkitem) = New List(Of ReparseWorkitem)
        Dim cnt As Integer = 0
        Dim POSearchResult As DataTable

        ReparseWorkItemList.Clear()
        ListView_Reparse.Items.Clear()

        ' **** Only applies to reparsing DSD (GSS) Orders ****
        For Each row As UltraGridRow In UltraGrid_EInvoices.Selected.Rows
            Dim EInv As EInvoicingDAO = New EInvoicingDAO
            POSearchResult = EInv.GetOrderHeaderIDForDSDOrder(row.Cells("Invoice").Value.ToString, _
                                                              row.Cells("PurchaseOrder").Value.ToString, _
                                                              row.Cells("VendorID").Value.ToString)


            If IsNumeric(POSearchResult.Rows(0).Item("OrderHeader_ID")) And _
                POSearchResult.Rows(0).Item("OrderHeader_ID") > 0 And _
                POSearchResult.Rows(0).Item("InvoiceMatch") = True Then
                ' Only an invoice # match will trigger the update of the po_num field in EInvoicing_Invoices table,
                ' using the OrderHeader_ID from the receiving document.
                Dim DAO As EInvoicingDAO = New EInvoicingDAO
                DAO.UpdateInvoicePOInformation(row.Cells("Einvoice_id").Value, POSearchResult.Rows(0).Item("OrderHeader_ID").ToString, 0)
                row.Cells("PurchaseOrder").Value = POSearchResult.Rows(0).Item("OrderHeader_ID").ToString()
                DAO.Dispose()
            Else
                If POSearchResult.Rows(0).Item("ErrorFound") = True Then
                    ToolStripStatusLabel_Info.Text = POSearchResult.Rows(0).Item("Message")
                End If
            End If

            EInv.Dispose()
        Next
        ' ****

        For Each row As UltraGridRow In UltraGrid_EInvoices.Selected.Rows
            rwi = New ReparseWorkitem(row.Cells("Einvoice_id").Value, row.Cells("PurchaseOrder").Value.ToString)
            If Not ReparseWorkItemList.Contains(rwi) Then
                item = New ListViewItem(row.Cells("Einvoice_id").Value.ToString)
                item.SubItems.Add(row.Cells("PurchaseOrder").Value.ToString)
                item.SubItems.Add(row.Cells("Invoice").Value.ToString)
                item.SubItems.Add(row.Cells("Store").Value.ToString)
                item.SubItems.Add(row.Cells("Vendor").Value.ToString)
                item.SubItems.Add("")
                ListView_Reparse.Items.Add(item)

                ReparseWorkItemList.Add(rwi)
                cnt += 1
            Else
                ToolStripStatusLabel_Info.Text = "Duplicates caused by stores with the same Business Unit have been filtered out."
            End If
        Next

        Panel_Reparse.Visible = True
        UltraButton_Search.Enabled = False
        Panel_Reparse.Refresh()
        BackgroundWorker1.RunWorkerAsync(ReparseWorkItemList)

    End Sub

    Private Sub ViewErrorInformationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewErrorInformationToolStripMenuItem.Click
        Dim frm As EInvoicing_ErrorInformation = New EInvoicing_ErrorInformation()
        frm.EInvoiceId = DirectCast(_cell.Row.Cells("EInvoice_id").Value, Integer)
        frm.ShowDialog()
        frm.Dispose()
    End Sub

    Private Sub SuspendedEInvoicesToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SuspendedEInvoicesToolStripMenuItem.Click
        EInvoicing_SuspendedEInvoicesReport.ShowDialog()
        EInvoicing_SuspendedEInvoicesReport.Dispose()
    End Sub

    Private Sub UnmatchedPOsForEInvoicingVendorsToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UnmatchedPOsForEInvoicingVendorsToolStripMenuItem.Click
        Dim reportserver As String
        reportserver = ConfigurationServices.AppSettings("reportingServicesURL")
        System.Diagnostics.Process.Start(reportserver & "EInvVendorPOMissingEInvoice")
    End Sub

    Private Sub ChangePOInformationToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ChangePOInformationToolStripMenuItem.Click
        Dim frm As EInvoicing_EditPOInformation = New EInvoicing_EditPOInformation

        frm.PSVendorId = _cell.Row.Cells("VendorId").Value.ToString
        frm.VendorName = _cell.Row.Cells("Vendor").Value.ToString
        frm.OriginalPONumber = _cell.Row.Cells("PurchaseOrder").Value
        frm.EInvoiceId = _cell.Row.Cells("EInvoice_Id").Value

        frm.ShowDialog()
        frm.Dispose()

        refreshData(GenerateSearchCritera())
    End Sub

    Private Sub UltraGrid_EInvoicing_InitializeRow(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeRowEventArgs) Handles UltraGrid_EInvoices.InitializeRow
        If Not e.Row.Cells("ErrorCode_Id").Value.Equals(DBNull.Value) Then
            e.Row.Appearance = _redAppearance
            e.Row.ToolTipText = e.Row.Cells("ErrorMessage").Value.ToString()
        End If
    End Sub

    Private Sub CheckBox_ViewArchived_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        refreshData(GenerateSearchCritera())
    End Sub

    Private Sub UltraGrid_EInvoices_BeforeRowFilterDropDownPopulate(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeRowFilterDropDownPopulateEventArgs) Handles UltraGrid_EInvoices.BeforeRowFilterDropDownPopulate
        e.Handled = True
    End Sub

    Private Sub UltraGrid_EInvoices_BeforeRowFilterDropDown(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.BeforeRowFilterDropDownEventArgs) Handles UltraGrid_EInvoices.BeforeRowFilterDropDown
        e.ValueList.ValueListItems.Clear()

        Dim filterRow As UltraGridFilterRow = Me.UltraGrid_EInvoices.Rows.FilterRow
        filterRow.Cells("Status").Value = String.Empty

        If GetType(String) Is e.Column.DataType Then
            Dim condition As FilterCondition
            ' You can add items with custom criteria as well by using FilterCondition object
            ' as the DataValue of the value list item.
            condition = New FilterCondition(e.Column, FilterComparisionOperator.Match, "^Suspended")
            e.ValueList.ValueListItems.Insert(0, condition, "Suspended")

            condition = New FilterCondition(e.Column, FilterComparisionOperator.Match, "^Success")
            e.ValueList.ValueListItems.Insert(1, condition, "Success")

            condition = New FilterCondition(e.Column, FilterComparisionOperator.Match, "^S")
            e.ValueList.ValueListItems.Insert(2, condition, "All")
        End If
    End Sub

    Private Sub ArchiveSelectedToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ArchiveSelectedToolStripMenuItem.Click
        Dim EInvoiceId As Integer
        Dim dao As EInvoicingDAO = New EInvoicingDAO

        For Each row As UltraGridRow In UltraGrid_EInvoices.Selected.Rows
            EInvoiceId = DirectCast(row.Cells("EInvoice_id").Value, Integer)
            dao.ArchiveEInvoice(EInvoiceId, 1)
        Next

        dao.Dispose()
        refreshData(GenerateSearchCritera())
    End Sub

    Private Function VendorSearch() As Integer
        Dim retval As Integer = Nothing

        '-- Set glVendorID to none found before beginning the search
        glVendorID = 0

        '-- Set the search type
        giSearchType = iSearchVendorCompany

        '-- Open the search form
        Dim fSearch As New frmSearch
        fSearch.Text = "Vendor Search"
        fSearch.ShowDialog()
        fSearch.Close()
        fSearch.Dispose()

        '-- if the glVendorID is not zero, then something was found
        If glVendorID <> 0 Then
            retval = glVendorID
        End If

        Return retval
    End Function

    Private Sub LoadVendors()
        Dim vendorsList As List(Of VendorBO)
        Dim blankVendor As WholeFoods.IRMA.ItemHosting.BusinessLogic.VendorBO

        blankVendor = New WholeFoods.IRMA.ItemHosting.BusinessLogic.VendorBO()
        blankVendor.VendorID = -1
        blankVendor.VendorName = String.Empty
        blankVendor.PSVendorId = String.Empty

        vendorsList = VendorDAO.GetVendors()
        vendorsList.Insert(0, blankVendor)

        With UltraCombo_Vendors
            .DataSource = vendorsList
            .DisplayMember = "VendorName"
            .ValueMember = "PSVendorId"
            .DataBind()
            .DisplayLayout.Bands(0).Columns("VendorName").Width = .Width
            .DisplayLayout.Bands(0).Columns("VendorCurrencyID").Hidden = True
            .DisplayLayout.Bands(0).Columns("VendorCurrencyCode").Hidden = True
        End With
    End Sub

    Private Sub LoadStores()
        Dim storesList As ArrayList
        Dim blankStore As WholeFoods.IRMA.Administration.Common.BusinessLogic.StoreBO

        blankStore = New WholeFoods.IRMA.Administration.Common.BusinessLogic.StoreBO()
        blankStore.StoreName = ""
        blankStore.BusinessUnitId = Nothing

        storesList = WholeFoods.IRMA.Administration.StoreAdmin.DataAccess.StoreDAO.GetStoresAndFacilities()
        storesList.Insert(0, blankStore)

        With UltraCombo_Store
            .DataSource = storesList
            .DisplayMember = "StoreName"
            .ValueMember = "BusinessUnitId"
            .DataBind()
            .DisplayLayout.Bands(0).Columns("StoreName").Width = .Width
        End With
    End Sub

    Private Sub LoadEinvoiceErrorMessages()
        Dim errorCodesList As ArrayList
        Dim dao As EinvoicingConfigDAO = New EinvoicingConfigDAO

        errorCodesList = dao.loadEInvoicingErrorCodes()

        Dim blankCode As WholeFoods.IRMA.Administration.EInvoicing.BusinessLogic.EinvoicingErrorCodeBO
        blankCode = New WholeFoods.IRMA.Administration.EInvoicing.BusinessLogic.EinvoicingErrorCodeBO(-1, String.Empty, String.Empty)

        errorCodesList.Insert(0, blankCode)
        With UltraCombo_ErrorMessage
            .DataSource = errorCodesList
            .DisplayMember = "ErrorMessage"
            .ValueMember = "ErrorCodeId"
            .DataBind()

            .DisplayLayout.Bands(0).Columns("ErrorMessage").Width = .Width
        End With
    End Sub

    Private Sub LoadEinvoiceStatuses()
        Dim statusList As List(Of String) = New List(Of String)

        statusList.AddRange({"", "Success", "Suspended"})

        With UltraCombo_Status
            .DataSource = statusList
            .DataBind()
        End With
    End Sub

    Private Sub UltraButton_Search_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles UltraButton_Search.Click
        refreshData(GenerateSearchCritera())
    End Sub

    Private Function GenerateSearchCritera() As EinvoicingSearchBO
        Dim searchData As EinvoicingSearchBO = New EinvoicingSearchBO

        With searchData
            .InvoiceStartDate = UltraDateTimeEditor_InvoiceStart.Value
            .InvoiceEndDate = UltraDateTimeEditor_InvoiceEnd.Value
            .ImportStartDate = UltraDateTimeEditor_ImportStart.Value
            If Len(UltraDateTimeEditor_ImportEnd.Value) > 0 Then
                .ImportEndDate = DateTime.Parse(UltraDateTimeEditor_ImportEnd.Value).AddDays(1).AddTicks(-1)
            Else
                .ImportEndDate = UltraDateTimeEditor_ImportEnd.Value
            End If
            .PONumber = UltraTextEditor_PONumber.Text.Trim
            .InvoiceNumber = UltraTextEditor_InvoiceNumber.Text.Trim

            Dim ugr As UltraGridRow = Nothing
            ugr = UltraCombo_Store.SelectedRow
            If ugr Is Nothing Then
                .BusinessUnit = String.Empty
            Else
                .BusinessUnit = IIf(IsNothing(ugr.Cells("BusinessUnitId").Value), String.Empty, ugr.Cells("BusinessUnitId").Value)
            End If

            ugr = UltraCombo_Vendors.SelectedRow
            If ugr Is Nothing Then
                .PSVendorId = String.Empty
            Else
                .PSVendorId = IIf(IsNothing(ugr.Cells("PSVendorId").Value), String.Empty, ugr.Cells("PSVendorId").Value)
            End If

            ugr = UltraCombo_Status.SelectedRow
            If ugr Is Nothing Then
                .Status = String.Empty
            Else
                .Status = ugr.Cells("Value").Value
            End If

            ugr = UltraCombo_ErrorMessage.SelectedRow
            If ugr Is Nothing Then
                .ErrorCodeId = Nothing
            Else
                .ErrorCodeId = IIf(ugr.Cells("ErrorCodeId").Value.Equals(-1), Nothing, ugr.Cells("ErrorCodeId").Value)
            End If

            .Archived = IIf(CheckBox_Archived.Checked, 1, Nothing)
        End With

        Return searchData
    End Function

    Private Sub UltraCombo_ErrorMessage_MouseEnterElement(ByVal sender As System.Object, ByVal e As Infragistics.Win.UIElementEventArgs) Handles UltraCombo_ErrorMessage.MouseEnterElement, UltraCombo_Store.MouseEnterElement, UltraCombo_Vendors.MouseEnterElement
        Dim acell As UltraGridCell = e.Element.GetContext(GetType(Infragistics.Win.UltraWinGrid.UltraGridCell))

        If acell IsNot Nothing Then
            If acell.Value IsNot Nothing Then
                If Not acell.Value.Equals(String.Empty) Then
                    DisplayToolTip(e.Element.Control, String.Empty, acell.Value, New Point(Cursor.Position.X, Cursor.Position.Y), False)
                End If
            End If
        End If
    End Sub

    Private Sub UltraCombo_ErrorMessage_MouseLeaveElement(ByVal sender As System.Object, ByVal e As Infragistics.Win.UIElementEventArgs) Handles UltraCombo_ErrorMessage.MouseLeaveElement, UltraCombo_Store.MouseLeaveElement, UltraCombo_Vendors.MouseEnterElement
        UltraToolTipManager1.HideToolTip()
    End Sub

    Private Function validateSearchCriteria() As Boolean
        Dim retval As Boolean = False
        Dim toolTipMessage As String = String.Empty

        toolTipMessage = "Invoice Begin and End dates are required."
        If UltraDateTimeEditor_InvoiceStart.Value Is Nothing Then
            DisplayToolTip(UltraDateTimeEditor_ImportStart, String.Empty, toolTipMessage, True, True)
            Return False
        End If

        If UltraDateTimeEditor_InvoiceEnd.Value Is Nothing Then
            DisplayToolTip(UltraDateTimeEditor_InvoiceEnd, String.Empty, toolTipMessage, True, True)
            Return False
        End If

        If UltraDateTimeEditor_InvoiceEnd.Value < UltraDateTimeEditor_InvoiceStart.Value Then
            toolTipMessage = "Invoice End Date must be greater than Invoice Begin Date."
            DisplayToolTip(UltraDateTimeEditor_InvoiceStart, String.Empty, toolTipMessage, True, True)
            Return False
        End If

        If Math.Abs(DateDiff(DateInterval.Day, UltraDateTimeEditor_InvoiceEnd.Value, UltraDateTimeEditor_InvoiceStart.Value)) > 60 Then
            toolTipMessage = "Date ranges cannot be greather than 60 days."
            DisplayToolTip(UltraDateTimeEditor_InvoiceStart, String.Empty, toolTipMessage, True, True)
            Return False
        End If

        If (UltraDateTimeEditor_ImportStart.Value IsNot Nothing And UltraDateTimeEditor_ImportEnd.Value Is Nothing) Or
            (UltraDateTimeEditor_ImportStart.Value Is Nothing And UltraDateTimeEditor_ImportEnd.Value IsNot Nothing) Then
            toolTipMessage = "Please provide an Import Begin and End date."
            DisplayToolTip(UltraDateTimeEditor_ImportStart, String.Empty, toolTipMessage, True, True)
            Return False
        End If

        If UltraDateTimeEditor_ImportEnd.Value < UltraDateTimeEditor_ImportStart.Value Then
            toolTipMessage = "Import End Date must be greater than Invoice Begin Date."
            DisplayToolTip(UltraDateTimeEditor_ImportStart, String.Empty, toolTipMessage, True, True)
            Return False
        End If

        If UltraDateTimeEditor_ImportStart.Value IsNot Nothing And UltraDateTimeEditor_ImportEnd.Value IsNot Nothing Then
            If Math.Abs(DateDiff(DateInterval.Day, UltraDateTimeEditor_ImportEnd.Value, UltraDateTimeEditor_ImportStart.Value)) > 60 Then
                toolTipMessage = "Date ranges cannot be greather than 60 days."
                DisplayToolTip(UltraDateTimeEditor_ImportStart, String.Empty, toolTipMessage, True, True)
                Return False
            End If
        End If

        Return True
    End Function

    Private Sub DisplayToolTip(ByRef control As Control, ByVal title As String, ByVal message As String, ByVal giveFocus As Boolean, ByVal adjustForControlHeight As Boolean)
        ' shows the tool tip aligned below the target Control
        Dim p As Point = New Point()

        If control.Parent Is Nothing Then
            p = MousePosition
        Else
            p = control.Parent.PointToScreen(control.Location)
        End If

        If adjustForControlHeight Then p.Y += control.Height

        UltraToolTipManager1.SetUltraToolTip(control, New UltraWinToolTip.UltraToolTipInfo(message, Nothing, title, DefaultableBoolean.True))
        UltraToolTipManager1.ShowToolTip(control, p)

        If giveFocus Then control.Focus()
    End Sub

    Private Sub DisplayToolTip(ByRef control As Control, ByVal title As String, ByVal message As String, ByVal targetPoint As Point, ByVal giveFocus As Boolean)
        ' shows the tool tip at the specified point.
        UltraToolTipManager1.SetUltraToolTip(control, New UltraWinToolTip.UltraToolTipInfo(message, Nothing, title, DefaultableBoolean.True))
        UltraToolTipManager1.ShowToolTip(control, targetPoint)
    End Sub

    Private Sub ForceLoadToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ForceLoadToolStripMenuItem.Click
        If MessageBox.Show(String.Format("This will cause an eInvoice to be matched to a PO and overwrite any existing eInvoice information that is arleady there.{0}Do you wish to continue?", vbCrLf), "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = DialogResult.Yes Then
            PerformForceLoad()
        End If
    End Sub

    Private Sub PerformForceLoad()
        Me.Cursor = Cursors.WaitCursor
        Dim eInv As EInvoicingJob = New EInvoicingJob()

        eInv.LoadEInvoicingDataFromString(eInv.GetEInvoiceXML(DirectCast(_cell.Row.Cells("EInvoice_id").Value, Integer)))
        eInv.ParseInvoicesFromXML(eInv.XMLData, DirectCast(_cell.Row.Cells("EInvoice_id").Value, Integer), True, DirectCast(_cell.Row.Cells("PurchaseOrder").Value, String))

        refreshData(GenerateSearchCritera())
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub btnCancelReparse_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnCancelReparse.Click
        btnCancelReparse.Text = "Cancelling..."
        ToolStripStatusLabel_Info.Text = "Reparsing process will end after the current eInvoice finishes."
        BackgroundWorker1.CancelAsync()
    End Sub

    Private Sub BackgroundWorker_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        Dim ReparseWorkItemList As List(Of ReparseWorkitem) = DirectCast(e.Argument, List(Of ReparseWorkitem))
        Dim bgw As BackgroundWorker = DirectCast(sender, BackgroundWorker)
        Dim ReparseProgressInfo As ReparseProgressInfo
        Dim cnt As Integer = 0

        For Each i As ReparseWorkitem In ReparseWorkItemList
            If Not bgw.CancellationPending Then
                Try
                    cnt += 1
                    ReparseProgressInfo = New ReparseProgressInfo(i.EinvoiceId, i.OrderHeaderId, ReparseWorkItemList.Count, cnt, "In Progress", Nothing)
                    bgw.ReportProgress(cnt, ReparseProgressInfo)

                    Dim eInv As EInvoicingJob = New EInvoicingJob()

                    eInv.LoadEInvoicingDataFromString(eInv.GetEInvoiceXML(i.EinvoiceId))
                    eInv.ParseInvoicesFromXML(eInv.XMLData, i.EinvoiceId, i.OrderHeaderId.ToString())

                    ReparseProgressInfo = New ReparseProgressInfo(i.EinvoiceId, i.OrderHeaderId, ReparseWorkItemList.Count, cnt, "Completed", Nothing)
                    bgw.ReportProgress(cnt, ReparseProgressInfo)
                Catch ex As Exception
                    ReparseProgressInfo = New ReparseProgressInfo(i.EinvoiceId, i.OrderHeaderId, ReparseWorkItemList.Count, cnt, "Error", ex)
                    bgw.ReportProgress(cnt, ReparseProgressInfo)
                End Try
            End If
        Next
    End Sub

    Private Sub BackgroundWorker_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        Dim ReparseProgressInfo As ReparseProgressInfo
        Dim CurrentIndex As Integer

        ReparseProgressInfo = DirectCast(e.UserState, ReparseProgressInfo)

        ' update the progress bar.
        With ToolStripProgressBar1
            .Maximum = ReparseProgressInfo.TotalItems
            .Minimum = 0
            .Value = ReparseProgressInfo.CurrentItem
            .Invalidate()
        End With

        'find the item we are working on in the list view. 
        Dim item As ListViewItem
        item = ListView_Reparse.FindItemWithText(ReparseProgressInfo.EinvoiceId.ToString())
        CurrentIndex = ListView_Reparse.Items.IndexOf(item)

        'set focus to that item. 
        item.Focused = True

        'scroll in listview to show last 2 worked, and current worked.
        ListView_Reparse.TopItem = ListView_Reparse.Items(IIf(CurrentIndex > 1, CurrentIndex - 2, CurrentIndex))
        item.SubItems(5).Text = ReparseProgressInfo.State

        'if an error exists set it as tool tip
        If Not ReparseProgressInfo.ErrorInfo Is Nothing Then
            item.BackColor = Color.Crimson
            item.ToolTipText = ReparseProgressInfo.ErrorInfo.Message
        End If

        ToolStripStatusLabel_Count.Text = String.Format("{0} of {1}", ReparseProgressInfo.CurrentItem.ToString, ReparseProgressInfo.TotalItems.ToString)
    End Sub

    Private Sub BackgroundWorker_RunWorkerComplete(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Panel_Reparse.Refresh()

        Dim FirstErrorItem As ListViewItem
        FirstErrorItem = ListView_Reparse.FindItemWithText("Error", True, 0, 0)

        If Not FirstErrorItem Is Nothing Then
            ListView_Reparse.TopItem = FirstErrorItem
        End If

        ToolStripStatusLabel_Count.Text = "Refreshing..."
        Panel_Reparse.Refresh()

        refreshData(GenerateSearchCritera())
        Threading.Thread.Sleep(2500)

        'reset things.
        ToolStripStatusLabel_Count.Text = String.Empty
        Panel_Reparse.Visible = False
        btnCancelReparse.Text = "Cancel"
        btnCancelReparse.Enabled = True
        UltraButton_Search.Enabled = True
    End Sub
End Class