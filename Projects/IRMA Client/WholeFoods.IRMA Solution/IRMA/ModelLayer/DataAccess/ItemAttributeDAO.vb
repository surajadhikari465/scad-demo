

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

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


#End Region

    End Class

End Namespace
