Imports Microsoft.VisualBasic
Imports WholeFoods.Utility.DataAccess
Imports System.Data.SqlClient

Public Class DAOHelpLinks

    Public Property HelpLinksID() As Integer
        Get

        End Get
        Set(ByVal value As Integer)

        End Set
    End Property


    Private con As String = ConfigurationManager.ConnectionStrings("MultiPOTool").ConnectionString()

    Public Function IsAdmin(ByVal UserID As Integer) As Boolean

        Dim factory As New DataFactory(con, True)

        Dim sql As New StringBuilder
        Dim result As Object = Nothing

        sql.Append("select Administrator from Users where UserID = " & UserID.ToString())

        Try
            result = factory.ExecuteScalar(sql.ToString)
        Catch ex As Exception
            Debug.WriteLine(ex.InnerException)
            Debug.WriteLine(ex.Message)
        End Try

        Return CBool(result)

    End Function

    Public Function GetLinks() As DataSet

        Dim factory As New DataFactory(con, True)
        Dim results As New DataSet

        Try
            results = factory.GetStoredProcedureDataSet("GetLinks")
        Catch ex As Exception
            Throw ex
        End Try

        Return results

    End Function

    Public Sub InsertLink(ByVal LinkFields As ArrayList)
        Dim factory As New DataFactory(con, True)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam
        Dim results As ArrayList

        Try
            ' setup parameters for stored proc

            currentParam = New DBParam
            currentParam.Name = "LinkDescription"
            currentParam.Value = LinkFields.Item(0)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "LinkURL"
            currentParam.Value = LinkFields.Item(1)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UpdatedUserID"
            currentParam.Value = LinkFields.Item(2)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "OrderOfAppearance"
            currentParam.Value = LinkFields.Item(3)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)


            results = factory.ExecuteStoredProcedure("InsertLink", paramList)

        Catch ex As Exception
            Throw ex
            Throw ex.InnerException
        End Try
    End Sub

    Public Sub UpdateLink(ByVal LinkFields As ArrayList)
        Dim factory As New DataFactory(con, True)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam
        Dim results As ArrayList

        Try
            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "HelpLinksID"
            currentParam.Value = LinkFields.Item(0)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "LinkDescription"
            currentParam.Value = LinkFields.Item(1)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "LinkURL"
            currentParam.Value = LinkFields.Item(2)
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "UpdatedUserID"
            currentParam.Value = LinkFields.Item(3)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "OrderOfAppearance"
            currentParam.Value = LinkFields.Item(4)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.ExecuteStoredProcedure("UpdateLink", paramList)

        Catch ex As Exception
            Throw ex.InnerException
        End Try
    End Sub

    Public Sub DeleteLink(ByVal HelpLinksID As Integer)
        Dim factory As New DataFactory(con, True)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam
        Dim results As ArrayList

        Try
            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "HelpLinksID"
            currentParam.Value = HelpLinksID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            results = factory.ExecuteStoredProcedure("DeleteLink", paramList)

        Catch ex As Exception
            Throw ex.InnerException
        End Try
    End Sub
End Class
