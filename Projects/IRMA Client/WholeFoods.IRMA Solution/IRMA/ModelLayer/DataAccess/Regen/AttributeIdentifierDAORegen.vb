

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

	''' <summary>
	''' Generated Data Access Object base class for the AttributeIdentifier db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	James Winfield
	''' Created   :	Mar 01, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class AttributeIdentifierDAORegen
    
		''' <summary>
		''' Returns a list of AttributeIdentifiers from the provided resultset.
		''' </summary>
		''' <returns>ArrayList of AttributeIdentifiers</returns>
		''' <remarks></remarks>
		Protected Overridable Function GetAttributeIdentifiersFromResultSet (ByVal results As SqlDataReader) As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim businessObject As AttributeIdentifier
			
			While results.Read

				businessObject = New AttributeIdentifier()

				businessObject.AttributeIdentifierID = results.GetInt32(results.GetOrdinal("AttributeIdentifier_ID"))                         

				' Screen_Text can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Screen_Text"))) Then
					businessObject.ScreenText = Nothing
				Else
					businessObject.ScreenText = results.GetString(results.GetOrdinal("Screen_Text"))                         
				End If

				' field_type can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("field_type"))) Then
					businessObject.FieldType = Nothing
				Else
					businessObject.FieldType = results.GetString(results.GetOrdinal("field_type"))                         
				End If

				' combo_box can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("combo_box"))) Then
					' the businessObject.ComboBox property cannot be set to null
					' so set the businessObject.IsComboBoxNull flag to true.
					businessObject.IsComboBoxNull = True
				Else
					businessObject.ComboBox = results.GetBoolean(results.GetOrdinal("combo_box"))                         
				End If

				' max_width can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("max_width"))) Then
					' the businessObject.MaxWidth property cannot be set to null
					' so set the businessObject.IsMaxWidthNull flag to true.
					businessObject.IsMaxWidthNull = True
				Else
					businessObject.MaxWidth = results.GetInt32(results.GetOrdinal("max_width"))                         
				End If

				' default_value can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("default_value"))) Then
					businessObject.DefaultValue = Nothing
				Else
					businessObject.DefaultValue = results.GetString(results.GetOrdinal("default_value"))                         
				End If

				' field_values can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("field_values"))) Then
					businessObject.FieldValues = Nothing
				Else
					businessObject.FieldValues = results.GetString(results.GetOrdinal("field_values"))                         
				End If
				
				' reset all the flags
				businessObject.IsNew = False
				businessObject.IsDirty = False
				businessObject.IsMarkedForDelete = False
				businessObject.IsDeleted = False

				' add business object to BusinessObjectCollection with PK as key
				businessObjectTable.Add(businessObject.AttributeIdentifierID, businessObject)
				
			End While

			Trace.WriteLine("Loading " + businessObjectTable.Count.ToString() + " AttributeIdentifiers from the database.")
			
			Return businessObjectTable

		End Function
		
		''' <summary>
		''' Set this in the AttributeIdentifierDAO sub class to change the stored
		''' procedure used by the GetAllAttributeIdentifiers function.
		''' </summary>
		Public GetAllAttributeIdentifiersStoredProcName As String = "ItemAttributes_Regen_GetAllAttributeIdentifiers"
		
		''' <summary>
		''' Returns all AttributeIdentifiers.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetAllAttributeIdentifiers () As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim results As SqlDataReader = Nothing

			Try

				' Execute the stored procedure 
				results = factory.GetStoredProcedureDataReader(Me.GetAllAttributeIdentifiersStoredProcName, paramList)

				businessObjectTable = GetAttributeIdentifiersFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the AttributeIdentifierDAO sub class to change the stored
		''' procedure used by the GetAttributeIdentifierByPK function.
		''' </summary>
		Protected GetAttributeIdentifierByPKStoredProcName As String = "ItemAttributes_Regen_GetAttributeIdentifierByPK"
		
		''' <summary>
		''' Returns zero or more AttributeIdentifiers by PK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetAttributeIdentifierByPK (ByRef attributeIdentifierID As System.Int32) As AttributeIdentifier


			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim results As SqlDataReader = Nothing

			Try
				' setup identifier for stored proc
				currentParam = New DBParam
				currentParam.Name = "AttributeIdentifier_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = attributeIdentifierID 
				paramList.Add(currentParam)

				' Execute the stored procedure 
				results = factory.GetStoredProcedureDataReader(Me.GetAttributeIdentifierByPKStoredProcName, paramList)

				businessObjectTable = GetAttributeIdentifiersFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Dim businessObject As AttributeIdentifier = Nothing
            If businessObjectTable.Count > 0 Then
                businessObject = CType(businessObjectTable.Item(0), AttributeIdentifier)
            End If
			
			Return businessObject

		End Function


		Public Overridable Sub InsertAttributeIdentifier(ByRef businessObject As AttributeIdentifier)

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim pkValueList As New ArrayList
            
			' setup parameters for stored proc

				' Screen_Text
				currentParam = New DBParam
				currentParam.Name = "Screen_Text"
				currentParam.Type = DBParamType.String
				If businessObject.IsScreenTextNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ScreenText
				End If

				paramList.Add(currentParam)

				' field_type
				currentParam = New DBParam
				currentParam.Name = "field_type"
				currentParam.Type = DBParamType.String
				If businessObject.IsFieldTypeNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.FieldType
				End If

				paramList.Add(currentParam)

				' combo_box
				currentParam = New DBParam
				currentParam.Name = "combo_box"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsComboBoxNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ComboBox
				End If

				paramList.Add(currentParam)

				' max_width
				currentParam = New DBParam
				currentParam.Name = "max_width"
				currentParam.Type = DBParamType.Int
				If businessObject.IsMaxWidthNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.MaxWidth
				End If

				paramList.Add(currentParam)

				' default_value
				currentParam = New DBParam
				currentParam.Name = "default_value"
				currentParam.Type = DBParamType.String
				If businessObject.IsDefaultValueNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DefaultValue
				End If

				paramList.Add(currentParam)

				' field_values
				currentParam = New DBParam
				currentParam.Name = "field_values"
				currentParam.Type = DBParamType.String
				If businessObject.IsFieldValuesNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.FieldValues
				End If

				paramList.Add(currentParam)

			' Get the output value
			currentParam = New DBParam
			currentParam.Name = "AttributeIdentifier_ID"
			currentParam.Type = DBParamType.Int
			paramList.Add(currentParam)

			' Execute Stored Procedure to Create Price Batch Detail records for the price change
			pkValueList = factory.ExecuteStoredProcedure("ItemAttributes_Regen_InsertAttributeIdentifier", paramList)
			
			' set the returned pk value
			businessObject.AttributeIdentifierID = CInt(pkValueList(0))

		End Sub
		
		Public Overridable Function UpdateAttributeIdentifier(ByVal businessObject As AttributeIdentifier) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' AttributeIdentifier_ID
				currentParam = New DBParam
				currentParam.Name = "AttributeIdentifier_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.AttributeIdentifierID

				paramList.Add(currentParam)

				' Screen_Text
				currentParam = New DBParam
				currentParam.Name = "Screen_Text"
				currentParam.Type = DBParamType.String
				If businessObject.IsScreenTextNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ScreenText
				End If

				paramList.Add(currentParam)

				' field_type
				currentParam = New DBParam
				currentParam.Name = "field_type"
				currentParam.Type = DBParamType.String
				If businessObject.IsFieldTypeNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.FieldType
				End If

				paramList.Add(currentParam)

				' combo_box
				currentParam = New DBParam
				currentParam.Name = "combo_box"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsComboBoxNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.ComboBox
				End If

				paramList.Add(currentParam)

				' max_width
				currentParam = New DBParam
				currentParam.Name = "max_width"
				currentParam.Type = DBParamType.Int
				If businessObject.IsMaxWidthNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.MaxWidth
				End If

				paramList.Add(currentParam)

				' default_value
				currentParam = New DBParam
				currentParam.Name = "default_value"
				currentParam.Type = DBParamType.String
				If businessObject.IsDefaultValueNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DefaultValue
				End If

				paramList.Add(currentParam)

				' field_values
				currentParam = New DBParam
				currentParam.Name = "field_values"
				currentParam.Type = DBParamType.String
				If businessObject.IsFieldValuesNull Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.FieldValues
				End If

				paramList.Add(currentParam)

			' Get the output value
			currentParam = New DBParam
			currentParam.Name = "UpdateCount"
			currentParam.Type = DBParamType.Int
			paramList.Add(currentParam)

			' Execute Stored Procedure to Create Price Batch Detail records for the price change
			outputList = factory.ExecuteStoredProcedure("ItemAttributes_Regen_UpdateAttributeIdentifier", paramList)

			Return CInt(outputList(0))

		End Function
		
		Public Overridable Function DeleteAttributeIdentifier(ByVal businessObject As AttributeIdentifier) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' AttributeIdentifier_ID
				currentParam = New DBParam
				currentParam.Name = "AttributeIdentifier_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.AttributeIdentifierID

				paramList.Add(currentParam)

			' Get the output value
			currentParam = New DBParam
			currentParam.Name = "DeleteCount"
			currentParam.Type = DBParamType.Int
			paramList.Add(currentParam)

			' Execute Stored Procedure to Create Price Batch Detail records for the price change
			outputList = factory.ExecuteStoredProcedure("ItemAttributes_Regen_DeleteAttributeIdentifier", paramList)

			Return CInt(outputList(0))

		End Function

    End Class

End Namespace
