
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

	''' <summary>
	''' Generated Business object base class for the AttributeIdentifier db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	James Winfield
	''' Created   :	Mar 01, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class AttributeIdentifierRegen
		Inherits BusinessObjectBase
	
#Region "Persistent Fields and Properties"

		Private _attributeIdentifierID As System.Int32
		Private _isAttributeIdentifierIDNull As Boolean
		Private _screenText As System.String
		Private _isScreenTextNull As Boolean = True
		Private _fieldType As System.String
		Private _isFieldTypeNull As Boolean = True
		Private _comboBox As System.Boolean
        Private _isComboBoxNull As Boolean
		Private _maxWidth As System.Int32
		Private _isMaxWidthNull As Boolean
		Private _defaultValue As System.String
		Private _isDefaultValueNull As Boolean = True
		Private _fieldValues As System.String
		Private _isFieldValuesNull As Boolean = True

		Public Overridable Property AttributeIdentifierID() As System.Int32
		    Get
				Return _attributeIdentifierID
		    End Get
		    Set(ByVal value As System.Int32)
			
				' set the IsDirty and is null flags
				Me.IsAttributeIdentifierIDNull = False
				If _attributeIdentifierID <> value Then
					Me.IsDirty = True
				End If
								
				_attributeIdentifierID = value
		    End Set
		End Property

		Public Overridable Property IsAttributeIdentifierIDNull() As Boolean
		    Get
				Return _isAttributeIdentifierIDNull
		    End Get
		    Set(ByVal value As Boolean)
				_isAttributeIdentifierIDNull = value
		    End Set
		End Property

		Public Overridable Property ScreenText() As System.String
		    Get
				Return _screenText
		    End Get
		    Set(ByVal value As System.String)
			
				' set the IsDirty and is null flags
				Me.IsScreenTextNull = IsNothing(value)
				If (IsNothing(_screenText) And Not IsNothing(value)) Or _
						(Not IsNothing(_screenText) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_screenText) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _screenText.Equals(value) Then
					Me.IsDirty = True
				End If
								
				_screenText = value
		    End Set
		End Property

		Public Overridable Property IsScreenTextNull() As Boolean
		    Get
				Return _isScreenTextNull
		    End Get
		    Set(ByVal value As Boolean)
				_isScreenTextNull = value
		    End Set
		End Property

		Public Overridable Property FieldType() As System.String
		    Get
				Return _fieldType
		    End Get
		    Set(ByVal value As System.String)
			
				' set the IsDirty and is null flags
				Me.IsFieldTypeNull = IsNothing(value)
				If (IsNothing(_fieldType) And Not IsNothing(value)) Or _
						(Not IsNothing(_fieldType) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_fieldType) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _fieldType.Equals(value) Then
					Me.IsDirty = True
				End If
								
				_fieldType = value
		    End Set
		End Property

		Public Overridable Property IsFieldTypeNull() As Boolean
		    Get
				Return _isFieldTypeNull
		    End Get
		    Set(ByVal value As Boolean)
				_isFieldTypeNull = value
		    End Set
		End Property

		Public Overridable Property ComboBox() As System.Boolean
		    Get
				Return _comboBox
		    End Get
		    Set(ByVal value As System.Boolean)
			
				' set the IsDirty and is null flags
				Me.IsComboBoxNull = False
				If _comboBox <> value Then
					Me.IsDirty = True
				End If
								
				_comboBox = value
		    End Set
		End Property

		Public Overridable Property IsComboBoxNull() As Boolean
		    Get
				Return _isComboBoxNull
		    End Get
		    Set(ByVal value As Boolean)
				_isComboBoxNull = value
		    End Set
		End Property

		Public Overridable Property MaxWidth() As System.Int32
		    Get
				Return _maxWidth
		    End Get
		    Set(ByVal value As System.Int32)
			
				' set the IsDirty and is null flags
				Me.IsMaxWidthNull = False
				If _maxWidth <> value Then
					Me.IsDirty = True
				End If
								
				_maxWidth = value
		    End Set
		End Property

		Public Overridable Property IsMaxWidthNull() As Boolean
		    Get
				Return _isMaxWidthNull
		    End Get
		    Set(ByVal value As Boolean)
				_isMaxWidthNull = value
		    End Set
		End Property

		Public Overridable Property DefaultValue() As System.String
		    Get
				Return _defaultValue
		    End Get
		    Set(ByVal value As System.String)
			
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

		Public Overridable Property FieldValues() As System.String
		    Get
				Return _fieldValues
		    End Get
		    Set(ByVal value As System.String)
			
				' set the IsDirty and is null flags
				Me.IsFieldValuesNull = IsNothing(value)
				If (IsNothing(_fieldValues) And Not IsNothing(value)) Or _
						(Not IsNothing(_fieldValues) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_fieldValues) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _fieldValues.Equals(value) Then
					Me.IsDirty = True
				End If
								
				_fieldValues = value
		    End Set
		End Property

		Public Overridable Property IsFieldValuesNull() As Boolean
		    Get
				Return _isFieldValuesNull
		    End Get
		    Set(ByVal value As Boolean)
				_isFieldValuesNull = value
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
			Me.AttributeIdentifierID = AttributeIdentifier.NextTemporaryId
		End Sub
		

#End Region

#Region "Just-in-time Instantiated Parents"

#End Region

#Region "Just-in-time Instantiated Children Collections"

#End Region

#Region "CRUD Methods"

       ''' <summary>
        ''' Save this AttributeIdentifier
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Save() As Boolean
		
			' first delete this or any child Business Object that
			' is marked for delete
			Me.Delete(True)
			
			If Me.IsDirty And Not Me.IsDeleted Then
				If Me.IsNew Then
					AttributeIdentifierDAO.Instance.InsertAttributeIdentifier(CType(Me, AttributeIdentifier))
					Trace.WriteLine("Inserting a new AttributeIdentifier")
				Else
					AttributeIdentifierDAO.Instance.UpdateAttributeIdentifier(CType(Me, AttributeIdentifier))
					Trace.WriteLine("Updating an existing AttributeIdentifier")
				End If
				
				Me.IsDirty = False
				Me.IsNew = False
			End If
			
		End Function

       ''' <summary>
        ''' Delete this AttributeIdentifier
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Delete(ByVal onlyMarked As Boolean) As Boolean
 
            If Not onlyMarked Or (onlyMarked And Me.IsMarkedForDelete) Then
                If Not Me.IsNew Then
                    AttributeIdentifierDAO.Instance.DeleteAttributeIdentifier(CType(Me, AttributeIdentifier))
                    Trace.WriteLine("Deleting a AttributeIdentifier.")
                Else
                    Trace.WriteLine("Removing a new unsaved AttributeIdentifier.")
                End If

                Me.IsDeleted = True
                Me.IsMarkedForDelete = False

            End If
			
		End Function

#End Region

	End Class

End Namespace

