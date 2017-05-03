Imports System.Data
Imports System.Data.Common
Imports System.Data.SqlClient
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Replenishment.CloseReceiving.DataAccess
    Public Class CloseReceivingDAO
        Public Shared Sub AutoCloseReceiving(Optional ByVal user_ID As Integer = 0)
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim parameters As New ArrayList()
            Dim dbParam As New DBParam()

            With dbParam
                .Name = "User_ID"
                .Type = DBParamType.Int
                .Value = user_ID
            End With

            parameters.Add(dbParam)

            factory.ExecuteStoredProcedure("AutoCloseReceiving", parameters)
        End Sub
    End Class
End Namespace