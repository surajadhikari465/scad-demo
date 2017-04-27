Option Strict Off
Option Explicit On

Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.Utility
Imports log4net
Imports Infragistics.Win


Friend Class frmItemVendors
    Inherits System.Windows.Forms.Form

    Private m_lItemKey As Integer
    Private mdt As DataTable

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Sub New(ByVal lItemKey As Integer)

        MyBase.New()
        logger.Debug("Entry with lItemKey=" + lItemKey.ToString())

        InitializeComponent()
        m_lItemKey = lItemKey
        logger.Debug("New Constructor Exit")
    End Sub
    Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click

        logger.Debug("cmdAdd_Click Entry")

        Dim rsItemVendor As DAO.Recordset = Nothing

        '-- Set glvendorid to none found
        glVendorID = 0

        '-- Set the search type     
        giSearchType = iSearchVendorCompany

        '-- Open the search form
        Dim fSearch As New frmSearch
        fSearch.Text = ResourcesItemHosting.GetString("SearchVendorByCompany")
        fSearch.ShowDialog()
        fSearch.Close()
        fSearch.Dispose()

        '-- if its not zero, then something was found
        If glVendorID <> 0 Then

            Try
                '-- Check to see if the name already exists
                rsItemVendor = SQLOpenRecordSet("EXEC CheckForDuplicateItemVendors " & glVendorID & ", " & glItemID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbForwardOnly + DAO.RecordsetOptionEnum.dbSQLPassThrough)
                If rsItemVendor.Fields("ItemVendorCount").Value > 0 Then
                    MsgBox(ResourcesItemHosting.GetString("VendorInList"), MsgBoxStyle.Exclamation, Me.Text)
                Else
                    If (DetermineVendorInternal() = True) Then
                        If Not (gbDistribution_Center) And Not (gbManufacturer) Then
                            MsgBox(ResourcesItemHosting.GetString("RetailNotVendor"), MsgBoxStyle.Exclamation, Me.Text)
                            rsItemVendor.Close()
                            logger.Info(ResourcesItemHosting.GetString("RetailNotVendor"))
                            logger.Debug("cmdAdd_Click Exit")
                            Exit Sub
                        End If
                    End If

                    '-- Add the new record
                    SQLExecute("EXEC InsertItemVendor " & glVendorID & ", " & glItemID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    RefreshGrid()
                End If
            Finally
                If rsItemVendor IsNot Nothing Then
                    rsItemVendor.Close()
                    rsItemVendor = Nothing
                End If
            End Try
        End If

        logger.Debug("cmdAdd_Click Exit")

    End Sub

    Private Sub cmdCost_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCost.Click

        logger.Debug("cmdCost_Click Entry")
        If ugrdVend.Selected.Rows.Count = 1 Then
            Dim fVendorCost As New frmVendorCost
            Dim vendorInfo As New VendorBO

            vendorInfo.VendorID = CInt(ugrdVend.Selected.Rows(0).Cells("Vendor_ID").Value)
            vendorInfo.VendorName = ugrdVend.Selected.Rows(0).Cells("CompanyName").Value
            vendorInfo.VendorCurrencyCode = ugrdVend.Selected.Rows(0).Cells("CurrencyCode").Value
            
            fVendorCost.poVendorInfo = vendorInfo
            fVendorCost.plItem_Key = m_lItemKey
            fVendorCost.ShowDialog()

            RefreshGrid()
            ugrdVend.Update()

            fVendorCost.Dispose()
        Else
            '-- No vendor was selected
            MsgBox(ResourcesItemHosting.GetString("VendorHighlight"), MsgBoxStyle.Exclamation, Me.Text)
            logger.Info(ResourcesItemHosting.GetString("VendorHighlight"))
        End If
        logger.Debug("cmdCost_Click Exit")
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click

        logger.Debug("cmdDelete_Click Entry")


        If ugrdVend.Selected.Rows.Count = 1 Then
            If ugrdVend.Rows.Count = 1 Then
                If MsgBox(ResourcesItemHosting.GetString("msg_wanring_RemoveVendorItem"), MsgBoxStyle.YesNo) = MsgBoxResult.Yes Then
                    DeleteVendor()

                End If
            Else
                DeleteVendor()
            End If
        Else
            '-- No vendor was selected
            MsgBox(ResourcesItemHosting.GetString("VendorHighlight"), MsgBoxStyle.Exclamation, Me.Text)

            logger.Info(ResourcesItemHosting.GetString("VendorHighlight"))

        End If

        logger.Debug("cmdDelete_Click Exit")
    End Sub

    Private Sub cmdEditItem_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdEditItem.Click

        logger.Debug("cmdEditItem_Click Entry")

        'If gridVendorList.SelBookmarks.Count = 1 Then
        If ugrdVend.Selected.Rows.Count = 1 Then
            'glVendorID = gridVendorList.Columns(0).value
            glVendorID = CInt(ugrdVend.Selected.Rows(0).Cells("Vendor_ID").Value)
            Dim fItemVendorID As New frmItemVendorID
            fItemVendorID.ShowDialog()
            'fItemVendorID.Close()
            fItemVendorID.Dispose()
            '-- Refresh the grid and seek the new one of its place
            RefreshGrid()
        Else
            '-- No vendor was selected
            MsgBox(ResourcesItemHosting.GetString("VendorHighlight"), MsgBoxStyle.Exclamation, Me.Text)
            logger.Info(ResourcesItemHosting.GetString("VendorHighlight"))
        End If

        logger.Debug("cmdEditItem_Click Exit")

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        logger.Debug("cmdExit_Click Entry")
        '-- Close the form
        Me.Close()
        logger.Debug("cmdExit_Click Exit")

    End Sub

    Private Sub frmItemVendors_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmItemVendors_Load Entry")

        '-- Center the form
        CenterForm(Me)

        SetActive(cmdAdd, (gbItemAdministrator And frmItem.pbUserSubTeam) Or gbSuperUser)
        SetActive(cmdEditItem, cmdAdd.Enabled)
        SetActive(cmdDelete, cmdAdd.Enabled)

        SetupDataTable()
        RefreshGrid()

        logger.Debug("frmItemVendors_Load Exit")

    End Sub

    Public Sub RefreshGrid()

        logger.Debug("RefreshGrid Entry")

        Dim rsVendorList As DAO.Recordset = Nothing
        Dim row As DataRow

        mdt.Clear()

        Try
            '-- Set up the databound stuff
            rsVendorList = SQLOpenRecordSet("EXEC GetItemVendors " & glItemID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

            While Not rsVendorList.EOF
                row = mdt.NewRow
                row("Vendor_ID") = rsVendorList.Fields("Vendor_ID").Value
                row("CompanyName") = rsVendorList.Fields("CompanyName").Value
                row("CurrencyCode") = If(IsDBNull(rsVendorList.Fields("CurrencyCode").Value), ConfigurationServices.AppSettings("CurrencyDefault"), rsVendorList.Fields("CurrencyCode").Value)
                row("Location") = rsVendorList.Fields("Location").Value
                row("RetailCasePack") = IIf(IsDBNull(rsVendorList.Fields("RetailCasePack").Value), "See Cost", rsVendorList.Fields("RetailCasePack").Value)
                row("Item_ID") = rsVendorList.Fields("Item_ID").Value
                row("VendorItemStatus") = rsVendorList.Fields("VendorItemStatus").Value
                row("VendorItemStatusFull") = rsVendorList.Fields("VendorItemStatusFull").Value
                row("VendorItemDescription") = IIf(IsDBNull(rsVendorList.Fields("VendorItemDescription").Value), rsVendorList.Fields("Item_Description").Value, rsVendorList.Fields("VendorItemDescription").Value)
                mdt.Rows.Add(row)
                rsVendorList.MoveNext()
            End While

        Finally
            If rsVendorList IsNot Nothing Then
                rsVendorList.Close()
                rsVendorList = Nothing
            End If
        End Try

        mdt.AcceptChanges()
        ugrdVend.DataSource = mdt

        If ugrdVend.Rows.Count > 0 Then
            ugrdVend.Rows(0).Selected = True
        End If

        logger.Debug("RefreshGrid Exit")

    End Sub

    Private Sub SetupDataTable()

        logger.Debug("SetupDataTable Entry")

        mdt = New DataTable("ItemPrices")

        mdt.Columns.Add(New DataColumn("Vendor_ID", GetType(Integer)))
        mdt.Columns.Add(New DataColumn("CompanyName", GetType(String)))
        mdt.Columns.Add(New DataColumn("CurrencyCode", GetType(String)))
        mdt.Columns.Add(New DataColumn("Location", GetType(String)))
        mdt.Columns.Add(New DataColumn("RetailCasePack", GetType(String)))
        mdt.Columns.Add(New DataColumn("Item_ID", GetType(String)))
        mdt.Columns.Add(New DataColumn("VendorItemStatus", GetType(String)))
        mdt.Columns.Add(New DataColumn("VendorItemStatusFull", GetType(String)))
        mdt.Columns.Add(New DataColumn("VendorItemDescription", GetType(String)))

        logger.Debug("SetupDataTable Exit")
    End Sub

    Private Sub ugrdVend_DoubleClickRow(ByVal sender As Object, ByVal e As Infragistics.Win.UltraWinGrid.DoubleClickRowEventArgs) Handles ugrdVend.DoubleClickRow
        logger.Debug("ugrdVend_DoubleClickRow Entry")
        Call cmdCost_Click(cmdCost, New System.EventArgs())
        logger.Debug("ugrdVend_DoubleClickRow Exit")
    End Sub

    Private Sub DeleteVendor()

        logger.Debug("DeleteVendor Entry")

        Dim fSelectDate As frmSelectDate
        Dim fPrimVendSelect As frmNewPrimVend

        glVendorID = CInt(ugrdVend.Selected.Rows(0).Cells("Vendor_ID").Value)

        '--Get the remove date
        fSelectDate = New frmSelectDate(ResourcesItemHosting.GetString("EnterDateVendorRemove"), SystemDateTime)
        fSelectDate.ShowDialog()

        '-- Delete the vendor from the database
        If fSelectDate.ReturnDate <> "" Then
            ' The primary vendor must be swapped for each store, if there is another vendor available for that store,
            ' before deleting it from the database.
            Dim storeList As IEnumerator = VendorDAO.GetStoresWithPrimaryVendorThatCanSwap(glItemID, glVendorID).GetEnumerator()
            Dim storeNo As Integer
            Dim deleteOK As Boolean = True
            While (storeList.MoveNext)
                storeNo = CInt(storeList.Current)
                ' Show the screen for the user to switch the primary vendor for this store.
                fPrimVendSelect = New frmNewPrimVend(CStr(Me.ugrdVend.Selected.Rows(0).Cells("CompanyName").Value), glVendorID, glItemID, storeNo)
                fPrimVendSelect.ShowDialog()
                If fPrimVendSelect.UnassignedItems <> 0 Then
                    deleteOK = False
                    Call MsgBox(ResourcesItemHosting.GetString("SelectNewPrimaryVendor"), MsgBoxStyle.Critical, Me.Text)
                    logger.Info(ResourcesItemHosting.GetString("SelectNewPrimaryVendor"))
                End If
                fPrimVendSelect.Close()
                fPrimVendSelect.Dispose()
            End While

            If deleteOK Then
                ' The primary vendor has been reassinged for all stores, so the vendor can now be deleted.
                SQLExecute("EXEC DeleteItemVendor " & glVendorID & ", " & glItemID & ", '" & VB6.Format(fSelectDate.ReturnDate, ResourcesIRMA.GetString("YearDateFormat")) & "'", DAO.RecordsetOptionEnum.dbSQLPassThrough)
            End If

            '-- Refresh the grid and seek the new one of its place
            RefreshGrid()
        End If
        fSelectDate.Close()

        logger.Debug("DeleteVendor Exit")
    End Sub

    Private Sub ugrdVend_MouseLeaveElement(ByVal sender As System.Object, ByVal e As Infragistics.Win.UIElementEventArgs) Handles ugrdVend.MouseLeaveElement
        ToolTip1.SetToolTip(ugrdVend, Nothing)
    End Sub

    Private Sub ugrdVend_MouseEnterElement(ByVal sender As System.Object, ByVal e As Infragistics.Win.UIElementEventArgs) Handles ugrdVend.MouseEnterElement
        ' tool tip over cell for VendorItemStatus only
        Dim cell As UltraWinGrid.UltraGridCell = e.Element.GetContext(GetType(Infragistics.Win.UltraWinGrid.UltraGridCell))
        If Not cell Is Nothing Then
            If cell.Column.Key = "VendorItemStatus" Then
                If Not IsDBNull(cell.Value) Then
                    ToolTip1.Active = True
                    ToolTip1.SetToolTip(sender, cell.Row.GetCellValue("VendorItemStatusFull") & "")
                    Exit Sub
                End If

            End If
        End If
    End Sub
End Class

