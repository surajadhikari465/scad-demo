

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

	''' <summary>
	''' Generated Data Access Object base class for the UploadTypeTemplateAttribute db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	David Marine
	''' Created   :	Jul 29, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class UploadTypeTemplateAttributeDAORegen
    
		''' <summary>
		''' Returns a list of UploadTypeTemplateAttributes from the provided resultset.
		''' </summary>
		''' <returns>ArrayList of UploadTypeTemplateAttributes</returns>
		''' <remarks></remarks>
		Protected Overridable Function GetUploadTypeTemplateAttributesFromResultSet (ByVal results As SqlDataReader) As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim businessObject As UploadTypeTemplateAttribute
			
			While results.Read

				businessObject = New UploadTypeTemplateAttribute()

				businessObject.UploadTypeTemplateAttributeID = results.GetInt32(results.GetOrdinal("UploadTypeTemplateAttribute_ID"))                         
				businessObject.UploadTypeTemplateID = results.GetInt32(results.GetOrdinal("UploadTypeTemplate_ID"))                         
				businessObject.UploadTypeAttributeID = results.GetInt32(results.GetOrdinal("UploadTypeAttribute_ID"))                         
				
				' reset all the flags
				businessObject.IsNew = False
				businessObject.IsDirty = False
				businessObject.IsMarkedForDelete = False
				businessObject.IsDeleted = False

				' add business object to BusinessObjectCollection with abstracted PK key
				businessObjectTable.Add(businessObject.PrimaryKey, businessObject)
				
			End While

			Trace.WriteLine("Loading " + businessObjectTable.Count.ToString() + " UploadTypeTemplateAttributes from the database.")
			
			Return businessObjectTable

		End Function
		
		''' <summary>
		''' Set this in the UploadTypeTemplateAttributeDAO sub class to change the stored
		''' procedure used by the GetAllUploadTypeTemplateAttributes function.
		''' </summary>
		Public GetAllUploadTypeTemplateAttributesStoredProcName As String = "EIM_Regen_GetAllUploadTypeTemplateAttributes"
		
		''' <summary>
		''' Returns all UploadTypeTemplateAttributes.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetAllUploadTypeTemplateAttributes () As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim results As SqlDataReader = Nothing

			Try
			
				' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetAllUploadTypeTemplateAttributesStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetAllUploadTypeTemplateAttributesStoredProcName, paramList)
				End If
				
				businessObjectTable = GetUploadTypeTemplateAttributesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadTypeTemplateAttributeDAO sub class to change the stored
		''' procedure used by the GetUploadTypeTemplateAttributeByPK function.
		''' </summary>
		Protected GetUploadTypeTemplateAttributeByPKStoredProcName As String = "EIM_Regen_GetUploadTypeTemplateAttributeByPK"
		
		''' <summary>
		''' Returns zero or more UploadTypeTemplateAttributes by PK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadTypeTemplateAttributeByPK (ByRef uploadTypeTemplateAttributeID As System.Int32) As UploadTypeTemplateAttribute


			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim results As SqlDataReader = Nothing

			Try
				' setup identifier for stored proc
				currentParam = New DBParam
				currentParam.Name = "UploadTypeTemplateAttribute_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = uploadTypeTemplateAttributeID 
				paramList.Add(currentParam)

								' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetUploadTypeTemplateAttributeByPKStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadTypeTemplateAttributeByPKStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadTypeTemplateAttributesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Dim businessObject As UploadTypeTemplateAttribute = Nothing
            If businessObjectTable.Count > 0 Then
                businessObject = CType(businessObjectTable.Item(0), UploadTypeTemplateAttribute)
            End If
			
			Return businessObject

		End Function


		''' <summary>
		''' Set this in the UploadTypeTemplateAttributeDAO sub class to change the stored
		''' procedure used by the GetUploadTypeTemplateAttributesByUploadTypeTemplateID function.
		''' </summary>
		Protected GetUploadTypeTemplateAttributesByUploadTypeTemplateIDStoredProcName As String = "EIM_Regen_GetUploadTypeTemplateAttributesByUploadTypeTemplateID"
		
		''' <summary>
		''' Returns UploadTypeTemplateAttributes by a uploadTypeTemplateID FK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadTypeTemplateAttributesByUploadTypeTemplateID (ByRef uploadTypeTemplateID As System.Int32) As BusinessObjectCollection


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
					results = factory.GetStoredProcedureDataReader(Me.GetUploadTypeTemplateAttributesByUploadTypeTemplateIDStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadTypeTemplateAttributesByUploadTypeTemplateIDStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadTypeTemplateAttributesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadTypeTemplateAttributeDAO sub class to change the stored
		''' procedure used by the GetUploadTypeTemplateAttributesByUploadTypeAttributeID function.
		''' </summary>
		Protected GetUploadTypeTemplateAttributesByUploadTypeAttributeIDStoredProcName As String = "EIM_Regen_GetUploadTypeTemplateAttributesByUploadTypeAttributeID"
		
		''' <summary>
		''' Returns UploadTypeTemplateAttributes by a uploadTypeAttributeID FK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadTypeTemplateAttributesByUploadTypeAttributeID (ByRef uploadTypeAttributeID As System.Int32) As BusinessObjectCollection


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
					results = factory.GetStoredProcedureDataReader(Me.GetUploadTypeTemplateAttributesByUploadTypeAttributeIDStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadTypeTemplateAttributesByUploadTypeAttributeIDStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadTypeTemplateAttributesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadTypeTemplateAttributeDAO sub class to change the stored
		''' procedure used by the GetAllUploadTypeTemplateAttributes function.
		''' </summary>
		Public InsertUploadTypeTemplateAttributeStoredProcName As String = "EIM_Regen_InsertUploadTypeTemplateAttribute"

		Public Overridable Sub InsertUploadTypeTemplateAttribute(ByRef businessObject As UploadTypeTemplateAttribute)

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim pkValueList As New ArrayList
            
			' setup parameters for stored proc

				' UploadTypeTemplate_ID
				currentParam = New DBParam
				currentParam.Name = "UploadTypeTemplate_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadTypeTemplateID

				paramList.Add(currentParam)

				' UploadTypeAttribute_ID
				currentParam = New DBParam
				currentParam.Name = "UploadTypeAttribute_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadTypeAttributeID

				paramList.Add(currentParam)

			' Get the output value
			currentParam = New DBParam
			currentParam.Name = "UploadTypeTemplateAttribute_ID"
			currentParam.Type = DBParamType.Int
			paramList.Add(currentParam)

			' get the current transaction if any
			Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
			
			' Execute the stored procedure
			If Not IsNothing(theTransaction) Then
				pkValueList = factory.ExecuteStoredProcedure(InsertUploadTypeTemplateAttributeStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				pkValueList = factory.ExecuteStoredProcedure(InsertUploadTypeTemplateAttributeStoredProcName, paramList)
			End If

			' set the returned pk value
			businessObject.UploadTypeTemplateAttributeID = CInt(pkValueList(0))

		End Sub
		
		''' <summary>
		''' Set this in the UploadTypeTemplateAttributeDAO sub class to change the stored
		''' procedure used by the GetAllUploadTypeTemplateAttributes function.
		''' </summary>
		Public UpdateUploadTypeTemplateAttributeStoredProcName As String = "EIM_Regen_UpdateUploadTypeTemplateAttribute"

		Public Overridable Function UpdateUploadTypeTemplateAttribute(ByVal businessObject As UploadTypeTemplateAttribute) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' UploadTypeTemplateAttribute_ID
				currentParam = New DBParam
				currentParam.Name = "UploadTypeTemplateAttribute_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadTypeTemplateAttributeID

				paramList.Add(currentParam)

				' UploadTypeTemplate_ID
				currentParam = New DBParam
				currentParam.Name = "UploadTypeTemplate_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadTypeTemplateID

				paramList.Add(currentParam)

				' UploadTypeAttribute_ID
				currentParam = New DBParam
				currentParam.Name = "UploadTypeAttribute_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadTypeAttributeID

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
				outputList = factory.ExecuteStoredProcedure(UpdateUploadTypeTemplateAttributeStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				outputList = factory.ExecuteStoredProcedure(UpdateUploadTypeTemplateAttributeStoredProcName, paramList)
			End If

			Return CInt(outputList(0))

		End Function
		
		''' <summary>
		''' Set this in the UploadTypeTemplateAttributeDAO sub class to change the stored
		''' procedure used by the GetAllUploadTypeTemplateAttributes function.
		''' </summary>
		Public DeleteUploadTypeTemplateAttributeStoredProcName As String = "EIM_Regen_DeleteUploadTypeTemplateAttribute"

		Public Overridable Function DeleteUploadTypeTemplateAttribute(ByVal businessObject As UploadTypeTemplateAttribute) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' UploadTypeTemplateAttribute_ID
				currentParam = New DBParam
				currentParam.Name = "UploadTypeTemplateAttribute_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadTypeTemplateAttributeID

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
				outputList = factory.ExecuteStoredProcedure(DeleteUploadTypeTemplateAttributeStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				outputList = factory.ExecuteStoredProcedure(DeleteUploadTypeTemplateAttributeStoredProcName, paramList)
			End If

			Return CInt(outputList(0))

		End Function

    End Class

End Namespace
