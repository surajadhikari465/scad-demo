Imports Infragistics.WebUI
Imports Infragistics.Excel
Imports System.Web.UI

Partial Class UserInterface_VendorRequest_VendorProcessed
    Inherits System.Web.UI.Page

    Protected Sub DataSource_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles SqlDataSource1.Selected
        UltraWebGrid1.Visible = Not e.AffectedRows = 0
        Label_Message.Visible = e.AffectedRows = 0
        Label_Message.Text = "There are currently no processed Vendor requests."
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        Dim book As Workbook = New Workbook()
        book.Worksheets.Add("Processed Vendors")

        UltraWebGridExcelExporter1.Export(UltraWebGrid1, book)
        'BIFF8Writer.WriteWorkbookToStream(book, Response.OutputStream)
        book.Save(Response.OutputStream)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' ****** Check Permission **************
        If Not IsPostBack Then
            If Not Session("VendorRequest") = True And Not Session("AccessLevel") = 3 Then
                Response.Redirect("~/AccessDenied.aspx", True)
            End If
        End If
        ' *****************************
        UpdateMenuLinks()
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
