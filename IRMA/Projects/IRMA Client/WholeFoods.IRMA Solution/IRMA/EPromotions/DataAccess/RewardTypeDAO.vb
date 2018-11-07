Option Explicit On
Option Strict On

Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports System.ComponentModel
Imports WholeFoods.IRMA.EPromotions.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.EPromotions.DataAccess
    Public Class RewardTypeDAO
        ''' <summary>
        ''' Read complete list of Reward Type data and return ArrayList of RewardTypeBO objects
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetRewardTypeList() As ArrayList
            Dim RewardTypeList As New ArrayList
            Dim RewardTypeBO As RewardTypeBO
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing

            Try
                ' setup parameters for stored proc;  0 returns all values
                Dim paramList As New ArrayList

                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("EPromotions_GetRewardTypes", paramList)

                While results.Read

                    RewardTypeBO = New RewardTypeBO()
                    With RewardTypeBO

                        If (Not results.IsDBNull(results.GetOrdinal("RewardType_ID"))) Then
                            .RewardTypeID = results.GetInt32(results.GetOrdinal("RewardType_ID"))
                        End If

                        If (Not results.IsDBNull(results.GetOrdinal("Reward_Name"))) Then
                            .Name = results.GetString(results.GetOrdinal("Reward_Name"))
                        End If

                    End With

                    RewardTypeList.Add(RewardTypeBO)

                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            Return RewardTypeList
        End Function
    End Class
End Namespace

