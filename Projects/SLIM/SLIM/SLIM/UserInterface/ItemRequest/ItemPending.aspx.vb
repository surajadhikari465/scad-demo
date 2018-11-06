
Imports Infragistics.WebUI
Imports Infragistics.Excel

Partial Class UserInterface_ItemRequest_ItemPending
    Inherits System.Web.UI.Page

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As system.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Not Session("ItemRequest") = True And Not Session("AccessLevel") = 3 Then
                Response.Redirect("~/AccessDenied.aspx", True)
            End If
        End If

        If Not Request.QueryString("action") Is Nothing Then
            Try
                ProcessActions()
            Catch ex As Exception
                MsgLabel.Text = ex.Message
            End Try
        End If
        UltraWebGrid1.Columns.FromKey("PushtoEIM").Hidden = False
        UltraWebGrid1.Columns.FromKey("ItemDetail").Hidden = False
        UltraWebGrid1.Columns.FromKey("ScaleDetail").Hidden = False
        UltraWebGrid1.Columns.FromKey("RejectItem").Hidden = False
        UltraWebGrid1.Columns.FromKey("DeleteItem").Hidden = False

        UpdateMenuLinks()
    End Sub

    Protected Sub UltraWebGrid1_ClickCellButton(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.CellEventArgs) Handles UltraWebGrid1.ClickCellButton

        ' ****** Check Permission **************
        If Not Session("IRMAPush") = True And Not Session("AccessLevel") = 3 Then
            Response.Redirect("~/AccessDenied.aspx", True)
        End If
        ' ************************************************************
        Dim upcExistsIrma As Boolean
        Dim vendorKey As String
        ' ************************************************************
        ' ********** Check if Ready to Apply  and UPC exists *********
        ' ************************************************************
        upcExistsIrma = InputValidation.IdentifierCheck(UltraWebGrid1.DisplayLayout.ActiveRow.Cells(5).Text.Trim)
        ' ************************************************************
        vendorKey = InputValidation.GetVendorKey(UltraWebGrid1.DisplayLayout.ActiveRow.DataKey)
        ' ************* Submit this mess to SPIRMA *******************
        ' ********** Get the DataKey - ItemRequestID *****************
        If UltraWebGrid1.DisplayLayout.ActiveRow Is Nothing Then
            MsgLabel.Text = "No Record Selected!"
            'ElseIf GridView1.SelectedRow.Cells(9).Text = False Then
            'MsgLabel.Text = "Item Not Ready To Apply!"
            'ElseIf vendorKey = Nothing Then
            '    MsgLabel.Text = "No VendorKey/PeopleSoft number found!"
        ElseIf upcExistsIrma = True Then
            MsgLabel.Text = "Identifier already exists in IRMA!"
            'ElseIf Not GridView1.SelectedRow.Cells(9).FindControl("ReadyToApply") Then
            'MsgLabel.Text = "Item not Ready to Apply!"
        ElseIf UltraWebGrid1.DisplayLayout.ActiveRow.Cells(5).Text.StartsWith("P") = True Then
            MsgLabel.Text = "PLU Requests cannot be submitted to IRMA!"
        ElseIf UltraWebGrid1.DisplayLayout.ActiveRow.Cells(5).Text.Trim = "20000000000" Then
            MsgLabel.Text = "Temporary Scale PLU cannot be submitted to IRMA!"
        Else
            Try
                Dim requestID As Integer = CInt(UltraWebGrid1.DisplayLayout.ActiveRow.DataKey)
                ' *****************************************
                'This was commented out while we send to EIM instead
                Dim ir As New ItemInsert(requestID)
                ' ****** Insert the Item ********
                ir.InsertItemIntoIrma()
                ir = Nothing
                MsgLabel.Text = "Item has been submitted to EIM."
                'MsgLabel.Text = "Item Submitted to IRMA!"
                ' ***************************************
                'TODO: Send Email Notes after IRMA push
                ' ***************************************
                Try
                    ' ***************** Sent the Notification E-Mail ************
                    'TODO: Send E-mail Notes after Item has been pushed
                    If Application.Get("IRMAPushEmail") = "1" Then
                        Dim em As New EmailNotifications
                        em.EmailType = "EIM/SLIM Push"
                        em.Identifier = UltraWebGrid1.DisplayLayout.ActiveRow.Cells(5).Text
                        em.ItemDescription = UltraWebGrid1.DisplayLayout.ActiveRow.Cells(6).Text
                        em.Store_Name = "Regional Office"
                        em.Store_No = Session("Store_No")
                        em.SubTeam_No = 600000
                        em.SubTeam_Name = UltraWebGrid1.DisplayLayout.ActiveRow.Cells(14).Text
                        em.Current_Price = UltraWebGrid1.DisplayLayout.ActiveRow.Cells(2).Text
                        em.New_Price = UltraWebGrid1.DisplayLayout.ActiveRow.Cells(8).Text
                        em.New_Cost = UltraWebGrid1.DisplayLayout.ActiveRow.Cells(10).Text
                        em.User = Session("UserName")
                        em.User_ID = Session("UserID")
                        'em.Item_Key = Request.QueryString("ItemKey")
                        em.SentEmail()
                    End If
                    ' ***********************************************************
                Catch ex As Exception
                    Debug.WriteLine(ex.Message)
                    'Error_Log.throwException(ex.Message, ex)
                End Try
            Catch ex As Exception
                Debug.WriteLine(ex.Message)
                MsgLabel.Text = "Item not Submitted - Error encountered!"
            End Try
        End If
        UltraWebGrid1.DataBind()

    End Sub

    Protected Sub UltraGrid1_InitializeRow(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles UltraWebGrid1.InitializeRow

        Dim PushToEIMCell As Infragistics.WebUI.UltraWebGrid.UltraGridCell
        Dim ScaleDetailCell As Infragistics.WebUI.UltraWebGrid.UltraGridCell
        Dim ItemDetailCell As Infragistics.WebUI.UltraWebGrid.UltraGridCell
        Dim RejectItemCell As Infragistics.WebUI.UltraWebGrid.UltraGridCell
        Dim DeleteItemCell As Infragistics.WebUI.UltraWebGrid.UltraGridCell

        Dim RequestID As Integer = 0

        Dim Item_Desc As String = String.Empty
        Dim UPC As String = String.Empty
        Dim RequestedBy As String = String.Empty
        Dim SubTeam As String = String.Empty

        PushToEIMCell = e.Row.Cells.FromKey("PushtoEIM")
        ItemDetailCell = e.Row.Cells.FromKey("ItemDetail")
        ScaleDetailCell = e.Row.Cells.FromKey("ScaleDetail")
        RejectItemCell = e.Row.Cells.FromKey("RejectItem")
        DeleteItemCell = e.Row.Cells.FromKey("DeleteItem")

        RequestID = e.Row.Cells.FromKey("ItemRequest_ID").Value
        Item_Desc = "&Item_Description=" & e.Row.Cells.FromKey("Item_Description").Value
        UPC = "&Identifier=" & e.Row.Cells.FromKey("Identifier").Value
        RequestedBy = "&RequestedBy=" & e.Row.Cells.FromKey("RequestedBy").Value
        SubTeam = "&SubTeam_name=" & e.Row.Cells.FromKey("SubTeam_Name").Value


        PushToEIMCell.Value = "Push to EIM"
        ItemDetailCell.Value = "<a onMouseOver='window.status=""View Item Detail""; return true;' href='ItemDetail.aspx?ItemRequest_ID=" & RequestID.ToString() & "'>Item Detail</a>"
        ScaleDetailCell.Value = "<a onMouseOver='window.status=""View Scale Detail""; return true;' href='ItemScaleDetail.aspx?ItemRequest_ID=" & RequestID.ToString() & "'>Scale Detail</a>"
        RejectItemCell.Value = "<a onMouseOver='window.status=""Reject Item Request"";return true;' href='javascript:popitup(""ItemRequestComments.aspx?ItemRequest_ID=" & RequestID.ToString() & UPC & Item_Desc & SubTeam & RequestedBy & """)'>Reject</a>"
        DeleteItemCell.Value = "<a onMouseOver='window.status=""Delete Item Request""; return true;' href='ItemPending.aspx?action=delete&value=" & RequestID.ToString() & "'>Delete</a>"

    End Sub

    Protected Sub UltraGrid1_InitalizeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles UltraWebGrid1.InitializeLayout

        UltraWebGrid1.Columns.FromKey("PushtoEIM").Width = 90
        UltraWebGrid1.Columns.FromKey("ItemDetail").Width = 55
        UltraWebGrid1.Columns.FromKey("ScaleDetail").Width = 60
        UltraWebGrid1.Columns.FromKey("RejectItem").Width = 40
        UltraWebGrid1.Columns.FromKey("DeleteItem").Width = 40



        Dim backColor As System.Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#004000")
        Dim AltColor As System.Drawing.Color = System.Drawing.ColorTranslator.FromHtml("#C0FFC0")

        UltraWebGrid1.Font.Size = 12
        UltraWebGrid1.Font.Name = "Tahoma"

        With UltraWebGrid1.DisplayLayout
            .Pager.AllowPaging = True
            .Pager.StyleMode = UltraWebGrid.PagerStyleMode.CustomLabels
            .Pager.Pattern = "[currentpageindex] of [pagecount] : [page:first:First] [prev] [next] [page:last:Last]"
            .Pager.StyleMode = Infragistics.WebUI.UltraWebGrid.PagerStyleMode.PrevNext
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
        ' ****** Check Permission **************
        'This hides the process links from store users 
        If Not Session("IRMAPush") = True And Not Session("AccessLevel") = 3 Then
            UltraWebGrid1.DisplayLayout.Bands(0).Columns(0).Hidden = True
            UltraWebGrid1.DisplayLayout.Bands(0).Columns(18).Hidden = True
            UltraWebGrid1.DisplayLayout.Bands(0).Columns(19).Hidden = True
        End If

    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles LinkButton1.Click
        Dim book As New Workbook()

        book.Worksheets.Add("New")

        UltraWebGrid1.Columns.FromKey("PushtoEIM").Hidden = True
        UltraWebGrid1.Columns.FromKey("ItemDetail").Hidden = True
        UltraWebGrid1.Columns.FromKey("ScaleDetail").Hidden = True
        UltraWebGrid1.Columns.FromKey("RejectItem").Hidden = True
        UltraWebGrid1.Columns.FromKey("DeleteItem").Hidden = True

        UltraWebGridExcelExporter1.Export(UltraWebGrid1, book)
        'BIFF8Writer.WriteWorkbookToStream(book, Response.OutputStream)
        book.Save(Response.OutputStream)


        UltraWebGrid1.Columns.FromKey("PushtoEIM").Hidden = False
        UltraWebGrid1.Columns.FromKey("ItemDetail").Hidden = False
        UltraWebGrid1.Columns.FromKey("ScaleDetail").Hidden = False
        UltraWebGrid1.Columns.FromKey("RejectItem").Hidden = False
        UltraWebGrid1.Columns.FromKey("DeleteItem").Hidden = False

    End Sub

    Protected Sub ProcessActions()
        Dim value As String = String.Empty
        value = Request.QueryString("value")
        Select Case Request.QueryString("action").ToLower()
            Case "delete"
                DeleteItemRequest(value)
        End Select
        UltraWebGrid1.DataBind()
    End Sub

    Protected Sub DeleteItemRequest(ByRef value As String)
        Dim df As New DataFactory(DataFactory.ItemCatalog)
        Dim currentParam As DBParam
        Dim paramList As ArrayList = New ArrayList
        Try

            currentParam = New DBParam
            currentParam.Name = "RequestId"
            currentParam.Value = CType(value, Integer)
            currentParam.Type = DBParamType.Int
            paramList.Add(currentParam)

            df.ExecuteStoredProcedure("SLIM_DeleteItemRequest", paramList)

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
