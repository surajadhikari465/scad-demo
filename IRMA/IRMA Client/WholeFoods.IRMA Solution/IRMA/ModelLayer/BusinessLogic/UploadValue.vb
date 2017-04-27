
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

    ''' <summary>
    ''' Business Object for the UploadValue db table.
    '''
    ''' This class inherits persistent properties from the regenerable base class.
    ''' These properties map one-to-one to columns in the UploadValue db table.
    '''
    ''' MODIFY THIS CLASS, NOT THE BASE CLASS.
    '''
    ''' Created By:	David Marine
    ''' Created   :	Feb 12, 2007
    ''' </summary>
    ''' <remarks></remarks>
    Public Class UploadValue
        Inherits UploadValueRegen


#Region "Overriden Fields and Properties"

        ''' <summary>
        ''' Overriden to default a bit (Boolean) datatype to False.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Property Value() As System.String
            Get
                Dim theValue As String = MyBase.Value

                ' if the data is bit (boolean) then
                ' default its value to false if it is null or empty
                If Me.DbDataType.ToLower().Equals("bit") Then
                    If IsNothing(theValue) Or String.IsNullOrEmpty(theValue) Then
                        theValue = "False"
                    ElseIf Not (theValue.ToLower().Equals("true") Or theValue.ToLower().Equals("false")) Then
                        theValue = "False"
                    End If
                End If

                Return theValue
            End Get
            Set(ByVal value As System.String)

                ' trim it unless the incoming value is all spaces
                If Trim(value).Length > 0 Then
                    value = Trim(value)
                    If (_dbDataType = "smalldatetime" Or _dbDataType = "datetime") _
                            AndAlso Not IsValidDate(value) Then
                        Try
                            '20100216 - Dave Stacey - remove hard-coded date mask string w/configured one
                            value = Format(Date.FromOADate(CDbl(value)), gsUG_DateMask)
                        Catch ex As Exception
                            'The value must not have been an Excel Date.  Return empty. 
                            value = ""
                        End Try
                    End If
                End If

                MyBase.Value = value

            End Set
        End Property

        ''' <summary>
        ''' Overriden to always have a bit (Boolean) datatype return non-null.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Property IsValueNull() As Boolean
            Get
                Dim isNull As Boolean = MyBase.IsValueNull

                If Me.DbDataType.ToLower().Equals("bit") Then
                    isNull = False
                End If

                Return isNull
            End Get
            Set(ByVal value As Boolean)
                MyBase.IsValueNull = value
            End Set
        End Property

#End Region

