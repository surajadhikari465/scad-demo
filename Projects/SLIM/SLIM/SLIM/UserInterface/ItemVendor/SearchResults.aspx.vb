Imports Infragistics.WebUI

Partial Class UserInterface_ItemVendor_SearchResults
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub SqlDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SqlDataSource1.Selecting
        If Request.QueryString.Count = 0 Then
            e.Cancel = True
        End If
    End Sub

    Protected Sub UltraWebGrid1_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles UltraWebGrid1.InitializeLayout

        With UltraWebGrid1.DisplayLayout
            .Pager.AllowPaging = True
            .Pager.StyleMode = Infragistics.WebUI.UltraWebGrid.PagerStyleMode.PrevNext
            .Pager.PageSize = 150
            .Bands(0).Columns(0).Width = 50
            .Bands(0).Columns(0).CellStyle.HorizontalAlign = HorizontalAlign.Center
            .Bands(0).AllowAdd = UltraWebGrid.AllowAddNew.No
            .Bands(0).AllowDelete = UltraWebGrid.AllowDelete.No
            .Bands(0).AllowUpdate = UltraWebGrid.AllowUpdate.No
        End With


    End Sub




    Protected Sub UltraWebGrid1_InitializeRow(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles UltraWebGrid1.InitializeRow

        Dim url As String = String.Empty
        Dim upc As String = String.Empty
        Dim itemkey As String = String.Empty
        Dim Desc As String = String.Empty
        Dim BrandId As String = String.Empty
        Dim BrandName As String = String.Empty


        With e.Row.Cells
            itemkey = .FromKey("Item_Key").Value.ToString()
            upc = .FromKey("Identifier").Value.ToString()
            Desc = .FromKey("item_description").Value.ToString()
            BrandId = .FromKey("Brand_Id").Value.ToString()
            BrandName = .FromKey("brand_name").Value.ToString()
        End With

     
        url = "ItemVendors.aspx?"
        url += "i=" & itemkey & "&"
        url += "u=" & upc & "&"
        url += "d=" & Desc

        e.Row.Cells(0).Value = "<a href='" & url & "' onMouseOver='window.status=""View Items for UPC: " & upc & """; return true;'>Select</a>"


    End Sub


End Class
