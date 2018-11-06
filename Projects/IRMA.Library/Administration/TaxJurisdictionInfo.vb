Namespace Administration
    <Serializable()> _
    Public Class TaxJurisdictionInfo
        Inherits ReadOnlyBase(Of TaxJurisdictionInfo)

        Private mTaxJurisdictionID As Integer
        Private mTaxJurisdictionName As String

        Friend Sub New()
            mTaxJurisdictionID = -1
            mTaxJurisdictionName = "Copy Values From Existing Tax Jurisdiction"
        End Sub

        Friend Sub New(ByVal DR As SafeDataReader)
            mTaxJurisdictionID = DR.GetInt32("TaxJurisdictionID")
            mTaxJurisdictionName = DR.GetString("TaxJurisdictionDesc")
        End Sub

        Protected Overrides Function GetIdValue() As Object
            Return mTaxJurisdictionID
        End Function

        Public ReadOnly Property TaxJurisdictionID() As Integer
            Get
                Return mTaxJurisdictionID
            End Get
        End Property

        Public ReadOnly Property TaxJurisdictionName() As String
            Get
                Return mTaxJurisdictionName
            End Get
        End Property

    End Class
End Namespace

