
Imports System.Web.Configuration
Imports System
Imports System.Web
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.DirectoryServices

Namespace pp_irma

    Partial Class _default
        Inherits System.Web.UI.Page
        Dim adoCON As SqlConnection
        Dim strConnection As String

#Region " Web Form Designer Generated Code "

        'This call is required by the Web Form Designer.
        <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

        End Sub


        Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
            'CODEGEN: This method call is required by the Web Form Designer
            'Do not modify it using the code editor.
            InitializeComponent()
        End Sub

#End Region

        Private Sub Page_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
            Session.Timeout = 2400
            strConnection = System.Configuration.ConfigurationManager.ConnectionStrings("PromoPlanner_Conn").ToString
            adoCON = New SqlConnection(strConnection)
        End Sub

        Private Sub login1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles login1.Click
            Dim ad As New pp_irma.AD_Access(Trim(username.Text), Trim(password.Text))
            Dim Authenticated As Boolean
            Dim ValUser, ValGroup As Boolean

            Try
                ValUser = ad.IsValidUser
            Catch ex As Exception
                Response.Write("Error =>" & ex.Message)
            End Try

            ' ************ ********************* *************
            If Not Session("group") Is Nothing Then
                Try
                    ValGroup = True  ' ad.IsValidGroup(Session("group"))
                Catch ex As Exception
                    Response.Write("Error =>" & ex.Message)
                End Try
            End If

            ValGroup = True

            If ValUser And ValGroup Then

                Authenticated = True

            End If

            If (Authenticated) Then

                Dim getPerm As New SqlClient.SqlCommand("SELECT Telxon_Store_Limit = ISNULL(Telxon_Store_Limit, -1), PromoAccessLevel = ISNULL(PromoAccessLevel, -1) FROM Users WHERE UserName = '" & username.Text & "'", adoCON)
                Dim readPerm As SqlClient.SqlDataReader
                Dim store As Integer
                Dim accessLevel As Integer

                adoCON.Open()

                readPerm = getPerm.ExecuteReader

                While readPerm.Read
                    store = readPerm("Telxon_Store_Limit")
                    accessLevel = readPerm("PromoAccessLevel")
                End While

                Session("storeno") = store

                adoCON.Close()

                Select Case accessLevel
                    Case 3
                        If store = -1 Then
                            Response.Redirect("index.aspx")
                        Else
                            Response.Redirect("SecurityError.aspx")
                        End If
                    Case 1
                        If store = -1 Then
                            Response.Redirect("SecurityError.aspx")
                        Else
                            Response.Redirect("promoorders1.aspx")
                        End If
                    Case Else
                        Response.Redirect("SecurityError.aspx")
                End Select

            End If

        End Sub

    End Class

End Namespace
