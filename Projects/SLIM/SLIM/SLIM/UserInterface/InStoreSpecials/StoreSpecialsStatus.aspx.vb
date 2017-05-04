Imports Infragistics.Excel
Imports System.Web.UI
Imports SLIM.WholeFoods.IRMA.Common.BusinessLogic
Imports SLIM.WholeFoods.IRMA.Common.DataAccess

Partial Class UserInterface_InStoreSpecials_StoreSpecialsStatus
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Label_RejectionReason.Visible = optReject.Checked
        Textbox_RejectionReason.Visible = optReject.Checked
        rfvRejectionReason.Enabled = optReject.Checked
        Dim showEndISS = InstanceDataDAO.IsFlagActive("EnableSLIMEndISS")

        If Not IsPostBack Then
            If Not Session("StoreSpecials") = True And Not Session("AccessLevel") = 3 Then
                Response.Redirect("~/AccessDenied.aspx", True)
            End If

            cmbStore.AppendDataBoundItems = True
            cmbStore.Items.Insert(0, New ListItem("All Stores", "0"))

            cmbSubTeam.AppendDataBoundItems = True
            cmbSubTeam.Items.Insert(0, New ListItem("All SubTeams", "0"))
        End If

        If Not Request.QueryString("action") Is Nothing Then
            Try
                ProcessActions(Request.QueryString("value"), Request.QueryString("action").ToLower())
            Catch ex As Exception
                Label_Message.Text = ex.Message
            End Try
        End If

        UpdateMenuLinks()

        If Not Request.QueryString("status") Is Nothing Then
            Select Case Request.QueryString("status")
                Case "1"
                    grdISS.Columns(0).Visible = True
                    pnlFilters.Visible = True
                    Page.Title = "Pending ISS's"

                    If Not Page.IsPostBack Then
                        optApprove.Checked = True
                        optReject.Checked = False
                        optEndISS.Checked = False
                        optReprocess.Checked = False
                    End If

                    tblOptions.Rows(0).Visible = True
                    tblOptions.Rows(1).Visible = False
                    tblOptions.Rows(2).Visible = False
                    tblOptions.Rows(3).Visible = True

                Case "2"
                    grdISS.Columns(0).Visible = False
                    tblOptions.Visible = False

                    If Not Page.IsPostBack Then
                        optApprove.Checked = False
                        optReject.Checked = False
                        optEndISS.Checked = False
                        optReprocess.Checked = False
                    End If

                    tblOptions.Visible = False

                    Button_Submit.Visible = False
                    pnlFilters.Visible = True
                    cmbStore.Visible = True
                    cmbSubTeam.Visible = True
                    dtpStartDate.Visible = True
                    dtpEndDate.Visible = True
                    Page.Title = "In-Process ISS's"

                Case "3"

                    If Not Page.IsPostBack Then
                        optApprove.Checked = False
                        optReject.Checked = False
                        optEndISS.Checked = True
                        optReprocess.Checked = False
                    End If

                    tblOptions.Rows(0).Visible = False
                    tblOptions.Rows(1).Visible = False
                    tblOptions.Rows(2).Visible = showEndISS
                    tblOptions.Rows(3).Visible = showEndISS

                    grdISS.Columns(0).Visible = True
                    pnlFilters.Visible = True
                    cmbStore.Visible = True
                    cmbSubTeam.Visible = True
                    dtpStartDate.Visible = True
                    dtpEndDate.Visible = True
                    Page.Title = "Processed ISS's"

                Case "4"
                    grdISS.Columns(0).Visible = True
                    pnlFilters.Visible = True
                    Page.Title = "Rejected ISS's"

                    If Not Page.IsPostBack Then
                        optApprove.Checked = False
                        optReject.Checked = False
                        optEndISS.Checked = False
                        optReprocess.Checked = True
                    End If

                    tblOptions.Rows(0).Visible = False
                    tblOptions.Rows(1).Visible = True
                    tblOptions.Rows(2).Visible = False
                    tblOptions.Rows(3).Visible = True
            End Select
        End If
    End Sub

    Protected Sub ResetLabelMessage()
        Label_Message.Text = String.Empty
        Label_Message.ForeColor = Drawing.Color.Black
    End Sub

    Protected Sub ProcessActions(ByVal value As String, ByVal action As String)
        Select Case action
            Case "EndSale"
                EndStoreSpecial(value)
            Case "InProcess"
                ProcessStoreSpecial(value)
            Case "process"
                ProcessStoreSpecial(value)
            Case "reject"
                RejectStoreSpecial(value)
            Case "reprocess"
                ReProcessStoreSpecial(value)
        End Select
    End Sub

    Protected Sub EndStoreSpecial(ByRef value As String)

        Dim EndStore As New EndStoreSpecial()

        Dim df As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim paramList As ArrayList = New ArrayList
        Dim dr As StoreSpecials.PriceBatchDetailRow
        Dim ds As DataSet = Nothing

        Try

            ResetLabelMessage()

            Dim RequestID As Integer
            currentParam = New DBParam
            currentParam.Name = "RequestID"
            currentParam.Value = CType(value, Integer)
            RequestID = CType(value, Integer)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ds = df.GetStoredProcedureDataSet("SLIM_GetPriceBatchDetailInfo", paramList)
            paramList.Clear()

            Dim updateStatus As Integer
            Dim errStr As New StringBuilder

            With ds.Tables(0).Rows(0)

                Dim dt As New StoreSpecials.PriceBatchDetailDataTable

                dr = dt.NewPriceBatchDetailRow

                dr.PriceBatchDetailID = .Item("PriceBatchDetailID")
                dr.Item_Key = .Item("Item_Key")
                dr.Store_No = .Item("Store_No")
                dr.Multiple = .Item("Multiple")
                dr.Price = .Item("Price")
                dr.POSPrice = .Item("POSPrice")
                dr.Sale_End_Date = .Item("EndDate")

                ' end the sale early
                updateStatus = EndStore.ProcessEndSale(dr)

                If updateStatus <> 0 Then  ' 0 is the VALID code
                    ' A validation error was encountered during the save.  Let the user know and exit processing.
                    ' Make sure it wasn't just a warning.
                    If ValidationDAO.IsErrorCode(updateStatus) Then
                        Dim validationCode As ValidationBO = ValidationDAO.GetValidationCodeDetails(updateStatus)
                        errStr.AppendLine(String.Format("Errors occurred during save: Code {0}, {1}", validationCode.ValidationCode, validationCode.ValidationCodeTypeDesc))
                    End If
                End If

            End With

            If errStr.Length > 0 Then
                Label_Message.ForeColor = Drawing.Color.Red
                Label_Message.Text = errStr.ToString
            End If

        Catch ex As Exception

            Throw ex

        Finally

            If Not ds Is Nothing Then ds.Dispose()

        End Try

    End Sub

    Protected Sub ProcessStoreSpecial(ByRef value As String)
        Dim InStore As New InsertStoreSpecial()

        Dim df As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim paramList As ArrayList = New ArrayList
        Dim dr As StoreSpecials.PriceBatchDetailRow
        Dim ds As DataSet = Nothing

        Try
            ResetLabelMessage()

            Dim RequestID As Integer
            currentParam = New DBParam
            currentParam.Name = "RequestId"
            currentParam.Value = CType(value, Integer)
            RequestID = CType(value, Integer)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            ds = df.GetStoredProcedureDataSet("SLIM_GetInStoreSpecials", paramList)
            paramList.Clear()

            With ds.Tables(0).Rows(0)
                Dim dt As New StoreSpecials.PriceBatchDetailDataTable
                dr = dt.NewPriceBatchDetailRow
                dr.Price = .Item("Price")
                dr.Multiple = .Item("Multiple")
                dr.Sale_Multiple = .Item("SaleMultiple")
                dr.Sale_Price = .Item("SalePrice")
                dr.Item_Key = .Item("Item_Key")
                dr.Store_No = .Item("Store_No")
                dr.POSPrice = .Item("POSPrice")
                dr.POSSale_Price = .Item("SalePrice")
                dr.StartDate = .Item("StartDate")
                dr.Sale_End_Date = .Item("EndDate")
                dr.SLIMRequestID = RequestID
            End With

            Dim insertStatus As Integer = InStore.InsertPriceBatchDetails(dr)

            If insertStatus <> 0 Then  ' 0 is the VALID code
                ' A validation error was encountered during the save.  Let the user know and exit processing.
                ' Make sure it wasn't just a warning.
                If ValidationDAO.IsErrorCode(insertStatus) Then
                    Dim validationCode As ValidationBO = ValidationDAO.GetValidationCodeDetails(insertStatus)
                    Label_Message.ForeColor = Drawing.Color.Red
                    Label_Message.Text = String.Format("Errors occurred during save: Code {0}, {1}", validationCode.ValidationCode, validationCode.ValidationCodeTypeDesc)
                    Exit Sub
                End If
            End If

            'Update the status for the In-store-Special after it's inserted into the PriceBatchDetails table.
            Try
                currentParam = New DBParam
                currentParam.Name = "RequestId"
                currentParam.Value = CType(value, Integer)
                currentParam.Type = DBParamType.Int
                paramList.Add(currentParam)

                currentParam = New DBParam
                currentParam.Name = "ProcessedBy"
                currentParam.Value = Session("UserName")
                currentParam.Type = DBParamType.String
                paramList.Add(currentParam)

                ds = df.GetStoredProcedureDataSet("SLIM_ProcessStoreSpecial", paramList)
                paramList.Clear()

            Catch ex As Exception
                Throw ex
            End Try

        Catch ex As Exception
            Throw ex
        Finally
            If Not ds Is Nothing Then ds.Dispose()
        End Try
    End Sub

    Protected Sub RejectStoreSpecial(ByRef value As String)
        Dim df As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim paramList As ArrayList = New ArrayList

        Try
            currentParam = New DBParam
            currentParam.Name = "RequestId"
            currentParam.Value = CType(value, Integer)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "ProcessedBy"
            currentParam.Value = Session("UserName")
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            currentParam = New DBParam
            currentParam.Name = "Comments"
            currentParam.Value = Textbox_RejectionReason.Text
            currentParam.Type = DBParamType.String
            paramList.Add(currentParam)

            df.ExecuteStoredProcedure("SLIM_RejectStoreSpecial", paramList)
        Catch ex As Exception
            Throw ex
        End Try
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

            df.ExecuteStoredProcedure("SLIM_ReProcessStoreSpecial", paramList)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        'Hide the checkbox column so it doesn't get exported
        grdISS.Columns(0).Visible = False
        grdISS.DataBind()

        ' Clear the response  
        Response.Clear()

        ' Set the type and filename  
        Response.AddHeader("content-disposition", "attachment;filename=ISS.xls")
        Response.Charset = ""
        Response.ContentType = "application/vnd.xls"

        ' Add the HTML from the GridView to a StringWriter so we can write it out later  
        Dim sw As System.IO.StringWriter = New System.IO.StringWriter
        Dim hw As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(sw)
        grdISS.RenderControl(hw)

        ' Write out the data
        Response.Write(Replace(sw.ToString, "font-size:Small", "font-size:10px"))
        Response.End()

        'Unhide the checkbox column
        grdISS.Columns(0).Visible = True
        grdISS.DataBind()
    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

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

    Protected Sub CheckBox_All_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_All.CheckedChanged
        Dim row As GridViewRow

        For Each row In grdISS.Rows
            CType(row.FindControl("chkSelect"), CheckBox).Checked = CheckBox_All.Checked
        Next
    End Sub

    Protected Sub Button_Submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Submit.Click
        Dim row As GridViewRow
        Dim sMsg As String = ""

        For Each row In grdISS.Rows

            If optApprove.Checked Then
                If CType(row.FindControl("chkSelect"), CheckBox).Checked Then
                    ProcessActions(row.Cells(GetColumnIndexByHeader("RequestId")).Text, "InProcess")

                    If sMsg = "" Then
                        sMsg = "The following item(s) have been approved: " & vbCrLf & vbCrLf & _
                               "UPC: " & row.Cells(GetColumnIndexByHeader("Identifier")).Text & vbCrLf & _
                               "Item Description: " & row.Cells(GetColumnIndexByHeader("Description")).Text.Trim & vbCrLf & _
                               "Current Price: " & row.Cells(GetColumnIndexByHeader("Price")).Text & vbCrLf & _
                               "Sale Price: " & row.Cells(GetColumnIndexByHeader("Sale Price")).Text & vbCrLf & _
                               "Start Date: " & row.Cells(GetColumnIndexByHeader("Start Date")).Text & vbCrLf & _
                               "End Date: " & row.Cells(GetColumnIndexByHeader("End Date")).Text & vbCrLf
                    Else
                        sMsg = sMsg & vbCrLf & _
                               "UPC: " & row.Cells(GetColumnIndexByHeader("Identifier")).Text & vbCrLf & _
                               "Item Description: " & row.Cells(GetColumnIndexByHeader("Description")).Text.Trim & vbCrLf & _
                               "Current Price: " & row.Cells(GetColumnIndexByHeader("Price")).Text & vbCrLf & _
                               "Sale Price: " & row.Cells(GetColumnIndexByHeader("Sale Price")).Text & vbCrLf & _
                               "Start Date: " & row.Cells(GetColumnIndexByHeader("Start Date")).Text & vbCrLf & _
                               "End Date: " & row.Cells(GetColumnIndexByHeader("End Date")).Text & vbCrLf
                    End If
                End If
            End If

            If optReject.Checked Then
                If CType(row.FindControl("chkSelect"), CheckBox).Checked Then
                    ProcessActions(row.Cells(1).Text, "reject")

                    If sMsg = "" Then
                        sMsg = "The following item(s) have been rejected because: " & vbCrLf & vbTab & Textbox_RejectionReason.Text & _
                               vbCrLf & vbCrLf & _
                               "UPC: " & row.Cells(GetColumnIndexByHeader("Identifier")).Text & vbCrLf & _
                               "Item Description: " & row.Cells(GetColumnIndexByHeader("Description")).Text.Trim & vbCrLf & _
                               "Current Price: " & row.Cells(GetColumnIndexByHeader("Price")).Text & vbCrLf & _
                               "Sale Price: " & row.Cells(GetColumnIndexByHeader("Sale Price")).Text & vbCrLf & _
                               "Start Date: " & row.Cells(GetColumnIndexByHeader("Start Date")).Text & vbCrLf & _
                               "End Date: " & row.Cells(GetColumnIndexByHeader("End Date")).Text & vbCrLf & _
                               "Requested By: " & row.Cells(GetColumnIndexByHeader("Requested By")).Text & vbCrLf & _
                               "Requested Store: " & row.Cells(GetColumnIndexByHeader("Store Name")).Text & vbCrLf
                    Else
                        sMsg = sMsg & vbCrLf & _
                               "UPC: " & row.Cells(GetColumnIndexByHeader("Identifier")).Text & vbCrLf & _
                               "Item Description: " & row.Cells(GetColumnIndexByHeader("Description")).Text.Trim & vbCrLf & _
                               "Current Price: " & row.Cells(GetColumnIndexByHeader("Price")).Text & vbCrLf & _
                               "Sale Price: " & row.Cells(GetColumnIndexByHeader("Sale Price")).Text & vbCrLf & _
                               "Start Date: " & row.Cells(GetColumnIndexByHeader("Start Date")).Text & vbCrLf & _
                               "End Date: " & row.Cells(GetColumnIndexByHeader("End Date")).Text & vbCrLf & _
                               "Requested By: " & row.Cells(GetColumnIndexByHeader("Requested By")).Text & vbCrLf & _
                               "Requested Store: " & row.Cells(GetColumnIndexByHeader("Store Name")).Text & vbCrLf
                    End If
                End If
            End If

            If optEndISS.Checked Then
                If CType(row.FindControl("chkSelect"), CheckBox).Checked Then
                    ProcessActions(row.Cells(1).Text, "EndSale")

                    If sMsg = "" Then
                        sMsg = "Sale ended successfully! " & vbCrLf & vbCrLf & _
                               "UPC: " & row.Cells(GetColumnIndexByHeader("Identifier")).Text & vbCrLf & _
                               "Item Description: " & row.Cells(GetColumnIndexByHeader("Description")).Text.Trim & vbCrLf & _
                               "Current Price: " & row.Cells(GetColumnIndexByHeader("Price")).Text & vbCrLf & _
                               "Sale Price: " & row.Cells(GetColumnIndexByHeader("Sale Price")).Text & vbCrLf & _
                               "Start Date: " & row.Cells(GetColumnIndexByHeader("Start Date")).Text & vbCrLf & _
                               "End Date: " & row.Cells(GetColumnIndexByHeader("End Date")).Text & vbCrLf & _
                               "Requested By: " & row.Cells(GetColumnIndexByHeader("Requested By")).Text & vbCrLf & _
                               "Requested Store: " & row.Cells(GetColumnIndexByHeader("Store Name")).Text & vbCrLf
                    Else
                        sMsg = sMsg & vbCrLf & _
                               "UPC: " & row.Cells(GetColumnIndexByHeader("Identifier")).Text & vbCrLf & _
                               "Item Description: " & row.Cells(GetColumnIndexByHeader("Description")).Text.Trim & vbCrLf & _
                               "Current Price: " & row.Cells(GetColumnIndexByHeader("Price")).Text & vbCrLf & _
                               "Sale Price: " & row.Cells(GetColumnIndexByHeader("Sale Price")).Text & vbCrLf & _
                               "Start Date: " & row.Cells(GetColumnIndexByHeader("Start Date")).Text & vbCrLf & _
                               "End Date: " & row.Cells(GetColumnIndexByHeader("End Date")).Text & vbCrLf & _
                               "Requested By: " & row.Cells(GetColumnIndexByHeader("Requested By")).Text & vbCrLf & _
                               "Requested Store: " & row.Cells(GetColumnIndexByHeader("Store Name")).Text & vbCrLf

                    End If
                End If
            End If

            If optReprocess.Checked Then
                If CType(row.FindControl("chkSelect"), CheckBox).Checked Then
                    ProcessActions(row.Cells(1).Text, "reprocess")

                    If sMsg = "" Then
                        sMsg = "The following item(s) have been reprocessed " & vbCrLf & vbCrLf & _
                               "UPC: " & row.Cells(GetColumnIndexByHeader("Identifier")).Text & vbCrLf & _
                               "Item Description: " & row.Cells(GetColumnIndexByHeader("Description")).Text.Trim & vbCrLf & _
                               "Current Price: " & row.Cells(GetColumnIndexByHeader("Price")).Text & vbCrLf & _
                               "Sale Price: " & row.Cells(GetColumnIndexByHeader("Sale Price")).Text & vbCrLf & _
                               "Start Date: " & row.Cells(GetColumnIndexByHeader("Start Date")).Text & vbCrLf & _
                               "End Date: " & row.Cells(GetColumnIndexByHeader("End Date")).Text & vbCrLf & _
                               "Requested By: " & row.Cells(GetColumnIndexByHeader("Requested By")).Text & vbCrLf & _
                               "Requested Store: " & row.Cells(GetColumnIndexByHeader("Store Name")).Text & vbCrLf
                    Else
                        sMsg = sMsg & vbCrLf & _
                               "UPC: " & row.Cells(GetColumnIndexByHeader("Identifier")).Text & vbCrLf & _
                               "Item Description: " & row.Cells(GetColumnIndexByHeader("Description")).Text.Trim & vbCrLf & _
                               "Current Price: " & row.Cells(GetColumnIndexByHeader("Price")).Text & vbCrLf & _
                               "Sale Price: " & row.Cells(GetColumnIndexByHeader("Sale Price")).Text & vbCrLf & _
                               "Start Date: " & row.Cells(GetColumnIndexByHeader("Start Date")).Text & vbCrLf & _
                               "End Date: " & row.Cells(GetColumnIndexByHeader("End Date")).Text & vbCrLf & _
                               "Requested By: " & row.Cells(GetColumnIndexByHeader("Requested By")).Text & vbCrLf & _
                               "Requested Store: " & row.Cells(GetColumnIndexByHeader("Store Name")).Text & vbCrLf
                    End If
                End If
            End If
        Next

        ISSSendConfirmationEmail(sMsg, optApprove.Checked, optReject.Checked, optEndISS.Checked)
        grdISS.DataBind()
    End Sub

    Private Sub ISSSendConfirmationEmail(ByVal sMsgBody As String, ByVal blnApprove As Boolean, ByVal blnReject As Boolean, ByVal blnEndISS As Boolean)
        Try
            If blnApprove Then
                If Application.Get("InStoreSpecialEmail") = "1" And sMsgBody <> "" Then
                    Dim em As New EmailNotifications
                    em.EmailType = "InStoreSpecialProcess"
                    em.Store_No = Session("Store_No")
                    em.User = Session("UserName")
                    em.User_ID = Session("UserID")
                    em.Message_Body = sMsgBody
                    em.SentEmail()
                End If
            ElseIf blnReject Then
                If Application.Get("InStoreSpecialRejectEmail") = "1" And sMsgBody <> "" Then
                    Dim em As New EmailNotifications
                    em.EmailType = "InStoreSpecialReject"
                    em.Store_No = Session("Store_No")
                    em.User = Session("UserName")
                    em.User_ID = Session("UserID")
                    em.Message_Body = sMsgBody
                    em.SentEmail()
                End If
            ElseIf blnEndISS Then
                If Application.Get("InStoreSpecialEndSaleEarlyEmail") = "1" And sMsgBody <> "" Then
                    Dim em As New EmailNotifications
                    em.EmailType = "EndSaleEarly"
                    em.Store_No = Session("Store_No")
                    em.Store_Name = Session("Store_Name")
                    em.SubTeam_Name = Session("SubTeam_Name")
                    em.User = Session("UserName")
                    em.User_ID = Session("UserID")
                    em.Message_Body = sMsgBody
                    em.SentEmail()
                End If
            End If
        Catch ex As Exception
            Debug.WriteLine(ex.Message)
        End Try
    End Sub

    Protected Sub grdISS_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles grdISS.DataBound
        If grdISS.Rows.Count > 0 And Request.QueryString("status") <> 2 Then
            CheckBox_All.Visible = True
        Else
            CheckBox_All.Visible = False
        End If
    End Sub

    Protected Sub grdISS_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles grdISS.RowDataBound
        e.Row.Cells(GetColumnIndexByHeader("RequestId")).Visible = False
        e.Row.Cells(GetColumnIndexByHeader("Item_Key")).Visible = False
        e.Row.Cells(GetColumnIndexByHeader("Store_No")).Visible = False
        e.Row.Cells(GetColumnIndexByHeader("SubTeam_No")).Visible = False

        If Request.QueryString("status") = 4 Then
            e.Row.Cells(GetColumnIndexByHeader("Comments")).Visible = True
        Else
            e.Row.Cells(GetColumnIndexByHeader("Comments")).Visible = False
        End If
    End Sub

    Private Function GetColumnIndexByHeader(ByVal sColumnHeader) As Integer
        Dim x As Integer = 0

        For x = 0 To grdISS.Columns.Count - 1
            If grdISS.Columns(x).HeaderText = sColumnHeader Then
                Return x
            End If
        Next
    End Function

    Private Function GetFilterExpression() As String
        Dim exp As String = ""

        If cmbStore.SelectedIndex > 0 Then
            exp = "Store_No = " & cmbStore.SelectedValue.ToString
        Else
            exp = ""
        End If

        If cmbSubTeam.SelectedIndex > 0 Then
            If exp = "" Then
                exp = "SubTeam_No = " & cmbSubTeam.SelectedValue.ToString
            Else
                exp = exp & " AND SubTeam_No = " & cmbSubTeam.SelectedValue.ToString
            End If
        End If

        If Not dtpStartDate.Value Is Nothing Then
            If exp = "" Then
                exp = "StartDate >= #" & CDate(dtpStartDate.Value).ToShortDateString & "#"
            Else
                exp = exp & " AND StartDate >= #" & CDate(dtpStartDate.Value).ToShortDateString & "#"
            End If
        End If

        If Not dtpEndDate.Value Is Nothing Then
            If exp = "" Then
                exp = "EndDate <= #" & CDate(dtpEndDate.Value).ToShortDateString & "#"
            Else
                exp = exp & " AND EndDate <= #" & CDate(dtpEndDate.Value).ToShortDateString & "#"
            End If
        End If

        Return exp
    End Function

    Protected Sub cmbStore_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.SelectedIndexChanged
        dsISS.FilterExpression = GetFilterExpression()
        grdISS.DataBind()
    End Sub

    Protected Sub cmbSubTeam_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.SelectedIndexChanged
        dsISS.FilterExpression = GetFilterExpression()
        grdISS.DataBind()
    End Sub

    Protected Sub dtpStartDate_ValueChanged(ByVal sender As Object, ByVal e As Infragistics.WebUI.WebSchedule.WebDateChooser.WebDateChooserEventArgs) Handles dtpStartDate.ValueChanged
        dsISS.FilterExpression = GetFilterExpression()
        grdISS.DataBind()
    End Sub

    Protected Sub dtpEndDate_ValueChanged(ByVal sender As Object, ByVal e As Infragistics.WebUI.WebSchedule.WebDateChooser.WebDateChooserEventArgs) Handles dtpEndDate.ValueChanged
        dsISS.FilterExpression = GetFilterExpression()
        grdISS.DataBind()
    End Sub
End Class