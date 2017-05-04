'********************************************************************************************************************************************************************************
'clsRipeImport Summary
'The middle layer class that calls function in RipeDAO for RipeUI

'UPDATED_BY         UPDATED_DATE        UPDATED_FUNCTION_NAME               UPDATION_SUMMARY
'--------------------------------------------------------------------------------------------
' vayals            12/14/09            Ripe_LoadRipeLocations              Calls function in RipeDAO
' vayals            12/14/09            Ripe_LoadCombo                      Calls function in RipeDAO
' vayals            12/14/09            Ripe_LoadRipeZones                  Calls function in RipeDAO
' vayals            12/14/09            Ripe_CheckforExistingDistributions  Calls function in RipeDAO
' vayals            12/14/09            Ripe_CheckForImportErrors           Calls function in RipeDAO
' vayals            12/14/09            Ripe_ImportOrder                    Calls function in RipeDAO
' vayals            12/14/09            Ripe_GetRipeLocationStoreNo         Calls function in RipeDAO
' vayals            12/14/09            Ripe_GetRipeCustomerStoreNo         Calls function in RipeDAO
' vayals            12/14/09            Ripe_RetreiveStoreVendorID          Calls function in RipeDAO
' vayals            12/14/09            Ripe_SQLOpenRecordSet               Calls function in RipeDAO
' vayals            12/14/09            Ripe_GetDataReader                  Calls function in RipeDAO
' vayals            12/14/09            Ripe_SystemDateTime                 Calls function in RipeDAO
'********************************************************************************************************************************************************************************

Imports Infragistics.Win.UltraWinDataSource
Imports Infragistics.Shared
Imports Infragistics.Win
Imports Infragistics.Win.UltraWinGrid
Imports log4net
Imports System.Configuration
Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.IRMA.Common
Imports WholeFoods.IRMA.Common.BusinessLogic
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.Tools.BusinessLogic
Imports WholeFoods.Utility.Encryption
Imports WholeFoods.IRMA.Tools.DataAccess



Namespace WholeFoods.IRMA.Tools.BusinessLogic
    Public Class clsRipeImport
        Public Event PrintingInvoice(ByRef iInvoiceCnt As Short)

        Public Sub Ripe_LoadRipeLocations(ByRef cmbComboBox As System.Windows.Forms.ComboBox, Optional ByVal sUserName As String = "")
            Try
                Dim paramList As New DBParamList()
                Dim objDaoRipe As New RipeDAO
                paramList.Add(New DBParam("UserName", DBParamType.String, sUserName))
                Call objDaoRipe.Ripe_LoadCombo(cmbComboBox, "GetRipeLocations", "LocationName", "LocationID", paramList)
                paramList = Nothing

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Public Sub Ripe_LoadRipeZones(ByRef cmbComboBox As System.Windows.Forms.ComboBox, Optional ByVal locationID As Short = -1)
            Try
                Call Ripe_LoadCombo(cmbComboBox, "GetRipeZones", "ZoneName", "ZoneID")

            Catch ex As Exception
                Throw ex
            End Try
        End Sub

        Private Function Ripe_LoadCombo(ByRef cbo As System.Windows.Forms.ComboBox, ByVal StoredProcedure As String, ByVal DataTextField As String, ByVal DataValueField As String) As Boolean
            Try
                Dim objDaoRipe As New RipeDAO
                Return objDaoRipe.Ripe_LoadCombo(cbo, StoredProcedure, DataTextField, DataValueField, Nothing)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function Ripe_CheckforExistingDistributions(ByRef sDistDate As String, ByRef lRipeLocationID As Integer, ByRef sSelectedCust() As String) As Array
            Try
                Dim objDaoRipe As New RipeDAO
                Return objDaoRipe.Ripe_CheckforExistingDistributions(sDistDate, lRipeLocationID, sSelectedCust)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function Ripe_CheckForImportErrors(ByRef sDistribution As String, ByRef lFromStore As Integer, ByRef lToStore As Integer) As Boolean
            Try
                Dim objDaoRipe As New RipeDAO
                objDaoRipe.Ripe_CheckForImportErrors(sDistribution, lFromStore, lToStore)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function Ripe_ImportOrder(ByRef sDistribution As String, ByRef lFromStore As Integer, ByRef lToStore As Integer, ByRef lUserID As Integer, ByRef sImportDateTime As String) As Integer
            Try
                Dim objDaoRipe As New RipeDAO
                Ripe_ImportOrder = objDaoRipe.Ripe_ImportOrder(sDistribution, lFromStore, lToStore, lUserID, sImportDateTime)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function Ripe_SQLOpenRecordSet(ByRef sCall As String, ByRef param3 As Integer, ByRef Param4 As String, ByRef Param1 As Integer, ByRef Param2 As Integer, Optional ByRef bValidateLogon As Boolean = True) As ArrayList
            Try
                Dim objDaoRipe As New RipeDAO
                Ripe_SQLOpenRecordSet = objDaoRipe.Ripe_SQLOpenRecordSet(sCall, param3, Param4, Param1, Param2, bValidateLogon)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        Public Function Ripe_SystemDateTime(Optional ByRef bDateOnly As Boolean = False) As Date
            Try
                Dim objDaoRipe As New RipeDAO
                Ripe_SystemDateTime = objDaoRipe.Ripe_SystemDateTime(bDateOnly)
            Catch ex As Exception
                Throw ex
            End Try
        End Function


        Public Function Ripe_GetImportedOrders(ByRef strImportDateTime As String) As String
            Try
                Dim objDaoRipe As New RipeDAO
                Ripe_GetImportedOrders = objDaoRipe.Ripe_GetImportedOrders(strImportDateTime)
            Catch ex As Exception
                Throw ex
            End Try
        End Function

    End Class
End Namespace
