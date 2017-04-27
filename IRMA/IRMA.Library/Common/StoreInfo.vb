Namespace Common
    <Serializable()> _
    Public Class StoreInfo
        Inherits ReadOnlyBase(Of StoreInfo)

        Private mStoreID As Integer
        Private mStoreName As String

        Friend Sub New()
        End Sub

        Friend Sub New(ByVal DR As SafeDataReader)
            mStoreID = DR.GetInt32("Store_No")
            mStoreName = DR.GetString("Store_Name")
        End Sub

        Protected Overrides Function GetIdValue() As Object
            Return mStoreID
        End Function

        Public ReadOnly Property StoreID() As Integer
            Get
                Return mStoreID
            End Get
        End Property

        Public ReadOnly Property StoreName() As String
            Get
                Return mStoreName
            End Get
        End Property

    End Class
End Namespace

