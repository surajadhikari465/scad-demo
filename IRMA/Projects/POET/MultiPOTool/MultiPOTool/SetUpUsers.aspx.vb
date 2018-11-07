Imports Infragistics.WebUI.UltraWebGrid
Partial Public Class SetUpUsers
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'errorlabel.Text = String.Empty
        'If Not IsPostBack Then
        '    If Session("Administrator") Is Nothing Then

        '        Response.Redirect("ErrorPage.aspx", True)

        '    End If
        'End If


    End Sub

    Private Sub UltraWebGrid1_AddRow(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles UltraWebGrid1.AddRow

        Dim param As New ArrayList
        Dim az As New BOManageUsers
        If e.Row.DataChanged = DataChanged.Added Then
            If e.Row.Cells.Item(1).Value Is Nothing Then
                errorlabel.Text = "Please Enter a UserName!"
                Exit Sub
            End If
            ' ***** Get parameters ******
            With e.Row.Cells
                param.Add(.Item(1).Value)
                param.Add(CInt(.Item(2).Value))
                param.Add(.Item(3).Value)
                param.Add(.Item(4).Value)
                param.Add(.Item(5).Value)
                param.Add(.Item(7).Value)
                param.Add(.Item(8).Value)
            End With
            Try
                az.InsertUser(param)
            Catch ex As Exception
                errorlabel.Text = ex.Message.ToString
            End Try

            'databinding in this method throws object reference not set error... 
            'moved to UpdateGrid event
            'UltraWebGrid1.DataBind()

        End If
    End Sub

    Private Sub UltraWebGrid1_DeleteRow(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles UltraWebGrid1.DeleteRow
        Dim az As New BOManageUsers
        Try
            az.DeleteUser(CInt(e.Row.Cells.Item(0).Value))
        Catch ex As Exception
            errorlabel.Text = ex.Message.ToString
        End Try
        UltraWebGrid1.DataBind()
    End Sub


    Private Sub UltraWebGrid1_InitializeLayout(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.LayoutEventArgs) Handles UltraWebGrid1.InitializeLayout

        Dim a As New BOManageUsers
        Dim isAdmin As Boolean
        isAdmin = a.IsAdmin(CInt(Session("UserID")))

        If Not isAdmin Then
            e.Layout.Bands(0).AllowAdd = AllowAddNew.No
            e.Layout.Bands(0).AllowDelete = AllowDelete.No
            e.Layout.Bands(0).AllowUpdate = AllowUpdate.No
        End If

        UltraWebGrid1.DataKeyField = "UserID"

        e.Layout.Pager.AllowPaging = True
        e.Layout.Pager.PageSize = 25
        e.Layout.Pager.PagerStyle.Font.Size = 16
        'e.Layout.LoadOnDemand = LoadOnDemand.Automatic

        ' **** Create the Region dropDown *****
        Dim valueList As New ValueList(True)
        Dim regions As New BOManageUsers
        Dim ds As DataSet
        ds = regions.SelectRegions()
        valueList.DataBind(ds, "", "RegionName", "RegionID")
        ' ********************************************************
        With e.Layout.AddNewBox
            .Location = BoxLocation.Top
            .Prompt = "Add New User"

        End With


        With e.Layout.Bands(0).Columns.FromKey("UserID")
            .Hidden = True
            .Key = "UserID"
        End With
        With e.Layout.Bands(0).Columns.FromKey("UserName")
            '.AllowUpdate = AllowUpdate.Yes
            .Width = 100
            .Header.Caption = "User Name"
        End With

        With e.Layout.Bands(0).Columns.FromKey("RegionID")
            .Width = 100
            .Header.Caption = "Region"
            .Type = ColumnType.DropDownList
            .ValueList = valueList
            .Key = "RegionID"
            ' .AllowUpdate = AllowUpdate.Yes
            .AllowNull = False

        End With
        With e.Layout.Bands(0).Columns.FromKey("GlobalBuyer")
            '.AllowUpdate = AllowUpdate.Yes
            .Header.Caption = "Global Buyer"
            .Width = 60

        End With
        With e.Layout.Bands(0).Columns.FromKey("Administrator")
            ' .AllowUpdate = AllowUpdate.Yes
            .Width = 60
            .Header.Caption = "Admin"
            .Width = 60

        End With
        With e.Layout.Bands(0).Columns.FromKey("Active")
            '.AllowUpdate = AllowUpdate.Yes
            .Width = 60
            .Header.Caption = "Active"
            .Width = 60

        End With
        With e.Layout.Bands(0).Columns.FromKey("InsertDate")
            .AllowUpdate = AllowUpdate.No
            .Header.Caption = "Insert Date"

            If Session("regionName") = "United Kingdom" Then
                .Format = Format("dd/MM/yyyy")
            Else
                .Format = Format("MM/dd/yyyy")
            End If

            .Width = 70
        End With

        With e.Layout.Bands(0).Columns.FromKey("Email")
            '.AllowUpdate = AllowUpdate.Yes
            .Header.Caption = "E-Mail"
            .Width = 120
        End With

        With e.Layout.Bands(0).Columns.FromKey("CCEmail")
            '.AllowUpdate = AllowUpdate.Yes
            .Header.Caption = "CC E-Mail"
            .Width = 120

        End With

        With e.Layout.Bands(0).Columns.FromKey("RegionName")
            .Hidden = True
        End With

    End Sub

    Private Sub UltraWebGrid1_UpdateGrid(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.UpdateEventArgs) Handles UltraWebGrid1.UpdateGrid
        UltraWebGrid1.DataBind()
    End Sub

    Private Sub UltraWebGrid1_UpdateRow(ByVal sender As Object, ByVal e As Infragistics.WebUI.UltraWebGrid.RowEventArgs) Handles UltraWebGrid1.UpdateRow

        Dim az As New BOManageUsers
        Dim param As New ArrayList

        If e.Row.DataChanged = DataChanged.Modified Then
            If e.Row.Cells.Item(2).Value Is Nothing Then
                errorlabel.ForeColor = Drawing.Color.Red
                errorlabel.Text = "Please Select a Region"
                Exit Sub
            End If
            ' ***** Get parameters ******
            With e.Row.Cells
                param.Add(.Item(0).Value)
                param.Add(.Item(1).Value)
                param.Add(CInt(.Item(2).Value))
                param.Add(.Item(3).Value)
                param.Add(.Item(4).Value)
                param.Add(.Item(5).Value)
                param.Add(.Item(7).Value)
                param.Add(.Item(8).Value)
            End With
            Try
                az.UpdateUser(param)
            Catch ex As Exception
                errorlabel.Text = ex.Message.ToString
            End Try
            UltraWebGrid1.DataBind()
        End If


    End Sub

    Protected Sub ObjectDataSource1_Selecting(ByVal sender As System.Object, ByVal e As System.Web.UI.WebControls.ObjectDataSourceSelectingEventArgs) Handles ObjectDataSource1.Selecting
        If ddlPaging.SelectedValue.ToString() = "all" Then
            UltraWebGrid1.DisplayLayout.Pager.AllowPaging = False
        Else
            UltraWebGrid1.DisplayLayout.Pager.AllowPaging = True
            UltraWebGrid1.DisplayLayout.Pager.PageSize = CInt(ddlPaging.SelectedValue)
        End If
    End Sub
End Class