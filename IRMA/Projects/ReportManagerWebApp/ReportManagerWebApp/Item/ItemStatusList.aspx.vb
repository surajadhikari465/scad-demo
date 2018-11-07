
Partial Class Item_ItemStatusList
    Inherits System.Web.UI.Page


    Protected Sub Page_InitComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.InitComplete
        Add_Identifier.SelectedValue					= "0"
        Default_Identifier.SelectedValue				= "0"
        Deleted_Identifier.SelectedValue				= "0"
        Discontinue_Item.SelectedValue					= "0"
        EXEDistributed.SelectedValue					= "0"
        Full_Pallet_Only.SelectedValue					= "0"
        HFM_Item.SelectedValue							= "True"
        Keep_Frozen.SelectedValue						= "0"
        LockAuth.SelectedValue							= "0"
        National_Identifier.SelectedValue				= "0"
        NoDistMarkup.SelectedValue						= "0"
        Not_Available.SelectedValue						= "0"
        Organic.SelectedValue							= "0"
        Pre_Order.SelectedValue							= "0"
        Recall_Flag.SelectedValue						= "0"
        Refrigerated.SelectedValue						= "0"
        Remove_Identifier.SelectedValue					= "0"
        Retail_Sale.SelectedValue						= "True"
        Scale_Identifier.SelectedValue					= "0"
        Shipper_Item.SelectedValue						= "0"
		WFM_Item.SelectedValue							= "True"

    End Sub

    Protected Sub btnReport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReport.Click
        Dim sReportURL As New System.Text.StringBuilder


        Dim sVendorItemStatuses As String = String.Empty

        'report name
        sReportURL.Append("ItemStatusList")

        'report display
        If cmbReportFormat.SelectedValue <> "HTML" Then
            sReportURL.Append("&rs:Format=" & cmbReportFormat.SelectedValue)
        End If

        sReportURL.Append("&rs:Command=Render")
        sReportURL.Append("&rc:Parameters=False")

        sReportURL.Append(RdbtnListCheck(Add_Identifier))
        sReportURL.Append(RdbtnListCheck(Default_Identifier))
        sReportURL.Append(RdbtnListCheck(Deleted_Identifier))
        sReportURL.Append(RdbtnListCheck(Discontinue_Item))
        sReportURL.Append(RdbtnListCheck(EXEDistributed))
        sReportURL.Append(RdbtnListCheck(Full_Pallet_Only))
        sReportURL.Append(RdbtnListCheck(HFM_Item))
        sReportURL.Append(RdbtnListCheck(Keep_Frozen))
        sReportURL.Append(RdbtnListCheck(LockAuth))
        sReportURL.Append(RdbtnListCheck(National_Identifier))
        sReportURL.Append(RdbtnListCheck(NoDistMarkup))
        sReportURL.Append(RdbtnListCheck(Not_Available))
        sReportURL.Append(RdbtnListCheck(Organic))
        sReportURL.Append(RdbtnListCheck(Pre_Order))
        sReportURL.Append(RdbtnListCheck(Recall_Flag))
        sReportURL.Append(RdbtnListCheck(Refrigerated))
        sReportURL.Append(RdbtnListCheck(Remove_Identifier))
        sReportURL.Append(RdbtnListCheck(Retail_Sale))
        sReportURL.Append(RdbtnListCheck(Scale_Identifier))
        sReportURL.Append(RdbtnListCheck(Shipper_Item))
		
        For Each item As ListItem In cbListVendorItemStatus.Items
            If item.Selected Then
                sVendorItemStatuses += item.Value & ","
            End If
        Next

        sVendorItemStatuses = sVendorItemStatuses.Remove(sVendorItemStatuses.Length - 1)
        sReportURL.AppendFormat("&VendorItemStatuses={0}", sVendorItemStatuses)


        'Response.Write(sReportServer + sReportURL.ToString())
        'show the report

        'Try
        Response.Redirect(Application.Get("reportingServicesURL") + sReportURL.ToString())
        'Catch ex As Exception
        '    Response.Write("There was an error in processing your request.<br>")
        '    Response.Write("If you are trying to view a report with more than 65,000 rows in excel format, please go back and limit your report")
        'End Try
    End Sub

    Protected Sub btnClearForm_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClearForm.Click
		Add_Identifier.SelectedValue					= "0"
        Default_Identifier.SelectedValue				= "0"
        Deleted_Identifier.SelectedValue				= "0"
        Discontinue_Item.SelectedValue					= "0"
        EXEDistributed.SelectedValue					= "0"
        Full_Pallet_Only.SelectedValue					= "0"
        HFM_Item.SelectedValue							= "True"
        Keep_Frozen.SelectedValue						= "0"
        LockAuth.SelectedValue							= "0"
        National_Identifier.SelectedValue				= "0"
        NoDistMarkup.SelectedValue						= "0"
        Not_Available.SelectedValue						= "0"
        Organic.SelectedValue							= "0"
        Pre_Order.SelectedValue							= "0"
        Recall_Flag.SelectedValue						= "0"
        Refrigerated.SelectedValue						= "0"
        Remove_Identifier.SelectedValue					= "0"
        Retail_Sale.SelectedValue						= "True"
        Scale_Identifier.SelectedValue					= "0"
        Shipper_Item.SelectedValue						= "0"
		WFM_Item.SelectedValue							= "True"

    End Sub

    Protected Function RdbtnListCheck(ByVal RadioButtonListCntrl As RadioButtonList) As String

        If RadioButtonListCntrl.SelectedValue = Nothing Or RadioButtonListCntrl.SelectedValue = "0" Then
            Return "&" + RadioButtonListCntrl.ID.ToString() + ":isnull=true"
        Else
            Return "&" + RadioButtonListCntrl.ID.ToString() + "=" + RadioButtonListCntrl.SelectedValue
        End If
    End Function

   
    Private Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete

    End Sub

    Protected Sub cbListVendorItemStatus_DataBound(ByVal sender As Object, ByVal e As EventArgs) Handles cbListVendorItemStatus.DataBound
        cbListVendorItemStatus.Items.FindByValue("A").Selected = True
    End Sub
End Class


