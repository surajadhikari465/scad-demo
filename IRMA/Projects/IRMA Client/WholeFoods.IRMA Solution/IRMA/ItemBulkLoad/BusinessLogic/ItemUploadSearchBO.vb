Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemBulkLoad.DataAccess
Namespace WholeFoods.IRMA.ItemBulkLoad.BusinessLogic

    Public Class ItemUploadSearchBO

        Private _itemUploadHeaderID As Integer
        Private _itemUploadTypeID As Integer
        Private _userID As Integer
        Private _createDate As Date
        Private _detailColl As Collection

        Property DetailColl() As Collection
            Get
                Return _detailColl
            End Get
            Set(ByVal value As Collection)
                _detailColl = value
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

        Property ItemUploadTypeID() As Integer
            Get
                Return _itemUploadTypeID
            End Get
            Set(ByVal value As Integer)
                _itemUploadTypeID = value
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

        Property CreateDate() As Date
            Get
                Return _createDate
            End Get
            Set(ByVal value As Date)
                _createDate = value
            End Set
        End Property

        Public Sub DefaultValues()
            _itemUploadHeaderID = 0
            _itemUploadTypeID = 0
            _userID = 0
            _createDate = Nothing
            _detailColl = Nothing
        End Sub

        Public Function Search() As DataTable
            Dim itemUploadDAO As New ItemUploadDAO
            Dim row As DataRow
            Dim table As New DataTable("ItemUploadHeader")
            Dim results As SqlDataReader = Nothing

            Try
                results = itemUploadDAO.ItemUploadSearch(Me)

                'add columns to table
                table.Columns.Add(New DataColumn("ItemUploadHeaderID", GetType(Integer)))
                table.Columns.Add(New DataColumn("ItemsProcessedCount", GetType(Integer)))
                table.Columns.Add(New DataColumn("ItemsLoadedCount", GetType(Integer)))
                table.Columns.Add(New DataColumn("ErrorsCount", GetType(Integer)))
                table.Columns.Add(New DataColumn("UploadedDateTime", GetType(Date)))

                While results.Read
                    row = table.NewRow

                    If results.GetValue(results.GetOrdinal("ItemUploadHeader_ID")).GetType IsNot GetType(DBNull) Then
                        row("ItemUploadHeaderID") = results.GetInt32(results.GetOrdinal("ItemUploadHeader_ID"))
                    End If
                    If results.GetValue(results.GetOrdinal("ItemsProcessedCount")).GetType IsNot GetType(DBNull) Then
                        row("ItemsProcessedCount") = results.GetInt32(results.GetOrdinal("ItemsProcessedCount"))
                    End If
                    If results.GetValue(results.GetOrdinal("ItemsLoadedCount")).GetType IsNot GetType(DBNull) Then
                        row("ItemsLoadedCount") = results.GetInt32(results.GetOrdinal("ItemsLoadedCount"))
                    End If
                    If results.GetValue(results.GetOrdinal("ErrorsCount")).GetType IsNot GetType(DBNull) Then
                        row("ErrorsCount") = results.GetInt32(results.GetOrdinal("ErrorsCount"))
                    End If
                    If results.GetValue(results.GetOrdinal("UploadDateTime")).GetType IsNot GetType(DBNull) Then
                        row("UploadedDateTime") = results.GetDateTime(results.GetOrdinal("UploadDateTime"))
                    End If

                    table.Rows.Add(row)

                End While

                ' reset property values back to defaults in preparation for next search
                Me.DefaultValues()

            Catch e As Exception
                'TODO handle exception
                Throw e
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return table

        End Function

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
    End Class

End Namespace
