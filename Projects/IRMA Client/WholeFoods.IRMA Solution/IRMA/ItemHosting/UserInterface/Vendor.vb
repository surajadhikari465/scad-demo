Option Strict Off
Option Explicit On

Imports System.Collections.Specialized
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Threading
Imports WholeFoods.IRMA.Common.BusinessLogic
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.Utility
Imports log4net


Friend Class frmVendor
	Inherits System.Windows.Forms.Form
    Private IsInitializing As Boolean

	Dim rsVendor As dao.Recordset
    Dim pbDataChanged As Boolean
    Dim vendorKeyChanged As Boolean = False
    Dim psVendorExportChanged As Boolean = False
    Dim psVendorChanged As Boolean = False
    Dim plVendorID As Integer
    Dim hideVendorItemButton As Boolean = False
    Dim multipleCurrencies As Boolean = False
    Dim _dataSet As DataSet
    Dim _PS_Export_Vendor_ID As String = String.Empty
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Private DVOVendorSetupOK As Boolean = False



#Region "Control Events"
    Private Sub chkField_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkField.CheckStateChanged


        If Me.IsInitializing Then Exit Sub

        logger.Debug("chkField_CheckStateChanged Entry")

        Dim Index As Short = chkField.GetIndex(eventSender)

        pbDataChanged = True

        logger.Debug("chkField_CheckStateChanged Exit")

    End Sub

    Private Sub chkField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles chkField.KeyPress

        logger.Debug("chkField_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        Dim Index As Short = chkField.GetIndex(eventSender)

        If KeyAscii = 13 Then
            KeyAscii = 0
            System.Windows.Forms.SendKeys.Send("{TAB}")
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If

        logger.Debug("chkField_KeyPress Exit")
    End Sub

    Private Sub chkNonProductVendor_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkNonProductVendor.CheckStateChanged

        If Me.IsInitializing Then Exit Sub
        logger.Debug("chkNonProductVendor_CheckStateChanged Entry")
        pbDataChanged = True

        If chkNonProductVendor.CheckState Then
            chkField(iVendorCustomer).Enabled = False
            cmbField(iVendorStore_No).Enabled = False
            chkField(iVendorWFM).Enabled = False
            txtField(iVendorDefault_Account_No).Enabled = (Not lblReadOnly.Visible) And (gbSuperUser Or gbVendorAdministrator)
            chkField(iVendorCustomer).CheckState = System.Windows.Forms.CheckState.Unchecked
            cmbField(iVendorStore_No).SelectedIndex = -1
            chkField(iVendorWFM).CheckState = System.Windows.Forms.CheckState.Unchecked
        Else
            chkField(iVendorCustomer).Enabled = (Not lblReadOnly.Visible) And (gbSuperUser Or gbVendorAdministrator)
            chkField(iVendorInternalCustomer).Enabled = (Not lblReadOnly.Visible) And (gbSuperUser Or gbVendorAdministrator)
            cmbField(iVendorStore_No).Enabled = (Not lblReadOnly.Visible) And (gbSuperUser Or gbVendorAdministrator)
            chkField(iVendorWFM).Enabled = (Not lblReadOnly.Visible) And (gbSuperUser Or gbVendorAdministrator)
            txtField(iVendorDefault_Account_No).Enabled = False
        End If

        logger.Debug("chkNonProductVendor_CheckStateChanged Exit")

    End Sub

    Private Sub cmbField_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbField.SelectedIndexChanged
        If Me.IsInitializing Then Exit Sub
        Dim Index As Short = cmbField.GetIndex(eventSender)

        pbDataChanged = True

    End Sub

    Private Sub cmbField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbField.KeyPress
        logger.Debug("cmbField_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        Dim Index As Short = cmbField.GetIndex(eventSender)

        If KeyAscii = 13 Then
            KeyAscii = 0
            System.Windows.Forms.SendKeys.Send("{TAB}")
        ElseIf KeyAscii = 8 And Index = iVendorStore_No Then
            cmbField(Index).SelectedIndex = -1
        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If

        logger.Debug("cmbField_KeyPress Exit")
    End Sub

    Private Sub cmbCurrency_SelectedIndexChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmbCurrency.SelectedIndexChanged
        If Me.IsInitializing Then Exit Sub

        pbDataChanged = True

    End Sub

    Private Sub cmbCurrency_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles cmbCurrency.KeyPress
        logger.Debug("cmbField_KeyPress Entry")
        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)

        If KeyAscii = 8 Then
            pbDataChanged = True
        End If

        logger.Debug("cmbField_KeyPress Exit")
    End Sub


    Private Sub txtField_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.TextChanged

        logger.Debug("txtField_TextChanged Entry")

        If Me.IsInitializing Then Exit Sub
        Dim Index As Short = txtField.GetIndex(eventSender)

        pbDataChanged = True
        If Index = iVendorVendor_Key Then
            vendorKeyChanged = True
        End If
        If Index = iVendorPS_Vendor_ID Then
            psVendorChanged = True
        End If

        logger.Debug("txtField_TextChanged Exit")

    End Sub

    Private Sub mskdField_TextChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs)

        logger.Debug("mskdField_TextChanged Entry")

        If Me.IsInitializing Then Exit Sub
        Dim Index As Short = txtField.GetIndex(eventSender)

        pbDataChanged = True
        If Index = iVendorVendor_Key Then
            vendorKeyChanged = True
        End If
        If Index = iVendorPS_Vendor_ID Then
            psVendorChanged = True
        End If

        logger.Debug("mskdField_TextChanged Exit")

    End Sub

    Private Sub TextBox_PSVendorExport_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox_PSVendorExport.TextChanged
        logger.Debug("TextBox_PSVendorExport_TextChanged Entry")
        If Me.IsInitializing Then Exit Sub
        pbDataChanged = True
        psVendorExportChanged = True
        logger.Debug("TextBox_PSVendorExport_TextChanged Exit")
    End Sub

    Private Sub txtField_Enter(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles txtField.Enter

        logger.Debug("txtField_Enter Entry")
        Dim Index As Short = txtField.GetIndex(eventSender)

        HighlightText(txtField(Index))

        logger.Debug("txtField_Enter Exit")

    End Sub

    Private Sub txtField_KeyPress(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.KeyPressEventArgs) Handles txtField.KeyPress

        logger.Debug("txtField_KeyPress Entry")

        Dim KeyAscii As Short = Asc(eventArgs.KeyChar)
        Dim Index As Short = txtField.GetIndex(eventSender)

        If KeyAscii = 13 And iVendorComments <> Index Then

            KeyAscii = 0
            System.Windows.Forms.SendKeys.Send("{TAB}")

        ElseIf Not txtField(Index).ReadOnly Then

            KeyAscii = ValidateKeyPressEvent(KeyAscii, txtField(Index).Tag, txtField(Index), 0, 0, 0)

        End If

        eventArgs.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If

        logger.Debug("txtField_KeyPress Exit")
    End Sub

    Private Sub txtCounty_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCounty.Enter

        logger.Debug("txtCounty_Enter Entry")

        HighlightText(txtCounty)
        logger.Debug("txtCounty_Enter Exit")

    End Sub

    Private Sub txtCounty_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtCounty.KeyPress

        logger.Debug("txtCounty_KeyPress Entry")
        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 13 Then

            KeyAscii = 0
            System.Windows.Forms.SendKeys.Send("{TAB}")

        Else

            KeyAscii = ValidateKeyPressEvent(KeyAscii, txtCounty.Tag, txtCounty, 0, 0, 0)

        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If

        logger.Debug("txtCounty_KeyPress Exit")
    End Sub

    Private Sub txtCounty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtCounty.TextChanged
        If Me.IsInitializing Then Exit Sub

        logger.Debug("txtCounty_TextChanged Entry")

        pbDataChanged = True

        logger.Debug("txtCounty_TextChanged Exit")

    End Sub

    Private Sub txtPayToCounty_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPayToCounty.Enter
        HighlightText(txtPayToCounty)

    End Sub

    Private Sub txtPayToCounty_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtPayToCounty.KeyPress
        logger.Debug("txtPayToCounty_KeyPress Entry")

        Dim KeyAscii As Short = Asc(e.KeyChar)

        If KeyAscii = 13 Then

            KeyAscii = 0
            System.Windows.Forms.SendKeys.Send("{TAB}")

        Else

            KeyAscii = ValidateKeyPressEvent(KeyAscii, txtPayToCounty.Tag, txtPayToCounty, 0, 0, 0)

        End If

        e.KeyChar = Chr(KeyAscii)
        If KeyAscii = 0 Then
            e.Handled = True
        End If

        logger.Debug("txtPayToCounty_KeyPress Exit")

    End Sub

    Private Sub txtPayToCounty_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtPayToCounty.TextChanged
        If Me.IsInitializing Then Exit Sub

        logger.Debug("txtPayToCounty_TextChanged Entry")
        pbDataChanged = True
        logger.Debug("txtPayToCounty_TextChanged Exit")

    End Sub

    Private Sub _optPOTrans_0_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _optPOTrans_0.CheckedChanged
        If Me.IsInitializing Then Exit Sub

        logger.Debug("rbFax_CheckedChanged Entry")
        pbDataChanged = True
        logger.Debug("rbFax_CheckedChanged Exit")
    End Sub

    Private Sub _optPOTrans_1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _optPOTrans_1.CheckedChanged
        If Me.IsInitializing Then Exit Sub

        logger.Debug("rbEmail_CheckedChanged Entry")
        pbDataChanged = True
        logger.Debug("rbEmail_CheckedChanged Exit")
    End Sub

    Private Sub _optPOTrans_2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _optPOTrans_2.CheckedChanged
        If Me.IsInitializing Then Exit Sub

        logger.Debug("rbManual_CheckedChanged Entry")
        pbDataChanged = True
        logger.Debug("rbManual_CheckedChanged Exit")
    End Sub

    Private Sub _optPOTrans_3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _optPOTrans_3.CheckedChanged
        If Me.IsInitializing Then Exit Sub

        logger.Debug("rbElectronic_CheckedChanged Entry")
        SetActive(DSDVendor_StoreSetup, (gbVendorAdministrator Or gbSuperUser) And CheckBox_EInvoicing.Checked And CheckBox_EinvoiceReqd.Checked And optPOTrans(iVendorPOElectronicTransmission).Checked And DVOVendorSetupOK)
        pbDataChanged = True
        logger.Debug("rbElectronic_CheckedChanged Exit")
    End Sub

    Private Sub CheckBox_EInvoicing_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_EInvoicing.CheckedChanged
        If Me.IsInitializing Then Exit Sub

        logger.Debug("CheckBox_EInvoicing_CheckedChanged Entry")
        pbDataChanged = True
        If CheckBox_EInvoicing.Checked = True Then
            CheckBox_EinvoiceReqd.Enabled = True
        Else
            CheckBox_EinvoiceReqd.Enabled = False
        End If
        SetActive(DSDVendor_StoreSetup, (gbVendorAdministrator Or gbSuperUser) And CheckBox_EInvoicing.Checked And CheckBox_EinvoiceReqd.Checked And optPOTrans(iVendorPOElectronicTransmission).Checked And DVOVendorSetupOK)
        logger.Debug("CheckBox_EInvoicing_CheckedChanged Exit")
    End Sub

    Private Sub CheckBox_EinvoiceReqd_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox_EinvoiceReqd.CheckedChanged
        If Me.IsInitializing Then Exit Sub
        SetActive(DSDVendor_StoreSetup, (gbVendorAdministrator Or gbSuperUser) And CheckBox_EInvoicing.Checked And CheckBox_EinvoiceReqd.Checked And optPOTrans(iVendorPOElectronicTransmission).Checked And DVOVendorSetupOK)
        logger.Debug("CheckBox_EinvoiceReqd_CheckedChanged Entry")
        pbDataChanged = True
        logger.Debug("CheckBox_EinvoiceReqd_CheckedChanged Exit")
    End Sub

    Private Sub _chkField_0_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _chkField_0.CheckedChanged
        SetConfiguredMsg()
    End Sub

    Private Sub _chkField_2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _chkField_2.CheckedChanged
        SetConfiguredMsg()
    End Sub

    Private Sub _chkField_1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _chkField_1.CheckedChanged
        SetConfiguredMsg()
    End Sub

    Private Sub txtAccountingContactEmail_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtAccountingContactEmail.TextChanged
        pbDataChanged = True
    End Sub
#End Region

#Region "Button Events"
    Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click
        logger.Debug("cmdAdd_Click Entry")

        '-- Force Validation
        If plVendorID > -1 Then
            If Not lblReadOnly.Visible Then
                If Not SaveData() Then
                    logger.Debug("cmdAdd_Click Exit- SaveData returns False)")
                    Exit Sub
                End If
            End If
        End If

        '-- Everything is alright so go on
        glVendorID = 0

        '-- Call the adding form
        Dim fVendorAdd As New frmVendorAdd
        fVendorAdd.ShowDialog()
        fVendorAdd.Close()
        fVendorAdd.Dispose()

        '-- a new vendor was added
        If glVendorID = -2 Then
            '-- Put the new data in
            RefreshDataSource(-2)
            '-- go to enter the other new data
            txtField(iVendorAddress_Line_1).Focus()
        ElseIf plVendorID > -1 Then
            RefreshDataSource(plVendorID)
        End If
        logger.Debug("cmdAdd_Click Exit")
    End Sub

    Private Sub cmdCompanySearch_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCompanySearch.Click

        logger.Debug("cmdCompanySearch_Click Entry")

        '-- Force Validation
        If Not lblReadOnly.Visible Then
            If Not SaveData Then
                Exit Sub
            End If
        End If

        '-- Set glvendorid to none found
        glVendorID = 0

        '-- Set the search type
        giSearchType = iSearchAllVendors

        '-- Open the search form
        Dim fSearch As New frmSearch
        fSearch.Text = ResourcesItemHosting.GetString("SearchVendorByCompany")
        fSearch.ShowDialog()
        fSearch.Close()
        fSearch.Dispose()

        '-- if its not zero, then something was found
        If glVendorID <> 0 Then
            RefreshDataSource(glVendorID)
        Else
            RefreshDataSource(plVendorID)
        End If
        logger.Debug("cmdCompanySearch_Click Exit")

    End Sub

    Private Sub cmdContacts_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdContacts.Click

        logger.Debug("cmdContacts_Click Entry")

        '-- Force Validation
        If Not lblReadOnly.Visible Then
            If Not SaveData Then
                Exit Sub
            End If
        End If

        RefreshDataSource(plVendorID)

        '-- If everything is find then go on
        glVendorID = plVendorID

        '-- Call the contacts form
        Dim fContact As New frmContact
        fContact.Text = String.Format(ResourcesItemHosting.GetString("ContactsForTitle"), txtField(iVendorCompanyName).Text)
        fContact.ShowDialog()
        fContact.Close()
        fContact.Dispose()

        logger.Debug("cmdContacts_Click Exit")

    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdDelete.Click

        logger.Debug("cmdDelete_Click Entry")
        '-- Make sure there is a record to delete
        If plVendorID = -1 Then
            MsgBox(ResourcesItemHosting.GetString("NoCurrentRecordDelete"), MsgBoxStyle.Exclamation, Me.Text)
            logger.Info(ResourcesItemHosting.GetString("NoCurrentRecordDelete"))
            logger.Debug("cmdDelete_Click Exit")
            Exit Sub
        End If

        Try
            gRSRecordset = SQLOpenRecordSet("EXEC GetVendorLinks " & plVendorID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            If Not gRSRecordset.EOF Then
                MsgBox(ResourcesItemHosting.GetString("VendorLinksExist"), MsgBoxStyle.Exclamation, Me.Text)
                logger.Info(ResourcesItemHosting.GetString("VendorLinksExist"))
                logger.Debug("cmdDelete_Click Exit")
                Exit Sub
            End If
            gRSRecordset.Close()
        Finally
            If gRSRecordset IsNot Nothing Then
                gRSRecordset = Nothing
            End If
        End Try

        '-- Disallow this vendor for all items
        'Dim fSelectDate As frmSelectDate
        'Set fSelectDate = frmSelectDate.CreateObj("Enter the date you want this Vendor to be removed from the allowable vendor list for all items.", SystemDateTime)
        'fSelectDate.Show vbModal
        'If fSelectDate.ReturnDate <> "" Then
        Dim rsChkPrimVend As DAO.Recordset
        rsChkPrimVend = SQLOpenRecordSet("EXEC CheckIfPrimVendCanSwap " & plVendorID & ", null, null", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbForwardOnly + DAO.RecordsetOptionEnum.dbSQLPassThrough)
        Dim fPrimVendSelect As frmNewPrimVend
        If rsChkPrimVend.Fields("IsPrimVend").Value = 1 Then
            'This vendor is primary vendor for at least one item
            rsChkPrimVend.Close()
            rsChkPrimVend = Nothing

            fPrimVendSelect = New frmNewPrimVend(Me.txtField(1).Text, plVendorID)
            fPrimVendSelect.ShowDialog()
            If fPrimVendSelect.UnassignedItems = 0 Then
                'Confirm the delete
                If MsgBox(String.Format(ResourcesItemHosting.GetString("DeleteVendor"), txtField(iVendorCompanyName).Text), MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then

                    logger.Info("Delete Vendor - Yes")
                    '-- Delete the vendor from the database
                    SQLExecute("EXEC DeleteVendor " & plVendorID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    '-- Refresh the grid and seek the new one of its place
                    RefreshDataSource(-1)
                    '-- Make sure there are still vendors!
                    CheckNoVendors()
                End If
            Else
                Call MsgBox(ResourcesItemHosting.GetString("AssignNewPrimaryVendor"), MsgBoxStyle.Critical, Me.Text)
                logger.Info(ResourcesItemHosting.GetString("AssignNewPrimaryVendor"))
            End If
            fPrimVendSelect.Close()
            fPrimVendSelect.Dispose()
        Else
            rsChkPrimVend.Close()
            rsChkPrimVend = Nothing

            'items can't be reassigned to other avail vendors as primary, but need to confirm that this vendor 
            'even is set as the primary for any items; if yes, then can't delete.  if no, then can delete
            Try
                rsChkPrimVend = SQLOpenRecordSet("EXEC CheckIfVendorIsPrimaryForAnyItems " & plVendorID & ", null, null", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbForwardOnly + DAO.RecordsetOptionEnum.dbSQLPassThrough)
                If rsChkPrimVend.Fields("IsPrimVend").Value = 1 Then
                    'can't delete this vendor because it's the primary for items that can't be reassigned
                    Call MsgBox(ResourcesItemHosting.GetString("AssignNewPrimaryVendor"), MsgBoxStyle.Critical, Me.Text)
                    logger.Info(ResourcesItemHosting.GetString("AssignNewPrimaryVendor"))
                Else
                    'Confirm the delete
                    If MsgBox(String.Format(ResourcesItemHosting.GetString("DeleteVendor"), txtField(iVendorCompanyName).Text), MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                        '-- Delete the vendor from the database
                        logger.Debug("Delete Vendor confirmed - True")
                        SQLExecute("EXEC DeleteVendor " & plVendorID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                        '-- Refresh the grid and seek the new one of its place
                        RefreshDataSource(-1)
                        '-- Make sure there are still vendors!
                        CheckNoVendors()
                    End If
                End If

            Finally
                If rsChkPrimVend IsNot Nothing Then
                    rsChkPrimVend.Close()
                    rsChkPrimVend = Nothing
                End If
            End Try
        End If

        logger.Debug("cmdDelete_Click Exit")
        'End If
        'Unload fSelectDate
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        Me.Close()

    End Sub

    Private Sub cmdItems_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdItems.Click

        logger.Debug("cmdItems_Click Entry")

        '-- Force Validation
        If Not lblReadOnly.Visible Then
            If Not SaveData Then
                Exit Sub
            End If
        End If

        RefreshDataSource(plVendorID)

        '-- Set glvendorid to none found
        Dim fVendorItems As New frmVendorItems(plVendorID, txtField(iVendorCompanyName).Text)
        '-- Open the search form
        fVendorItems.Text = "Items supplied by " & txtField(iVendorCompanyName).Text
        fVendorItems.ShowDialog()
        'TODO: Original code was frmSearch = nothing.  I have assumed this is incorrect and should have been frmVendorItems = nothing, thus the upgrade code is:
        fVendorItems.Close()
        fVendorItems.Dispose()

        logger.Debug("cmdItems_Click Exit")

    End Sub

    Private Sub cmdReports_Click()
        logger.Debug("cmdReports_Click Entry")

        '-- Force Validation
        If Not lblReadOnly.Visible Then
            If Not SaveData() Then
                logger.Info("SaveData-False")
                logger.Debug("cmdReports_Click Exit")
                Exit Sub
            End If
        End If

        RefreshDataSource(plVendorID)

        '-- Set glvendorid to none found
        glVendorID = plVendorID

        '-- Open the search form
        Dim fVendorReports As New frmVendorReports
        fVendorReports.ShowDialog()
        'TODO: Original code was frmSearch = nothing.  I have assumed this is incorrect and should have been FrmVendorReports = nothing, thus the upgrade code is:
        fVendorReports.Close()
        fVendorReports.Dispose()
        logger.Debug("cmdReports_Click Exit")
    End Sub

    Private Sub cmdSameData_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSameData.Click

        logger.Debug("cmdSameData_Click Entry")
        '-- Cycle the pay to data
        txtField(iVendorPayTo_Fax).Text = txtField(iVendorFax).Text
        txtField(iVendorPayTo_Phone).Text = txtField(iVendorPhone).Text
        txtField(iVendorPayTo_Phone_Ext).Text = txtField(iVendorPhone_Ext).Text
        txtField(iVendorPayTo_Country).Text = txtField(iVendorCountry).Text
        txtField(iVendorPayTo_Zip_Code).Text = txtField(iVendorZip_Code).Text
        txtField(iVendorPayTo_State).Text = txtField(iVendorState).Text
        txtField(iVendorPayTo_City).Text = txtField(iVendorCity).Text
        txtField(iVendorPayTo_Address_Line_2).Text = txtField(iVendorAddress_Line_2).Text
        txtField(iVendorPayTo_Address_Line_1).Text = txtField(iVendorAddress_Line_1).Text
        txtField(iVendorPayTo_CompanyName).Text = txtField(iVendorCompanyName).Text
        txtPayToCounty.Text = txtCounty.Text
        logger.Debug("cmdSameData_Click Exit")

    End Sub


    Private Sub cmdUnlock_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdUnlock.Click

        logger.Debug("cmdUnlock_Click Entry")

        If MsgBox(ResourcesIRMA.GetString("UnlockRecord"), MsgBoxStyle.YesNo + MsgBoxStyle.Question, Me.Text) = MsgBoxResult.Yes Then
            SQLExecute("EXEC UnlockVendor " & plVendorID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            RefreshDataSource(plVendorID)
        End If

        logger.Debug("cmdUnlock_Click Exit")

    End Sub

#End Region

    Function SaveData() As Boolean

        Dim _reqDigits As Integer
        Dim _value As String

        logger.Debug("SaveData Entry")

        SaveData = True

        If glVendorID = -1 Or lblReadOnly.Visible Then Exit Function

        ' The Key is now required for some regions.  Check this even if the user did not edit anything.
        If InstanceDataDAO.IsFlagActive("VendorKeyRequired") Then
            If Trim(txtField(iVendorVendor_Key).Text) = vbNullString Then
                MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblKey.Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
                logger.Info(String.Format(ResourcesIRMA.GetString("Required"), lblKey.Text.Replace(":", "")))
                tabVendor.SelectTab(0)
                txtField(iVendorVendor_Key).Focus()
                SaveData = False
                logger.Debug("SaveData Exit with False")
                Exit Function
            End If
        End If

        If pbDataChanged Then

            'Payment Currency Required.  
            If InstanceDataDAO.IsFlagActive("MultipleCurrencies") Then
                If cmbCurrency.SelectedIndex = -1 Then
                    MsgBox(String.Format(ResourcesIRMA.GetString("Required"), lblCurrency.Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
                    logger.Info(String.Format(ResourcesIRMA.GetString("Required"), lblCurrency.Text.Replace(":", "")))
                    tabVendor.SelectTab(2)
                    cmbCurrency.Focus()
                    SaveData = False
                    logger.Debug("SaveData Exit with False")
                    Exit Function
                End If
            End If

            '-- Make sure they haven't duped a key
            If vendorKeyChanged And VendorDAO.VendorKeyExists(Trim(txtField(iVendorVendor_Key).Text)) Then
                SQLOpenRS(rsVendor, "EXEC GetVendorInfo " & glVendorID, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)

                If Trim(txtField(iVendorVendor_Key).Text) <> rsVendor.Fields("Vendor_Key").Value Then
                    MsgBox(String.Format(ResourcesIRMA.GetString("Duplicate"), lblKey.Text.Replace(":", "")), MsgBoxStyle.Exclamation, Me.Text)
                    logger.Info(String.Format(ResourcesIRMA.GetString("Duplicate"), lblKey.Text.Replace(":", "")))
                    SaveData = False
                    txtField(iVendorVendor_Key).Focus()
                    logger.Debug("SaveData Exit with False")
                    Exit Function
                End If

                rsVendor.Close()
            End If

            '-- Make sure they entered a company name
            If Trim(txtField(iVendorCompanyName).Text) = vbNullString Then
                If MsgBox(String.Format(ResourcesItemHosting.GetString("ChangesNotMade"), Chr(13), lblCompany.Text.Replace(":", "")), MsgBoxStyle.Exclamation + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                    logger.Info(String.Format(ResourcesItemHosting.GetString("ChangesNotMade"), Chr(13), lblCompany.Text.Replace(":", "")))
                    txtField(iVendorCompanyName).Focus()
                    logger.Debug("SaveData Exit with False")
                    SaveData = False
                Else
                    SQLExecute("EXEC UnlockVendor " & plVendorID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                End If
                Exit Function
            End If

            '-- Make sure they haven't duped a name
            '-- This check uses the "child" PS vendor name, not the parent
            SQLOpenRS(rsVendor, "EXEC CheckForDuplicateVendors " & plVendorID & ", '" & txtField(iVendorCompanyName).Text & "'," & TextValue("'" & txtField(iVendorPS_Vendor_ID).Text & "'") & "," & TextValue("'" & txtField(iVendorPS_Address_Sequence).Text & "'"), DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            If rsVendor.Fields("VendorCount").Value > 0 Then
                If MsgBox(String.Format(ResourcesItemHosting.GetString("DuplicateCompanyModify"), vbCrLf), MsgBoxStyle.Critical + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.Yes Then
                    logger.Info("DuplicateCompanyModify - Yes")
                    SaveData = False
                Else
                    SQLExecute("EXEC UnlockVendor " & plVendorID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                End If
                rsVendor.Close()
                Exit Function
            End If
            rsVendor.Close()

            '-- Make sure they don't make unwanted changes.
            If Not bProfessional Then
                If MsgBox(ResourcesIRMA.GetString("SaveChanges"), MsgBoxStyle.Question + MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                    logger.Info("SaveChanges - No")
                    SQLExecute("EXEC UnlockVendor " & plVendorID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                    Exit Function
                End If
            End If

        End If

        If pbDataChanged Then

            Dim res As DialogResult = MessageBox.Show(ResourcesItemHosting.GetString("msg_question_VendorIsWFMBusinessUnit"), Application.ProductName, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            Select Case res

                Case Windows.Forms.DialogResult.Yes

                    ' skip all the PS validation and save the form data

                    GoTo SaveAndClose

                Case Windows.Forms.DialogResult.No

                    ' do the PS Vendor ID and Export ID validation

                    If Not IsNumeric(txtField(iVendorPS_Vendor_ID).Text.Trim()) Then
                        MsgBox(String.Format(ResourcesItemHosting.GetString("PSVendorNumericValue"), _lblLabel_24.Text.Replace(":", "")), MsgBoxStyle.Exclamation, Application.ProductName)
                        logger.Info(String.Format(ResourcesItemHosting.GetString("PSVendorNumericValue"), _lblLabel_24.Text.Replace(":", "")))
                        tabVendor.SelectTab(2)
                        txtField(iVendorPS_Vendor_ID).Focus()
                        SaveData = False
                        logger.Debug("SaveData Exit with False")
                        Exit Function
                    End If

                    If Not IsNumeric(TextBox_PSVendorExport.Text.Trim()) Then
                        MsgBox(String.Format(ResourcesItemHosting.GetString("PSVendorNumericValue"), Label_PSVendorExport.Text.Replace(":", "")), MsgBoxStyle.Exclamation, Application.ProductName)
                        logger.Info(String.Format(ResourcesItemHosting.GetString("PSVendorNumericValue"), Label_PSVendorExport.Text.Replace(":", "")))
                        tabVendor.SelectTab(2)
                        TextBox_PSVendorExport.Focus()
                        SaveData = False
                        logger.Debug("SaveData Exit with False")
                        Exit Function
                    End If

                    ' -- Warn the user of any changes that have an impact on external systems before performing the save.
                    If vendorKeyChanged Or psVendorChanged Or psVendorExportChanged And Not Me.IsInitializing Then
                        Dim vendorMessage As New StringBuilder

                        vendorMessage.Append(Environment.NewLine)
                        vendorMessage.Append(ResourcesItemHosting.GetString("msg_warning_VendorInfoChanged"))
                        vendorMessage.Append(Environment.NewLine)
                        vendorMessage.Append(Environment.NewLine)

                        If vendorKeyChanged Then
                            vendorMessage.Append(Environment.NewLine)
                            vendorMessage.Append(String.Format(ResourcesItemHosting.GetString("msg_warning_VendorKeyChanged"), _txtField_11.Text))
                            vendorMessage.Append(Environment.NewLine)
                        End If
                        If psVendorChanged Then
                            vendorMessage.Append(Environment.NewLine)
                            vendorMessage.Append(String.Format(ResourcesItemHosting.GetString("msg_warning_PSVendorChanged"), _txtField_23.Text))
                            vendorMessage.Append(Environment.NewLine)
                        End If
                        If psVendorExportChanged Then
                            vendorMessage.Append(Environment.NewLine)
                            vendorMessage.Append(String.Format(ResourcesItemHosting.GetString("msg_warning_PSVendorExportChanged"), TextBox_PSVendorExport.Text))
                            vendorMessage.Append(Environment.NewLine)
                        End If

                        vendorMessage.Append(Environment.NewLine)
                        vendorMessage.Append(Environment.NewLine)
                        vendorMessage.Append(ResourcesItemHosting.GetString("msg_warning_VendorInfoChangedInstruction"))
                        vendorMessage.Append(Environment.NewLine)

                        If MsgBox(vendorMessage.ToString, MsgBoxStyle.Exclamation + MsgBoxStyle.OkCancel, "Critical Information Changed") = MsgBoxResult.Cancel Then
                            SQLExecute("EXEC UnlockVendor " & plVendorID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
                            SaveData = False
                            Exit Function
                        End If

                    End If

                    ' Validate the PS Vendor id is 10 digits in length
                    If Not txtField(iVendorPS_Vendor_ID).Text.Trim().Length = 10 Then
                        ' pad it
                        _reqDigits = 10 - txtField(iVendorPS_Vendor_ID).Text.Trim().Length
                        _value = txtField(iVendorPS_Vendor_ID).Text.Trim()
                        Do Until _reqDigits = 0
                            _value = "0" + _value
                            _reqDigits = _reqDigits - 1
                        Loop

                        If _value <> "0" Then
                            txtField(iVendorPS_Vendor_ID).Text = _value
                        End If
                    End If

                    If Not TextBox_PSVendorExport.Text.Trim().Length = 10 Then
                        ' pad it
                        _reqDigits = 10 - TextBox_PSVendorExport.Text.Trim().Length
                        _value = TextBox_PSVendorExport.Text.Trim()
                        Do Until _reqDigits = 0
                            _value = "0" + _value
                            _reqDigits = _reqDigits - 1
                        Loop
                        TextBox_PSVendorExport.Text = _value
                    End If

                    'TFS 8316 Validate Fax number is entered and is valid if Fax is selected as Default PO Transmission
                    If optPOTrans(iVendorPOFaxTransmission).Checked Then
                        Dim pattern As String = "^(1\s*[-\/\.]?)?(\((\d{3})\)|(\d{3}))\s*[-\/\.]?\s*(\d{3})\s*[-\/\.]?\s*(\d{4})\s*(([xX]|[eE][xX][tT])\.?\s*(\d+))*$"
                        Dim faxNumberMatch As Match

                        If String.IsNullOrEmpty(txtField(iVendorFax).Text) Then
                            MsgBox(String.Format(ResourcesItemHosting.GetString("msg_warning_FaxNumberRequired"), _optPOTrans_0.Text), MsgBoxStyle.Exclamation, Application.ProductName)
                            logger.Info(String.Format(ResourcesItemHosting.GetString("msg_warning_FaxNumberRequired"), _optPOTrans_0.Text))
                            tabVendor.SelectTab(0)
                            txtField(iVendorFax).Focus()
                            SaveData = False
                            logger.Debug("SaveData Exit with False")
                            Exit Function
                        End If


                        faxNumberMatch = Regex.Match(txtField(iVendorFax).Text, pattern)

                        If Not faxNumberMatch.Success Then
                            MsgBox(String.Format(ResourcesItemHosting.GetString("msg_warning_FaxNumberRequired"), _optPOTrans_0.Text), MsgBoxStyle.Exclamation, Application.ProductName)
                            logger.Info(String.Format(ResourcesItemHosting.GetString("msg_warning_FaxNumberRequired"), _optPOTrans_0.Text))
                            tabVendor.SelectTab(0)
                            txtField(iVendorFax).Focus()
                            SaveData = False
                            logger.Debug("SaveData Exit with False")
                            Exit Function
                        End If

                        'TFS 8316 Validate Email address is entered and is valid if Email is selected as Default PO Transmission
                    ElseIf optPOTrans(iVendorPOEmailTransmission).Checked Then
                        Dim pattern As String = "^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"
                        Dim emailAddressMatch As Match

                        If String.IsNullOrEmpty(txtField(iVendorEmail).Text) Then
                            MsgBox(String.Format(ResourcesItemHosting.GetString("msg_warning_EmailRequired"), _optPOTrans_0.Text), MsgBoxStyle.Exclamation, Application.ProductName)
                            logger.Info(String.Format(ResourcesItemHosting.GetString("msg_warning_EmailRequired"), _optPOTrans_0.Text))
                            tabVendor.SelectTab(0)
                            txtField(iVendorEmail).Focus()
                            SaveData = False
                            logger.Debug("SaveData Exit with False")
                            Exit Function
                        End If


                        emailAddressMatch = Regex.Match(txtField(iVendorEmail).Text, pattern)

                        If Not emailAddressMatch.Success Then
                            MsgBox(String.Format(ResourcesItemHosting.GetString("msg_warning_EmailRequired"), _optPOTrans_0.Text), MsgBoxStyle.Exclamation, Application.ProductName)
                            logger.Info(String.Format(ResourcesItemHosting.GetString("msg_warning_EmailRequired"), _optPOTrans_0.Text))
                            tabVendor.SelectTab(0)
                            txtField(iVendorEmail).Focus()
                            SaveData = False
                            logger.Debug("SaveData Exit with False")
                            Exit Function
                        End If
                    End If


                Case Else

                    ' user clicked cancel or otherwise closed the dialog without a Yes or No
                    ' do nothing
                    SaveData = False
                    Exit Function

            End Select

        End If

SaveAndClose:

        If pbDataChanged Then
            ' TFS 8316
            ' Map Radio button to character for POTransmssionTypeID
            Dim POTransmissionTypeID As Integer
            If optPOTrans(iVendorPOFaxTransmission).Checked Then
                POTransmissionTypeID = 1 'Fax
            ElseIf optPOTrans(iVendorPOEmailTransmission).Checked Then
                POTransmissionTypeID = 2 'Email
            ElseIf optPOTrans(iVendorPOManualTransmission).Checked Then
                POTransmissionTypeID = 3 'Manual
            ElseIf optPOTrans(iVendorPOElectronicTransmission).Checked Then
                POTransmissionTypeID = 4 'Electronic
            End If

            'Dim s As String = ComboValue(ComboBox_PaymentTerms).ToString()

            If Thread.CurrentThread.CurrentUICulture.Name = "en-GB" Then
                ' State and PayTo_State will be NULL
                ' County and PayTo_County will be populated

                SQLExecute("EXEC UpdateVendorInfo " & plVendorID & ", '" & _
                        txtField(iVendorVendor_Key).Text & "', '" & _
                        ConvertQuotes(txtField(iVendorCompanyName).Text) & "', '" & _
                        ConvertQuotes(txtField(iVendorAddress_Line_1).Text) & "', '" & _
                        ConvertQuotes(txtField(iVendorAddress_Line_2).Text) & "', '" & _
                        ConvertQuotes(txtField(iVendorCity).Text) & "', '', '" & _
                        txtField(iVendorZip_Code).Text & "', '" & _
                        ConvertQuotes(txtField(iVendorCountry).Text) & "', '" & _
                        ConvertQuotes(txtCounty.Text) & "', '" & _
                        txtField(iVendorPhone).Text & "', '" & _
                        txtField(iVendorPhone_Ext).Text & "', '" & _
                        txtField(iVendorFax).Text & "', '" & _
                        ConvertQuotes(txtField(iVendorPayTo_CompanyName).Text) & "', " & "'" & _
                        ConvertQuotes(txtField(iVendorPayTo_Attention).Text) & "', '" & _
                        ConvertQuotes(txtField(iVendorPayTo_Address_Line_1).Text) & "', '" & _
                        ConvertQuotes(txtField(iVendorPayTo_Address_Line_2).Text) & "', '" & _
                        ConvertQuotes(txtField(iVendorPayTo_City).Text) & "', '', " & "'" & _
                        txtField(iVendorPayTo_Zip_Code).Text & "', '" & _
                        ConvertQuotes(txtField(iVendorPayTo_Country).Text) & "', '" & _
                        ConvertQuotes(txtPayToCounty.Text) & "', '" & _
                        txtField(iVendorPayTo_Phone).Text & "', '" & _
                        txtField(iVendorPayTo_Phone_Ext).Text & "', '" & _
                        txtField(iVendorPayTo_Fax).Text & "', " & "'" & _
                        txtField(iVendorPS_Vendor_ID).Text & "', '" & _
                        TextBox_PSVendorExport.Text & "', '" & _
                        txtField(iVendorPS_Location_Code).Text & "', '" & _
                        txtField(iVendorPS_Address_Sequence).Text & "', '" & _
                        ConvertQuotes(txtField(iVendorComments).Text) & "', -" & _
                        chkField(iVendorCustomer).CheckState & ", -" & _
                        chkField(iVendorInternalCustomer).CheckState & ", -" & _
                        chkField(iVendorWFM).CheckState & ", '" & _
                        txtField(iVendorDefault_Account_No).Text & "', " & _
                        chkNonProductVendor.CheckState & ",'" & _
                        txtField(iVendorEmail).Text & "', -" & _
                        chkField(iVendorEFT).CheckState & ", '" & _
                        txtField(iVendorPONote).Text & "', '" & _
                        txtField(iVendorReceivingAuthNote).Text & "', '" & _
                        txtField(iVendorOtherName).Text & "', '" & _
                        Thread.CurrentThread.CurrentUICulture.Name & "', " & _
                        IIf(txtField(iCaseDistHandlingCharge).Text = "", 0, _
                        txtField(iCaseDistHandlingCharge).Text) & ", " & _
                        POTransmissionTypeID & ", " & _
                        IIf(CheckBox_EInvoicing.Checked, 1, 0) & ", " & _
                        IIf(CheckBox_EinvoiceReqd.Checked, 1, 0) & ", " & _
                        ComboValue(cmbCurrency) & ", '" & _
                        txtLeadTimeDays.Text.Trim & "', " & _
                        cmbLeadTimeDayOfWeek.SelectedIndex + 1 & ", " & _
                        giUserID & ", '" & _
                        txtAccountingContactEmail.Text & "'," & _
                        ComboValue(ComboBox_PaymentTerms) & ", " & _
                        CheckBoxAllowReceiveAll.CheckState & ", " & _
                        IIf(cbxShortpayProhibited.Checked, 1, 0) & ", " & _
                        IIf(cbxActive.Checked, 1, 0) & ", " & _
                        IIf(CheckBoxAllowBarcodePOReport.Checked, 1, 0),
                        DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Else
                ' State and PayTo_State will be populated
                ' County and PayTo_County will be NULL
                SQLExecute("EXEC UpdateVendorInfo " & plVendorID & ", '" & _
                           txtField(iVendorVendor_Key).Text & "', '" & _
                           ConvertQuotes(txtField(iVendorCompanyName).Text) & "', '" & _
                           ConvertQuotes(txtField(iVendorAddress_Line_1).Text) & "', '" & _
                           ConvertQuotes(txtField(iVendorAddress_Line_2).Text) & "', '" & _
                           ConvertQuotes(txtField(iVendorCity).Text) & "', " & "'" & _
                           txtField(iVendorState).Text & "', '" & _
                           txtField(iVendorZip_Code).Text & "', '" & _
                           ConvertQuotes(txtField(iVendorCountry).Text) & "', '', '" & _
                           txtField(iVendorPhone).Text & "', '" & _
                           txtField(iVendorPhone_Ext).Text & "', '" & _
                           txtField(iVendorFax).Text & "', '" & _
                           ConvertQuotes(txtField(iVendorPayTo_CompanyName).Text) & "', " & "'" & _
                           ConvertQuotes(txtField(iVendorPayTo_Attention).Text) & "', '" & _
                           ConvertQuotes(txtField(iVendorPayTo_Address_Line_1).Text) & "', '" & _
                           ConvertQuotes(txtField(iVendorPayTo_Address_Line_2).Text) & "', '" & _
                           ConvertQuotes(txtField(iVendorPayTo_City).Text) & "', '" & _
                           txtField(iVendorPayTo_State).Text & "', " & "'" & _
                           txtField(iVendorPayTo_Zip_Code).Text & "', '" & _
                           ConvertQuotes(txtField(iVendorPayTo_Country).Text) & "', '', '" & _
                           txtField(iVendorPayTo_Phone).Text & "', '" & _
                           txtField(iVendorPayTo_Phone_Ext).Text & "', '" & _
                           txtField(iVendorPayTo_Fax).Text & "', " & "'" & _
                           txtField(iVendorPS_Vendor_ID).Text & "', '" & _
                           TextBox_PSVendorExport.Text & "', '" & _
                           txtField(iVendorPS_Location_Code).Text & "', '" & _
                           txtField(iVendorPS_Address_Sequence).Text & "', '" & _
                           ConvertQuotes(txtField(iVendorComments).Text) & "', -" & _
                           chkField(iVendorCustomer).CheckState & ", -" & _
                           chkField(iVendorInternalCustomer).CheckState & ", -" & _
                           chkField(iVendorWFM).CheckState & ", '" & _
                           txtField(iVendorDefault_Account_No).Text & "', " & _
                           chkNonProductVendor.CheckState & ",'" & _
                           txtField(iVendorEmail).Text & "', -" & _
                           chkField(iVendorEFT).CheckState & ", '" & _
                           txtField(iVendorPONote).Text & "', '" & _
                           txtField(iVendorReceivingAuthNote).Text & "', '" & _
                           txtField(iVendorOtherName).Text & "', '" & _
                           Thread.CurrentThread.CurrentUICulture.Name & "', " & _
                           IIf(txtField(iCaseDistHandlingCharge).Text = "", 0, _
                           txtField(iCaseDistHandlingCharge).Text) & ", " & _
                           POTransmissionTypeID & ", " & _
                           IIf(CheckBox_EInvoicing.Checked, 1, 0) & ", " & _
                           IIf(CheckBox_EinvoiceReqd.Checked, 1, 0) & ", " & _
                           ComboValue(cmbCurrency) & ", '" & _
                           txtLeadTimeDays.Text.Trim & "', " & _
                           cmbLeadTimeDayOfWeek.SelectedIndex + 1 & ", " & _
                           giUserID & ", '" & _
                           txtAccountingContactEmail.Text & "'," & _
                           ComboValue(ComboBox_PaymentTerms) & ", " & _
                           CheckBoxAllowReceiveAll.CheckState & ", " & _
                           IIf(cbxShortpayProhibited.Checked, 1, 0) & ", " & _
                           IIf(cbxActive.Checked, 1, 0) & ", " & _
                            IIf(CheckBoxAllowBarcodePOReport.Checked, 1, 0),
                        DAO.RecordsetOptionEnum.dbSQLPassThrough)
            End If
        Else
            SQLExecute("EXEC UnlockVendor " & plVendorID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End If

        logger.Debug("SaveData Exit")

    End Function

    Private Sub frmVendor_Activated(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Activated
        If Me.IsInitializing Then Exit Sub
        '-- Make sure there are vendors
        CheckNoVendors()

    End Sub

    Private Sub frmVendor_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load


        logger.Debug("frmVendor_Load Entry")

        Dim region As String
        region = ConfigurationServices.AppSettings("region")

        If region = "EU" Then
            lblKey.Text = "IRIS Code :"
            lblState.Text = "County :"
            lblZip.Text = "Postal Code :"
            lblCity.Text = "Town/City :"
            _lblLabel_11.Text = "Postal Code :"
            _lblLabel_12.Text = "County :"
            _lblLabel_13.Text = "Town/City :"
        End If

        '-- Center the form
        CenterForm(Me)

        hideVendorItemButton = InstanceDataDAO.IsFlagActive("HideVendorItemButtonOnVendorInfoScreen")
        multipleCurrencies = InstanceDataDAO.IsFlagActive("MultipleCurrencies")

        LoadInventoryStore(cmbField(iVendorStore_No))

        LoadCurrency(cmbCurrency)

        LoadPaymentTerms(ComboBox_PaymentTerms)

        If bSpecificVendor = True Then
            RefreshDataSource(glVendorID)
        Else
            RefreshDataSource(-1)

            cmdCompanySearch_Click(eventSender, eventArgs)
        End If

        SetConfiguredMsg()

        'Look at InstanceDataFlags, figure out if Fax transmission is allowed.
        Dim allowFaxOption = InstanceDataDAO.IsFlagActive("AllowVendorWithFax") AndAlso gbSuperUser
        'If InstanceDataDAO.IsFlagActive("AllowVendorWithFax") AndAlso (cmbLabelType.Text Is Nothing Or cmbLabelType.Text = "") Then
        _optPOTrans_0.Enabled = allowFaxOption
        _optPOTrans_0.Visible = allowFaxOption
        _txtField_9.Enabled = allowFaxOption
        _txtField_9.Visible = allowFaxOption

        logger.Debug("frmVendor_Load Exit")
    End Sub

    Sub CheckNoVendors()
        logger.Debug("CheckNoVendors Entry")

        '-- Make sure there is data for them to be in this form
        If plVendorID = -1 Then
            If MsgBox("NoVendorsFound", MsgBoxStyle.YesNo + MsgBoxStyle.Question, Me.Text) = MsgBoxResult.Yes Then
                logger.Info("NoVendorsFound-Yes")

                cmdAdd_Click(cmdAdd, New System.EventArgs())
                If plVendorID = -1 Then
                    Me.Close()
                End If
            Else
                Me.Close()
            End If
        End If

        logger.Debug("CheckNoVendors")

    End Sub

    Private Sub RefreshDataSource(ByRef lRecord As Integer)

        logger.Debug("RefreshDataSource Entry")

        Dim iLoop As Short
        Dim iLoop2 As Short
        Dim ctrl As Control
        Dim iDefaultPOTransmission As Integer

        iDefaultPOTransmission = ConfigurationServices.AppSettings("DefaultPOTransmission")

        Select Case lRecord
            Case -2 : SQLOpenRS(rsVendor, "EXEC GetVendorInfoLast", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Case -1 : SQLOpenRS(rsVendor, "EXEC GetVendorInfoFirst", DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Case Else : SQLOpenRS(rsVendor, "EXEC GetVendorInfo " & lRecord, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough)
        End Select

        If rsVendor.EOF Then

            plVendorID = -1

        Else

            plVendorID = rsVendor.Fields("Vendor_ID").Value
            glVendorID = plVendorID
            _PS_Export_Vendor_ID = rsVendor.Fields("PS_Export_Vendor_ID").Value & ""
            ToolStripDropDownButton1.Enabled = False

            ' This RunWorkerAsync call is made twice in quick succession - during the form loading event, and again in
            ' the cmdCompanySearch_Click event.  Check the busy state of the process to ensure that
            ' the second call waits for the first.
            If BackgroundWorker1.IsBusy = False Then
                BackgroundWorker1.RunWorkerAsync()
            Else
                ToolStripStatusLabel1.Text = "Waiting for operation to complete..."
            End If

    '-- Do Text fields
            txtField(iVendorVendor_Key).Text = rsVendor.Fields("Vendor_Key").Value & ""
            txtField(iVendor_ID).Text = rsVendor.Fields("Vendor_ID").Value & ""
            txtField(iVendorCompanyName).Text = rsVendor.Fields("CompanyName").Value & ""
            txtField(iVendorAddress_Line_1).Text = rsVendor.Fields("Address_Line_1").Value & ""
            txtField(iVendorAddress_Line_2).Text = rsVendor.Fields("Address_Line_2").Value & ""
            txtField(iVendorCity).Text = rsVendor.Fields("City").Value & ""
            If Thread.CurrentThread.CurrentUICulture.Name = "en-GB" Then
    ' State and PayTo_State will be NULL
    ' County and PayTo_County will be populated
                txtCounty.Text = rsVendor.Fields("County").Value & ""
                txtPayToCounty.Text = rsVendor.Fields("PayTo_County").Value & ""
            Else
    ' State and PayTo_State will be populated
    ' County and PayTo_County will be NULL
                txtField(iVendorState).Text = rsVendor.Fields("State").Value & ""
                txtField(iVendorPayTo_State).Text = rsVendor.Fields("PayTo_State").Value & ""
            End If

            txtField(iVendorZip_Code).Text = rsVendor.Fields("Zip_Code").Value & ""
            txtField(iVendorCountry).Text = rsVendor.Fields("Country").Value & ""
            txtField(iVendorPhone).Text = rsVendor.Fields("Phone").Value & ""
            txtField(iVendorPhone_Ext).Text = rsVendor.Fields("Phone_Ext").Value & ""
            txtField(iVendorFax).Text = rsVendor.Fields("Fax").Value & ""
            txtField(iVendorPayTo_CompanyName).Text = rsVendor.Fields("PayTo_CompanyName").Value & ""
            txtField(iVendorPayTo_Attention).Text = rsVendor.Fields("PayTo_Attention").Value & ""
            txtField(iVendorPayTo_Address_Line_1).Text = rsVendor.Fields("PayTo_Address_Line_1").Value & ""
            txtField(iVendorPayTo_Address_Line_2).Text = rsVendor.Fields("PayTo_Address_Line_2").Value & ""
            txtField(iVendorPayTo_City).Text = rsVendor.Fields("PayTo_City").Value & ""
            txtField(iVendorPayTo_Zip_Code).Text = rsVendor.Fields("PayTo_Zip_Code").Value & ""
            txtField(iVendorPayTo_Country).Text = rsVendor.Fields("PayTo_Country").Value & ""
            txtField(iVendorPayTo_Phone).Text = rsVendor.Fields("PayTo_Phone").Value & ""
            txtField(iVendorPayTo_Phone_Ext).Text = rsVendor.Fields("PayTo_Phone_Ext").Value & ""
            txtField(iVendorPayTo_Fax).Text = rsVendor.Fields("PayTo_Fax").Value & ""
            txtField(iVendorPS_Vendor_ID).Text = rsVendor.Fields("PS_Vendor_ID").Value & ""
            TextBox_PSVendorExport.Text = _PS_Export_Vendor_ID
            txtField(iVendorPS_Location_Code).Text = rsVendor.Fields("PS_Location_Code").Value & ""
            txtField(iVendorPS_Address_Sequence).Text = rsVendor.Fields("PS_Address_Sequence").Value & ""
            txtField(iVendorComments).Text = rsVendor.Fields("Comment").Value & ""
            txtField(iVendorDefault_Account_No).Text = rsVendor.Fields("Default_GLNumber").Value & ""
            txtField(iVendorEmail).Text = rsVendor.Fields("Email").Value & ""

            txtField(iVendorOtherName).Text = rsVendor.Fields("Other_Name").Value & ""
            txtField(iVendorPONote).Text = rsVendor.Fields("PO_Note").Value & ""
            txtField(iVendorReceivingAuthNote).Text = rsVendor.Fields("Receiving_Authorization_Note").Value & ""
            txtField(iCaseDistHandlingCharge).Text = VB6.Format(rsVendor.Fields("CaseDistHandlingCharge").Value, "#####0.00##") & ""
    'DN added 8/5/2009 per 10619
            txtAccountingContactEmail.Text = rsVendor.Fields("AccountingContactEmail").Value & ""
    'DN end new 8/5/2009
    '-- Set check boxes
            CheckBox_EInvoicing.CheckState = System.Math.Abs(CInt(rsVendor.Fields("EInvoicing").Value))
            CheckBox_EinvoiceReqd.CheckState = System.Math.Abs(CInt(rsVendor.Fields("EinvoiceRequired").Value))
            chkField(iVendorCustomer).CheckState = System.Math.Abs(CInt(rsVendor.Fields("Customer").Value))
            chkField(iVendorInternalCustomer).CheckState = System.Math.Abs(CInt(rsVendor.Fields("InternalCustomer").Value))
            chkField(iVendorWFM).CheckState = System.Math.Abs(CInt(rsVendor.Fields("WFM").Value))
            chkNonProductVendor.CheckState = System.Math.Abs(CInt(rsVendor.Fields("Non_Product_Vendor").Value))
            chkField(iVendorEFT).CheckState = System.Math.Abs(CInt(rsVendor.Fields("EFT").Value))
            CheckBoxAllowReceiveAll.CheckState = System.Math.Abs(CInt(rsVendor.Fields("AllowReceiveAll").Value))
            CheckBoxAllowBarcodePOReport.CheckState = System.Math.Abs(CInt(rsVendor.Fields("AllowBarcodePOReport").Value))
            'DN: 11/05/2012 - TFS 8323 Added Shortpay Prohibited checkbox
            '4.8 - Refusal functionality is disabled until final requirements are delivered.
            'cbxShortpayProhibited.CheckState = System.Math.Abs(CInt(rsVendor.Fields("ShortpayProhibited").Value))

            cbxActive.CheckState = System.Math.Abs(CInt(rsVendor.Fields("ActiveVendor").Value))

            If rsVendor.Fields("PaymentTermID").Value IsNot DBNull.Value Then
                SetCombo(ComboBox_PaymentTerms, rsVendor.Fields("PaymentTermID").Value)
            End If

            ' Lead-Time field setup.
            ResetLeadTimeFields()
            ' If the vendor is not a lead-time vendor, the fields in the lead-time tab should be left blank.
            chkEnableLeadTime.Checked = CBool(rsVendor.Fields("IsLeadTimeVendor").Value)
            If chkEnableLeadTime.Checked Then
                Dim leadTimeDays As Integer = CInt(rsVendor.Fields("LeadTimeDays").Value)
                Dim leadTimeDayOfWeek As Integer = -1
                If Not IsDBNull(rsVendor.Fields("LeadTimeDayOfWeek").Value) Then leadTimeDayOfWeek = CInt(rsVendor.Fields("LeadTimeDayOfWeek").Value)
                If leadTimeDays > 0 Then
                    txtLeadTimeDays.Text = leadTimeDays
                ElseIf leadTimeDayOfWeek > 0 Then
                    chkUseLeadTimeDayOfWeek.Checked = True
                    ' DOW index is one-based, but combo boxes are zero-based, so we subtract one to select correct DOW.
                    cmbLeadTimeDayOfWeek.SelectedIndex = leadTimeDayOfWeek - 1
                End If
            End If

            ' Task 1300 - Enable Lead Time info only for Admins
            SetActive(txtLeadTimeDays, (gbVendorAdministrator Or gbSuperUser))
            SetActive(chkUseLeadTimeDayOfWeek, (gbVendorAdministrator Or gbSuperUser))
            SetActive(cmbLeadTimeDayOfWeek, (gbVendorAdministrator Or gbSuperUser))

            '-- set Combo boxes
            SetCombo(cmbField(iVendorStore_No), rsVendor.Fields("Store_No").Value)
            SetCombo(cmbCurrency, rsVendor.Fields("CurrencyID").Value)

            '-- Task 8316 1/6/2009 - Load Default PO Transmission Value
            '-- set Radio buttons
            If Not IsDBNull(rsVendor.Fields("POTransmissionTypeID").Value) Then
                Select Case rsVendor.Fields("POTransmissionTypeID").Value
                    Case 1 'Fax
                        optPOTrans(iVendorPOFaxTransmission).Checked = True
                    Case 2 'Email
                        optPOTrans(iVendorPOEmailTransmission).Checked = True
                    Case 4 'Electronic
                        optPOTrans(iVendorPOElectronicTransmission).Checked = True
                    Case Else 'Manual
                        optPOTrans(iVendorPOManualTransmission).Checked = True
                End Select
                'If Vendor PO Transmission is not set, use the regional default set in the client config
            ElseIf Not IsDBNull(iDefaultPOTransmission) Then
                Select Case iDefaultPOTransmission
                    Case 1 'Fax
                        optPOTrans(iOrderHeaderPOFaxTransmission).Checked = True
                    Case 2 'Email
                        optPOTrans(iOrderHeaderPOEmailTransmission).Checked = True
                    Case 4 'Electronic
                        optPOTrans(iVendorPOElectronicTransmission).Checked = True
                    Case Else
                        optPOTrans(iOrderHeaderPOManualTransmission).Checked = True
                End Select
            Else
                '-- Clear any previous selections on the form
                optPOTrans(iVendorPOFaxTransmission).Checked = False
                optPOTrans(iVendorPOEmailTransmission).Checked = False
                optPOTrans(iVendorPOManualTransmission).Checked = False
                optPOTrans(iVendorPOElectronicTransmission).Checked = False
            End If


            '-- Do file locking
            If IsDBNull(rsVendor.Fields("User_ID").Value) Then
                lblReadOnly.Visible = False
                cmdUnlock.Enabled = False
                'SQLExecute("EXEC LockVendor " & plVendorID & ", " & giUserID, DAO.RecordsetOptionEnum.dbSQLPassThrough)
            Else
                lblReadOnly.Text = "Read Only (" & GetInvUserName(rsVendor.Fields("User_ID").Value) & ")"
                lblReadOnly.Visible = True
                cmdUnlock.Enabled = (gbLockAdministrator Or giUserID = rsVendor.Fields("User_ID").Value)
            End If

            '-- Set whats active
            For iLoop = 0 To tabVendor.Controls.Count - 1
                For iLoop2 = 0 To tabVendor.Controls(iLoop).Controls.Count - 1
                    ctrl = tabVendor.Controls(iLoop).Controls(iLoop2)

                    If TypeOf ctrl Is CheckBox Then
                        SetActive(CType(ctrl, CheckBox), (Not lblReadOnly.Visible) And gbVendorAdministrator)
                    ElseIf TypeOf ctrl Is TextBox Then
                        SetActive(CType(ctrl, TextBox), (Not lblReadOnly.Visible) And gbVendorAdministrator)
                    ElseIf TypeOf ctrl Is RadioButton Then
                        SetActive(CType(ctrl, RadioButton), (Not lblReadOnly.Visible) And gbVendorAdministrator)
                    End If
                Next
            Next

            ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
            cbxShortpayProhibited.Enabled = False

            SetActive(txtCounty, (Not lblReadOnly.Visible) And gbVendorAdministrator)
            SetActive(txtPayToCounty, (Not lblReadOnly.Visible) And gbVendorAdministrator)

            If (Not lblReadOnly.Visible) And gbItemAdministrator Then
                SetActive(txtField(iVendorPhone), True)
                SetActive(txtField(iVendorPhone_Ext), True)
                SetActive(txtField(iVendorFax), True)
                SetActive(txtField(iVendorVendor_Key), True)
                SetActive(txtField(iVendorEmail), True)
            End If

            For iLoop = chkField.LBound To chkField.UBound

                Select Case iLoop
                    Case iVendorWFM, iVendorEFT
                        SetActive(chkField(iLoop), (Not lblReadOnly.Visible) And gbVendorAdministrator)
                    Case Else
                        SetActive(chkField(iLoop), (Not lblReadOnly.Visible) And gbSuperUser)
                End Select
            Next iLoop

            SetActive(chkNonProductVendor, (Not lblReadOnly.Visible) And gbVendorAdministrator)

            SetActive(txtField(iVendor_ID), False)
            SetActive(cmdDelete, (Not lblReadOnly.Visible) And gbVendorAdministrator)
            SetActive(cmdItems, (Not lblReadOnly.Visible) And (gbBuyer Or gbAccountant Or gbVendorAdministrator Or gbDistributor Or gbItemAdministrator), hideVendorItemButton)
            SetActive(cmdContacts, (Not lblReadOnly.Visible) And (gbBuyer Or gbAccountant Or gbVendorAdministrator))
            SetActive(cmdSameData, (Not lblReadOnly.Visible) And (gbVendorAdministrator))
            SetActive(cmdAdd, gbVendorAdministrator)
            SetActive(cmbField(iVendorStore_No), (Not lblReadOnly.Visible) And gbVendorAdministrator)

            'MD 7/31/2009, WI 10499 applied the restriction rules to the Store Level Override Button
            SetActive(StoreLevelOverridesButton, (Not lblReadOnly.Visible) And (gbVendorAdministrator Or gbSuperUser))
            SetActive((cmbCurrency), (Not lblReadOnly.Visible) And (gbVendorAdministrator Or gbSuperUser) And multipleCurrencies)

            'TFS 2455, DSD Vendors Enhancement
            SetActive(CheckBox_EinvoiceReqd, ((gbVendorAdministrator Or gbSuperUser) And CheckBox_EInvoicing.Checked))
            SetActive(DSDVendor_StoreSetup, (gbVendorAdministrator Or gbSuperUser) And CheckBox_EInvoicing.Checked And CheckBox_EinvoiceReqd.Checked And optPOTrans(iVendorPOElectronicTransmission).Checked And DVOVendorSetupOK)

            cmbCurrency.Visible = multipleCurrencies
            lblCurrency.Visible = multipleCurrencies

            If Not IsDBNull(rsVendor.Fields("Store_No").Value) Then
                If VendorDAO.IsStoreDistributionCenter(rsVendor.Fields("Store_No").Value) Then
                    If gbDCAdmin Then
                        SetActive(txtField(iCaseDistHandlingCharge), True)
                    Else
                        SetActive(txtField(iCaseDistHandlingCharge), False)
                    End If
                Else
                    SetActive(txtField(iCaseDistHandlingCharge), False)
                End If
            End If
        End If
        rsVendor.Close()

        chkNonProductVendor_CheckStateChanged(chkNonProductVendor, New System.EventArgs())

        pbDataChanged = False
        vendorKeyChanged = False
        psVendorExportChanged = False
        psVendorChanged = False

        If lRecord <> -1 And plVendorID = -1 Then
            RefreshDataSource(-1)
            CheckNoVendors()
        End If

        logger.Debug("RefreshDataSource Exit")
    End Sub

    Private Sub frmVendor_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        logger.Debug("frmVendor_FormClosing Entry")
        If plVendorID > -1 Then
            e.Cancel = Not SaveData()
        End If
        logger.Debug("frmVendor_FormClosing Exit")

    End Sub

    Private Sub SetConfiguredMsg()

        Me._label_ConfiguredAs.ForeColor = System.Drawing.SystemColors.ControlText

        If Not Me._chkField_0.Checked And Not Me._chkField_1.Checked And Not Me._chkField_2.Checked Then

            Me._label_ConfiguredAs.Text = ResourcesItemHosting.GetString("msg_configureVendor_ExternalVendor")

        ElseIf Me._chkField_0.Checked And Me._chkField_1.Checked And Not Me._chkField_2.Checked Then

            Me._label_ConfiguredAs.Text = ResourcesItemHosting.GetString("msg_configureVendor_RegionalCustomerVendor")

        ElseIf Me._chkField_2.Checked And Not Me._chkField_0.Checked And Not Me._chkField_1.Checked Then

            Me._label_ConfiguredAs.Text = ResourcesItemHosting.GetString("msg_configureVendor_NonRegionalWFMFacility")

        ElseIf Me._chkField_2.Checked And Me._chkField_0.Checked And Me._chkField_1.Checked Then

            Me._label_ConfiguredAs.Text = ResourcesItemHosting.GetString("msg_configureVendor_NonRegionalWFMFacility")

        ElseIf Me._chkField_0.Checked And Me._chkField_2.Checked And Not Me._chkField_1.Checked Then

            Me._label_ConfiguredAs.Text = ResourcesItemHosting.GetString("msg_configureVendor_NonRegionalWFMFacility")

        Else

            Me._label_ConfiguredAs.ForeColor = Color.Red
            Me._label_ConfiguredAs.Text = ResourcesItemHosting.GetString("msg_configureVendor_NotSupported")

        End If

        Me._label_ConfiguredAs.Refresh()

    End Sub

    Private Sub StoreLevelOverridesButton_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles StoreLevelOverridesButton.Click
        frmVendorPOPaymentTypeSettings.ShowDialog()
        frmVendorPOPaymentTypeSettings.Dispose()
    End Sub

    Private Sub ComboBox_PaymentTerms_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_PaymentTerms.SelectedIndexChanged
        If Me.IsInitializing Then Exit Sub

        pbDataChanged = True
    End Sub

    Private Sub chkEnableLeadTime_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkEnableLeadTime.CheckedChanged
        If Me.IsInitializing Then Exit Sub

        pbDataChanged = True
        txtAuthorizedBy.Text = gsUserName
        txtAuthorziedDate.Text = Date.Now

        If chkEnableLeadTime.Checked Then
            txtLeadTimeDays.Enabled = True
            chkUseLeadTimeDayOfWeek.Enabled = True
        Else
            txtLeadTimeDays.Text = 0
            txtLeadTimeDays.Enabled = False
            chkUseLeadTimeDayOfWeek.Enabled = False
        End If

        chkUseLeadTimeDayOfWeek.Checked = False
        cmbLeadTimeDayOfWeek.Enabled = False
        cmbLeadTimeDayOfWeek.SelectedIndex = -1
    End Sub

    Private Sub chkUseLeadTimeDayOfWeek_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkUseLeadTimeDayOfWeek.CheckedChanged
        If Me.IsInitializing Then Exit Sub

        pbDataChanged = True
        If chkUseLeadTimeDayOfWeek.Checked Then
            cmbLeadTimeDayOfWeek.Enabled = True
            txtLeadTimeDays.Text = 0
            txtLeadTimeDays.Enabled = False
        Else
            cmbLeadTimeDayOfWeek.Enabled = False
            txtLeadTimeDays.Enabled = True
        End If
    End Sub

    Private Sub txtLeadTimeDays_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtLeadTimeDays.TextChanged
        pbDataChanged = True
    End Sub

    Private Sub cmbLeadTimeDayOfWeek_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbLeadTimeDayOfWeek.SelectedIndexChanged
        pbDataChanged = True
    End Sub

    Private Sub ResetLeadTimeFields()
        chkEnableLeadTime.Enabled = True
        chkEnableLeadTime.Checked = False
        txtLeadTimeDays.Enabled = False
        txtLeadTimeDays.Text = 0

        chkUseLeadTimeDayOfWeek.Enabled = False
        chkUseLeadTimeDayOfWeek.Checked = False
        cmbLeadTimeDayOfWeek.Enabled = False
        cmbLeadTimeDayOfWeek.SelectedIndex = -1

        txtAuthorizedBy.Clear()
        txtAuthorziedDate.Clear()
    End Sub


    Private Sub dsdVendorSetup_Click(sender As System.Object, e As System.EventArgs) Handles DSDVendor_StoreSetup.Click
        frmReceivingDocumentSetting.ShowDialog()
        frmReceivingDocumentSetting.Dispose()
    End Sub

    Private Sub CheckBoxAllowReceiveAll_CheckedChanged(sender As System.Object, e As System.EventArgs) Handles CheckBoxAllowReceiveAll.CheckedChanged
        pbDataChanged = True
    End Sub

    Private Sub BackgroundWorker_DoWork(sender As System.Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        BackgroundWorker1.ReportProgress(1, New BackgroundInformationObject("Validing vendor credit status in DVO...", Nothing))
        Dim svc As ElectronicOrderWebService = New ElectronicOrderWebService()


        Dim serviceReturnValue As String
        Try
            serviceReturnValue = svc.isCreditVendor(_PS_Export_Vendor_ID)
            Select Case serviceReturnValue.ToLower
                Case "true"
                    e.Result = New BackgroundInformationObject("true", Nothing)
                Case "false"
                    e.Result = New BackgroundInformationObject("false", Nothing)
                Case Else
                    Throw New Exception(String.Format("Unknown value was returned from the DVO webservice. value: [{0}] ", serviceReturnValue))
            End Select
        Catch ex As Exception
            e.Result = New BackgroundInformationObject("error", ex)
        End Try

    End Sub

    Private Sub BackgroundWorker_RunWorkerCompleted(sender As System.Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted
        Dim info As BackgroundInformationObject = DirectCast(e.Result, BackgroundInformationObject)
        If info.ErrorInfo Is Nothing Then
            With ToolStripStatusLabel1
                If info.Message = "true" Then
                    ToolStripDropDownButton1.Enabled = False
                    DVOVendorSetupOK = True
                    ' DSDVendor_StoreSetup.Enabled = True
                    SetActive(DSDVendor_StoreSetup, (gbVendorAdministrator Or gbSuperUser) And CheckBox_EInvoicing.Checked And CheckBox_EinvoiceReqd.Checked And optPOTrans(iVendorPOElectronicTransmission).Checked And DVOVendorSetupOK)
                    .Text = "Vendor setup in DVO has been validated. Receiving Document Settings enabled. "
                    .ForeColor = Color.Green
                Else
                    ToolStripDropDownButton1.Enabled = True
                    DVOVendorSetupOK = False
                    DSDVendor_StoreSetup.Enabled = False
                    .Text = "Vendor setup in DVO is not complete. Receiving Document Settings disabled."
                    .ToolTipText = "This vendor is not configured to support DSD ordering in DVO."
                    .ForeColor = Color.Crimson
                End If
            End With
        Else
            ToolStripDropDownButton1.Enabled = True
            DVOVendorSetupOK = False
            DSDVendor_StoreSetup.Enabled = False
            ToolStripStatusLabel1.Text = "Error: Vendor setup in DVO could not be validated. Receiving Document Settings disabled."
            ToolStripStatusLabel1.ToolTipText = info.ErrorInfo.Message
            ToolStripStatusLabel1.ForeColor = Color.Crimson
            logger.ErrorFormat("Vendor setup in DVO could not be validated")
            logger.ErrorFormat("{0}", info.ErrorInfo.Message)

        End If
    End Sub

    Private Sub BackgroundWorker_ProgressChanged(sender As System.Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged
        ToolStripStatusLabel1.Text = DirectCast(e.UserState, BackgroundInformationObject).Message
        ToolStripStatusLabel1.ForeColor = Color.Black
        ToolStripStatusLabel1.Invalidate()
    End Sub

    Private Class BackgroundInformationObject
        Public Message As String
        Public ErrorInfo As Exception

        Sub New(message As String, ErrorInfo As Exception)
            Me.Message = message
            Me.ErrorInfo = ErrorInfo
        End Sub
    End Class


    Private Sub ToolStripDropDownButton1_Click(sender As System.Object, e As System.EventArgs) Handles ToolStripDropDownButton1.Click
        ToolStripDropDownButton1.Enabled = False
        BackgroundWorker1.RunWorkerAsync()
    End Sub

    Private Sub cbxShortpayProhibited_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxShortpayProhibited.CheckedChanged
        ' 4.8 - Refusal functionality is disabled until final requirements are delivered.
        'pbDataChanged = True
    End Sub
    Private Sub CheckBoxAllowBarcodePOReport_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBoxAllowBarcodePOReport.CheckedChanged
        pbDataChanged = True
    End Sub

    Private Sub cbxActive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbxActive.CheckedChanged
        pbDataChanged = True
    End Sub
End Class
