Imports Infragistics.WebUI
Imports Infragistics.Excel
Partial Class UserInterface_ItemRequest_ItemRejected
    Inherits System.Web.UI.Page

    Protected Sub UltraGrid1_InitializeRow(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles UltraWebGrid1.InitializeRow

        Dim ReProcessCell As Infragistics.WebUI.UltraWebGrid.UltraGridCell

        Dim RequestId As Integer = 0
        Dim StatusParam As String = String.Empty
        Dim Item_Desc As String = String.Empty
        Dim UPC As String = String.Empty

        If Not Request.QueryString("Status") Is Nothing Then
            StatusParam = "&status=" & Request.QueryString("status")
        End If

        ReProcessCell = e.Row.Cells.FromKey("ReProcess")
        RequestId = e.Row.Cells.FromKey("ItemRequest_ID").Value
        Item_Desc = "&I=" & e.Row.Cells.FromKey("Item_Description").Value
        UPC = "&U=" & e.Row.Cells.FromKey("Identifier").Value

        ReProcessCell.Value = "<a onMouseOver='window.status=""Re-Process a Store Special for this item""; return true;' href='ItemRejected.aspx?action=ReProcess&value=" & RequestId.ToString() & StatusParam & "'>Re-Process</a>"

    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click

        'UltraWebGrid1.DataBind()
        Dim book As Workbook = New Workbook()
        book.Worksheets.Add("New")
        UltraWebGrid1.Columns.FromKey("ReProcess").Hidden = True
        UltraWebGridExcelExporter1.Export(UltraWebGrid1, book)
        'BIFF8Writer.WriteWorkbookToStream(book, Response.OutputStream)
        book.Save(Response.OutputStream)
        UltraWebGrid1.Columns.FromKey("ReProcess").Hidden = False

    End Sub
    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Request.QueryString("action") Is Nothing Then
            Try
                ProcessActions()
            Catch ex As Exception
                MsgLabel.Text = ex.Message
            End Try
        End If

        If Not Request.QueryString("status") Is Nothing Then
            Select Case Request.QueryString("status")
                Case "4"
                    UltraWebGrid1.Columns.FromKey("ReProcess").Hidden = False
                    Page.Title = "Rejected Item Requests"
            End Select

        End If
        UpdateMenuLinks()
    End Sub

    Protected Sub UltraGrid1_InitalizeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles UltraWebGrid1.InitializeLayout

        UltraWebGrid1.Columns.FromKey("ReProcess").Width = 70

        Dim backColor As System.Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#004000")
        Dim AltColor As System.Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#C0FFC0")

        UltraWebGrid1.Font.Size = 12
        UltraWebGrid1.Font.Name = "Tahoma"

        With UltraWebGrid1.DisplayLayout
            .Pager.AllowPaging = True
            .Pager.StyleMode = UltraWebGrid.PagerStyleMode.CustomLabels
            .Pager.Pattern = "[currentpageindex] of [pagecount] : [page:first:First] [prev] [next] [page:last:Last]"
            .Pager.PageSize = 50
            .Bands(0).Columns(0).CellStyle.HorizontalAlign = HorizontalAlign.Center
            .Bands(0).AllowAdd = UltraWebGrid.AllowAddNew.No
            .Bands(0).AllowDelete = UltraWebGrid.AllowDelete.No
            .Bands(0).AllowUpdate = UltraWebGrid.AllowUpdate.No
            .Bands(0).RowAlternateStyle.BackColor = AltColor
            .Bands(0).HeaderStyle.BackColor = backColor
            .Bands(0).HeaderStyle.ForeColor = Drawing.Color.White
            .ColWidthDefault = Nothing
            .HeaderStyleDefault.BackColor = backColor
            .HeaderStyleDefault.ForeColor = Drawing.Color.White
            .RowAlternateStyleDefault.BackColor = AltColor
        End With

    End Sub

    Protected Sub ResetLabelMessage()
        MsgLabel.Text = String.Empty
        MsgLabel.ForeColor = Drawing.Color.Black
    End Sub

    Protected Sub ProcessActions()
        Dim value As String = String.Empty
        value = Request.QueryString("value")
        Select Case Request.QueryString("action").ToLower()
            Case "reprocess"
                ReProcessStoreSpecial(value)
        End Select
    End Sub

    Protected Sub ReProcessStoreSpecial(ByRef value As String)
        Dim df As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim paramList As ArrayList = New ArrayList
        Try

            ResetLabelMessage()

            currentParam = New DBParam
            currentParam.Name = "RequestId"
            currentParam.Value = CType(value, Integer)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            df.ExecuteStoredProcedure("SLIM_ReProcessItemRequest", paramList)

        Catch ex As Exception
            Throw ex
        End Try


    End Sub

    Protected Sub UpdateMenuLinks()
        If Not Session("Store_No") > 0 Then
            Master.HideMenuLinks("ISS", "ISSNew", False)
            Master.HideMenuLinks("ItemRequest", "NewItem", False)
        Else
            Master.HideMenuLinks("ISS", "ISSNew", True)
            Master.HideMenuLinks("ItemRequest", "NewItem", True)
        End If
    End Sub
End Class
