Imports Microsoft.VisualBasic

Public Class GetHierarchyDataSet



    ''' <summary>
    ''' Get Web Query Search Results
    ''' </summary>
    Private Function GetWebQueryData(ByVal QString As NameValueCollection) As DataTable
        Dim dt As DataTable
        Dim da As New WebQuerySearchTableAdapters.GetItemWebQueryTableAdapter
        ' ****************************************************
        dt = da.GetData(QString.Item("u"), QString.Item("d"), CheckForEmptyString(QString.Item("t")), _
        CheckForEmptyString(QString.Item("s")), CheckForEmptyString(QString.Item("c")), _
        CheckForEmptyString(QString.Item("3")), CheckForEmptyString(QString.Item("4")), _
        CheckForEmptyString(QString.Item("b")), CheckForEmptyString(QString.Item("v")), _
        QString.Item("x"), _
        QString.Item("tn"), QString.Item("stn"), QString.Item("cn"), QString.Item("l3n"), _
        QString.Item("l4n"), QString.Item("bn"), QString.Item("vn"), QString.Item("j"))
        Return dt
    End Function

    ''' <summary>
    ''' Get the Alternate Jurisdiction Data
    ''' </summary>
    Private Function GetItemOverrideData(ByVal ItemKeyList As String) As DataTable
        Dim dt As New DataTable
        Dim da As New IrmaItemTableAdapters.SLIM_GetItemOverrideTableAdapter 
        dt = da.GetItemOverride(ItemKeyList)
        Return dt
    End Function

    ''' <summary>
    ''' Create Relation for two DataTables
    ''' </summary>
    Private Function CreateRelation(ByVal dt1 As DataTable, ByVal dt2 As DataTable) As DataSet
        Dim ds As New DataSet
        ds.Tables.Add(dt1)
        ds.Tables.Add(dt2)
        ' *** Create Relation/Constraint ***
        ds.Relations.Add("AZ", ds.Tables(0).Columns("Item_Key"), ds.Tables(1).Columns("Item_Key"))
        Return ds
    End Function

    ''' <summary>
    ''' Return DataSet with two tables - Relation - for hierarchical display in UltrawebGrid
    ''' </summary>
    Public Function GetHierarchicalData(ByVal QueryStringCol As NameValueCollection) As DataSet
        Dim ds As New DataSet
        Dim dt1 As New DataTable
        Dim dt2 As New DataTable
        Dim al As String

        ' *** WebQuerySearch Data ***
        dt1 = GetWebQueryData(QueryStringCol)
        ' *** Get Item Keys Data ***
        al = GetItemKeyList(dt1)
        ' *** ItemOverride Data ***
        dt2 = GetItemOverrideData(al)
        ' *** Create Relation ***
        ds = CreateRelation(dt1, dt2)

        ' *** Return Full DataSet ***
        Return ds

    End Function

    ''' <summary>
    ''' Return List of Keys from GetItemWebQuery
    ''' </summary>
    Private Function GetItemKeyList(ByVal ItemWebQueryTable As DataTable) As String
        Dim al As New StringBuilder
        Dim dr As DataRow
        Dim dt As New DataTable
        dt = ItemWebQueryTable
        For Each dr In dt.Rows
            al.Append(dr.Item("Item_Key") & "|")
        Next
        If al.Length > 0 Then
            al = al.Remove(al.Length - 1, 1)
        End If
        Return al.ToString
    End Function


    Private Function CheckForEmptyString(ByVal querystring As String) As Integer
        Dim param As Integer
        If String.IsNullOrEmpty(querystring) Then
            param = Nothing
        Else
            param = CInt(querystring)
        End If
        Return param
    End Function

    Public Sub New()

    End Sub
End Class
