Imports WholeFoods.Utility.DataAccess

''' <summary>
''' Handles read/writes
''' </summary>
''' <remarks></remarks>
Public Class UserStoreTeamTitleDAO

    Public Shared Function Add(ByVal Entry As UserStoreTeamTitleBO) As Boolean

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
        currentParam.Name = "@Store_No"
        currentParam.Value = Entry.StoreNo
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@Team_No"
        currentParam.Value = Entry.TeamNo
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@Title_ID"
        currentParam.Value = Entry.TitleID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@Action"
        currentParam.Value = "A"
        currentParam.Type = DBParamType.Char
        paramList.Add(currentParam)

        Try

            factory.ExecuteStoredProcedure("Administration_UserStoreTeamTitle", paramList)

            Return True

        Catch ex As Exception

            Throw ex

        End Try

    End Function

    Public Shared Function Remove(ByVal Entry As UserStoreTeamTitleBO) As Boolean

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
        currentParam.Name = "@Store_No"
        currentParam.Value = Entry.StoreNo
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@Team_No"
        currentParam.Value = Entry.TeamNo
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@Title_ID"
        currentParam.Value = Entry.TitleID
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "@Action"
        currentParam.Value = "D"
        currentParam.Type = DBParamType.Char
        paramList.Add(currentParam)

        Try

            factory.ExecuteStoredProcedure("Administration_UserStoreTeamTitle", paramList)

            Return True

        Catch ex As Exception

            Throw ex

        End Try

    End Function

End Class
