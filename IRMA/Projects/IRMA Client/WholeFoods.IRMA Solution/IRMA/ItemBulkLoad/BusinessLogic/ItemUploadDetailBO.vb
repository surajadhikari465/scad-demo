
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient
Namespace WholeFoods.IRMA.ItemBulkLoad.BusinessLogic
    Public Class ItemUploadDetailBO

        Private _itemUploadHeaderID As Integer
        Private _itemIdentifier As String
        Private _posDescription As String
        Private _description As String
        Private _taxClassID As String
        Private _foodStamps As String
        Private _restrictedHours As String
        Private _employeeDiscountable As String
        Private _discontinued As String
        Private _nationalClassID As String
        Private _itemUploadError_ID As Integer
        Private _itemUploadDetail_ID As Integer
        Private _subTeamNo As Integer
        Private _itemIdentifierValid As Integer
        Private _subTeamAllowed As Integer
        Private _subTeamName As String
        Private _uploaded As Boolean
        Private _item_key As Integer

        Property Item_Key() As Integer
            Get
                Return _item_key
            End Get
            Set(ByVal value As Integer)
                _item_key = value
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

        Property ItemIdentifier() As String
            Get
                Return _itemIdentifier
            End Get
            Set(ByVal value As String)
                _itemIdentifier = value
            End Set
        End Property

        Property PosDescription() As String
            Get
                Return _posDescription
            End Get
            Set(ByVal value As String)
                _posDescription = value
            End Set
        End Property

        Property Description() As String
            Get
                Return _description
            End Get
            Set(ByVal value As String)
                _description = value
            End Set
        End Property

        Property TaxClassID() As String
            Get
                Return _taxClassID
            End Get
            Set(ByVal value As String)
                _taxClassID = value
            End Set
        End Property

        Property FoodStamps() As String
            Get
                Return _foodStamps
            End Get
            Set(ByVal value As String)
                _foodStamps = value
            End Set
        End Property


        Property RestrictedHours() As String
            Get
                Return _restrictedHours
            End Get
            Set(ByVal value As String)
                _restrictedHours = value
            End Set
        End Property

        Property EmployeeDiscountable() As String
            Get
                Return _employeeDiscountable
            End Get
            Set(ByVal value As String)
                _employeeDiscountable = value
            End Set
        End Property

        Property Discontinued() As String
            Get
                Return _discontinued
            End Get
            Set(ByVal value As String)
                _discontinued = value
            End Set
        End Property

        Property NationalClassID() As String
            Get
                Return _nationalClassID
            End Get
            Set(ByVal value As String)
                _nationalClassID = value
            End Set
        End Property

        Property ItemUploadError_ID() As Integer
            Get
                Return _itemUploadError_ID
            End Get
            Set(ByVal value As Integer)
                _itemUploadError_ID = value
            End Set
        End Property

        Property ItemUploadDetail_ID() As Integer
            Get
                Return _itemUploadDetail_ID
            End Get
            Set(ByVal value As Integer)
                _itemUploadDetail_ID = value
            End Set
        End Property

        Property SubTeamNo() As Integer
            Get
                Return _subTeamNo
            End Get
            Set(ByVal value As Integer)
                _subTeamNo = value
            End Set
        End Property

        Property ItemIdentifierValid() As Integer
            Get
                Return _itemIdentifierValid
            End Get
            Set(ByVal value As Integer)
                _itemIdentifierValid = value
            End Set
        End Property

        Property SubTeamAllowed() As Integer
            Get
                Return _subTeamAllowed
            End Get
            Set(ByVal value As Integer)
                _subTeamAllowed = value
            End Set
        End Property

        Property SubTeamName() As String
            Get
                Return _subTeamName
            End Get
            Set(ByVal value As String)
                _subTeamName = value
            End Set
        End Property

        Property Uploaded() As Boolean
            Get
                Return _uploaded
            End Get
            Set(ByVal value As Boolean)
                _uploaded = value
            End Set
        End Property

        Public Sub Update()
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim reader As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ItemUploadDetail_ID"
                If ItemUploadDetail_ID <= 0 Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = ItemUploadDetail_ID
                End If
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ItemIdentifier"
                If ItemIdentifier Is Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = ItemIdentifier
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "POSDescription"
                If PosDescription Is Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = PosDescription
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Description"
                If Description Is Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = Description
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "TaxClassID"
                If TaxClassID Is Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = TaxClassID
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "FoodStamps"
                If FoodStamps Is Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = FoodStamps
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "RestrictedHours"
                If RestrictedHours Is Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = RestrictedHours
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "EmployeeDiscountable"
                If EmployeeDiscountable Is Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = EmployeeDiscountable
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Discontinued"
                If Discontinued Is Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = Discontinued
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "NationalClassID"
                If NationalClassID Is Nothing Then
                    currentParam.Value = DBNull.Value
                Else
                    currentParam.Value = NationalClassID
                End If
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Uploaded"
                currentParam.Value = Uploaded
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                reader = factory.GetStoredProcedureDataReader("UpdateItemUploadDetail", paramList)

            Catch ex As Exception
                'TODO handle exception
                Throw ex
            End Try
        End Sub

        Public Sub New()

        End Sub
    End Class

End Namespace
