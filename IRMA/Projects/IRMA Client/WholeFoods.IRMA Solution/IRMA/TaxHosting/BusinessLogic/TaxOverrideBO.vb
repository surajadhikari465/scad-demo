Public Enum TaxOverrideStatus
    Valid
    Error_Duplicate_TaxFlagKey
    Error_Required_TaxFlagKey
End Enum

Namespace WholeFoods.IRMA.TaxHosting.BusinessLogic
    Public Class TaxOverrideBO

#Region "Property Definitions"

        Private _itemKey As Integer
        Private _storeNo As Integer
        Private _storeName As String
        Private _taxFlagKey As String
        Private _taxFlagValue As Boolean

#End Region

#Region "Constructors"

        ''' <summary>
        ''' empty constructor
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub New()

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

        Public Property StoreNo() As Integer
            Get
                Return _storeNo
            End Get
            Set(ByVal value As Integer)
                _storeNo = value
            End Set
        End Property

        Public Property StoreName() As String
            Get
                Return _storeName
            End Get
            Set(ByVal value As String)
                _storeName = value
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

#End Region

        ''' <summary>
        ''' validates data elements of current instance of TaxOverrideBO object
        ''' </summary>
        ''' <returns>TaxOverrideStatus</returns>
        ''' <remarks></remarks>
        Public Function ValidateTaxFlagData(ByVal isEdit As Boolean, ByVal existingTaxFlags As Hashtable) As TaxOverrideStatus
            Dim status As TaxOverrideStatus

            'tax flag key required
            If Me.TaxFlagKey Is Nothing Or (Me.TaxFlagKey IsNot Nothing AndAlso Me.TaxFlagKey.Trim.Equals("")) Then
                status = TaxOverrideStatus.Error_Required_TaxFlagKey
            ElseIf Not isEdit AndAlso existingTaxFlags IsNot Nothing AndAlso existingTaxFlags.Contains(Me.TaxFlagKey) Then
                status = TaxOverrideStatus.Error_Duplicate_TaxFlagKey
            Else
                status = TaxOverrideStatus.Valid
            End If

            Return status
        End Function

    End Class
End Namespace
