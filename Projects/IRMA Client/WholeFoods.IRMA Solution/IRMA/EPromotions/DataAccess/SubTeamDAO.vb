
Option Explicit On
Option Strict On

Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports WholeFoods.IRMA.EPromotions.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.EPromotions.DataAccess
    Public Class SubTeamDAO
        ''' <summary>
        ''' Read complete list of Sub Team data and return ArrayList of SubTeamBO objects
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetSubTeamList() As BindingList(Of SubTeamBO)
            Dim subteamList As New BindingList(Of SubTeamBO)
            Dim SubTeamBO As SubTeamBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing

            Try
                ' setup parameters for stored proc;  0 returns all values
                Dim paramList As New ArrayList

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetSubTeams", paramList)

                While results.Read

                    SubTeamBO = New SubTeamBO()
                    With SubTeamBO

                        If (Not results.IsDBNull(results.GetOrdinal("SubTeam_No"))) Then
                            .SubTeamNo = results.GetInt32(results.GetOrdinal("SubTeam_No"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("SubTeam_Name"))) Then
                            .SubTeamName = results.GetString(results.GetOrdinal("SubTeam_Name"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("SubTeam_Unrestricted"))) Then
                            '.Unrestricted = results.GetInt32(results.GetOrdinal("SubTeam_Unrestricted"))
                        End If
                    End With

                    subteamList.Add(SubTeamBO)

                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return subteamList
        End Function
    End Class
End Namespace

