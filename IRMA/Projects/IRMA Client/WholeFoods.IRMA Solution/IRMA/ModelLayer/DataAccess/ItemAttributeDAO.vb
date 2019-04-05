

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports System.Linq

Namespace WholeFoods.IRMA.ModelLayer.DataAccess
    Public Enum ItemAttributeValidationStatus
        Valid
        Error_Text1InvalidCharacters
        Error_Text2InvalidCharacters
        Error_Text3InvalidCharacters
        Error_Text4InvalidCharacters
        Error_Text5InvalidCharacters
        Error_Text6InvalidCharacters
        Error_Text7InvalidCharacters
        Error_Text8InvalidCharacters
        Error_Text9InvalidCharacters
        Error_Text10InvalidCharacters
    End Enum

    ''' <summary>
    ''' Generated Data Access Object class for the ItemAttribute db table.
    '''
    ''' This class inherits the following CRUD methods from its regenerable base class:
    '''    1. Find method by PK
    '''    2. Find method for each FK
    '''    3. Insert Method
    '''    4. Update Method
    '''    5. Delete Method
    '''
    ''' MODIFY THIS CLASS, NOT THE BASE CLASS.
    '''
    ''' Created By:	James Winfield
    ''' Created   :	Feb 23, 2007
    ''' </summary>
    ''' <remarks></remarks>
    Public Class ItemAttributeDAO
        Inherits ItemAttributeDAORegen

#Region "Static Singleton Accessor"

        Private Shared _instance As ItemAttributeDAO = Nothing

        Public Shared ReadOnly Property Instance() As ItemAttributeDAO
            Get
                If IsNothing(_instance) Then
                    _instance = New ItemAttributeDAO()
                End If

                Return _instance
            End Get
        End Property

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Returns the item key for the provided item identifier.
        ''' </summary>
        ''' <returns>Integer</returns>
        ''' <remarks></remarks>
        Public Overridable Function GetItemKeyByIdentifier(ByVal identifier As String) As Integer

            Dim itemKey As Integer = -1
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            Try
                ' setup identifier for stored proc
                currentParam = New DBParam
                currentParam.Name = "Identifier"
                currentParam.Type = DBParamType.String
                currentParam.Value = identifier
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetItemInfoByIdentifier", paramList)

                If results.Read Then
                    itemKey = results.GetInt32(results.GetOrdinal("Item_Key"))
                End If

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return itemKey

        End Function

        ''' <summary>
        ''' Set this in the ItemAttributeDAO sub class to change the stored
        ''' procedure used by the GetItemAttributeByPK function.
        ''' </summary>
        Protected GetItemAttributeByItemKeyStoredProcName As String = "ItemAttributes_GetItemAttributeByItemKey"

        ''' <summary>
        ''' Returns zero or more ItemAttributes by PK value.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function GetItemAttributeByItemKey(ByRef itemKey As System.Int32) As ItemAttribute


            Dim businessObjectTable As New BusinessObjectCollection
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            Try
                ' setup identifier for stored proc
                currentParam = New DBParam
                currentParam.Name = "Item_Key"
                currentParam.Type = DBParamType.Int
                currentParam.Value = itemKey
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader(Me.GetItemAttributeByItemKeyStoredProcName, paramList)

                businessObjectTable = GetItemAttributesFromResultSet(results)

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Dim businessObject As ItemAttribute = Nothing
            If businessObjectTable.Count > 0 Then
                businessObject = CType(businessObjectTable.Item(0), ItemAttribute)
            End If

            Return businessObject

        End Function

        Public Const INVALID_CHARACTERS = "|"

        Public Overridable Function ValidateItemAttributeText(ByVal businessObject As ItemAttribute) As ArrayList
            Dim statusList As New ArrayList

            If Not String.IsNullOrEmpty(businessObject.Text1) Then
                If businessObject.Text1.ToCharArray().Any(Function(c) INVALID_CHARACTERS.Contains(c)) Then
                    statusList.Add(ItemAttributeValidationStatus.Error_Text1InvalidCharacters)
                End If
            End If
            If Not String.IsNullOrEmpty(businessObject.Text2) Then
                If businessObject.Text2.ToCharArray().Any(Function(c) INVALID_CHARACTERS.Contains(c)) Then
                    statusList.Add(ItemAttributeValidationStatus.Error_Text2InvalidCharacters)
                End If
            End If
            If Not String.IsNullOrEmpty(businessObject.Text3) Then
                If businessObject.Text3.ToCharArray().Any(Function(c) INVALID_CHARACTERS.Contains(c)) Then
                    statusList.Add(ItemAttributeValidationStatus.Error_Text3InvalidCharacters)
                End If
            End If
            If Not String.IsNullOrEmpty(businessObject.Text4) Then
                If businessObject.Text4.ToCharArray().Any(Function(c) INVALID_CHARACTERS.Contains(c)) Then
                    statusList.Add(ItemAttributeValidationStatus.Error_Text4InvalidCharacters)
                End If
            End If
            If Not String.IsNullOrEmpty(businessObject.Text5) Then
                If businessObject.Text5.ToCharArray().Any(Function(c) INVALID_CHARACTERS.Contains(c)) Then
                    statusList.Add(ItemAttributeValidationStatus.Error_Text5InvalidCharacters)
                End If
            End If
            If Not String.IsNullOrEmpty(businessObject.Text6) Then
                If businessObject.Text6.ToCharArray().Any(Function(c) INVALID_CHARACTERS.Contains(c)) Then
                    statusList.Add(ItemAttributeValidationStatus.Error_Text6InvalidCharacters)
                End If
            End If
            If Not String.IsNullOrEmpty(businessObject.Text7) Then
                If businessObject.Text7.ToCharArray().Any(Function(c) INVALID_CHARACTERS.Contains(c)) Then
                    statusList.Add(ItemAttributeValidationStatus.Error_Text7InvalidCharacters)
                End If
            End If
            If Not String.IsNullOrEmpty(businessObject.Text8) Then
                If businessObject.Text8.ToCharArray().Any(Function(c) INVALID_CHARACTERS.Contains(c)) Then
                    statusList.Add(ItemAttributeValidationStatus.Error_Text8InvalidCharacters)
                End If
            End If
            If Not String.IsNullOrEmpty(businessObject.Text9) Then
                If businessObject.Text9.ToCharArray().Any(Function(c) INVALID_CHARACTERS.Contains(c)) Then
                    statusList.Add(ItemAttributeValidationStatus.Error_Text9InvalidCharacters)
                End If
            End If
            If Not String.IsNullOrEmpty(businessObject.Text10) Then
                If businessObject.Text10.ToCharArray().Any(Function(c) INVALID_CHARACTERS.Contains(c)) Then
                    statusList.Add(ItemAttributeValidationStatus.Error_Text10InvalidCharacters)
                End If
            End If

            Return statusList
        End Function
#End Region

    End Class

End Namespace
