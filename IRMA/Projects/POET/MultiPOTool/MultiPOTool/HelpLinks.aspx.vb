Imports Infragistics.WebUI
Imports Infragistics.WebUI.UltraWebGrid

Partial Public Class HelpLinks
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    'Protected Sub uwgHelpLinks_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles uwgHelpLinks.InitializeLayout
    '    Dim a As New BOHelpLinks
    '    Dim isAdmin As Boolean
    '    isAdmin = a.IsAdmin(CInt(Session("UserID")))

    '    uwgHelpLinks.DataKeyField = "HelpLinksID"

    '    e.Layout.Pager.AllowPaging = False
    '    e.Layout.GridLinesDefault = Infragistics.WebUI.UltraWebGrid.UltraGridLines.None

    '    If Not isAdmin Then
    '        e.Layout.AllowAddNewDefault = AllowAddNew.No
    '        e.Layout.AllowDeleteDefault = AllowDelete.No
    '        e.Layout.AllowUpdateDefault = AllowUpdate.No
    '        e.Layout.Bands(0).Columns.FromKey("OrderOfAppearance").Hidden = True
    '        e.Layout.Bands(0).ColHeadersVisible = Infragistics.WebUI.UltraWebGrid.ShowMarginInfo.No
    '    End If

    '    With e.Layout.AddNewBox
    '        .Hidden = True
    '    End With
    '    With e.Layout.Bands(0).Columns.FromKey("HelpLinksID")
    '        .Hidden = True
    '        .Key = "HelpLinksID"
    '    End With
    '    With e.Layout.Bands(0).Columns.FromKey("LinkDescription")
    '        .Header.Caption = "Link Description"
    '        .Width = 200
    '    End With
    '    With e.Layout.Bands(0).Columns.FromKey("LinkURL")
    '        .Type = Infragistics.WebUI.UltraWebGrid.ColumnType.HyperLink
    '        .Header.Caption = "URL"
    '        .Width = 500
    '    End With
    '    'With e.Layout.Bands(0).Columns.FromKey("OrderOfAppearance")
    '    '    .Header.Caption = "OrderOfAppearance"
    '    '    If Not isAdmin Then
    '    '        .Hidden = True
    '    '    End If
    '    'End With

    'End Sub

    'Protected Sub uwgHelpLinks_AddRow(ByVal sender As System.Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles uwgHelpLinks.AddRow
    '    Dim param As New ArrayList
    '    Dim a As New BOHelpLinks
    '    If e.Row.DataChanged = DataChanged.Added Then

    '        ' ***** Get parameters ******
    '        With e.Row.Cells
    '            param.Add(.Item(1).Value)
    '            param.Add(.Item(2).Value)
    '            param.Add(CInt(Session("UserID")))
    '            param.Add(.Item(3).Value)
    '        End With
    '        Try
    '            a.InsertLink(param)
    '        Catch ex As Exception
    '        End Try

    '        uwgHelpLinks.DataBind()
    '    End If
    'End Sub

    'Protected Sub uwgHelpLinks_UpdateRow(ByVal sender As System.Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles uwgHelpLinks.UpdateRow
    '    Dim param As New ArrayList
    '    Dim a As New BOHelpLinks
    '    If e.Row.DataChanged = DataChanged.Modified Then

    '        ' ***** Get parameters ******
    '        With e.Row.Cells
    '            param.Add(.Item(0).Value)
    '            param.Add(.Item(1).Value)
    '            param.Add(.Item(2).Value)
    '            param.Add(CInt(Session("UserID")))
    '            param.Add(.Item(3).Value)
    '        End With
    '        Try
    '            a.UpdateLink(param)
    '        Catch ex As Exception
    '        End Try

    '        uwgHelpLinks.DataBind()
    '    End If
    'End Sub

    'Protected Sub uwgHelpLinks_DeleteRow(ByVal sender As System.Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles uwgHelpLinks.DeleteRow

    '    Dim a As New BOHelpLinks
    '    Dim helpLinksID As Integer
    '    helpLinksID = CInt(e.Row.Cells.Item(0).Value)
    '    If e.Row.DataChanged = DataChanged.Deleted Then
    '        a.DeleteLink(helpLinksID)
    '    End If

    'End Sub
End Class