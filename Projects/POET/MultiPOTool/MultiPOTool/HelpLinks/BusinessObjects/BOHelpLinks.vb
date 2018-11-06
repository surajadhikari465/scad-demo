Imports Microsoft.VisualBasic
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient


Public Class BOHelpLinks

    Public Property HelpLinksID() As Integer
        Get

        End Get
        Set(ByVal value As Integer)

        End Set
    End Property

    Public Function IsAdmin(ByVal UserID As Integer) As Boolean
        Dim a As New DAOHelpLinks

        Return a.IsAdmin(UserID)

    End Function
    Public Function GetLinks() As DataSet

        Dim a As New DAOHelpLinks

        Return a.GetLinks

    End Function


    Public Function InsertLink(ByVal param As ArrayList) As Integer

        Dim a As New DAOHelpLinks

        a.InsertLink(param)

    End Function

    Public Function UpdateLink(ByVal param As ArrayList) As Integer

        Dim a As New DAOHelpLinks

        a.UpdateLink(param)

    End Function

    Public Function DeleteLink(ByVal HelpLinksID As Integer) As Integer

        Dim a As New DAOHelpLinks
        a.DeleteLink(HelpLinksID)

    End Function


End Class
