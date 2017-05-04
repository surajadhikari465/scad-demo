

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

	''' <summary>
	''' Generated Data Access Object base class for the UploadTypeTemplate db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	David Marine
	''' Created   :	Jul 29, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class UploadTypeTemplateDAORegen
    
		''' <summary>
		''' Returns a list of UploadTypeTemplates from the provided resultset.
		''' </summary>
		''' <returns>ArrayList of UploadTypeTemplates</returns>
		''' <remarks></remarks>
		Protected Overridable Function GetUploadTypeTemplatesFromResultSet (ByVal results As SqlDataReader) As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim businessObject As UploadTypeTemplate
			
			While results.Read

				businessObject = New UploadTypeTemplate()

				businessObject.UploadTypeTemplateID = results.GetInt32(results.GetOrdinal("UploadTypeTemplate_ID"))                         
				businessObject.UploadTypeCode = results.GetString(results.GetOrdinal("UploadType_Code"))                         
				businessObject.Name = results.GetString(results.GetOrdinal("Name"))                         
				businessObject.CreatedByUserID = results.GetInt32(results.GetOrdinal("CreatedByUserID"))                         
				businessObject.CreatedDateTime = results.GetDateTime(results.GetOrdinal("CreatedDateTime"))                         

				' ModifiedByUserID can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("ModifiedByUserID"))) Then
					' the businessObject.ModifiedByUserID property cannot be set to null
					' so set the businessObject.IsModifiedByUserIDNull flag to true.
					businessObject.IsModifiedByUserIDNull = True
				Else
					businessObject.ModifiedByUserID = results.GetInt32(results.GetOrdinal("ModifiedByUserID"))                         
				End If

				' ModifiedDateTime can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("ModifiedDateTime"))) Then
					' the businessObject.ModifiedDateTime property cannot be set to null
					' so set the businessObject.IsModifiedDateTimeNull flag to true.
					businessObject.IsModifiedDateTimeNull = True
				Else
					businessObject.ModifiedDateTime = results.GetDateTime(results.GetOrdinal("ModifiedDateTime"))                         
				End If
				
				' reset all the flags
				businessObject.IsNew = False
				businessObject.IsDirty = False
				businessObject.IsMarkedForDelete = False
				businessObject.IsDeleted = False

				' add business object to BusinessObjectCollection with abstracted PK key
				businessObjectTable.Add(businessObject.PrimaryKey, businessObject)
				
			End While

			Trace.WriteLine("Loading " + businessObjectTable.Count.ToString() + " UploadTypeTemplates from the database.")
			
			Return businessObjectTable

		End Function
		
		''' <summary>
		''' Set this in the UploadTypeTemplateDAO sub class to change the stored
		''' procedure used by the GetAllUploadTypeTemplates function.
		''' </summary>
		Public GetAllUploadTypeTemplatesStoredProcName As String = "EIM_Regen_GetAllUploadTypeTemplates"
		
		''' <summary>
		''' Returns all UploadTypeTemplates.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetAllUploadTypeTemplates () As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim results As SqlDataReader = Nothing

			Try
			
				' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetAllUploadTypeTemplatesStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetAllUploadTypeTemplatesStoredProcName, paramList)
				End If
				
				businessObjectTable = GetUploadTypeTemplatesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadTypeTemplateDAO sub class to change the stored
		''' procedure used by the GetUploadTypeTemplateByPK function.
		''' </summary>
		Protected GetUploadTypeTemplateByPKStoredProcName As String = "EIM_Regen_GetUploadTypeTemplateByPK"
		
		''' <summary>
		''' Returns zero or more UploadTypeTemplates by PK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadTypeTemplateByPK (ByRef uploadTypeTemplateID As System.Int32) As UploadTypeTemplate


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
					results = factory.GetStoredProcedureDataReader(Me.GetUploadTypeTemplateByPKStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadTypeTemplateByPKStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadTypeTemplatesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Dim businessObject As UploadTypeTemplate = Nothing
            If businessObjectTable.Count > 0 Then
                businessObject = CType(businessObjectTable.Item(0), UploadTypeTemplate)
            End If
			
			Return businessObject

		End Function


		''' <summary>
		''' Set this in the UploadTypeTemplateDAO sub class to change the stored
		''' procedure used by the GetUploadTypeTemplatesByUploadTypeCode function.
		''' </summary>
		Protected GetUploadTypeTemplatesByUploadTypeCodeStoredProcName As String = "EIM_Regen_GetUploadTypeTemplatesByUploadTypeCode"
		
		''' <summary>
		''' Returns UploadTypeTemplates by a uploadTypeCode FK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadTypeTemplatesByUploadTypeCode (ByRef uploadTypeCode As System.String) As BusinessObjectCollection


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
					results = factory.GetStoredProcedureDataReader(Me.GetUploadTypeTemplatesByUploadTypeCodeStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadTypeTemplatesByUploadTypeCodeStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadTypeTemplatesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadTypeTemplateDAO sub class to change the stored
		''' procedure used by the GetAllUploadTypeTemplates function.
		''' </summary>
		Public InsertUploadTypeTemplateStoredProcName As String = "EIM_Regen_InsertUploadTypeTemplate"

		Public Overridable Sub InsertUploadTypeTemplate(ByRef businessObject As UploadTypeTemplate)

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim pkValueList As New ArrayList
            
			' setup parameters for stored proc

				' UploadType_Code
				currentParam = New DBParam
				currentParam.Name = "UploadType_Code"
				currentParam.Type = DBParamType.String
				currentParam.Value = businessObject.UploadTypeCode

				paramList.Add(currentParam)

				' Name
				currentParam = New DBParam
				currentParam.Name = "Name"
				currentParam.Type = DBParamType.String
				currentParam.Value = businessObject.Name

				paramList.Add(currentParam)

				' CreatedByUserID
				currentParam = New DBParam
				currentParam.Name = "CreatedByUserID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.CreatedByUserID

				paramList.Add(currentParam)

				' CreatedDateTime
				currentParam = New DBParam
				currentParam.Name = "CreatedDateTime"
				currentParam.Type = DBParamType.DateTime
				currentParam.Value = businessObject.CreatedDateTime

				paramList.Add(currentParam)

				' ModifiedByUserID
				currentParam = New DBParam
				currentParam.Name = "ModifiedByUserID"
				currentParam.Type = DBParamType.Int
				If businessObject.IsModifiedByUserIDNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ModifiedByUserID
				End If

				paramList.Add(currentParam)

				' ModifiedDateTime
				currentParam = New DBParam
				currentParam.Name = "ModifiedDateTime"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsModifiedDateTimeNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ModifiedDateTime
				End If

				paramList.Add(currentParam)

			' Get the output value
			currentParam = New DBParam
			currentParam.Name = "UploadTypeTemplate_ID"
			currentParam.Type = DBParamType.Int
			paramList.Add(currentParam)

			' get the current transaction if any
			Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
			
			' Execute the stored procedure
			If Not IsNothing(theTransaction) Then
				pkValueList = factory.ExecuteStoredProcedure(InsertUploadTypeTemplateStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				pkValueList = factory.ExecuteStoredProcedure(InsertUploadTypeTemplateStoredProcName, paramList)
			End If

			' set the returned pk value
			businessObject.UploadTypeTemplateID = CInt(pkValueList(0))

		End Sub
		
		''' <summary>
		''' Set this in the UploadTypeTemplateDAO sub class to change the stored
		''' procedure used by the GetAllUploadTypeTemplates function.
		''' </summary>
		Public UpdateUploadTypeTemplateStoredProcName As String = "EIM_Regen_UpdateUploadTypeTemplate"

		Public Overridable Function UpdateUploadTypeTemplate(ByVal businessObject As UploadTypeTemplate) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' UploadTypeTemplate_ID
				currentParam = New DBParam
				currentParam.Name = "UploadTypeTemplate_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadTypeTemplateID

				paramList.Add(currentParam)

				' UploadType_Code
				currentParam = New DBParam
				currentParam.Name = "UploadType_Code"
				currentParam.Type = DBParamType.String
				currentParam.Value = businessObject.UploadTypeCode

				paramList.Add(currentParam)

				' Name
				currentParam = New DBParam
				currentParam.Name = "Name"
				currentParam.Type = DBParamType.String
				currentParam.Value = businessObject.Name

				paramList.Add(currentParam)

				' CreatedByUserID
				currentParam = New DBParam
				currentParam.Name = "CreatedByUserID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.CreatedByUserID

				paramList.Add(currentParam)

				' CreatedDateTime
				currentParam = New DBParam
				currentParam.Name = "CreatedDateTime"
				currentParam.Type = DBParamType.DateTime
				currentParam.Value = businessObject.CreatedDateTime

				paramList.Add(currentParam)

				' ModifiedByUserID
				currentParam = New DBParam
				currentParam.Name = "ModifiedByUserID"
				currentParam.Type = DBParamType.Int
				If businessObject.IsModifiedByUserIDNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ModifiedByUserID
				End If

				paramList.Add(currentParam)

				' ModifiedDateTime
				currentParam = New DBParam
				currentParam.Name = "ModifiedDateTime"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsModifiedDateTimeNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ModifiedDateTime
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
				outputList = factory.ExecuteStoredProcedure(UpdateUploadTypeTemplateStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				outputList = factory.ExecuteStoredProcedure(UpdateUploadTypeTemplateStoredProcName, paramList)
			End If

			Return CInt(outputList(0))

		End Function
		
		''' <summary>
		''' Set this in the UploadTypeTemplateDAO sub class to change the stored
		''' procedure used by the GetAllUploadTypeTemplates function.
		''' </summary>
		Public DeleteUploadTypeTemplateStoredProcName As String = "EIM_Regen_DeleteUploadTypeTemplate"

		Public Overridable Function DeleteUploadTypeTemplate(ByVal businessObject As UploadTypeTemplate) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' UploadTypeTemplate_ID
				currentParam = New DBParam
				currentParam.Name = "UploadTypeTemplate_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadTypeTemplateID

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
				outputList = factory.ExecuteStoredProcedure(DeleteUploadTypeTemplateStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				outputList = factory.ExecuteStoredProcedure(DeleteUploadTypeTemplateStoredProcName, paramList)
			End If

			Return CInt(outputList(0))

		End Function

    End Class

End Namespace
