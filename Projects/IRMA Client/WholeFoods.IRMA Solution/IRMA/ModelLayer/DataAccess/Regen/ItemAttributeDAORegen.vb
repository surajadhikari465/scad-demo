

Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.ModelLayer.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.DataAccess

	''' <summary>
	''' Generated Data Access Object base class for the ItemAttribute db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	James Winfield
	''' Created   :	Feb 26, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class ItemAttributeDAORegen
    
		''' <summary>
		''' Returns a list of ItemAttributes from the provided resultset.
		''' </summary>
		''' <returns>ArrayList of ItemAttributes</returns>
		''' <remarks></remarks>
		Protected Overridable Function GetItemAttributesFromResultSet (ByVal results As SqlDataReader) As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim businessObject As ItemAttribute
			
			While results.Read

				businessObject = New ItemAttribute()

				businessObject.ItemAttributeID = results.GetInt32(results.GetOrdinal("ItemAttribute_ID"))                         

				' Item_Key can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Item_Key"))) Then
					' the businessObject.ItemKey property cannot be set to null
					' so set the businessObject.IsItemKeyNull flag to true.
					businessObject.IsItemKeyNull = True
				Else
					businessObject.ItemKey = results.GetInt32(results.GetOrdinal("Item_Key"))                         
				End If

				' Check_Box_1 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Check_Box_1"))) Then
					' the businessObject.CheckBox1 property cannot be set to null
					' so set the businessObject.IsCheckBox1Null flag to true.
					businessObject.IsCheckBox1Null = True
				Else
					businessObject.CheckBox1 = results.GetBoolean(results.GetOrdinal("Check_Box_1"))                         
				End If

				' Check_Box_2 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Check_Box_2"))) Then
					' the businessObject.CheckBox2 property cannot be set to null
					' so set the businessObject.IsCheckBox2Null flag to true.
					businessObject.IsCheckBox2Null = True
				Else
					businessObject.CheckBox2 = results.GetBoolean(results.GetOrdinal("Check_Box_2"))                         
				End If

				' Check_Box_3 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Check_Box_3"))) Then
					' the businessObject.CheckBox3 property cannot be set to null
					' so set the businessObject.IsCheckBox3Null flag to true.
					businessObject.IsCheckBox3Null = True
				Else
					businessObject.CheckBox3 = results.GetBoolean(results.GetOrdinal("Check_Box_3"))                         
				End If

				' Check_Box_4 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Check_Box_4"))) Then
					' the businessObject.CheckBox4 property cannot be set to null
					' so set the businessObject.IsCheckBox4Null flag to true.
					businessObject.IsCheckBox4Null = True
				Else
					businessObject.CheckBox4 = results.GetBoolean(results.GetOrdinal("Check_Box_4"))                         
				End If

				' Check_Box_5 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Check_Box_5"))) Then
					' the businessObject.CheckBox5 property cannot be set to null
					' so set the businessObject.IsCheckBox5Null flag to true.
					businessObject.IsCheckBox5Null = True
				Else
					businessObject.CheckBox5 = results.GetBoolean(results.GetOrdinal("Check_Box_5"))                         
				End If

				' Check_Box_6 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Check_Box_6"))) Then
					' the businessObject.CheckBox6 property cannot be set to null
					' so set the businessObject.IsCheckBox6Null flag to true.
					businessObject.IsCheckBox6Null = True
				Else
					businessObject.CheckBox6 = results.GetBoolean(results.GetOrdinal("Check_Box_6"))                         
				End If

				' Check_Box_7 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Check_Box_7"))) Then
					' the businessObject.CheckBox7 property cannot be set to null
					' so set the businessObject.IsCheckBox7Null flag to true.
					businessObject.IsCheckBox7Null = True
				Else
					businessObject.CheckBox7 = results.GetBoolean(results.GetOrdinal("Check_Box_7"))                         
				End If

				' Check_Box_8 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Check_Box_8"))) Then
					' the businessObject.CheckBox8 property cannot be set to null
					' so set the businessObject.IsCheckBox8Null flag to true.
					businessObject.IsCheckBox8Null = True
				Else
					businessObject.CheckBox8 = results.GetBoolean(results.GetOrdinal("Check_Box_8"))                         
				End If

				' Check_Box_9 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Check_Box_9"))) Then
					' the businessObject.CheckBox9 property cannot be set to null
					' so set the businessObject.IsCheckBox9Null flag to true.
					businessObject.IsCheckBox9Null = True
				Else
					businessObject.CheckBox9 = results.GetBoolean(results.GetOrdinal("Check_Box_9"))                         
				End If

				' Check_Box_10 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Check_Box_10"))) Then
					' the businessObject.CheckBox10 property cannot be set to null
					' so set the businessObject.IsCheckBox10Null flag to true.
					businessObject.IsCheckBox10Null = True
				Else
					businessObject.CheckBox10 = results.GetBoolean(results.GetOrdinal("Check_Box_10"))                         
				End If

				' Check_Box_11 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Check_Box_11"))) Then
					' the businessObject.CheckBox11 property cannot be set to null
					' so set the businessObject.IsCheckBox11Null flag to true.
					businessObject.IsCheckBox11Null = True
				Else
					businessObject.CheckBox11 = results.GetBoolean(results.GetOrdinal("Check_Box_11"))                         
				End If

				' Check_Box_12 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Check_Box_12"))) Then
					' the businessObject.CheckBox12 property cannot be set to null
					' so set the businessObject.IsCheckBox12Null flag to true.
					businessObject.IsCheckBox12Null = True
				Else
					businessObject.CheckBox12 = results.GetBoolean(results.GetOrdinal("Check_Box_12"))                         
				End If

				' Check_Box_13 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Check_Box_13"))) Then
					' the businessObject.CheckBox13 property cannot be set to null
					' so set the businessObject.IsCheckBox13Null flag to true.
					businessObject.IsCheckBox13Null = True
				Else
					businessObject.CheckBox13 = results.GetBoolean(results.GetOrdinal("Check_Box_13"))                         
				End If

				' Check_Box_14 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Check_Box_14"))) Then
					' the businessObject.CheckBox14 property cannot be set to null
					' so set the businessObject.IsCheckBox14Null flag to true.
					businessObject.IsCheckBox14Null = True
				Else
					businessObject.CheckBox14 = results.GetBoolean(results.GetOrdinal("Check_Box_14"))                         
				End If

				' Check_Box_15 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Check_Box_15"))) Then
					' the businessObject.CheckBox15 property cannot be set to null
					' so set the businessObject.IsCheckBox15Null flag to true.
					businessObject.IsCheckBox15Null = True
				Else
					businessObject.CheckBox15 = results.GetBoolean(results.GetOrdinal("Check_Box_15"))                         
				End If

				' Check_Box_16 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Check_Box_16"))) Then
					' the businessObject.CheckBox16 property cannot be set to null
					' so set the businessObject.IsCheckBox16Null flag to true.
					businessObject.IsCheckBox16Null = True
				Else
					businessObject.CheckBox16 = results.GetBoolean(results.GetOrdinal("Check_Box_16"))                         
				End If

				' Check_Box_17 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Check_Box_17"))) Then
					' the businessObject.CheckBox17 property cannot be set to null
					' so set the businessObject.IsCheckBox17Null flag to true.
					businessObject.IsCheckBox17Null = True
				Else
					businessObject.CheckBox17 = results.GetBoolean(results.GetOrdinal("Check_Box_17"))                         
				End If

				' Check_Box_18 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Check_Box_18"))) Then
					' the businessObject.CheckBox18 property cannot be set to null
					' so set the businessObject.IsCheckBox18Null flag to true.
					businessObject.IsCheckBox18Null = True
				Else
					businessObject.CheckBox18 = results.GetBoolean(results.GetOrdinal("Check_Box_18"))                         
				End If

				' Check_Box_19 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Check_Box_19"))) Then
					' the businessObject.CheckBox19 property cannot be set to null
					' so set the businessObject.IsCheckBox19Null flag to true.
					businessObject.IsCheckBox19Null = True
				Else
					businessObject.CheckBox19 = results.GetBoolean(results.GetOrdinal("Check_Box_19"))                         
				End If

				' Check_Box_20 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Check_Box_20"))) Then
					' the businessObject.CheckBox20 property cannot be set to null
					' so set the businessObject.IsCheckBox20Null flag to true.
					businessObject.IsCheckBox20Null = True
				Else
					businessObject.CheckBox20 = results.GetBoolean(results.GetOrdinal("Check_Box_20"))                         
				End If

				' Text_1 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Text_1"))) Then
					businessObject.Text1 = Nothing
				Else
					businessObject.Text1 = results.GetString(results.GetOrdinal("Text_1"))                         
				End If

				' Text_2 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Text_2"))) Then
					businessObject.Text2 = Nothing
				Else
					businessObject.Text2 = results.GetString(results.GetOrdinal("Text_2"))                         
				End If

				' Text_3 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Text_3"))) Then
					businessObject.Text3 = Nothing
				Else
					businessObject.Text3 = results.GetString(results.GetOrdinal("Text_3"))                         
				End If

				' Text_4 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Text_4"))) Then
					businessObject.Text4 = Nothing
				Else
					businessObject.Text4 = results.GetString(results.GetOrdinal("Text_4"))                         
				End If

				' Text_5 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Text_5"))) Then
					businessObject.Text5 = Nothing
				Else
					businessObject.Text5 = results.GetString(results.GetOrdinal("Text_5"))                         
				End If

				' Text_6 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Text_6"))) Then
					businessObject.Text6 = Nothing
				Else
					businessObject.Text6 = results.GetString(results.GetOrdinal("Text_6"))                         
				End If

				' Text_7 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Text_7"))) Then
					businessObject.Text7 = Nothing
				Else
					businessObject.Text7 = results.GetString(results.GetOrdinal("Text_7"))                         
				End If

				' Text_8 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Text_8"))) Then
					businessObject.Text8 = Nothing
				Else
					businessObject.Text8 = results.GetString(results.GetOrdinal("Text_8"))                         
				End If

				' Text_9 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Text_9"))) Then
					businessObject.Text9 = Nothing
				Else
					businessObject.Text9 = results.GetString(results.GetOrdinal("Text_9"))                         
				End If

				' Text_10 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Text_10"))) Then
					businessObject.Text10 = Nothing
				Else
					businessObject.Text10 = results.GetString(results.GetOrdinal("Text_10"))                         
				End If

				' Date_Time_1 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Date_Time_1"))) Then
					' the businessObject.DateTime1 property cannot be set to null
					' so set the businessObject.IsDateTime1Null flag to true.
					businessObject.IsDateTime1Null = True
				Else
					businessObject.DateTime1 = results.GetDateTime(results.GetOrdinal("Date_Time_1"))                         
				End If

				' Date_Time_2 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Date_Time_2"))) Then
					' the businessObject.DateTime2 property cannot be set to null
					' so set the businessObject.IsDateTime2Null flag to true.
					businessObject.IsDateTime2Null = True
				Else
					businessObject.DateTime2 = results.GetDateTime(results.GetOrdinal("Date_Time_2"))                         
				End If

				' Date_Time_3 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Date_Time_3"))) Then
					' the businessObject.DateTime3 property cannot be set to null
					' so set the businessObject.IsDateTime3Null flag to true.
					businessObject.IsDateTime3Null = True
				Else
					businessObject.DateTime3 = results.GetDateTime(results.GetOrdinal("Date_Time_3"))                         
				End If

				' Date_Time_4 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Date_Time_4"))) Then
					' the businessObject.DateTime4 property cannot be set to null
					' so set the businessObject.IsDateTime4Null flag to true.
					businessObject.IsDateTime4Null = True
				Else
					businessObject.DateTime4 = results.GetDateTime(results.GetOrdinal("Date_Time_4"))                         
				End If

				' Date_Time_5 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Date_Time_5"))) Then
					' the businessObject.DateTime5 property cannot be set to null
					' so set the businessObject.IsDateTime5Null flag to true.
					businessObject.IsDateTime5Null = True
				Else
					businessObject.DateTime5 = results.GetDateTime(results.GetOrdinal("Date_Time_5"))                         
				End If

				' Date_Time_6 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Date_Time_6"))) Then
					' the businessObject.DateTime6 property cannot be set to null
					' so set the businessObject.IsDateTime6Null flag to true.
					businessObject.IsDateTime6Null = True
				Else
					businessObject.DateTime6 = results.GetDateTime(results.GetOrdinal("Date_Time_6"))                         
				End If

				' Date_Time_7 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Date_Time_7"))) Then
					' the businessObject.DateTime7 property cannot be set to null
					' so set the businessObject.IsDateTime7Null flag to true.
					businessObject.IsDateTime7Null = True
				Else
					businessObject.DateTime7 = results.GetDateTime(results.GetOrdinal("Date_Time_7"))                         
				End If

				' Date_Time_8 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Date_Time_8"))) Then
					' the businessObject.DateTime8 property cannot be set to null
					' so set the businessObject.IsDateTime8Null flag to true.
					businessObject.IsDateTime8Null = True
				Else
					businessObject.DateTime8 = results.GetDateTime(results.GetOrdinal("Date_Time_8"))                         
				End If

				' Date_Time_9 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Date_Time_9"))) Then
					' the businessObject.DateTime9 property cannot be set to null
					' so set the businessObject.IsDateTime9Null flag to true.
					businessObject.IsDateTime9Null = True
				Else
					businessObject.DateTime9 = results.GetDateTime(results.GetOrdinal("Date_Time_9"))                         
				End If

				' Date_Time_10 can be null so set values appropriately
				If (results.IsDbNull(results.GetOrdinal("Date_Time_10"))) Then
					' the businessObject.DateTime10 property cannot be set to null
					' so set the businessObject.IsDateTime10Null flag to true.
					businessObject.IsDateTime10Null = True
				Else
					businessObject.DateTime10 = results.GetDateTime(results.GetOrdinal("Date_Time_10"))                         
				End If
				
				' reset all the flags
				businessObject.IsNew = False
				businessObject.IsDirty = False
				businessObject.IsMarkedForDelete = False
				businessObject.IsDeleted = False

				' add business object to BusinessObjectCollection with PK as key
				businessObjectTable.Add(businessObject.ItemAttributeID, businessObject)
				
			End While

			Trace.WriteLine("Loading " + businessObjectTable.Count.ToString() + " ItemAttributes from the database.")
			
			Return businessObjectTable

		End Function
		
		''' <summary>
		''' Set this in the ItemAttributeDAO sub class to change the stored
		''' procedure used by the GetAllItemAttributes function.
		''' </summary>
		Public GetAllItemAttributesStoredProcName As String = "ItemAttributes_Regen_GetAllItemAttributes"
		
		''' <summary>
		''' Returns all ItemAttributes.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetAllItemAttributes () As BusinessObjectCollection

			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim results As SqlDataReader = Nothing

			Try

				' Execute the stored procedure 
				results = factory.GetStoredProcedureDataReader(Me.GetAllItemAttributesStoredProcName, paramList)

				businessObjectTable = GetItemAttributesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Return businessObjectTable

		End Function


		''' <summary>
		''' Set this in the ItemAttributeDAO sub class to change the stored
		''' procedure used by the GetItemAttributeByPK function.
		''' </summary>
		Protected GetItemAttributeByPKStoredProcName As String = "ItemAttributes_Regen_GetItemAttributeByPK"
		
		''' <summary>
		''' Returns zero or more ItemAttributes by PK value.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Overridable Function GetItemAttributeByPK (ByRef itemAttributeID As System.Int32) As ItemAttribute


			Dim businessObjectTable As New BusinessObjectCollection
			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim results As SqlDataReader = Nothing

			Try
				' setup identifier for stored proc
				currentParam = New DBParam
				currentParam.Name = "ItemAttribute_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = itemAttributeID 
				paramList.Add(currentParam)

				' Execute the stored procedure 
				results = factory.GetStoredProcedureDataReader(Me.GetItemAttributeByPKStoredProcName, paramList)

				businessObjectTable = GetItemAttributesFromResultSet(results)

			Finally
				If results IsNot Nothing Then
				results.Close()
				End If
			End Try

			Dim businessObject As ItemAttribute = Nothing
            If businessObjectTable.Count > 0 Then
                businessObject = CType(businessObjectTable.Item(0), ItemAttribute)
            End If
			
			Return businessObject

		End Function


		Public Overridable Sub InsertItemAttribute(ByRef businessObject As ItemAttribute)

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

				' Check_Box_1
				currentParam = New DBParam
				currentParam.Name = "Check_Box_1"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox1Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox1
				End If

				paramList.Add(currentParam)

				' Check_Box_2
				currentParam = New DBParam
				currentParam.Name = "Check_Box_2"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox2Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox2
				End If

				paramList.Add(currentParam)

				' Check_Box_3
				currentParam = New DBParam
				currentParam.Name = "Check_Box_3"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox3Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox3
				End If

				paramList.Add(currentParam)

				' Check_Box_4
				currentParam = New DBParam
				currentParam.Name = "Check_Box_4"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox4Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox4
				End If

				paramList.Add(currentParam)

				' Check_Box_5
				currentParam = New DBParam
				currentParam.Name = "Check_Box_5"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox5Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox5
				End If

				paramList.Add(currentParam)

				' Check_Box_6
				currentParam = New DBParam
				currentParam.Name = "Check_Box_6"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox6Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox6
				End If

				paramList.Add(currentParam)

				' Check_Box_7
				currentParam = New DBParam
				currentParam.Name = "Check_Box_7"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox7Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox7
				End If

				paramList.Add(currentParam)

				' Check_Box_8
				currentParam = New DBParam
				currentParam.Name = "Check_Box_8"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox8Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox8
				End If

				paramList.Add(currentParam)

				' Check_Box_9
				currentParam = New DBParam
				currentParam.Name = "Check_Box_9"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox9Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox9
				End If

				paramList.Add(currentParam)

				' Check_Box_10
				currentParam = New DBParam
				currentParam.Name = "Check_Box_10"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox10Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox10
				End If

				paramList.Add(currentParam)

				' Check_Box_11
				currentParam = New DBParam
				currentParam.Name = "Check_Box_11"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox11Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox11
				End If

				paramList.Add(currentParam)

				' Check_Box_12
				currentParam = New DBParam
				currentParam.Name = "Check_Box_12"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox12Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox12
				End If

				paramList.Add(currentParam)

				' Check_Box_13
				currentParam = New DBParam
				currentParam.Name = "Check_Box_13"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox13Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox13
				End If

				paramList.Add(currentParam)

				' Check_Box_14
				currentParam = New DBParam
				currentParam.Name = "Check_Box_14"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox14Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox14
				End If

				paramList.Add(currentParam)

				' Check_Box_15
				currentParam = New DBParam
				currentParam.Name = "Check_Box_15"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox15Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox15
				End If

				paramList.Add(currentParam)

				' Check_Box_16
				currentParam = New DBParam
				currentParam.Name = "Check_Box_16"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox16Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox16
				End If

				paramList.Add(currentParam)

				' Check_Box_17
				currentParam = New DBParam
				currentParam.Name = "Check_Box_17"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox17Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox17
				End If

				paramList.Add(currentParam)

				' Check_Box_18
				currentParam = New DBParam
				currentParam.Name = "Check_Box_18"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox18Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox18
				End If

				paramList.Add(currentParam)

				' Check_Box_19
				currentParam = New DBParam
				currentParam.Name = "Check_Box_19"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox19Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox19
				End If

				paramList.Add(currentParam)

				' Check_Box_20
				currentParam = New DBParam
				currentParam.Name = "Check_Box_20"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox20Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox20
				End If

				paramList.Add(currentParam)

				' Text_1
				currentParam = New DBParam
				currentParam.Name = "Text_1"
				currentParam.Type = DBParamType.String
				If businessObject.IsText1Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Text1
				End If

				paramList.Add(currentParam)

				' Text_2
				currentParam = New DBParam
				currentParam.Name = "Text_2"
				currentParam.Type = DBParamType.String
				If businessObject.IsText2Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Text2
				End If

				paramList.Add(currentParam)

				' Text_3
				currentParam = New DBParam
				currentParam.Name = "Text_3"
				currentParam.Type = DBParamType.String
				If businessObject.IsText3Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Text3
				End If

				paramList.Add(currentParam)

				' Text_4
				currentParam = New DBParam
				currentParam.Name = "Text_4"
				currentParam.Type = DBParamType.String
				If businessObject.IsText4Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Text4
				End If

				paramList.Add(currentParam)

				' Text_5
				currentParam = New DBParam
				currentParam.Name = "Text_5"
				currentParam.Type = DBParamType.String
				If businessObject.IsText5Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Text5
				End If

				paramList.Add(currentParam)

				' Text_6
				currentParam = New DBParam
				currentParam.Name = "Text_6"
				currentParam.Type = DBParamType.String
				If businessObject.IsText6Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Text6
				End If

				paramList.Add(currentParam)

				' Text_7
				currentParam = New DBParam
				currentParam.Name = "Text_7"
				currentParam.Type = DBParamType.String
				If businessObject.IsText7Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Text7
				End If

				paramList.Add(currentParam)

				' Text_8
				currentParam = New DBParam
				currentParam.Name = "Text_8"
				currentParam.Type = DBParamType.String
				If businessObject.IsText8Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Text8
				End If

				paramList.Add(currentParam)

				' Text_9
				currentParam = New DBParam
				currentParam.Name = "Text_9"
				currentParam.Type = DBParamType.String
				If businessObject.IsText9Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Text9
				End If

				paramList.Add(currentParam)

				' Text_10
				currentParam = New DBParam
				currentParam.Name = "Text_10"
				currentParam.Type = DBParamType.String
				If businessObject.IsText10Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Text10
				End If

				paramList.Add(currentParam)

				' Date_Time_1
				currentParam = New DBParam
				currentParam.Name = "Date_Time_1"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsDateTime1Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DateTime1
				End If

				paramList.Add(currentParam)

				' Date_Time_2
				currentParam = New DBParam
				currentParam.Name = "Date_Time_2"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsDateTime2Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DateTime2
				End If

				paramList.Add(currentParam)

				' Date_Time_3
				currentParam = New DBParam
				currentParam.Name = "Date_Time_3"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsDateTime3Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DateTime3
				End If

				paramList.Add(currentParam)

				' Date_Time_4
				currentParam = New DBParam
				currentParam.Name = "Date_Time_4"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsDateTime4Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DateTime4
				End If

				paramList.Add(currentParam)

				' Date_Time_5
				currentParam = New DBParam
				currentParam.Name = "Date_Time_5"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsDateTime5Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DateTime5
				End If

				paramList.Add(currentParam)

				' Date_Time_6
				currentParam = New DBParam
				currentParam.Name = "Date_Time_6"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsDateTime6Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DateTime6
				End If

				paramList.Add(currentParam)

				' Date_Time_7
				currentParam = New DBParam
				currentParam.Name = "Date_Time_7"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsDateTime7Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DateTime7
				End If

				paramList.Add(currentParam)

				' Date_Time_8
				currentParam = New DBParam
				currentParam.Name = "Date_Time_8"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsDateTime8Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DateTime8
				End If

				paramList.Add(currentParam)

				' Date_Time_9
				currentParam = New DBParam
				currentParam.Name = "Date_Time_9"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsDateTime9Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DateTime9
				End If

				paramList.Add(currentParam)

				' Date_Time_10
				currentParam = New DBParam
				currentParam.Name = "Date_Time_10"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsDateTime10Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DateTime10
				End If

				paramList.Add(currentParam)

			' Get the output value
			currentParam = New DBParam
			currentParam.Name = "ItemAttribute_ID"
			currentParam.Type = DBParamType.Int
			paramList.Add(currentParam)

			' Execute Stored Procedure to Create Price Batch Detail records for the price change
			pkValueList = factory.ExecuteStoredProcedure("ItemAttributes_Regen_InsertItemAttribute", paramList)
			
			' set the returned pk value
			businessObject.ItemAttributeID = CInt(pkValueList(0))

		End Sub
		
		Public Overridable Function UpdateItemAttribute(ByVal businessObject As ItemAttribute) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' ItemAttribute_ID
				currentParam = New DBParam
				currentParam.Name = "ItemAttribute_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.ItemAttributeID

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

				' Check_Box_1
				currentParam = New DBParam
				currentParam.Name = "Check_Box_1"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox1Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox1
				End If

				paramList.Add(currentParam)

				' Check_Box_2
				currentParam = New DBParam
				currentParam.Name = "Check_Box_2"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox2Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox2
				End If

				paramList.Add(currentParam)

				' Check_Box_3
				currentParam = New DBParam
				currentParam.Name = "Check_Box_3"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox3Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox3
				End If

				paramList.Add(currentParam)

				' Check_Box_4
				currentParam = New DBParam
				currentParam.Name = "Check_Box_4"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox4Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox4
				End If

				paramList.Add(currentParam)

				' Check_Box_5
				currentParam = New DBParam
				currentParam.Name = "Check_Box_5"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox5Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox5
				End If

				paramList.Add(currentParam)

				' Check_Box_6
				currentParam = New DBParam
				currentParam.Name = "Check_Box_6"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox6Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox6
				End If

				paramList.Add(currentParam)

				' Check_Box_7
				currentParam = New DBParam
				currentParam.Name = "Check_Box_7"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox7Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox7
				End If

				paramList.Add(currentParam)

				' Check_Box_8
				currentParam = New DBParam
				currentParam.Name = "Check_Box_8"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox8Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox8
				End If

				paramList.Add(currentParam)

				' Check_Box_9
				currentParam = New DBParam
				currentParam.Name = "Check_Box_9"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox9Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox9
				End If

				paramList.Add(currentParam)

				' Check_Box_10
				currentParam = New DBParam
				currentParam.Name = "Check_Box_10"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox10Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox10
				End If

				paramList.Add(currentParam)

				' Check_Box_11
				currentParam = New DBParam
				currentParam.Name = "Check_Box_11"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox11Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox11
				End If

				paramList.Add(currentParam)

				' Check_Box_12
				currentParam = New DBParam
				currentParam.Name = "Check_Box_12"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox12Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox12
				End If

				paramList.Add(currentParam)

				' Check_Box_13
				currentParam = New DBParam
				currentParam.Name = "Check_Box_13"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox13Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox13
				End If

				paramList.Add(currentParam)

				' Check_Box_14
				currentParam = New DBParam
				currentParam.Name = "Check_Box_14"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox14Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox14
				End If

				paramList.Add(currentParam)

				' Check_Box_15
				currentParam = New DBParam
				currentParam.Name = "Check_Box_15"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox15Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox15
				End If

				paramList.Add(currentParam)

				' Check_Box_16
				currentParam = New DBParam
				currentParam.Name = "Check_Box_16"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox16Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox16
				End If

				paramList.Add(currentParam)

				' Check_Box_17
				currentParam = New DBParam
				currentParam.Name = "Check_Box_17"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox17Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox17
				End If

				paramList.Add(currentParam)

				' Check_Box_18
				currentParam = New DBParam
				currentParam.Name = "Check_Box_18"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox18Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox18
				End If

				paramList.Add(currentParam)

				' Check_Box_19
				currentParam = New DBParam
				currentParam.Name = "Check_Box_19"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox19Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox19
				End If

				paramList.Add(currentParam)

				' Check_Box_20
				currentParam = New DBParam
				currentParam.Name = "Check_Box_20"
				currentParam.Type = DBParamType.Bit
				If businessObject.IsCheckBox20Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.CheckBox20
				End If

				paramList.Add(currentParam)

				' Text_1
				currentParam = New DBParam
				currentParam.Name = "Text_1"
				currentParam.Type = DBParamType.String
				If businessObject.IsText1Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Text1
				End If

				paramList.Add(currentParam)

				' Text_2
				currentParam = New DBParam
				currentParam.Name = "Text_2"
				currentParam.Type = DBParamType.String
				If businessObject.IsText2Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Text2
				End If

				paramList.Add(currentParam)

				' Text_3
				currentParam = New DBParam
				currentParam.Name = "Text_3"
				currentParam.Type = DBParamType.String
				If businessObject.IsText3Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Text3
				End If

				paramList.Add(currentParam)

				' Text_4
				currentParam = New DBParam
				currentParam.Name = "Text_4"
				currentParam.Type = DBParamType.String
				If businessObject.IsText4Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Text4
				End If

				paramList.Add(currentParam)

				' Text_5
				currentParam = New DBParam
				currentParam.Name = "Text_5"
				currentParam.Type = DBParamType.String
				If businessObject.IsText5Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Text5
				End If

				paramList.Add(currentParam)

				' Text_6
				currentParam = New DBParam
				currentParam.Name = "Text_6"
				currentParam.Type = DBParamType.String
				If businessObject.IsText6Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Text6
				End If

				paramList.Add(currentParam)

				' Text_7
				currentParam = New DBParam
				currentParam.Name = "Text_7"
				currentParam.Type = DBParamType.String
				If businessObject.IsText7Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Text7
				End If

				paramList.Add(currentParam)

				' Text_8
				currentParam = New DBParam
				currentParam.Name = "Text_8"
				currentParam.Type = DBParamType.String
				If businessObject.IsText8Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Text8
				End If

				paramList.Add(currentParam)

				' Text_9
				currentParam = New DBParam
				currentParam.Name = "Text_9"
				currentParam.Type = DBParamType.String
				If businessObject.IsText9Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Text9
				End If

				paramList.Add(currentParam)

				' Text_10
				currentParam = New DBParam
				currentParam.Name = "Text_10"
				currentParam.Type = DBParamType.String
				If businessObject.IsText10Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.Text10
				End If

				paramList.Add(currentParam)

				' Date_Time_1
				currentParam = New DBParam
				currentParam.Name = "Date_Time_1"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsDateTime1Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DateTime1
				End If

				paramList.Add(currentParam)

				' Date_Time_2
				currentParam = New DBParam
				currentParam.Name = "Date_Time_2"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsDateTime2Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DateTime2
				End If

				paramList.Add(currentParam)

				' Date_Time_3
				currentParam = New DBParam
				currentParam.Name = "Date_Time_3"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsDateTime3Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DateTime3
				End If

				paramList.Add(currentParam)

				' Date_Time_4
				currentParam = New DBParam
				currentParam.Name = "Date_Time_4"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsDateTime4Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DateTime4
				End If

				paramList.Add(currentParam)

				' Date_Time_5
				currentParam = New DBParam
				currentParam.Name = "Date_Time_5"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsDateTime5Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DateTime5
				End If

				paramList.Add(currentParam)

				' Date_Time_6
				currentParam = New DBParam
				currentParam.Name = "Date_Time_6"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsDateTime6Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DateTime6
				End If

				paramList.Add(currentParam)

				' Date_Time_7
				currentParam = New DBParam
				currentParam.Name = "Date_Time_7"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsDateTime7Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DateTime7
				End If

				paramList.Add(currentParam)

				' Date_Time_8
				currentParam = New DBParam
				currentParam.Name = "Date_Time_8"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsDateTime8Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DateTime8
				End If

				paramList.Add(currentParam)

				' Date_Time_9
				currentParam = New DBParam
				currentParam.Name = "Date_Time_9"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsDateTime9Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DateTime9
				End If

				paramList.Add(currentParam)

				' Date_Time_10
				currentParam = New DBParam
				currentParam.Name = "Date_Time_10"
				currentParam.Type = DBParamType.DateTime
				If businessObject.IsDateTime10Null Then
					currentParam.Value = DBNull.Value
				Else
					currentParam.Value = businessObject.DateTime10
				End If

				paramList.Add(currentParam)

			' Get the output value
			currentParam = New DBParam
			currentParam.Name = "UpdateCount"
			currentParam.Type = DBParamType.Int
			paramList.Add(currentParam)

			' Execute Stored Procedure to Create Price Batch Detail records for the price change
			outputList = factory.ExecuteStoredProcedure("ItemAttributes_Regen_UpdateItemAttribute", paramList)

			Return CInt(outputList(0))

		End Function
		
		Public Overridable Function DeleteItemAttribute(ByVal businessObject As ItemAttribute) As Integer

			Dim factory As New DataFactory(DataFactory.ItemCatalog)
			Dim paramList As New ArrayList
			Dim currentParam As DBParam
			Dim outputList As New ArrayList

			' setup parameters for stored proc

				' ItemAttribute_ID
				currentParam = New DBParam
				currentParam.Name = "ItemAttribute_ID"
				currentParam.Type = DBParamType.Int
				currentParam.Value = businessObject.ItemAttributeID

				paramList.Add(currentParam)

			' Get the output value
			currentParam = New DBParam
			currentParam.Name = "DeleteCount"
			currentParam.Type = DBParamType.Int
			paramList.Add(currentParam)

			' Execute Stored Procedure to Create Price Batch Detail records for the price change
			outputList = factory.ExecuteStoredProcedure("ItemAttributes_Regen_DeleteItemAttribute", paramList)

			Return CInt(outputList(0))

		End Function

    End Class

End Namespace
