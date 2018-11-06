
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ModelLayer.DataAccess

Namespace WholeFoods.IRMA.ModelLayer.BusinessLogic

    ''' <summary>
    ''' Business Object for the UploadRow db table.
    '''
    ''' This class inherits persistent properties from the regenerable base class.
    ''' These properties map one-to-one to columns in the UploadRow db table.
    '''
    ''' MODIFY THIS CLASS, NOT THE BASE CLASS.
    '''
    ''' Created By:	David Marine
    ''' Created   :	Feb 12, 2007
    ''' </summary>
    ''' <remarks></remarks>
    Public Class UploadRow
        Inherits UploadRowRegen


#Region "Overriden Fields and Properties"


#End Region

#Region "New Fields and Properties"

        Private _hasValidatedIdentifier As Boolean = False
        Private _hasValidatedLinkedIdentifier As Boolean = False

        ''' <summary>
        ''' Returns false only in the beginning or if
        ''' the identifier value has changed.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property HasValidatedIdentifier() As Boolean
            Get
                Return Me._hasValidatedIdentifier
            End Get
            Set(ByVal value As Boolean)
                Me._hasValidatedIdentifier = value
            End Set
        End Property

        ''' <summary>
        ''' Returns false only in the beginning or if
        ''' the identifier value has changed.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property HasValidatedLinkedIdentifier() As Boolean
            Get
                Return Me._hasValidatedLinkedIdentifier
            End Get
            Set(ByVal value As Boolean)
                Me._hasValidatedLinkedIdentifier = value
            End Set
        End Property

        ''' <summary>
        ''' Returns a concatonated string of all data
        ''' necessary to persist the UploadRow's UploadValues.
        ''' This is used by the optimized function for saving UploadRows.
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property ConcatonatedValuesString() As String
            Get
                Dim theConcatonatedValuesString As String = ""

                Dim theValueIndex As Integer = 0
                For Each theUploadValue As UploadValue In Me.UploadValueCollection

                    If Not IsNothing(theUploadValue.Value) AndAlso _
                            (theUploadValue.Value.IndexOf("^") > -1 OrElse theUploadValue.Value.IndexOf("|") > -1) Then

                        ' we cannot use the optimized save with this UploadRow
                        ' as it one of its values contains at least one of the delimeters
                        ' we use to concatonate the values together
                        theConcatonatedValuesString = ""
                        Exit For
                    Else

                        theValueIndex = theValueIndex + 1

                        theConcatonatedValuesString = theConcatonatedValuesString + theUploadValue.UploadValueID.ToString() + "^"
                        theConcatonatedValuesString = theConcatonatedValuesString + theUploadValue.UploadAttributeID.ToString() + "^"

                        If IsNothing(theUploadValue.Value) Or String.IsNullOrEmpty(theUploadValue.Value) Then
                            theConcatonatedValuesString = theConcatonatedValuesString
                        Else
                            theConcatonatedValuesString = theConcatonatedValuesString + theUploadValue.Value
                        End If

                        ' can't have a final trailing delimeter
                        If theValueIndex < Me.UploadValueCollection.Count Then
                            theConcatonatedValuesString = theConcatonatedValuesString + "|"
                        End If
                    End If
                Next

                Return theConcatonatedValuesString

            End Get
        End Property

#End Region

