Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.Text.RegularExpressions

Public Enum ProcessFlow

    IsImported
    IsValidated
    IsUploaded

End Enum

Public Enum VendorRowColumns

    Vendor = 0
    VendorPSNumber = 1
    SubTeam = 2
    ExpectedDate = 5
    AutoPushDate = 6
    DiscountType = 7
    DiscountAmount = 8
    ReasonCode = 9
    Notes = 10

End Enum

Public Enum ItemRowColumns

    Identifier = 0
    VendorItemNumber = 1
    ItemBrand = 3
    ItemDescription = 4
    DiscountType = 7
    FreeQuantity = 7
    DiscountAmount = 8
    ReasonCode = 9

End Enum

Public Class BOSpreadsheetValidation

#Region "Private Fields"

    Private _filename As String

    Private _UserID As Integer

    Private _ImportSpreadSheet As DataSet

    Private _poLookUp As DataSet

    Private _orderList As List(Of OrderObject)

    Private _isValidated As Boolean

    Private _errorRowIndex As Integer

    Private _errorColumnIndex As Integer

    Private _errorMessage As String

    Private _region As String

#End Region

#Region "Properties"
    Public Property Region() As String
        Get
            Return _region
        End Get
        Set(ByVal value As String)
            _region = value
        End Set
    End Property

    Public Property ImportSpreadSheet() As DataSet
        Get
            Return _ImportSpreadSheet
        End Get
        Set(ByVal value As DataSet)
            _ImportSpreadSheet = value
        End Set
    End Property

    Public Property POLookUp() As DataSet
        Get
            Return _poLookUp
        End Get
        Set(ByVal value As DataSet)
            _poLookUp = value
        End Set
    End Property

    Public Property FileName() As String
        Get
            Return _filename
        End Get
        Set(ByVal value As String)
            _filename = value
        End Set
    End Property

    Public Property ErrorMessage() As String
        Get
            Return _errorMessage
        End Get
        Set(ByVal value As String)
            _errorMessage = value
        End Set
    End Property

    Public Property UserID() As Integer
        Get
            Return _UserID
        End Get
        Set(ByVal value As Integer)
            _UserID = value
        End Set
    End Property

    Public Property IsValidated() As Boolean
        Get
            Return _isValidated
        End Get

        Set(ByVal value As Boolean)
            _isValidated = value
        End Set
    End Property

    Public Property OrderList() As List(Of OrderObject)
        Get
            Return _orderList
        End Get
        Set(ByVal value As List(Of OrderObject))
            _orderList = value
        End Set
    End Property

    Public Property ErrorRowIndex() As Integer
        Get
            Return _errorRowIndex
        End Get
        Set(ByVal value As Integer)
            _errorRowIndex = value
        End Set
    End Property

    Public Property ErrorColumnIndex() As Integer
        Get
            Return _errorColumnIndex
        End Get
        Set(ByVal value As Integer)
            _errorColumnIndex = value
        End Set
    End Property

#End Region


#Region "Private Methods"

    Private Sub ConvertImportSheet(ByVal ds As DataSet)

        '*****************************************
        Dim EmptyColumns As New Regex(ConfigurationManager.AppSettings("EmptyColumns"))
        Dim StartStores As New Regex(ConfigurationManager.AppSettings("StoreAbbreviations"))
        Dim stores As New Collection
        Dim orders As New Collection
        Dim dt As New DataTable("Originaltable")
        dt = ds.Tables(0)
        Dim colEnum As IEnumerator = dt.Rows(0).ItemArray.GetEnumerator
        Dim rowEnum As IEnumerator = dt.Rows.GetEnumerator()
        '**********************************
        '***** GET THE STORES ******
        '**********************************

        Dim noc As Boolean = ValidateNumberOfColumns(colEnum)
        If noc = False Then Exit Sub

        Dim st As New StoreList(GetAllStores(colEnum))

        If Not st.StoreList.Count > 0 Then
            SetErrorIndex(0, -1, "No Valid Store Business Units Found")
            Exit Sub
        ElseIf _isValidated = False Then
            Exit Sub        ' **** Column Count indicates missing columns ******
        End If

        Dim storeEnum As IEnumerator = st.StoreList.GetEnumerator

        '**********************************
        '********** PO Look Up*************
        '**********************************
        PONumberLookUp()
        If Not POLookUp.Tables(0).Rows.Count > 0 Then
            SetErrorIndex(0, -1, "No Valid PO-Numbers Found")
            Exit Sub
        End If
        '*****************************************
        '***** LOOP ROWS - FILL OBJECTS **********
        '*****************************************
        LoopSpreadSheetRows(rowEnum, storeEnum)
        '**********************************
        '*****XXXXXXXXXXXXXXXXXXXXXXX******
        '**********************************
        dt.Dispose()
    End Sub