#Region "Fields and Properties from Associated UploadAttribute to Improve Performance"

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
        Private _isSizeNull As Boolean
        Private _isRequiredValue As System.Boolean
        Private _isIsRequiredValueNull As Boolean
        Private _isActive As System.Boolean
        Private _isIsActiveNull As Boolean
        Private _populateProcedure As System.String
        Private _isPopulateProcedureNull As Boolean = True
        Private _populateIndexField As System.String
        Private _isPopulateIndexFieldNull As Boolean = True
        Private _populateDescriptionField As System.String
        Private _isPopulateDescriptionFieldNull As Boolean = True
        Private _spreadsheetPosition As System.Int32
        Private _isSpreadsheetPositionNull As Boolean
        Private _optionalMinValue As System.String
        Private _isOptionalMinValueNull As Boolean = True
        Private _optionalMaxValue As System.String
        Private _isOptionalMaxValueNull As Boolean = True

        Public Overridable Property Name() As System.String
            Get
                Return _name
            End Get
            Set(ByVal value As System.String)

                ' set the IsDirty and is null flags
                Me.IsNameNull = IsNothing(_name)
                If (IsNothing(_name) And Not IsNothing(value)) Or _
                  (Not IsNothing(_name) And IsNothing(value)) Then
                    Me.IsDirty = True
                ElseIf IsNothing(_name) And IsNothing(value) Then
                    Me.IsDirty = False
                ElseIf Not _name.Equals(value) Then
                    Me.IsDirty = True
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

                ' set the IsDirty and is null flags
                Me.IsTableNameNull = IsNothing(_tableName)
                If (IsNothing(_tableName) And Not IsNothing(value)) Or _
                  (Not IsNothing(_tableName) And IsNothing(value)) Then
                    Me.IsDirty = True
                ElseIf IsNothing(_tableName) And IsNothing(value) Then
                    Me.IsDirty = False
                ElseIf Not _tableName.Equals(value) Then
                    Me.IsDirty = True
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

                ' set the IsDirty and is null flags
                Me.IsColumnNameOrKeyNull = IsNothing(_columnNameOrKey)
                If (IsNothing(_columnNameOrKey) And Not IsNothing(value)) Or _
                  (Not IsNothing(_columnNameOrKey) And IsNothing(value)) Then
                    Me.IsDirty = True
                ElseIf IsNothing(_columnNameOrKey) And IsNothing(value) Then
                    Me.IsDirty = False
                ElseIf Not _columnNameOrKey.Equals(value) Then
                    Me.IsDirty = True
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

                ' set the IsDirty and is null flags
                Me.IsControlTypeNull = IsNothing(_controlType)
                If (IsNothing(_controlType) And Not IsNothing(value)) Or _
                  (Not IsNothing(_controlType) And IsNothing(value)) Then
                    Me.IsDirty = True
                ElseIf IsNothing(_controlType) And IsNothing(value) Then
                    Me.IsDirty = False
                ElseIf Not _controlType.Equals(value) Then
                    Me.IsDirty = True
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

                ' set the IsDirty and is null flags
                Me.IsDbDataTypeNull = IsNothing(_dbDataType)
                If (IsNothing(_dbDataType) And Not IsNothing(value)) Or _
                  (Not IsNothing(_dbDataType) And IsNothing(value)) Then
                    Me.IsDirty = True
                ElseIf IsNothing(_dbDataType) And IsNothing(value) Then
                    Me.IsDirty = False
                ElseIf Not _dbDataType.Equals(value) Then
                    Me.IsDirty = True
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

                ' set the IsDirty and is null flags
                Me.IsSizeNull = False
                If _size <> value Then
                    Me.IsDirty = True
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

                ' set the IsDirty and is null flags
                _isRequiredValue = False
                If _isRequiredValue <> value Then
                    Me.IsDirty = True
                End If

                _isRequiredValue = value
            End Set
        End Property

        Public Overridable Property IsIsRequiredNull() As Boolean
            Get
                Return _isIsRequiredValueNull
            End Get
            Set(ByVal value As Boolean)
                _isIsRequiredValueNull = value
            End Set
        End Property

        Public Overridable Property IsActive() As System.Boolean
            Get
                Return _isActive
            End Get
            Set(ByVal value As System.Boolean)

                ' set the IsDirty and is null flags
                Me.IsIsActiveNull = False
                If _isActive <> value Then
                    Me.IsDirty = True
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

        Public Overridable Property PopulateProcedure() As System.String
            Get
                Return _populateProcedure
            End Get
            Set(ByVal value As System.String)

                ' set the IsDirty and is null flags
                Me.IsPopulateProcedureNull = IsNothing(_populateProcedure)
                If (IsNothing(_populateProcedure) And Not IsNothing(value)) Or _
                  (Not IsNothing(_populateProcedure) And IsNothing(value)) Then
                    Me.IsDirty = True
                ElseIf IsNothing(_populateProcedure) And IsNothing(value) Then
                    Me.IsDirty = False
                ElseIf Not _populateProcedure.Equals(value) Then
                    Me.IsDirty = True
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

                ' set the IsDirty and is null flags
                Me.IsPopulateIndexFieldNull = IsNothing(_populateIndexField)
                If (IsNothing(_populateIndexField) And Not IsNothing(value)) Or _
                  (Not IsNothing(_populateIndexField) And IsNothing(value)) Then
                    Me.IsDirty = True
                ElseIf IsNothing(_populateIndexField) And IsNothing(value) Then
                    Me.IsDirty = False
                ElseIf Not _populateIndexField.Equals(value) Then
                    Me.IsDirty = True
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

                ' set the IsDirty and is null flags
                Me.IsPopulateDescriptionFieldNull = IsNothing(_populateDescriptionField)
                If (IsNothing(_populateDescriptionField) And Not IsNothing(value)) Or _
                  (Not IsNothing(_populateDescriptionField) And IsNothing(value)) Then
                    Me.IsDirty = True
                ElseIf IsNothing(_populateDescriptionField) And IsNothing(value) Then
                    Me.IsDirty = False
                ElseIf Not _populateDescriptionField.Equals(value) Then
                    Me.IsDirty = True
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

                ' set the IsDirty and is null flags
                Me.IsSpreadsheetPositionNull = False
                If _spreadsheetPosition <> value Then
                    Me.IsDirty = True
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


        Public Overridable Property OptionalMinValue() As System.String
            Get
                Return _optionalMinValue
            End Get
            Set(ByVal value As System.String)

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

