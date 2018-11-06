Imports Infragistics.WebUI
Imports Infragistics.Documents.Report

Partial Public Class Exceptions
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load, uwgExceptions.Load

        If Session("UserID") Is Nothing Then
            lblExceptions.Text = "There isn't anyone logged in"
        Else
            lblExceptions.Text = "Validation Exceptions for " + Session.Item("UserName").ToString
        End If

        Me.uwgExceptions.DisplayLayout.ViewType = UltraWebGrid.ViewType.OutlookGroupBy

    End Sub

    Protected Sub dsExceptions_Selecting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles dsExceptions.Selecting

        Dim sessionID As Integer

        If ddlExceptionSessions.SelectedValue = "" Then
            sessionID = 0
        Else : sessionID = CInt(ddlExceptionSessions.SelectedValue)
        End If
        e.InputParameters("UploadSessionHistoryID") = sessionID

        If ddlPaging.SelectedValue.ToString() = "all" Then
            uwgExceptions.DisplayLayout.Pager.AllowPaging = False
        Else
            uwgExceptions.DisplayLayout.Pager.AllowPaging = True
            uwgExceptions.DisplayLayout.Pager.PageSize = CInt(ddlPaging.SelectedValue)
        End If

    End Sub

    Protected Sub uwgExceptions_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles uwgExceptions.InitializeLayout

        e.Layout.FilterOptionsDefault.FilterUIType = UltraWebGrid.FilterUIType.FilterRow
        e.Layout.FilterOptionsDefault.AllowRowFiltering = UltraWebGrid.RowFiltering.OnServer
        e.Layout.FilterOptionsDefault.RowFilterMode = UltraWebGrid.RowFilterMode.AllRowsInBand
        e.Layout.Pager.PagerStyle.Font.Size = 16
        'e.Layout.Pager.AllowPaging = True
        'e.Layout.Pager.PageSize = 25

        'If ddlExceptionsPaging.SelectedValue.ToString() = "all" Then
        '    uwgExceptions.DisplayLayout.Pager.AllowPaging = False
        'Else
        '    uwgExceptions.DisplayLayout.Pager.AllowPaging = True
        '    uwgExceptions.DisplayLayout.Pager.PageSize = CInt(ddlExceptionsPaging.SelectedValue)
        'End If

        'uwgExceptions.Browser = UltraWebGrid.BrowserLevel.Xml
        'e.Layout.LoadOnDemand = UltraWebGrid.LoadOnDemand.Xml


        'Me.uwgExceptions.DisplayLayout.Bands(0).Columns.FromKey("Identifier").IsGroupByColumn = True
        'Me.uwgExceptions.DisplayLayout.Bands(0).Columns.FromKey("ItemBrand").IsGroupByColumn = True
        'Me.uwgExceptions.DisplayLayout.Bands(0).Columns.FromKey("Subteam").IsGroupByColumn = True
        'Me.uwgExceptions.DisplayLayout.Bands(0).Columns.FromKey("RegionName").IsGroupByColumn = True

        With e.Layout.Bands(0).Columns.FromKey("Identifier")
            .Header.Caption = "Identifier"
            .Width = 125
        End With
        With e.Layout.Bands(0).Columns.FromKey("ItemBrand")
            .Header.Caption = "Brand"
            .Width = 125
        End With
        With e.Layout.Bands(0).Columns.FromKey("ItemDescription")
            .Header.Caption = "Item Description"
            .Width = 200
        End With
        With e.Layout.Bands(0).Columns.FromKey("Subteam")
            .Header.Caption = "Subteam"
            .Width = 50
        End With
        With e.Layout.Bands(0).Columns.FromKey("VendorName")
            .Header.Caption = "Vendor"
            .Width = 125
        End With
        With e.Layout.Bands(0).Columns.FromKey("RegionName")
            .Header.Caption = "Region"
            .Width = 125
        End With

        With e.Layout.Bands(1).Columns.FromKey("SessionName")
            .Header.Caption = "Session"
            .Width = 75
            .Hidden = True
        End With
        With e.Layout.Bands(1).Columns.FromKey("UploadedDate")
            .Header.Caption = "Uploaded Date"
            .Width = 125
            .Format = Format("g")
        End With
        With e.Layout.Bands(1).Columns.FromKey("UserName")
            .Header.Caption = "User"
            .Width = 75
            .Hidden = True
        End With
        With e.Layout.Bands(1).Columns.FromKey("VendorName")
            .Header.Caption = "Vendor"
            .Width = 125
        End With
        With e.Layout.Bands(1).Columns.FromKey("VendorPSNumber")
            .Header.Caption = "Vendor PS Number"
            .Width = 100
        End With
        With e.Layout.Bands(1).Columns.FromKey("RegionName")
            .Header.Caption = "Region"
            .Width = 125
        End With
        With e.Layout.Bands(1).Columns.FromKey("BusinessUnit")
            .Header.Caption = "Business Unit"
            .Width = 75
        End With
        With e.Layout.Bands(1).Columns.FromKey("StoreAbbr")
            .Header.Caption = "Store"
            .Width = 50
        End With
        With e.Layout.Bands(1).Columns.FromKey("PONumber")
            .Header.Caption = "PO Number"
            .Width = 75
            .AllowRowFiltering = False
        End With
        With e.Layout.Bands(1).Columns.FromKey("Identifier")
            .Header.Caption = "Identifier"
            .Width = 125
        End With
        With e.Layout.Bands(1).Columns.FromKey("VendorItemNumber")
            .Header.Caption = "VIN"
            .Width = 150
        End With
        With e.Layout.Bands(1).Columns.FromKey("Subteam")
            .Header.Caption = "Subteam"
            .Width = 50
        End With
        With e.Layout.Bands(1).Columns.FromKey("ItemBrand")
            .Header.Caption = "Brand"
            .Width = 125
        End With
        With e.Layout.Bands(1).Columns.FromKey("ItemDescription")
            .Header.Caption = "Item Description"
            .Width = 200
        End With
        With e.Layout.Bands(1).Columns.FromKey("ExceptionDescription")
            .Header.Caption = "Exception"
            .Width = 175
        End With
        With e.Layout.Bands(1).Columns.FromKey("ValidationAttemptDate")
            .Header.Caption = "Validation Date"
            .Width = 75
            .Format = Format("g")
        End With

    End Sub

    Protected Sub dsExceptionSessions_Selecting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles dsExceptionSessions.Selecting

        e.InputParameters("UserID") = Session("UserID")

    End Sub

    Protected Sub btnExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnExport.Click

        Select Case ddlExportFormat.SelectedValue.ToString()
            Case "PDF"
                udeExportExceptions.Format = FileFormat.PDF
                udeExportExceptions.DownloadName = "Exceptions.pdf"
                udeExportExceptions.Export(uwgExceptions)
            Case "Text"
                udeExportExceptions.Format = FileFormat.PlainText
                udeExportExceptions.DownloadName = "Exceptions.txt"
                udeExportExceptions.Export(uwgExceptions)
            Case "Excel"
                Dim currentPaging As String
                currentPaging = ddlPaging.SelectedValue.ToString()
                ddlPaging.SelectedValue = "all"
                uwgExceptions.DataBind()
                udeExcelExportExceptions.DownloadName = "Exceptions.xls"
                udeExcelExportExceptions.Export(uwgExceptions)
                ddlPaging.SelectedValue = currentPaging
                uwgExceptions.DataBind()
        End Select

    End Sub

    Protected Sub btnClearFilters_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClearFilters.Click
        With uwgExceptions.DisplayLayout
            .Bands(0).ColumnFilters.Clear()
        End With
        uwgExceptions.DataBind()
    End Sub
End Class