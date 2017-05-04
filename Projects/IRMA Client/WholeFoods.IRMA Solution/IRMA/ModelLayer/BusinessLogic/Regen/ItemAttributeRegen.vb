
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.Common
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

	''' <summary>
	''' Generated Business object base class for the ItemAttribute db table.
	'''
	''' THIS FILE WAS CODE GENERATED - DO NOT MODIFY!
	'''
	''' Created By:	James Winfield
	''' Created   :	Feb 26, 2007
	''' </summary>
	''' <remarks></remarks>
	Public Class ItemAttributeRegen
		Inherits BusinessObjectBase
	
#Region "Persistent Fields and Properties"

		Private _itemAttributeID As System.Int32
		Private _isItemAttributeIDNull As Boolean
		Private _itemKey As System.Int32
		Private _isItemKeyNull As Boolean
        Private _checkBox1 As System.Boolean = True
		Private _isCheckBox1Null As Boolean
        Private _checkBox2 As System.Boolean = True
		Private _isCheckBox2Null As Boolean
        Private _checkBox3 As System.Boolean = True
		Private _isCheckBox3Null As Boolean
        Private _checkBox4 As System.Boolean = True
		Private _isCheckBox4Null As Boolean
        Private _checkBox5 As System.Boolean = True
		Private _isCheckBox5Null As Boolean
        Private _checkBox6 As System.Boolean = True
		Private _isCheckBox6Null As Boolean
        Private _checkBox7 As System.Boolean = True
		Private _isCheckBox7Null As Boolean
        Private _checkBox8 As System.Boolean = True
		Private _isCheckBox8Null As Boolean
        Private _checkBox9 As System.Boolean = True
		Private _isCheckBox9Null As Boolean
        Private _checkBox10 As System.Boolean = True
		Private _isCheckBox10Null As Boolean
        Private _checkBox11 As System.Boolean = True
		Private _isCheckBox11Null As Boolean
        Private _checkBox12 As System.Boolean = True
		Private _isCheckBox12Null As Boolean
        Private _checkBox13 As System.Boolean = True
		Private _isCheckBox13Null As Boolean
        Private _checkBox14 As System.Boolean = True
		Private _isCheckBox14Null As Boolean
        Private _checkBox15 As System.Boolean = True
		Private _isCheckBox15Null As Boolean
        Private _checkBox16 As System.Boolean = True
		Private _isCheckBox16Null As Boolean
        Private _checkBox17 As System.Boolean = True
		Private _isCheckBox17Null As Boolean
        Private _checkBox18 As System.Boolean = True
		Private _isCheckBox18Null As Boolean
        Private _checkBox19 As System.Boolean = True
		Private _isCheckBox19Null As Boolean
        Private _checkBox20 As System.Boolean = True
        Private _isCheckBox20Null As Boolean = True
		Private _text1 As System.String
		Private _isText1Null As Boolean = True
		Private _text2 As System.String
		Private _isText2Null As Boolean = True
		Private _text3 As System.String
		Private _isText3Null As Boolean = True
		Private _text4 As System.String
		Private _isText4Null As Boolean = True
		Private _text5 As System.String
		Private _isText5Null As Boolean = True
		Private _text6 As System.String
		Private _isText6Null As Boolean = True
		Private _text7 As System.String
		Private _isText7Null As Boolean = True
		Private _text8 As System.String
		Private _isText8Null As Boolean = True
		Private _text9 As System.String
		Private _isText9Null As Boolean = True
		Private _text10 As System.String
		Private _isText10Null As Boolean = True
		Private _dateTime1 As System.DateTime
        Private _isDateTime1Null As Boolean = True
		Private _dateTime2 As System.DateTime
        Private _isDateTime2Null As Boolean = True
		Private _dateTime3 As System.DateTime
        Private _isDateTime3Null As Boolean = True
		Private _dateTime4 As System.DateTime
        Private _isDateTime4Null As Boolean = True
		Private _dateTime5 As System.DateTime
        Private _isDateTime5Null As Boolean = True
		Private _dateTime6 As System.DateTime
        Private _isDateTime6Null As Boolean = True
		Private _dateTime7 As System.DateTime
        Private _isDateTime7Null As Boolean = True
		Private _dateTime8 As System.DateTime
        Private _isDateTime8Null As Boolean = True
		Private _dateTime9 As System.DateTime
        Private _isDateTime9Null As Boolean = True
		Private _dateTime10 As System.DateTime
        Private _isDateTime10Null As Boolean = True

		Public Overridable Property ItemAttributeID() As System.Int32
		    Get
				Return _itemAttributeID
		    End Get
		    Set(ByVal value As System.Int32)
			
				' set the IsDirty and is null flags
				Me.IsItemAttributeIDNull = False
				If _itemAttributeID <> value Then
					Me.IsDirty = True
				End If
								
				_itemAttributeID = value
		    End Set
		End Property

		Public Overridable Property IsItemAttributeIDNull() As Boolean
		    Get
				Return _isItemAttributeIDNull
		    End Get
		    Set(ByVal value As Boolean)
				_isItemAttributeIDNull = value
		    End Set
		End Property

		Public Overridable Property ItemKey() As System.Int32
		    Get
				Return _itemKey
		    End Get
		    Set(ByVal value As System.Int32)
			
				' set the IsDirty and is null flags
				Me.IsItemKeyNull = False
				If _itemKey <> value Then
					Me.IsDirty = True
				End If
								
				_itemKey = value
		    End Set
		End Property

		Public Overridable Property IsItemKeyNull() As Boolean
		    Get
				Return _isItemKeyNull
		    End Get
		    Set(ByVal value As Boolean)
				_isItemKeyNull = value
		    End Set
		End Property

		Public Overridable Property CheckBox1() As System.Boolean
		    Get
				Return _checkBox1
		    End Get
		    Set(ByVal value As System.Boolean)
			
				' set the IsDirty and is null flags
				Me.IsCheckBox1Null = False
				If _checkBox1 <> value Then
					Me.IsDirty = True
				End If
								
				_checkBox1 = value
		    End Set
		End Property

		Public Overridable Property IsCheckBox1Null() As Boolean
		    Get
				Return _isCheckBox1Null
		    End Get
		    Set(ByVal value As Boolean)
				_isCheckBox1Null = value
		    End Set
		End Property

		Public Overridable Property CheckBox2() As System.Boolean
		    Get
				Return _checkBox2
		    End Get
		    Set(ByVal value As System.Boolean)
			
				' set the IsDirty and is null flags
				Me.IsCheckBox2Null = False
				If _checkBox2 <> value Then
					Me.IsDirty = True
				End If
								
				_checkBox2 = value
		    End Set
		End Property

		Public Overridable Property IsCheckBox2Null() As Boolean
		    Get
				Return _isCheckBox2Null
		    End Get
		    Set(ByVal value As Boolean)
				_isCheckBox2Null = value
		    End Set
		End Property

		Public Overridable Property CheckBox3() As System.Boolean
		    Get
				Return _checkBox3
		    End Get
		    Set(ByVal value As System.Boolean)
			
				' set the IsDirty and is null flags
				Me.IsCheckBox3Null = False
				If _checkBox3 <> value Then
					Me.IsDirty = True
				End If
								
				_checkBox3 = value
		    End Set
		End Property

		Public Overridable Property IsCheckBox3Null() As Boolean
		    Get
				Return _isCheckBox3Null
		    End Get
		    Set(ByVal value As Boolean)
				_isCheckBox3Null = value
		    End Set
		End Property

		Public Overridable Property CheckBox4() As System.Boolean
		    Get
				Return _checkBox4
		    End Get
		    Set(ByVal value As System.Boolean)
			
				' set the IsDirty and is null flags
				Me.IsCheckBox4Null = False
				If _checkBox4 <> value Then
					Me.IsDirty = True
				End If
								
				_checkBox4 = value
		    End Set
		End Property

		Public Overridable Property IsCheckBox4Null() As Boolean
		    Get
				Return _isCheckBox4Null
		    End Get
		    Set(ByVal value As Boolean)
				_isCheckBox4Null = value
		    End Set
		End Property

		Public Overridable Property CheckBox5() As System.Boolean
		    Get
				Return _checkBox5
		    End Get
		    Set(ByVal value As System.Boolean)
			
				' set the IsDirty and is null flags
				Me.IsCheckBox5Null = False
				If _checkBox5 <> value Then
					Me.IsDirty = True
				End If
								
				_checkBox5 = value
		    End Set
		End Property

		Public Overridable Property IsCheckBox5Null() As Boolean
		    Get
				Return _isCheckBox5Null
		    End Get
		    Set(ByVal value As Boolean)
				_isCheckBox5Null = value
		    End Set
		End Property

		Public Overridable Property CheckBox6() As System.Boolean
		    Get
				Return _checkBox6
		    End Get
		    Set(ByVal value As System.Boolean)
			
				' set the IsDirty and is null flags
				Me.IsCheckBox6Null = False
				If _checkBox6 <> value Then
					Me.IsDirty = True
				End If
								
				_checkBox6 = value
		    End Set
		End Property

		Public Overridable Property IsCheckBox6Null() As Boolean
		    Get
				Return _isCheckBox6Null
		    End Get
		    Set(ByVal value As Boolean)
				_isCheckBox6Null = value
		    End Set
		End Property

		Public Overridable Property CheckBox7() As System.Boolean
		    Get
				Return _checkBox7
		    End Get
		    Set(ByVal value As System.Boolean)
			
				' set the IsDirty and is null flags
				Me.IsCheckBox7Null = False
				If _checkBox7 <> value Then
					Me.IsDirty = True
				End If
								
				_checkBox7 = value
		    End Set
		End Property

		Public Overridable Property IsCheckBox7Null() As Boolean
		    Get
				Return _isCheckBox7Null
		    End Get
		    Set(ByVal value As Boolean)
				_isCheckBox7Null = value
		    End Set
		End Property

		Public Overridable Property CheckBox8() As System.Boolean
		    Get
				Return _checkBox8
		    End Get
		    Set(ByVal value As System.Boolean)
			
				' set the IsDirty and is null flags
				Me.IsCheckBox8Null = False
				If _checkBox8 <> value Then
					Me.IsDirty = True
				End If
								
				_checkBox8 = value
		    End Set
		End Property

		Public Overridable Property IsCheckBox8Null() As Boolean
		    Get
				Return _isCheckBox8Null
		    End Get
		    Set(ByVal value As Boolean)
				_isCheckBox8Null = value
		    End Set
		End Property

		Public Overridable Property CheckBox9() As System.Boolean
		    Get
				Return _checkBox9
		    End Get
		    Set(ByVal value As System.Boolean)
			
				' set the IsDirty and is null flags
				Me.IsCheckBox9Null = False
				If _checkBox9 <> value Then
					Me.IsDirty = True
				End If
								
				_checkBox9 = value
		    End Set
		End Property

		Public Overridable Property IsCheckBox9Null() As Boolean
		    Get
				Return _isCheckBox9Null
		    End Get
		    Set(ByVal value As Boolean)
				_isCheckBox9Null = value
		    End Set
		End Property

		Public Overridable Property CheckBox10() As System.Boolean
		    Get
				Return _checkBox10
		    End Get
		    Set(ByVal value As System.Boolean)
			
				' set the IsDirty and is null flags
				Me.IsCheckBox10Null = False
				If _checkBox10 <> value Then
					Me.IsDirty = True
				End If
								
				_checkBox10 = value
		    End Set
		End Property

		Public Overridable Property IsCheckBox10Null() As Boolean
		    Get
				Return _isCheckBox10Null
		    End Get
		    Set(ByVal value As Boolean)
				_isCheckBox10Null = value
		    End Set
		End Property

		Public Overridable Property CheckBox11() As System.Boolean
		    Get
				Return _checkBox11
		    End Get
		    Set(ByVal value As System.Boolean)
			
				' set the IsDirty and is null flags
				Me.IsCheckBox11Null = False
				If _checkBox11 <> value Then
					Me.IsDirty = True
				End If
								
				_checkBox11 = value
		    End Set
		End Property

		Public Overridable Property IsCheckBox11Null() As Boolean
		    Get
				Return _isCheckBox11Null
		    End Get
		    Set(ByVal value As Boolean)
				_isCheckBox11Null = value
		    End Set
		End Property

		Public Overridable Property CheckBox12() As System.Boolean
		    Get
				Return _checkBox12
		    End Get
		    Set(ByVal value As System.Boolean)
			
				' set the IsDirty and is null flags
				Me.IsCheckBox12Null = False
				If _checkBox12 <> value Then
					Me.IsDirty = True
				End If
								
				_checkBox12 = value
		    End Set
		End Property

		Public Overridable Property IsCheckBox12Null() As Boolean
		    Get
				Return _isCheckBox12Null
		    End Get
		    Set(ByVal value As Boolean)
				_isCheckBox12Null = value
		    End Set
		End Property

		Public Overridable Property CheckBox13() As System.Boolean
		    Get
				Return _checkBox13
		    End Get
		    Set(ByVal value As System.Boolean)
			
				' set the IsDirty and is null flags
				Me.IsCheckBox13Null = False
				If _checkBox13 <> value Then
					Me.IsDirty = True
				End If
								
				_checkBox13 = value
		    End Set
		End Property

		Public Overridable Property IsCheckBox13Null() As Boolean
		    Get
				Return _isCheckBox13Null
		    End Get
		    Set(ByVal value As Boolean)
				_isCheckBox13Null = value
		    End Set
		End Property

		Public Overridable Property CheckBox14() As System.Boolean
		    Get
				Return _checkBox14
		    End Get
		    Set(ByVal value As System.Boolean)
			
				' set the IsDirty and is null flags
				Me.IsCheckBox14Null = False
				If _checkBox14 <> value Then
					Me.IsDirty = True
				End If
								
				_checkBox14 = value
		    End Set
		End Property

		Public Overridable Property IsCheckBox14Null() As Boolean
		    Get
				Return _isCheckBox14Null
		    End Get
		    Set(ByVal value As Boolean)
				_isCheckBox14Null = value
		    End Set
		End Property

		Public Overridable Property CheckBox15() As System.Boolean
		    Get
				Return _checkBox15
		    End Get
		    Set(ByVal value As System.Boolean)
			
				' set the IsDirty and is null flags
				Me.IsCheckBox15Null = False
				If _checkBox15 <> value Then
					Me.IsDirty = True
				End If
								
				_checkBox15 = value
		    End Set
		End Property

		Public Overridable Property IsCheckBox15Null() As Boolean
		    Get
				Return _isCheckBox15Null
		    End Get
		    Set(ByVal value As Boolean)
				_isCheckBox15Null = value
		    End Set
		End Property

		Public Overridable Property CheckBox16() As System.Boolean
		    Get
				Return _checkBox16
		    End Get
		    Set(ByVal value As System.Boolean)
			
				' set the IsDirty and is null flags
				Me.IsCheckBox16Null = False
				If _checkBox16 <> value Then
					Me.IsDirty = True
				End If
								
				_checkBox16 = value
		    End Set
		End Property

		Public Overridable Property IsCheckBox16Null() As Boolean
		    Get
				Return _isCheckBox16Null
		    End Get
		    Set(ByVal value As Boolean)
				_isCheckBox16Null = value
		    End Set
		End Property

		Public Overridable Property CheckBox17() As System.Boolean
		    Get
				Return _checkBox17
		    End Get
		    Set(ByVal value As System.Boolean)
			
				' set the IsDirty and is null flags
				Me.IsCheckBox17Null = False
				If _checkBox17 <> value Then
					Me.IsDirty = True
				End If
								
				_checkBox17 = value
		    End Set
		End Property

		Public Overridable Property IsCheckBox17Null() As Boolean
		    Get
				Return _isCheckBox17Null
		    End Get
		    Set(ByVal value As Boolean)
				_isCheckBox17Null = value
		    End Set
		End Property

		Public Overridable Property CheckBox18() As System.Boolean
		    Get
				Return _checkBox18
		    End Get
		    Set(ByVal value As System.Boolean)
			
				' set the IsDirty and is null flags
				Me.IsCheckBox18Null = False
				If _checkBox18 <> value Then
					Me.IsDirty = True
				End If
								
				_checkBox18 = value
		    End Set
		End Property

		Public Overridable Property IsCheckBox18Null() As Boolean
		    Get
				Return _isCheckBox18Null
		    End Get
		    Set(ByVal value As Boolean)
				_isCheckBox18Null = value
		    End Set
		End Property

		Public Overridable Property CheckBox19() As System.Boolean
		    Get
				Return _checkBox19
		    End Get
		    Set(ByVal value As System.Boolean)
			
				' set the IsDirty and is null flags
				Me.IsCheckBox19Null = False
				If _checkBox19 <> value Then
					Me.IsDirty = True
				End If
								
				_checkBox19 = value
		    End Set
		End Property

		Public Overridable Property IsCheckBox19Null() As Boolean
		    Get
				Return _isCheckBox19Null
		    End Get
		    Set(ByVal value As Boolean)
				_isCheckBox19Null = value
		    End Set
		End Property

		Public Overridable Property CheckBox20() As System.Boolean
		    Get
				Return _checkBox20
		    End Get
		    Set(ByVal value As System.Boolean)
			
				' set the IsDirty and is null flags
				Me.IsCheckBox20Null = False
				If _checkBox20 <> value Then
					Me.IsDirty = True
				End If
								
				_checkBox20 = value
		    End Set
		End Property

		Public Overridable Property IsCheckBox20Null() As Boolean
		    Get
				Return _isCheckBox20Null
		    End Get
		    Set(ByVal value As Boolean)
				_isCheckBox20Null = value
		    End Set
		End Property

		Public Overridable Property Text1() As System.String
		    Get
				Return _text1
		    End Get
		    Set(ByVal value As System.String)
			
				' set the IsDirty and is null flags
				Me.IsText1Null = IsNothing(value)
				If (IsNothing(_text1) And Not IsNothing(value)) Or _
						(Not IsNothing(_text1) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_text1) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _text1.Equals(value) Then
					Me.IsDirty = True
				End If
								
				_text1 = value
		    End Set
		End Property

		Public Overridable Property IsText1Null() As Boolean
		    Get
				Return _isText1Null
		    End Get
		    Set(ByVal value As Boolean)
				_isText1Null = value
		    End Set
		End Property

		Public Overridable Property Text2() As System.String
		    Get
				Return _text2
		    End Get
		    Set(ByVal value As System.String)
			
				' set the IsDirty and is null flags
				Me.IsText2Null = IsNothing(value)
				If (IsNothing(_text2) And Not IsNothing(value)) Or _
						(Not IsNothing(_text2) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_text2) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _text2.Equals(value) Then
					Me.IsDirty = True
				End If
								
				_text2 = value
		    End Set
		End Property

		Public Overridable Property IsText2Null() As Boolean
		    Get
				Return _isText2Null
		    End Get
		    Set(ByVal value As Boolean)
				_isText2Null = value
		    End Set
		End Property

		Public Overridable Property Text3() As System.String
		    Get
				Return _text3
		    End Get
		    Set(ByVal value As System.String)
			
				' set the IsDirty and is null flags
				Me.IsText3Null = IsNothing(value)
				If (IsNothing(_text3) And Not IsNothing(value)) Or _
						(Not IsNothing(_text3) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_text3) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _text3.Equals(value) Then
					Me.IsDirty = True
				End If
								
				_text3 = value
		    End Set
		End Property

		Public Overridable Property IsText3Null() As Boolean
		    Get
				Return _isText3Null
		    End Get
		    Set(ByVal value As Boolean)
				_isText3Null = value
		    End Set
		End Property

		Public Overridable Property Text4() As System.String
		    Get
				Return _text4
		    End Get
		    Set(ByVal value As System.String)
			
				' set the IsDirty and is null flags
				Me.IsText4Null = IsNothing(value)
				If (IsNothing(_text4) And Not IsNothing(value)) Or _
						(Not IsNothing(_text4) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_text4) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _text4.Equals(value) Then
					Me.IsDirty = True
				End If
								
				_text4 = value
		    End Set
		End Property

		Public Overridable Property IsText4Null() As Boolean
		    Get
				Return _isText4Null
		    End Get
		    Set(ByVal value As Boolean)
				_isText4Null = value
		    End Set
		End Property

		Public Overridable Property Text5() As System.String
		    Get
				Return _text5
		    End Get
		    Set(ByVal value As System.String)
			
				' set the IsDirty and is null flags
				Me.IsText5Null = IsNothing(value)
				If (IsNothing(_text5) And Not IsNothing(value)) Or _
						(Not IsNothing(_text5) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_text5) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _text5.Equals(value) Then
					Me.IsDirty = True
				End If
								
				_text5 = value
		    End Set
		End Property

		Public Overridable Property IsText5Null() As Boolean
		    Get
				Return _isText5Null
		    End Get
		    Set(ByVal value As Boolean)
				_isText5Null = value
		    End Set
		End Property

		Public Overridable Property Text6() As System.String
		    Get
				Return _text6
		    End Get
		    Set(ByVal value As System.String)
			
				' set the IsDirty and is null flags
				Me.IsText6Null = IsNothing(value)
				If (IsNothing(_text6) And Not IsNothing(value)) Or _
						(Not IsNothing(_text6) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_text6) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _text6.Equals(value) Then
					Me.IsDirty = True
				End If
								
				_text6 = value
		    End Set
		End Property

		Public Overridable Property IsText6Null() As Boolean
		    Get
				Return _isText6Null
		    End Get
		    Set(ByVal value As Boolean)
				_isText6Null = value
		    End Set
		End Property

		Public Overridable Property Text7() As System.String
		    Get
				Return _text7
		    End Get
		    Set(ByVal value As System.String)
			
				' set the IsDirty and is null flags
				Me.IsText7Null = IsNothing(value)
				If (IsNothing(_text7) And Not IsNothing(value)) Or _
						(Not IsNothing(_text7) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_text7) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _text7.Equals(value) Then
					Me.IsDirty = True
				End If
								
				_text7 = value
		    End Set
		End Property

		Public Overridable Property IsText7Null() As Boolean
		    Get
				Return _isText7Null
		    End Get
		    Set(ByVal value As Boolean)
				_isText7Null = value
		    End Set
		End Property

		Public Overridable Property Text8() As System.String
		    Get
				Return _text8
		    End Get
		    Set(ByVal value As System.String)
			
				' set the IsDirty and is null flags
				Me.IsText8Null = IsNothing(value)
				If (IsNothing(_text8) And Not IsNothing(value)) Or _
						(Not IsNothing(_text8) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_text8) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _text8.Equals(value) Then
					Me.IsDirty = True
				End If
								
				_text8 = value
		    End Set
		End Property

		Public Overridable Property IsText8Null() As Boolean
		    Get
				Return _isText8Null
		    End Get
		    Set(ByVal value As Boolean)
				_isText8Null = value
		    End Set
		End Property

		Public Overridable Property Text9() As System.String
		    Get
				Return _text9
		    End Get
		    Set(ByVal value As System.String)
			
				' set the IsDirty and is null flags
				Me.IsText9Null = IsNothing(value)
				If (IsNothing(_text9) And Not IsNothing(value)) Or _
						(Not IsNothing(_text9) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_text9) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _text9.Equals(value) Then
					Me.IsDirty = True
				End If
								
				_text9 = value
		    End Set
		End Property

		Public Overridable Property IsText9Null() As Boolean
		    Get
				Return _isText9Null
		    End Get
		    Set(ByVal value As Boolean)
				_isText9Null = value
		    End Set
		End Property

		Public Overridable Property Text10() As System.String
		    Get
				Return _text10
		    End Get
		    Set(ByVal value As System.String)
			
				' set the IsDirty and is null flags
				Me.IsText10Null = IsNothing(value)
				If (IsNothing(_text10) And Not IsNothing(value)) Or _
						(Not IsNothing(_text10) And IsNothing(value)) Then
					Me.IsDirty = True
				ElseIf IsNothing(_text10) And IsNothing(value) Then
					Me.IsDirty = False
				ElseIf Not _text10.Equals(value) Then
					Me.IsDirty = True
				End If
								
				_text10 = value
		    End Set
		End Property

		Public Overridable Property IsText10Null() As Boolean
		    Get
				Return _isText10Null
		    End Get
		    Set(ByVal value As Boolean)
				_isText10Null = value
		    End Set
		End Property

		Public Overridable Property DateTime1() As System.DateTime
		    Get
				Return _dateTime1
		    End Get
		    Set(ByVal value As System.DateTime)
			
				' set the IsDirty and is null flags
				Me.IsDateTime1Null = False
				If _dateTime1 <> value Then
					Me.IsDirty = True
				End If
								
				_dateTime1 = value
		    End Set
		End Property

		Public Overridable Property IsDateTime1Null() As Boolean
		    Get
				Return _isDateTime1Null
		    End Get
		    Set(ByVal value As Boolean)
				_isDateTime1Null = value
		    End Set
		End Property

		Public Overridable Property DateTime2() As System.DateTime
		    Get
				Return _dateTime2
		    End Get
		    Set(ByVal value As System.DateTime)
			
				' set the IsDirty and is null flags
				Me.IsDateTime2Null = False
				If _dateTime2 <> value Then
					Me.IsDirty = True
				End If
								
				_dateTime2 = value
		    End Set
		End Property

		Public Overridable Property IsDateTime2Null() As Boolean
		    Get
				Return _isDateTime2Null
		    End Get
		    Set(ByVal value As Boolean)
				_isDateTime2Null = value
		    End Set
		End Property

		Public Overridable Property DateTime3() As System.DateTime
		    Get
				Return _dateTime3
		    End Get
		    Set(ByVal value As System.DateTime)
			
				' set the IsDirty and is null flags
				Me.IsDateTime3Null = False
				If _dateTime3 <> value Then
					Me.IsDirty = True
				End If
								
				_dateTime3 = value
		    End Set
		End Property

		Public Overridable Property IsDateTime3Null() As Boolean
		    Get
				Return _isDateTime3Null
		    End Get
		    Set(ByVal value As Boolean)
				_isDateTime3Null = value
		    End Set
		End Property

		Public Overridable Property DateTime4() As System.DateTime
		    Get
				Return _dateTime4
		    End Get
		    Set(ByVal value As System.DateTime)
			
				' set the IsDirty and is null flags
				Me.IsDateTime4Null = False
				If _dateTime4 <> value Then
					Me.IsDirty = True
				End If
								
				_dateTime4 = value
		    End Set
		End Property

		Public Overridable Property IsDateTime4Null() As Boolean
		    Get
				Return _isDateTime4Null
		    End Get
		    Set(ByVal value As Boolean)
				_isDateTime4Null = value
		    End Set
		End Property

		Public Overridable Property DateTime5() As System.DateTime
		    Get
				Return _dateTime5
		    End Get
		    Set(ByVal value As System.DateTime)
			
				' set the IsDirty and is null flags
				Me.IsDateTime5Null = False
				If _dateTime5 <> value Then
					Me.IsDirty = True
				End If
								
				_dateTime5 = value
		    End Set
		End Property

		Public Overridable Property IsDateTime5Null() As Boolean
		    Get
				Return _isDateTime5Null
		    End Get
		    Set(ByVal value As Boolean)
				_isDateTime5Null = value
		    End Set
		End Property

		Public Overridable Property DateTime6() As System.DateTime
		    Get
				Return _dateTime6
		    End Get
		    Set(ByVal value As System.DateTime)
			
				' set the IsDirty and is null flags
				Me.IsDateTime6Null = False
				If _dateTime6 <> value Then
					Me.IsDirty = True
				End If
								
				_dateTime6 = value
		    End Set
		End Property

		Public Overridable Property IsDateTime6Null() As Boolean
		    Get
				Return _isDateTime6Null
		    End Get
		    Set(ByVal value As Boolean)
				_isDateTime6Null = value
		    End Set
		End Property

		Public Overridable Property DateTime7() As System.DateTime
		    Get
				Return _dateTime7
		    End Get
		    Set(ByVal value As System.DateTime)
			
				' set the IsDirty and is null flags
				Me.IsDateTime7Null = False
				If _dateTime7 <> value Then
					Me.IsDirty = True
				End If
								
				_dateTime7 = value
		    End Set
		End Property

		Public Overridable Property IsDateTime7Null() As Boolean
		    Get
				Return _isDateTime7Null
		    End Get
		    Set(ByVal value As Boolean)
				_isDateTime7Null = value
		    End Set
		End Property

		Public Overridable Property DateTime8() As System.DateTime
		    Get
				Return _dateTime8
		    End Get
		    Set(ByVal value As System.DateTime)
			
				' set the IsDirty and is null flags
				Me.IsDateTime8Null = False
				If _dateTime8 <> value Then
					Me.IsDirty = True
				End If
								
				_dateTime8 = value
		    End Set
		End Property

		Public Overridable Property IsDateTime8Null() As Boolean
		    Get
				Return _isDateTime8Null
		    End Get
		    Set(ByVal value As Boolean)
				_isDateTime8Null = value
		    End Set
		End Property

		Public Overridable Property DateTime9() As System.DateTime
		    Get
				Return _dateTime9
		    End Get
		    Set(ByVal value As System.DateTime)
			
				' set the IsDirty and is null flags
				Me.IsDateTime9Null = False
				If _dateTime9 <> value Then
					Me.IsDirty = True
				End If
								
				_dateTime9 = value
		    End Set
		End Property

		Public Overridable Property IsDateTime9Null() As Boolean
		    Get
				Return _isDateTime9Null
		    End Get
		    Set(ByVal value As Boolean)
				_isDateTime9Null = value
		    End Set
		End Property

		Public Overridable Property DateTime10() As System.DateTime
		    Get
				Return _dateTime10
		    End Get
		    Set(ByVal value As System.DateTime)
			
				' set the IsDirty and is null flags
				Me.IsDateTime10Null = False
				If _dateTime10 <> value Then
					Me.IsDirty = True
				End If
								
				_dateTime10 = value
		    End Set
		End Property

		Public Overridable Property IsDateTime10Null() As Boolean
		    Get
				Return _isDateTime10Null
		    End Get
		    Set(ByVal value As Boolean)
				_isDateTime10Null = value
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
			Me.ItemAttributeID = ItemAttribute.NextTemporaryId
		End Sub
		

#End Region

#Region "Just-in-time Instantiated Parents"

#End Region

#Region "Just-in-time Instantiated Children Collections"

#End Region

#Region "CRUD Methods"

       ''' <summary>
        ''' Save this ItemAttribute
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Save() As Boolean
		
			' first delete this or any child Business Object that
			' is marked for delete
			Me.Delete(True)
			
			If Me.IsDirty And Not Me.IsDeleted Then
				If Me.IsNew Then
					ItemAttributeDAO.Instance.InsertItemAttribute(CType(Me, ItemAttribute))
					Trace.WriteLine("Inserting a new ItemAttribute")
				Else
					ItemAttributeDAO.Instance.UpdateItemAttribute(CType(Me, ItemAttribute))
					Trace.WriteLine("Updating an existing ItemAttribute")
				End If
				
				Me.IsDirty = False
				Me.IsNew = False
			End If
			
		End Function

       ''' <summary>
        ''' Delete this ItemAttribute
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overridable Function Delete(ByVal onlyMarked As Boolean) As Boolean
 
            If Not onlyMarked Or (onlyMarked And Me.IsMarkedForDelete) Then
                If Not Me.IsNew Then
                    ItemAttributeDAO.Instance.DeleteItemAttribute(CType(Me, ItemAttribute))
                    Trace.WriteLine("Deleting a ItemAttribute.")
                Else
                    Trace.WriteLine("Removing a new unsaved ItemAttribute.")
                End If

                Me.IsDeleted = True
                Me.IsMarkedForDelete = False

            End If
			
		End Function

#End Region

	End Class

End Namespace

