Imports System.Data.SqlClient
Imports log4net
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.ItemHosting.DataAccess

    Public Class DSDVendorDAO
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Shared Function GetDSDVendorAllStore(ByVal vendorID As Integer) As DataTable
            logger.Debug("GetDSDVendorAllStore Entry:")

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing
            Dim dt As New DataTable()

            Try
                currentParam = New DBParam
                currentParam.Name = "Vendor_ID"
                currentParam.Value = vendorID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                results = factory.GetStoredProcedureDataReader("GetDSDVendorAllStores", paramList)

                dt.Columns.Add("Store_Number")
                dt.Columns.Add("Store_Name")
                dt.Columns.Add("IsReceivingDocument")

                Dim row As DataRow
                While results.Read

                    row = dt.NewRow()

                    row("Store_Number") = results.GetInt32(results.GetOrdinal("Store_No"))
                    row("Store_Name") = results.GetString(results.GetOrdinal("Store_Name"))
                    row("IsReceivingDocument") = results.GetInt32(results.GetOrdinal("IsReceivingDocument"))
                    dt.Rows.Add(row)
                End While
            Catch ex As DataFactoryException
                logger.Error("GetDSDVendorAllStores Exception: ", ex)
                'send message about exception
                ErrorHandler.ProcessError(ErrorType.DataFactoryException, SeverityLevel.Warning, ex)
            Finally
                If results IsNot Nothing Then
                    results.Close()
                End If
            End Try
            Return dt

            logger.Debug("GetDSDVendorAllStore Exit: ")
        End Function



        Public Shared Function updateDSDVendorSetup(ByVal storeNumber As Integer, ByVal vendorID As Integer, ByVal effectiveDate As Date, ByVal addFlag As Boolean) As Boolean
            logger.Debug("updateDSDVendorSetup Entry: storeNumber=" & storeNumber & ", vendorID=" & vendorID & ", effectiveDate=" & effectiveDate & ", deleteFlag=" & addFlag)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As ArrayList
            Dim success As Boolean
            Try
                ' setup parameters for stored proc
                currentParam = New DBParam
                currentParam.Name = "Store_No"
                currentParam.Value = storeNumber
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "Vendor_ID"
                currentParam.Value = vendorID
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "EffectiveDate"
                currentParam.Value = effectiveDate
                currentParam.Type = DBParamType.DateTime
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "AddFlag"
                currentParam.Value = addFlag
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' -- output --
                currentParam = New DBParam
                currentParam.Name = "Success"
                currentParam.Type = DBParamType.Bit
                paramList.Add(currentParam)

                ' Execute the stored procedure 
                outputList = factory.ExecuteStoredProcedure("dbo.UpdateDSDVendorStore", paramList)
                success = CBool(outputList(0))

            Catch ex As DataFactoryException
                logger.Error("UpdateDSDVendorStore Exception: ", ex)
                'send message about exception
                ErrorHandler.ProcessError(ErrorType.DataFactoryException, SeverityLevel.Warning, ex)
            End Try
            logger.Debug("UpdateDSDVendorStore Exit: success=" + success.ToString)
            Return success

        End Function
    End Class
End Namespace

