

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

	''' <summary>
	''' Generated Data Access Object base class for the UploadType db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	David Marine
	''' Created   :	Jul 29, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class UploadTypeDAORegen
    
		''' <summary>
		''' Returns a list of UploadTypes from the provided resultset.
		''' </summary>
		''' <returns>ArrayList of UploadTypes</returns>
		''' <remarks></remarks>
		Protected Overridable Function GetUploadTypesFromResultSet (ByVal results As SqlDataReader) As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim businessObject As UploadType
			
			While results.Read

				businessObject = New UploadType()

				businessObject.UploadTypeCode = results.GetString(results.GetOrdinal("UploadType_Code"))                         
				businessObject.Name = results.GetString(results.GetOrdinal("Name"))                         

				' Description can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Description"))) Then
					businessObject.Description = Nothing
				Else
					businessObject.Description = results.GetString(results.GetOrdinal("Description"))                         
				End If
				businessObject.IsActive = results.GetBoolean(results.GetOrdinal("IsActive"))                         
				
				' reset all the flags
				businessObject.IsNew = False
				businessObject.IsDirty = False
				businessObject.IsMarkedForDelete = False
				businessObject.IsDeleted = False

				' add business object to BusinessObjectCollection with abstracted PK key
				businessObjectTable.Add(businessObject.PrimaryKey, businessObject)
				
			End While

			Trace.WriteLine("Loading " + businessObjectTable.Count.ToString() + " UploadTypes from the database.")
			
			Return businessObjectTable

		End Function
		
		''' <summary>
		''' Set this in the UploadTypeDAO sub class to change the stored
		''' procedure used by the GetAllUploadTypes function.
		''' </summary>
		Public GetAllUploadTypesStoredProcName As String = "EIM_Regen_GetAllUploadTypes"
		
		''' <summary>
		''' Returns all UploadTypes.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetAllUploadTypes () As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim results As SqlDataReader = Nothing

			Try
			
				' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetAllUploadTypesStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetAllUploadTypesStoredProcName, paramList)
				End If
				
				businessObjectTable = GetUploadTypesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadTypeDAO sub class to change the stored
		''' procedure used by the GetUploadTypeByPK function.
		''' </summary>
		Protected GetUploadTypeByPKStoredProcName As String = "EIM_Regen_GetUploadTypeByPK"
		
		''' <summary>
		''' Returns zero or more UploadTypes by PK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadTypeByPK (ByRef uploadTypeCode As System.String) As UploadType


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
					results = factory.GetStoredProcedureDataReader(Me.GetUploadTypeByPKStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadTypeByPKStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadTypesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Dim businessObject As UploadType = Nothing
            If businessObjectTable.Count > 0 Then
                businessObject = CType(businessObjectTable.Item(0), UploadType)
            End If
			
			Return businessObject

		End Function


		''' <summary>
		''' Set this in the UploadTypeDAO sub class to change the stored
		''' procedure used by the GetAllUploadTypes function.
		''' </summary>
		Public InsertUploadTypeStoredProcName As String = "EIM_Regen_InsertUploadType"

		Public Overridable Sub InsertUploadType(ByRef businessObject As UploadType)

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim pkValueList As New ArrayList
            
			' setup parameters for stored proc

				' Name
				currentParam = New DBParam
				currentParam.Name = "Name"
				currentParam.Type = DBParamType.String
				currentParam.Value = businessObject.Name

				paramList.Add(currentParam)

				' Description
				currentParam = New DBParam
				currentParam.Name = "Description"
				currentParam.Type = DBParamType.String
				If businessObject.IsDescriptionNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Description
				End If

				paramList.Add(currentParam)

				' IsActive
				currentParam = New DBParam
				currentParam.Name = "IsActive"
				currentParam.Type = DBParamType.Bit
				currentParam.Value = businessObject.IsActive

				paramList.Add(currentParam)


			' get the current transaction if any
			Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
			
			' Execute the stored procedure
			If Not IsNothing(theTransaction) Then
				pkValueList = factory.ExecuteStoredProcedure(InsertUploadTypeStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				pkValueList = factory.ExecuteStoredProcedure(InsertUploadTypeStoredProcName, paramList)
			End If


		End Sub
		
		''' <summary>
		''' Set this in the UploadTypeDAO sub class to change the stored
		''' procedure used by the GetAllUploadTypes function.
		''' </summary>
		Public UpdateUploadTypeStoredProcName As String = "EIM_Regen_UpdateUploadType"

		Public Overridable Function UpdateUploadType(ByVal businessObject As UploadType) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

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

				' Description
				currentParam = New DBParam
				currentParam.Name = "Description"
				currentParam.Type = DBParamType.String
				If businessObject.IsDescriptionNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Description
				End If

				paramList.Add(currentParam)

				' IsActive
				currentParam = New DBParam
				currentParam.Name = "IsActive"
				currentParam.Type = DBParamType.Bit
				currentParam.Value = businessObject.IsActive

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
				outputList = factory.ExecuteStoredProcedure(UpdateUploadTypeStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				outputList = factory.ExecuteStoredProcedure(UpdateUploadTypeStoredProcName, paramList)
			End If

			Return CInt(outputList(0))

		End Function
		
		''' <summary>
		''' Set this in the UploadTypeDAO sub class to change the stored
		''' procedure used by the GetAllUploadTypes function.
		''' </summary>
		Public DeleteUploadTypeStoredProcName As String = "EIM_Regen_DeleteUploadType"

		Public Overridable Function DeleteUploadType(ByVal businessObject As UploadType) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' UploadType_Code
				currentParam = New DBParam
				currentParam.Name = "UploadType_Code"
				currentParam.Type = DBParamType.String
				currentParam.Value = businessObject.UploadTypeCode

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
				outputList = factory.ExecuteStoredProcedure(DeleteUploadTypeStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				outputList = factory.ExecuteStoredProcedure(DeleteUploadTypeStoredProcName, paramList)
			End If

			Return CInt(outputList(0))

		End Function

    End Class

End Namespace
