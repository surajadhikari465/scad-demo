

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

	''' <summary>
	''' Generated Data Access Object base class for the UploadTypeAttribute db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	David Marine
	''' Created   :	Jul 29, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class UploadTypeAttributeDAORegen
    
		''' <summary>
		''' Returns a list of UploadTypeAttributes from the provided resultset.
		''' </summary>
		''' <returns>ArrayList of UploadTypeAttributes</returns>
		''' <remarks></remarks>
		Protected Overridable Function GetUploadTypeAttributesFromResultSet (ByVal results As SqlDataReader) As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim businessObject As UploadTypeAttribute
			
			While results.Read

				businessObject = New UploadTypeAttribute()

				businessObject.UploadTypeAttributeID = results.GetInt32(results.GetOrdinal("UploadTypeAttribute_ID"))                         

				' UploadType_Code can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("UploadType_Code"))) Then
					businessObject.UploadTypeCode = Nothing
				Else
					businessObject.UploadTypeCode = results.GetString(results.GetOrdinal("UploadType_Code"))                         
				End If

				' UploadAttribute_ID can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("UploadAttribute_ID"))) Then
					' the businessObject.UploadAttributeID property cannot be set to null
					' so set the businessObject.IsUploadAttributeIDNull flag to true.
					businessObject.IsUploadAttributeIDNull = True
				Else
					businessObject.UploadAttributeID = results.GetInt32(results.GetOrdinal("UploadAttribute_ID"))                         
				End If
				businessObject.IsRequiredForUploadTypeForExistingItems = results.GetBoolean(results.GetOrdinal("IsRequiredForUploadTypeForExistingItems"))                         
				businessObject.IsReadOnlyForExistingItems = results.GetBoolean(results.GetOrdinal("IsReadOnlyForExistingItems"))                         
				businessObject.IsHidden = results.GetBoolean(results.GetOrdinal("IsHidden"))                         
				businessObject.GridPosition = results.GetInt32(results.GetOrdinal("GridPosition"))                         

				' IsRequiredForUploadTypeForNewItems can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("IsRequiredForUploadTypeForNewItems"))) Then
					' the businessObject.IsRequiredForUploadTypeForNewItems property cannot be set to null
					' so set the businessObject.IsIsRequiredForUploadTypeForNewItemsNull flag to true.
					businessObject.IsIsRequiredForUploadTypeForNewItemsNull = True
				Else
					businessObject.IsRequiredForUploadTypeForNewItems = results.GetBoolean(results.GetOrdinal("IsRequiredForUploadTypeForNewItems"))                         
				End If

				' IsReadOnlyForNewItems can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("IsReadOnlyForNewItems"))) Then
					' the businessObject.IsReadOnlyForNewItems property cannot be set to null
					' so set the businessObject.IsIsReadOnlyForNewItemsNull flag to true.
					businessObject.IsIsReadOnlyForNewItemsNull = True
				Else
					businessObject.IsReadOnlyForNewItems = results.GetBoolean(results.GetOrdinal("IsReadOnlyForNewItems"))                         
				End If

				' GroupName can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("GroupName"))) Then
					businessObject.GroupName = Nothing
				Else
					businessObject.GroupName = results.GetString(results.GetOrdinal("GroupName"))                         
				End If
				
				' reset all the flags
				businessObject.IsNew = False
				businessObject.IsDirty = False
				businessObject.IsMarkedForDelete = False
				businessObject.IsDeleted = False

				' add business object to BusinessObjectCollection with abstracted PK key
				businessObjectTable.Add(businessObject.PrimaryKey, businessObject)
				
			End While

			Trace.WriteLine("Loading " + businessObjectTable.Count.ToString() + " UploadTypeAttributes from the database.")
			
			Return businessObjectTable

		End Function
		
		''' <summary>
		''' Set this in the UploadTypeAttributeDAO sub class to change the stored
		''' procedure used by the GetAllUploadTypeAttributes function.
		''' </summary>
		Public GetAllUploadTypeAttributesStoredProcName As String = "EIM_Regen_GetAllUploadTypeAttributes"
		
		''' <summary>
		''' Returns all UploadTypeAttributes.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetAllUploadTypeAttributes () As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim results As SqlDataReader = Nothing

			Try
			
				' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetAllUploadTypeAttributesStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetAllUploadTypeAttributesStoredProcName, paramList)
				End If
				
				businessObjectTable = GetUploadTypeAttributesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadTypeAttributeDAO sub class to change the stored
		''' procedure used by the GetUploadTypeAttributeByPK function.
		''' </summary>
		Protected GetUploadTypeAttributeByPKStoredProcName As String = "EIM_Regen_GetUploadTypeAttributeByPK"
		
		''' <summary>
		''' Returns zero or more UploadTypeAttributes by PK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadTypeAttributeByPK (ByRef uploadTypeAttributeID As System.Int32) As UploadTypeAttribute


			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim results As SqlDataReader = Nothing

			Try
				' setup identifier for stored proc
				currentParam = New DBParam
				currentParam.Name = "UploadTypeAttribute_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = uploadTypeAttributeID 
				paramList.Add(currentParam)

								' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetUploadTypeAttributeByPKStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadTypeAttributeByPKStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadTypeAttributesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Dim businessObject As UploadTypeAttribute = Nothing
            If businessObjectTable.Count > 0 Then
                businessObject = CType(businessObjectTable.Item(0), UploadTypeAttribute)
            End If
			
			Return businessObject

		End Function


		''' <summary>
		''' Set this in the UploadTypeAttributeDAO sub class to change the stored
		''' procedure used by the GetUploadTypeAttributesByUploadTypeCode function.
		''' </summary>
		Protected GetUploadTypeAttributesByUploadTypeCodeStoredProcName As String = "EIM_Regen_GetUploadTypeAttributesByUploadTypeCode"
		
		''' <summary>
		''' Returns UploadTypeAttributes by a uploadTypeCode FK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadTypeAttributesByUploadTypeCode (ByRef uploadTypeCode As System.String) As BusinessObjectCollection


			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim results As SqlDataReader = Nothing

			Try
				' setup identifier for stored proc
				currentParam = New DBParam
				currentParam.Name = "UploadType_Code"
				currentParam.Type = DBParamType.String
				currentParam.Value = uploadTypeCode 
				paramList.Add(currentParam)

								' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetUploadTypeAttributesByUploadTypeCodeStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadTypeAttributesByUploadTypeCodeStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadTypeAttributesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadTypeAttributeDAO sub class to change the stored
		''' procedure used by the GetUploadTypeAttributesByUploadAttributeID function.
		''' </summary>
		Protected GetUploadTypeAttributesByUploadAttributeIDStoredProcName As String = "EIM_Regen_GetUploadTypeAttributesByUploadAttributeID"
		
		''' <summary>
		''' Returns UploadTypeAttributes by a uploadAttributeID FK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadTypeAttributesByUploadAttributeID (ByRef uploadAttributeID As System.Int32) As BusinessObjectCollection


			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim results As SqlDataReader = Nothing

			Try
				' setup identifier for stored proc
				currentParam = New DBParam
				currentParam.Name = "UploadAttribute_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = uploadAttributeID 
				paramList.Add(currentParam)

								' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetUploadTypeAttributesByUploadAttributeIDStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadTypeAttributesByUploadAttributeIDStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadTypeAttributesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadTypeAttributeDAO sub class to change the stored
		''' procedure used by the GetAllUploadTypeAttributes function.
		''' </summary>
		Public InsertUploadTypeAttributeStoredProcName As String = "EIM_Regen_InsertUploadTypeAttribute"

		Public Overridable Sub InsertUploadTypeAttribute(ByRef businessObject As UploadTypeAttribute)

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim pkValueList As New ArrayList
            
			' setup parameters for stored proc

				' UploadType_Code
				currentParam = New DBParam
				currentParam.Name = "UploadType_Code"
				currentParam.Type = DBParamType.String
				If businessObject.IsUploadTypeCodeNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.UploadTypeCode
				End If

				paramList.Add(currentParam)

				' UploadAttribute_ID
				currentParam = New DBParam
				currentParam.Name = "UploadAttribute_ID"
				currentParam.Type = DBParamType.Int
				If businessObject.IsUploadAttributeIDNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.UploadAttributeID
				End If

				paramList.Add(currentParam)

				' IsRequiredForUploadTypeForExistingItems
				currentParam = New DBParam
				currentParam.Name = "IsRequiredForUploadTypeForExistingItems"
				currentParam.Type = DBParamType.Bit
				currentParam.Value = businessObject.IsRequiredForUploadTypeForExistingItems

				paramList.Add(currentParam)

				' IsReadOnlyForExistingItems
				currentParam = New DBParam
				currentParam.Name = "IsReadOnlyForExistingItems"
				currentParam.Type = DBParamType.Bit
				currentParam.Value = businessObject.IsReadOnlyForExistingItems

				paramList.Add(currentParam)

				' IsHidden
				currentParam = New DBParam
				currentParam.Name = "IsHidden"
				currentParam.Type = DBParamType.Bit
				currentParam.Value = businessObject.IsHidden

				paramList.Add(currentParam)

				' GridPosition
				currentParam = New DBParam
				currentParam.Name = "GridPosition"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.GridPosition

				paramList.Add(currentParam)

				' IsRequiredForUploadTypeForNewItems
				currentParam = New DBParam
				currentParam.Name = "IsRequiredForUploadTypeForNewItems"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsIsRequiredForUploadTypeForNewItemsNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.IsRequiredForUploadTypeForNewItems
				End If

				paramList.Add(currentParam)

				' IsReadOnlyForNewItems
				currentParam = New DBParam
				currentParam.Name = "IsReadOnlyForNewItems"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsIsReadOnlyForNewItemsNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.IsReadOnlyForNewItems
				End If

				paramList.Add(currentParam)

				' GroupName
				currentParam = New DBParam
				currentParam.Name = "GroupName"
				currentParam.Type = DBParamType.String
				If businessObject.IsGroupNameNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.GroupName
				End If

				paramList.Add(currentParam)

			' Get the output value
			currentParam = New DBParam
			currentParam.Name = "UploadTypeAttribute_ID"
			currentParam.Type = DBParamType.Int
			paramList.Add(currentParam)

			' get the current transaction if any
			Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
			
			' Execute the stored procedure
			If Not IsNothing(theTransaction) Then
				pkValueList = factory.ExecuteStoredProcedure(InsertUploadTypeAttributeStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				pkValueList = factory.ExecuteStoredProcedure(InsertUploadTypeAttributeStoredProcName, paramList)
			End If

			' set the returned pk value
			businessObject.UploadTypeAttributeID = CInt(pkValueList(0))

		End Sub
		
		''' <summary>
		''' Set this in the UploadTypeAttributeDAO sub class to change the stored
		''' procedure used by the GetAllUploadTypeAttributes function.
		''' </summary>
		Public UpdateUploadTypeAttributeStoredProcName As String = "EIM_Regen_UpdateUploadTypeAttribute"

		Public Overridable Function UpdateUploadTypeAttribute(ByVal businessObject As UploadTypeAttribute) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' UploadTypeAttribute_ID
				currentParam = New DBParam
				currentParam.Name = "UploadTypeAttribute_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadTypeAttributeID

				paramList.Add(currentParam)

				' UploadType_Code
				currentParam = New DBParam
				currentParam.Name = "UploadType_Code"
				currentParam.Type = DBParamType.String
				If businessObject.IsUploadTypeCodeNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.UploadTypeCode
				End If

				paramList.Add(currentParam)

				' UploadAttribute_ID
				currentParam = New DBParam
				currentParam.Name = "UploadAttribute_ID"
				currentParam.Type = DBParamType.Int
				If businessObject.IsUploadAttributeIDNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.UploadAttributeID
				End If

				paramList.Add(currentParam)

				' IsRequiredForUploadTypeForExistingItems
				currentParam = New DBParam
				currentParam.Name = "IsRequiredForUploadTypeForExistingItems"
				currentParam.Type = DBParamType.Bit
				currentParam.Value = businessObject.IsRequiredForUploadTypeForExistingItems

				paramList.Add(currentParam)

				' IsReadOnlyForExistingItems
				currentParam = New DBParam
				currentParam.Name = "IsReadOnlyForExistingItems"
				currentParam.Type = DBParamType.Bit
				currentParam.Value = businessObject.IsReadOnlyForExistingItems

				paramList.Add(currentParam)

				' IsHidden
				currentParam = New DBParam
				currentParam.Name = "IsHidden"
				currentParam.Type = DBParamType.Bit
				currentParam.Value = businessObject.IsHidden

				paramList.Add(currentParam)

				' GridPosition
				currentParam = New DBParam
				currentParam.Name = "GridPosition"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.GridPosition

				paramList.Add(currentParam)

				' IsRequiredForUploadTypeForNewItems
				currentParam = New DBParam
				currentParam.Name = "IsRequiredForUploadTypeForNewItems"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsIsRequiredForUploadTypeForNewItemsNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.IsRequiredForUploadTypeForNewItems
				End If

				paramList.Add(currentParam)

				' IsReadOnlyForNewItems
				currentParam = New DBParam
				currentParam.Name = "IsReadOnlyForNewItems"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsIsReadOnlyForNewItemsNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.IsReadOnlyForNewItems
				End If

				paramList.Add(currentParam)

				' GroupName
				currentParam = New DBParam
				currentParam.Name = "GroupName"
				currentParam.Type = DBParamType.String
				If businessObject.IsGroupNameNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.GroupName
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
				outputList = factory.ExecuteStoredProcedure(UpdateUploadTypeAttributeStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				outputList = factory.ExecuteStoredProcedure(UpdateUploadTypeAttributeStoredProcName, paramList)
			End If

			Return CInt(outputList(0))

		End Function
		
		''' <summary>
		''' Set this in the UploadTypeAttributeDAO sub class to change the stored
		''' procedure used by the GetAllUploadTypeAttributes function.
		''' </summary>
		Public DeleteUploadTypeAttributeStoredProcName As String = "EIM_Regen_DeleteUploadTypeAttribute"

		Public Overridable Function DeleteUploadTypeAttribute(ByVal businessObject As UploadTypeAttribute) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' UploadTypeAttribute_ID
				currentParam = New DBParam
				currentParam.Name = "UploadTypeAttribute_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadTypeAttributeID

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
				outputList = factory.ExecuteStoredProcedure(DeleteUploadTypeAttributeStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				outputList = factory.ExecuteStoredProcedure(DeleteUploadTypeAttributeStoredProcName, paramList)
			End If

			Return CInt(outputList(0))

		End Function

    End Class

End Namespace
