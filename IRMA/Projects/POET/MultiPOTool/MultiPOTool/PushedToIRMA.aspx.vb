Imports Microsoft.VisualBasic
Imports Infragistics.WebUI.UltraWebGrid
Imports WholeFoods.Utility.SMTP
Imports Infragistics.Documents.Report
Imports WholeFoods.Utility.DataAccess



Partial Public Class PushedToIRMA
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ErrorLabel.Text = String.Empty

        If Page.EnableViewState = False Then
            Page.EnableViewState = True
        End If

        If Not Page.IsPostBack Then

            If CInt(Session("UserID")) = "2" Then
                trRegionFilter.Visible = True

                Dim DS As DataSet = Utility.GetRegionsByUser(CInt(Session("UserID")))

                If DS.Tables(0).Rows.Count > 0 Then

                    For Each row As DataRow In DS.Tables(0).Rows
                        Dim li As New ListItem
                        li.Text = row("RegionName")
                        li.Value = "[" & row("IRMAserver") & "].[" & row("IRMADatabase") & "].[dbo]."

                        drpRegions.Items.Add(li)
                    Next

                End If
            End If

            Me.rdoPOType.Items(0).Text = "POs uploaded by " & Session("UserName")
            Me.rdoPOType.Items(1).Text = "POs pushed by " & Session("UserName")

            Session("POType") = 1
           
            drpStore.DataSource = BOValidatedPOs.GetStoreNames(CInt(Session("UserID")), drpRegions.SelectedValue)
            drpStore.DataBind()

            drpSubTeam.DataSource = BOValidatedPOs.GetSubTeams(CInt(Session("UserID")), drpRegions.SelectedValue)
            drpSubTeam.DataBind()

            drpVendor.DataSource = BOValidatedPOs.GetVendors(CInt(Session("UserID")), drpRegions.SelectedValue)
            drpVendor.DataBind()

        End If

    End Sub

    Private Sub ObjectDataSource1_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource1.Selecting
        If Session("UserID") Is Nothing Then
            lblPushedToIRMA.Text = "There isn't anyone logged in"
        Else
  

            e.InputParameters("UserID") = CInt(Session("UserID"))
            If Not txtNumber.Text = String.Empty Then
                e.InputParameters("Top") = Convert.ToInt32(txtNumber.Text)
            Else
                e.InputParameters("Top") = 100
            End If

            If Not txtStartDate.Text = String.Empty Then
                e.InputParameters("StartDate") = Convert.ToDateTime(txtStartDate.Text)
            Else
                e.InputParameters("StartDate") = "01/01/1900"
            End If

            If Not txtEndDate.Text = String.Empty Then
                e.InputParameters("EndDate") = DateAdd(DateInterval.Day, 1, Convert.ToDateTime(txtEndDate.Text))
            Else
                e.InputParameters("EndDate") = "01/01/1900"
            End If
            If Not drpStore.SelectedValue = "" Then
                e.InputParameters("Store") = drpStore.SelectedValue
            Else
                e.InputParameters("Store") = "0"
            End If

            If Not drpVendor.SelectedValue = "" Then
                e.InputParameters("Vendor") = drpVendor.SelectedValue
            Else
                e.InputParameters("Vendor") = 0
            End If
            If Not drpSubTeam.SelectedValue = "" Then
                e.InputParameters("Subteam") = drpSubTeam.SelectedValue
            Else
                e.InputParameters("Subteam") = 0
            End If


            lblPushedToIRMA.Text = "POs that have been pushed to IRMA for " + Session.Item("UserName").ToString
        End If
        ult.DisplayLayout.Pager.AllowPaging = True
        ult.DisplayLayout.Pager.PageSize = CInt(ddlPaging.SelectedValue)

    End Sub

    Protected Sub excelExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles excelExport.Click
        Select Case ddlExportFormat.SelectedValue.ToString()
            Case "PDF"
                ULTWDE.Format = FileFormat.PDF
                ULTWDE.DownloadName = "POsPushedToIrma.pdf"
                ULTWDE.Export(ult)
            Case "Text"
                ULTWDE.Format = FileFormat.PlainText
                ULTWDE.DownloadName = "POsPushedToIrma.txt"
                ULTWDE.Export(ult)
            Case "Excel"
                Dim currentPaging As String
                currentPaging = ddlPaging.SelectedValue.ToString()
                ult.DataBind()
                ULTWGEE.DownloadName = "POsPushedToIrma.xls"
                ULTWGEE.Export(ult)
                ddlPaging.SelectedValue = currentPaging
                ult.DataBind()
        End Select

    End Sub

    Protected Sub btnGet_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGet.Click

        Me.UpdateGrid()

    End Sub

    Protected Sub drpRegions_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles drpRegions.SelectedIndexChanged


        drpStore.DataSource = BOValidatedPOs.GetStoreNames(CInt(Session("UserID")), drpRegions.SelectedValue)
        drpStore.DataBind()

        drpSubTeam.DataSource = BOValidatedPOs.GetSubTeams(CInt(Session("UserID")), drpRegions.SelectedValue)
        drpSubTeam.DataBind()

        drpVendor.DataSource = BOValidatedPOs.GetVendors(CInt(Session("UserID")), drpRegions.SelectedValue)
        drpVendor.DataBind()
    End Sub

    Private Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        drpRegions.SelectedValue = ""
        drpStore.SelectedValue = ""
        drpSubTeam.SelectedValue = "0"
        drpVendor.SelectedValue = "0"
        txtStartDate.Text = ""
        txtEndDate.Text = ""
        txtNumber.Text = ""
        drpPOType.SelectedValue = 1
    End Sub

    Protected Sub ult_ClickCellButton(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.CellEventArgs) Handles ult.ClickCellButton
        Dim dal As New DAOValidatedPOs
        'Dim result As Integer

        Dim az As String = e.Cell.Tag.ToString()
        Dim st() As String
        st = az.Split(CChar("|"))
        Dim regionID As Integer = CInt(st(1))
        Dim PONumber As Integer = CInt(st(0))
        Dim sessionID As Integer = CInt(st(2))
        Try
            dal.ConfirmPOInIRMA(regionID, PONumber, sessionID)
            ErrorLabel.ForeColor = Drawing.Color.Green
            ErrorLabel.Text = "PO " & PONumber.ToString & "Successfully confirmed in IRMA"
            ult.DataBind()

        Catch ex As Exception
            ErrorLabel.Text = "ERROR in confirming PO " & PONumber.ToString & " in IRMA "
        End Try
    End Sub

    Protected Sub ult_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles ult.InitializeLayout
        e.Layout.Pager.AllowPaging = True
        e.Layout.Pager.PageSize = 25
        e.Layout.Pager.PagerStyle.Font.Size = 16

        With e.Layout.Bands(0).Columns.FromKey("UploadSessionHistoryID")
            .Header.Caption = "Session ID"
            .Width = 75
            .Key = "UploadSessionHistoryID"
            .Hidden = True
        End With
        With e.Layout.Bands(0).Columns.FromKey("POHeaderID")
            .Hidden = True
            .Key = "POHeaderID"
        End With
        With e.Layout.Bands(0).Columns.FromKey("IRMAPONUmber")

            .Key = "IRMAPONUmber"
            .Width = 75
            .Header.Caption = "IRMA <br>PO Number"
        End With
        With e.Layout.Bands(0).Columns.FromKey("RegionID")
            .Header.Caption = "Region ID"
            .Width = 75
            .Hidden = True
        End With
        With e.Layout.Bands(0).Columns.FromKey("RegionName")
            .Header.Caption = "Region"
            .Width = 75
            .Hidden = False
        End With
        With e.Layout.Bands(0).Columns.FromKey("PONumber")
            .Header.Caption = "PO Number"
            .Width = 75
            .Hidden = False
        End With
        With e.Layout.Bands(0).Columns.FromKey("BusinessUnit")
            .Header.Caption = "Business Unit"
            .Width = 75
            .Hidden = False
            .FilterIcon = True
        End With
        With e.Layout.Bands(0).Columns.FromKey("StoreAbbr")
            .Header.Caption = "Store"
            .Width = 75
            .FilterIcon = True
        End With
        With e.Layout.Bands(0).Columns.FromKey("Subteam")
            .Header.Caption = "Subteam"
            .Width = 75
            .Hidden = False
        End With
        With e.Layout.Bands(0).Columns.FromKey("VendorName")
            .Header.Caption = "Vendor"
            .Width = 75
            .Hidden = False
            .FilterIcon = True
        End With
        With e.Layout.Bands(0).Columns.FromKey("VendorPSNumber")
            .Header.Caption = "Vendor PS #"
            .Width = 75
            .Hidden = False
        End With
        With e.Layout.Bands(0).Columns.FromKey("OrderItemCount")
            .Header.Caption = "Item Count"
            .Width = 75
            .Hidden = False
        End With
        With e.Layout.Bands(0).Columns.FromKey("TotalPOCost")
            .Header.Caption = "Total PO Cost"
            .Width = 75
            .Hidden = False
        End With
        With e.Layout.Bands(0).Columns.FromKey("CreatedDate")
            .Header.Caption = "Created Date"

            If Session("regionName") = "United Kingdom" Then
                .Format = Format("dd/MM/yyyy")
            Else
                .Format = Format("MM/dd/yyyy")
            End If

            .Width = 75
            .Hidden = False
            .FilterIcon = True
        End With
        With e.Layout.Bands(0).Columns.FromKey("ExpectedDate")
            .Header.Caption = "Expected Date"

            If Session("regionName") = "United Kingdom" Then
                .Format = Format("dd/MM/yyyy")
            Else
                .Format = Format("MM/dd/yyyy")
            End If

            .Width = 75
            .Hidden = False
        End With
        With e.Layout.Bands(0).Columns.FromKey("PushedToIRMADate")
            .Header.Caption = "Pushed To IRMA"

            If Session("regionName") = "United Kingdom" Then
                .Format = Format("dd/MM/yyyy")
            Else
                .Format = Format("MM/dd/yyyy")
            End If

            .Width = 75
            .Hidden = False
            .FilterIcon = True
        End With

        With e.Layout.Bands(0).Columns.FromKey("AutoPushDate")
            .Header.Caption = "Auto Push Date"

            If Session("regionName") = "United Kingdom" Then
                .Format = Format("dd/MM/yyyy")
            Else
                .Format = Format("MM/dd/yyyy")
            End If

            .Width = 75
            .Hidden = False
            .FilterIcon = True
        End With

        With e.Layout.Bands(0).Columns.FromKey("Notes")
            .Header.Caption = "Notes"
            .Width = 300
            .Hidden = True
        End With
        With e.Layout.Bands(0).Columns.FromKey("UploadedBy")
            .Header.Caption = "Uploaded By"
            .Width = 75
            .Hidden = False
            .FilterIcon = True
        End With
        With e.Layout.Bands(0).Columns.FromKey("PushedBy")
            .Header.Caption = "Pushed By"
            .Width = 75
            .Hidden = False
            .FilterIcon = True
        End With

        ult.DataKeyField = "POHeaderID"
    End Sub

    Protected Sub UpdateGrid()
        Try
            ult.DataBind()
        Catch ex As Exception
            Dim send As New BOExceptions
            send.SendErrorEmail("PushedToIRMA.aspx", "btnGet_Click", ex.Message)

        End Try
    End Sub

    Private Sub rdoPOType_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles rdoPOType.SelectedIndexChanged
        Session("POType") = Me.rdoPOType.SelectedValue
        Me.UpdateGrid()
    End Sub
End Class
