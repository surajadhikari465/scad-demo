Imports System.Collections.Generic

Partial Class UserInterface_ItemSearch
    Inherits System.Web.UI.UserControl

#Region "Member Variables"

    Private _filterByStore As Boolean = False
    Private _subTeamExclusionList As String = Nothing

#End Region

#Region "Properties"

    Public Property FilterByStore() As Boolean
        Get
            Return _filterByStore
        End Get
        Set(ByVal value As Boolean)
            _filterByStore = value
        End Set
    End Property

    Public Property SubTeamExclusionList() As String
        Get
            Return _subTeamExclusionList
        End Get
        Set(ByVal value As String)
            _subTeamExclusionList = value
        End Set
    End Property

    Private ReadOnly Property IsMobilePageView() As Boolean
        Get
            Return (TypeOf Me.Page Is MobileViewablePage AndAlso CType(Me.Page, MobileViewablePage).IsMobileDevice)
        End Get
    End Property

#End Region

#Region "Helper Methods"

    Private Sub ToggleControlPairVisibility(ByVal toHide As Control, ByVal toShow As Control)
        toHide.Visible = False
        toShow.Visible = True
    End Sub

    Private Sub AddMobileParameter(ByVal builder As StringBuilder, ByVal textBox As TextBox, ByVal queryStringKey As String)
        If textBox.Text.Trim().Length > 0 Then
            builder.AppendFormat("&{0}={1}", queryStringKey, HttpUtility.UrlEncode(textBox.Text))
        Else
            builder.AppendFormat("&{0}=", queryStringKey)
        End If
    End Sub

#End Region

    Protected Sub SearchItem_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles SearchItem.Click
        Page.Validate()
        If Page.IsValid Then
            ' Declare variables
            Dim upc As String = String.Empty
            Dim desc As String = String.Empty
            Dim brand As String = String.Empty
            Dim vendor As String = String.Empty
            Dim team As String = String.Empty
            Dim subteam As String = String.Empty
            Dim category As String = String.Empty
            Dim level3 As String = String.Empty
            Dim level4 As String = String.Empty
            Dim extratext As String = String.Empty
            Dim mobileParameterBuilder As New StringBuilder()
            Dim url As String = String.Empty
            Dim Jurisdiction As String = String.Empty
            ' Get values from the form
            upc = upcTxBx.Text.ToString.Trim()
            desc = descTxBx.Text.ToString.Trim()
            extratext = TextBox_ExtraText.Text.Trim()

            AddMobileParameter(mobileParameterBuilder, txtTeam, "tn")
            AddMobileParameter(mobileParameterBuilder, txtSubTeam, "stn")
            AddMobileParameter(mobileParameterBuilder, txtCategory, "cn")
            AddMobileParameter(mobileParameterBuilder, txtLevel3, "l3n")
            AddMobileParameter(mobileParameterBuilder, txtLevel4, "l4n")

            If IsMobilePageView Then
                AddMobileParameter(mobileParameterBuilder, txtBrand, "bn")
                AddMobileParameter(mobileParameterBuilder, txtVendor, "vn")
            Else
                team = ddlTeam.SelectedValue
                subteam = DropDown_SubTeam.SelectedValue
                category = DropDown_Category.SelectedValue
                level3 = DropDown_Level3.SelectedValue
                level4 = DropDown_Level4.SelectedValue
                Jurisdiction = DropDown_Jurisdiction.SelectedValue

                If txtBrand.Text.Trim().Length > 0 Then
                    Dim brandId As Nullable(Of Integer) = ItemSearch.GetBrandIDByName(txtBrand.Text)

                    If brandId.HasValue Then
                        brand = brandId.Value.ToString()
                    End If
                End If

                If txtVendor.Text.Trim.Length > 0 Then
                    Dim vendorId As Nullable(Of Integer) = ItemSearch.GetVendorIDByName(txtVendor.Text)

                    If vendorId.HasValue Then
                        vendor = vendorId.Value.ToString()
                    End If
                End If

                mobileParameterBuilder.Append("&bn=&vn=")
            End If

            ' Set defaults for empty values
            If upc.Equals(String.Empty) Then upc = "*"
            If desc.Equals(String.Empty) Then desc = "*"
            If category.Equals(String.Empty) Then category = "0"
            If level3.Equals(String.Empty) Then level3 = "0"
            If level4.Equals(String.Empty) Then level4 = "0"
            If extratext.Equals(String.Empty) Then extratext = "*"
            If Jurisdiction.Equals(String.Empty) Then Jurisdiction = "0"
            ' Redirect 
            Response.Redirect(String.Format("SearchResults.aspx?u={0}&d={1}&t={2}&s={3}&c={4}&3={5}&4={6}&b={7}&v={8}&x={9}&j={10}{11}", upc, desc, team, subteam, category, level3, level4, brand, vendor, extratext, Jurisdiction, mobileParameterBuilder.ToString()))
        End If
    End Sub

    Protected Sub DropDown_SubTeam_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles DropDown_SubTeam.SelectedIndexChanged
        ' Filter Brands by sub-team
        acBrand.ContextKey = DropDown_SubTeam.SelectedValue
    End Sub

    Private Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If _filterByStore Then
                Dim builder As New StringBuilder()

                If Session("AccessLevel") = 1 Then
                    builder.Append(Session("UserID"))
                End If

                builder.Append(":")

                If Not String.IsNullOrEmpty(_subTeamExclusionList) Then
                    For Each item As String In _subTeamExclusionList.Split(",")
                        builder.AppendFormat("{0},", item)
                    Next
                End If

                cddSubTeam.ContextKey = builder.ToString()
            End If

            If Me.IsMobilePageView Then
                ' Hide drop down lists and disable auto-completion for mobile users
                ToggleControlPairVisibility(ddlTeam, txtTeam)
                ToggleControlPairVisibility(DropDown_SubTeam, txtSubTeam)
                ToggleControlPairVisibility(DropDown_Category, txtCategory)
                ToggleControlPairVisibility(DropDown_Level3, txtLevel3)
                ToggleControlPairVisibility(DropDown_Level4, txtLevel4)
                ToggleControlPairVisibility(DropDown_Jurisdiction, txtJurisdiction)
            End If
        End If
    End Sub

End Class
