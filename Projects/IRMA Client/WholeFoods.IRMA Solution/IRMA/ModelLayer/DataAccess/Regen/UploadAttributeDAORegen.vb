

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

	''' <summary>
	''' Generated Data Access Object base class for the UploadAttribute db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	David Marine
	''' Created   :	Jul 29, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class UploadAttributeDAORegen
    
		''' <summary>
		''' Returns a list of UploadAttributes from the provided resultset.
		''' </summary>
		''' <returns>ArrayList of UploadAttributes</returns>
		''' <remarks></remarks>
		Protected Overridable Function GetUploadAttributesFromResultSet (ByVal results As SqlDataReader) As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim businessObject As UploadAttribute
			
			While results.Read

				businessObject = New UploadAttribute()

				businessObject.UploadAttributeID = results.GetInt32(results.GetOrdinal("UploadAttribute_ID"))                         
				businessObject.Name = results.GetString(results.GetOrdinal("Name"))                         
				businessObject.TableName = results.GetString(results.GetOrdinal("TableName"))                         
				businessObject.ColumnNameOrKey = results.GetString(results.GetOrdinal("ColumnNameOrKey"))                         
				businessObject.ControlType = results.GetString(results.GetOrdinal("ControlType"))                         
				businessObject.DbDataType = results.GetString(results.GetOrdinal("DbDataType"))                         

				' Size can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Size"))) Then
					' the businessObject.Size property cannot be set to null
					' so set the businessObject.IsSizeNull flag to true.
					businessObject.IsSizeNull = True
				Else
					businessObject.Size = results.GetInt32(results.GetOrdinal("Size"))                         
				End If
				businessObject.IsRequiredValue = results.GetBoolean(results.GetOrdinal("IsRequiredValue"))                         
				businessObject.IsCalculated = results.GetBoolean(results.GetOrdinal("IsCalculated"))                         

				' OptionalMinValue can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("OptionalMinValue"))) Then
					businessObject.OptionalMinValue = Nothing
				Else
					businessObject.OptionalMinValue = results.GetString(results.GetOrdinal("OptionalMinValue"))                         
				End If

				' OptionalMaxValue can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("OptionalMaxValue"))) Then
					businessObject.OptionalMaxValue = Nothing
				Else
					businessObject.OptionalMaxValue = results.GetString(results.GetOrdinal("OptionalMaxValue"))                         
				End If
				businessObject.IsActive = results.GetBoolean(results.GetOrdinal("IsActive"))                         

				' DisplayFormatString can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("DisplayFormatString"))) Then
					businessObject.DisplayFormatString = Nothing
				Else
					businessObject.DisplayFormatString = results.GetString(results.GetOrdinal("DisplayFormatString"))                         
				End If

				' PopulateProcedure can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("PopulateProcedure"))) Then
					businessObject.PopulateProcedure = Nothing
				Else
					businessObject.PopulateProcedure = results.GetString(results.GetOrdinal("PopulateProcedure"))                         
				End If

				' PopulateIndexField can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("PopulateIndexField"))) Then
					businessObject.PopulateIndexField = Nothing
				Else
					businessObject.PopulateIndexField = results.GetString(results.GetOrdinal("PopulateIndexField"))                         
				End If

				' PopulateDescriptionField can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("PopulateDescriptionField"))) Then
					businessObject.PopulateDescriptionField = Nothing
				Else
					businessObject.PopulateDescriptionField = results.GetString(results.GetOrdinal("PopulateDescriptionField"))                         
				End If
				businessObject.SpreadsheetPosition = results.GetInt32(results.GetOrdinal("SpreadsheetPosition"))                         

				' ValueListStaticData can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("ValueListStaticData"))) Then
					businessObject.ValueListStaticData = Nothing
				Else
					businessObject.ValueListStaticData = results.GetString(results.GetOrdinal("ValueListStaticData"))                         
				End If

				' DefaultValue can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("DefaultValue"))) Then
					businessObject.DefaultValue = Nothing
				Else
					businessObject.DefaultValue = results.GetString(results.GetOrdinal("DefaultValue"))                         
				End If
				
				' reset all the flags
				businessObject.IsNew = False
				businessObject.IsDirty = False
				businessObject.IsMarkedForDelete = False
				businessObject.IsDeleted = False

				' add business object to BusinessObjectCollection with abstracted PK key
				businessObjectTable.Add(businessObject.PrimaryKey, businessObject)
				
			End While

			Trace.WriteLine("Loading " + businessObjectTable.Count.ToString() + " UploadAttributes from the database.")
			
			Return businessObjectTable

		End Function
		
		''' <summary>
		''' Set this in the UploadAttributeDAO sub class to change the stored
		''' procedure used by the GetAllUploadAttributes function.
		''' </summary>
		Public GetAllUploadAttributesStoredProcName As String = "EIM_Regen_GetAllUploadAttributes"
		
		''' <summary>
		''' Returns all UploadAttributes.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetAllUploadAttributes () As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim results As SqlDataReader = Nothing

			Try
			
				' get the current transaction if any
				Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
				
				' Execute the stored procedure
				If Not IsNothing(theTransaction) Then
					results = factory.GetStoredProcedureDataReader(Me.GetAllUploadAttributesStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetAllUploadAttributesStoredProcName, paramList)
				End If
				
				businessObjectTable = GetUploadAttributesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the UploadAttributeDAO sub class to change the stored
		''' procedure used by the GetUploadAttributeByPK function.
		''' </summary>
		Protected GetUploadAttributeByPKStoredProcName As String = "EIM_Regen_GetUploadAttributeByPK"
		
		''' <summary>
		''' Returns zero or more UploadAttributes by PK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetUploadAttributeByPK (ByRef uploadAttributeID As System.Int32) As UploadAttribute


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
					results = factory.GetStoredProcedureDataReader(Me.GetUploadAttributeByPKStoredProcName, paramList, theTransaction.SqlTransaction)
				Else
					results = factory.GetStoredProcedureDataReader(Me.GetUploadAttributeByPKStoredProcName, paramList)
				End If

				businessObjectTable = GetUploadAttributesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Dim businessObject As UploadAttribute = Nothing
            If businessObjectTable.Count > 0 Then
                businessObject = CType(businessObjectTable.Item(0), UploadAttribute)
            End If
			
			Return businessObject

		End Function


		''' <summary>
		''' Set this in the UploadAttributeDAO sub class to change the stored
		''' procedure used by the GetAllUploadAttributes function.
		''' </summary>
		Public InsertUploadAttributeStoredProcName As String = "EIM_Regen_InsertUploadAttribute"

		Public Overridable Sub InsertUploadAttribute(ByRef businessObject As UploadAttribute)

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

				' TableName
				currentParam = New DBParam
				currentParam.Name = "TableName"
				currentParam.Type = DBParamType.String
				currentParam.Value = businessObject.TableName

				paramList.Add(currentParam)

				' ColumnNameOrKey
				currentParam = New DBParam
				currentParam.Name = "ColumnNameOrKey"
				currentParam.Type = DBParamType.String
				currentParam.Value = businessObject.ColumnNameOrKey

				paramList.Add(currentParam)

				' ControlType
				currentParam = New DBParam
				currentParam.Name = "ControlType"
				currentParam.Type = DBParamType.String
				currentParam.Value = businessObject.ControlType

				paramList.Add(currentParam)

				' DbDataType
				currentParam = New DBParam
				currentParam.Name = "DbDataType"
				currentParam.Type = DBParamType.String
				currentParam.Value = businessObject.DbDataType

				paramList.Add(currentParam)

				' Size
				currentParam = New DBParam
				currentParam.Name = "Size"
				currentParam.Type = DBParamType.Int
				If businessObject.IsSizeNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Size
				End If

				paramList.Add(currentParam)

				' IsRequiredValue
				currentParam = New DBParam
				currentParam.Name = "IsRequiredValue"
				currentParam.Type = DBParamType.Bit
				currentParam.Value = businessObject.IsRequiredValue

				paramList.Add(currentParam)

				' IsCalculated
				currentParam = New DBParam
				currentParam.Name = "IsCalculated"
				currentParam.Type = DBParamType.Bit
				currentParam.Value = businessObject.IsCalculated

				paramList.Add(currentParam)

				' OptionalMinValue
				currentParam = New DBParam
				currentParam.Name = "OptionalMinValue"
				currentParam.Type = DBParamType.String
				If businessObject.IsOptionalMinValueNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.OptionalMinValue
				End If

				paramList.Add(currentParam)

				' OptionalMaxValue
				currentParam = New DBParam
				currentParam.Name = "OptionalMaxValue"
				currentParam.Type = DBParamType.String
				If businessObject.IsOptionalMaxValueNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.OptionalMaxValue
				End If

				paramList.Add(currentParam)

				' IsActive
				currentParam = New DBParam
				currentParam.Name = "IsActive"
				currentParam.Type = DBParamType.Bit
				currentParam.Value = businessObject.IsActive

				paramList.Add(currentParam)

				' DisplayFormatString
				currentParam = New DBParam
				currentParam.Name = "DisplayFormatString"
				currentParam.Type = DBParamType.String
				If businessObject.IsDisplayFormatStringNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DisplayFormatString
				End If

				paramList.Add(currentParam)

				' PopulateProcedure
				currentParam = New DBParam
				currentParam.Name = "PopulateProcedure"
				currentParam.Type = DBParamType.String
				If businessObject.IsPopulateProcedureNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.PopulateProcedure
				End If

				paramList.Add(currentParam)

				' PopulateIndexField
				currentParam = New DBParam
				currentParam.Name = "PopulateIndexField"
				currentParam.Type = DBParamType.String
				If businessObject.IsPopulateIndexFieldNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.PopulateIndexField
				End If

				paramList.Add(currentParam)

				' PopulateDescriptionField
				currentParam = New DBParam
				currentParam.Name = "PopulateDescriptionField"
				currentParam.Type = DBParamType.String
				If businessObject.IsPopulateDescriptionFieldNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.PopulateDescriptionField
				End If

				paramList.Add(currentParam)

				' SpreadsheetPosition
				currentParam = New DBParam
				currentParam.Name = "SpreadsheetPosition"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.SpreadsheetPosition

				paramList.Add(currentParam)

				' ValueListStaticData
				currentParam = New DBParam
				currentParam.Name = "ValueListStaticData"
				currentParam.Type = DBParamType.String
				If businessObject.IsValueListStaticDataNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ValueListStaticData
				End If

				paramList.Add(currentParam)

				' DefaultValue
				currentParam = New DBParam
				currentParam.Name = "DefaultValue"
				currentParam.Type = DBParamType.String
				If businessObject.IsDefaultValueNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DefaultValue
				End If

				paramList.Add(currentParam)

			' Get the output value
			currentParam = New DBParam
			currentParam.Name = "UploadAttribute_ID"
			currentParam.Type = DBParamType.Int
			paramList.Add(currentParam)

			' get the current transaction if any
			Dim theTransaction As Transaction = TransactionManager.Instance.CurrentTransaction
			
			' Execute the stored procedure
			If Not IsNothing(theTransaction) Then
				pkValueList = factory.ExecuteStoredProcedure(InsertUploadAttributeStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				pkValueList = factory.ExecuteStoredProcedure(InsertUploadAttributeStoredProcName, paramList)
			End If

			' set the returned pk value
			businessObject.UploadAttributeID = CInt(pkValueList(0))

		End Sub
		
		''' <summary>
		''' Set this in the UploadAttributeDAO sub class to change the stored
		''' procedure used by the GetAllUploadAttributes function.
		''' </summary>
		Public UpdateUploadAttributeStoredProcName As String = "EIM_Regen_UpdateUploadAttribute"

		Public Overridable Function UpdateUploadAttribute(ByVal businessObject As UploadAttribute) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' UploadAttribute_ID
				currentParam = New DBParam
				currentParam.Name = "UploadAttribute_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadAttributeID

				paramList.Add(currentParam)

				' Name
				currentParam = New DBParam
				currentParam.Name = "Name"
				currentParam.Type = DBParamType.String
				currentParam.Value = businessObject.Name

				paramList.Add(currentParam)

				' TableName
				currentParam = New DBParam
				currentParam.Name = "TableName"
				currentParam.Type = DBParamType.String
				currentParam.Value = businessObject.TableName

				paramList.Add(currentParam)

				' ColumnNameOrKey
				currentParam = New DBParam
				currentParam.Name = "ColumnNameOrKey"
				currentParam.Type = DBParamType.String
				currentParam.Value = businessObject.ColumnNameOrKey

				paramList.Add(currentParam)

				' ControlType
				currentParam = New DBParam
				currentParam.Name = "ControlType"
				currentParam.Type = DBParamType.String
				currentParam.Value = businessObject.ControlType

				paramList.Add(currentParam)

				' DbDataType
				currentParam = New DBParam
				currentParam.Name = "DbDataType"
				currentParam.Type = DBParamType.String
				currentParam.Value = businessObject.DbDataType

				paramList.Add(currentParam)

				' Size
				currentParam = New DBParam
				currentParam.Name = "Size"
				currentParam.Type = DBParamType.Int
				If businessObject.IsSizeNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Size
				End If

				paramList.Add(currentParam)

				' IsRequiredValue
				currentParam = New DBParam
				currentParam.Name = "IsRequiredValue"
				currentParam.Type = DBParamType.Bit
				currentParam.Value = businessObject.IsRequiredValue

				paramList.Add(currentParam)

				' IsCalculated
				currentParam = New DBParam
				currentParam.Name = "IsCalculated"
				currentParam.Type = DBParamType.Bit
				currentParam.Value = businessObject.IsCalculated

				paramList.Add(currentParam)

				' OptionalMinValue
				currentParam = New DBParam
				currentParam.Name = "OptionalMinValue"
				currentParam.Type = DBParamType.String
				If businessObject.IsOptionalMinValueNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.OptionalMinValue
				End If

				paramList.Add(currentParam)

				' OptionalMaxValue
				currentParam = New DBParam
				currentParam.Name = "OptionalMaxValue"
				currentParam.Type = DBParamType.String
				If businessObject.IsOptionalMaxValueNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.OptionalMaxValue
				End If

				paramList.Add(currentParam)

				' IsActive
				currentParam = New DBParam
				currentParam.Name = "IsActive"
				currentParam.Type = DBParamType.Bit
				currentParam.Value = businessObject.IsActive

				paramList.Add(currentParam)

				' DisplayFormatString
				currentParam = New DBParam
				currentParam.Name = "DisplayFormatString"
				currentParam.Type = DBParamType.String
				If businessObject.IsDisplayFormatStringNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DisplayFormatString
				End If

				paramList.Add(currentParam)

				' PopulateProcedure
				currentParam = New DBParam
				currentParam.Name = "PopulateProcedure"
				currentParam.Type = DBParamType.String
				If businessObject.IsPopulateProcedureNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.PopulateProcedure
				End If

				paramList.Add(currentParam)

				' PopulateIndexField
				currentParam = New DBParam
				currentParam.Name = "PopulateIndexField"
				currentParam.Type = DBParamType.String
				If businessObject.IsPopulateIndexFieldNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.PopulateIndexField
				End If

				paramList.Add(currentParam)

				' PopulateDescriptionField
				currentParam = New DBParam
				currentParam.Name = "PopulateDescriptionField"
				currentParam.Type = DBParamType.String
				If businessObject.IsPopulateDescriptionFieldNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.PopulateDescriptionField
				End If

				paramList.Add(currentParam)

				' SpreadsheetPosition
				currentParam = New DBParam
				currentParam.Name = "SpreadsheetPosition"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.SpreadsheetPosition

				paramList.Add(currentParam)

				' ValueListStaticData
				currentParam = New DBParam
				currentParam.Name = "ValueListStaticData"
				currentParam.Type = DBParamType.String
				If businessObject.IsValueListStaticDataNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ValueListStaticData
				End If

				paramList.Add(currentParam)

				' DefaultValue
				currentParam = New DBParam
				currentParam.Name = "DefaultValue"
				currentParam.Type = DBParamType.String
				If businessObject.IsDefaultValueNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DefaultValue
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
				outputList = factory.ExecuteStoredProcedure(UpdateUploadAttributeStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				outputList = factory.ExecuteStoredProcedure(UpdateUploadAttributeStoredProcName, paramList)
			End If

			Return CInt(outputList(0))

		End Function
		
		''' <summary>
		''' Set this in the UploadAttributeDAO sub class to change the stored
		''' procedure used by the GetAllUploadAttributes function.
		''' </summary>
		Public DeleteUploadAttributeStoredProcName As String = "EIM_Regen_DeleteUploadAttribute"

		Public Overridable Function DeleteUploadAttribute(ByVal businessObject As UploadAttribute) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' UploadAttribute_ID
				currentParam = New DBParam
				currentParam.Name = "UploadAttribute_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.UploadAttributeID

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
				outputList = factory.ExecuteStoredProcedure(DeleteUploadAttributeStoredProcName, paramList, theTransaction.SqlTransaction)
			Else
				outputList = factory.ExecuteStoredProcedure(DeleteUploadAttributeStoredProcName, paramList)
			End If

			Return CInt(outputList(0))

		End Function

    End Class

End Namespace
