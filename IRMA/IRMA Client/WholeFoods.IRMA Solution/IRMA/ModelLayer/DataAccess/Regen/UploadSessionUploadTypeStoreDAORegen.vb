

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

	''' <summary>
	''' Generated Data Access Object base class for the UploadSessionUploadTypeStore db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	David Marine
	''' Created   :	Jul 29, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class UploadSessionUploadTypeStoreDAORegen
    
		''' <summary>
		''' Returns a list of UploadSessionUploadTypeStores from the provided resultset.
		''' </summary>
		''' <returns>ArrayList of UploadSessionUploadTypeStores</returns>
		''' <remarks></remarks>
		Protected Overridable Function GetUploadSessionUploadTypeStoresFromResultSet (ByVal results As SqlDataReader) As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim businessObject As UploadSessionUploadTypeStore
			
			While results.Read

				businessObject = New UploadSessionUploadTypeStore()

				businessObject.UploadSessionUploadTypeStoreID = results.GetInt32(results.GetOrdinal("UploadSessionUploadTypeStore_ID"))                         

				' UploadSessionUploadType_ID can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("UploadSessionUploadType_ID"))) Then
					' the businessObject.UploadSessionUploadTypeID property cannot be set to null
					' so set the businessObject.IsUploadSessionUploadTypeIDNull flag to true.
					businessObject.IsUploadSessionUploadTypeIDNull = True
				Else
					businessObject.UploadSessionUploadTypeID = results.GetInt32(results.GetOrdinal("UploadSessionUploadType_ID"))                         
				End If

				' Store_No can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Store_No"))) Then
					' the businessObject.StoreNo property cannot be set to null
					' so set the businessObject.IsStoreNoNull flag to true.
					businessObject.IsStoreNoNull = True
				Else
					businessObject.StoreNo = results.GetInt32(results.GetOrdinal("Store_No"))                         
				End If
				
				' reset all the flags
				businessObject.IsNew = False
				businessObject.IsDirty = False
				businessObject.IsMarkedForDelete = False
				businessObject.IsDeleted = False

				' add business object to BusinessObjectCollection with abstracted PK key
				businessObjectTable.Add(businessObject.PrimaryKey, businessObject)
				
			End While

			Trace.WriteLine("Loading " + businessObjectTable.Count.ToString() + " UploadSessionUploadTypeStores from the database.")
			
			Return businessObjectTable

		End Function
		
		''' <summary>
		''' Set this in the UploadSessionUploadTypeStoreDAO sub class to change the stored
		''' procedure used by the GetAllUploadSessionUploadTypeStores function.
		''' </summary>
		Public GetAllUploadSessionUploadTypeStoresStoredProcName As String = "EIM_Regen_GetAllUploadSessionUploadTypeStores"
		
		''' <summary>
		''' Returns all UploadSessionUploadTypeStores.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetAllUploadSessionUploadTypeStores () As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim results As SqlDataReader = Nothing

			Try
			
				' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetAllUploadSessionUploadTypeStoresStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetAllUploadSessionUploadTypeStoresStoredProcName, paramList)
				End If
				
				businessObjectTable = GetUploadSessionUploadTypeStoresFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadSessionUploadTypeStoreDAO sub class to change the stored
		''' procedure used by the GetUploadSessionUploadTypeStoreByPK function.
		''' </summary>
		Protected GetUploadSessionUploadTypeStoreByPKStoredProcName As String = "EIM_Regen_GetUploadSessionUploadTypeStoreByPK"
		
		''' <summary>
		''' Returns zero or more UploadSessionUploadTypeStores by PK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadSessionUploadTypeStoreByPK (ByRef uploadSessionUploadTypeStoreID As System.Int32) As UploadSessionUploadTypeStore


			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim results As SqlDataReader = Nothing

			Try
				' setup identifier for stored proc
				currentParam = New DBParam
				currentParam.Name = "UploadSessionUploadTypeStore_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = uploadSessionUploadTypeStoreID 
				paramList.Add(currentParam)

								' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetUploadSessionUploadTypeStoreByPKStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadSessionUploadTypeStoreByPKStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadSessionUploadTypeStoresFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Dim businessObject As UploadSessionUploadTypeStore = Nothing
            If businessObjectTable.Count > 0 Then
                businessObject = CType(businessObjectTable.Item(0), UploadSessionUploadTypeStore)
            End If
			
			Return businessObject

		End Function


		''' <summary>
		''' Set this in the UploadSessionUploadTypeStoreDAO sub class to change the stored
		''' procedure used by the GetUploadSessionUploadTypeStoresByUploadSessionUploadTypeID function.
		''' </summary>
		Protected GetUploadSessionUploadTypeStoresByUploadSessionUploadTypeIDStoredProcName As String = "EIM_Regen_GetUploadSessionUploadTypeStoresByUploadSessionUploadTypeID"
		
		''' <summary>
		''' Returns UploadSessionUploadTypeStores by a uploadSessionUploadTypeID FK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadSessionUploadTypeStoresByUploadSessionUploadTypeID (ByRef uploadSessionUploadTypeID As System.Int32) As BusinessObjectCollection


			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim results As SqlDataReader = Nothing

			Try
				' setup identifier for stored proc
				currentParam = New DBParam
				currentParam.Name = "UploadSessionUploadType_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = uploadSessionUploadTypeID 
				paramList.Add(currentParam)

								' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetUploadSessionUploadTypeStoresByUploadSessionUploadTypeIDStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadSessionUploadTypeStoresByUploadSessionUploadTypeIDStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadSessionUploadTypeStoresFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadSessionUploadTypeStoreDAO sub class to change the stored
		''' procedure used by the GetUploadSessionUploadTypeStoresByStoreNo function.
		''' </summary>
		Protected GetUploadSessionUploadTypeStoresByStoreNoStoredProcName As String = "EIM_Regen_GetUploadSessionUploadTypeStoresByStoreNo"
		
		''' <summary>
		''' Returns UploadSessionUploadTypeStores by a storeNo FK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadSessionUploadTypeStoresByStoreNo (ByRef storeNo As System.Int32) As BusinessObjectCollection


			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim results As SqlDataReader = Nothing

			Try
				' setup identifier for stored proc
				currentParam = New DBParam
				currentParam.Name = "Store_No"
				currentParam.Type = DBParamType.Int
				currentParam.Value = storeNo 
				paramList.Add(currentParam)

								' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetUploadSessionUploadTypeStoresByStoreNoStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadSessionUploadTypeStoresByStoreNoStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadSessionUploadTypeStoresFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadSessionUploadTypeStoreDAO sub class to change the stored
		''' procedure used by the GetAllUploadSessionUploadTypeStores function.
		''' </summary>
		Public InsertUploadSessionUploadTypeStoreStoredProcName As String = "EIM_Regen_InsertUploadSessionUploadTypeStore"

		Public Overridable Sub InsertUploadSessionUploadTypeStore(ByRef businessObject As UploadSessionUploadTypeStore)

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim pkValueList As New ArrayList
            
			' setup parameters for stored proc

				' UploadSessionUploadType_ID
				currentParam = New DBParam
				currentParam.Name = "UploadSessionUploadType_ID"
				currentParam.Type = DBParamType.Int
				If businessObject.IsUploadSessionUploadTypeIDNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.UploadSessionUploadTypeID
				End If

				paramList.Add(currentParam)

				' Store_No
				currentParam = New DBParam
				currentParam.Name = "Store_No"
				currentParam.Type = DBParamType.Int
				If businessObject.IsStoreNoNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.StoreNo
				End If

				paramList.Add(currentParam)

			' Get the output value
			currentParam = New DBParam
			currentParam.Name = "UploadSessionUploadTypeStore_ID"
			currentParam.Type = DBParamType.Int
			paramList.Add(currentParam)

			' get the current transaction if any
			Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
			
			' Execute the stored procedure
			If Not IsNothing(theTransaction) Then
				pkValueList = factory.ExecuteStoredProcedure(InsertUploadSessionUploadTypeStoreStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				pkValueList = factory.ExecuteStoredProcedure(InsertUploadSessionUploadTypeStoreStoredProcName, paramList)
			End If

			' set the returned pk value
			businessObject.UploadSessionUploadTypeStoreID = CInt(pkValueList(0))

		End Sub
		
		''' <summary>
		''' Set this in the UploadSessionUploadTypeStoreDAO sub class to change the stored
		''' procedure used by the GetAllUploadSessionUploadTypeStores function.
		''' </summary>
		Public UpdateUploadSessionUploadTypeStoreStoredProcName As String = "EIM_Regen_UpdateUploadSessionUploadTypeStore"

		Public Overridable Function UpdateUploadSessionUploadTypeStore(ByVal businessObject As UploadSessionUploadTypeStore) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' UploadSessionUploadTypeStore_ID
				currentParam = New DBParam
				currentParam.Name = "UploadSessionUploadTypeStore_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadSessionUploadTypeStoreID

				paramList.Add(currentParam)

				' UploadSessionUploadType_ID
				currentParam = New DBParam
				currentParam.Name = "UploadSessionUploadType_ID"
				currentParam.Type = DBParamType.Int
				If businessObject.IsUploadSessionUploadTypeIDNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.UploadSessionUploadTypeID
				End If

				paramList.Add(currentParam)

				' Store_No
				currentParam = New DBParam
				currentParam.Name = "Store_No"
				currentParam.Type = DBParamType.Int
				If businessObject.IsStoreNoNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.StoreNo
				End If

				paramList.Add(currentParam)

			' Get the output value
			currentParam = New DBParam
			currentParam.Name = "UpdateCount"
			currentParam.Type = DBParamType.Int
			paramList.Add(currentParam)

			' get the current transaction if any
			Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
			
			' Execute the stored procedure
			If Not IsNothing(theTransaction) Then
				outputList = factory.ExecuteStoredProcedure(UpdateUploadSessionUploadTypeStoreStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				outputList = factory.ExecuteStoredProcedure(UpdateUploadSessionUploadTypeStoreStoredProcName, paramList)
			End If

			Return CInt(outputList(0))

		End Function
		
		''' <summary>
		''' Set this in the UploadSessionUploadTypeStoreDAO sub class to change the stored
		''' procedure used by the GetAllUploadSessionUploadTypeStores function.
		''' </summary>
		Public DeleteUploadSessionUploadTypeStoreStoredProcName As String = "EIM_Regen_DeleteUploadSessionUploadTypeStore"

		Public Overridable Function DeleteUploadSessionUploadTypeStore(ByVal businessObject As UploadSessionUploadTypeStore) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' UploadSessionUploadTypeStore_ID
				currentParam = New DBParam
				currentParam.Name = "UploadSessionUploadTypeStore_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadSessionUploadTypeStoreID

				paramList.Add(currentParam)

			' Get the output value
			currentParam = New DBParam
			currentParam.Name = "DeleteCount"
			currentParam.Type = DBParamType.Int
			paramList.Add(currentParam)
			
			' get the current transaction if any
			Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction

			' Execute the stored procedure
			If Not IsNothing(theTransaction) Then
				outputList = factory.ExecuteStoredProcedure(DeleteUploadSessionUploadTypeStoreStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				outputList = factory.ExecuteStoredProcedure(DeleteUploadSessionUploadTypeStoreStoredProcName, paramList)
			End If

			Return CInt(outputList(0))

		End Function

    End Class

End Namespace
