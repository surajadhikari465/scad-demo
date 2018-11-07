
Partial Class UserInterface_ItemVendor_ItemVendors
    Inherits System.Web.UI.Page




    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Label6.Text = Session("Store_Name")
        Label9.Text = Nothing
        Label10.Text = Nothing
        Label3.Text = Request.QueryString("u")
        Label4.Text = Request.QueryString("d")
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView1.SelectedIndexChanged

        ' *************************************************
        Dim ia As New ItemAuth()
        ' *************************************************
        Dim cntVendors As Integer = GridView1.Rows.Count
        Dim auth As String = GridView1.SelectedRow.Cells(3).Text
        Dim prim As String = GridView1.SelectedRow.Cells(4).Text
        Dim vendorID As Integer = GridView1.SelectedDataKey.Item("Vendor_ID").ToString
        Dim ItemKey As Integer = GridView1.SelectedDataKey.Item("Item_Key").ToString
        Dim Store_No As Integer = Session("Store_No")
        ' ******** Select Appropriate Action **************
        If cntVendors > 1 And auth = "Y" And prim = "N" Then
            ' ************ Swap Vendors **********
            Try
                ia.SetPrimaryVendor(Store_No, ItemKey, vendorID)
                Label10.Text = "Primary Vendor Changed "
            Catch ex As Exception
                Label9.Text = "Primary Vendor Could Not be Set!"
                Debug.WriteLine(ex.Message)
                Error_Log.throwException(ex.Message, ex)
            End Try
        ElseIf cntVendors = 1 And auth = "N" And prim = "N" Then
            ' ************ Make StoreItemVendor Entry and Primary ****
            Try
                ia.InsertStoreItemVendor(vendorID, Store_No, ItemKey)
                ia.SetPrimaryVendor(Store_No, ItemKey, vendorID)
                Label10.Text = "Item Authorized and Vendor set to Primary"
            Catch ex As Exception
                Label9.Text = "Authorization Unsuccessful!"
                Debug.WriteLine(ex.Message)
                Error_Log.throwException(ex.Message, ex)
            End Try
        ElseIf cntVendors = 1 And auth = "Y" And prim = "N" Then
            ' ************ Make Vendor primary **********************
            Try
                ia.InsertStoreItemVendor(vendorID, Store_No, ItemKey)
                ia.SetPrimaryVendor(Store_No, ItemKey, vendorID)
                Label10.Text = "Vendor set to Primary"
            Catch ex As Exception
                Label9.Text = "Primary Vendor Could Not be Set!"
                Debug.WriteLine(ex.Message)
                Error_Log.throwException(ex.Message, ex)
            End Try
        ElseIf cntVendors = 1 And auth = "Y" And prim = "Y" Then
            ' ************ De-authorize *****************************
            Try
                ia.DeleteStoreItemVendor(vendorID, Store_No, ItemKey, Date.Today)
                Label10.Text = "Item De-Authorized"
            Catch ex As Exception
                Label9.Text = "De-Authorization Unsuccessful!"
                Debug.WriteLine(ex.Message)
                Error_Log.throwException(ex.Message, ex)
            End Try
        ElseIf cntVendors > 1 And auth = "Y" And prim = "Y" Then
            ' *************************************************
            ' ************ De-authorize and try to swap ***********
            Try
                ia.DeleteStoreItemVendor(vendorID, Store_No, ItemKey, Date.Today)
                Label10.Text = "Item De-Authorized"
            Catch ex As Exception
                Label9.Text = "De-Authorization Unsuccessful!"
                Debug.WriteLine(ex.Message)
                Error_Log.throwException(ex.Message, ex)
            End Try
        ElseIf cntVendors = 1 And auth = "N" And prim = "Y" Then
            ' ***************************************************
            ' *********** Authorize the Vendor ******************
            Try
                ia.InsertStoreItemVendor(vendorID, Store_No, ItemKey)
                Label10.Text = "Item Authorized for " & Session("Store_Name")
            Catch ex As Exception
                Label9.Text = "Authorization Unsuccessful!"
                Debug.WriteLine(ex.Message)
                Error_Log.throwException(ex.Message, ex)
            End Try
        ElseIf cntVendors > 1 And auth = "N" And prim = "Y" Then
            ' ***************************************************
            ' *********** Authorize the Vendor and Swap vendor ******************
            Try
                ia.InsertStoreItemVendor(vendorID, Store_No, ItemKey)
                ia.SetPrimaryVendor(Store_No, ItemKey, vendorID)
                Label10.Text = "Item Authorized for " & Session("Store_Name")
            Catch ex As Exception
                Label9.Text = "Authorization Unsuccessful!"
                Debug.WriteLine(ex.Message)
                Error_Log.throwException(ex.Message, ex)
            End Try
        End If
        ' *************************************************
        GridView1.SelectedIndex() = -1
        GridView1.DataBind()
        ia = Nothing
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim ia As New ItemAuth()
        Dim ItemKey As Integer = Request.QueryString("ItemKey")
        If WarehouseTxBx.Text = "" Then
            Label9.Text = "VendorItemID was not supplied!"
        ElseIf VendorDropDown.SelectedValue = 0 Then
            Label9.Text = "Vendor was not supplied!"
        Else
            Try
                ia.InsertItemVendor(ItemKey, _
                VendorDropDown.SelectedValue, _
                WarehouseTxBx.Text.ToString.ToUpper)
            Catch ex As Exception
                Label9.Text = "Vendor Not Authorized!"
                Debug.WriteLine(ex.Message)
                Error_Log.throwException(ex.Message, ex)
            End Try
            GridView1.SelectedIndex() = -1
            GridView1.DataBind()
        End If
    End Sub
End Class
