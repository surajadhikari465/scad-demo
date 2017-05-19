Imports Microsoft.VisualBasic
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient

Public Class BOValidatedPOs

    Public Shared Function GetPOsReadyToPushByUserByVal(ByVal UserID As Integer) As DataSet

        Dim mu As New DAOValidatedPOs

        Return mu.GetPOsReadyToPushByUser(UserID)

    End Function

    Public Sub DeletePOs(ByVal UserID As Integer, ByVal POHeaderID As Integer)

        Dim mu As New DAOValidatedPOs

        mu.DeletePOs(UserID, POHeaderID)

    End Sub

    Public Shared Function GetVendors(ByVal UserID As Integer, ByVal con As String) As DataSet

        Dim mu As New DAOValidatedPOs

        Return mu.GetVendors(UserID, con)

    End Function

    Public Shared Function GetSubTeams(ByVal UserID As Integer, ByVal con As String) As DataSet

        Dim mu As New DAOValidatedPOs

        Return mu.GetSubTeams(UserID, con)

    End Function
    Public Shared Function GetStoreNames(ByVal UserID As Integer, ByVal con As String) As DataSet

        Dim mu As New DAOValidatedPOs

        Return mu.GetStoreNames(UserID, con)

    End Function
    Public Shared Function GetPOsPushedToIRMA(ByVal UserID As Integer, ByVal Top As Integer,
                                       ByVal StartDate As Date, ByVal EndDate As Date,
                                       ByVal Store As String, ByVal Vendor As Integer,
                                       ByVal Subteam As Integer,
                                       ByVal POType As Integer) As DataSet

        Dim mu As New DAOValidatedPOs

        Return mu.GetPOsPushedToIRMA(UserID, Top, StartDate, EndDate, Store, Vendor, Subteam, POType)

    End Function

    Public Shared Function GetRegionalUsers(ByVal UserID As Integer) As DataTable
        Dim mu As New DAOValidatedPOs

        Return mu.GetRegionalUsers(UserID)

    End Function

End Class
