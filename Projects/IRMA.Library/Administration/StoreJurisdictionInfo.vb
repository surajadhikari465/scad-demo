Namespace Administration
    <Serializable()> _
    Public Class StoreJurisdictionInfo
        Inherits ReadOnlyBase(Of StoreJurisdictionInfo)

        Private mStoreJurisdictionID As Integer
        Private mStoreJurisdictionName As String

        Friend Sub New()
        End Sub

        Friend Sub New(ByVal DR As SafeDataReader)
            mStoreJurisdictionID = DR.GetInt32("StoreJurisdictionID")
            mStoreJurisdictionName = DR.GetString("StoreJurisdictionDesc")
        End Sub

        Protected Overrides Function GetIdValue() As Object
            Return mStoreJurisdictionID
        End Function

        Public ReadOnly Property StoreJurisdictionID() As Integer
            Get
                Return mStoreJurisdictionID
            End Get
        End Property

        Public ReadOnly Property StoreJurisdictionName() As String
            Get
                Return mStoreJurisdictionName
            End Get
        End Property

    End Class
End Namespace

