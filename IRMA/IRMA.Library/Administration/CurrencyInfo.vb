Namespace Administration
    <Serializable()> _
    Public Class CurrencyInfo
        Inherits ReadOnlyBase(Of CurrencyInfo)

        Private mCurrencyID As Integer
        Private mCurrencyName As String

        Friend Sub New()
            mCurrencyID = -1
            mCurrencyName = ""
        End Sub

        Friend Sub New(ByVal DR As SafeDataReader)
            mCurrencyID = DR.GetInt32("Currency_ID")
            mCurrencyName = DR.GetString("Currency_Name")
        End Sub

        Protected Overrides Function GetIdValue() As Object
            Return mCurrencyID
        End Function

        Public ReadOnly Property CurrencyID() As Integer
            Get
                Return mCurrencyID
            End Get
        End Property

        Public ReadOnly Property CurrencyName() As String
            Get
                Return mCurrencyName
            End Get
        End Property
    End Class
End Namespace

