Imports WholeFoods.Utility
Imports System.IO
Imports WholeFoods.Utility.DataAccess

Public Class Form_WeeklySalesRollup

    Private Sub RunButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RunButton.Click
        RunWeeklySalesRollup()
    End Sub


    Private Sub CancelButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CancelButton.Click
        Me.Close()
    End Sub

    Private Sub RunWeeklySalesRollup()

        Dim Val As ArrayList
        Dim factory As DataAccess.DataFactory
        factory = New DataAccess.DataFactory(DataAccess.DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        Try
            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "StartDate"
            currentParam.Value = DateTime_StartDate.Value
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "EndDate"
            currentParam.Value = DateTime_EndDate.Value
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            Val = factory.ExecuteStoredProcedure("WeeklyRollUp_SalesSumByItem", paramList)
        Catch ex As Exception
            Throw ex
        End Try

        Me.RunButton.Enabled = False

    End Sub

End Class