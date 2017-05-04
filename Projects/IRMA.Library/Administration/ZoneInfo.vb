Namespace Administration
    <Serializable()> _
    Public Class ZoneInfo
        Inherits ReadOnlyBase(Of ZoneInfo)

        Private mZoneID As Integer
        Private mZoneName As String
        Private mZoneGLMarketingExpenseAcct As Integer
        Private mZoneRegionID As Integer

        Friend Sub New()
            mZoneID = -1
            mZoneName = ""
            mZoneGLMarketingExpenseAcct = -1
            mZoneRegionID = -1
        End Sub

        Friend Sub New(ByVal DR As SafeDataReader)
            mZoneID = DR.GetInt32("Zone_ID")
            mZoneName = DR.GetString("Zone_Name")
            mZoneGLMarketingExpenseAcct = DR.GetInt32("GLMarketingExpenseAcct")
            mZoneRegionID = DR.GetInt32("Region_ID")
        End Sub

        Protected Overrides Function GetIdValue() As Object
            Return mZoneID
        End Function

        Public ReadOnly Property ZoneID() As Integer
            Get
                Return mZoneID
            End Get
        End Property

        Public ReadOnly Property ZoneName() As String
            Get
                Return mZoneName
            End Get
        End Property

        Public ReadOnly Property ZoneGLMarketingExpenseAcct() As Integer
            Get
                Return mZoneGLMarketingExpenseAcct
            End Get
        End Property

        Public ReadOnly Property ZoneRegionID() As Integer
            Get
                Return mZoneRegionID
            End Get
        End Property
    End Class
End Namespace

