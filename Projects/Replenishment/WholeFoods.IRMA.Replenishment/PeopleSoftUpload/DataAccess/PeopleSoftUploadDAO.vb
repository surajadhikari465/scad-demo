Imports log4net
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Replenishment.PeopleSoftUpload.BusinessLogic
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.Replenishment.PeopleSoftUpload.DataAccess
    Public Class PeopleSoftUploadDAO
        ' -----------------------------------------------------------------
        ' Update History
        ' -----------------------------------------------------------------
        ' TFS 12198 (v3.6)
        ' Tom Lux
        ' 04/12/2010
        ' Removed the 'Uploaded Date' parameter passed to the 'set ap uploads' process because the value in the order is now set in the stored procedure.

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Shared Function GetAPUploads(ByVal region As String) As SqlDataReader
            logger.Debug("GetAPUploads entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim results As SqlDataReader = Nothing

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "Region_Code"
            currentParam.Value = region
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            results = factory.GetStoredProcedureDataReader("GetAPUploads", paramList)

            logger.Debug("GetAPUploads exit: results.HasRows=" + results.HasRows().ToString())
            Return results
        End Function

        ''' <summary>
        ''' Reads the unique region codes assigned to stores in the StoreRegionMapping table.
        ''' </summary>
        ''' <returns>An ArrayList of String objects</returns>
        ''' <remarks></remarks>
        Public Shared Function GetRegionList() As ArrayList
            logger.Debug("GetRegionList entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim regionList As New ArrayList()

            Try
                ' Execute the stored procedure 
                results = factory.GetStoredProcedureDataReader("GetRegionList")

                ' Parse the results into a collection of String objects
                While (results.Read())
                    If (Not results.IsDBNull(results.GetOrdinal("Region_Code"))) Then
                        regionList.Add(results.GetString(results.GetOrdinal("Region_Code")))
                    End If
                End While
            Catch ex As Exception
                Throw ex
            Finally
                ' Close the result set and the connection
                If (results IsNot Nothing) Then
                    results.Close()
                End If
            End Try

            logger.Debug("GetRegionList exit: regionList.Count=" + regionList.Count.ToString)
            Return regionList
        End Function

        Public Shared Sub SetOrderAsUploaded(ByRef peopleSoftOrder As PeopleSoftUploadBO)
            logger.Debug("SetOrderAsUploaded entry")
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim results As SqlDataReader = Nothing
            Dim paramList As New ArrayList
            Dim currentParam As DBParam

            ' setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = peopleSoftOrder.OrderHeaderID
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Freight3Party"
            currentParam.Value = peopleSoftOrder.ThirdPartyFreightInvoice
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            'MD 8/10/2009: WI 10393, WI 10721 Added the PurchaseAccountsTotal (includes only 500000 account charges) and 
            'APUploadedCost (includes all charges 500000, allowance and charges) field to be stored in OrderHeader table
            currentParam = New DBParam
            currentParam.Name = "PurchaseAccountsTotal"
            currentParam.Value = peopleSoftOrder.DIST_MERCHANDISE_AMT
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "APUploadedCost"
            currentParam.Value = peopleSoftOrder.GROSS_AMT
            currentParam.Type = DBParamType.Money
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            factory.ExecuteStoredProcedure("SetAPUploadsUploaded", paramList)

            logger.Debug("SetOrderAsUploaded exit")
        End Sub

    End Class
End Namespace

