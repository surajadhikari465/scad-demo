Imports Microsoft.Office.Interop
Imports System.IO


Partial Public Class Upload
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'TODO Check if User has permissions
        ErrorLabel.Visible = False
        ResultLabel.Visible = False
        UltraWebGrid1.Visible = False
        UltraWebGrid1.Bands(0).RowStyle.ForeColor = Drawing.Color.Black

        If Not Page.IsPostBack Then
            btnImport.Visible = True
            btnValidate.Visible = False
            btnUpload.Visible = False
            img1.ImageUrl = "App_Themes/Theme1/Images/number_17.jpg"
        End If

    End Sub

    Private Sub SetWebGridError(ByVal errorRow As Integer, ByVal errorColumn As Integer, ByVal Errormessage As String)

        If errorColumn = -1 Then
            With UltraWebGrid1.DisplayLayout
                 .Bands(0).RowStyle.ForeColor = Drawing.Color.Crimson
            End With
        Else
            With UltraWebGrid1.DisplayLayout
                .Rows(errorRow).Cells(errorColumn).Style.BackColor = Drawing.Color.LightGray
                .Rows(errorRow).Cells(errorColumn).Style.ForeColor = Drawing.Color.Crimson

            End With
        End If
    End Sub

    Private Sub SetWebGridSuccess()
        With UltraWebGrid1.DisplayLayout
            .Bands(0).RowStyle.ForeColor = Drawing.Color.Green
        End With
    End Sub

    Private Sub btnImport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnImport.Click
        Dim ExcelFile As String '= Server.MapPath("~/ExcelImport") & Session.SessionID & ".xlsx"
        Dim stFilePath As String
        Dim AppSetting As New BOAppSettings

        stFilePath = AppSetting.GetKeyValue("ImportPath")
        If stFilePath.Substring(Len(stFilePath) - 1, 1) <> "\" Then
            stFilePath = stFilePath & "\"
        End If

        If FileUpload1.HasFile Then
            ExcelFile = stFilePath & Session.SessionID & System.IO.Path.GetExtension(FileUpload1.FileName)
            Try
                FileUpload1.SaveAs(ExcelFile)
            Catch ex As Exception
                ErrorLabel.Visible = True
                ErrorLabel.Text = "Error in Upload - " & ex.Message
                Exit Sub
            End Try
        Else
            ErrorLabel.Visible = True
            ErrorLabel.Text = "Please Select a File to Upload"
            Exit Sub
        End If

        Dim ds As New DataSet("ExcelSheet")
        Dim az As New BOSpreadsheetImport(ExcelFile)

        Try
            Debug.WriteLine("Importing SpreadSheet")
            ds = az.GetAllExcelRows()
            ResultLabel.Visible = True
            ResultLabel.Text = "Spreadsheet Imported!"
        Catch ex As Exception
            Debug.WriteLine("Importing SpreadSheet -- {0} -- {1}")
            ErrorLabel.Visible = True
            ErrorLabel.Text = ex.Message
        End Try

        ' ***** Cache the Spreadsheet and FileName  *****
        Session.Add("Spreadsheet", ds)
        Session.Add("ExcelFileName", FileUpload1.FileName.ToString)
        Session.Add("IsImported", True)

        UltraWebGrid1.Visible = True
        UltraWebGrid1.DataSource = ds
        UltraWebGrid1.DataBind()

        btnImport.Visible = False
        btnValidate.Visible = True
        btnUpload.Visible = False
        trUpload.Visible = False
        img1.ImageUrl = "App_Themes/Theme1/Images/number_21.jpg"
        ' *** Clean up Excel File ***
        Try
            File.Delete(ExcelFile)
        Catch ex As Exception

        End Try

    End Sub

    Private Sub btnValidate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnValidate.Click
        Debug.WriteLine("Validating SpreadSheet")

        If CType(Session("IsImported"), Boolean) = False Then
            ErrorLabel.Visible = True
            ErrorLabel.Text = "Please Import Spreadsheet first"
            Exit Sub
        End If
        If Session("UserID") Is Nothing Or Session("ExcelFileName") Is Nothing Then
            ErrorLabel.Visible = True
            ErrorLabel.Text = "Could not find UserId - Cache expired or not logged in"
            Exit Sub
        End If
        Dim validate As New BOSpreadsheetValidation()
        With validate
            .ImportSpreadSheet = CType(Session("Spreadsheet"), DataSet)
            .FileName = CType(Session("ExcelFileName"), String)
            .UserID = CType(Session("UserID"), Int32)
        End With
        Try
            validate.ValidateSpreadSheet()
        Catch ex As Exception
            validate.IsValidated = False
        End Try
        UltraWebGrid1.Visible = True
        If validate.IsValidated = True Then
            Session.Add("IsValidated", True)
            Session.Remove("Spreadsheet")
            Session.Add("OrderList", validate.OrderList)
            ResultLabel.Visible = True
            ResultLabel.Text = "Spreadsheet Validated!"
            SetWebGridSuccess()

            btnImport.Visible = False
            trUpload.Visible = False
            btnValidate.Visible = False
            btnUpload.Visible = True
            img1.ImageUrl = "App_Themes/Theme1/Images/number_25.jpg"
        Else
            ErrorLabel.Visible = True
            ErrorLabel.Text = "Validation Not Successful! - " & validate.ErrorMessage
            SetWebGridError(validate.ErrorRowIndex, validate.ErrorColumnIndex, validate.ErrorMessage)

        End If

    End Sub

    Private Sub btnUpload_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpload.Click
        Debug.WriteLine("Uploading SpreadSheet")
        If CType(Session("IsValidated"), Boolean) = False Then
            ErrorLabel.Visible = True
            ErrorLabel.Text = "Please Validate Spreadsheet First"
            Exit Sub
        End If
        If Session("UserID") Is Nothing Then
            ErrorLabel.Visible = True
            ErrorLabel.Text = "Could not find UserId - Cache expired or not logged in"
            Exit Sub
        End If
        Dim upload As New BOSpreadsheetUpload()
        With upload
            .MyOrderList = CType(Session("OrderList"), List(Of OrderObject))
            .UserID = CType(Session("UserID"), Int32)
            .FileName = CType(Session("ExcelFileName"), String)
        End With
        Try
            upload.UploadSpreadSheet()
        Catch ex As Exception
            upload.IsUploaded = False
        End Try
        If upload.IsUploaded = True Then
            Session.Add("IsUploaded", True)
            Session.Remove("ExcelFileName")
            Session.Remove("OrderList")
            ResultLabel.Visible = True
            ResultLabel.Text = "Spreadsheet Uploaded!"
            btnUpload.Visible = False
            UltraWebGrid1.Visible = True
            UltraWebGrid1.DisplayLayout.ViewType = Infragistics.WebUI.UltraWebGrid.ViewType.Hierarchical
            UltraWebGrid1.DataSource = upload.GetUploadedDataSet
            UltraWebGrid1.DataBind()
        Else
            ErrorLabel.Visible = True
            ErrorLabel.Text = "Upload Not Successful!"
            Session.Remove("ExcelFileName")
            Session.Remove("OrderList")
        End If
        upload = Nothing
    End Sub

    Private Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        btnImport.Visible = True
        btnValidate.Visible = False
        btnUpload.Visible = False
        trUpload.Visible = True
        img1.ImageUrl = "App_Themes/Theme1/Images/number_17.jpg"
    End Sub
End Class