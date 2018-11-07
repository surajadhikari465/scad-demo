Imports Infragistics.WebUI
Imports Infragistics.Excel

Partial Class UserInterface_WebQuery_SearchResults
    Inherits WebQueryPage

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

    Protected Sub HideColumn(ByVal column As UltraWebGrid.UltraGridColumn)
        column.ServerOnly = True
        column.Hidden = True
    End Sub

    Protected Sub UltraWebGrid1_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles UltraWebGrid1.InitializeLayout

        With UltraWebGrid1.DisplayLayout
            .Bands(0).Columns(0).Width = 50
            .Bands(1).Columns(0).Width = 50
            .Bands(0).Columns(0).CellStyle.HorizontalAlign = HorizontalAlign.Center
            HideColumn(.Bands(0).Columns.FromKey("Item_Key"))
            HideColumn(.Bands(1).Columns.FromKey("Item_Key"))
            HideColumn(.Bands(0).Columns.FromKey("Brand_ID"))
            HideColumn(.Bands(0).Columns.FromKey("StoreJurisdictionID"))
            HideColumn(.Bands(1).Columns.FromKey("StoreJurisdictionID"))
            .Bands(0).AllowAdd = UltraWebGrid.AllowAddNew.No
            .Bands(0).AllowDelete = UltraWebGrid.AllowDelete.No
            .Bands(0).AllowUpdate = UltraWebGrid.AllowUpdate.No
            .Pager.PagerAppearance = UltraWebGrid.PagerAppearance.Both
            .Pager.PageSize = 60
            .Pager.StyleMode = UltraWebGrid.PagerStyleMode.CustomLabels
            .Pager.Pattern = "[currentpageindex] of [pagecount] : [page:first:First] [prev] [next] [page:last:Last]"
        End With
    End Sub

    Protected Sub UltraWebGrid1_InitializeRow(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles UltraWebGrid1.InitializeRow
        Dim upc As String = String.Empty
        Dim url As String = String.Empty
        Dim itemkey As String = String.Empty
        Dim BrandId As String = String.Empty
        Dim BrandName As String = String.Empty
        Dim storeJurisdictionId As String = String.Empty
        Dim cellCollection As UltraWebGrid.CellsCollection

        itemkey = e.Row.Cells.FromKey("Item_Key").Value.ToString()

        If IsNothing(e.Row.Cells.FromKey("StoreJurisdictionID").Value) = False Then
            storeJurisdictionId = e.Row.Cells.FromKey("StoreJurisdictionID").Value.ToString()
        End If


        If Not e.Row.HasParent Then
            cellCollection = e.Row.Cells
        Else
            cellCollection = e.Row.ParentRow.Cells
        End If

        With cellCollection
            upc = .FromKey("Identifier").Value.ToString()
            BrandId = .FromKey("Brand_Id").Value.ToString()
            BrandName = .FromKey("brand_name").Value.ToString()
        End With

        url = String.Format("SearchItemDetail.aspx?i={0}&j={1}", itemkey, storeJurisdictionId)

        e.Row.Cells(0).Value = String.Format("<a href=""{0}"" onMouseOver='window.status=""View Item Detail for UPC: {1}""; return true;'>Select</a>", url, upc)

        url = String.Format("SearchResults.aspx?u=*&d=*&t=0&s=0&c=0&3=0&4=0&b={0}&v=0&x=*&tn=&stn=&cn=&l3n=&l4n=&bn=&vn=", BrandId)

        If Not e.Row.HasParent Then
            e.Row.Cells.FromKey("brand_name").Value = BrandName
            e.Row.Cells.FromKey("brand_name").Title = String.Format("View Items from brand: {0}", BrandName)
            e.Row.Cells.FromKey("brand_name").TargetURL = url
        End If
    End Sub

#End Region

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            mvGrids.ActiveViewIndex = IIf(_isMobileDevice, 1, 0)
            ' **********************************************************************************
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

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        Dim book As Workbook = New Workbook()
        book.Worksheets.Add("New")
        UltraWebGrid1.DisplayLayout.Pager.AllowPaging = False
        UltraWebGrid1.DataBind()
        UltraWebGrid1.Columns(0).Hidden = True
        UltraWebGrid1.Bands(1).Columns(0).Hidden = True
        UltraWebGridExcelExporter1.Export(UltraWebGrid1, book)
        'BIFF8Writer.WriteWorkbookToStream(book, Response.OutputStream)
        book.Save(Response.OutputStream)
        UltraWebGrid1.Bands(1).Columns(0).Hidden = False
        UltraWebGrid1.Columns(0).Hidden = False
        UltraWebGrid1.DisplayLayout.Pager.AllowPaging = True
    End Sub

End Class
