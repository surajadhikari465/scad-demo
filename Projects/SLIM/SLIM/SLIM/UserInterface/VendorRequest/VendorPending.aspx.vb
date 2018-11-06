Imports SLIM.WholeFoods.IRMA.Common.BusinessLogic
Imports SLIM.WholeFoods.IRMA.Common.DataAccess

Partial Class UserInterface_VendorRequest_VendorPending
    Inherits System.Web.UI.Page

    Dim i As Integer

    Protected Sub GridView1_RowEditing(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewEditEventArgs) Handles GridView1.RowEditing
        Session("EditIndex") = e.NewEditIndex
        Session("DataKey") = GridView1.SelectedDataKey
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' ****** Check Permission **************
        If Not IsPostBack Then
            If Not Session("VendorRequest") = True And Not Session("AccessLevel") = 3 Then
                Response.Redirect("~/AccessDenied.aspx", True)
            End If
        End If
        ' *****************************
        MsgLabel.Text = ""
        UpdateMenuLinks()
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

    Protected Sub GridView1_RowUpdating(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewUpdateEventArgs) Handles GridView1.RowUpdating

        If Not Page.IsValid() Then
            e.Cancel = True
        End If

    End Sub

    Protected Sub ValidateVendorKey(ByVal source As Object, ByVal args As ServerValidateEventArgs)

        If InstanceDataDAO.IsFlagActive("VendorKeyRequired") And args.Value.Length = 0 Then
            args.IsValid = False
        Else
            args.IsValid = True
        End If

    End Sub


End Class
