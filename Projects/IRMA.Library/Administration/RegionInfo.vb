Namespace Administration
    <Serializable()> _
    Public Class RegionInfo
        Inherits ReadOnlyBase(Of RegionInfo)

        Private mRegionID As Integer
        Private mRegionName As String

        Friend Sub New()
        End Sub

        Friend Sub New(ByVal DR As SafeDataReader)
            mRegionID = DR.GetInt32("Region_ID")
            mRegionName = DR.GetString("RegionName")
        End Sub

        Protected Overrides Function GetIdValue() As Object
            Return mRegionID
        End Function

        Public ReadOnly Property RegionID() As Integer
            Get
                Return mRegionID
            End Get
        End Property

        Public ReadOnly Property RegionName() As String
            Get
                Return mRegionName
            End Get
        End Property

    End Class
End Namespace