#Region "Constructors"

        Public Sub New()
            MyBase.New()
            Me.ValidationLevel = EIM_Constants.ValidationLevels.Valid
        End Sub

        ''' <summary>
        ''' Construct a UploadRow by passing in
        ''' its parent business objects.
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New(ByRef inUploadSession As UploadSession)
            MyBase.New(inUploadSession)
            Me.ValidationLevel = EIM_Constants.ValidationLevels.Valid
        End Sub

#End Region

#Region "Public Methods"

        ''' <summary>
        ''' Returns the UploadValue of this UploadRow for the given attribute name.
        ''' </summary>
        ''' <param name="attributeKey"></param>
        ''' <returns>UploadValue</returns>
        ''' <remarks></remarks>
        Public Function FindValueByAttributeKey(ByVal attributeKey As String) As String

            Dim theValue As String = Nothing

            Dim theUploadValue As UploadValue = FindUploadValueByAttributeKey(attributeKey)

            If Not IsNothing(theUploadValue) Then
                theValue = theUploadValue.Value
            End If

            Return theValue

        End Function

        Private _uploadValueByAttributeKeyCache As New Hashtable()

        ''' <summary>
        ''' Returns the UploadValue of this UploadRow for the given attribute name.
        ''' </summary>
        ''' <param name="attributeKey"></param>
        ''' <returns>UploadValue</returns>
        ''' <remarks></remarks>
        Public Function FindUploadValueByAttributeKey(ByVal attributeKey As String) As UploadValue

            ' try to get it from the cache
            Dim theFoundUploadValue As UploadValue = CType(Me._uploadValueByAttributeKeyCache.Item(attributeKey), UploadValue)

            If IsNothing(theFoundUploadValue) Then

                ' not in the cache so look for it
                For Each theUploadValue As UploadValue In Me.UploadValueCollection

                    ' add the UploadValue to the cache if its not there
                    If Not Me._uploadValueByAttributeKeyCache.ContainsKey(theUploadValue.Key) Then
                        Me._uploadValueByAttributeKeyCache.Add(theUploadValue.Key, theUploadValue)
                    End If

                    If theUploadValue.Key.Equals(attributeKey) Then

                        ' got it so return it
                        theFoundUploadValue = theUploadValue
                        Exit For
                    End If
                Next
            End If

            Return theFoundUploadValue

        End Function

        Public Sub SetUploadValueByAttributeKey(ByVal attributeKey As String)

            ' try to get it from the cache
            Dim theFoundUploadValue As UploadValue = CType(Me._uploadValueByAttributeKeyCache.Item(attributeKey), UploadValue)

            If IsNothing(theFoundUploadValue) Then

                ' not in the cache so look for it
                For Each theUploadValue As UploadValue In Me.UploadValueCollection

                    ' add the UploadValue to the cache if its not there
                    If Not Me._uploadValueByAttributeKeyCache.ContainsKey(theUploadValue.Key) Then
                        Me._uploadValueByAttributeKeyCache.Add(theUploadValue.Key, theUploadValue)
                    End If

                    If theUploadValue.Key.Equals(attributeKey) Then

                        ' got it so return it
                        theUploadValue.Value = Me.isUploadExclusion
                        Exit For
                    End If
                Next
            End If
        End Sub

        '''<summary>
        ''' Returns the UploadTypeAttribute of this UploadRow for the given upload code and attribute key.
        ''' </summary>
        ''' <param name="attributeKey"></param>
        ''' <returns>UploadValue</returns>
        ''' <remarks></remarks>
        Public Function FindUploadTypeAttribute(ByVal inUploadTypeCode As String, _
                ByVal attributeKey As String) As UploadTypeAttribute

            For Each theUploadValue As UploadValue In Me.UploadValueCollection
                For Each theUploadTypeAttribute As UploadTypeAttribute In theUploadValue.UploadAttribute.UploadTypeAttributeCollection
                    If theUploadValue.Key.Equals(attributeKey) And _
                            theUploadTypeAttribute.UploadTypeCode.Equals(inUploadTypeCode) Then
                        Return theUploadTypeAttribute
                    End If
                Next
            Next
            Return Nothing

        End Function

#End Region

#Region "Private Methods"


#End Region

#Region "Overriden CRUD Methods"

        ''' <summary>
        ''' Save this UploadRow
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overrides Function Save() As Boolean

            Dim saveSucceeded As Boolean = False

            ' ConcatonatedValuesString would be empty if one of the row's values contains
            ' one of the delimeters (^ or |) used to concatonate the values together
            If Not String.IsNullOrEmpty(Me.ConcatonatedValuesString) And Me.ConcatonatedValuesString.Length <= 8000 Then

                ' only do the optimized update and insert if the ConcatonatedValuesString
                ' is less than 8,000 chars
                If Me.IsDirty And Not Me.IsDeleted Then
                    Try
                        'Throw New Exception("Test")
                        If Me.IsNew Then
                            UploadRowDAO.Instance.OptimizedInsertUploadRow(Me)
                            Trace.WriteLine("Inserting a new UploadRow")
                        Else
                            UploadRowDAO.Instance.OptimizedUpdateUploadRow(Me)
                            Trace.WriteLine("Updating an existing UploadRow")
                        End If

                        ' mark all the UploadValues as not new
                        For Each theUploadValue As UploadValue In Me.UploadValueCollection
                            theUploadValue.IsDirty = False
                            theUploadValue.IsNew = False
                        Next

                    Catch ex As DataFactoryException

                        ' log it
                        ' ErrorDialog.HandleError("An error occured during an optimized UploadRow save.", _
                        '  "This is a non-fatal and non-visible error for the user. The non-optimized save was used instead.", _
                        '  ex.ToString(), ErrorDialog.NotificationTypes.EmailOnly, "EIM_")

                        ' this is a bit of defensive programming to work around
                        ' a rare nasty failure in the parsing of the concatonated UploadValues string
                        ' the parsing occurs and fails in the stored proc and cannot be reproduced
                        ' directly after in the UI or by directly executing the stored proc with the data that failed

                        ' so, until this issue is resolved, if a row fails insertion, then let's save the row the
                        ' unoptimized way

                        saveSucceeded = MyBase.Save()
                    End Try

                    Me.IsDirty = False
                    Me.IsNew = False
                End If
            Else
                ' otherwise use the slower one UploadValue at a time
                ' unoptimized base save
                saveSucceeded = MyBase.Save()
            End If

            Return saveSucceeded

        End Function

#End Region

    End Class

End Namespace

