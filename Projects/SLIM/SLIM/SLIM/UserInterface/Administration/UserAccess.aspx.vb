
Partial Class UserInterface_Administration_UserAccess
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim da As New SlimAccessTableAdapters.SlimAccessTableAdapter
        If UserDropDown.SelectedValue = 0 Then
            MsgLabel.Text = "No User Selected!"
        ElseIf da.UserExists(UserDropDown.SelectedValue) Then
            MsgLabel.Text = "User Already Exists!"
        Else
            Try
                da.Insert(UserDropDown.SelectedValue, Nothing, Nothing, _
                            Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, Date.Now)
            Catch ex As Exception
                MsgLabel.Text = ex.Message
            End Try
            MsgLabel.Text = "User Added!"
            da = Nothing
            GridView1.DataBind()
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        MsgLabel.Text = ""
        If Not IsPostBack Then
            ' ****** Check Permission **************
            If Not Session("UserAdmin") = True And Not Session("AccessLevel") = 3 Then
                Response.Redirect("~/AccessDenied.aspx", True)
            End If
            ' *****************************
        End If
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
