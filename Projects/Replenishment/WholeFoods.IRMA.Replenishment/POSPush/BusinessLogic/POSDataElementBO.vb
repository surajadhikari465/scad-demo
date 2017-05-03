Imports WholeFoods.Utility
Imports WholeFoods.IRMA.Replenishment.POSPush.Controller
Imports System.Data.SqlClient

Namespace WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic

    Public Class POSDataElementBO

#Region "Property Definitions"
        Private _changeType As Integer
        Private _columnOrder As Integer
        Private _bitOrder As Integer
        Private _rowOrder As Integer
        Private _dataElement As String
        Private _fieldId As String
        Private _maxFieldWidth As Integer
        Private _truncLeft As Boolean
        Private _defaultValue As String
        Private _isTaxFlag As Boolean
        Private _isLiteral As Boolean
        Private _isPackedDecimal As Boolean
        Private _isBinaryInt As Boolean
        Private _isDecimalValue As Boolean
        Private _decimalPrecision As Integer
        Private _includeDecimal As Boolean
        Private _isBoolean As Boolean
        Private _booleanTrueChar As String
        Private _booleanFalseChar As String
        ' Applies when _fixedWidth is true: Flag set to true if data is left-padded
        Private _padLeft As Boolean
        ' Applies when _fixedWidth is true: Specifies the fill character to pad data
        Private _fillChar As Char
        Private _leadingChars As String
        Private _trailingChars As String
        ' Allows the field to be fixed width when the writer is not
        Private _fixedWidthField As Boolean
        Private _packLength As Integer
#End Region

#Region "Methods to populate the object with stored procedure result sets"

        ' PopulateFromPOSWriterFileConfig stored procedure results
        Public Sub PopulateFromPOSWriterFileConfig(ByRef results As SqlDataReader)
            ' Assign values to the properties
            If (Not results.IsDBNull(results.GetOrdinal("POSChangeTypeKey"))) Then
                _changeType = results.GetInt32(results.GetOrdinal("POSChangeTypeKey"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("ColumnOrder"))) Then
                _columnOrder = results.GetInt32(results.GetOrdinal("ColumnOrder"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("BitOrder"))) Then
                _bitOrder = CType(results.GetByte(results.GetOrdinal("BitOrder")), Integer)
            End If
            If (Not results.IsDBNull(results.GetOrdinal("RowOrder"))) Then
                _rowOrder = results.GetInt32(results.GetOrdinal("RowOrder"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("DataElement"))) Then
                _dataElement = results.GetString(results.GetOrdinal("DataElement"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("FieldID"))) Then
                _fieldId = results.GetString(results.GetOrdinal("FieldID"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("MaxFieldWidth"))) Then
                _maxFieldWidth = results.GetInt32(results.GetOrdinal("MaxFieldWidth"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("TruncLeft"))) Then
                _truncLeft = results.GetBoolean(results.GetOrdinal("TruncLeft"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("DefaultValue"))) Then
                _defaultValue = results.GetString(results.GetOrdinal("DefaultValue"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("IsTaxFlag"))) Then
                _isTaxFlag = results.GetBoolean(results.GetOrdinal("IsTaxFlag"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("IsLiteral"))) Then
                _isLiteral = results.GetBoolean(results.GetOrdinal("IsLiteral"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("IsPackedDecimal"))) Then
                _isPackedDecimal = results.GetBoolean(results.GetOrdinal("IsPackedDecimal"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("IsBinaryInt"))) Then
                _isBinaryInt = results.GetBoolean(results.GetOrdinal("IsBinaryInt"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("IsDecimalValue"))) Then
                _isDecimalValue = results.GetBoolean(results.GetOrdinal("IsDecimalValue"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("DecimalPrecision"))) Then
                _decimalPrecision = results.GetInt32(results.GetOrdinal("DecimalPrecision"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("IncludeDecimal"))) Then
                _includeDecimal = results.GetBoolean(results.GetOrdinal("IncludeDecimal"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("PadLeft"))) Then
                _padLeft = results.GetBoolean(results.GetOrdinal("PadLeft"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("FillChar"))) Then
                _fillChar = CType(results.GetString(results.GetOrdinal("FillChar")), Char)
            End If
            If (Not results.IsDBNull(results.GetOrdinal("LeadingChars"))) Then
                _leadingChars = CType(results.GetString(results.GetOrdinal("LeadingChars")), String)
            End If
            If (Not results.IsDBNull(results.GetOrdinal("TrailingChars"))) Then
                _trailingChars = CType(results.GetString(results.GetOrdinal("TrailingChars")), String)
            End If
            If (Not results.IsDBNull(results.GetOrdinal("IsBoolean"))) Then
                _isBoolean = results.GetBoolean(results.GetOrdinal("IsBoolean"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("BooleanTrueChar"))) Then
                _booleanTrueChar = CType(results.GetString(results.GetOrdinal("BooleanTrueChar")), String)
            End If
            If (Not results.IsDBNull(results.GetOrdinal("BooleanFalseChar"))) Then
                _booleanFalseChar = CType(results.GetString(results.GetOrdinal("BooleanFalseChar")), String)
            End If
            If (Not results.IsDBNull(results.GetOrdinal("FixedWidthField"))) Then
                _fixedWidthField = results.GetBoolean(results.GetOrdinal("FixedWidthField"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("PackLength"))) Then
                _packLength = results.GetInt32(results.GetOrdinal("PackLength"))
            End If
        End Sub

