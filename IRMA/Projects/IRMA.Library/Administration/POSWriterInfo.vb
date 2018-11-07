Namespace Administration
    <Serializable()> _
    Public Class POSWriterInfo
        Inherits ReadOnlyBase(Of POSWriterInfo)

        Private mPOSWriterID As Integer
        Private mPOSWriterName As String

        Friend Sub New()
        End Sub

        Friend Sub New(ByVal DR As SafeDataReader)
            mPOSWriterID = DR.GetInt32("POSFileWriterKey")
            mPOSWriterName = DR.GetString("POSFileWriterCode")
        End Sub

        Protected Overrides Function GetIdValue() As Object
            Return mPOSWriterID
        End Function

        Public ReadOnly Property POSWriterID() As Integer
            Get
                Return mPOSWriterID
            End Get
        End Property

        Public ReadOnly Property POSWriterName() As String
            Get
                Return mPOSWriterName
            End Get
        End Property

    End Class
End Namespace

