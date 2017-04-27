Imports log4net
Imports WholeFoods.Utility
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Public Class Form_ManageItemUnit

    ' ############################################################################################################################################################
    ' This screen was added to allow the update of existing ItemUnits and addition of new ones. 
    ' Removal and more advanced changes of ItemUnits must be done through Change Control due to the their sensitive nature.
    ' ############################################################################################################################################################

    ' TFS 13058, v4.0, 8/3/2010, Tom Lux: When I removed the dup item-unit classes, this form had issues, "key not found" error in UltraGrid1_InitializeLayout(),
    ' because datasource had not yet been applied to the grid, so it had no columns, so the column reference would fail.  Moved RefreshData() from form-load method to
    ' UltraGrid1_InitializeLayout().  Added regions and logger; some other cleanup.


#Region "Private Members"

    Private ItemUnitsData As List(Of ItemUnitBO)
    Private DAO As ItemUnitDAO = New ItemUnitDAO
    Private CurrentUnitID As Integer

    ''' <summary>
    ''' Log4Net logger for this class.
    ''' </summary>
    ''' <remarks></remarks>
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

#End Region

#Region "Form Events"

    Private Sub Form_ManageItemUnit_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        logger.Info("Manage Item Units screen loading.")
        EditorHidden()
        Button_Save.Enabled = False
        ' We do not need to call RefreshData() here because it is called during grid init, UltraGrid1_InitializeLayout().
    End Sub

    Private Sub Button_AddNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_AddNew.Click
        EditorVisible()
    End Sub

    Private Sub Button_NewItemUnit_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_NewItemUnit_Close.Click
        TextBox_NewItemUnit_Name.Enabled = True
        EditorHidden()
    End Sub

    Private Sub ClearPanelControls()
        For Each c As Control In UltraPanel1.ClientArea.Controls

            If c.GetType() Is GetType(TextBox) Then
                DirectCast(c, TextBox).Text = String.Empty
            End If

            If c.GetType() Is GetType(CheckBox) Then
                DirectCast(c, CheckBox).Checked = False
            End If
        Next
    End Sub

    Private Sub Button_NewItemUnit_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_NewItemUnit_Save.Click
        Dim itemUnit As ItemUnitBO = New ItemUnitBO()

        If TextBox_NewItemUnit_Name.Text.Trim().Length > 0 Then
            If MessageBox.Show("Are you sure you want to make this change?", "Caution", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) = Windows.Forms.DialogResult.OK Then

                With itemUnit
                    .UnitId = CurrentUnitID
                    .UnitName = TextBox_NewItemUnit_Name.Text.Trim()
                    .UnitAbbreviation = TextBox_NewItemUnit_Abbreviation.Text.Trim()
                    .PlumUnitAbbr = TextBox_NewItemUnit_PlumAbbrev.Text.Trim()
                    .EDISysCode = TextBox_NewItemUnit_EDISysCode.Text.Trim()
                    .UnitSysCode = TextBox_NewItemUnit_UnitSysCode.Text.Trim()

                    .IsPackageUnit = CheckBox_NewItemUnit_PackageUnit.Checked
                    .WeightUnit = CheckBox_NewItemUnit_WeightUnit.Checked

                    .Save()
                End With
            End If
            EditorHidden()
            TextBox_NewItemUnit_Name.Enabled = True
            RefreshData()
        Else
            MessageBox.Show("You must include a Unit Name.", "Caution", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If

    End Sub

    Private Sub UltraGrid1_DoubleClickRow(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles UltraGrid1.DoubleClickRow
        ClearPanelControls()

        TextBox_NewItemUnit_Name.Text = e.Row.Cells("UnitName").Value.ToString().Trim
        TextBox_NewItemUnit_Abbreviation.Text = e.Row.Cells("UnitAbbreviation").Text.Trim
        TextBox_NewItemUnit_PlumAbbrev.Text = e.Row.Cells("PlumUnitAbbr").Text.Trim
        TextBox_NewItemUnit_EDISysCode.Text = e.Row.Cells("EDISYSCode").Text.Trim

        CheckBox_NewItemUnit_PackageUnit.Checked = e.Row.Cells("IsPackageUnit").Value
        CheckBox_NewItemUnit_WeightUnit.Checked = e.Row.Cells("WeightUnit").Value

        TextBox_NewItemUnit_UnitSysCode.Text = e.Row.Cells("UnitSysCode").Text
        CurrentUnitID = e.Row.Cells("UnitId").Value

        ' editing a current Item. Dont allow people to change the Name.
        TextBox_NewItemUnit_Name.Enabled = True
        EditorVisible()

    End Sub

    Private Sub UltraGrid1_InitializeLayout(ByVal sender As System.Object, ByVal e As Infragistics.Win.UltraWinGrid.InitializeLayoutEventArgs) Handles UltraGrid1.InitializeLayout
        RefreshData()

        UltraGrid1.DisplayLayout.Bands(0).Columns("UserId").Hidden = True
        UltraGrid1.DisplayLayout.Bands(0).Columns("DataChanged").Hidden = True
        UltraGrid1.DisplayLayout.Bands(0).Columns("Valid").Hidden = True
        UltraGrid1.DisplayLayout.Bands(0).Columns("ErrorMessage").Hidden = True

        UltraGrid1.DisplayLayout.Bands(0).Override.AllowUpdate = Infragistics.Win.DefaultableBoolean.False

    End Sub

    Private Sub Button_Close_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Close.Click
        Me.Close()
        Me.Dispose()
        logger.Info("Manage Item Units screen closing.")
    End Sub

#End Region

#Region "Private Methods"

    Private Sub RefreshData()
        ItemUnitsData = DAO.GetItemUnits()
        ItemUnitBOBindingSource.DataSource = ItemUnitsData
        UltraGrid1.DataSource = ItemUnitBOBindingSource
    End Sub

    Private Sub EditorHidden()
        ClearPanelControls()

        UltraGrid1.Enabled = True
        UltraPanel1.Visible = False
    End Sub

    Private Sub EditorVisible()
        Dim panelwidth As Integer
        Dim panelheight As Integer
        Dim centerx As Integer
        Dim centery As Integer

        Dim panelx As Integer
        Dim panely As Integer

        Dim windowwidth As Integer
        Dim windowheight As Integer

        windowwidth = Me.Width
        windowheight = Me.Height

        panelwidth = UltraPanel1.Width
        panelheight = UltraPanel1.Height

        centerx = windowwidth / 2
        centery = windowheight / 2

        panelx = centerx - (panelwidth / 2)
        panely = centery - (panelheight / 2)

        UltraPanel1.Top = panely
        UltraPanel1.Left = panelx

        UltraPanel1.BringToFront()

        UltraGrid1.Enabled = False

        UltraPanel1.Visible = True
    End Sub

#End Region

End Class