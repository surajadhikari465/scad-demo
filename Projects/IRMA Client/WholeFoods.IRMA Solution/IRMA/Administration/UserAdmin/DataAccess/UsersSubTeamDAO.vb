Imports WholeFoods.Utility.DataAccess

''' <summary>
''' Handles read/writes
''' </summary>
''' <remarks></remarks>
Public Class UsersSubTeamDAO

    Public Shared Function Add(ByVal Entry As UsersSubTeamBO) As Boolean

        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        'setup parameters for stored proc
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "@User_ID"
        currentParam.Value = Entry.UserID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@SubTeam_No"
        currentParam.Value = Entry.SubTeamNo
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@IsCoordinator"
        currentParam.Value = Entry.Coordinator
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@Action"
        currentParam.Value = "A"
        currentParam.Type = DBParamType.Char
        paramList.Add(currentParam)

        Try

            factory.ExecuteStoredProcedure("Administration_UserSubteam", paramList)

            Return True

        Catch ex As Exception

            Throw ex

        End Try

    End Function

    Public Shared Function Remove(ByVal Entry As UsersSubTeamBO) As Boolean

        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        'setup parameters for stored proc
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "@User_ID"
        currentParam.Value = Entry.UserID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@SubTeam_No"
        currentParam.Value = Entry.SubTeamNo
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@IsCoordinator"
        currentParam.Value = Entry.Coordinator
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@Action"
        currentParam.Value = "D"
        currentParam.Type = DBParamType.Char
        paramList.Add(currentParam)

        Try

            factory.ExecuteStoredProcedure("Administration_UserSubteam", paramList)

            Return True

        Catch ex As Exception

            Throw ex

        End Try

    End Function

    Public Shared Function Update(ByVal Entry As UsersSubTeamBO) As Boolean

        Dim factory As New DataFactory(DataFactory.ItemCatalog)

        'setup parameters for stored proc
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "@User_ID"
        currentParam.Value = Entry.UserID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@SubTeam_No"
        currentParam.Value = Entry.SubTeamNo
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@IsCoordinator"
        currentParam.Value = Entry.Coordinator
        currentParam.Type = DBParamType.Bit
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@Action"
        currentParam.Value = "U"
        currentParam.Type = DBParamType.Char
        paramList.Add(currentParam)

        Try

            factory.ExecuteStoredProcedure("Administration_UserSubteam", paramList)

            Return True

        Catch ex As Exception

            Throw ex

        End Try

    End Function

End Class
