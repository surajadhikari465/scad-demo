Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Text

Public Class BOSpreadsheetUpload

    Private _UserID As Integer
    Private _numberOfStores As Integer
    Private _numberOrderItems As Integer
    Private _SessionHistoryID As Integer
    Private _numberOfColumns As Integer
    Private _currentPOHeaderID As Integer
    Private _myOrderList As List(Of OrderObject)
    Private _fileName As String
    Private _isUploaded As Boolean


#Region "Properties"

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property FileName() As String
        Get
            Return _fileName
        End Get
        Set(ByVal value As String)
            _fileName = value
        End Set
    End Property


    Public ReadOnly Property CurrentPOHeaderID() As Integer
        Get
            Return _currentPOHeaderID
        End Get
    End Property


    Public Property MyOrderList() As List(Of OrderObject)
        Get
            Return _myOrderList
        End Get
        Set(ByVal value As List(Of OrderObject))
            _myOrderList = value
        End Set
    End Property

    Public Property IsUploaded() As Boolean
        Get
            Return _isUploaded
        End Get
        Set(ByVal value As Boolean)
            _isUploaded = value
        End Set
    End Property

    Public ReadOnly Property NumberStores() As Integer
        Get
            Return _numberOfStores
        End Get
    End Property

    Public ReadOnly Property NumberOrderItems() As Integer
        Get
            Return _numberOrderItems
        End Get
    End Property

    Public ReadOnly Property SessionHistoryID() As Integer
        Get
            Return _SessionHistoryID
        End Get
    End Property

#End Region

#Region "Private Methods"

    Private Sub CreatePOHeaderRow(ByRef dt As POHeaderItems.POHeaderDataTable, ByRef thisOrder As OrderObject)

        Dim dr As POHeaderItems.POHeaderRow = dt.NewPOHeaderRow

        dr.POHeaderID = thisOrder.POHeaderID
        dr.PONumberID = thisOrder.PONumber.PONumberID
        dr.UploadSessionHistoryID = SessionHistoryID
        dr.CreatedDate = DateAdd(DateInterval.Hour, -2, Date.Now) 'Date.Now
        dr.VendorPSNumber = thisOrder.Vendor.VendorPSNumber.PadLeft(10, CType("0", Char))
        dr.VendorName = ""
        dr.RegionID = thisOrder.PONumber.RegionID
        dr.SetBusinessUnitNull() 'added
        dr.Subteam = thisOrder.SubTeam
        dr.ExpectedDate = thisOrder.ExpectedDate

        If thisOrder.AutoPushDate = "#12:00:00 AM#" Then
            dr.SetAutoPushDateNull()
        Else
            dr.AutoPushDate = thisOrder.AutoPushDate
        End If

        dr.PushedByUserID = thisOrder.PushedByUserID

        dr.OrderItemCount = thisOrder.NumberOfItems
        dr.SetTotalPOCostNull()
        dr.Notes = thisOrder.Notes
        dr.SetDeletedDateNull()
        dr.SetIRMAVendor_IDNull()
        dr.SetExceptionItemCountNull()
        dr.SetValidationAttemptDateNull()
        dr.SetExpiredNull()
        dr.SetPushedToIRMADateNull()
        dr.SetConfirmedInIRMADateNull()
        dr.StoreAbbr = thisOrder.Store.StoreAbbr 'added
        dr.DiscountType = thisOrder.DiscountType
        dr.DiscountAmount = thisOrder.DiscountAmount
        dr.ReasonCode = thisOrder.HeaderReasonCode

        dt.AddPOHeaderRow(dr)
        dt.AcceptChanges()

    End Sub

    Private Sub CreatePOItemRow(ByRef dt As POHeaderItems.POItemDataTable, _
    ByRef thisOrderItem As OrderItemObject, _
    ByVal HeaderID As Integer)

        Dim dr As POHeaderItems.POItemRow = dt.NewPOItemRow

        dr.POHeaderID = HeaderID
        dr.Identifier = thisOrderItem.UPC
        dr.SetItem_KeyNull()
        dr.VendorItemNumber = thisOrderItem.VIN
        dr.ItemBrand = thisOrderItem.ItemBrand
        dr.ItemDescription = thisOrderItem.ItemDescription
        dr.OrderQuantity = thisOrderItem.OrderQuantity
        dr.FreeQuantity = thisOrderItem.FreeQuantity
        dr.SetUnitCostNull()
        dr.SetUnitCostUOMNull()
        dr.DiscountType = thisOrderItem.DiscountType
        dr.DiscountAmount = thisOrderItem.DiscountAmount
        dr.ReasonCode = thisOrderItem.ItemReasonCode
        dt.AddPOItemRow(dr)
        dt.AcceptChanges()

    End Sub

    Private Function InsertSessionHistory() As Integer

        Dim Upload As New DALSpreadSheetUpload()

        Return Upload.InsertSessionHistory(GetArrayList())

    End Function

    Private Function GetCurrentPOHeaderID() As Integer

        Dim Upload As New DALSpreadSheetUpload()

        Return Upload.GetNextPOHeaderID()

    End Function


    Private Function ConvertOrderList() As DataSet

        '*****************************************
        Dim finalSet As New DataSet
        Dim header As New POHeaderItems.POHeaderDataTable
        Dim items As New POHeaderItems.POItemDataTable
        Dim orderEnum As IEnumerator
        Dim orderItemEnum As IEnumerator
        orderEnum = MyOrderList.GetEnumerator
        Dim myOrder As OrderObject
        Dim myOrderItem As OrderItemObject
        '*****************************************
        '************ LOOP ORDER LIST ************
        '*****************************************
        Try

            While orderEnum.MoveNext

                _currentPOHeaderID += 1

                myOrder = CType(orderEnum.Current, OrderObject)

                myOrder.POHeaderID = CurrentPOHeaderID

                orderItemEnum = myOrder.OrderItems.GetEnumerator()

                CreatePOHeaderRow(header, myOrder)

                While orderItemEnum.MoveNext

                    myOrderItem = CType(orderItemEnum.Current, OrderItemObject)

                    CreatePOItemRow(items, myOrderItem, myOrder.POHeaderID)

                End While

            End While

        Catch ex As Exception

            Debug.WriteLine(ex.Message)
            Debug.WriteLine(ex.StackTrace)
            Debug.WriteLine(ex.Source)
        End Try


        '*******************************
        finalSet.Tables.Add(header)
        finalSet.Tables.Add(items)
        finalSet.AcceptChanges()
        Return finalSet
    End Function

    Private Function GetArrayList() As ArrayList

        Dim al As New ArrayList

        al.Add(_UserID)
        al.Add(_filename)
        al.Add(Date.Now)

        Return al

    End Function

    Private Sub InsertPOHeader(ByVal dt As DataTable)

        Dim dal As New DALSpreadSheetUpload()

        dal.InsertOrderHeader(dt)

    End Sub

    Private Sub InsertPOItem(ByVal dt As DataTable)

        Dim dal As New DALSpreadSheetUpload()

        dal.InsertOrderItem(dt)

    End Sub

