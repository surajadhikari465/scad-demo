Namespace Administration
    <Serializable()> _
    Public Class SalePriceChgTypeInfo
        Inherits ReadOnlyBase(Of SalePriceChgTypeInfo)

        Private mSalePriceChgTypeID As Integer
        Private mSalePriceChgTypeName As String

        Friend Sub New()
        End Sub
        Friend Sub New(ByVal DR As SafeDataReader)
            mSalePriceChgTypeID = DR.GetByte("pricechgtypeid")
            mSalePriceChgTypeName = DR.GetString("pricechgtypedesc")
        End Sub

        Protected Overrides Function GetIdValue() As Object
            Return mSalePriceChgTypeID
        End Function

        Public ReadOnly Property SalePriceChgTypeID() As Integer
            Get
                Return mSalePriceChgTypeID
            End Get
        End Property

        Public ReadOnly Property SalePriceChgTypeName() As String
            Get
                Return mSalePriceChgTypeName
            End Get
        End Property

    End Class
End Namespace