#End Region

#Region "Property access methods"
        Public Property ChangeType() As Integer
            Get
                Return _changeType
            End Get
            Set(ByVal value As Integer)
                _changeType = value
            End Set
        End Property

        Public Property ColumnOrder() As Integer
            Get
                Return _columnOrder
            End Get
            Set(ByVal value As Integer)
                _columnOrder = value
            End Set
        End Property

        Public Property BitOrder() As Integer
            Get
                Return _bitOrder
            End Get
            Set(ByVal value As Integer)
                _bitOrder = value
            End Set
        End Property

        Public Property RowOrder() As Integer
            Get
                Return _rowOrder
            End Get
            Set(ByVal value As Integer)
                _rowOrder = value
            End Set
        End Property

        Public Property DataElement() As String
            Get
                Return _dataElement
            End Get
            Set(ByVal value As String)
                _dataElement = value
            End Set
        End Property

        Public Property FieldId() As String
            Get
                Return _fieldId
            End Get
            Set(ByVal value As String)
                _fieldId = value
            End Set
        End Property

        Public Property MaxFieldWidth() As Integer
            Get
                Return _maxFieldWidth
            End Get
            Set(ByVal value As Integer)
                _maxFieldWidth = value
            End Set
        End Property

        Public Property TruncLeft() As Boolean
            Get
                Return _truncLeft
            End Get
            Set(ByVal value As Boolean)
                _truncLeft = value
            End Set
        End Property

        Public Property DefaultValue() As String
            Get
                Return _defaultValue
            End Get
            Set(ByVal value As String)
                _defaultValue = value
            End Set
        End Property

        Public Property IsTaxFlag() As Boolean
            Get
                Return _isTaxFlag
            End Get
            Set(ByVal value As Boolean)
                _isTaxFlag = value
            End Set
        End Property

        Public Property IsLiteral() As Boolean
            Get
                Return _isLiteral
            End Get
            Set(ByVal value As Boolean)
                _isLiteral = value
            End Set
        End Property

        Public Property IsPackedDecimal() As Boolean
            Get
                Return _isPackedDecimal
            End Get
            Set(ByVal value As Boolean)
                _isPackedDecimal = value
            End Set
        End Property

        Public Property IsBinaryInt() As Boolean
            Get
                Return _isBinaryInt
            End Get
            Set(ByVal value As Boolean)
                _isBinaryInt = value
            End Set
        End Property

        Public Property IsDecimalValue() As Boolean
            Get
                Return _isDecimalValue
            End Get
            Set(ByVal value As Boolean)
                _isDecimalValue = value
            End Set
        End Property

        Public Property DecimalPrecision() As Integer
            Get
                Return _decimalPrecision
            End Get
            Set(ByVal value As Integer)
                _decimalPrecision = value
            End Set
        End Property

        Public Property IncludeDecimal() As Boolean
            Get
                Return _includeDecimal
            End Get
            Set(ByVal value As Boolean)
                _includeDecimal = value
            End Set
        End Property

        Public Property IsBoolean() As Boolean
            Get
                Return _isBoolean
            End Get
            Set(ByVal value As Boolean)
                _isBoolean = value
            End Set
        End Property

        Public Property BooleanTrueChar() As String
            Get
                Return _booleanTrueChar
            End Get
            Set(ByVal value As String)
                _booleanTrueChar = value
            End Set
        End Property

        Public Property BooleanFalseChar() As String
            Get
                Return _booleanFalseChar
            End Get
            Set(ByVal value As String)
                _booleanFalseChar = value
            End Set
        End Property

        Public Property PadLeft() As Boolean
            Get
                Return _padLeft
            End Get
            Set(ByVal value As Boolean)
                _padLeft = value
            End Set
        End Property

        Public Property FillChar() As Char
            Get
                Return _fillChar
            End Get
            Set(ByVal value As Char)
                _fillChar = value
            End Set
        End Property

        Public Property LeadingChars() As String
            Get
                Return _leadingChars
            End Get
            Set(ByVal value As String)
                _leadingChars = value
            End Set
        End Property

        Public Property TrailingChars() As String
            Get
                Return _trailingChars
            End Get
            Set(ByVal value As String)
                _trailingChars = value
            End Set
        End Property

        Public Property FixedWidthField() As Boolean
            Get
                Return _fixedWidthField
            End Get
            Set(ByVal value As Boolean)
                _fixedWidthField = value
            End Set
        End Property

        Public Property PackLength() As Integer
            Get
                Return _packLength
            End Get
            Set(ByVal value As Integer)
                _packLength = value
            End Set
        End Property
#End Region

    End Class

End Namespace

