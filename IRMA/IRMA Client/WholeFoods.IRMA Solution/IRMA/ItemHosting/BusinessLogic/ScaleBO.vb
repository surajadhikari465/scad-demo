Imports System.Text

Namespace WholeFoods.IRMA.ItemHosting.BusinessLogic

    Public Enum ScaleStatus
        Valid
        Error_Required_ScaleDesc1
    End Enum

    Public Class ScaleBO

        Private _itemKey As Integer
        Private _scaleDesc1 As String
        Private _scaleDesc2 As String
        Private _scaleDesc3 As String
        Private _scaleDesc4 As String
        Private _ingredients As String
        Private _shelfLifeLength As Integer
        Private _tare As Integer
        Private _useBy As Integer
        Private _forcedTare As Char

#Region "Property Access Methods"

        Public Property ItemKey() As Integer
            Get
                Return _itemKey
            End Get
            Set(ByVal value As Integer)
                _itemKey = value
            End Set
        End Property

        Public Property ScaleDesc1() As String
            Get
                Return _scaleDesc1
            End Get
            Set(ByVal value As String)
                _scaleDesc1 = value
            End Set
        End Property

        Public Property ScaleDesc2() As String
            Get
                Return _scaleDesc2
            End Get
            Set(ByVal value As String)
                _scaleDesc2 = value
            End Set
        End Property

        Public Property ScaleDesc3() As String
            Get
                Return _scaleDesc3
            End Get
            Set(ByVal value As String)
                _scaleDesc3 = value
            End Set
        End Property

        Public Property ScaleDesc4() As String
            Get
                Return _scaleDesc4
            End Get
            Set(ByVal value As String)
                _scaleDesc4 = value
            End Set
        End Property

        Public Property Ingredients() As String
            Get
                Return _ingredients
            End Get
            Set(ByVal value As String)
                _ingredients = value
            End Set
        End Property

        Public Property ShelfLifeLength() As Integer
            Get
                Return _shelfLifeLength
            End Get
            Set(ByVal value As Integer)
                _shelfLifeLength = value
            End Set
        End Property

        Public Property Tare() As Integer
            Get
                Return _tare
            End Get
            Set(ByVal value As Integer)
                _tare = value
            End Set
        End Property

        Public Property UseBy() As Integer
            Get
                Return _useBy
            End Get
            Set(ByVal value As Integer)
                _useBy = value
            End Set
        End Property

        Public Property ForcedTare() As Char
            Get
                Return _forcedTare
            End Get
            Set(ByVal value As Char)
                _forcedTare = value
            End Set
        End Property

#End Region

#Region "business rules"

        ' TODO: Move this to a common BO that can be shared among many screens
        Public Shared Function ValidateNumericValue(ByVal stringIn As String) As Boolean
            Dim valid As Boolean = False
            Try
                'if value present then validate that it's a number
                If stringIn IsNot Nothing AndAlso Not stringIn.Trim.Equals("") AndAlso IsNumeric(stringIn) Then
                    valid = True
                End If
            Catch ex As Exception
                valid = False
            End Try

            Return valid
        End Function

        ''' <summary>
        ''' checks that the passed in Integer is not a negative number.
        ''' if canBeZero is true, then zero values are allowed, otherwise they are not 
        ''' </summary>
        ''' <param name="checkInt"></param>
        ''' <param name="canBeZero"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ValidatePositiveNumber(ByVal checkInt As Integer, ByVal canBeZero As Boolean) As Boolean
            Dim success As Boolean = False

            If (canBeZero AndAlso checkInt >= 0) Or (canBeZero = False AndAlso checkInt > 0) Then
                success = True
            End If

            Return success
        End Function

        Public Function ValidateScaleData() As ArrayList
            Dim statusList As New ArrayList

            'ScaleDesc1 is required
            If Me.ScaleDesc1 Is Nothing Or (Me.ScaleDesc1 IsNot Nothing AndAlso Me.ScaleDesc1.Trim.Equals("")) Then
                statusList.Add(ScaleStatus.Error_Required_ScaleDesc1)
            End If

            'shelf life must be > 0
            If Me.ShelfLifeLength < 0 Then

            End If

            If statusList.Count <= 0 Then
                statusList.Add(ScaleStatus.Valid)
            End If

            Return statusList
        End Function

#End Region

    End Class

End Namespace