Imports Infragistics.Excel
Imports System.Data
Imports System.Data.SqlClient
Imports Infragistics.WebUI.UltraWebGrid.ExcelExport

Partial Class UserInterface_ItemRequest_ItemProcessed
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        UltraWebGrid1.Height = Unit.Empty
        UltraWebGrid1.Width = Unit.Empty
        UpdateMenuLinks()

    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click

        'UltraWebGrid1.DataBind()
        Dim book As Workbook = New Workbook()
        book.Worksheets.Add("New")

        UltraWebGridExcelExporter1.Export(UltraWebGrid1, book)
        'BIFF8Writer.WriteWorkbookToStream(book, Response.OutputStream)
        book.Save(Response.OutputStream)

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
