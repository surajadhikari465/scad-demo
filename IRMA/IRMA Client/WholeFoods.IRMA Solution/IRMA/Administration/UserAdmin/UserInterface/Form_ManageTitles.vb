Imports System.Text
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic
Imports WholeFoods.IRMA.Administration.Common.DataAccess
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess
Imports System.Collections.Generic
Imports System.Collections.ObjectModel
Imports System.Net.Mail

Public Class Form_ManageTitles

    Private Sub Button_DeleteTitle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_DeleteTitle.Click
        Dim dtTitles As DataTable = TitleDAO.GetUsersWithTitle(ComboBox_Titles.SelectedValue)

        If dtTitles.Rows.Count > 0 Then
            Dim frm As New Form_TitleDeleteConflicts

            If MsgBox("There are users assigned to this title, would you like to view and change them so that this title can be deleted?", MsgBoxStyle.YesNo, "IRMA Delete Title") = MsgBoxResult.Yes Then
                frm.TitleId = ComboBox_Titles.SelectedValue
                frm.ShowDialog()

                If frm.ConflictsResolved Then
                    TitleDAO.DeleteTitle(ComboBox_Titles.SelectedValue)
                    LoadComboBox()
                End If

                frm.Dispose()
            End If
        Else
            Try
                If MsgBox("Are you sure you want to delete the " & ComboBox_Titles.Text & " title?", MsgBoxStyle.YesNo, "IRMA Delete Title") = MsgBoxResult.Yes Then
                    TitleDAO.DeleteTitle(ComboBox_Titles.SelectedValue)
                    LoadComboBox()
                End If
            Catch ex As Exception
                Logger.LogError("Exception: ", Me.GetType(), ex)
                MessageBox.Show(ex.Message)
                Dim args(1) As String
                args(0) = "Form_ManageTitles form: Button_DeleteTitle_Click"
                ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
                Me.Close()
            End Try
        End If
    End Sub

    Private Sub Button_AddTitle_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_AddTitle.Click
        Dim frm As Form_AddEditTitle = New Form_AddEditTitle

        frm.IsEdit = False
        frm.TitleId = ComboBox_Titles.SelectedValue
        frm.ShowDialog()

        Try
            If frm.TitleDesc.ToString <> "" Then
                TitleDAO.AddTitle(frm.TitleDesc.ToString)
            End If
        Catch ex As Exception
            Logger.LogError("Exception: ", Me.GetType(), ex)
            MessageBox.Show(ex.Message)
            Dim args(1) As String
            args(0) = "Form_ManageTitles form: Button_AddTitle_Click"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
            Me.Close()
        Finally
            frm.Dispose()
            LoadComboBox()
        End Try
    End Sub

    Private Sub Form_ManageTitles_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadComboBox()
        SetToolTips()
    End Sub

    Private Sub LoadComboBox()
        ComboBox_Titles.DataSource = TitleDAO.GetTitles
        ComboBox_Titles.ValueMember = "Title_ID"
        ComboBox_Titles.DisplayMember = "Title_Desc"
        ComboBox_Titles.SelectedIndex = -1

        ResetForm()
    End Sub

    Private Sub Button_Edit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Edit.Click
        Dim frm As Form_AddEditTitle = New Form_AddEditTitle

        frm.IsEdit = True
        frm.TitleDesc = ComboBox_Titles.Text
        frm.TitleId = ComboBox_Titles.SelectedValue
        frm.ShowDialog()

        If frm.TitleDesc.ToString <> "" Then
            Try
                TitleDAO.UpdateTitle(ComboBox_Titles.SelectedValue, frm.TitleDesc.ToString)
                LoadComboBox()
                ComboBox_Titles.SelectedValue = frm.TitleId
            Catch ex As Exception
                Logger.LogError("Exception: ", Me.GetType(), ex)
                MessageBox.Show(ex.Message)
                Dim args(1) As String
                args(0) = "Form_ManageTitles form: Button_Edit_Click"
                ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
                Me.Close()
            End Try
        End If

        frm.Dispose()
    End Sub

    Private Sub Button_Save_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Save.Click
        Dim sConflicts As String
        Dim sEmail As String
        Dim frm As New Form_RoleConflictReasons

        Try
            Cursor = Cursors.WaitCursor

            sConflicts = CheckForRoleConflicts()

            Try
                sEmail = ConfigurationServices.AppSettings("IRMASOXRoleConflictEmail")
            Catch ex As Exception
                MsgBox("Your changes will not be saved because there is no IRMA SOX Conflict e-mail address set up.  Please have the System Administrator add the IRMASOXRoleConflictEmail key to the IRMA Client and try again.", MsgBoxStyle.Critical, "IRMA SOX Role Conflict")
                Exit Sub
            End Try

            If sConflicts <> "" Then
                frm.RoleConflicts = sConflicts
                frm.ConflictType = "T"
                frm.Title = ComboBox_Titles.Text
                frm.TitleId = ComboBox_Titles.SelectedValue
                frm.ShowDialog()
            Else
                frm.ConflictRiskAccepted = True
            End If

            If frm.ConflictRiskAccepted Then
                TitleDAO.SaveTitlePermissions(ComboBox_Titles.SelectedValue, CheckBox_Accountant.Checked, CheckBox_BatchBuildOnly.Checked, _
                                              CheckBox_Buyer.Checked, CheckBox_Coordinator.Checked, CheckBox_CostAdmin.Checked, _
                                              CheckBox_FacilityCreditProcessor.Checked, CheckBox_DCAdmin.Checked, CheckBox_DeletePO.Checked, CheckBox_Einvoicing.Checked, _
                                              CheckBox_InventoryAdmin.Checked, CheckBox_ItemAdmin.Checked, CheckBox_LockAdmin.Checked, _
                                              CheckBox_POAccountant.Checked, CheckBox_POApprovalAdmin.Checked, CheckBox_POEditor.Checked, _
                                              CheckBox_PriceBatchProcessor.Checked, CheckBox_Receiver.Checked, CheckBox_VendorAdmin.Checked, _
                                              CheckBox_VendorCostDiscrepancyAdmin.Checked, CheckBox_Warehouse.Checked, _
                                              CheckBox_AppConfigAdmin.Checked, CheckBox_DataAdministrator.Checked, _
                                              CheckBox_POSInterfaceAdministrator.Checked, CheckBox_JobAdministrator.Checked, _
                                              CheckBox_StoreAdministrator.Checked, CheckBox_UserMaintenance.Checked, CheckBox_Shrink.Checked, CheckBox_ShrinkAdmin.Checked, _
                                              CheckBox_TaxAdministrator.Checked)


                Button_Save.Enabled = False
            End If


        Catch ex As Exception
            Logger.LogError("Exception: ", Me.GetType(), ex)
            MessageBox.Show(ex.Message)
            Dim args(1) As String
            args(0) = "Form_ManageTitles form: Button_Save_Click"
            ErrorHandler.ProcessError(ErrorType.POSPush_AdminError, args, SeverityLevel.Warning)
            Me.Close()
        Finally
            Cursor = Cursors.Arrow
        End Try
    End Sub

    Private Sub ComboBox_Titles_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox_Titles.SelectedIndexChanged
        If ComboBox_Titles.SelectedIndex = -1 Then
            Button_Save.Enabled = False
            Button_DeleteTitle.Enabled = False
            Button_Edit.Enabled = False
        Else
            Button_Save.Enabled = True
            Button_DeleteTitle.Enabled = True
            Button_Edit.Enabled = True

            ResetForm()
            PopulateForm()
        End If
    End Sub

    Private Sub ResetForm()
        Dim ctrl As Control

        For Each ctrl In Me.GroupBox_AdminRoles.Controls
            If TypeOf ctrl Is CheckBox Then
                CType(ctrl, CheckBox).Checked = False
            End If
        Next

        For Each ctrl In Me.GroupBox_IRMARoles.Controls
            If TypeOf ctrl Is CheckBox Then
                CType(ctrl, CheckBox).Checked = False
            End If
        Next
    End Sub

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        Me.Close()
    End Sub

    Private Sub PopulateForm()
        Dim dt As DataTable = Nothing
        Dim dr As DataRow = Nothing

        If IsNumeric(ComboBox_Titles.SelectedValue) Then
            dt = TitleDAO.GetTitlePermissions(ComboBox_Titles.SelectedValue)

            For Each dr In dt.Rows
                CheckBox_Accountant.Checked = CBool(dr.Item("Accountant"))
                CheckBox_BatchBuildOnly.Checked = CBool(dr.Item("BatchBuildOnly"))
                CheckBox_Buyer.Checked = CBool(dr.Item("Buyer"))
                CheckBox_Coordinator.Checked = CBool(dr.Item("Coordinator"))
                CheckBox_CostAdmin.Checked = CBool(dr.Item("CostAdministrator"))
                CheckBox_FacilityCreditProcessor.Checked = CBool(dr.Item("FacilityCreditProcessor"))
                CheckBox_DCAdmin.Checked = CBool(dr.Item("DCAdmin"))
                CheckBox_DeletePO.Checked = CBool(dr.Item("DeletePO"))
                CheckBox_Einvoicing.Checked = CBool(dr.Item("EInvoicing"))
                CheckBox_InventoryAdmin.Checked = CBool(dr.Item("InventoryAdministrator"))
                CheckBox_ItemAdmin.Checked = CBool(dr.Item("ItemAdministrator"))
                CheckBox_LockAdmin.Checked = CBool(dr.Item("LockAdministrator"))
                CheckBox_POAccountant.Checked = CBool(dr.Item("POAccountant"))
                CheckBox_POApprovalAdmin.Checked = CBool(dr.Item("POApprovalAdministrator"))
                CheckBox_POEditor.Checked = CBool(dr.Item("POEditor"))
                CheckBox_PriceBatchProcessor.Checked = CBool(dr.Item("PriceBatchProcessor"))
                CheckBox_Receiver.Checked = CBool(dr.Item("Distributor"))
                CheckBox_VendorAdmin.Checked = CBool(dr.Item("VendorAdministrator"))
                CheckBox_VendorCostDiscrepancyAdmin.Checked = CBool(dr.Item("VendorCostDiscrepancyAdmin"))
                CheckBox_Warehouse.Checked = CBool(dr.Item("Warehouse"))
                CheckBox_AppConfigAdmin.Checked = CBool(dr.Item("ApplicationConfigAdmin"))
                CheckBox_DataAdministrator.Checked = CBool(dr.Item("DataAdministrator"))
                CheckBox_JobAdministrator.Checked = CBool(dr.Item("JobAdministrator"))
                CheckBox_POSInterfaceAdministrator.Checked = CBool(dr.Item("POSInterfaceAdministrator"))
                CheckBox_StoreAdministrator.Checked = CBool(dr.Item("StoreAdministrator"))
                CheckBox_UserMaintenance.Checked = CBool(dr.Item("UserMaintenance"))
                CheckBox_Shrink.Checked = CBool(dr.Item("Shrink"))
                CheckBox_ShrinkAdmin.Checked = CBool(dr.Item("ShrinkAdmin"))
                CheckBox_TaxAdministrator.Checked = CBool(dr.Item("TaxAdministrator"))
            Next
        End If
    End Sub

    Private Sub EnableSave(ByVal sender As Object, ByVal e As System.EventArgs) Handles CheckBox_Accountant.CheckStateChanged, CheckBox_BatchBuildOnly.CheckStateChanged, _
                                                                                        CheckBox_Buyer.CheckStateChanged, CheckBox_Coordinator.CheckStateChanged, CheckBox_CostAdmin.CheckStateChanged, _
                                                                                        CheckBox_FacilityCreditProcessor.CheckStateChanged, CheckBox_DCAdmin.CheckStateChanged, CheckBox_DeletePO.CheckStateChanged, _
                                                                                        CheckBox_Einvoicing.CheckStateChanged, CheckBox_InventoryAdmin.CheckStateChanged, CheckBox_ItemAdmin.CheckStateChanged, CheckBox_LockAdmin.CheckStateChanged, _
                                                                                        CheckBox_POAccountant.CheckStateChanged, CheckBox_POApprovalAdmin.CheckStateChanged, CheckBox_POEditor.CheckStateChanged, _
                                                                                        CheckBox_PriceBatchProcessor.CheckStateChanged, CheckBox_Receiver.CheckStateChanged, CheckBox_VendorAdmin.CheckStateChanged, _
                                                                                        CheckBox_VendorCostDiscrepancyAdmin.CheckStateChanged, CheckBox_Warehouse.CheckStateChanged, CheckBox_AppConfigAdmin.CheckStateChanged, _
                                                                                        CheckBox_DataAdministrator.CheckStateChanged, CheckBox_JobAdministrator.CheckStateChanged, CheckBox_POSInterfaceAdministrator.CheckStateChanged, _
                                                                                        CheckBox_StoreAdministrator.CheckStateChanged, CheckBox_UserMaintenance.CheckStateChanged, CheckBox_Shrink.CheckStateChanged, CheckBox_ShrinkAdmin.CheckStateChanged, _
                                                                                        CheckBox_TaxAdministrator.CheckStateChanged
        If ComboBox_Titles.SelectedIndex <> -1 Then
            Button_Save.Enabled = True
        Else
            Button_Save.Enabled = False
        End If
    End Sub

    Private Sub Button_ManageConflicts_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button_ViewRoleConflicts.Click
        Dim frm As New Form_ManageRoleConflicts

        frm.IsReadOnly = True
        frm.ShowDialog()
    End Sub

    Private Function CheckForRoleConflicts() As String
        Dim dt As DataTable
        Dim dr As DataRow
        Dim ctrl As Control
        Dim ctrl2 As Control
        Dim blnPrimaryChecked As Boolean = False
        Dim blnConflictChecked As Boolean = False
        Dim sConflictsExist As String = ""

        dt = TitleDAO.GetRoleConflicts

        For Each dr In dt.Rows
            For Each ctrl In Me.Controls
                If TypeOf ctrl Is GroupBox Then
                    For Each ctrl2 In ctrl.Controls
                        If UCase(ctrl2.Text) = UCase(dr("Role1").ToString) Then
                            blnPrimaryChecked = CType(ctrl2, CheckBox).Checked
                        End If

                        If UCase(ctrl2.Text) = UCase(dr("Role2").ToString) Then
                            blnConflictChecked = CType(ctrl2, CheckBox).Checked
                        End If
                    Next
                End If
            Next

            If blnPrimaryChecked And blnConflictChecked Then
                sConflictsExist = sConflictsExist & dr("Role1").ToString & "^" & dr("Role2") & "|"
            End If
        Next

        sConflictsExist = CheckIfConflictAlreadyLogged(sConflictsExist)

        Return sConflictsExist
    End Function

    Private Function CheckIfConflictAlreadyLogged(ByVal sConflicts As String)
        Dim dt As DataTable
        Dim x As Integer = 0

        dt = TitleDAO.GetRoleConflictReason("T", -1, ComboBox_Titles.SelectedValue)

        For x = 0 To dt.Rows.Count - 1
            If InStr(sConflicts, dt.Rows(x)("Role1") & "^" & dt.Rows(x)("Role2")) > 0 Then
                sConflicts = Replace(sConflicts, dt.Rows(x)("Role1") & "^" & dt.Rows(x)("Role2"), "")
            End If

            If InStr(sConflicts, dt.Rows(x)("Role2") & "^" & dt.Rows(x)("Role1")) > 0 Then
                sConflicts = Replace(sConflicts, dt.Rows(x)("Role2") & "^" & dt.Rows(x)("Role1"), "")
            End If
        Next

        sConflicts = Replace(sConflicts, "||", "|")

        If Len(sConflicts) > 0 Then
            If Len(sConflicts) = 1 Then
                sConflicts = ""
            Else
                If Mid(sConflicts, 1, 1) = "|" Then sConflicts = Mid(sConflicts, 2)
            End If
        End If

        If Len(sConflicts) > 0 Then
            If Mid(sConflicts, Len(sConflicts), 1) = "|" Then sConflicts = Mid(sConflicts, 1, Len(sConflicts) - 1)
        End If

        Return sConflicts
    End Function

    Private Sub SetToolTips()
        Dim ctrl As Control
        Dim ctrl2 As Control

        For Each ctrl In Me.Controls
            If TypeOf ctrl Is GroupBox Then
                For Each ctrl2 In ctrl.Controls
                    If TypeOf ctrl2 Is CheckBox Then ttRoleDescription.SetToolTip(ctrl2, TitleDAO.GetToolTipText(ctrl2.Text))
                Next
            End If
        Next
    End Sub
End Class
