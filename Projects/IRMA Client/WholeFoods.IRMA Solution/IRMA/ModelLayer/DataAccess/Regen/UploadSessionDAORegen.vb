

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

	''' <summary>
	''' Generated Data Access Object base class for the UploadSession db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	David Marine
	''' Created   :	Jul 29, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class UploadSessionDAORegen
    
		''' <summary>
		''' Returns a list of UploadSessions from the provided resultset.
		''' </summary>
		''' <returns>ArrayList of UploadSessions</returns>
		''' <remarks></remarks>
		Protected Overridable Function GetUploadSessionsFromResultSet (ByVal results As SqlDataReader) As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim businessObject As UploadSession
			
			While results.Read

				businessObject = New UploadSession()

				businessObject.UploadSessionID = results.GetInt32(results.GetOrdinal("UploadSession_ID"))                         

				' Name can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Name"))) Then
					businessObject.Name = Nothing
				Else
					businessObject.Name = results.GetString(results.GetOrdinal("Name"))                         
				End If
				businessObject.IsUploaded = results.GetBoolean(results.GetOrdinal("IsUploaded"))                         

				' ItemsProcessedCount can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("ItemsProcessedCount"))) Then
					' the businessObject.ItemsProcessedCount property cannot be set to null
					' so set the businessObject.IsItemsProcessedCountNull flag to true.
					businessObject.IsItemsProcessedCountNull = True
				Else
					businessObject.ItemsProcessedCount = results.GetInt32(results.GetOrdinal("ItemsProcessedCount"))                         
				End If

				' ItemsLoadedCount can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("ItemsLoadedCount"))) Then
					' the businessObject.ItemsLoadedCount property cannot be set to null
					' so set the businessObject.IsItemsLoadedCountNull flag to true.
					businessObject.IsItemsLoadedCountNull = True
				Else
					businessObject.ItemsLoadedCount = results.GetInt32(results.GetOrdinal("ItemsLoadedCount"))                         
				End If

				' ErrorsCount can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("ErrorsCount"))) Then
					' the businessObject.ErrorsCount property cannot be set to null
					' so set the businessObject.IsErrorsCountNull flag to true.
					businessObject.IsErrorsCountNull = True
				Else
					businessObject.ErrorsCount = results.GetInt32(results.GetOrdinal("ErrorsCount"))                         
				End If
				businessObject.EmailToAddress = results.GetString(results.GetOrdinal("EmailToAddress"))                         
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

				' IsNewItemSessionFlag can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("IsNewItemSessionFlag"))) Then
					' the businessObject.IsNewItemSessionFlag property cannot be set to null
					' so set the businessObject.IsIsNewItemSessionFlagNull flag to true.
					businessObject.IsIsNewItemSessionFlagNull = True
				Else
					businessObject.IsNewItemSessionFlag = results.GetBoolean(results.GetOrdinal("IsNewItemSessionFlag"))                         
				End If
				businessObject.IsFromSLIM = results.GetBoolean(results.GetOrdinal("IsFromSLIM"))                         
				
				' reset all the flags
				businessObject.IsNew = False
				businessObject.IsDirty = False
				businessObject.IsMarkedForDelete = False
				businessObject.IsDeleted = False

				' add business object to BusinessObjectCollection with abstracted PK key
				businessObjectTable.Add(businessObject.PrimaryKey, businessObject)
				
			End While

			Trace.WriteLine("Loading " + businessObjectTable.Count.ToString() + " UploadSessions from the database.")
			
			Return businessObjectTable

		End Function
		
		''' <summary>
		''' Set this in the UploadSessionDAO sub class to change the stored
		''' procedure used by the GetAllUploadSessions function.
		''' </summary>
		Public GetAllUploadSessionsStoredProcName As String = "EIM_Regen_GetAllUploadSessions"
		
		''' <summary>
		''' Returns all UploadSessions.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetAllUploadSessions () As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim results As SqlDataReader = Nothing

			Try
			
				' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetAllUploadSessionsStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetAllUploadSessionsStoredProcName, paramList)
				End If
				
				businessObjectTable = GetUploadSessionsFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadSessionDAO sub class to change the stored
		''' procedure used by the GetUploadSessionByPK function.
		''' </summary>
		Protected GetUploadSessionByPKStoredProcName As String = "EIM_Regen_GetUploadSessionByPK"
		
		''' <summary>
		''' Returns zero or more UploadSessions by PK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadSessionByPK (ByRef uploadSessionID As System.Int32) As UploadSession


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
					results = factory.GetStoredProcedureDataReader(Me.GetUploadSessionByPKStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadSessionByPKStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadSessionsFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Dim businessObject As UploadSession = Nothing
            If businessObjectTable.Count > 0 Then
                businessObject = CType(businessObjectTable.Item(0), UploadSession)
            End If
			
			Return businessObject

		End Function


		''' <summary>
		''' Set this in the UploadSessionDAO sub class to change the stored
		''' procedure used by the GetAllUploadSessions function.
		''' </summary>
		Public InsertUploadSessionStoredProcName As String = "EIM_Regen_InsertUploadSession"

		Public Overridable Sub InsertUploadSession(ByRef businessObject As UploadSession)

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim pkValueList As New ArrayList
            
			' setup parameters for stored proc

				' Name
				currentParam = New DBParam
				currentParam.Name = "Name"
				currentParam.Type = DBParamType.String
				If businessObject.IsNameNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Name
				End If

				paramList.Add(currentParam)

				' IsUploaded
				currentParam = New DBParam
				currentParam.Name = "IsUploaded"
				currentParam.Type = DBParamType.Bit
				currentParam.Value = businessObject.IsUploaded

				paramList.Add(currentParam)

				' ItemsProcessedCount
				currentParam = New DBParam
				currentParam.Name = "ItemsProcessedCount"
				currentParam.Type = DBParamType.Int
				If businessObject.IsItemsProcessedCountNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ItemsProcessedCount
				End If

				paramList.Add(currentParam)

				' ItemsLoadedCount
				currentParam = New DBParam
				currentParam.Name = "ItemsLoadedCount"
				currentParam.Type = DBParamType.Int
				If businessObject.IsItemsLoadedCountNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ItemsLoadedCount
				End If

				paramList.Add(currentParam)

				' ErrorsCount
				currentParam = New DBParam
				currentParam.Name = "ErrorsCount"
				currentParam.Type = DBParamType.Int
				If businessObject.IsErrorsCountNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ErrorsCount
				End If

				paramList.Add(currentParam)

				' EmailToAddress
				currentParam = New DBParam
				currentParam.Name = "EmailToAddress"
				currentParam.Type = DBParamType.String
				currentParam.Value = businessObject.EmailToAddress

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

				' IsNewItemSessionFlag
				currentParam = New DBParam
				currentParam.Name = "IsNewItemSessionFlag"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsIsNewItemSessionFlagNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.IsNewItemSessionFlag
				End If

				paramList.Add(currentParam)

            ' IsDeleteItemSessionFlag
            currentParam = New DBParam
            currentParam.Name = "IsDeleteItemSessionFlag"
            currentParam.Type = DBParamType.Bit
            If businessObject.IsIsDeleteItemSessionFlagNull Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = businessObject.IsDeleteItemSessionFlag
            End If

            paramList.Add(currentParam)

				' IsFromSLIM
				currentParam = New DBParam
				currentParam.Name = "IsFromSLIM"
				currentParam.Type = DBParamType.Bit
				currentParam.Value = businessObject.IsFromSLIM

				paramList.Add(currentParam)

			' Get the output value
			currentParam = New DBParam
			currentParam.Name = "UploadSession_ID"
			currentParam.Type = DBParamType.Int
			paramList.Add(currentParam)

			' get the current transaction if any
			Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
			
			' Execute the stored procedure
			If Not IsNothing(theTransaction) Then
				pkValueList = factory.ExecuteStoredProcedure(InsertUploadSessionStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				pkValueList = factory.ExecuteStoredProcedure(InsertUploadSessionStoredProcName, paramList)
			End If

			' set the returned pk value
			businessObject.UploadSessionID = CInt(pkValueList(0))

		End Sub
		
		''' <summary>
		''' Set this in the UploadSessionDAO sub class to change the stored
		''' procedure used by the GetAllUploadSessions function.
		''' </summary>
		Public UpdateUploadSessionStoredProcName As String = "EIM_Regen_UpdateUploadSession"

		Public Overridable Function UpdateUploadSession(ByVal businessObject As UploadSession) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' UploadSession_ID
				currentParam = New DBParam
				currentParam.Name = "UploadSession_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadSessionID

				paramList.Add(currentParam)

				' Name
				currentParam = New DBParam
				currentParam.Name = "Name"
				currentParam.Type = DBParamType.String
				If businessObject.IsNameNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Name
				End If

				paramList.Add(currentParam)

				' IsUploaded
				currentParam = New DBParam
				currentParam.Name = "IsUploaded"
				currentParam.Type = DBParamType.Bit
				currentParam.Value = businessObject.IsUploaded

				paramList.Add(currentParam)

				' ItemsProcessedCount
				currentParam = New DBParam
				currentParam.Name = "ItemsProcessedCount"
				currentParam.Type = DBParamType.Int
				If businessObject.IsItemsProcessedCountNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ItemsProcessedCount
				End If

				paramList.Add(currentParam)

				' ItemsLoadedCount
				currentParam = New DBParam
				currentParam.Name = "ItemsLoadedCount"
				currentParam.Type = DBParamType.Int
				If businessObject.IsItemsLoadedCountNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ItemsLoadedCount
				End If

				paramList.Add(currentParam)

				' ErrorsCount
				currentParam = New DBParam
				currentParam.Name = "ErrorsCount"
				currentParam.Type = DBParamType.Int
				If businessObject.IsErrorsCountNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ErrorsCount
				End If

				paramList.Add(currentParam)

				' EmailToAddress
				currentParam = New DBParam
				currentParam.Name = "EmailToAddress"
				currentParam.Type = DBParamType.String
				currentParam.Value = businessObject.EmailToAddress

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

				' IsNewItemSessionFlag
				currentParam = New DBParam
				currentParam.Name = "IsNewItemSessionFlag"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsIsNewItemSessionFlagNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.IsNewItemSessionFlag
				End If

				paramList.Add(currentParam)

            ' IsDeleteItemSessionFlag
            currentParam = New DBParam
            currentParam.Name = "IsDeleteItemSessionFlag"
            currentParam.Type = DBParamType.Bit
            If businessObject.IsIsDeleteItemSessionFlagNull Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = businessObject.IsDeleteItemSessionFlag
            End If

            paramList.Add(currentParam)

            ' IsFromSLIM
            currentParam = New DBParam
            currentParam.Name = "IsFromSLIM"
            currentParam.Type = DBParamType.Bit
            currentParam.Value = businessObject.IsFromSLIM

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
				outputList = factory.ExecuteStoredProcedure(UpdateUploadSessionStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				outputList = factory.ExecuteStoredProcedure(UpdateUploadSessionStoredProcName, paramList)
			End If

			Return CInt(outputList(0))

		End Function
		
		''' <summary>
		''' Set this in the UploadSessionDAO sub class to change the stored
		''' procedure used by the GetAllUploadSessions function.
		''' </summary>
		Public DeleteUploadSessionStoredProcName As String = "EIM_Regen_DeleteUploadSession"

		Public Overridable Function DeleteUploadSession(ByVal businessObject As UploadSession) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' UploadSession_ID
				currentParam = New DBParam
				currentParam.Name = "UploadSession_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadSessionID

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
				outputList = factory.ExecuteStoredProcedure(DeleteUploadSessionStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				outputList = factory.ExecuteStoredProcedure(DeleteUploadSessionStoredProcName, paramList)
			End If

			Return CInt(outputList(0))

		End Function

    End Class

End Namespace
