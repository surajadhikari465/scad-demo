
Partial Class UserInterface_Main
    Inherits MobileViewablePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Session.Timeout = 60

        ' ************* check if User Logged in ************
        If Not Session("LoggedIn") = 1 Then
            Response.Redirect("../Default.aspx", True)
        End If
        
        '' ***** Getting User Credentials ********
        If Session("UserName") = "" Then
            Session("UserName") = "SLIM Test User"
        End If

        Label2.Text = Session("UserID")
        Label3.Text = Session("UserName")
        Label4.Text = Session("AccessLevel")
        
        If Not IsPostBack Then

            If Session("AccessLevel") = 3 Then

                StoreDropDown.DataSource = SqlDataSource2

                If Not _isMobileDevice Then
                    Master.HideMenuLinks("ISS", "ISSNew", False)
                    Master.HideMenuLinks("ItemRequest", "NewItem", False)
                End If

            Else

                StoreDropDown.DataSource = SqlDataSource1

            End If

            StoreDropDown.DataBind()

            If Session("AccessLevel") > 1 Then
                StoreDropDown.Items.Insert(0, New ListItem("All Stores", "0"))
            End If

            If Not Session("Store_No") Is Nothing Then
                StoreDropDown.SelectedValue = Session("Store_No")
            End If

            If Not UpdateMenuLinks() Then
                pnlUserInfo.Visible = False
            Else
                pnlUserInfo.Visible = True
            End If
        End If


        Try
            If StoreDropDown.SelectedValue = "" Or StoreDropDown.SelectedValue = 0 Then
                Master.HideMenuLinks("ItemRequest", "NewItemRequest", "NewItem", False)
            Else
                Master.HideMenuLinks("ItemRequest", "NewItemRequest", "NewItem", True)
            End If
        Catch ex As Exception
            Master.HideMenuLinks("ItemRequest", "NewItemRequest", "NewItem", False)
            ErrorLabel.Text = "Cannot Get User Permissions"
        End Try

    End Sub

    Protected Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If Session("LoggedIn") = 1 And StoreDropDown.Items.Count > 0 Then
            Session("Store_No") = StoreDropDown.SelectedValue
            Session("Store_Name") = StoreDropDown.SelectedItem.ToString
        End If
    End Sub

    Protected Sub StoreDropDown_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles StoreDropDown.SelectedIndexChanged
        Session("Store_No") = StoreDropDown.SelectedValue
        Session("Store_Name") = StoreDropDown.SelectedItem.ToString

        UpdateMenuLinks()
    End Sub

    Protected Function UpdateMenuLinks() As Boolean
        Dim storeID As Integer

        If Integer.TryParse(StoreDropDown.SelectedValue, storeID) Then
            If Not _isMobileDevice Then
                If Not storeID > 0 Then
                    Master.HideMenuLinks("ISS", "ISSNew", False)
                    Master.HideMenuLinks("ItemRequest", "NewItem", False)
                Else
                    Master.HideMenuLinks("ISS", "ISSNew", True)
                    Master.HideMenuLinks("ItemRequest", "NewItem", True)
                End If
            End If

            Return True
        Else
            Return False
        End If
    End Function
End Class
