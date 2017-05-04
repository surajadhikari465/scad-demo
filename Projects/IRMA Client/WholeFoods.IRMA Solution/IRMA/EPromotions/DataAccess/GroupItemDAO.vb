Imports WholeFoods.IRMA.EPromotions.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports System.Collections.Generic
Imports System.Data.SqlClient


Namespace WholeFoods.IRMA.EPromotions.DataAccess
    Public Class GroupItemDAO

        Public Function getGroupItemsList(ByRef ItemGroup As ItemGroupBO) As List(Of GroupItemBO)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim groupItemsList As List(Of GroupItemBO) = New List(Of GroupItemBO)
            Dim groupItem As GroupItemBO
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "GroupId"
                currentParam.Value = ItemGroup.GroupID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("EPromotions_GetGroupItems", paramList)

                While results.Read
                    groupItem = New GroupItemBO
                    groupItem.Description = results.GetString(results.GetOrdinal("Item_Description"))
                    groupItem.Key = results.GetInt32(results.GetOrdinal("Item_Key"))
                    groupItem.UserId = results.GetInt32(results.GetOrdinal("User_Id"))
                    groupItemsList.Add(groupItem)
                End While
                results.Close()

            Catch ex As Exception
                
            End Try

            Return groupItemsList
        End Function

    End Class
End Namespace
