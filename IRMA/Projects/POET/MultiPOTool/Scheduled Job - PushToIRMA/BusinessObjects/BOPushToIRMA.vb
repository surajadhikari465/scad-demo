Public Class BOPushToIRMA

    Public Function ListRegions() As ArrayList
        Dim mu As New DAOPushToIRMA()
        Return mu.ListRegions()
    End Function

    Public Sub PushByRegion(ByVal RegionID As Integer)
        Dim mu As New DAOPushToIRMA()
        mu.PushByRegion(RegionID)
    End Sub
End Class
