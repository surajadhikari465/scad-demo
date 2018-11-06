Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility
Imports log4net

Namespace WholeFoods.IRMA.Ordering.DataAccess
    Public Class OrderValidationDAO
        Private Shared CLASSTYPE As Type = System.Type.GetType("WholeFoods.IRMA.Ordering.DataAccess.OrderValidationDAO")
        Public Shared FAX As Integer = 1
        Public Shared ELECTRONIC_TRANSFER As Integer = 2
        Public Shared FILE_TYPE As Integer = 3
        Public Shared FTP_HOST As Integer = 4
        Public Shared FTP_USER As Integer = 5
        Public Shared FTP_PASSWORD As Integer = 6
        Public Shared EMAIL As Integer = 7
        Public Shared FAX_ORDER As Integer = 8
        Public Shared EMAIL_ORDER As Integer = 9
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


        Public Function GetVendorConfigDataset(ByVal orderId As Integer) As Hashtable

            logger.Debug("GetVendorConfigDataset Entry for OrderID = " & orderId.ToString)

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim reader As SqlDataReader = Nothing
            Dim results As Hashtable = New Hashtable
            'Dim strsql As String
            Dim currentParam As DBParam
            Dim paramList As New ArrayList

            'setup parameters for stored proc
            currentParam = New DBParam
            currentParam.Name = "OrderHeader_ID"
            currentParam.Value = orderId
            currentParam.Type = DBParamType.Int

            paramList.Add(currentParam)

            Try
                'strsql = "select fax, electronic_transfer, file_type, ftp_addr, ftp_user, ftp_password from vendor where vendor.vendor_id = "
                'strsql = strsql + " select vendor_id from orderheader where orderheader_id = " + CStr(orderId)
                'strsql = "SELECT Item_Key, LEFT(Identifier,12) AS Identifier FROM [ItemCatalog].[dbo].[ItemIdentifier] "
                'strsql = strsql + "  WHERE Deleted_Identifier = 0 AND Add_Identifier = 0 "

                'reader = factory.GetDataSet(strsql, VendorConfigDataset, strFillTableName)

                reader = factory.GetStoredProcedureDataReader("GetOrderVendorConfig", paramList)

                If reader.Read Then
                    results.Add(FAX, getStringValue(reader.GetValue((reader.GetOrdinal("fax")))))
                    results.Add(ELECTRONIC_TRANSFER, getStringValue(reader.GetValue((reader.GetOrdinal("electronic_transfer")))))
                    results.Add(FILE_TYPE, getStringValue(reader.GetValue((reader.GetOrdinal("file_type")))))
                    results.Add(FTP_HOST, getStringValue(reader.GetValue((reader.GetOrdinal("ftp_addr")))))
                    results.Add(FTP_USER, getStringValue(reader.GetValue((reader.GetOrdinal("ftp_user")))))
                    results.Add(FTP_PASSWORD, getStringValue(reader.GetValue((reader.GetOrdinal("ftp_password")))))
                    results.Add(EMAIL, getStringValue(reader.GetValue((reader.GetOrdinal("email")))))
                    results.Add(FAX_ORDER, getStringValue(reader.GetValue((reader.GetOrdinal("fax_order")))))
                    results.Add(EMAIL_ORDER, getStringValue(reader.GetValue((reader.GetOrdinal("email_order")))))
                End If

            Catch ex As Exception

                'TODO handle exception

                Throw ex

            End Try

            Return results
            logger.Debug("GetVendorConfigDataset Exit")

        End Function

        Public Shared Function getStringValue(ByVal myObject As Object) As String
            logger.Debug("getStringValue Entry")
            If (myObject Is DBNull.Value) Then
                Return ""
            Else
                Return CStr(myObject)
            End If
            logger.Debug("getStringValue Exit")
        End Function

    End Class
End Namespace
