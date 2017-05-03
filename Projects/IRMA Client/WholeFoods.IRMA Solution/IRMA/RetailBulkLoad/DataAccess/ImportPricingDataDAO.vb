Option Strict Off
Imports System.Data.SqlClient
Imports System.Text
Imports System.Collections.Generic
Imports WholeFoods.IRMA.RetailBulkLoad.BusinessLogic
Imports WholeFoods.Utility.DataAccess

Namespace WholeFoods.IRMA.RetailBulkLoad.DataAccess
    Public Class ImportPricingDataDAO

        Public Shared Function GetItemInfoByIdentifier(ByVal Id As String) As Boolean

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim sql As String
            Dim cnt As Integer
            sql = "exec GetItemInfoByIdentifier '" & Id & "'"
            cnt = CBool(factory.ExecuteScalar(sql))
            Return cnt
        End Function

        Public Shared Function GetItemKeyByIdentifier(ByVal Id As String) As Integer

            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim sql As String
            Dim cnt As Integer
            sql = "exec GetItemInfoByIdentifier '" & Id & "'"
            cnt = (factory.ExecuteScalar(sql))
            Return cnt
        End Function

        Public Shared Function GetItemPriceInfo(ByVal itemKey As String, ByVal store As String) As Hashtable
            Dim factory As New DataFactory(DataFactory.ItemCatalog)
            Dim sql As String
            Dim currentPriceInfo As New Hashtable
            Dim read As SqlDataReader
            sql = "select Multiple, Price, POSPrice from Price where Store_No = " & store & " and Item_Key = " & _
            itemKey
            read = (factory.GetDataReader(sql))
            While read.Read()
                currentPriceInfo("Multiple") = read(0).ToString
                currentPriceInfo("Price") = read(1).ToString
                currentPriceInfo("POSPrice") = read(2).ToString
            End While
            read.Close()
            read = Nothing
            Return currentPriceInfo
        End Function
    End Class
End Namespace
