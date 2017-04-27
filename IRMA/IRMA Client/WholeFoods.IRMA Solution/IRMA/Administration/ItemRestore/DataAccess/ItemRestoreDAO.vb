Imports log4net
Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess

Public Class ItemRestoreDAO

    ' ----------------------------------------------------------------------------
    ' Revision History
    ' ----------------------------------------------------------------------------
    ' 8/18/10             Tom Lux               TFS 13138        Added PendingDeleteGetInfo method to return batch info.
    ' 8/18/10             Tom Lux               TFS 13138        Added IsItemActive method to determine if an item "active", meaning it cannot be restored (validation for restore-deleted-item screen).


#Region "Private Members"

    ''' <summary>
    ''' Log4Net logger for this class.
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#End Region

    Public Shared Function Restore(ByVal identifier As String) As Boolean
        Dim results As SqlDataReader = Nothing
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        'setup parameters for stored proc
        currentParam = New DBParam
        currentParam.Name = "Identifier"
        currentParam.Value = identifier
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        ' Execute the stored procedure 
        results = factory.GetStoredProcedureDataReader("UpdateItemRestore", paramList)

        ' It would be nice to have some kind of confirmation or verification of whether or not a row was update
        ' *** yeah .it would ****
        logger.DebugFormat("[UpdateItemRestore] Rows affected: {0}", results.RecordsAffected)
        If results.RecordsAffected Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' Returns a SQL reader containing pending-delete information for an item, if found.
    ''' </summary>
    ''' <param name="identifier">Identifier for which pending-delete info is retrieved.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function PendingDeleteGetInfo(ByVal identifier As String) As SqlDataReader
        Dim dataReader As SqlDataReader
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim paramList As New ArrayList
        Dim currentParam As DBParam

        currentParam = New DBParam
        currentParam.Name = "Item_Key"
        currentParam.Value = DBNull.Value
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Identifier"
        currentParam.Value = identifier
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        dataReader = factory.GetStoredProcedureDataReader("ItemDeletePendingGetInfo", paramList)
        logger.Debug("[PendingDeleteGetInfo][identifier=" & identifier & "] Pending-delete reader has rows?: " & dataReader.HasRows)

        Return dataReader

    End Function

    ''' <summary>
    ''' Determines if an item is active, which should be TRUE as long as it is not pending delete.
    ''' </summary>
    ''' <param name="identifier">Identifier to check for active status.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function IsItemActive(ByVal identifier As String) As Boolean
        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        ' We don't have the item key, so we need to pass NULL.
        Return CType(factory.ExecuteScalar(String.Format("select dbo.fn_IsItemActive(null, {0})", identifier)), Boolean)
    End Function

End Class
