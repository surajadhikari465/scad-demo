Imports log4net
Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility
Namespace WholeFoods.IRMA.ItemHosting.DataAccess

    Public Class VendorStorePayAgreedCostDAO

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Shared Function updatePayAgreedCostSetup(ByVal storeNumber As Integer, ByVal vendorID As Integer, ByVal effectiveDate As Date, ByVal deleteFlag As Boolean) As Boolean
            logger.Debug("updatePayAgreedCostSetup Entry: storeNumber=" & storeNumber & ", vendorID=" & vendorID & ", effectiveDate=" & effectiveDate & ", deleteFlag=" & deleteFlag)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim paramList As New ArrayList
            Dim currentParam As DBParam
            Dim outputList As ArrayList
            Dim success As Boolean

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
            currentParam.Name = "DeleteFlag"
            currentParam.Value = deleteFlag
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' -- output --
            currentParam = New DBParam
            currentParam.Name = "Success"
            currentParam.Type = DBParamType.Bit
            paramList.Add(currentParam)

            ' Execute the stored procedure 
            outputList = factory.ExecuteStoredProcedure("dbo.UpdateVendorStorePayAgreedCostSetup", paramList)
            success = CBool(outputList(0))
            logger.Debug("updatePayAgreedCostSetup Exit: success=" + success.ToString)
            Return success

        End Function
    End Class
End Namespace
