Imports System.Text
Imports SLIM.WholeFoods.Utility
Imports SLIM.WholeFoods.Utility.DataAccess
Imports SLIM.WholeFoods.IRMA.Common.DataAccess
Imports SLIM.WholeFoods.IRMA.Common.BusinessLogic
Imports System.Data.SqlClient

Partial Class UserInterface_InStoreSpecials_StoreSpecials
    Inherits System.Web.UI.Page

    Private ReadOnly Property DefaultStartDate() As String
        Get
            Return Date.Now.AddDays(CInt(Application.Get("Text_ISS_Process_Delay"))).ToShortDateString
        End Get
    End Property

    Private ReadOnly Property DefaultEndDate()
        Get
            Return CDate(DefaultStartDate).AddDays(CDbl(Application.Get("Text_ISS_Duration"))).Date.ToShortDateString
        End Get
    End Property

    Private defaultMinDate As Date = DateTime.Now

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim al As New ArrayList

        Me.SalePrice.Enabled = CheckBox_All.Checked
        Me.SaleMultiple.Enabled = CheckBox_All.Checked
        Me.StartDate.Enabled = CheckBox_All.Checked
        Me.EndDate.Enabled = CheckBox_All.Checked
        Button_Apply.Enabled = CheckBox_All.Checked

        If CheckBox_All.Checked Then
            Me.SalePrice.BackColor = Nothing
            Me.SaleMultiple.BackColor = Nothing
            Me.StartDate.BackColor = Nothing
            Me.EndDate.BackColor = Nothing
            Me.SaleMultiple.Focus()

            Button_Submit.ValidationGroup = "All"
        Else
            Me.SalePrice.BackColor = Drawing.Color.Silver
            Me.SaleMultiple.BackColor = Drawing.Color.Silver
            Me.StartDate.BackColor = Drawing.Color.Silver
            Me.EndDate.BackColor = Drawing.Color.Silver

            Me.SalePrice.Text = ""
            Me.SaleMultiple.Text = ""
            Me.StartDate.Value = Nothing
            Me.EndDate.Value = Nothing

            Button_Submit.ValidationGroup = ""
        End If

        If Not Page.IsPostBack Then
            StartDate.MinDate = defaultMinDate
            StartDate.Value = defaultStartDate
            EndDate.Value = defaultEndDate
        End If
    End Sub

    Protected Sub Button_Submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Submit.Click
        Dim row As GridViewRow = Nothing

        Dim sErrors As String = ""
        Dim sTemp As String = ""
        Dim sb As New StringBuilder

        For Each row In GridView1.Rows
            sTemp = ValidateISS(row)

            If sTemp <> "" Then
                sTemp = "<li>" & row.Cells(2).Text & _
                             "<ul>" & _
                                   sTemp & _
                             "</ul>" & _
                        "</li>"
            End If

            sErrors = sErrors & sTemp
        Next

        If sErrors = "" Then
            For Each row In GridView1.Rows
                CreateStoreSpecial(row, sb)
            Next

            Literal_Message.Text = sb.ToString
            Literal_Message.Visible = True
        Else
            Literal_Message.Text = sErrors
            Literal_Message.Visible = True
        End If
    End Sub

    Protected Sub DataSource_Selected(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceStatusEventArgs) Handles SqlDataSource1.Selected
        GridView1.Visible = Not e.AffectedRows.Equals(0)

        If e.AffectedRows.Equals(0) Then
            Literal_Message.Text = "<ul><li style='color: red'>No current price information found</li></ul>"
            Literal_Message.Visible = True
        Else
            Literal_Message.Visible = False
        End If
    End Sub

    Protected Function Validate_Price(ByVal row As GridViewRow) As String
        Dim retval As Boolean = True
        Dim Price As Single
        Dim Multiple As Integer
        Dim UnitPrice As Single
        Dim Margin As Decimal
        Dim sSalePrice As Single
        Dim iSaleMult As Integer
        Dim SaleUnitPrice As Single
        Dim Cost As Decimal

        Validate_Price = ""

        If Not CType(row.FindControl("txtSalePrice"), TextBox).Text.Trim.Equals(String.Empty) And Not CType(row.FindControl("txtSaleMultiple"), TextBox).Text.Trim().Equals(String.Empty) Then
            Multiple = CInt(row.Cells(4).Text)
            Price = CType(row.Cells(5).Text, Single)
            Cost = CDec(row.Cells(8).Text)
            Margin = CDec(row.Cells(9).Text)

            UnitPrice = Price / Multiple

            sSalePrice = CType(CType(row.FindControl("txtSalePrice"), TextBox).Text, Single)
            iSaleMult = CInt(CType(row.FindControl("txtSaleMultiple"), TextBox).Text)

            SaleUnitPrice = sSalePrice / iSaleMult

            If SaleUnitPrice > UnitPrice Then
                Validate_Price = Validate_Price & "<li>Sale Price cannot be higher than regular price</li>"
            End If

            Dim textISSAmountOff As Object = Application.Get("Text_ISS_Amount_Off")

            Dim maxDiscount As Single = Math.Round((UnitPrice * Application.Get("Text_ISS_Amount_Off")) / 100, 2)

            If SaleUnitPrice < (UnitPrice - maxDiscount) Then
                Validate_Price = Validate_Price & "<li>The max allowable discount is " & Application.Get("Text_ISS_Amount_Off") & "% off the current price (No less than " & String.Format("{0:C}", (UnitPrice - maxDiscount)) & ") </li>"
            End If

            Dim textISSMinMargin As Decimal = CDec(Application.Get("Text_ISS_Min_Margin"))

            Dim ISSMargin As Decimal = ComputeISSMargin(Price, Multiple, Cost)


            'If Margin < CDec(Application.Get("Text_ISS_Min_Margin")) Then
            If ISSMargin < CDec(Application.Get("Text_ISS_Min_Margin")) Then
                Validate_Price = Validate_Price & "<li>Margin is below the Minimum Margin Percentage allowed</li>"
                retval = False
            End If
        End If
    End Function

    Protected Function ComputeISSMargin(ByVal Price As Decimal, ByVal Multiple As Integer, ByVal Cost As Decimal) As Decimal
        ComputeISSMargin = 0D
        If Price * Multiple > 0 Then
            ComputeISSMargin = 100D * ((Price / Multiple) - Cost) / (Price / Multiple)
        End If
    End Function

    Protected Sub ResetLabelMessage()
        Label_Message.Text = String.Empty
        Label_Message.ForeColor = Drawing.Color.Black
    End Sub

    Private Sub ProcessSpecial(ByRef sb As StringBuilder, ByVal RegPrice As Single, ByVal RegMultiple As Integer, ByVal WeekendProcessing As Boolean, ByVal AutoUpload As Boolean, ByVal row As GridViewRow)
        Dim InStore As New InsertStoreSpecial()

        ResetLabelMessage()
        Try

            Dim Factory As New DataFactory(DataFactory.ItemCatalog)
            Dim transaction As SqlTransaction = Factory.BeginTransaction(IsolationLevel.ReadCommitted)

            Dim dt As New StoreSpecials.PriceBatchDetailDataTable
            Dim dr As StoreSpecials.PriceBatchDetailRow = dt.NewPriceBatchDetailRow

            If AutoUpload Then
                dr.SLIMRequestID = PendingSpecial(sb, RegPrice, RegMultiple, SlimStatusTypes.InProcess, row, transaction)
            Else
                dr.SLIMRequestID = PendingSpecial(sb, RegPrice, RegMultiple, SlimStatusTypes.Pending, row, Nothing)
            End If


            dr.Price = RegPrice
            dr.Multiple = RegMultiple
            dr.Sale_Multiple = CInt(CType(row.FindControl("txtSaleMultiple"), TextBox).Text)
            dr.Sale_Price = CType(CType(row.FindControl("txtSalePrice"), TextBox).Text.Trim, Single)
            dr.Item_Key = CInt(row.Cells(1).Text)
            dr.Store_No = Session("Store_No")
            dr.POSPrice = 0
            dr.POSSale_Price = CType(CType(row.FindControl("txtSalePrice"), TextBox).Text.Trim, Single)
            dr.StartDate = CType(row.FindControl("dtpStartDate"), Infragistics.WebUI.WebSchedule.WebDateChooser).Value
            dr.Sale_End_Date = CType(row.FindControl("dtpEndDate"), Infragistics.WebUI.WebSchedule.WebDateChooser).Value

            Dim insertStatus As Integer = InStore.InsertPriceBatchDetails(dr)

            If insertStatus <> 0 Then  ' 0 is the VALID code
                ' A validation error was encountered during the save.  Let the user know and exit processing.
                ' Make sure it wasn't just a warning.
                If ValidationDAO.IsErrorCode(insertStatus) Then
                    Dim validationCode As ValidationBO = ValidationDAO.GetValidationCodeDetails(insertStatus)
                    Label_Message.ForeColor = Drawing.Color.Red
                    Label_Message.Text = String.Format("Errors occurred during save: Code {0}, {1}", validationCode.ValidationCode, validationCode.ValidationCodeTypeDesc)
                    transaction.Rollback()

                    If InStr(sb.ToString, "<li style='font-size: 12px; font-family: tahoma; color: green;'>Store special was successfully created and is now in Pending status</li>") = 1 Then
                        sb.Remove(Len(sb.ToString()) - 136, 136)
                    End If

                    Exit Sub
                End If
            Else
                transaction.Commit()
            End If

            'set default message
            'sb.Append("<li><font face='tahoma' color='green'>Store special was automatically processed because AutoUpload is turned <b>ON</b></font></li>")

            'set autoupload message if flag is turned on.
            If AutoUpload Then
                sb.Append("<li><font face='tahoma' color='green'>Store special was automatically processed because AutoUpload is turned <b>ON</b></font></li>")
            End If

            'set weekendprocessing message if flag is turned on.
            If WeekendProcessing Then
                sb.Append("<li><font face='tahoma' color='green'>Store special was automatically processed because WeekendProcessing is turned <b>ON</b></font></li>")
            End If

            Literal_Message.Visible = True
            InStore = Nothing


            '***********************************************************
            Try
                ' ***************** Sent the Notification E-Mail ************
                'TODO: Send E-mail Notes after Item requested
                If Application.Get("InStoreSpecialEmail") = "1" Then
                    Dim em As New EmailNotifications
                    em.EmailType = "InStoreSpecial"
                    em.Identifier = row.Cells(2).Text
                    em.ItemDescription = row.Cells(3).Text
                    em.Store_Name = Session("Store_Name")
                    em.Store_No = Session("Store_No")
                    em.SubTeam_No = Session("SubTeam_No")
                    em.SubTeam_Name = Session("SubTeam_Name")
                    em.Current_Price = dr.Price
                    em.New_Price = dr.Sale_Price
                    em.Start_Date = dr.StartDate
                    em.End_Date = dr.Sale_End_Date
                    em.User = Session("UserName")
                    em.User_ID = Session("UserID")
                    em.Item_Key = dr.Item_Key
                    em.SentEmail()
                End If
                ' ***********************************************************
            Catch ex As Exception
                Literal_Message.Text = ex.Message
                Literal_Message.Visible = True
            End Try

        Catch ex As Exception
            sb.Append("<li>" & ex.Message & "</li>")
        End Try
    End Sub

    Private Function PendingSpecial(ByRef sb As StringBuilder, ByVal RegPrice As Single, ByVal RegMultiple As Integer, ByVal statusType As SlimStatusTypes, ByVal row As GridViewRow, ByVal dbTransaction As SqlTransaction) As Integer
        Dim Factory As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim paramList As ArrayList = New ArrayList
        Dim requestedBy As String = Session("UserName")
        Dim requestID As Integer = 0

        currentParam = New DBParam
        currentParam.Name = "ItemKey"
        currentParam.Value = CInt(row.Cells(1).Text)
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Store_No"
        currentParam.Value = Session("Store_No")
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Price"
        currentParam.Value = RegPrice
        currentParam.Type = DBParamType.Money
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Multiple"
        currentParam.Value = RegMultiple
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "SalePrice"
        currentParam.Value = CType(CType(row.FindControl("txtSalePrice"), TextBox).Text.Trim, Single)
        currentParam.Type = DBParamType.Money
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "SaleMultiple"
        currentParam.Value = CInt(CType(row.FindControl("txtSaleMultiple"), TextBox).Text.Trim)
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "POSPrice"
        currentParam.Value = RegPrice
        currentParam.Type = DBParamType.Money
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "POSSalePrice"
        currentParam.Value = CType(CType(row.FindControl("txtSalePrice"), TextBox).Text.Trim, Single)
        currentParam.Type = DBParamType.Money
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "StartDate"
        currentParam.Value = CType(row.FindControl("dtpStartDate"), Infragistics.WebUI.WebSchedule.WebDateChooser).Value
        currentParam.Type = DBParamType.DateTime
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "EndDate"
        currentParam.Value = CType(row.FindControl("dtpEndDate"), Infragistics.WebUI.WebSchedule.WebDateChooser).Value
        currentParam.Type = DBParamType.DateTime
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Status"
        currentParam.Value = CInt(statusType)
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Identifier"
        currentParam.Value = row.Cells(2).Text
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "Item_Description"
        currentParam.Value = row.Cells(3).Text
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "SubTeam_Name"
        currentParam.Value = row.Cells(13).Text
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "RequestedBy"
        currentParam.Value = requestedBy
        currentParam.Type = DBParamType.String
        paramList.Add(currentParam)

        currentParam = New DBParam
        currentParam.Name = "RequestID"
        currentParam.Type = DBParamType.Int
        paramList.Add(currentParam)

        Try
            requestID = Factory.ExecuteStoredProcedure("SLIM_CreateStoreSpecial", paramList, dbTransaction).Item(0).ToString

            If statusType <> SlimStatusTypes.Processed Then
                If InStr(sb.ToString, "<li style='font-size: 12px; font-family: tahoma; color: green;'>Store special was successfully created and is now in Pending status</li>") = 0 Then
                    sb.Append("<li style='font-size: 12px; font-family: tahoma; color: green;'>Store special was successfully created and is now in Pending status</li>")
                End If

                Button_Submit.Enabled = False
                row.Cells(0).Enabled = False
                row.Cells(14).Enabled = False
                row.Cells(15).Enabled = False
                row.Cells(16).Enabled = False
                row.Cells(17).Enabled = False
            End If
        Catch ex As Exception
            sb.Append("<li>" & ex.Message & "</li>")
        End Try

        Return requestID
    End Function

    Protected Sub CreateStoreSpecial(ByVal row As GridViewRow, ByRef sb As StringBuilder)
        Dim RegPrice As Single
        Dim RegMultiple As Integer
        Dim WeekendProcessing As Boolean = False
        Dim AutoUpload As Boolean = False
        Dim CheckISSWeekendProc As Boolean = False
        Dim CheckISSAutoUpload As Boolean = False

        If Application.Get("Check_ISS_Weekend_Proc") = "True" Then
            CheckISSWeekendProc = True
        End If

        If Application.Get("Check_ISS_AutoUpload") = "True" Then
            CheckISSAutoUpload = True
        End If

        If (CheckISSWeekendProc = True And (DateTime.Now.DayOfWeek = DayOfWeek.Saturday Or DateTime.Now.DayOfWeek = DayOfWeek.Sunday)) Then
            WeekendProcessing = True
        End If
        If (CheckISSAutoUpload = True) Then
            AutoUpload = True
        End If

        RegMultiple = CType(row.Cells(4).Text, Integer)
        RegPrice = CType(row.Cells(5).Text, Single)

        If AutoUpload Or WeekendProcessing Then
            ProcessSpecial(sb, RegPrice, RegMultiple, WeekendProcessing, AutoUpload, row)
        Else
            PendingSpecial(sb, RegPrice, RegMultiple, SlimStatusTypes.Pending, row, Nothing)
        End If
    End Sub

    Protected Function ValidateSubteam(ByRef sb As StringBuilder, ByVal ItemKey As Integer, ByVal SubTeamName As String) As Boolean
        Dim retval As Boolean = True
        Dim SubTeam_No As Integer
        Dim item As String = String.Empty
        ' ******* Get Items SubTeam ********
        Dim da As New SlimEmailTableAdapters.SlimEmailTableAdapter
        SubTeam_No = da.GetItemsSubTeam(ItemKey)
        da.Dispose()
        ' *********************************
        If Not String.IsNullOrEmpty(Application.Get("Text_ISS_Subteam_Ex")) Then
            For Each item In Application.Get("Text_ISS_Subteam_Ex").Split(",")
                If item.Trim.Equals(CStr(SubTeam_No)) Then
                    'sb.Append("<li>The Subteam " & SubTeamName & " cannot process InStoreSpecials </li>")
                    Return retval = False
                End If
            Next
        End If
        Return retval
    End Function

    Protected Sub Button_Apply_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_Apply.Click
        Dim row As GridViewRow

        For Each row In GridView1.Rows
            If SaleMultiple.Text <> "" Then CType(row.FindControl("txtSaleMultiple"), TextBox).Text = SaleMultiple.Text
            If SalePrice.Text <> "" Then CType(row.FindControl("txtSalePrice"), TextBox).Text = SalePrice.Text
            If Not StartDate.Value Is Nothing Then CType(row.FindControl("dtpStartDate"), Infragistics.WebUI.WebSchedule.WebDateChooser).Value = StartDate.Value
            If Not EndDate.Value Is Nothing Then CType(row.FindControl("dtpEndDate"), Infragistics.WebUI.WebSchedule.WebDateChooser).Value = EndDate.Value
        Next
    End Sub

    Private Function ValidateISS(ByVal row As GridViewRow) As String
        Dim sb As New StringBuilder
        Dim sErrors As String = ""


        ValidateISS = ""

        If ValidateSubteam(sb, row.Cells(1).Text, Request.QueryString("SubTeam")) Then
            If CType(row.FindControl("txtSaleMultiple"), TextBox).Text.Trim().Equals(String.Empty) Then
                sErrors = sErrors & "<li>You must enter a valid Sale Multiple</li>"
            End If

            If CType(row.FindControl("txtSalePrice"), TextBox).Text.Trim().Equals(String.Empty) Then
                sErrors = sErrors & "<li>You must enter a valid Sale Price</li>"
            End If

            If CType(row.FindControl("dtpStartDate"), Infragistics.WebUI.WebSchedule.WebDateChooser).Value = Nothing Then
                sErrors = sErrors & "<li>You must choose a valid Start Date</li>"
            End If

            If CType(row.FindControl("dtpEndDate"), Infragistics.WebUI.WebSchedule.WebDateChooser).Value = Nothing Then
                sErrors = sErrors & "<li>You must choose a valid End Date</li>"
            End If

            If Not (CType(row.FindControl("dtpStartDate"), Infragistics.WebUI.WebSchedule.WebDateChooser).Value Is Nothing Or CType(row.FindControl("dtpStartDate"), Infragistics.WebUI.WebSchedule.WebDateChooser).Value Is Nothing) Then
                If CType(row.FindControl("dtpStartDate"), Infragistics.WebUI.WebSchedule.WebDateChooser).Value < DateTime.Now.AddDays(Application.Get("Text_ISS_Process_Delay")).Date Then
                    sErrors = sErrors & "<li>The earliest valid Start Date is " & Date.Now.AddDays(Application.Get("Text_ISS_Process_Delay")).ToShortDateString() & "</li>"
                End If

                If CType(row.FindControl("dtpEndDate"), Infragistics.WebUI.WebSchedule.WebDateChooser).Value > CType(CType(row.FindControl("dtpStartDate"), Infragistics.WebUI.WebSchedule.WebDateChooser).Value, Date).AddDays(Application.Get("Text_ISS_Duration")).Date Then
                    sErrors = sErrors & "<li>The max duration for a special can only be " & Application.Get("Text_ISS_Duration") & " day(s) from your startdate"
                End If

                If CType(row.FindControl("dtpEndDate"), Infragistics.WebUI.WebSchedule.WebDateChooser).Value < Date.Now Or CType(row.FindControl("dtpStartDate"), Infragistics.WebUI.WebSchedule.WebDateChooser).Value < Date.Now.AddDays(-1) Then
                    sErrors = sErrors & "<li>Dates cannot be in the past</li>"
                End If
            End If

            sErrors = sErrors & Validate_Price(row)
        Else
            sErrors = sErrors & "<li>The Subteam " & Request.QueryString("SubTeam") & " cannot process InStoreSpecials </li>"
        End If

        ValidateISS = sErrors
    End Function

    Private Sub GridView1_Load(sender As Object, e As System.EventArgs) Handles GridView1.Load

    End Sub

    Protected Sub GridView1_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GridView1.RowCreated
        e.Row.Cells(1).Visible = False
        e.Row.Cells(9).Visible = False
        If Not e.Row.Cells(16).FindControl("dtpStartDate") Is Nothing Then
            CType(e.Row.Cells(16).FindControl("dtpStartDate"), Infragistics.WebUI.WebSchedule.WebDateChooser).Value = DefaultStartDate
            CType(e.Row.Cells(17).FindControl("dtpEndDate"), Infragistics.WebUI.WebSchedule.WebDateChooser).Value = DefaultEndDate
        End If
    End Sub

    Protected Sub GridView1_RowDeleting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewDeleteEventArgs) Handles GridView1.RowDeleting
        Dim sItemKey As String = ""

        sItemKey = GridView1.Rows(e.RowIndex).Cells(1).Text

        If InStr(Session("ISSItemKeyList").ToString, sItemKey & "|") > 0 Then
            Session("ISSItemKeyList") = Session("ISSItemKeyList").ToString.Replace(sItemKey & "|", "")
        ElseIf InStr(Session("ISSItemKeyList").ToString, sItemKey) > 0 Then
            Session("ISSItemKeyList") = Session("ISSItemKeyList").ToString.Replace(sItemKey, "")
        End If

        GridView1.DataBind()
    End Sub
End Class
