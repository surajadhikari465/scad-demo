

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

	''' <summary>
	''' Generated Data Access Object base class for the UploadSessionUploadType db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	David Marine
	''' Created   :	Jul 29, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class UploadSessionUploadTypeDAORegen
    
		''' <summary>
		''' Returns a list of UploadSessionUploadTypes from the provided resultset.
		''' </summary>
		''' <returns>ArrayList of UploadSessionUploadTypes</returns>
		''' <remarks></remarks>
		Protected Overridable Function GetUploadSessionUploadTypesFromResultSet (ByVal results As SqlDataReader) As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim businessObject As UploadSessionUploadType
			
			While results.Read

				businessObject = New UploadSessionUploadType()

				businessObject.UploadSessionUploadTypeID = results.GetInt32(results.GetOrdinal("UploadSessionUploadType_ID"))                         
				businessObject.UploadSessionID = results.GetInt32(results.GetOrdinal("UploadSession_ID"))                         
				businessObject.UploadTypeCode = results.GetString(results.GetOrdinal("UploadType_Code"))                         

				' UploadTypeTemplate_ID can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("UploadTypeTemplate_ID"))) Then
					' the businessObject.UploadTypeTemplateID property cannot be set to null
					' so set the businessObject.IsUploadTypeTemplateIDNull flag to true.
					businessObject.IsUploadTypeTemplateIDNull = True
				Else
					businessObject.UploadTypeTemplateID = results.GetInt32(results.GetOrdinal("UploadTypeTemplate_ID"))                         
				End If

				' StoreSelectionType can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("StoreSelectionType"))) Then
					businessObject.StoreSelectionType = Nothing
				Else
					businessObject.StoreSelectionType = results.GetString(results.GetOrdinal("StoreSelectionType"))                         
				End If

				' Zone_ID can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Zone_ID"))) Then
					' the businessObject.ZoneID property cannot be set to null
					' so set the businessObject.IsZoneIDNull flag to true.
					businessObject.IsZoneIDNull = True
				Else
					businessObject.ZoneID = results.GetInt32(results.GetOrdinal("Zone_ID"))                         
				End If

				' State can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("State"))) Then
					businessObject.State = Nothing
				Else
					businessObject.State = results.GetString(results.GetOrdinal("State"))                         
				End If
				
				' reset all the flags
				businessObject.IsNew = False
				businessObject.IsDirty = False
				businessObject.IsMarkedForDelete = False
				businessObject.IsDeleted = False

				' add business object to BusinessObjectCollection with abstracted PK key
				businessObjectTable.Add(businessObject.PrimaryKey, businessObject)
				
			End While

			Trace.WriteLine("Loading " + businessObjectTable.Count.ToString() + " UploadSessionUploadTypes from the database.")
			
			Return businessObjectTable

		End Function
		
		''' <summary>
		''' Set this in the UploadSessionUploadTypeDAO sub class to change the stored
		''' procedure used by the GetAllUploadSessionUploadTypes function.
		''' </summary>
		Public GetAllUploadSessionUploadTypesStoredProcName As String = "EIM_Regen_GetAllUploadSessionUploadTypes"
		
		''' <summary>
		''' Returns all UploadSessionUploadTypes.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetAllUploadSessionUploadTypes () As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim results As SqlDataReader = Nothing

			Try
			
				' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetAllUploadSessionUploadTypesStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetAllUploadSessionUploadTypesStoredProcName, paramList)
				End If
				
				businessObjectTable = GetUploadSessionUploadTypesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadSessionUploadTypeDAO sub class to change the stored
		''' procedure used by the GetUploadSessionUploadTypeByPK function.
		''' </summary>
		Protected GetUploadSessionUploadTypeByPKStoredProcName As String = "EIM_Regen_GetUploadSessionUploadTypeByPK"
		
		''' <summary>
		''' Returns zero or more UploadSessionUploadTypes by PK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadSessionUploadTypeByPK (ByRef uploadSessionUploadTypeID As System.Int32) As UploadSessionUploadType


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
					results = factory.GetStoredProcedureDataReader(Me.GetUploadSessionUploadTypeByPKStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadSessionUploadTypeByPKStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadSessionUploadTypesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Dim businessObject As UploadSessionUploadType = Nothing
            If businessObjectTable.Count > 0 Then
                businessObject = CType(businessObjectTable.Item(0), UploadSessionUploadType)
            End If
			
			Return businessObject

		End Function


		''' <summary>
		''' Set this in the UploadSessionUploadTypeDAO sub class to change the stored
		''' procedure used by the GetUploadSessionUploadTypesByUploadSessionID function.
		''' </summary>
		Protected GetUploadSessionUploadTypesByUploadSessionIDStoredProcName As String = "EIM_Regen_GetUploadSessionUploadTypesByUploadSessionID"
		
		''' <summary>
		''' Returns UploadSessionUploadTypes by a uploadSessionID FK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadSessionUploadTypesByUploadSessionID (ByRef uploadSessionID As System.Int32) As BusinessObjectCollection


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
					results = factory.GetStoredProcedureDataReader(Me.GetUploadSessionUploadTypesByUploadSessionIDStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadSessionUploadTypesByUploadSessionIDStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadSessionUploadTypesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadSessionUploadTypeDAO sub class to change the stored
		''' procedure used by the GetUploadSessionUploadTypesByUploadTypeCode function.
		''' </summary>
		Protected GetUploadSessionUploadTypesByUploadTypeCodeStoredProcName As String = "EIM_Regen_GetUploadSessionUploadTypesByUploadTypeCode"
		
		''' <summary>
		''' Returns UploadSessionUploadTypes by a uploadTypeCode FK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadSessionUploadTypesByUploadTypeCode (ByRef uploadTypeCode As System.String) As BusinessObjectCollection


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
					results = factory.GetStoredProcedureDataReader(Me.GetUploadSessionUploadTypesByUploadTypeCodeStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadSessionUploadTypesByUploadTypeCodeStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadSessionUploadTypesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadSessionUploadTypeDAO sub class to change the stored
		''' procedure used by the GetUploadSessionUploadTypesByUploadTypeTemplateID function.
		''' </summary>
		Protected GetUploadSessionUploadTypesByUploadTypeTemplateIDStoredProcName As String = "EIM_Regen_GetUploadSessionUploadTypesByUploadTypeTemplateID"
		
		''' <summary>
		''' Returns UploadSessionUploadTypes by a uploadTypeTemplateID FK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadSessionUploadTypesByUploadTypeTemplateID (ByRef uploadTypeTemplateID As System.Int32) As BusinessObjectCollection


			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim results As SqlDataReader = Nothing

			Try
				' setup identifier for stored proc
				currentParam = New DBParam
				currentParam.Name = "UploadTypeTemplate_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = uploadTypeTemplateID 
				paramList.Add(currentParam)

								' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetUploadSessionUploadTypesByUploadTypeTemplateIDStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadSessionUploadTypesByUploadTypeTemplateIDStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadSessionUploadTypesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadSessionUploadTypeDAO sub class to change the stored
		''' procedure used by the GetUploadSessionUploadTypesByZoneID function.
		''' </summary>
		Protected GetUploadSessionUploadTypesByZoneIDStoredProcName As String = "EIM_Regen_GetUploadSessionUploadTypesByZoneID"
		
		''' <summary>
		''' Returns UploadSessionUploadTypes by a zoneID FK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadSessionUploadTypesByZoneID (ByRef zoneID As System.Int32) As BusinessObjectCollection


			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim results As SqlDataReader = Nothing

			Try
				' setup identifier for stored proc
				currentParam = New DBParam
				currentParam.Name = "Zone_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = zoneID 
				paramList.Add(currentParam)

								' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetUploadSessionUploadTypesByZoneIDStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadSessionUploadTypesByZoneIDStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadSessionUploadTypesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadSessionUploadTypeDAO sub class to change the stored
		''' procedure used by the GetAllUploadSessionUploadTypes function.
		''' </summary>
		Public InsertUploadSessionUploadTypeStoredProcName As String = "EIM_Regen_InsertUploadSessionUploadType"

		Public Overridable Sub InsertUploadSessionUploadType(ByRef businessObject As UploadSessionUploadType)

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim pkValueList As New ArrayList
            
			' setup parameters for stored proc

				' UploadSession_ID
				currentParam = New DBParam
				currentParam.Name = "UploadSession_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadSessionID

				paramList.Add(currentParam)

				' UploadType_Code
				currentParam = New DBParam
				currentParam.Name = "UploadType_Code"
				currentParam.Type = DBParamType.String
				currentParam.Value = businessObject.UploadTypeCode

				paramList.Add(currentParam)

				' UploadTypeTemplate_ID
				currentParam = New DBParam
				currentParam.Name = "UploadTypeTemplate_ID"
				currentParam.Type = DBParamType.Int
				If businessObject.IsUploadTypeTemplateIDNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.UploadTypeTemplateID
				End If

				paramList.Add(currentParam)

				' StoreSelectionType
				currentParam = New DBParam
				currentParam.Name = "StoreSelectionType"
				currentParam.Type = DBParamType.String
				If businessObject.IsStoreSelectionTypeNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.StoreSelectionType
				End If

				paramList.Add(currentParam)

				' Zone_ID
				currentParam = New DBParam
				currentParam.Name = "Zone_ID"
				currentParam.Type = DBParamType.Int
				If businessObject.IsZoneIDNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ZoneID
				End If

				paramList.Add(currentParam)

				' State
				currentParam = New DBParam
				currentParam.Name = "State"
				currentParam.Type = DBParamType.String
				If businessObject.IsStateNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.State
				End If

				paramList.Add(currentParam)

			' Get the output value
			currentParam = New DBParam
			currentParam.Name = "UploadSessionUploadType_ID"
			currentParam.Type = DBParamType.Int
			paramList.Add(currentParam)

			' get the current transaction if any
			Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
			
			' Execute the stored procedure
			If Not IsNothing(theTransaction) Then
				pkValueList = factory.ExecuteStoredProcedure(InsertUploadSessionUploadTypeStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				pkValueList = factory.ExecuteStoredProcedure(InsertUploadSessionUploadTypeStoredProcName, paramList)
			End If

			' set the returned pk value
			businessObject.UploadSessionUploadTypeID = CInt(pkValueList(0))

		End Sub
		
		''' <summary>
		''' Set this in the UploadSessionUploadTypeDAO sub class to change the stored
		''' procedure used by the GetAllUploadSessionUploadTypes function.
		''' </summary>
		Public UpdateUploadSessionUploadTypeStoredProcName As String = "EIM_Regen_UpdateUploadSessionUploadType"

		Public Overridable Function UpdateUploadSessionUploadType(ByVal businessObject As UploadSessionUploadType) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' UploadSessionUploadType_ID
				currentParam = New DBParam
				currentParam.Name = "UploadSessionUploadType_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadSessionUploadTypeID

				paramList.Add(currentParam)

				' UploadSession_ID
				currentParam = New DBParam
				currentParam.Name = "UploadSession_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadSessionID

				paramList.Add(currentParam)

				' UploadType_Code
				currentParam = New DBParam
				currentParam.Name = "UploadType_Code"
				currentParam.Type = DBParamType.String
				currentParam.Value = businessObject.UploadTypeCode

				paramList.Add(currentParam)

				' UploadTypeTemplate_ID
				currentParam = New DBParam
				currentParam.Name = "UploadTypeTemplate_ID"
				currentParam.Type = DBParamType.Int
				If businessObject.IsUploadTypeTemplateIDNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.UploadTypeTemplateID
				End If

				paramList.Add(currentParam)

				' StoreSelectionType
				currentParam = New DBParam
				currentParam.Name = "StoreSelectionType"
				currentParam.Type = DBParamType.String
				If businessObject.IsStoreSelectionTypeNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.StoreSelectionType
				End If

				paramList.Add(currentParam)

				' Zone_ID
				currentParam = New DBParam
				currentParam.Name = "Zone_ID"
				currentParam.Type = DBParamType.Int
				If businessObject.IsZoneIDNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ZoneID
				End If

				paramList.Add(currentParam)

				' State
				currentParam = New DBParam
				currentParam.Name = "State"
				currentParam.Type = DBParamType.String
				If businessObject.IsStateNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.State
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
				outputList = factory.ExecuteStoredProcedure(UpdateUploadSessionUploadTypeStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				outputList = factory.ExecuteStoredProcedure(UpdateUploadSessionUploadTypeStoredProcName, paramList)
			End If

			Return CInt(outputList(0))

		End Function
		
		''' <summary>
		''' Set this in the UploadSessionUploadTypeDAO sub class to change the stored
		''' procedure used by the GetAllUploadSessionUploadTypes function.
		''' </summary>
		Public DeleteUploadSessionUploadTypeStoredProcName As String = "EIM_Regen_DeleteUploadSessionUploadType"

		Public Overridable Function DeleteUploadSessionUploadType(ByVal businessObject As UploadSessionUploadType) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' UploadSessionUploadType_ID
				currentParam = New DBParam
				currentParam.Name = "UploadSessionUploadType_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadSessionUploadTypeID

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
				outputList = factory.ExecuteStoredProcedure(DeleteUploadSessionUploadTypeStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				outputList = factory.ExecuteStoredProcedure(DeleteUploadSessionUploadTypeStoredProcName, paramList)
			End If

			Return CInt(outputList(0))

		End Function

    End Class

End Namespace
