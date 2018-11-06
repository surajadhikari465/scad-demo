Imports System.Data.SqlClient
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.Utility.DataAccess
Imports log4net


Namespace WholeFoods.IRMA.ItemHosting.DataAccess
    Public Class ZoneDAO

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


        Public Shared Function GetZoneList() As ArrayList

            logger.Debug("GetZoneList Entry")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim zone As ZoneBO
            Dim zoneList As New ArrayList
            Dim results As SqlDataReader = Nothing

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetZones")

                While results.Read
                    zone = New ZoneBO()
                    zone.ZoneID = results.GetInt32(results.GetOrdinal("Zone_ID"))
                    zone.ZoneName = results.GetString(results.GetOrdinal("Zone_Name"))

                    zoneList.Add(zone)
                End While
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetZoneList Exit")
            Return zoneList
        End Function

        Public Shared Function GetZones() As DataTable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)

            Try
                Return factory.GetStoredProcedureDataTable("Administration_GetZones")
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Shared Function InsertZone(ByVal sZoneName As String, ByVal iGLMarketingExpenseAcct As String, ByVal iRegionId As Integer, ByVal iLastUpdateUserId As Integer) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Zone_Name"
                currentParam.Value = sZoneName
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "GLMarketingExpenseAcct"
                currentParam.Value = iGLMarketingExpenseAcct
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "RegionId"
                currentParam.Value = iRegionId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "LastUpdateUserID"
                currentParam.Value = iLastUpdateUserId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "NewZoneID"
                currentParam.Value = DBNull.Value
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "NewLastUpdate"
                currentParam.Value = DBNull.Value
                currentParam.Type = DBParamType.Timestamp
                paramList.Add(currentParam)

                factory.ExecuteStoredProcedure("Administration_InsertZone", paramList)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Shared Function DeleteZone(ByVal iZoneId As Integer) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "ZoneID"
                currentParam.Value = iZoneId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                factory.ExecuteStoredProcedure("Administration_DeleteZone", paramList)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Shared Function UpdateZone(ByVal iZoneId As Integer, ByVal sZoneName As String, ByVal iGLMarketingExpenseAcct As String, ByVal iRegionId As Integer, ByVal iLastUpdateUserId As Integer) As Boolean
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Zone_Name"
                currentParam.Value = sZoneName
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "GLMarketingExpenseAcct"
                currentParam.Value = iGLMarketingExpenseAcct
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "RegionId"
                currentParam.Value = iRegionId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "LastUpdate"
                currentParam.Value = Now
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "LastUpdateUserID"
                currentParam.Value = iLastUpdateUserId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ZoneID"
                currentParam.Value = iZoneId
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "NewLastUpdate"
                currentParam.Value = DBNull.Value
                currentParam.Type = DBParamType.Timestamp
                paramList.Add(currentParam)

                factory.ExecuteStoredProcedure("Administration_UpdateZone", paramList)
            Catch ex As Exception
                Throw ex
            End Try
        End Function
    End Class
End Namespace