#End Region

#Region "Events"

    Public Event FailSpreadSheet As EventHandler

#End Region

#Region "Public Methods"

    Public Sub ValidateSpreadSheet()

        ' ***** Initiate validation = true *******
        _isValidated = True
        ConvertImportSheet(_ImportSpreadSheet)

    End Sub
    Private Function ValidateNumberOfColumns(ByRef columns As IEnumerator) As Boolean

        Dim rowItem As String
        Dim count As Integer = 0

        Const fixedColumns As Integer = 11       ' *** Number of Columns that are mandatory ****

        '**********************************
        '*****Loop the Columns******
        '**********************************
        columns.Reset()
        While columns.MoveNext()
            If count = 11 Then Exit While

            rowItem = Trim(CStr(CType(columns.Current(), String))).ToUpper
            If count = 0 And rowItem <> "UPC" Then
                SetErrorIndex(0, 1, "Missing UPC column-- Upload new template")
                Return False
            End If

            If count = 1 And rowItem <> "VIN" Then
                SetErrorIndex(0, 1, "Missing VIN column-- Upload new template")
                Return False
            End If

            If count = 2 And rowItem <> "SUBTEAM" Then
                SetErrorIndex(0, 2, "Missing Subteam column-- Upload new template")
                Return False
            End If

            If count = 3 And rowItem <> "BRAND" Then
                SetErrorIndex(0, 3, "Missing Brand column-- Upload new template")
                Return False
            End If

            If count = 4 And rowItem <> "DESCRIPTION" Then
                SetErrorIndex(0, 4, "Missing Description column-- Upload new template")
                Return False
            End If

            If count = 5 And rowItem <> "EXPECTED DATE" Then
                SetErrorIndex(0, 5, "Missing Expected Date column-- Upload new template")
                Return False
            End If

            If count = 6 And rowItem <> "AUTO PUSH DATE" Then
                SetErrorIndex(0, 6, "Missing AutoPush Date column-- Upload new template")
                Return False
            End If

            If count = 7 And rowItem <> "DISCOUNT TYPE" Then
                SetErrorIndex(0, 7, "Missing Discount Type column-- Upload new template")
                Return False
            End If

            If count = 8 And rowItem <> "DISCOUNT AMOUNT" Then
                SetErrorIndex(0, 8, "Missing Discount amount column-- Upload new template")
                Return False
            End If

            If count = 9 And rowItem <> "REASON CODE" Then
                SetErrorIndex(0, 9, "Missing Reason Code column-- Upload new template")
                Return False
            End If

            If count = 10 And rowItem <> "NOTES" Then
                SetErrorIndex(0, 10, "Missing Notes column-- Upload new template")
                Return False
            End If

            count += 1
        End While

        ' **** Make sure there are no missing or additional columns ****
        If count < fixedColumns Then
            SetErrorIndex(0, count - 1, "Invalid Column Count")
            Return False
        End If

        Return True
    End Function

    Private Function GetAllStores(ByRef columns As IEnumerator) As List(Of StoreObject)

        Dim rowItem As String
        Dim count As Integer = 0
        Dim stores As New List(Of StoreObject)

        Dim StartStores As New Regex(ConfigurationManager.AppSettings("StoreAbbreviations"))

        Const fixedColumns As Integer = 11       ' *** Number of Columns that are mandatory ****
        Dim FirstStoreColumn As Integer = 0
        '**********************************
        '*****Loop the Columns******
        '**********************************
        columns.Reset()
        While columns.MoveNext()
            If count >= fixedColumns Then
                If columns.Current().GetType.Name = "String" Then
                    rowItem = Trim(CStr(CType(columns.Current(), String))).ToUpper()
                    If StartStores.IsMatch(rowItem) = True Then
                        If FirstStoreColumn = 0 Then
                            FirstStoreColumn = count
                        End If
                        stores.Add(New StoreObject(CStr(rowItem), count))
                    End If
                End If
            End If
            count += 1
        End While

        ' **** Make sure there are no missing or additional columns ****
        If Not FirstStoreColumn = fixedColumns Then
            SetErrorIndex(0, FirstStoreColumn, "Invalid Column Count - BusinessUnits must start at F11")
        End If
        Return stores

    End Function
    Private Sub LoopSpreadSheetRows(ByRef rows As IEnumerator, ByRef stores As IEnumerator)

        Dim dr As DataRow
        Dim st As New StoreObject()
        Dim VendorRows As New Regex(ConfigurationManager.AppSettings("VendorRows"), RegexOptions.IgnoreCase)
        Dim ItemRows As New Regex(ConfigurationManager.AppSettings("ItemRows"))
        Dim DiscountType As New Regex(ConfigurationManager.AppSettings("DiscountType"))
        Dim DiscountAmount As New Regex(ConfigurationManager.AppSettings("DiscountAmount"))
        Dim order As New OrderObject
        Dim orderItemlist As New List(Of OrderItemObject)
        Dim orderList As New List(Of OrderObject)
        Dim rowNumber As Integer
        Dim columnNumber As Integer
        Dim po As New PONumberObject
        Dim poList As New List(Of Integer)
        Dim Val As New BOExceptions

        Dim BOPO As New BOPONumbers
        Dim DT As DataTable = BOPO.GetReasonCodesForUser(UserID).Tables(0)
        Dim MyReasonCodes As New Hashtable
        For Each row As DataRow In DT.Rows
            MyReasonCodes.Add(row("ReasonCode").ToString.Trim, row("ReasonCodeDetailID"))
        Next

        '**********************************
        '*****Loop the Rows******
        '**********************************
        Try
            stores.Reset()
            While stores.MoveNext
                rows.Reset()
                rowNumber = 0
                st = CType(stores.Current(), StoreObject)

                While rows.MoveNext()
                    Dim isVendorRow As Boolean = False
                    dr = CType(rows.Current(), DataRow)
                    ' **** FIND HEADER ROWS ************
                    If VendorRows.IsMatch(dr.Item(VendorRowColumns.Vendor).ToString) = True Then
                        isVendorRow = True
                        ' *** Finish Previous Order ****
                        If orderItemlist.Count > 0 Then
                            ' *** Finish the Order **** And Restart
                            order.OrderItems = orderItemlist
                            If order.GetTotalItemQuantity > 0 Then
                                orderList.Add(order)
                            End If
                            order = Nothing
                            orderItemlist = Nothing
                            po = Nothing
                            order = New OrderObject
                            orderItemlist = New List(Of OrderItemObject)
                            po = New PONumberObject

                        End If

                        ' **** Vendor PS Number *****
                        columnNumber = VendorRowColumns.VendorPSNumber
                        If Trim(dr.Item(VendorRowColumns.VendorPSNumber).ToString) = String.Empty Or _
                        dr.Item(VendorRowColumns.VendorPSNumber).ToString.Length > 10 Then
                            SetErrorIndex(rowNumber, columnNumber, "Invalid Vendor PS Number")
                            Exit Sub
                        End If
                        order.Vendor = New VendorObject(dr.Item(VendorRowColumns.VendorPSNumber).ToString, rowNumber)
                        columnNumber = VendorRowColumns.SubTeam
                        order.SubTeam = CInt(dr.Item(VendorRowColumns.SubTeam))

                        columnNumber = VendorRowColumns.ExpectedDate
                        order.ExpectedDate = CDate(dr.Item(VendorRowColumns.ExpectedDate))

                        columnNumber = VendorRowColumns.AutoPushDate
                        order.AutoPushDate = CDate(dr.Item(VendorRowColumns.AutoPushDate))

                        '  *** Discounts ***
                        columnNumber = VendorRowColumns.DiscountType
                        If dr.Item(VendorRowColumns.DiscountType).ToString <> String.Empty Then
                            'headers currently can only be discounted by percentages
                            If dr.Item(VendorRowColumns.DiscountType).ToString = "%" Then
                                order.DiscountType = dr.Item(VendorRowColumns.DiscountType).ToString

                                If IsDBNull(dr.Item(VendorRowColumns.DiscountAmount)) Then
                                    SetErrorIndex(rowNumber, VendorRowColumns.DiscountAmount, "Missing Discount Amount")
                                    Exit Sub
                                End If
                            Else
                                SetErrorIndex(rowNumber, columnNumber, "Invalid Discount Type")
                                Exit Sub
                            End If

                            'check for missing/invalid reason code AM
                            columnNumber = VendorRowColumns.ReasonCode
                            If dr.Item(VendorRowColumns.ReasonCode).ToString = "" Then
                                SetErrorIndex(rowNumber, columnNumber, "Missing Reason Code")
                                Exit Sub
                            ElseIf Val.IsNotLetters(dr.Item(VendorRowColumns.ReasonCode).ToString) = True Then
                                SetErrorIndex(rowNumber, columnNumber, "Invalid Reason Code")
                                Exit Sub
                            ElseIf dr.Item(VendorRowColumns.ReasonCode).ToString.Length > 2 Then
                                SetErrorIndex(rowNumber, columnNumber, "Invalid Reason Code")
                                Exit Sub
                            Else
                                columnNumber = VendorRowColumns.ReasonCode
                                If MyReasonCodes.ContainsKey(dr.Item(VendorRowColumns.ReasonCode).ToString.ToUpper) Then
                                    order.HeaderReasonCode = MyReasonCodes.Item(dr.Item(VendorRowColumns.ReasonCode).ToString.ToUpper)
                                Else
                                    SetErrorIndex(rowNumber, columnNumber, "Invalid Reason Code")
                                    Exit Sub
                                End If
                            End If
                        End If

                        'check for expeced date to aviod past dates AM
                        columnNumber = VendorRowColumns.ExpectedDate
                        If Convert.ToDateTime(dr.Item(VendorRowColumns.ExpectedDate).ToString) < DateTime.Now.Date Then
                            SetErrorIndex(rowNumber, columnNumber, "Expected Date cannot be in the past")
                            Exit Sub
                        End If
                        'end

                        'check for autopush date to aviod past dates AM
                        columnNumber = VendorRowColumns.AutoPushDate
                        If dr.Item(VendorRowColumns.AutoPushDate).ToString <> String.Empty Then
                            If Convert.ToDateTime(dr.Item(VendorRowColumns.AutoPushDate).ToString) < DateTime.Now.Date Then
                                SetErrorIndex(rowNumber, columnNumber, "Auto Push Date cannot be in the past")
                                Exit Sub
                            End If

                            If order.AutoPushDate > order.ExpectedDate Then
                                SetErrorIndex(rowNumber, columnNumber, "Auto Push Date must be same as or earlier than Expected Date")
                                Exit Sub
                            End If
                        Else
                            SetErrorIndex(rowNumber, columnNumber, "Missing Auto Push Date")
                            Exit Sub
                        End If
                        'end

                        columnNumber = VendorRowColumns.DiscountAmount
                        If dr.Item(VendorRowColumns.DiscountAmount).ToString <> String.Empty Then
                            If DiscountAmount.IsMatch(dr.Item(VendorRowColumns.DiscountAmount)) Then
                                order.DiscountAmount = dr.Item(VendorRowColumns.DiscountAmount)
                                If (order.DiscountType = "%" And (order.DiscountAmount > 100.0 Or order.DiscountAmount < 0.0)) Then
                                    SetErrorIndex(rowNumber, columnNumber, "Discount Amount cannot be greater than 100 or less than 0")
                                    Exit Sub
                                End If

                                If IsDBNull(dr.Item(VendorRowColumns.DiscountType)) Then
                                    SetErrorIndex(rowNumber, VendorRowColumns.DiscountType, "Missing Discount Type")
                                    Exit Sub
                                End If
                            Else
                                SetErrorIndex(rowNumber, columnNumber, "Invalid Discount Amount")
                                Exit Sub
                            End If
                        End If

                        order.Notes = dr.Item(VendorRowColumns.Notes).ToString
                        If Len(order.Notes) > 245 Then          ' Cut the string according to field limit
                            order.Notes = order.Notes.Substring(0, 244)
                        End If

                        order.Store = st
                        ' **** Get The PO ****
                        columnNumber = st.ColumnOrdinal
                        With po
                            .PONumber = CInt(dr.Item(st.ColumnOrdinal))
                            .PONumberID = FindPONumberID(CInt(dr.Item(st.ColumnOrdinal)))
                            .POType = CInt(dr.Item(st.ColumnOrdinal).ToString.Substring(0, 1))
                            .RegionID = CInt(dr.Item(st.ColumnOrdinal).ToString.Substring(1, 2))
                        End With
                        order.PONumber = po


                        ' **** Add PO Number to List for dupe-checking *****
                        If poList.Contains(po.PONumber) Then
                            SetErrorIndex(rowNumber, columnNumber, "Duplicate PO")
                            Exit Sub
                        Else
                            poList.Add(po.PONumber)
                            '  NEW to set ponumber status as used as they are used in spreadsheet once - AM
                            Dim dal As New DAOValidatedPOs
                            dal.PoNumberUpdate(po.PONumber)
                            'end NEW
                        End If

                        ' **** FIND iTEM ROWS ************
                    ElseIf ItemRows.IsMatch(Trim(dr.Item(ItemRowColumns.Identifier).ToString)) = True Then

                        Dim oi As New OrderItemObject()
                        ' **** If Current Order object doesn't exist - Exit Sub ******
                        If order.PONumber Is Nothing Then
                            SetErrorIndex(rowNumber, -1, "Missing PO Header")
                            Exit Sub
                        End If

                        ' **** Identifier *****
                        columnNumber = ItemRowColumns.Identifier
                        oi.UPC = Trim(dr.Item(ItemRowColumns.Identifier).ToString)

                        ' **** VIN *****
                        columnNumber = ItemRowColumns.VendorItemNumber
                        If Trim(dr.Item(ItemRowColumns.VendorItemNumber).ToString) = String.Empty Or _
                        dr.Item(ItemRowColumns.VendorItemNumber).ToString.Length > 20 Then
                            SetErrorIndex(rowNumber, columnNumber, "Invalid VIN Number")
                            Exit Sub
                        End If
                        oi.VIN = Trim(dr.Item(ItemRowColumns.VendorItemNumber).ToString)

                        ' **** ItemBrand *****
                        columnNumber = ItemRowColumns.ItemBrand
                        If Trim(dr.Item(ItemRowColumns.ItemBrand).ToString) = String.Empty Then
                            SetErrorIndex(rowNumber, columnNumber, "Invalid Item Brand")
                            Exit Sub
                        End If
                        oi.ItemBrand = Trim(dr.Item(ItemRowColumns.ItemBrand).ToString)
                        If Len(oi.ItemBrand) > 25 Then      ' Cut the string according to field limit
                            oi.ItemBrand = oi.ItemBrand.Substring(0, 24)
                        End If

                        ' **** ItemDescription *****
                        columnNumber = ItemRowColumns.ItemDescription
                        If Trim(dr.Item(ItemRowColumns.ItemDescription).ToString) = String.Empty Then
                            SetErrorIndex(rowNumber, columnNumber, "Invalid Item Description")
                            Exit Sub
                        End If
                        oi.ItemDescription = Trim(dr.Item(ItemRowColumns.ItemDescription).ToString)
                        If Len(oi.ItemDescription) > 60 Then        ' Cut the string according to field limit
                            oi.ItemDescription = oi.ItemDescription.Substring(0, 59)
                        End If

                        ' **** Discount Type *****
                        columnNumber = ItemRowColumns.DiscountType
                        If dr.Item(ItemRowColumns.DiscountType).ToString <> String.Empty Then
                            If DiscountType.IsMatch(dr.Item(ItemRowColumns.DiscountType).ToString) Then
                                oi.DiscountType = dr.Item(ItemRowColumns.DiscountType).ToString
                            Else
                                SetErrorIndex(rowNumber, columnNumber, "Invalid Discount Type")
                                Exit Sub
                            End If

                            If IsDBNull(dr.Item(ItemRowColumns.DiscountAmount)) Then
                                SetErrorIndex(rowNumber, ItemRowColumns.DiscountAmount, "Missing Discount Amount")
                                Exit Sub
                            End If

                            'check for missing/invalid reason code AM
                            columnNumber = ItemRowColumns.ReasonCode
                            If dr.Item(ItemRowColumns.ReasonCode).ToString = "" Then
                                SetErrorIndex(rowNumber, columnNumber, "Missing Reason Code")
                                Exit Sub
                            ElseIf Val.IsNotLetters(dr.Item(ItemRowColumns.ReasonCode).ToString) = True Then
                                SetErrorIndex(rowNumber, columnNumber, "Invalid Reason Code")
                                Exit Sub
                            ElseIf dr.Item(ItemRowColumns.ReasonCode).ToString.Length > 2 Then
                                SetErrorIndex(rowNumber, columnNumber, "Invalid Reason Code")
                                Exit Sub
                            Else
                                If MyReasonCodes.ContainsKey(dr.Item(ItemRowColumns.ReasonCode).ToString.ToUpper) Then
                                    oi.ItemReasonCode = MyReasonCodes.Item(dr.Item(ItemRowColumns.ReasonCode).ToString.ToUpper)
                                Else
                                    SetErrorIndex(rowNumber, columnNumber, "Invalid Reason Code")
                                    Exit Sub
                                End If
                            End If
                        End If

                        ' **** Discount Amount *****
                        columnNumber = ItemRowColumns.DiscountAmount
                        If dr.Item(ItemRowColumns.DiscountAmount).ToString <> String.Empty Then
                            If DiscountAmount.IsMatch(dr.Item(ItemRowColumns.DiscountAmount)) Then
                                oi.DiscountAmount = dr.Item(ItemRowColumns.DiscountAmount)

                                If IsDBNull(dr.Item(ItemRowColumns.DiscountType)) Then
                                    SetErrorIndex(rowNumber, ItemRowColumns.DiscountType, "Missing Discount Type")
                                    Exit Sub
                                End If

                                If dr.Item(ItemRowColumns.DiscountType).ToString = "FF" Then
                                    oi.FreeQuantity = oi.DiscountAmount
                                ElseIf (dr.Item(ItemRowColumns.DiscountType).ToString = "%" And (oi.DiscountAmount > 100.0 Or oi.DiscountAmount < 0.0)) Then
                                    SetErrorIndex(rowNumber, columnNumber, "Discount Amount cannot be greater than 100 or less than 0")
                                    Exit Sub
                                End If
                            Else
                                SetErrorIndex(rowNumber, columnNumber, "Invalid Discount Amount")
                                Exit Sub
                            End If
                        End If

                        oi.RowOrdinal = rowNumber

                        columnNumber = st.ColumnOrdinal
                        oi.OrderQuantity = CInt(dr.Item(st.ColumnOrdinal))

                        ' BUG 11521 BR - added check for orderquantity so validation 
                        ' would not occur on non-authed items with qty = 0
                        If oi.OrderQuantity > 0 Then
                            orderItemlist.Add(oi)
                        End If
                    End If

                    'TFS 6109, 05/03/2012, Faisal Ahmed - Validation for UPC from 3rd row which is an item row
                    If (rowNumber > 1 And isVendorRow = False) Then
                        If Trim(dr.Item(ItemRowColumns.Identifier).ToString) = String.Empty And Not Trim(dr.Item(ItemRowColumns.VendorItemNumber).ToString) = String.Empty Then
                            SetErrorIndex(rowNumber, 0, "Missing UPC Number")
                            Exit Sub
                        Else
                            If Val.HasSpecialCharecter(dr.Item(ItemRowColumns.Identifier).ToString) Or inValidUPC(dr.Item(ItemRowColumns.Identifier).ToString) Then
                                SetErrorIndex(rowNumber, 0, "Invalid UPC Number")
                                Exit Sub
                            End If
                        End If

                    End If

                    rowNumber += 1

                End While 'HeaderLoop
            End While 'StoreLoop

            If orderItemlist.Count > 0 Then
                order.OrderItems = orderItemlist
                If order.GetTotalItemQuantity > 0 Then
                    orderList.Add(order)
                End If
                order = Nothing
                orderItemlist = Nothing
            End If

            If orderList.Count > 0 Then
                '***** Final OrderList ******
                _orderList = orderList
                IsValidated = True
            End If

        Catch ex As Exception
            Debug.WriteLine(ex.Message)
            Debug.WriteLine(ex.StackTrace)
            SetErrorIndex(rowNumber, columnNumber)
        End Try
    End Sub

    '05/03/2012, Faisal Ahmed 
    'This function returns true if the input string does not contains all digits
    Public Function inValidUPC(ByVal str As String) As Boolean

        Dim pattern As String = "^\d+$"
        Dim reg As New Regex(pattern)
        Return Not reg.IsMatch(str)

    End Function

    Private Sub PONumberLookUp()

        Dim dal As New DALSpreadsheetValidation

        POLookUp = dal.GetPONumberIDs(UserID)

    End Sub

    Private Function FindPONumberID(ByVal PONumber As Integer) As Integer

        Dim dr() As DataRow
        Dim str As New StringBuilder

        str.Append("PONUmber = ")
        str.AppendFormat(CStr(PONumber))

        dr = POLookUp.Tables(0).Select(str.ToString)

        If Not dr(0)(0) Is Nothing Then
            Return CInt(dr(0)(0))
        Else
            Return 0
        End If

    End Function

    ''' <summary>
    ''' Set Row/Column Error index values for WebGrid
    ''' </summary>
    Private Sub SetErrorIndex(ByVal row As Integer, ByVal col As Integer, Optional ByVal message As String = "-Invalid or Missing Entry-")

        IsValidated = False
        _errorRowIndex = row
        _errorColumnIndex = col
        _errorMessage = message

    End Sub

#End Region

End Class


