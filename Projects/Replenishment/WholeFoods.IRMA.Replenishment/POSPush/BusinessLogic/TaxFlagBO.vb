Imports System.Data.SqlClient

Namespace WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
    Public Class TaxFlagBO
#Region "Property Definitions"
        Private _itemKey As Integer
        Private _taxFlagKey As String
        Private _taxFlagValue As Boolean
        Private _taxPercent As Decimal
        Private _posId As Integer

#End Region

#Region "Constructors"

        ''' <summary>
        ''' empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()
        End Sub

        ''' <summary>
        ''' constructor that initializes the object with data from a result set
        ''' </summary>
        ''' <param name="results"></param>
        ''' <remarks></remarks>
        Public Sub New(ByRef results As SqlDataReader)
            _itemKey = results.GetInt32(results.GetOrdinal("Item_Key"))
            _taxFlagKey = results.GetString(results.GetOrdinal("TaxFlagKey"))
            _taxFlagValue = results.GetBoolean(results.GetOrdinal("TaxFlagValue"))
            If (Not results.IsDBNull(results.GetOrdinal("TaxPercent"))) Then
                _taxPercent = results.GetDecimal(results.GetOrdinal("TaxPercent"))
            End If
            If (Not results.IsDBNull(results.GetOrdinal("POSID"))) Then
                _posId = results.GetInt32(results.GetOrdinal("POSID"))
            End If
        End Sub
#End Region

#Region "Property Access Methods"

        Public Property ItemKey() As Integer
            Get
                Return _itemKey
            End Get
            Set(ByVal value As Integer)
                _itemKey = value
            End Set
        End Property

        Public Property TaxFlagKey() As String
            Get
                Return _taxFlagKey
            End Get
            Set(ByVal value As String)
                _taxFlagKey = value
            End Set
        End Property

        Public Property TaxFlagValue() As Boolean
            Get
                Return _taxFlagValue
            End Get
            Set(ByVal value As Boolean)
                _taxFlagValue = value
            End Set
        End Property

        Public Property TaxPercent() As Decimal
            Get
                Return _taxPercent
            End Get
            Set(ByVal value As Decimal)
                _taxPercent = value
            End Set
        End Property

        Public Property POSId() As Integer
            Get
                Return _posId
            End Get
            Set(ByVal value As Integer)
                _posId = value
            End Set
        End Property

#End Region
    End Class
End Namespace

