Namespace Administration
    <Serializable()> _
    Public Class POSSubteamInfo
        Inherits ReadOnlyBase(Of POSSubteamInfo)

        Private mPOSSubteamID As Integer
        Private mPOSSubteamName As String

        Friend Sub New()
        End Sub

        Friend Sub New(ByVal DR As SafeDataReader)
            mPOSSubteamID = DR.GetInt32("SubTeam_No")
            mPOSSubteamName = DR.GetString("SubTeam_Name")
        End Sub

        Protected Overrides Function GetIdValue() As Object
            Return mPOSSubteamID
        End Function

        Public ReadOnly Property POSSubteamID() As Integer
            Get
                Return mPOSSubteamID
            End Get
        End Property

        Public ReadOnly Property POSSubteamName() As String
            Get
                Return mPOSSubteamName
            End Get
        End Property

    End Class
End Namespace

