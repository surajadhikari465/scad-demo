Namespace Administration
    <Serializable()> _
    Public Class SubTeamSubstitutionInfo
        Inherits ReadOnlyBase(Of SubTeamSubstitutionInfo)

        Private mSubSubTeamID As Integer
        Private mSubSubTeamName As String
        Private mSubStoreID As Integer
        Private mSubStoreName As String

        Private Sub New()
        End Sub
        Protected Overrides Function GetIdValue() As Object
            Return mSubStoreID
        End Function
        Public ReadOnly Property SubSubTeamID() As Integer
            Get
                Return mSubSubTeamID
            End Get
        End Property

        Public ReadOnly Property SubSubTeamName() As String
            Get
                Return mSubSubTeamName
            End Get
        End Property
        '<ComponentModel.Browsable(False)> _
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
        Public Overrides Function ToString() As String
            Dim ColumnDelim As String
            ColumnDelim = ","
            Return mSubStoreID.ToString + ColumnDelim + mSubSubTeamID.ToString
        End Function
        Public Shared Function NewInfo(ByVal SubStore As SubStoreInfo, ByVal POSSubteam As POSSubteamInfo) As SubTeamSubstitutionInfo
            Dim ReturnInfo As New SubTeamSubstitutionInfo
            ReturnInfo.mSubSubTeamID = POSSubteam.POSSubteamID
            ReturnInfo.mSubSubTeamName = POSSubteam.POSSubteamName
            ReturnInfo.mSubStoreID = SubStore.SubStoreID
            ReturnInfo.mSubStoreName = SubStore.SubStoreName
            Return ReturnInfo
        End Function

    End Class
End Namespace

