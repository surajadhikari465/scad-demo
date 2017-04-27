Imports WholeFoods.Utility.DataAccess
Imports log4net

Public Class StoreSubTeamDiscountExceptionsDAO

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Function GetSubTeams(ByVal Store_No As Integer) As DataSet
        logger.Debug("GetSubTeams Entry")

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam


        ' setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "Store_No"
        currentParam.Value = Store_No
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)


        logger.Debug("GetSubTeams Exit")
        ' Execute the stored procedure 
        Return factory.GetStoredProcedureDataSet("SubTeams_GetDiscountExceptions", paramList)
    End Function

    Public Function AddDiscountException(ByVal Store_No As Integer, ByVal SubTeam_No As Integer) As DataSet
        logger.Debug("AddDiscountException Entry")

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

        logger.Debug("AddDiscountException Exit")
        ' Execute the stored procedure 
        Return factory.GetStoredProcedureDataSet("SubTeams_AddDiscountExceptions", paramList)
    End Function

    Public Function RemoveDiscountException(ByVal Store_No As Integer, ByVal SubTeam_No As Integer) As DataSet
        logger.Debug("RemoveDiscountException Entry")

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

        logger.Debug("RemoveDiscountException Exit")
        ' Execute the stored procedure 
        Return factory.GetStoredProcedureDataSet("SubTeams_RemoveDiscountExceptions", paramList)
    End Function

End Class
