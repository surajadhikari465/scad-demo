Option Strict Off
Option Explicit On
Imports log4net
Friend Class frmSearch
    Inherits System.Windows.Forms.Form

    Private mdt As DataTable
    Private mdv As DataView

	Dim rsSearch As dao.Recordset
    Dim bFound As Boolean

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Private Sub LoadDataTable(ByVal sSearchSQL As String)

        logger.Debug("LoadDataTable Entry")

        Dim rsSearch As DAO.Recordset = Nothing
        Dim row As DataRow
        Dim iLoop As Integer
        Dim MaxLoop As Short = 1000

        Try
            rsSearch = SQLOpenRecordSet(sSearchSQL, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor


            'Load the data set.
            mdt.Rows.Clear()
            While (Not rsSearch.EOF) And (iLoop < MaxLoop)
                iLoop = iLoop + 1
                row = mdt.NewRow
                Select Case giSearchType
                    Case iSearchVendorCompany, iSearchAllVendors, iSearchContactContact, iSearchBrand, iSearchShelfLife, iSearchUnit, iSearchOrigin, iSearchOrganizationCompany
                        'Visible on grid.
                        '--------------------
                        row("C1") = rsSearch.Fields("C1").Value
                        row("C2") = rsSearch.Fields("C2").Value

                        'iSearchOrganizationCompany, iSearchVendorCompany, iSearchAllVendors = gridSearchResults.Columns(1).Caption = "Company Name"
                        'iSearchContactContact = gridSearchResults.Columns(1).Caption = "Contact Name"
                        'iSearchBrand =  gridSearchResults.Columns(1).Caption = "Brand Name"
                        'iSearchShelfLife : gridSearchResults.Columns(1).Caption = "Shelf Life Name"
                        'iSearchUnit : gridSearchResults.Columns(1).Caption = "Unit Name"
                        'iSearchOrigin : gridSearchResults.Columns(1).Caption = "Origin Name"
                    Case iSearchCategory
                        row("C1") = rsSearch.Fields("C1").Value
                        row("C2") = rsSearch.Fields("C2").Value
                        row("C3") = rsSearch.Fields("C3").Value

                        'gridSearchResults.Columns(1).Caption = "Category Name"
                        'gridSearchResults.Columns(2).Caption = "Sub-Team Name"
                        'gridSearchResults.Columns(1).Width = (gridSearchResults.Width - GridWidth) / 2
                        'gridSearchResults.Columns(2).Width = (gridSearchResults.Width - GridWidth) / 2
                    Case iSearchToUnit, isearchFromUnit
                        row("C1") = rsSearch.Fields("C1").Value
                        row("C2") = rsSearch.Fields("C2").Value
                        row("C3") = rsSearch.Fields("C3").Value
                        row("C4") = rsSearch.Fields("C4").Value
                        '-iSearchFromUNit:
                        '      gridSearchResults.Columns(1).Caption = "From Unit"
                        '      gridSearchResults.Columns(2).Visible = False
                        '      gridSearchResults.Columns(3).Caption = "To Unit"
                        '      gridSearchResults.Columns(1).Width = (gridSearchResults.Width - GridWidth) / 2
                        '      gridSearchResults.Columns(3).Width = (gridSearchResults.Width - GridWidth) / 2

                        '-iSearchToUnit()
                        '      gridSearchResults.Columns(1).Caption = "To Unit"
                        '      gridSearchResults.Columns(2).Visible = False
                        '      gridSearchResults.Columns(3).Caption = "From Unit"
                        '      gridSearchResults.Columns(1).Width = (gridSearchResults.Width) - GridWidth / 2
                        '      gridSearchResults.Columns(3).Width = (gridSearchResults.Width) - GridWidth / 2
                End Select
                mdt.Rows.Add(row)
                rsSearch.MoveNext()
            End While

            If mdt.Rows.Count > 0 Then
                mdt.AcceptChanges()
                mdv = New System.Data.DataView(mdt)
                ugrdSearchResults.DataSource = mdv
            End If

            'This probably won’t be required in most forms.
            If Not rsSearch.EOF Then
                MsgBox("More data is available." & vbCrLf & "For more data, please limit search criteria.", MsgBoxStyle.Exclamation, "Notice!")
                logger.Info("More data is available." & vbCrLf & "For more data, please limit search criteria.")
            End If

            'This may or may not be required.
            If rsSearch.RecordCount > 0 Then

                ShowColumns()

                'Set the first item to selected.
                ugrdSearchResults.Rows(0).Selected = True
                ugrdSearchResults.Focus()
            Else
                MsgBox("No items found.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
                logger.Info("No items found.")
            End If

        Finally
            If rsSearch IsNot Nothing Then
                rsSearch.Close()
                rsSearch = Nothing
            End If
        End Try

        If iLoop = 1 Then ReturnSelection()

        logger.Debug("LoadDataTable Exit")

exitSub:
        System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        logger.Debug("LoadDataTable Exit from (exitSub:)")

    End Sub

    Private Sub SetupDataTable()
        ' Create a data table
        mdt = New DataTable("SearchResults")
        Select Case giSearchType
            Case iSearchVendorCompany, iSearchAllVendors, iSearchContactContact, iSearchBrand, iSearchShelfLife, iSearchUnit, iSearchOrigin, iSearchOrganizationCompany
                'Visible on grid.
                '--------------------
                mdt.Columns.Add(New DataColumn("C1", GetType(Integer)))
                mdt.Columns.Add(New DataColumn("C2", GetType(String)))

                Select Case giSearchType
                    Case iSearchVendorCompany, iSearchAllVendors, iSearchOrganizationCompany
                        mdt.Columns(1).Caption = "Company name"
                    Case iSearchContactContact
                        mdt.Columns(1).Caption = "Company name"
                    Case iSearchBrand
                        mdt.Columns(1).Caption = "Brand Name"
                    Case iSearchShelfLife
                        mdt.Columns(1).Caption = "Shelf Life Name"
                    Case iSearchUnit
                        mdt.Columns(1).Caption = "Unit Name"
                    Case iSearchOrigin
                        mdt.Columns(1).Caption = "Origin Name"
                End Select
            Case iSearchCategory
                mdt.Columns.Add(New DataColumn("C1", GetType(Integer)))
                mdt.Columns.Add(New DataColumn("C2", GetType(String)))
                mdt.Columns.Add(New DataColumn("C3", GetType(String)))
                mdt.Columns(1).Caption = "Category Name"
                mdt.Columns(2).Caption = "Sub-Team Name"
            Case iSearchToUnit, isearchFromUnit
                mdt.Columns.Add(New DataColumn("C1", GetType(Integer)))
                mdt.Columns.Add(New DataColumn("C2", GetType(String)))
                mdt.Columns.Add(New DataColumn("C3", GetType(String)))
                mdt.Columns.Add(New DataColumn("C4", GetType(String)))

                Select Case giSearchType
                    Case iSearchToUnit
                        mdt.Columns(1).Caption = "To Unit"
                        mdt.Columns(3).Caption = "From Unit"
                    Case isearchFromUnit
                        mdt.Columns(1).Caption = "From Unit"
                        mdt.Columns(3).Caption = "To Unit"
                End Select
        End Select

    End Sub

    Private Sub ShowControls(ByRef iSearchType As Short)
        Select Case iSearchType
            Case iSearchVendorCompany, iSearchAllVendors
                Me.gbxSearchBy.Visible = True
            Case Else
                Me.gbxSearchBy.Visible = False
                Me.gbxBody.Top = Me.gbxSearchBy.Top
                Me.gbxBody.Left = Me.gbxSearchBy.Left
        End Select
    End Sub
    Private Sub ShowColumns()
        Select Case giSearchType
            Case iSearchVendorCompany, iSearchAllVendors, iSearchContactContact, iSearchBrand, iSearchShelfLife, iSearchUnit, iSearchOrigin, iSearchOrganizationCompany
                'Visible on grid.
                '--------------------
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).AllowGroupBy = Infragistics.Win.DefaultableBoolean.False
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).AllowGroupBy = Infragistics.Win.DefaultableBoolean.False
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).AutoSizeEdit = Infragistics.Win.DefaultableBoolean.False
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).Width = 403
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(0).Hidden = True
            Case iSearchCategory
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(0).Hidden = True
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).AllowGroupBy = Infragistics.Win.DefaultableBoolean.False
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).AutoSizeEdit = Infragistics.Win.DefaultableBoolean.False
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).AllowGroupBy = Infragistics.Win.DefaultableBoolean.False
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).Width = 201
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(2).AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(2).AutoSizeEdit = Infragistics.Win.DefaultableBoolean.False
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(2).AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(2).CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(2).CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(2).Width = 201
            Case iSearchToUnit, isearchFromUnit
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(0).Hidden = True
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).AllowGroupBy = Infragistics.Win.DefaultableBoolean.False
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).AutoSizeEdit = Infragistics.Win.DefaultableBoolean.False
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(1).Width = 201
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(2).Hidden = True
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(3).AllowGroupBy = Infragistics.Win.DefaultableBoolean.False
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(3).AutoCompleteMode = Infragistics.Win.AutoCompleteMode.None
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(3).AutoSizeEdit = Infragistics.Win.DefaultableBoolean.False
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(3).AutoSizeMode = Infragistics.Win.UltraWinGrid.ColumnAutoSizeMode.None
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(3).CellActivation = Infragistics.Win.UltraWinGrid.Activation.NoEdit
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(3).CellClickAction = Infragistics.Win.UltraWinGrid.CellClickAction.RowSelect
                ugrdSearchResults.DisplayLayout.Bands(0).Columns(3).Width = 201
        End Select
    End Sub
	Private Sub frmSearch_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
		
		'-- Center the form and the buttons on the form
        CenterForm(Me)

        Call SetupDataTable()

        '-- Format the grid the way you want it seen
        Call ShowControls(giSearchType)

        txtSearch.Focus()

	End Sub

    Private Sub ReturnSelection()

        '-- Make sure one item was selected

        If ugrdSearchResults.Selected.Rows.Count = 1 Then
            Select Case giSearchType
                Case iSearchVendorCompany, iSearchAllVendors
                    glVendorID = ugrdSearchResults.Selected.Rows(0).Cells(0).Value()
                    gsVendorName = ugrdSearchResults.Selected.Rows(0).Cells(1).Value()
                Case iSearchContactContact
                    glContactID = ugrdSearchResults.Selected.Rows(0).Cells(0).Value()
                Case iSearchUnit
                    glUnitID = ugrdSearchResults.Selected.Rows(0).Cells(0).Value()
                Case iSearchBrand
                    glBrandID = ugrdSearchResults.Selected.Rows(0).Cells(0).Value()
                Case iSearchCategory
                    glCategoryID = ugrdSearchResults.Selected.Rows(0).Cells(0).Value()
                Case iSearchOrigin
                    glOriginID = ugrdSearchResults.Selected.Rows(0).Cells(0).Value()
                Case iSearchShelfLife
                    glShelfLifeID = ugrdSearchResults.Selected.Rows(0).Cells(0).Value()
                Case isearchFromUnit, iSearchToUnit
                    glFromUnitID = ugrdSearchResults.Selected.Rows(0).Cells(0).Value()
                    glToUnitID = ugrdSearchResults.Selected.Rows(0).Cells(2).Value()
                Case iSearchOrganizationCompany
                    glOrganizationID = ugrdSearchResults.Selected.Rows(0).Cells(0).Value()
            End Select

            Me.Close()

        Else
            MsgBox("An item from the list must be selected.", MsgBoxStyle.Exclamation, "Error!")
        End If

    End Sub
	
    Sub RefreshGrid()

        Dim lCount As Integer
        Dim iID As Integer
        Dim lID As Long
        Dim fld As DAO.Field
        Dim sSql As String
        Dim sSqlWhere As String
        Dim sParams As String 'company name
        sParams = String.Empty
        sSqlWhere = String.Empty

        txtSearch.Text = ConvertQuotes((txtSearch.Text))

        Select Case giSearchType
            Case iSearchAllVendors
                Select Case True
                    Case Me.optCompany.Checked
                        sSqlWhere = "WHERE CompanyName LIKE '%" & txtSearch.Text & "%' "
                    Case Me.optVendorID.Checked

                        If IsNumeric(txtSearch.Text) And InStr(1, txtSearch.Text, ".") = 0 And InStr(1, txtSearch.Text, "$") = 0 Then
                            'Check if the ID exceeds the value of a long.
                            On Error Resume Next
                            iID = CInt(txtSearch.Text)
                            If Err.Number = 6 Then
                                MsgBox("Value is to big.")
                                Exit Sub
                            End If
                            On Error GoTo 0

                            sSqlWhere = "WHERE Vendor_ID = " & txtSearch.Text & " "
                        Else
                            MsgBox("You have to search on a number")
                            Exit Sub
                        End If
                    Case Me.optPSVendorID.Checked
                        If IsNumeric(txtSearch.Text) And InStr(1, txtSearch.Text, ".") = 0 And InStr(1, txtSearch.Text, "$") = 0 Then
                            'Check if the ID exceeds the value of a long.
                            On Error Resume Next
                            lID = CLng(txtSearch.Text)
                            If Err.Number = 6 Then
                                MsgBox("Value is to big.")
                                Exit Sub
                            End If
                            On Error GoTo 0

                            sSqlWhere = "WHERE PS_Vendor_ID LIKE '%" & txtSearch.Text & "%' "
                        Else
                            MsgBox("You have to search on a number")
                            Exit Sub
                        End If
                End Select
                sSql = "SELECT Vendor_ID AS C1, CompanyName AS C2 FROM Vendor " & sSqlWhere & " ORDER BY CompanyName"
            Case iSearchVendorCompany
                Select Case True
                    Case Me.optCompany.Checked
                        sParams = "'" & txtSearch.Text & "', null, null"
                    Case Me.optVendorID.Checked
                        If IsNumeric(txtSearch.Text) And InStr(1, txtSearch.Text, ".") = 0 And InStr(1, txtSearch.Text, "$") = 0 Then
                            'Check if the ID exceeds the value of a long.
                            On Error Resume Next
                            iID = CInt(txtSearch.Text)
                            If Err.Number = 6 Then
                                MsgBox("Value is to big.")
                                Exit Sub
                            End If
                            On Error GoTo 0

                            sParams = "null, " & txtSearch.Text & ", null"
                        Else
                            MsgBox("You have to search on a number")
                            Exit Sub
                        End If
                    Case Me.optPSVendorID.Checked
                        If IsNumeric(txtSearch.Text) And InStr(1, txtSearch.Text, ".") = 0 And InStr(1, txtSearch.Text, "$") = 0 Then
                            'Check if the ID exceeds the value of a long.
                            On Error Resume Next
                            lID = CLng(txtSearch.Text)
                            If Err.Number = 6 Then
                                MsgBox("Value is to big.")
                                Exit Sub
                            End If
                            On Error GoTo 0

                            sParams = "null, null, " & txtSearch.Text
                        Else
                            MsgBox("You have to search on a number")
                            Exit Sub
                        End If
                End Select
                sSql = "EXEC GetPSVendors " & sParams
            Case iSearchContactContact
                sSql = "SELECT Contact_ID AS C1, Contact_Name AS C2 FROM Contact WHERE Contact_Name LIKE '%" & txtSearch.Text & "%' AND Vendor_ID = " & glVendorID & " ORDER BY Contact_Name"
            Case iSearchBrand
                sSql = "SELECT Brand_ID AS C1, Brand_Name AS C2 FROM ItemBrand WHERE Brand_Name LIKE '%" & txtSearch.Text & "%' ORDER BY Brand_Name"
            Case iSearchCategory
                sSql = "SELECT Category_ID AS C1, Category_Name AS C2, SubTeam_Name AS C3 FROM ItemCategory INNER JOIN SubTeam ON SubTeam.SubTeam_No = ItemCategory.SubTeam_No WHERE Category_Name LIKE '%" & txtSearch.Text & "%' ORDER BY Category_Name, SubTeam_Name"
            Case iSearchShelfLife
                sSql = "SELECT ShelfLife_ID AS C1, ShelfLife_Name AS C2 FROM ItemShelfLife WHERE ShelfLife_Name LIKE '%" & txtSearch.Text & "%' ORDER BY ShelfLife_Name"
            Case iSearchUnit
                sSql = "SELECT Unit_ID AS C1, Unit_Name AS C2 FROM ItemUnit WHERE Unit_Name LIKE '%" & txtSearch.Text & "%' ORDER BY Unit_Name"
            Case iSearchOrigin
                sSql = "SELECT Origin_ID AS C1, Origin_Name AS C2 FROM ItemOrigin WHERE Origin_Name LIKE '%" & txtSearch.Text & "%' ORDER BY Origin_Name"
            Case isearchFromUnit
                sSql = "SELECT FromUnit_ID AS C1, Unit_Name AS C2, ToUnit_ID AS C3, (SELECT Unit_Name FROM ItemUnit WHERE Unit_ID = ItemConversion.ToUnit_ID) AS C4 FROM ItemUnit INNER JOIN ItemConversion ON (ItemUnit.Unit_ID = ItemConversion.FromUnit_ID) WHERE Unit_Name LIKE '%" & txtSearch.Text & "%' ORDER BY Unit_Name"
            Case iSearchToUnit
                sSql = "SELECT FromUnit_ID AS C1, Unit_Name AS C2, ToUnit_ID AS C3, (SELECT Unit_Name FROM ItemUnit WHERE Unit_ID = ItemConversion.FromUnit_ID) AS C4 FROM ItemUnit INNER JOIN ItemConversion ON (ItemUnit.Unit_ID = ItemConversion.ToUnit_ID) WHERE Unit_Name LIKE '%" & txtSearch.Text & "%' ORDER BY Unit_Name"
            Case iSearchOrganizationCompany
                sSql = "SELECT Organization_ID AS C1, OrganizationName AS C2 FROM FSOrganization WHERE OrganizationName LIKE '%" & txtSearch.Text & "%' ORDER BY OrganizationName"
            Case Else
                sSql = "" 'should never happen
        End Select

        Call LoadDataTable(sSql)

    End Sub

    Private Sub txtSearch_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtSearch.KeyDown
        If e.KeyCode = Keys.Enter Then
            RefreshGrid()
        End If
    End Sub

    Private Sub txtSearch_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtSearch.KeyPress

        Dim KeyAscii As Short = Asc(e.KeyChar)

        KeyAscii = ValidateKeyPressEvent(KeyAscii, "String", txtSearch, 0, 0, 0)

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If

    End Sub



    Private Sub cmdExit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub cmdSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSearch.Click
        RefreshGrid()
    End Sub

    Private Sub cmdSelect_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdSelect.Click
        '-- Call the data collecting procedure
        ReturnSelection()
    End Sub

    Private Sub ugrdSearchResults_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles ugrdSearchResults.DoubleClickRow
        '-- call the data collecting procedure
        ReturnSelection()
    End Sub

    Private Sub ugrdSearchResults_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles ugrdSearchResults.KeyDown
        If e.KeyCode = Keys.Enter Then
            ReturnSelection()
        End If
    End Sub

    Private Sub SetFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles optCompany.CheckedChanged, optVendorID.CheckedChanged, optPSVendorID.CheckedChanged
        txtSearch.Focus()
    End Sub
End Class