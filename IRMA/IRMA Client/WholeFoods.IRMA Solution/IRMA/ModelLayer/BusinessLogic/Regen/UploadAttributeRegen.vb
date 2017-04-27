
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ModelLayer
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

	''' <summary>
	''' Generated Business object base class for the UploadAttribute db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	David Marine
	''' Created   :	Jul 29, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class UploadAttributeRegen
		Inherits BusinessObjectBase
	
#Region "Persistent Fields and Properties"

		Private _uploadAttributeID As System.Int32
		Private _isUploadAttributeIDNull As Boolean = True
		Private _name As System.String
		Private _isNameNull As Boolean = True
		Private _tableName As System.String
		Private _isTableNameNull As Boolean = True
		Private _columnNameOrKey As System.String
		Private _isColumnNameOrKeyNull As Boolean = True
		Private _controlType As System.String
		Private _isControlTypeNull As Boolean = True
		Private _dbDataType As System.String
		Private _isDbDataTypeNull As Boolean = True
		Private _size As System.Int32
		Private _isSizeNull As Boolean = True
		Private _isRequiredValue As System.Boolean
		Private _isIsRequiredValueNull As Boolean = True
		Private _isCalculated As System.Boolean
		Private _isIsCalculatedNull As Boolean = True
		Private _optionalMinValue As System.String
		Private _isOptionalMinValueNull As Boolean = True
		Private _optionalMaxValue As System.String
		Private _isOptionalMaxValueNull As Boolean = True
		Private _isActive As System.Boolean
		Private _isIsActiveNull As Boolean = True
		Private _displayFormatString As System.String
		Private _isDisplayFormatStringNull As Boolean = True
		Private _populateProcedure As System.String
		Private _isPopulateProcedureNull As Boolean = True
		Private _populateIndexField As System.String
		Private _isPopulateIndexFieldNull As Boolean = True
		Private _populateDescriptionField As System.String
		Private _isPopulateDescriptionFieldNull As Boolean = True
		Private _spreadsheetPosition As System.Int32
		Private _isSpreadsheetPositionNull As Boolean = True
		Private _valueListStaticData As System.String
		Private _isValueListStaticDataNull As Boolean = True
		Private _defaultValue As System.String
		Private _isDefaultValueNull As Boolean = True

		Public Overridable Property UploadAttributeID() As System.Int32
		    Get
				Return _uploadAttributeID
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsUploadAttributeIDNull = False
				If _uploadAttributeID <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "UploadAttributeID", theOldValue, value)
				End If
				
				' set the abstracted key property
				' which is used to add BusinessObjects to BusinessObjectCollections
				Me.PrimaryKey = value
			
				_uploadAttributeID = value
		    End Set
		End Property

		Public Overridable Property IsUploadAttributeIDNull() As Boolean
		    Get
				Return _isUploadAttributeIDNull
		    End Get
		    Set(ByVal value As Boolean)
				_isUploadAttributeIDNull = value
		    End Set
		End Property

		Public Overridable Property Name() As System.String
		    Get
				Return _name
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsNameNull = IsNothing(value)
				If (IsNothing(_name) And Not IsNothing(value)) Or _
						(Not IsNothing(_name) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_name) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _name.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "Name", theOldValue, value)
				End If
				
			
				_name = value
		    End Set
		End Property

		Public Overridable Property IsNameNull() As Boolean
		    Get
				Return _isNameNull
		    End Get
		    Set(ByVal value As Boolean)
				_isNameNull = value
		    End Set
		End Property

		Public Overridable Property TableName() As System.String
		    Get
				Return _tableName
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsTableNameNull = IsNothing(value)
				If (IsNothing(_tableName) And Not IsNothing(value)) Or _
						(Not IsNothing(_tableName) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_tableName) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _tableName.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "TableName", theOldValue, value)
				End If
				
			
				_tableName = value
		    End Set
		End Property

		Public Overridable Property IsTableNameNull() As Boolean
		    Get
				Return _isTableNameNull
		    End Get
		    Set(ByVal value As Boolean)
				_isTableNameNull = value
		    End Set
		End Property

		Public Overridable Property ColumnNameOrKey() As System.String
		    Get
				Return _columnNameOrKey
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsColumnNameOrKeyNull = IsNothing(value)
				If (IsNothing(_columnNameOrKey) And Not IsNothing(value)) Or _
						(Not IsNothing(_columnNameOrKey) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_columnNameOrKey) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _columnNameOrKey.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "ColumnNameOrKey", theOldValue, value)
				End If
				
			
				_columnNameOrKey = value
		    End Set
		End Property

		Public Overridable Property IsColumnNameOrKeyNull() As Boolean
		    Get
				Return _isColumnNameOrKeyNull
		    End Get
		    Set(ByVal value As Boolean)
				_isColumnNameOrKeyNull = value
		    End Set
		End Property

		Public Overridable Property ControlType() As System.String
		    Get
				Return _controlType
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsControlTypeNull = IsNothing(value)
				If (IsNothing(_controlType) And Not IsNothing(value)) Or _
						(Not IsNothing(_controlType) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_controlType) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _controlType.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "ControlType", theOldValue, value)
				End If
				
			
				_controlType = value
		    End Set
		End Property

		Public Overridable Property IsControlTypeNull() As Boolean
		    Get
				Return _isControlTypeNull
		    End Get
		    Set(ByVal value As Boolean)
				_isControlTypeNull = value
		    End Set
		End Property

		Public Overridable Property DbDataType() As System.String
		    Get
				Return _dbDataType
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsDbDataTypeNull = IsNothing(value)
				If (IsNothing(_dbDataType) And Not IsNothing(value)) Or _
						(Not IsNothing(_dbDataType) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_dbDataType) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _dbDataType.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "DbDataType", theOldValue, value)
				End If
				
			
				_dbDataType = value
		    End Set
		End Property

		Public Overridable Property IsDbDataTypeNull() As Boolean
		    Get
				Return _isDbDataTypeNull
		    End Get
		    Set(ByVal value As Boolean)
				_isDbDataTypeNull = value
		    End Set
		End Property

		Public Overridable Property Size() As System.Int32
		    Get
				Return _size
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsSizeNull = False
				If _size <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "Size", theOldValue, value)
				End If
				
			
				_size = value
		    End Set
		End Property

		Public Overridable Property IsSizeNull() As Boolean
		    Get
				Return _isSizeNull
		    End Get
		    Set(ByVal value As Boolean)
				_isSizeNull = value
		    End Set
		End Property

		Public Overridable Property IsRequiredValue() As System.Boolean
		    Get
				Return _isRequiredValue
		    End Get
		    Set(ByVal value As System.Boolean)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsIsRequiredValueNull = False
				If _isRequiredValue <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "IsRequiredValue", theOldValue, value)
				End If
				
			
				_isRequiredValue = value
		    End Set
		End Property

		Public Overridable Property IsIsRequiredValueNull() As Boolean
		    Get
				Return _isIsRequiredValueNull
		    End Get
		    Set(ByVal value As Boolean)
				_isIsRequiredValueNull = value
		    End Set
		End Property

		Public Overridable Property IsCalculated() As System.Boolean
		    Get
				Return _isCalculated
		    End Get
		    Set(ByVal value As System.Boolean)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsIsCalculatedNull = False
				If _isCalculated <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "IsCalculated", theOldValue, value)
				End If
				
			
				_isCalculated = value
		    End Set
		End Property

		Public Overridable Property IsIsCalculatedNull() As Boolean
		    Get
				Return _isIsCalculatedNull
		    End Get
		    Set(ByVal value As Boolean)
				_isIsCalculatedNull = value
		    End Set
		End Property

		Public Overridable Property OptionalMinValue() As System.String
		    Get
				Return _optionalMinValue
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsOptionalMinValueNull = IsNothing(value)
				If (IsNothing(_optionalMinValue) And Not IsNothing(value)) Or _
						(Not IsNothing(_optionalMinValue) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_optionalMinValue) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _optionalMinValue.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "OptionalMinValue", theOldValue, value)
				End If
				
			
				_optionalMinValue = value
		    End Set
		End Property

		Public Overridable Property IsOptionalMinValueNull() As Boolean
		    Get
				Return _isOptionalMinValueNull
		    End Get
		    Set(ByVal value As Boolean)
				_isOptionalMinValueNull = value
		    End Set
		End Property

		Public Overridable Property OptionalMaxValue() As System.String
		    Get
				Return _optionalMaxValue
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsOptionalMaxValueNull = IsNothing(value)
				If (IsNothing(_optionalMaxValue) And Not IsNothing(value)) Or _
						(Not IsNothing(_optionalMaxValue) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_optionalMaxValue) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _optionalMaxValue.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "OptionalMaxValue", theOldValue, value)
				End If
				
			
				_optionalMaxValue = value
		    End Set
		End Property

		Public Overridable Property IsOptionalMaxValueNull() As Boolean
		    Get
				Return _isOptionalMaxValueNull
		    End Get
		    Set(ByVal value As Boolean)
				_isOptionalMaxValueNull = value
		    End Set
		End Property

		Public Overridable Property IsActive() As System.Boolean
		    Get
				Return _isActive
		    End Get
		    Set(ByVal value As System.Boolean)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsIsActiveNull = False
				If _isActive <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "IsActive", theOldValue, value)
				End If
				
			
				_isActive = value
		    End Set
		End Property

		Public Overridable Property IsIsActiveNull() As Boolean
		    Get
				Return _isIsActiveNull
		    End Get
		    Set(ByVal value As Boolean)
				_isIsActiveNull = value
		    End Set
		End Property

		Public Overridable Property DisplayFormatString() As System.String
		    Get
				Return _displayFormatString
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsDisplayFormatStringNull = IsNothing(value)
				If (IsNothing(_displayFormatString) And Not IsNothing(value)) Or _
						(Not IsNothing(_displayFormatString) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_displayFormatString) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _displayFormatString.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "DisplayFormatString", theOldValue, value)
				End If
				
			
				_displayFormatString = value
		    End Set
		End Property

		Public Overridable Property IsDisplayFormatStringNull() As Boolean
		    Get
				Return _isDisplayFormatStringNull
		    End Get
		    Set(ByVal value As Boolean)
				_isDisplayFormatStringNull = value
		    End Set
		End Property

		Public Overridable Property PopulateProcedure() As System.String
		    Get
				Return _populateProcedure
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsPopulateProcedureNull = IsNothing(value)
				If (IsNothing(_populateProcedure) And Not IsNothing(value)) Or _
						(Not IsNothing(_populateProcedure) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_populateProcedure) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _populateProcedure.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "PopulateProcedure", theOldValue, value)
				End If
				
			
				_populateProcedure = value
		    End Set
		End Property

		Public Overridable Property IsPopulateProcedureNull() As Boolean
		    Get
				Return _isPopulateProcedureNull
		    End Get
		    Set(ByVal value As Boolean)
				_isPopulateProcedureNull = value
		    End Set
		End Property

		Public Overridable Property PopulateIndexField() As System.String
		    Get
				Return _populateIndexField
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsPopulateIndexFieldNull = IsNothing(value)
				If (IsNothing(_populateIndexField) And Not IsNothing(value)) Or _
						(Not IsNothing(_populateIndexField) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_populateIndexField) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _populateIndexField.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "PopulateIndexField", theOldValue, value)
				End If
				
			
				_populateIndexField = value
		    End Set
		End Property

		Public Overridable Property IsPopulateIndexFieldNull() As Boolean
		    Get
				Return _isPopulateIndexFieldNull
		    End Get
		    Set(ByVal value As Boolean)
				_isPopulateIndexFieldNull = value
		    End Set
		End Property

		Public Overridable Property PopulateDescriptionField() As System.String
		    Get
				Return _populateDescriptionField
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsPopulateDescriptionFieldNull = IsNothing(value)
				If (IsNothing(_populateDescriptionField) And Not IsNothing(value)) Or _
						(Not IsNothing(_populateDescriptionField) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_populateDescriptionField) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _populateDescriptionField.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "PopulateDescriptionField", theOldValue, value)
				End If
				
			
				_populateDescriptionField = value
		    End Set
		End Property

		Public Overridable Property IsPopulateDescriptionFieldNull() As Boolean
		    Get
				Return _isPopulateDescriptionFieldNull
		    End Get
		    Set(ByVal value As Boolean)
				_isPopulateDescriptionFieldNull = value
		    End Set
		End Property

		Public Overridable Property SpreadsheetPosition() As System.Int32
		    Get
				Return _spreadsheetPosition
		    End Get
		    Set(ByVal value As System.Int32)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsSpreadsheetPositionNull = False
				If _spreadsheetPosition <> value Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "SpreadsheetPosition", theOldValue, value)
				End If
				
			
				_spreadsheetPosition = value
		    End Set
		End Property

		Public Overridable Property IsSpreadsheetPositionNull() As Boolean
		    Get
				Return _isSpreadsheetPositionNull
		    End Get
		    Set(ByVal value As Boolean)
				_isSpreadsheetPositionNull = value
		    End Set
		End Property

		Public Overridable Property ValueListStaticData() As System.String
		    Get
				Return _valueListStaticData
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsValueListStaticDataNull = IsNothing(value)
				If (IsNothing(_valueListStaticData) And Not IsNothing(value)) Or _
						(Not IsNothing(_valueListStaticData) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_valueListStaticData) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _valueListStaticData.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "ValueListStaticData", theOldValue, value)
				End If
				
			
				_valueListStaticData = value
		    End Set
		End Property

		Public Overridable Property IsValueListStaticDataNull() As Boolean
		    Get
				Return _isValueListStaticDataNull
		    End Get
		    Set(ByVal value As Boolean)
				_isValueListStaticDataNull = value
		    End Set
		End Property

		Public Overridable Property DefaultValue() As System.String
		    Get
				Return _defaultValue
		    End Get
		    Set(ByVal value As System.String)
			
				Dim theOldValue As Object = Nothing
			
				' set the IsDirty and is null flags
				Me.IsDefaultValueNull = IsNothing(value)
				If (IsNothing(_defaultValue) And Not IsNothing(value)) Or _
						(Not IsNothing(_defaultValue) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_defaultValue) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _defaultValue.Equals(value) Then
					Me.IsDirty = True
				End If
				
				If Me.IsDirty Then
					RaisePropertyChangedEvent(Me, "DefaultValue", theOldValue, value)
				End If
				
			
				_defaultValue = value
		    End Set
		End Property

		Public Overridable Property IsDefaultValueNull() As Boolean
		    Get
				Return _isDefaultValueNull
		    End Get
		    Set(ByVal value As Boolean)
				_isDefaultValueNull = value
		    End Set
		End Property

		
#End Region

#Region "Non-persistent Fields and Properties"

		Private Shared _nextTemporaryId As Integer = 0

		Public Shared Property NextTemporaryId() As Integer
		    Get
				_nextTemporaryId = _nextTemporaryId - 1
				Return _nextTemporaryId
		    End Get
		    Set(ByVal value As Integer)
				_nextTemporaryId = value
		    End Set
		End Property
		
#End Region

#Region "Constructors"

		Public Sub New()
			Me.UploadAttributeID = UploadAttribute.NextTemporaryId
		End Sub
		

#End Region

#Region "Just-in-time Instantiated Parents"

#End Region

#Region "Just-in-time Instantiated Children Collections"

		Private _uploadTypeAttributeCollection As BusinessObjectCollection = Nothing

		''' <summary>
        ''' Gets and sets this uploadAttribute's collection of UploadTypeAttributes.
        ''' </summary>
        ''' <value></value>
        ''' <returns>BusinessObjectCollection</returns>
        ''' <remarks></remarks>
		Public Overridable Property UploadTypeAttributeCollection() As BusinessObjectCollection
		    Get
		    	If IsNothing(_uploadTypeAttributeCollection) Then
					If Not Me.IsNew Then
						_uploadTypeAttributeCollection = _
							UploadTypeAttributeDAO.Instance.GetUploadTypeAttributesByUploadAttributeID(Me.UploadAttributeID)
		    		Else
						_uploadTypeAttributeCollection = New BusinessObjectCollection()
					End If
					
					' assign this Business Object as the children's parent
					For Each theUploadTypeAttribute As UploadTypeAttribute In _
							_uploadTypeAttributeCollection
						theUploadTypeAttribute.UploadAttribute = CType(Me, UploadAttribute)
					Next
									
				' subscribe to the PropertyChanged event that bubbles up from the BusinessObjects through the BusinessObjectCollection
				' this will set this BusinessObject's IsDirty flag when that of a child object changes
				AddHandler _uploadTypeAttributeCollection.PropertyChanged, AddressOf Me.PropertyChangedHandler
				
				End If
			Return _uploadTypeAttributeCollection
		    End Get
		    Set(ByVal value As BusinessObjectCollection)
				_uploadTypeAttributeCollection = value
		    End Set
		End Property
		
		''' <summary>
        ''' Gets the first UploadTypeAttribute in this UploadAttribute's
        ''' UploadTypeAttributeCollection, if there is one.
        ''' This is a convenience for when a one-to-many is really a one-to-one.
        ''' </summary>
        ''' <value></value>
        ''' <returns>uploadTypeAttribute</returns>
        ''' <remarks></remarks>
        Public Overridable ReadOnly Property UploadTypeAttribute() As UploadTypeAttribute
            Get
                Dim theUploadTypeAttribute As UploadTypeAttribute = Nothing

                If Me.UploadTypeAttributeCollection.Count > 0 Then
                    theUploadTypeAttribute = CType(Me.UploadTypeAttributeCollection.Item(0), UploadTypeAttribute)
                End If
                Return theUploadTypeAttribute
            End Get
        End Property
		
		''' <summary>
        ''' Add the provided UploadTypeAttribute to this UploadAttribute's UploadTypeAttributeCollection
        ''' </summary>
        ''' <param name="inUploadTypeAttribute"></param>
        ''' <remarks></remarks>
        Public Overridable Sub AddUploadTypeAttribute(ByRef inUploadTypeAttribute As UploadTypeAttribute)

            inUploadTypeAttribute.UploadAttribute = Ctype(Me, UploadAttribute)
            inUploadTypeAttribute.UploadAttributeID = Me.UploadAttributeID

            Me.UploadTypeAttributeCollection.Add(inUploadTypeAttribute.PrimaryKey, inUploadTypeAttribute)
			
        End Sub

		Private _uploadValueCollection As BusinessObjectCollection = Nothing

		''' <summary>
        ''' Gets and sets this uploadAttribute's collection of UploadValues.
        ''' </summary>
        ''' <value></value>
        ''' <returns>BusinessObjectCollection</returns>
        ''' <remarks></remarks>
		Public Overridable Property UploadValueCollection() As BusinessObjectCollection
		    Get
		    	If IsNothing(_uploadValueCollection) Then
					If Not Me.IsNew Then
						_uploadValueCollection = _
							UploadValueDAO.Instance.GetUploadValuesByUploadAttributeID(Me.UploadAttributeID)
		    		Else
						_uploadValueCollection = New BusinessObjectCollection()
					End If
					
					' assign this Business Object as the children's parent
					For Each theUploadValue As UploadValue In _
							_uploadValueCollection
						theUploadValue.UploadAttribute = CType(Me, UploadAttribute)
					Next
									
				' subscribe to the PropertyChanged event that bubbles up from the BusinessObjects through the BusinessObjectCollection
				' this will set this BusinessObject's IsDirty flag when that of a child object changes
				AddHandler _uploadValueCollection.PropertyChanged, AddressOf Me.PropertyChangedHandler
				
				End If
			Return _uploadValueCollection
		    End Get
		    Set(ByVal value As BusinessObjectCollection)
				_uploadValueCollection = value
		    End Set
		End Property
		
		''' <summary>
        ''' Gets the first UploadValue in this UploadAttribute's
        ''' UploadValueCollection, if there is one.
        ''' This is a convenience for when a one-to-many is really a one-to-one.
        ''' </summary>
        ''' <value></value>
        ''' <returns>uploadValue</returns>
        ''' <remarks></remarks>
        Public Overridable ReadOnly Property UploadValue() As UploadValue
            Get
                Dim theUploadValue As UploadValue = Nothing

                If Me.UploadValueCollection.Count > 0 Then
                    theUploadValue = CType(Me.UploadValueCollection.Item(0), UploadValue)
                End If
                Return theUploadValue
            End Get
        End Property
		
		''' <summary>
        ''' Add the provided UploadValue to this UploadAttribute's UploadValueCollection
        ''' </summary>
        ''' <param name="inUploadValue"></param>
        ''' <remarks></remarks>
        Public Overridable Sub AddUploadValue(ByRef inUploadValue As UploadValue)

            inUploadValue.UploadAttribute = Ctype(Me, UploadAttribute)
            inUploadValue.UploadAttributeID = Me.UploadAttributeID

            Me.UploadValueCollection.Add(inUploadValue.PrimaryKey, inUploadValue)
			
        End Sub

#End Region

#Region "CRUD Methods"

       ''' <summary>
        ''' Save this UploadAttribute
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Save() As Boolean
		
			If Me.IsDirty And Not Me.IsDeleted Then
				If Me.IsNew Then
					UploadAttributeDAO.Instance.InsertUploadAttribute(CType(Me, UploadAttribute))
					Trace.WriteLine("Inserting a new UploadAttribute")
				Else
					UploadAttributeDAO.Instance.UpdateUploadAttribute(CType(Me, UploadAttribute))
					Trace.WriteLine("Updating an existing UploadAttribute")
				End If
				
				Me.IsDirty = False
				Me.IsNew = False
			End If
			
			' save the UploadTypeAttributes
			' ProgressCounter and ProgressComplete are to support
            ' progress feedback for the user
            Me.UploadTypeAttributeCollection.SaveProgressCounter = 0
            For Each theUploadTypeAttribute As UploadTypeAttribute _
                    In Me.UploadTypeAttributeCollection
                theUploadTypeAttribute.UploadAttributeID = Me.UploadAttributeID
            	theUploadTypeAttribute.Save()
				Me.UploadTypeAttributeCollection.SaveProgressCounter = Me.UploadTypeAttributeCollection.SaveProgressCounter + 1
			Next		
			' save the UploadValues
			' ProgressCounter and ProgressComplete are to support
            ' progress feedback for the user
            Me.UploadValueCollection.SaveProgressCounter = 0
            For Each theUploadValue As UploadValue _
                    In Me.UploadValueCollection
                theUploadValue.UploadAttributeID = Me.UploadAttributeID
            	theUploadValue.Save()
				Me.UploadValueCollection.SaveProgressCounter = Me.UploadValueCollection.SaveProgressCounter + 1
			Next		
		
			Me.UploadTypeAttributeCollection.SaveProgressComplete = True
			Me.UploadValueCollection.SaveProgressComplete = True
		
		End Function

       ''' <summary>
        ''' Delete this UploadAttribute
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Delete(ByVal onlyMarked As Boolean) As Boolean
 
			Dim childCollectionIndex As Integer = 0
			
			Dim theChild As Object
			' delete the UploadTypeAttributes
			' ProgressCounter and ProgressComplete are to support
            ' progress feedback for the user
            Me.UploadTypeAttributeCollection.DeleteProgressCounter = 0
			For Each theUploadTypeAttribute As UploadTypeAttribute _
					In Me.UploadTypeAttributeCollection
				
				' increment the counter only if the UploadTypeAttribute is deleted
				If Not onlyMarked Or (onlyMarked And (Me.IsMarkedForDelete Or theUploadTypeAttribute.IsMarkedForDelete)) Then
					Me.UploadTypeAttributeCollection.DeleteProgressCounter = Me.UploadTypeAttributeCollection.DeleteProgressCounter + 1
				End If
				
				' always delete the children if the parent is deleted
				theUploadTypeAttribute.Delete(onlyMarked And Not Me.IsMarkedForDelete)
			Next
		
			' remove all deleted children from the UploadTypeAttributeCollection
			childCollectionIndex = 0
			Do
				If childCollectionIndex >= Me.UploadTypeAttributeCollection.Count Then
					Exit Do
				End If
				theChild = Me.UploadTypeAttributeCollection.Item(childCollectionIndex)
				If CType(theChild, UploadTypeAttribute).IsDeleted Then
					Me.UploadTypeAttributeCollection.Remove(theChild)
                Else
					childCollectionIndex = childCollectionIndex + 1
				End If
			Loop
			
			' delete the UploadValues
			' ProgressCounter and ProgressComplete are to support
            ' progress feedback for the user
            Me.UploadValueCollection.DeleteProgressCounter = 0
			For Each theUploadValue As UploadValue _
					In Me.UploadValueCollection
				
				' increment the counter only if the UploadValue is deleted
				If Not onlyMarked Or (onlyMarked And (Me.IsMarkedForDelete Or theUploadValue.IsMarkedForDelete)) Then
					Me.UploadValueCollection.DeleteProgressCounter = Me.UploadValueCollection.DeleteProgressCounter + 1
				End If
				
				' always delete the children if the parent is deleted
				theUploadValue.Delete(onlyMarked And Not Me.IsMarkedForDelete)
			Next
		
			' remove all deleted children from the UploadValueCollection
			childCollectionIndex = 0
			Do
				If childCollectionIndex >= Me.UploadValueCollection.Count Then
					Exit Do
				End If
				theChild = Me.UploadValueCollection.Item(childCollectionIndex)
				If CType(theChild, UploadValue).IsDeleted Then
					Me.UploadValueCollection.Remove(theChild)
                Else
					childCollectionIndex = childCollectionIndex + 1
				End If
			Loop
			

			If Not onlyMarked Or (onlyMarked And Me.IsMarkedForDelete) Then
				If Not Me.IsNew Then
					UploadAttributeDAO.Instance.DeleteUploadAttribute(CType(Me, UploadAttribute))
					Trace.WriteLine("Deleting a UploadAttribute.")
				Else
					Trace.WriteLine("Removing a new unsaved UploadAttribute.")
				End If
				
				Me.IsDeleted = True
				Me.IsMarkedForDelete = False

			End If
		
			Me.UploadTypeAttributeCollection.DeleteProgressComplete = True
			Me.UploadValueCollection.DeleteProgressComplete = True

		End Function

#End Region

#Region "Public Methods"

       ''' <summary>
        ''' Make this UploadAttribute  and all of its
		''' decedents new so it
		''' will be inserted into the database when it is saved.
        ''' </summary>
        ''' <remarks></remarks>
        Public Overridable Sub MakeNew()
			
			Me.UploadAttributeID = UploadAttribute.NextTemporaryId
			Me.IsDirty = True
			Me.IsNew = True
		
            For Each theUploadTypeAttribute As UploadTypeAttribute _
                    In Me.UploadTypeAttributeCollection
                theUploadTypeAttribute.MakeNew()
			Next
		
            For Each theUploadValue As UploadValue _
                    In Me.UploadValueCollection
                theUploadValue.MakeNew()
			Next
		
		End Sub

#End Region

#Region "Private Methods"

        Protected Overrides Function GetNewInstance() As BusinessObjectBase
            Return New UploadAttribute()
        End Function

#End Region

	End Class

End Namespace

