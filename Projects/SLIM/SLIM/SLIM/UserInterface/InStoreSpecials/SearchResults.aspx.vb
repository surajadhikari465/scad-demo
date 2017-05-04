Imports Infragistics.WebUI

Partial Class UserInterface_InStoreSpecials_SearchResult
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Page.IsPostBack Then
            SetChecked()
        End If
    End Sub

    Protected Sub UltraWebGrid1_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles UltraWebGrid1.InitializeLayout
        With UltraWebGrid1.DisplayLayout
            .Pager.AllowPaging = True
            .Pager.StyleMode = Infragistics.WebUI.UltraWebGrid.PagerStyleMode.PrevNext
            .Pager.PageSize = 50
            .Bands(0).Columns(0).Width = 50
            .Bands(0).Columns(0).CellStyle.HorizontalAlign = HorizontalAlign.Center
            .Bands(0).AllowAdd = UltraWebGrid.AllowAddNew.No
            .Bands(0).AllowDelete = UltraWebGrid.AllowDelete.No
            .Bands(0).AllowUpdate = UltraWebGrid.AllowUpdate.No
        End With
    End Sub

    Protected Sub SqlDataSource1_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles SqlDataSource1.Selected
        UltraWebGrid1.Visible = Not (e.AffectedRows = 0)
        Label_Message.Visible = (e.AffectedRows = 0)
        Label_RowCount.Text = "Returned Rows: " & e.AffectedRows.ToString
        If Not e.AffectedRows = 0 Then
            Label_RowCount.Visible = True
        End If
    End Sub

    Protected Sub Button_Submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Submit.Click
        Dim row As UltraWebGrid.UltraGridRow
        Dim sItemList As String = ""
        Dim sIdentifierIDList As String = ""

        For Each row In UltraWebGrid1.Rows
            If row.Cells(0).Value Then
                If sItemList = "" Then
                    sItemList = row.Cells(1).Value
                    sIdentifierIDList = row.Cells(14).Value
                Else
                    sItemList = sItemList & "|" & row.Cells(1).Value
                    sIdentifierIDList = sIdentifierIDList & "|" & row.Cells(14).Value
                End If
            End If
        Next

        Session("ISSItemKeyList") = sItemList
        Session("ISSIdentifierIDList") = sIdentifierIDList
        Response.Redirect("StoreSpecials.aspx")
    End Sub

    Protected Sub CheckBox_All_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_All.CheckedChanged
        Dim row As UltraWebGrid.UltraGridRow

        For Each row In UltraWebGrid1.Rows
            row.Cells(0).Value = CheckBox_All.Checked
        Next

        GetChecked()
    End Sub

    Private Sub GetChecked()
        Dim rowEnum As UltraWebGrid.UltraGridRowsEnumerator = UltraWebGrid1.Bands(0).GetRowsEnumerator()
        Dim dr As New UltraWebGrid.UltraGridRow
        Dim itemList As New ArrayList

        rowEnum.Reset()
        While rowEnum.MoveNext
            dr = CType(rowEnum.Current(), UltraWebGrid.UltraGridRow)
            If dr.Cells(0).Value = True Then
                itemList.Add(dr.Cells(1).ToString)
            End If
        End While

        If itemList.Count <> 0 Then
            ViewState("ItemList") = itemList
        Else
            ViewState("ItemList") = Nothing
        End If
    End Sub

    Private Sub SetChecked()
        Dim rowEnum As UltraWebGrid.UltraGridRowsEnumerator = UltraWebGrid1.Bands(0).GetRowsEnumerator()
        Dim dr As New UltraWebGrid.UltraGridRow

        If ViewState("ItemList") IsNot Nothing Then
            Dim itemList As New ArrayList

            itemList = CType(ViewState("ItemList"), ArrayList)
            rowEnum.Reset()

            While rowEnum.MoveNext
                dr = CType(rowEnum.Current(), UltraWebGrid.UltraGridRow)
                If itemList.Contains(dr.Cells(1).ToString) Then
                    dr.Cells(0).Value = True
                End If
            End While
        End If
    End Sub
End Class
