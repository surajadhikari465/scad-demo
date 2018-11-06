Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient
Namespace WholeFoods.IRMA.ItemBulkLoad.BusinessLogic


    Public Class ItemUploadHeaderBO

        Private _itemUploadTypeID As Integer
        Private _itemProcessedCount As Integer
        Private _itemLoadedCount As Integer
        Private _errorsCount As Integer
        Private _emailToAddress As String
        Private _userID As Integer
        Private _itemUploadHeaderID As Integer
        Private _uploadDate As Date
        Private _detailColl As Collection

        Public Function InfoToString() As String
            Dim info As String
            info = String.Format("Upload ID: {0}  Processed {1} Items which loaded {2} Items with {3} Errors on {4}", _
                ItemUploadHeaderID.ToString(), _
                ItemProcessedCount.ToString(), _
                ItemLoadedCount.ToString(), _
                ErrorsCount.ToString(), _
                UploadDate.Date.ToString("M/d/yyyy"))
            Return info
        End Function
        ReadOnly Property UploadResults() As String
            Get
                Dim message As String

                message = String.Format(ResourcesItemBulkLoad.GetString("UploadResults"), _
                    vbCrLf & vbCrLf, ItemUploadHeaderID, vbCrLf & vbCrLf, ItemProcessedCount, vbCrLf, _
                    ItemLoadedCount, vbCrLf, ErrorsCount, vbCrLf)

                Return message
            End Get
        End Property
        Property UploadDate() As Date
            Get
                Return _uploadDate
            End Get
            Set(ByVal value As Date)
                _uploadDate = value
            End Set
        End Property
        Property DetailColl() As Collection
            Get
                Return _detailColl
            End Get
            Set(ByVal value As Collection)
                _detailColl = value
            End Set
        End Property

        Property ItemUploadTypeID() As Integer
            Get
                Return _itemUploadTypeID
            End Get
            Set(ByVal value As Integer)
                _itemUploadTypeID = value
            End Set
        End Property

        Property ItemProcessedCount() As Integer
            Get
                Return _itemProcessedCount
            End Get
            Set(ByVal value As Integer)
                _itemProcessedCount = value
            End Set
        End Property

        Property ItemLoadedCount() As Integer
            Get
                Return _itemLoadedCount
            End Get
            Set(ByVal value As Integer)
                _itemLoadedCount = value
            End Set
        End Property

        Property ErrorsCount() As Integer
            Get
                Return _errorsCount
            End Get
            Set(ByVal value As Integer)
                _errorsCount = value
            End Set
        End Property


        Property EmailToAddress() As String
            Get
                Return _emailToAddress
            End Get
            Set(ByVal value As String)
                _emailToAddress = value
            End Set
        End Property

        Property UserID() As Integer
            Get
                Return _userID
            End Get
            Set(ByVal value As Integer)
                _userID = value
            End Set
        End Property

        Property ItemUploadHeaderID() As Integer
            Get
                Return _itemUploadHeaderID
            End Get
            Set(ByVal value As Integer)
                _itemUploadHeaderID = value
            End Set
        End Property

        Public Sub Insert()
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim reader As New ArrayList
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim transaction As SqlTransaction = Nothing

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ItemUploadType_ID"
                If ItemUploadTypeID <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = ItemUploadTypeID
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "EmailToAddress"
                currentParam.Value = EmailToAddress
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "User_ID"
                If UserID <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = UserID
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Get the output value
                currentParam = New DBParam
                currentParam.Name = "ItemUploadHeader_ID"
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                'start database transaction
                transaction = factory.BeginTransaction(IsolationLevel.ReadCommitted)

                ' Execute the stored procedure 
                reader = factory.ExecuteStoredProcedure("InsertItemUploadHeader", paramList, transaction)

                ItemUploadHeaderID = CInt(reader(0))

                ' Now insert the detail records
                For Each oneInst As ItemUploadDetailBO In DetailColl
                    paramList = New ArrayList()
                    oneInst.ItemUploadHeaderID = ItemUploadHeaderID
                    paramList = PrepareParamList(oneInst)

                    reader = factory.ExecuteStoredProcedure("InsertItemUploadDetail", paramList, transaction)
                Next oneInst

                transaction.Commit()

            Catch ex As Exception
                'TODO handle exception
                Throw ex
            End Try
        End Sub

        Private Function PrepareParamList(ByVal detailBO As ItemUploadDetailBO) As ArrayList
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ItemUploadHeader_ID"
                currentParam.Value = detailBO.ItemUploadHeaderID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ItemIdentifier"
                currentParam.Value = IIf(detailBO.ItemIdentifier Is Nothing, DBNull.Value, detailBO.ItemIdentifier)
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "POSDescription"
                currentParam.Value = IIf(detailBO.PosDescription Is Nothing, DBNull.Value, detailBO.PosDescription)
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Description"
                currentParam.Value = IIf(detailBO.Description Is Nothing, DBNull.Value, detailBO.Description)
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TaxClassID"
                currentParam.Value = IIf(detailBO.TaxClassID Is Nothing, DBNull.Value, detailBO.TaxClassID)
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "FoodStamps"
                currentParam.Value = IIf(detailBO.FoodStamps Is Nothing, DBNull.Value, detailBO.FoodStamps)
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "RestrictedHours"
                currentParam.Value = IIf(detailBO.RestrictedHours Is Nothing, DBNull.Value, detailBO.RestrictedHours)
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "EmployeeDiscountable"
                currentParam.Value = IIf(detailBO.EmployeeDiscountable Is Nothing, DBNull.Value, detailBO.EmployeeDiscountable)
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Discontinued"
                currentParam.Value = IIf(detailBO.Discontinued Is Nothing, DBNull.Value, detailBO.Discontinued)
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "NationalClassID"
                currentParam.Value = IIf(detailBO.NationalClassID Is Nothing, DBNull.Value, detailBO.NationalClassID)
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

            Catch ex As Exception
                'TODO handle exception
                Throw ex
            End Try

            Return paramList

        End Function

        Public Function Update() As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim reader As New ArrayList
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim transaction As SqlTransaction = Nothing
            Dim updateSuccessful As Boolean

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ItemUploadHeader_ID"
                If ItemUploadHeaderID <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = ItemUploadHeaderID
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ItemsProcessedCount"
                If ItemProcessedCount <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = ItemProcessedCount
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ItemsLoadedCount"
                If ItemLoadedCount <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = ItemLoadedCount
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ErrorsCount"
                If ErrorsCount <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = ErrorsCount
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                reader = factory.ExecuteStoredProcedure("UpdateItemUploadHeader", paramList)
                updateSuccessful = True
            Catch ex As Exception
                updateSuccessful = False
            End Try
            Return updateSuccessful
        End Function

        Public Sub Delete()
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim reader As New ArrayList
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim transaction As SqlTransaction = Nothing

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ItemUploadHeader_ID"
                If ItemUploadHeaderID <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = ItemUploadHeaderID
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                reader = factory.ExecuteStoredProcedure("DeleteItemUpload", paramList)

            Catch ex As Exception
                'TODO handle exception
                Throw ex
            End Try
        End Sub

    End Class
End Namespace
