Imports System.IO
Imports System.Linq
Imports System.Net
Imports System.Text
Imports System.Web.Script.Serialization
Imports log4net
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.Planogram.BusinessLogic
Imports WholeFoods.IRMA.Planogram.DataAccess
Imports WholeFoods.IRMA.Pricing.DataAccess
Imports WholeFoods.Utility

Public Class PlanogramImport

    Private selectedFile As String
    Private planogramBusinessLogic As PlanogramBO = New PlanogramBO()
    Private validImportedPlanogramItems As List(Of PlanogramItemBO)
    Private storeNumberToBusinessUnit As Dictionary(Of Integer, Integer)

    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        TextBoxFilePath.Text = String.Empty
        LabelImportStatusValue.Text = String.Empty
        ButtonImport.Enabled = False
        ButtonSend.Enabled = False
        LabelImportStatusValue.Text = "No file selected."
    End Sub

    Private Sub PlanogramImport_Load(sender As Object, e As EventArgs) Handles Me.Load
        storeNumberToBusinessUnit = StoreListDAO.GetStoreNumberToBusinessUnitCollection()
    End Sub

    Private Sub ButtonSelectFile_Click(sender As Object, e As EventArgs) Handles ButtonSelectFile.Click
        SelectPlanogramFileForImport()
    End Sub

    Private Sub TextBoxFilePath_DoubleClick(sender As Object, e As EventArgs) Handles TextBoxFilePath.DoubleClick
        SelectPlanogramFileForImport()
    End Sub

    Private Sub SelectPlanogramFileForImport()
        OpenFileDialogSelectFile.InitialDirectory = ConfigurationServices.AppSettings("PlanogramLocalDir")

        OpenFileDialogSelectFile.ShowDialog()

        selectedFile = OpenFileDialogSelectFile.FileName()

        If String.IsNullOrEmpty(selectedFile) Then
            TextBoxFilePath.Text = String.Empty
            ButtonImport.Enabled = False
            ButtonSend.Enabled = False
        Else
            Dim isDatExtension As Boolean = selectedFile.EndsWith("dat", StringComparison.OrdinalIgnoreCase)

            If Not isDatExtension Then
                MessageBox.Show("Only .dat files can be imported.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
                TextBoxFilePath.Text = String.Empty
                ButtonImport.Enabled = False
                ButtonSend.Enabled = False
            Else
                TextBoxFilePath.Text = selectedFile
                ButtonImport.Enabled = True
                LabelImportStatusValue.Text = "Ready to import."
            End If
        End If
    End Sub

    Private Sub ButtonImport_Click(sender As Object, e As EventArgs) Handles ButtonImport.Click
        ButtonImport.Enabled = False
        ButtonSelectFile.Enabled = False
        Cursor = Cursors.WaitCursor

        Dim parsedPlanogramItems As List(Of PlanogramItemBO) = New List(Of PlanogramItemBO)
        Dim validIdentifiers As List(Of String) = New List(Of String)

        Try
            parsedPlanogramItems = planogramBusinessLogic.ParseFile(selectedFile)
            validIdentifiers = PlanogramDAO.GetValidPlanogramIdentifiers(parsedPlanogramItems)
        Catch ex As Exception
            logger.Info(String.Format("An error occurred while parsing the planogram file: {0}.  Error: {1}", selectedFile, ex.ToString()))
            MessageBox.Show(String.Format("An error occurred while parsing the planogram file: {0}", ex.Message))

            LabelImportStatusValue.Text = "Import error."
            ButtonSend.Enabled = False
            Exit Sub
        End Try

        validImportedPlanogramItems = parsedPlanogramItems.Where(Function(p) validIdentifiers.Contains(p.Identifier)).ToList()

        LabelImportStatusValue.Text = String.Format("Import successful.  Headers: {0} - Stores: {1} - Item Records: {2}",
                                                    planogramBusinessLogic.HeaderCount.ToString(), planogramBusinessLogic.StoreCount.ToString(), planogramBusinessLogic.ItemCount.ToString())

        ButtonSend.Enabled = True
        ButtonImport.Enabled = True
        ButtonSelectFile.Enabled = True
        Cursor = Cursors.Default
    End Sub

    Private Sub ButtonSend_Click(sender As Object, e As EventArgs) Handles ButtonSend.Click
        Dim message As String = String.Format("Items which are not validated or authorized will be excluded from the print request.{0}{0}", Environment.NewLine)
        Dim confirm As String = String.Format("Continue to send print batch requests for the currently imported planogram file with an effective date of {0}?", DateTimePickerEffectiveDate.Value.ToShortDateString())
        Dim errorsForUser As List(Of String) = New List(Of String)

        Dim result As DialogResult = MessageBox.Show(String.Format("{0}{1}", message, confirm), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

        If result <> DialogResult.Yes Then
            Exit Sub
        End If

        ToggleAcitveButtons(False)

        ' Group the items by store and plan numbers then send them off to be printed.
        validImportedPlanogramItems.GroupBy(Function(i) New With
        {
            Key i.StoreNo,
            Key i.PlanNumber
        }).ToList().ForEach(
            Sub(g)
                Dim storeNo = g.Key.StoreNo
                Dim planNumber = g.Key.PlanNumber

                Try
                    PrintPlanogramForStorePlan(storeNo, planNumber, g.ToList())
                Catch ex As Exception
                    errorsForUser.Add(HandlePrintPlanogramException(storeNo, planNumber, ex))
                End Try
            End Sub)

        If errorsForUser.Any Then
            Dim failedPrintRequests = String.Join(Environment.NewLine, errorsForUser)
            FailedRequestsDialog.HandleError(
                "The following print requests failed to process. This information has also been written to the local IRMA client log file.",
                failedPrintRequests,
                "Partial Failure")
        Else
            MessageBox.Show("All print requests have been processed.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If

        ToggleAcitveButtons(True)
    End Sub

    Private Sub PrintPlanogramForStorePlan(storeNo As Integer, planNumber As String, items As List(Of PlanogramItemBO))

        Dim businessUnit = storeNumberToBusinessUnit(storeNo)

        planogramBusinessLogic.SendPlanogramPrintBatchRequests(
            items,
            planNumber,
            businessUnit,
            DateTimePickerEffectiveDate.Value)

        logger.Info(String.Format("Successfully sent planogram print batch request to the SLAW API for store number {0}, set number: {1}.", storeNo, planNumber))

    End Sub

    Private Function HandlePrintPlanogramException(
        storeNo As Integer,
        planNumber As String,
        ex As Exception) As String

        Dim errorMessageStringBuilder = New StringBuilder()

        If TypeOf ex Is WebException Then
            Dim apiWebException As WebException = CType(ex, WebException)
            errorMessageStringBuilder.AppendLine(apiWebException.GetBaseException().Message) _
                                     .AppendLine("Store: " + storeNo.ToString()) _
                                     .AppendLine("Plan: " + planNumber).AppendLine()

            Dim slawJsonResponseText As String = Nothing
            Dim slawJsonResponse As SlawJsonResponse = Nothing
            Dim response As HttpWebResponse = TryCast(apiWebException.Response, HttpWebResponse)

            If response IsNot Nothing And response.StatusCode = CType(422, HttpStatusCode) Then
                Using responseReader As StreamReader = New StreamReader(response.GetResponseStream())
                    slawJsonResponseText = responseReader.ReadToEnd()
                    Dim serializer As JavaScriptSerializer = New JavaScriptSerializer()
                    slawJsonResponse = serializer.Deserialize(Of SlawJsonResponse)(slawJsonResponseText)
                End Using
                slawJsonResponse.ItemErrors.ForEach(Sub(ie) errorMessageStringBuilder.AppendLine(ie))
            End If
        Else
            errorMessageStringBuilder.AppendLine(ex.Message) _
                                     .AppendLine("Store: " + storeNo.ToString()) _
                                     .AppendLine("Plan: " + planNumber).AppendLine()
        End If

        logger.Info(
            String.Format(
                "Error in sending planogram print batch request to the SLAW API for store number {0}, set number: {1}: {2}",
                storeNo,
                planNumber,
                errorMessageStringBuilder.ToString()))

        Return errorMessageStringBuilder.ToString()
    End Function

    Private Sub ToggleAcitveButtons(isEnabled As Boolean)
        ButtonSend.Enabled = isEnabled
        ButtonImport.Enabled = isEnabled
        ButtonSelectFile.Enabled = isEnabled
        Cursor = IIf(isEnabled, Cursors.Default, Cursors.WaitCursor)
    End Sub

    Private Sub ButtonCancel_Click(sender As Object, e As EventArgs) Handles ButtonCancel.Click
        Me.Close()
    End Sub

    Private Sub PlanogramImport_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        If Not SlawPrintBatchConfigurationDAO.SlawPrintRequestsEnabledForRegion() Then
            MessageBox.Show("SLAW is not enabled for this region.  Planogram print requests will not be sent successfully.  Please check IRMA Client configuration.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub
End Class