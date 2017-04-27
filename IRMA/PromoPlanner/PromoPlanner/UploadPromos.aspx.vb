Imports System.IO
Imports System.Data
Imports System.Data.OleDb
Imports System.Drawing


Partial Class UploadPromos
    Inherits System.Web.UI.Page

    Protected Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click

        Try
            ClearMessage()
            ' if Upload Control Has A File, 
            If FileUpload1.HasFile Then
                'Get the Path.
                Dim filePath As String = FileUpload1.PostedFile.FileName
                ' Validate the document extension.
                If Not filePath.Substring(filePath.LastIndexOf("."), 4).ToLower() = ".xls" Then
                    Throw New Exception("Upload templates for PromoPlanner must be in Excel 2003 format and have a .XLS extension")
                End If
                ' Generate a new GUID to use as the temporary file name.
                Dim guid As System.Guid = System.Guid.NewGuid()
                ' Create temporary file name
                Dim newFileName As String = "temp-" & guid.ToString() & ".xls"
                ' Save the uploaded file to the server with the temporary name.
                FileUpload1.PostedFile.SaveAs(Server.MapPath(".\") & newFileName)
                WebGrid.DataSource = LoadXLSIntoDataset(Server.MapPath(".\") & newFileName)
                WebGrid.DataKeyField = "Id"
                WebGrid.DataBind()

                'Delete temporary file from server.
                File.Delete(Server.MapPath(".\") & newFileName)



            End If

        Catch Ex As Exception
            lblMessage.Text = Ex.Message()
            lblMessage.ForeColor = Color.Red
        Finally
        End Try

    End Sub

    Private Sub ClearMessage()
        lblMessage.Text = ""
    End Sub

    Private Function LoadXLSIntoDataset(ByVal path As String) As DataTable

        Dim DS As DataSet
        Dim MyCommand As OleDbDataAdapter
        Dim MyConnection As OleDbConnection

        MyConnection = New OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0; data source=" & path & "; Extended Properties=Excel 8.0;")


        MyCommand = New System.Data.OleDb.OleDbDataAdapter("select * from [Sheet1$]", MyConnection)
        DS = New System.Data.DataSet
        MyCommand.Fill(DS)
        MyConnection.Close()

        DS.Tables(0).Columns.Add(New DataColumn("id", GetType(Integer)))
        Dim cnt As Integer = 0
        For Each dr As DataRow In DS.Tables(0).Rows
            dr("id") = cnt
            cnt += 1
        Next


        Return DS.Tables(0)

    End Function
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session("storeno") Is Nothing Then Response.Redirect("~/")
    End Sub
End Class
