Imports System.Net

Partial Class UserInterface_MasterPage
    Inherits System.Web.UI.MasterPage

    Dim footer As String

    Protected Sub SiteMapPath1_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles SiteMapPath1.Init
        ' AddHandler SiteMap.SiteMapResolve, AddressOf ResolveSiteMapNode
    End Sub

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        ServerLabel.Text = String.Format("Server: {0}", Dns.GetHostName)
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim VersionDAO As New VersionDAO

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
                Session("Version") = VersionDAO.GetVersionInfo("SLIM").ToString
            End If

        End If

        Label1.Text = "Store Level Item Maintenance - " & Session("Region")
        Label2.Text = "Version " & Session("Version").ToString

        Dim authenticated As Boolean = Session("LoggedIn") = 1

        For Each item As Infragistics.WebUI.UltraWebNavigator.Item In UltraWebMenu1.Items
            Select Case item.Tag
                Case "Admin", "LogOff", "ItemRequest", "Vendor", "ISS", "Authorizations", "ECommerce"
                    item.Hidden = Not authenticated
                Case "EditItemVendor"
                    If Application.Get("ShowItemsVendor") = "False" Or Not authenticated Then
                        item.Hidden = True
                    End If
                Case "RetailCostMaintenance"
                    If Application.Get("ShowRetailCostMaint") = "False" Or Not authenticated Then
                        item.Hidden = True
                    End If
                Case Else
                    ' Do Nothing
            End Select
        Next

    End Sub

    Protected Function ResolveSiteMapNode(ByVal sender As Object, ByVal e As SiteMapResolveEventArgs) As SiteMapNode
        Dim currentNode As SiteMapNode = Nothing

        If SiteMap.CurrentNode IsNot Nothing Then
            currentNode = SiteMap.CurrentNode.Clone(True)

            Dim tempNode As SiteMapNode = currentNode

            While tempNode IsNot Nothing
                If tempNode.Description.Equals("Web Query") Then
                    If e.Context.Session("LoggedIn") = 1 Then
                        tempNode.Title = "Secure WebQuery"
                    Else
                        tempNode.Title = "UnSecure WebQuery"
                    End If

                    Exit While
                End If

                tempNode = tempNode.ParentNode
            End While
        End If

        Return currentNode
    End Function

    Public Overloads Sub HideMenuLinks(ByVal linkName As String, ByVal subLinkName As String, ByVal isEnabled As Boolean)
        For Each item As Infragistics.WebUI.UltraWebNavigator.Item In UltraWebMenu1.Items

            Select Case item.Tag
                Case linkName
                    For Each subItem As Infragistics.WebUI.UltraWebNavigator.Item In item.Items
                        Select Case subItem.Tag
                            Case subLinkName
                                subItem.Enabled = isEnabled
                            Case Else
                        End Select
                    Next
                Case Else
                    ' Do Nothing
            End Select
        Next

    End Sub

    Public Overloads Sub HideMenuLinks(ByVal linkName As String, ByVal subLinkName As String, ByVal subLinkChild As String, ByVal isEnabled As Boolean)
        For Each item As Infragistics.WebUI.UltraWebNavigator.Item In UltraWebMenu1.Items

            Select Case item.Tag
                Case linkName
                    For Each subItem As Infragistics.WebUI.UltraWebNavigator.Item In item.Items
                        Select Case subItem.Tag
                            Case subLinkName
                                For Each subLinkChildItem As Infragistics.WebUI.UltraWebNavigator.Item In subItem.Items
                                    Select Case subLinkChildItem.Tag
                                        Case subLinkChild
                                            subLinkChildItem.Enabled = isEnabled
                                        Case Else
                                    End Select
                                Next
                            Case Else
                        End Select
                    Next
                Case Else
                    ' Do Nothing
            End Select
        Next

    End Sub

    Protected Sub SiteMapPath1_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles SiteMapPath1.Unload
        ' RemoveHandler SiteMap.SiteMapResolve, AddressOf ResolveSiteMapNode
    End Sub
End Class

