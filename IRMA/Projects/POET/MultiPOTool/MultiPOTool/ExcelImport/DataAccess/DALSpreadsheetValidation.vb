Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports WholeFoods.Utility.DataAccess

Public Class DALSpreadsheetValidation
    Implements IValidateSpreadSheet

    Private con As String = ConfigurationManager.ConnectionStrings("MultiPOTool").ConnectionString
    Public Function GetPONumberIDs(ByVal UserID As Integer) As DataSet

        Dim factory As New DataFactory(con, True)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam
        Dim results As DataSet


        Try
            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "UserID"
            currentParam.Value = UserID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)


            results = factory.GetStoredProcedureDataSet("GetAvailablePONumberIDsByUserID", paramList)
        Catch ex As Exception
            Throw ex
        End Try

        Return results

    End Function
End Class
