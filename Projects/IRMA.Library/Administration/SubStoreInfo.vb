Namespace Administration
    <Serializable()> _
    Public Class SubStoreInfo
        Inherits ReadOnlyBase(Of SubStoreInfo)

        Private mSubStoreID As Integer
        Private mSubStoreName As String

        Friend Sub New()
        End Sub

        Friend Sub New(ByVal DR As SafeDataReader)
            mSubStoreID = DR.GetInt32("Store_No")
            mSubStoreName = DR.GetString("Store_Name")
        End Sub

        Protected Overrides Function GetIdValue() As Object
            Return mSubStoreID
        End Function

        Public ReadOnly Property SubStoreID() As Integer
            Get
                Return mSubStoreID
            End Get
        End Property

        Public ReadOnly Property SubStoreName() As String
            Get
                Return mSubStoreName
            End Get
        End Property

    End Class
End Namespace

