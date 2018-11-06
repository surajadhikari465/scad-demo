Imports Infragistics.WebUI

Partial Class UserInterface_ItemAuthorizations_SearchResults
    Inherits ItemAuthorizationPage

#Region "SqlDataSource Event Handlers"

    Protected Sub RowCount(ByVal AffectedRows As Integer)
        Dim rowsFound As Boolean = AffectedRows <> 0
        UltraWebGrid1.Visible = rowsFound
        gvResults.Visible = rowsFound
        Label_Message.Visible = Not rowsFound

        Label_RowCount.Text = "Returned Rows: " & AffectedRows.ToString

        If Not AffectedRows = 0 Then
            Label_RowCount.Visible = True
        End If
    End Sub

    Protected Sub SqlDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SqlDataSource1.Selecting
        If Request.QueryString.Count = 0 Then
            e.Cancel = True
        End If
    End Sub

#End Region

#Region "UltraWebGrid Event Handlers"

    Protected Sub UltraWebGrid1_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles UltraWebGrid1.InitializeLayout
        With UltraWebGrid1.DisplayLayout
            .Pager.AllowPaging = True
            .Pager.StyleMode = Infragistics.WebUI.UltraWebGrid.PagerStyleMode.PrevNext
            .Pager.PageSize = 150
            .Bands(1).Columns.FromKey("Item_Key").Hidden = True
            .Bands(0).Columns.FromKey("Item_Key").Hidden = True
            .Bands(0).Columns.FromKey("Brand_ID").Hidden = True
            .Bands(0).Columns.FromKey("StoreJurisdictionID").Hidden = True
            .Bands(0).Columns.FromKey("Package_desc1").Hidden = True
            .Bands(1).Columns.FromKey("Package_desc1").Hidden = True
            .Bands(0).Columns.FromKey("Category_Name").Hidden = True
            .Bands(0).Columns(0).Width = 50
            .Bands(0).Columns(0).CellStyle.HorizontalAlign = HorizontalAlign.Center
            .Bands(1).Columns(0).Width = 50
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


        If Not e.Row.HasParent Then
            With e.Row.Cells
                itemkey = .FromKey("Item_Key").Value.ToString()
                upc = .FromKey("Identifier").Value.ToString()
                Desc = .FromKey("item_description").Value.ToString()
                BrandId = .FromKey("Brand_Id").Value.ToString()
                BrandName = .FromKey("brand_name").Value.ToString()
            End With
        Else
            With e.Row.Cells
                itemkey = .FromKey("Item_Key").Value.ToString()
                Desc = .FromKey("item_description").Value.ToString()
            End With
            With e.Row.ParentRow.Cells
                upc = .FromKey("Identifier").Value.ToString()
                BrandId = .FromKey("Brand_Id").Value.ToString()
                BrandName = .FromKey("brand_name").Value.ToString()
            End With
        End If


        url = "Authorizations.aspx?"
        url += "i=" & itemkey & "&"
        url += "u=" & upc & "&"
        url += "d=" & Desc

        e.Row.Cells(0).Value = "<a href='" & url & "' onMouseOver='window.status=""View Item Authorization for UPC: " & upc & """; return true;'>Select</a>"
    End Sub

#End Region

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack AndAlso _isMobileDevice Then
            mvResults.ActiveViewIndex = 1
        End If
        ' ************************ Create DataSets to Feed WebGrid **************************
        Dim QueryStringCol As NameValueCollection = Request.QueryString
        Dim ghd As New GetHierarchyDataSet()
        Dim ds As DataSet
        ds = ghd.GetHierarchicalData(QueryStringCol)
        RowCount(ds.Tables(0).Rows.Count)
        UltraWebGrid1.DataSource = ds
        UltraWebGrid1.DataBind()
        ' **********************************************************************************
    End Sub

End Class
