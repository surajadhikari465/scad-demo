Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Imports log4net



Namespace WholeFoods.IRMA.ItemHosting.DataAccess



    Public Class TeamDAO

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Function GetTeam(ByVal Team_No As Integer) As DataSet

            logger.Debug("GetTeam Entry with Team_No = " + Team_No.ToString)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            currentParam = New DBParam
            currentParam.Name = "Team_No"
            currentParam.Value = Team_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            logger.Debug("GetTeam Exit")

            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("Teams_GetTeam", paramList)
        End Function

        Public Function GetTeams() As DataSet

            logger.Debug("GetTeams Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            logger.Debug("GetTeams Exit")

            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("Teams_LoadTeams")
        End Function

        Public Shared Function GetRetailTeams() As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As DataTable = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataTable("GetRetailTeams")
            Catch ex As Exception
                Throw ex
            End Try

            Return results
        End Function

        Public Function Validate_Teamname(ByVal CurrentTeamNo As Integer, ByVal Team_Name As String) As DataSet

            logger.Debug("Validate_Teamname Entry with CurrentTeamNo = " + CurrentTeamNo.ToString & " , Team_Name= " + Team_Name)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam


            currentParam = New DBParam
            currentParam.Name = "Team_No"
            currentParam.Value = CurrentTeamNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Team_Name"
            currentParam.Value = Team_Name
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            logger.Debug("Validate_Teamname Exit")

            Return factory.GetStoredProcedureDataSet("Teams_Validate_TeamName", paramList)
        End Function
        Public Function Validate_TeamAbbr(ByVal CurrentTeamNo As Integer, ByVal Team_Abbr As String) As DataSet

            logger.Debug("Validate_TeamAbbr Entry with CurrentTeamNo= " + CurrentTeamNo.ToString + " ,Team_Abbr =" + Team_Abbr)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam


            currentParam = New DBParam
            currentParam.Name = "Team_No"
            currentParam.Value = CurrentTeamNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)


            currentParam = New DBParam
            currentParam.Name = "Team_Abbr"
            currentParam.Value = Team_Abbr
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            logger.Debug("Validate_TeamAbbr Exit")

            Return factory.GetStoredProcedureDataSet("Teams_Validate_TeamAbbr", paramList)
        End Function

        Public Sub CreateNewTeam(ByVal BO As TeamBO)

            logger.Debug("CreateNewTeam Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam


            currentParam = New DBParam
            currentParam.Name = "Team_No"
            currentParam.Value = BO.TeamNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Team_Name"
            currentParam.Value = BO.TeamName
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Team_Abbr"
            currentParam.Value = BO.TeamAbbr
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("Teams_CreateNew", paramList)

            logger.Debug("CreateNewTeam Exit")

        End Sub

        Public Sub SaveChanges(ByVal BO As TeamBO)

            logger.Debug("SaveChanges Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam


            currentParam = New DBParam
            currentParam.Name = "Team_No"
            currentParam.Value = BO.TeamNo
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Team_Name"
            currentParam.Value = BO.TeamName
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Team_Abbr"
            currentParam.Value = BO.TeamAbbr
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            factory.ExecuteStoredProcedure("Teams_SaveChanges", paramList)

            logger.Debug("SaveChanges Exit")

        End Sub

    End Class

End Namespace
