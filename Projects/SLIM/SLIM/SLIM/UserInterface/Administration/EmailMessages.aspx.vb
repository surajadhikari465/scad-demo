
Partial Class UserInterface_Administration_EmailMessages
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim da As New SlimEmailTableAdapters.SlimEmailTableAdapter
        If TeamDropDown.SelectedValue = 0 Then
            MsgLabel.Text = "No Team Selected!"
        ElseIf StoreDropDown.SelectedValue = 0 Then
            MsgLabel.Text = "No Store Selected!"
        ElseIf da.EmailExists(StoreDropDown.SelectedValue, _
TeamDropDown.SelectedValue).HasValue Then
            MsgLabel.Text = "Email for Store and Team Already Exists!"
        Else
            Try
                da.Insert(StoreDropDown.SelectedValue, TeamDropDown.SelectedValue, _
                Nothing, Nothing, Nothing, Date.Now)
            Catch ex As Exception
                MsgLabel.Text = ex.Message
            End Try
            MsgLabel.Text = "Email Entry Added!"
            da = Nothing
            GridView1.DataBind()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MsgLabel.Text = ""
        ' ****** Check Permission **************
        If Not IsPostBack Then
            If Not Session("UserAdmin") = True And Not Session("AccessLevel") = 3 Then
                Response.Redirect("~/AccessDenied.aspx", True)
            End If
        End If
        ' *****************************

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
End Class
