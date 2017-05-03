
Partial Class UserInterface_Administration_StoreSubTeam
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles AddUser.Click
        MsgLabel.Text = Nothing
        If UserList.SelectedValue = 0 Then
            MsgLabel.Text = "Please select a User!"
        ElseIf StoreList.SelectedValue = 0 Then
            MsgLabel.Text = "Please select a Store!"
        ElseIf TeamList.SelectedValue = 0 Then
            MsgLabel.Text = "Please select a Team!"
        ElseIf TitleList.SelectedValue = 0 Then
            MsgLabel.Text = "Please select a Title!"
        Else
            ' ***** Get the Team_No ******
            Dim da As New SlimAccessTableAdapters.SlimAccessTableAdapter
            Try
                'Team_No = da.GetTeamBySubTeam(TeamList.SelectedValue)
                ' ************* Insert UserSubTeam ************************
                'da.InsertUserSubTeam(UserList.SelectedValue, _
                'SubTeamList.SelectedValue)
                '' ******** Insert UserStore *************
                da.InsertUserStore(UserList.SelectedValue, _
                StoreList.SelectedValue, TeamList.SelectedValue, TitleList.SelectedValue)
                MsgLabel.Text = "User Store/SubTeam added!"
            Catch ex As Exception
                Debug.WriteLine(ex.Message)
                MsgLabel.Text = ex.Message
            End Try
        End If
        GridView1.DataBind()
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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
