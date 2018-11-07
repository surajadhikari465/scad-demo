Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Imports log4net


Namespace WholeFoods.IRMA.ItemHosting.DataAccess
    Public Class StoreSubTeamDAO

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Function GetSubTeamToTeamRelationships(ByVal Store_No As Integer) As DataSet
            logger.Debug("GetSubTeamToTeamRelationships Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam


            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = Store_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)


            logger.Debug("GetSubTeamToTeamRelationships Exit")
            ' Execute the stored procedure 
            Return factory.GetStoredProcedureDataSet("SubTeams_GetTeamSubTeamRelationshipsByStore", paramList)
        End Function
        Public Function ValidateSubTeamToTeamRelationships(ByVal Store_No As Integer, ByVal SubTeam_No As Integer, ByVal Team_No As Integer) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam


            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = Store_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SubTeam_No"
            currentParam.Value = SubTeam_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Team_No"
            currentParam.Value = Team_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)


            Return factory.GetStoredProcedureDataSet("SubTeams_ValidateSubTeamToTeamRelationships", paramList)
        End Function

        Public Function UpdateSubTeamToTeamRelationship(ByVal Store_No As Integer, ByVal SubTeam_No As Integer, ByVal Team_No As Integer, ByVal PS_SubTeam_No As Integer, ByVal PS_Team_No As Integer, ByVal CostFactor As Decimal, ByVal ICVID As Integer) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam


            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = Store_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SubTeam_No"
            currentParam.Value = SubTeam_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Team_No"
            currentParam.Value = Team_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PS_SubTeam_No"
            If PS_SubTeam_No = -1 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = PS_SubTeam_No
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PS_Team_No"
            If PS_Team_No = -1 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = PS_Team_No
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "CostFactor"
            currentParam.Value = CostFactor
            currentParam.Type = DBParamType.Decimal
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ICVID"
            If ICVID = -1 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = ICVID
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Return factory.GetStoredProcedureDataSet("SubTeams_UpdateSubTeamToTeamRelationship", paramList)
        End Function
        Public Function CreateSubTeamToTeamRelationship(ByVal Store_No As Integer, ByVal SubTeam_No As Integer, ByVal Team_No As Integer, ByVal PS_SubTeam_No As Integer, ByVal PS_Team_No As Integer, ByVal CostFactor As Decimal, ByVal ICVID As Integer) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam


            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = Store_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SubTeam_No"
            currentParam.Value = SubTeam_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Team_No"
            currentParam.Value = Team_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PS_SubTeam_No"
            If PS_SubTeam_No = -1 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = PS_SubTeam_No
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "PS_Team_No"
            If PS_Team_No = -1 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = PS_Team_No
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "CostFactor"
            currentParam.Value = CostFactor
            currentParam.Type = DBParamType.Decimal
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ICVID"
            If ICVID = -1 Then
                currentParam.Value = DBNull.Value
            Else
                currentParam.Value = ICVID
            End If
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Return factory.GetStoredProcedureDataSet("SubTeams_CreateSubTeamToTeamRelationship", paramList)
        End Function

        Public Function RemoveSubTeamToTeamRelationship(ByVal Store_No As Integer, ByVal SubTeam_No As Integer, ByVal Team_No As Integer) As DataSet
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam


            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Store_No"
            currentParam.Value = Store_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "SubTeam_No"
            currentParam.Value = SubTeam_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Team_No"
            currentParam.Value = Team_No
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            Return factory.GetStoredProcedureDataSet("SubTeams_RemoveSubTeamToTeamRelationship", paramList)
        End Function
    End Class


End Namespace