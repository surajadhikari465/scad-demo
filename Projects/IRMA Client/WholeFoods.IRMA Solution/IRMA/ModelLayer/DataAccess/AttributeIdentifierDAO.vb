

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.IRMA.ModelLayer.DataAccess
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

	''' <summary>
	''' Generated Data Access Object class for the AttributeIdentifier db table.
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
	Public Class AttributeIdentifierDAO
		Inherits AttributeIdentifierDAORegen
	
#Region "Static Singleton Accessor"
	
		Private Shared _instance As AttributeIdentifierDAO = Nothing
	
        Public Shared ReadOnly Property Instance() As AttributeIdentifierDAO
            Get
            	If IsNothing(_instance) Then
            		_instance = New AttributeIdentifierDAO()
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

        Public GetAttributeIdentifiersByItemKeyStoredProcName As String = "ItemAttributes_GetAttributeIdentifiersByItemKey"

        ''' <summary>
        ''' Returns all AttributeIdentifiers with a given Item_Key.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function GetAttributeIdentifiersByItemKey(ByRef itemKey As System.Int32) As BusinessObjectCollection

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
                results = factory.GetStoredProcedureDataReader(Me.GetAttributeIdentifiersByItemKeyStoredProcName, paramList)

                businessObjectTable = GetAttributeIdentifiersFromResultSet(results)

            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return businessObjectTable

        End Function

#End Region

    End Class

End Namespace
