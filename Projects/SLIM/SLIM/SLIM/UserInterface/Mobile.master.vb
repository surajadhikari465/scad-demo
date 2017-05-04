Partial Class UserInterface_Mobile
    Inherits System.Web.UI.MasterPage

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("Region") = "" Then
                Try
                    Dim da As New UsersTableAdapters.UsersTableAdapter
                    Session("Region") = da.GetRegion()
                Catch ex As Exception
                    Debug.WriteLine(ex.Message)
                End Try
            End If
            If Session("Version") = "" Then
                Try
                    Dim da As New UsersTableAdapters.UsersTableAdapter
                    Session("Version") = da.GetVersion()
                Catch ex As Exception
                    Debug.WriteLine(ex.Message)
                End Try
            End If
        End If
        Label1.Text = "Store Level Item Maintenance - " & Session("Region")
        Label2.Text = "Version " & Session("Version")

        If Session("LoggedIn") <> 1 Then
            lnkAuthorizations.Visible = False
            lnkLogOff.Visible = False
        End If
    End Sub
End Class