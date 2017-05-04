Imports Microsoft.VisualBasic

Public Class ItemWebQuery

    Public Shared Function GetQueryResults(ByVal upc As String, ByVal desc As String, ByVal dept As Int32, ByVal brand As Int32) As DataTable
        Dim ht As New Hashtable
        Dim ds As New DataSet
        ht.Item("Identifier") = upc
        ht.Item("Item_Description") = desc
        ht.Item("SubTeam_No") = dept
        ht.Item("Brand_ID") = brand
        ds = ItemSearch.GetItemWebQuery(ht)
        Return ds.Tables(0)
    End Function

    Public Shared Function GetItemDetails(ByVal item_key As Integer, ByVal storeJurisdictionId As Integer) As DataRow
        Return ItemSearch.GetItemDetails(item_key, storeJurisdictionId)
    End Function

End Class
