

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

	''' <summary>
	''' Generated Data Access Object base class for the UploadRow db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	David Marine
	''' Created   :	Jul 29, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class UploadRowDAORegen
    
		''' <summary>
		''' Returns a list of UploadRows from the provided resultset.
		''' </summary>
		''' <returns>ArrayList of UploadRows</returns>
		''' <remarks></remarks>
		Protected Overridable Function GetUploadRowsFromResultSet (ByVal results As SqlDataReader) As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim businessObject As UploadRow
			
			While results.Read

				businessObject = New UploadRow()

				businessObject.UploadRowID = results.GetInt32(results.GetOrdinal("UploadRow_ID"))                         

				' Item_Key can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Item_Key"))) Then
					' the businessObject.ItemKey property cannot be set to null
					' so set the businessObject.IsItemKeyNull flag to true.
					businessObject.IsItemKeyNull = True
				Else
					businessObject.ItemKey = results.GetInt32(results.GetOrdinal("Item_Key"))                         
				End If
				businessObject.UploadSessionID = results.GetInt32(results.GetOrdinal("UploadSession_ID"))                         

				' Identifier can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Identifier"))) Then
					businessObject.Identifier = Nothing
				Else
					businessObject.Identifier = results.GetString(results.GetOrdinal("Identifier"))                         
				End If
				businessObject.ValidationLevel = results.GetInt32(results.GetOrdinal("ValidationLevel"))                         

				' ItemRequest_ID can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("ItemRequest_ID"))) Then
					' the businessObject.ItemRequestID property cannot be set to null
					' so set the businessObject.IsItemRequestIDNull flag to true.
					businessObject.IsItemRequestIDNull = True
				Else
					businessObject.ItemRequestID = results.GetInt32(results.GetOrdinal("ItemRequest_ID"))                         
				End If
				
				' reset all the flags
				businessObject.IsNew = False
				businessObject.IsDirty = False
				businessObject.IsMarkedForDelete = False
				businessObject.IsDeleted = False

				' add business object to BusinessObjectCollection with abstracted PK key
				businessObjectTable.Add(businessObject.PrimaryKey, businessObject)
				
			End While

			Trace.WriteLine("Loading " + businessObjectTable.Count.ToString() + " UploadRows from the database.")
			
			Return businessObjectTable

		End Function
		
		''' <summary>
		''' Set this in the UploadRowDAO sub class to change the stored
		''' procedure used by the GetAllUploadRows function.
		''' </summary>
		Public GetAllUploadRowsStoredProcName As String = "EIM_Regen_GetAllUploadRows"
		
		''' <summary>
		''' Returns all UploadRows.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetAllUploadRows () As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim results As SqlDataReader = Nothing

			Try
			
				' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetAllUploadRowsStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetAllUploadRowsStoredProcName, paramList)
				End If
				
				businessObjectTable = GetUploadRowsFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadRowDAO sub class to change the stored
		''' procedure used by the GetUploadRowByPK function.
		''' </summary>
		Protected GetUploadRowByPKStoredProcName As String = "EIM_Regen_GetUploadRowByPK"
		
		''' <summary>
		''' Returns zero or more UploadRows by PK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadRowByPK (ByRef uploadRowID As System.Int32) As UploadRow


			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim results As SqlDataReader = Nothing

			Try
				' setup identifier for stored proc
				currentParam = New DBParam
				currentParam.Name = "UploadRow_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = uploadRowID 
				paramList.Add(currentParam)

								' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetUploadRowByPKStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadRowByPKStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadRowsFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Dim businessObject As UploadRow = Nothing
            If businessObjectTable.Count > 0 Then
                businessObject = CType(businessObjectTable.Item(0), UploadRow)
            End If
			
			Return businessObject

		End Function


		''' <summary>
		''' Set this in the UploadRowDAO sub class to change the stored
		''' procedure used by the GetUploadRowsByUploadSessionID function.
		''' </summary>
		Protected GetUploadRowsByUploadSessionIDStoredProcName As String = "EIM_Regen_GetUploadRowsByUploadSessionID"
		
		''' <summary>
		''' Returns UploadRows by a uploadSessionID FK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadRowsByUploadSessionID (ByRef uploadSessionID As System.Int32) As BusinessObjectCollection


			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim results As SqlDataReader = Nothing

			Try
				' setup identifier for stored proc
				currentParam = New DBParam
				currentParam.Name = "UploadSession_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = uploadSessionID 
				paramList.Add(currentParam)

								' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetUploadRowsByUploadSessionIDStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadRowsByUploadSessionIDStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadRowsFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadRowDAO sub class to change the stored
		''' procedure used by the GetAllUploadRows function.
		''' </summary>
		Public InsertUploadRowStoredProcName As String = "EIM_Regen_InsertUploadRow"

		Public Overridable Sub InsertUploadRow(ByRef businessObject As UploadRow)

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim pkValueList As New ArrayList
            
			' setup parameters for stored proc

				' Item_Key
				currentParam = New DBParam
				currentParam.Name = "Item_Key"
				currentParam.Type = DBParamType.Int
				If businessObject.IsItemKeyNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ItemKey
				End If

				paramList.Add(currentParam)

				' UploadSession_ID
				currentParam = New DBParam
				currentParam.Name = "UploadSession_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadSessionID

				paramList.Add(currentParam)

				' Identifier
				currentParam = New DBParam
				currentParam.Name = "Identifier"
				currentParam.Type = DBParamType.String
				If businessObject.IsIdentifierNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Identifier
				End If

				paramList.Add(currentParam)

				' ValidationLevel
				currentParam = New DBParam
				currentParam.Name = "ValidationLevel"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.ValidationLevel

				paramList.Add(currentParam)

				' ItemRequest_ID
				currentParam = New DBParam
				currentParam.Name = "ItemRequest_ID"
				currentParam.Type = DBParamType.Int
				If businessObject.IsItemRequestIDNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ItemRequestID
				End If

				paramList.Add(currentParam)


            ' Get the output value
			currentParam = New DBParam
			currentParam.Name = "UploadRow_ID"
			currentParam.Type = DBParamType.Int
			paramList.Add(currentParam)

			' get the current transaction if any
			Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
			
			' Execute the stored procedure
			If Not IsNothing(theTransaction) Then
				pkValueList = factory.ExecuteStoredProcedure(InsertUploadRowStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				pkValueList = factory.ExecuteStoredProcedure(InsertUploadRowStoredProcName, paramList)
			End If

			' set the returned pk value
			businessObject.UploadRowID = CInt(pkValueList(0))

		End Sub
		
		''' <summary>
		''' Set this in the UploadRowDAO sub class to change the stored
		''' procedure used by the GetAllUploadRows function.
		''' </summary>
		Public UpdateUploadRowStoredProcName As String = "EIM_Regen_UpdateUploadRow"

		Public Overridable Function UpdateUploadRow(ByVal businessObject As UploadRow) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' UploadRow_ID
				currentParam = New DBParam
				currentParam.Name = "UploadRow_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadRowID

				paramList.Add(currentParam)

				' Item_Key
				currentParam = New DBParam
				currentParam.Name = "Item_Key"
				currentParam.Type = DBParamType.Int
				If businessObject.IsItemKeyNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ItemKey
				End If

				paramList.Add(currentParam)

				' UploadSession_ID
				currentParam = New DBParam
				currentParam.Name = "UploadSession_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadSessionID

				paramList.Add(currentParam)

				' Identifier
				currentParam = New DBParam
				currentParam.Name = "Identifier"
				currentParam.Type = DBParamType.String
				If businessObject.IsIdentifierNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Identifier
				End If

				paramList.Add(currentParam)

				' ValidationLevel
				currentParam = New DBParam
				currentParam.Name = "ValidationLevel"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.ValidationLevel

				paramList.Add(currentParam)

				' ItemRequest_ID
				currentParam = New DBParam
				currentParam.Name = "ItemRequest_ID"
				currentParam.Type = DBParamType.Int
				If businessObject.IsItemRequestIDNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ItemRequestID
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
				outputList = factory.ExecuteStoredProcedure(UpdateUploadRowStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				outputList = factory.ExecuteStoredProcedure(UpdateUploadRowStoredProcName, paramList)
			End If

			Return CInt(outputList(0))

		End Function
		
		''' <summary>
		''' Set this in the UploadRowDAO sub class to change the stored
		''' procedure used by the GetAllUploadRows function.
		''' </summary>
		Public DeleteUploadRowStoredProcName As String = "EIM_Regen_DeleteUploadRow"

		Public Overridable Function DeleteUploadRow(ByVal businessObject As UploadRow) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' UploadRow_ID
				currentParam = New DBParam
				currentParam.Name = "UploadRow_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadRowID

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
				outputList = factory.ExecuteStoredProcedure(DeleteUploadRowStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				outputList = factory.ExecuteStoredProcedure(DeleteUploadRowStoredProcName, paramList)
			End If

			Return CInt(outputList(0))

		End Function

    End Class

End Namespace
