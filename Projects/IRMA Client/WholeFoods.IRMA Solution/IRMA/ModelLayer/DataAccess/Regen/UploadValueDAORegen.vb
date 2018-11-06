

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

	''' <summary>
	''' Generated Data Access Object base class for the UploadValue db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	David Marine
	''' Created   :	Jul 29, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class UploadValueDAORegen
    
		''' <summary>
		''' Returns a list of UploadValues from the provided resultset.
		''' </summary>
		''' <returns>ArrayList of UploadValues</returns>
		''' <remarks></remarks>
		Protected Overridable Function GetUploadValuesFromResultSet (ByVal results As SqlDataReader) As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim businessObject As UploadValue
			
			While results.Read

				businessObject = New UploadValue()

				businessObject.UploadValueID = results.GetInt32(results.GetOrdinal("UploadValue_ID"))                         
				businessObject.UploadAttributeID = results.GetInt32(results.GetOrdinal("UploadAttribute_ID"))                         
				businessObject.UploadRowID = results.GetInt32(results.GetOrdinal("UploadRow_ID"))                         

				' Value can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Value"))) Then
					businessObject.Value = Nothing
				Else
					businessObject.Value = results.GetString(results.GetOrdinal("Value"))                         
				End If
				
				' reset all the flags
				businessObject.IsNew = False
				businessObject.IsDirty = False
				businessObject.IsMarkedForDelete = False
				businessObject.IsDeleted = False

				' add business object to BusinessObjectCollection with abstracted PK key
				businessObjectTable.Add(businessObject.PrimaryKey, businessObject)
				
			End While

			Trace.WriteLine("Loading " + businessObjectTable.Count.ToString() + " UploadValues from the database.")
			
			Return businessObjectTable

		End Function
		
		''' <summary>
		''' Set this in the UploadValueDAO sub class to change the stored
		''' procedure used by the GetAllUploadValues function.
		''' </summary>
		Public GetAllUploadValuesStoredProcName As String = "EIM_Regen_GetAllUploadValues"
		
		''' <summary>
		''' Returns all UploadValues.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetAllUploadValues () As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim results As SqlDataReader = Nothing

			Try
			
				' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetAllUploadValuesStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetAllUploadValuesStoredProcName, paramList)
				End If
				
				businessObjectTable = GetUploadValuesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadValueDAO sub class to change the stored
		''' procedure used by the GetUploadValueByPK function.
		''' </summary>
		Protected GetUploadValueByPKStoredProcName As String = "EIM_Regen_GetUploadValueByPK"
		
		''' <summary>
		''' Returns zero or more UploadValues by PK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadValueByPK (ByRef uploadValueID As System.Int32) As UploadValue


			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim results As SqlDataReader = Nothing

			Try
				' setup identifier for stored proc
				currentParam = New DBParam
				currentParam.Name = "UploadValue_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = uploadValueID 
				paramList.Add(currentParam)

								' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetUploadValueByPKStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadValueByPKStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadValuesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Dim businessObject As UploadValue = Nothing
            If businessObjectTable.Count > 0 Then
                businessObject = CType(businessObjectTable.Item(0), UploadValue)
            End If
			
			Return businessObject

		End Function


		''' <summary>
		''' Set this in the UploadValueDAO sub class to change the stored
		''' procedure used by the GetUploadValuesByUploadAttributeID function.
		''' </summary>
		Protected GetUploadValuesByUploadAttributeIDStoredProcName As String = "EIM_Regen_GetUploadValuesByUploadAttributeID"
		
		''' <summary>
		''' Returns UploadValues by a uploadAttributeID FK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadValuesByUploadAttributeID (ByRef uploadAttributeID As System.Int32) As BusinessObjectCollection


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
					results = factory.GetStoredProcedureDataReader(Me.GetUploadValuesByUploadAttributeIDStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadValuesByUploadAttributeIDStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadValuesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadValueDAO sub class to change the stored
		''' procedure used by the GetUploadValuesByUploadRowID function.
		''' </summary>
		Protected GetUploadValuesByUploadRowIDStoredProcName As String = "EIM_Regen_GetUploadValuesByUploadRowID"
		
		''' <summary>
		''' Returns UploadValues by a uploadRowID FK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadValuesByUploadRowID (ByRef uploadRowID As System.Int32) As BusinessObjectCollection


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
					results = factory.GetStoredProcedureDataReader(Me.GetUploadValuesByUploadRowIDStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadValuesByUploadRowIDStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadValuesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadValueDAO sub class to change the stored
		''' procedure used by the GetAllUploadValues function.
		''' </summary>
		Public InsertUploadValueStoredProcName As String = "EIM_Regen_InsertUploadValue"

		Public Overridable Sub InsertUploadValue(ByRef businessObject As UploadValue)

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim pkValueList As New ArrayList
            
			' setup parameters for stored proc

				' UploadAttribute_ID
				currentParam = New DBParam
				currentParam.Name = "UploadAttribute_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadAttributeID

				paramList.Add(currentParam)

				' UploadRow_ID
				currentParam = New DBParam
				currentParam.Name = "UploadRow_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadRowID

				paramList.Add(currentParam)

				' Value
				currentParam = New DBParam
				currentParam.Name = "Value"
				currentParam.Type = DBParamType.String
				If businessObject.IsValueNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Value
				End If

				paramList.Add(currentParam)

			' Get the output value
			currentParam = New DBParam
			currentParam.Name = "UploadValue_ID"
			currentParam.Type = DBParamType.Int
			paramList.Add(currentParam)

			' get the current transaction if any
			Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
			
			' Execute the stored procedure
			If Not IsNothing(theTransaction) Then
				pkValueList = factory.ExecuteStoredProcedure(InsertUploadValueStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				pkValueList = factory.ExecuteStoredProcedure(InsertUploadValueStoredProcName, paramList)
			End If

			' set the returned pk value
			businessObject.UploadValueID = CInt(pkValueList(0))

		End Sub
		
		''' <summary>
		''' Set this in the UploadValueDAO sub class to change the stored
		''' procedure used by the GetAllUploadValues function.
		''' </summary>
		Public UpdateUploadValueStoredProcName As String = "EIM_Regen_UpdateUploadValue"

		Public Overridable Function UpdateUploadValue(ByVal businessObject As UploadValue) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' UploadValue_ID
				currentParam = New DBParam
				currentParam.Name = "UploadValue_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadValueID

				paramList.Add(currentParam)

				' UploadAttribute_ID
				currentParam = New DBParam
				currentParam.Name = "UploadAttribute_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadAttributeID

				paramList.Add(currentParam)

				' UploadRow_ID
				currentParam = New DBParam
				currentParam.Name = "UploadRow_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadRowID

				paramList.Add(currentParam)

				' Value
				currentParam = New DBParam
				currentParam.Name = "Value"
				currentParam.Type = DBParamType.String
				If businessObject.IsValueNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Value
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
				outputList = factory.ExecuteStoredProcedure(UpdateUploadValueStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				outputList = factory.ExecuteStoredProcedure(UpdateUploadValueStoredProcName, paramList)
			End If

			Return CInt(outputList(0))

		End Function
		
		''' <summary>
		''' Set this in the UploadValueDAO sub class to change the stored
		''' procedure used by the GetAllUploadValues function.
		''' </summary>
		Public DeleteUploadValueStoredProcName As String = "EIM_Regen_DeleteUploadValue"

		Public Overridable Function DeleteUploadValue(ByVal businessObject As UploadValue) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' UploadValue_ID
				currentParam = New DBParam
				currentParam.Name = "UploadValue_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadValueID

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
				outputList = factory.ExecuteStoredProcedure(DeleteUploadValueStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				outputList = factory.ExecuteStoredProcedure(DeleteUploadValueStoredProcName, paramList)
			End If

			Return CInt(outputList(0))

		End Function

    End Class

End Namespace
