
Partial Class PriceBookGenerator
    Inherits System.Web.UI.Page

    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))
    Private oListAllStoresDefault As New System.Web.UI.WebControls.ListItem("**ALL STORES**", CType(1, String))

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        opt52Wk.Text = "52 Weeks" & "(" & Format(DateAdd(DateInterval.Year, -1, Now.Date), "MM/dd/yy") & " - " & Format(Now.Date, "MM/dd/yy") & ")"
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oListItemDefault = Nothing
    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click

        Dim sReportURL As New System.Text.StringBuilder

        Dim Item As ListItem
        Dim sAllStoresList As String
        Dim sSubTeamList As String



        'report name
        sReportURL.Append(Application.Get("region") + "_" + "PricebookReport")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        'report parameters
        sAllStoresList = ""
        If cmbStore.SelectedIndex = 1 Then
            cmbStore.Items.Remove(oListAllStoresDefault)
            For Each Item In cmbStore.Items
                If sAllStoresList = "" Then
                    If Item.Value <> "0" Then sAllStoresList = Mid(Item.Value, 1, InStr(1, Item.Value, "|") - 1)
                Else
                    If Item.Value <> "0" Then sAllStoresList = sAllStoresList & "|" & Mid(Item.Value, 1, InStr(1, Item.Value, "|") - 1)
                End If
            Next

            sReportURL.Append("&All_Stores=1")
            sReportURL.Append("&Store_No=" & sAllStoresList)

        Else
            sReportURL.Append("&All_Stores=0")
            sReportURL.Append("&Store_No=" & cmbStore.SelectedValue)
        End If

        sSubTeamList = ""
        For Each Item In lbSubTeam.Items
            If Item.Selected = True Then
                If sSubTeamList = "" Then
                    sSubTeamList = Item.Value
                Else
                    sSubTeamList = sSubTeamList & "|" & Item.Value
                End If
            End If
        Next

        sReportURL.Append("&SubTeam_No=" & sSubTeamList)

        sReportURL.Append("&Zone=" & cmbZone.SelectedValue)

        If txtVendor.Text = "" Then
            sReportURL.Append("&Vendor_Key=" & cmbVendor.SelectedValue)
        Else
            sReportURL.Append("&Vendor_Key=" & txtVendor.Text)
        End If

        Select Case cmbNarrowCriteria.SelectedValue
            Case 1
                sReportURL.Append("&Brand_Name=" & txtNarrowCriteria.Text)
                sReportURL.Append("&Manufacturer_Number:isnull=true")
                sReportURL.Append("&Item_Description:isnull=true")
                sReportURL.Append("&Price_Type:isnull=true")
            Case 2
                sReportURL.Append("&Brand_Name:isnull=true")
                sReportURL.Append("&Manufacturer_Number=" & txtNarrowCriteria.Text)
                sReportURL.Append("&Item_Description:isnull=true")
                sReportURL.Append("&Price_Type:isnull=true")
            Case 3
                sReportURL.Append("&Brand_Name:isnull=true")
                sReportURL.Append("&Manufacturer_Number:isnull=true")
                sReportURL.Append("&Item_Description=" & txtNarrowCriteria.Text)
                sReportURL.Append("&Price_Type:isnull=true")
            Case 4
                sReportURL.Append("&Brand_Name:isnull=true")
                sReportURL.Append("&Manufacturer_Number:isnull=true")
                sReportURL.Append("&Item_Description:isnull=true")
                sReportURL.Append("&Price_Type=" & txtNarrowCriteria.Text)
            Case Else
                sReportURL.Append("&Brand_Name:isnull=true")
                sReportURL.Append("&Manufacturer_Number:isnull=true")
                sReportURL.Append("&Item_Description:isnull=true")
                sReportURL.Append("&Price_Type:isnull=true")
        End Select

        If optNone.Checked Then
            sReportURL.Append("&Movement_Type=NONE")
        Else
            If optPTD.Checked Then
                sReportURL.Append("&Movement_Type=PTD")
            End If

            If optQTD.Checked Then
                sReportURL.Append("&Movement_Type=QTD")
            End If

            If optYTD.Checked Then
                sReportURL.Append("&Movement_Type=YTD")
            End If

            If opt52Wk.Checked Then
                sReportURL.Append("&Movement_Type=52WK")
            End If
        End If

        If Me.chkIncludeISS.Checked Then sReportURL.Append("&IncludeISS=true") Else sReportURL.Append("&IncludeISS=false")

        sReportURL.Append("&Report_Format=" & cmbPricebookFormat.SelectedValue)

        'show the report
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())

    End Sub

    Protected Sub cmbVendor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbVendor.DataBound
        Dim oListItemAll As New System.Web.UI.WebControls.ListItem("ALL VENDORS", CType("ALLVENDORS", String))

        'add the default item
        cmbVendor.Items.Insert(0, oListItemDefault)
        cmbVendor.Items.Insert(1, oListItemAll)
    End Sub

    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.DataBound
        'add the default item
        cmbStore.Items.Insert(0, oListItemDefault)
        cmbStore.Items.Insert(1, oListAllStoresDefault)
    End Sub

    Protected Sub cmbNarrowCriteria_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbNarrowCriteria.DataBound
        'add the default item
        cmbNarrowCriteria.Items.Insert(0, oListItemDefault)
    End Sub

    Protected Sub cmbPricebookFormat_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPricebookFormat.DataBound
        'add the default item
        cmbPricebookFormat.Items.Insert(0, oListItemDefault)
    End Sub

    Protected Sub cmbPTD_Date_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPTD_Date.DataBound
        optPTD.Text = "Period To Date" & "(" & Format(CDate(cmbPTD_Date.Text), "MM/dd/yy") & " - " & Format(Now.Date, "MM/dd/yy") & ")"
        cmbPTD_Date.Visible = False
    End Sub

    Protected Sub cmbQTD_Date_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbQTD_Date.DataBound
        optQTD.Text = "Quarter To Date" & "(" & Format(CDate(cmbQTD_Date.Text), "MM/dd/yy") & " - " & Format(Now.Date, "MM/dd/yy") & ")"
        cmbQTD_Date.Visible = False
    End Sub

    Protected Sub cmbYTD_Date_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbYTD_Date.DataBound
        optYTD.Text = "Year To Date" & "(" & Format(CDate(cmbYTD_Date.Text), "MM/dd/yy") & " - " & Format(Now.Date, "MM/dd/yy") & ")"
        cmbYTD_Date.Visible = False
    End Sub

    Private Function IsRegional(ByVal sStoreNo As String) As Boolean
        If Mid(cmbStore.SelectedValue, InStr(1, cmbStore.SelectedValue, "|") + 1) = "1" Then
            IsRegional = True
        Else
            IsRegional = False
        End If
    End Function
End Class
