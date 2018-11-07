Imports Microsoft.VisualBasic
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient

Public Class BOPONumbers

    Private _poTypeDescription As String
    Private _poTypeID As Integer
    Private _poNumber As Integer
    Private _userID As Integer

    Public Property POTypeDescription() As String
        Get
            Return _poTypeDescription
        End Get
        Set(ByVal value As String)
            _poTypeDescription = value
        End Set
    End Property

    Public Property POTypeID() As Integer
        Get
            Return _poTypeID
        End Get
        Set(ByVal value As Integer)
            _poTypeID = value
        End Set
    End Property

    Public Property PONumber() As Integer
        Get
            Return _poNumber
        End Get
        Set(ByVal value As Integer)
            _poNumber = value
        End Set
    End Property

    Public Property UserID() As Integer
        Get
            Return _userID
        End Get
        Set(ByVal value As Integer)
            _poNumber = value
        End Set
    End Property

    Public Function GetUnusedPONumbersByUserID(ByVal UserID As Integer) As DataSet

        Dim mu As New DAOPONumbers

        Return mu.GetUnusedPONumbersByUser(UserID)

    End Function

    Public Function AssignPONumbers(ByVal RegionID As Integer, ByVal POTypeID As Integer, ByVal UserID As Integer, ByVal POCount As Integer) As Integer

        Dim mu As New DAOPONumbers

        mu.AssignPONumbers(RegionID, POTypeID, UserID, POCount)

    End Function

    Public Function GetPOTypes() As DataSet

        Dim mu As New DAOPONumbers

        Return mu.GetPOTypes()

    End Function
    Public Shared Function GetVersion() As DataSet

        Dim mu As New DAOPONumbers

        Return mu.GetVersion()

    End Function

    Public Function DeletePONumber(ByVal iPONumber As Integer) As Boolean

        Dim br As New DAOPONumbers

        Return br.DeletePONumber(iPONumber)

    End Function
    Public Function GetReasonCodesForUser(ByVal UserID As Integer) As DataSet

        Dim mu As New DAOPONumbers

        Return mu.GetReasonCodesForUser(UserID)

    End Function
End Class