#End Region

#Region "New Fields and Properties"

        Private _key As System.String = Nothing

        ''' <summary>
        ''' Copied from the associated UploadAttribute along
        ''' with its properties to improve performance.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property Key() As String
            Get
                Dim theKey As String = Me._key

                If IsNothing(Me._key) Then
                    Me._key = String.Format("{0}_{1}", Me.TableName, Me.ColumnNameOrKey)
                End If

                Return Me._key
            End Get
        End Property

        ''' <summary>
        ''' Returns true if this UploadValue is
        ''' for one of the four hierarchy attributes.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property IsHierarchyValue() As Boolean
            Get
                Return Me.Key.Equals(EIM_Constants.ITEM_SUBTEAM_NO_ATTR_KEY) Or _
                        Me.Key.Equals(EIM_Constants.ITEM_CATEGORY_ID_ATTR_KEY) Or _
                        Me.Key.Equals(EIM_Constants.ITEM_LEVEL_3_ATTR_KEY) Or _
                        Me.Key.Equals(EIM_Constants.ITEM_LEVEL_4_ATTR_KEY)

            End Get
        End Property

#End Region

#Region "Constructors"

        Public Sub New()
            MyBase.New()
        End Sub

        ''' <summary>
        ''' Construct a UploadValue by passing in
        ''' its parent business objects.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByRef inUploadAttribute As UploadAttribute, ByRef inUploadRow As UploadRow)

            MyBase.New(inUploadAttribute, inUploadRow)

            ' copy the denormalized property values
            Me.Name = inUploadAttribute.Name
            Me.TableName = inUploadAttribute.TableName
            Me.ColumnNameOrKey = inUploadAttribute.ColumnNameOrKey
            Me.ControlType = inUploadAttribute.ControlType
            Me.DbDataType = inUploadAttribute.DbDataType
            Me.Size = inUploadAttribute.Size
            Me.SpreadsheetPosition = inUploadAttribute.SpreadsheetPosition
            Me.IsRequiredValue = inUploadAttribute.IsRequiredValue
            Me.OptionalMinValue = inUploadAttribute.OptionalMinValue
            Me.OptionalMaxValue = inUploadAttribute.OptionalMaxValue

        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Returns True if this UploadValue belongs to the provided
        ''' upload type code.
        ''' </summary>
        ''' <param name="uploadTypeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsForUpdateType(ByVal uploadTypeCode As String) As Boolean

            Dim _isForUpdateType As Boolean = False
            Dim theUploadAttribute As UploadAttribute = _
                Me.UploadRow.UploadSession.FindUploadAttributeByNameForSessionUploadTypes(Me.Name)

            If Not IsNothing(theUploadAttribute) Then
                For Each theUploadTypeAttribute As UploadTypeAttribute _
                    In theUploadAttribute.UploadTypeAttributeCollection()

                    If theUploadTypeAttribute. _
                        UploadTypeCode.Equals(uploadTypeCode) Then

                        _isForUpdateType = True
                        Exit For
                    End If
                Next
            End If

            Return _isForUpdateType

        End Function

        ''' <summary>
        ''' Returns True if this UploadValue ONLY belongs to the provided
        ''' upload type code.
        ''' </summary>
        ''' <param name="uploadTypeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsOnlyForUpdateType(ByVal uploadTypeCode As String) As Boolean

            Dim _isForUpdateType As Boolean = False

            Dim theUploadTypeAttributeCollection As BusinessObjectCollection = _
                Me.UploadRow.UploadSession.FindUploadAttributeByNameForSessionUploadTypes(Me.Name). _
                UploadTypeAttributeCollection()

            For Each theUploadTypeAttribute As UploadTypeAttribute In theUploadTypeAttributeCollection

                If theUploadTypeAttribute. _
                    UploadTypeCode.Equals(uploadTypeCode) Then

                    _isForUpdateType = True
                    Exit For
                End If
            Next

            Return _isForUpdateType And theUploadTypeAttributeCollection.Count = 1

        End Function

        ''' <summary>
        ''' Returns True if this UploadValue is read-only provided
        ''' upload type code.
        ''' </summary>
        ''' <param name="uploadTypeCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsReadOnlyForUpdateType(ByVal uploadTypeCode As String) As Boolean

            Dim _isReadOnlyForUpdateType As Boolean = False

            Dim theUploadAttribute As UploadAttribute = Me.UploadRow.UploadSession.FindUploadAttributeByNameForSessionUploadTypes(Me.Name)

            If Not IsNothing(theUploadAttribute) Then
                Dim theUploadTypeAttributeCollection As BusinessObjectCollection = _
                        theUploadAttribute. _
                        UploadTypeAttributeCollection()

                For Each theUploadTypeAttribute As UploadTypeAttribute In theUploadTypeAttributeCollection

                    If theUploadTypeAttribute. _
                        UploadTypeCode.Equals(uploadTypeCode) And Me.UploadRow.UploadSession.IsReadOnly(theUploadTypeAttribute) Then

                        _isReadOnlyForUpdateType = True
                        Exit For
                    End If
                Next
            End If

            Return _isReadOnlyForUpdateType

        End Function

        ''' <summary>
        ''' Translate the value, as needed, from the value that is stored to
        ''' what is displayed.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function TranslateUploadValueForDisplay() As String

            Dim theTranslatedValue As String = Me.Value

            If Me.Key.Equals(EIM_Constants.ITEM_CHAINS_ATTR_KEY) And Me.Value <> "-1" Then
                theTranslatedValue = ItemChainDAO.Instance.AddNamesToItemChainIdList(Me.Value)
            End If

            Return theTranslatedValue

        End Function

        ''' <summary>
        ''' Translate the value, as needed, from the value that is displayed to
        ''' what is stored.
        ''' </summary>
        ''' <remarks></remarks>
        Public Function TranslateUploadValueForPersistence() As String

            Dim theTranslatedValue As String = Me.Value
            Dim theValue As String = Me.Value

            If Me.Key.Equals(EIM_Constants.ITEM_CHAINS_ATTR_KEY) And Me.Value <> "-1" Then
                theTranslatedValue = ItemChainDAO.Instance.ExtractChainIdsFromIdAndNameList(theValue)
            End If

            Return theTranslatedValue

        End Function

        ''' <summary>
        ''' Translate the value, as needed, from the value that is stored to
        ''' what is exported.
        ''' </summary>
        ''' <remarks></remarks>
        Public Function TranslateUploadValueForExport() As String

            Dim theTranslatedValue As String = Me.Value
            Dim theValue As String = Me.Value

            If Me.Key.Equals(EIM_Constants.ITEM_CHAINS_ATTR_KEY) Then
                theTranslatedValue = ItemChainDAO.Instance.ExtractChainNamesFromIdAndNameList(theValue, True)
            End If

            Return theTranslatedValue

        End Function

        ''' <summary>
        ''' Translate the value, as needed, from the value that is stored to
        ''' what is exported.
        ''' </summary>
        ''' <remarks></remarks>
        Public Function TranslateUploadValueFromImport() As String

            Dim theTranslatedValue As String = Me.Value
            Dim theValue As String = Me.Value

            If Me.Key.Equals(EIM_Constants.ITEM_CHAINS_ATTR_KEY) Then

                If Not (IsNothing(theValue) Or String.IsNullOrEmpty(theValue)) Then

                    theTranslatedValue = ItemChainDAO.Instance.GetChainIdsFromNameList(theValue)
                End If
            End If

            If IsNothing(theTranslatedValue) Then
                theTranslatedValue = Me.UploadAttribute.DefaultValue
            End If

            Return theTranslatedValue

        End Function

#End Region

#Region "Private Methods"


#End Region

    End Class

End Namespace

