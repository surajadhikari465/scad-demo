Public Class BOValidateByRegion

    Public Function ListRegions() As ArrayList
        Dim mu As New DAOValidateByRegion()
        Return mu.ListRegions()
    End Function
    Public Function ValidationEmailInfo() As DataTable
        Dim mu As New DAOValidateByRegion()
        Return mu.GetEmailInfo
    End Function
    Public Function SessionValidated(ByVal SessionID As Integer) As Boolean
        Dim mu As New DAOValidateByRegion()
        Return mu.GetValidationSuccess(SessionID)
    End Function

    Public Sub ValidateByRegion(ByVal RegionID As Integer)
        Dim mu As New DAOValidateByRegion()
        mu.ValidateByRegion(RegionID)
    End Sub
End Class