#End Region


    ''' <summary>
    ''' PUBLIC INTERFACE TO UPLOAD
    ''' </summary>
    ''' 

#Region "Public Methods"

    Public Sub UploadSpreadSheet()

        Dim FinalDataSet As New DataSet

        Dim az As String = String.Empty
        Try
            '**********************************
            '***** INSERT SESSION HISTORY *****
            '**********************************
            _SessionHistoryID = InsertSessionHistory()
            '*****************************************
            '*** GET CURRENT PO HEADER ID ************
            '*****************************************
            _currentPOHeaderID = GetCurrentPOHeaderID()
            '**********************************
            '*********CONVERT SPREADSHEET******
            '**********************************
            FinalDataSet = ConvertOrderList()
            '**********************************
            '*****INSERT PO HEADER/ITEM********
            '**********************************
            InsertPOHeader(FinalDataSet.Tables(0))
            InsertPOItem(FinalDataSet.Tables(1))
            '**********************************
            _isUploaded = True
        Catch ex As Exception
            _isUploaded = False
            Debug.WriteLine(ex.StackTrace)
            Debug.WriteLine(ex.Message)
        End Try

    End Sub

    Public Function GetUploadedDataSet() As DataSet

        Dim dal As New DALSpreadSheetUpload()
        Dim ds As New DataSet

        Dim dt1 As New DataTable("POHeaders")
        Dim dt2 As New DataTable("POItems")

        Try
            dt1 = dal.GetUploadedPOHeaders(_SessionHistoryID)
            dt2 = dal.GetUploadedPOItems(_SessionHistoryID)
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try

        ds.Tables.Add(dt1)
        ds.Tables.Add(dt2)

        ' *** Create Relation *****
        ds.Relations.Add("POs", _
            ds.Tables(0).Columns("POHeaderID"), _
            ds.Tables(1).Columns("POHeaderID"))


        Return ds

    End Function

#End Region

End Class
