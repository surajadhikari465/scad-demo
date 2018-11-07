Imports System.Globalization

Partial Class Item_ItemMaster
    Inherits System.Web.UI.Page

    Private oListItemAll As New System.Web.UI.WebControls.ListItem("< All >", CType(0, String))
    Private oListItemDefault As New System.Web.UI.WebControls.ListItem("< Select a Value >", CType(0, String))

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        If Page.IsValid Then
            Dim sReportURL As New System.Text.StringBuilder

            'report name
            Select Case ReportType.SelectedIndex
                Case 0
                    sReportURL.Append(Application.Get("region") + "_" + "ItemMasterDownload")
                Case 1
                    sReportURL.Append(Application.Get("region") + "_" + "ToledoInfoDownload")
                Case 2
                    sReportURL.Append(Application.Get("region") + "_" + "ItemMasterWithToledo")
            End Select

            'report display
            If cmbReportFormat.SelectedValue <> "HTML" Then
                sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
            End If

            sReportURL.Append("&rs:Command=Render")
            sReportURL.Append("&rc:Parameters=False")

            'If cmbStore.SelectedValue = "ALL" Then
            sReportURL.Append("&Store_No:isnull=true")
            'Else
            '    sReportURL.Append("&Store_No=" & cmbStore.SelectedValue)
            'End If

            If cmbVendor.SelectedValue = 0 Then
                sReportURL.Append("&Vendor_No:isnull=true")
            Else
                sReportURL.Append("&Vendor_No=" & cmbVendor.SelectedValue)
            End If

            If cmbSubTeam.SelectedValue = 0 Then
                sReportURL.Append("&SubTeam_No:isnull=true")
            Else
                sReportURL.Append("&SubTeam_No=" & cmbSubTeam.SelectedValue)
            End If

            If cmbStore.SelectedValue = 0 Then
                sReportURL.Append("&Store_No:isnull=true")
            Else
                sReportURL.Append("&Store_No=" & cmbStore.SelectedValue)
            End If

            If cmbSubDept.SelectedValue = 0 Then
                sReportURL.Append("&SubDept_No:isnull=true")
            Else
                sReportURL.Append("&SubDept_No=" & cmbSubDept.SelectedValue)
            End If

            If cmbClass.SelectedValue = 0 Then
                sReportURL.Append("&Class_No:isnull=true")
            Else
                sReportURL.Append("&Class_No=" & cmbClass.SelectedValue)
            End If

            If cmbSubClass.SelectedValue = 0 Then
                sReportURL.Append("&SubClass_No:isnull=true")
            Else
                sReportURL.Append("&SubClass_No=" & cmbSubClass.SelectedValue)
            End If

            sReportURL.Append("&SKU_Status=" & Mid(cmbSKUStatus.SelectedValue, 1, 1))

            If cmbMerchGp.SelectedValue = 0 Then
                sReportURL.Append("&Merch_Group:isnull=true")
            Else
                sReportURL.Append("&Merch_Group=" & cmbMerchGp.SelectedValue)
            End If

            If cmbSupplierType.SelectedValue = 0 Then
                sReportURL.Append("&Supplier_Type:isnull=true")
            Else
                sReportURL.Append("&Supplier_Type=" & cmbSupplierType.SelectedValue)
            End If

            sReportURL.Append("&Plu_Type=" & cmbPLUType.SelectedValue)

            If txtFromId.Text <> "" Then
                sReportURL.Append("&From_PLU=" & txtFromId.Text)
            Else
                sReportURL.Append("&From_PLU:isnull=true")
            End If

            If txtToId.Text <> "" Then
                sReportURL.Append("&To_PLU=" & txtToId.Text)
            Else
                sReportURL.Append("&To_PLU:isnull=true")
            End If

            Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
        End If
    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        oListItemAll = Nothing
        oListItemDefault = Nothing
    End Sub

    Protected Sub cmbClass_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbClass.DataBound
        'add the default item
        cmbClass.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbMerchGp_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMerchGp.DataBound
        'add the default item
        cmbMerchGp.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbPLUType_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbPLUType.DataBound
        'add the default item
        cmbPLUType.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbReportFormat_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbReportFormat.DataBound
        'add the default item
        cmbReportFormat.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbSKUStatus_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSKUStatus.DataBound
        'add the default item
        cmbSKUStatus.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbSubDept_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubDept.DataBound
        'add the default item
        cmbSubDept.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbSubTeam_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubTeam.DataBound
        'add the default item
        cmbSubTeam.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbSupplierType_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSupplierType.DataBound
        'add the default item
        cmbSupplierType.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbVendor_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbVendor.DataBound
        'add the default item
        cmbVendor.Items.Insert(0, oListItemAll)
    End Sub

    Protected Sub cmbSubClass_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbSubClass.DataBound
        'add the default item
        cmbSubClass.Items.Insert(0, oListItemAll)
    End Sub
    Protected Sub cmbStore_DataBound(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbStore.DataBound
        'add the default item
        cmbStore.Items.Insert(0, oListItemAll)
    End Sub
End Class