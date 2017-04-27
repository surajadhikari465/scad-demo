Imports System.Text
Imports WholeFoods.IRMA.Administration.POSPush.DataAccess
Imports WholeFoods.IRMA.Administration.StoreAdmin.DataAccess
Imports WholeFoods.IRMA.ItemHosting.DataAccess

Public Class StoreAdd

#Region "Properties"
    Private StoreSubTeamSubstitution As SubTeamSubstitutionList
    Private mStoreAdd As StoreAddBO
    Private WithEvents _BackgroundWorker As System.ComponentModel.BackgroundWorker = New System.ComponentModel.BackgroundWorker()
    Private Property BackgroundWorker1 As System.ComponentModel.BackgroundWorker
#End Region

#Region "Subs and Functions"
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Call InitButtons()

    End Sub

    Private Sub InitButtons()

        Me.Text = "Create New Store"

    End Sub

    Private Sub BindcboSourceStore()
        Me.cboSourceStore.DataSource = StoreAddDAO.GetSourceStoreList(CInt(Me.cboPOSWriter.SelectedValue))
        Me.cboSourceStore.DisplayMember = "Store_Name"
        Me.cboSourceStore.ValueMember = "Store_No"
    End Sub
    Private Sub BindcboPOSSubTeam()
        Me.cboPOSSubTeam.DataSource = StoreAddDAO.GetPOSSubteamList(CInt(Me.cboPOSWriter.SelectedValue))
        Me.cboPOSSubTeam.DisplayMember = "Subteam_Name"
        Me.cboPOSSubTeam.ValueMember = "Subteam_No"
    End Sub
    Private Sub BindcboSubStore()
        '20110224 - DBS - added these checks
        If TypeOf cboSourceStore.SelectedValue Is DataRowView Then
            Exit Sub
        End If
        If TypeOf cboPOSSubTeam.SelectedValue Is DataRowView Then
            Exit Sub
        End If
        If TypeOf cboPOSWriter.SelectedValue Is DataRowView Then
            Exit Sub
        End If
        Me.cboSubStore.DataSource = StoreAddDAO.GetSubStoreList(CInt(cboSourceStore.SelectedValue), CInt(cboPOSSubTeam.SelectedValue), CInt(cboPOSWriter.SelectedValue))
        Me.cboSubStore.DisplayMember = "Store_Name"
        Me.cboSubStore.ValueMember = "Store_No"
    End Sub
    Private Sub BindSubStoreGrid()
        dgvSubStoreSubteam.DataSource = Nothing
        Me.SubTeamSubstitutionListBindingSource.DataSource = StoreSubTeamSubstitution
        Me.dgvSubStoreSubteam.DataSource = SubTeamSubstitutionListBindingSource
    End Sub
    Private Function ValidateInput() As String
        Dim stMessage As New StringBuilder

        If txtStoreNumber.Text = "" Then
            stMessage.Append("* Store Number Required").AppendLine()
        Else
            If Not Integer.TryParse(txtStoreNumber.Text, 0) Then
                stMessage.Append("* Store Number must be Integer").AppendLine()
            End If
        End If

        If txtStoreAbbrev.Text = "" Then
            stMessage.Append("* Store Abbreviation Required").AppendLine()
        End If

        If txtStoreName.Text = "" Then
            stMessage.Append("* Store Name Required").AppendLine()
        End If

        If txtBusinessUnitID.Text = "" Then
            stMessage.Append("* Business Unit ID Required").AppendLine()
        ElseIf Not Integer.TryParse(txtBusinessUnitID.Text, 0) Then
            stMessage.Append("* Business Unit ID must be Integer").AppendLine()
        Else
            Dim existingStore As String
            existingStore = StoreAddDAO.CheckDuplicateBusinessUnitID(CInt(txtBusinessUnitID.Text))
            If existingStore <> "" Then
                stMessage.Append("* Business Unit ID must be Unique. The chosen Business Unit ID has already been used by " + existingStore).AppendLine()
            End If
        End If

        If txtVendorName.Text = "" Then
            stMessage.Append("* Vendor Name Required").AppendLine()
        End If

        If txtVendorAddress.Text = "" Then
            stMessage.Append("* Vendor Address Required").AppendLine()
        End If

        If txtVendorCity.Text = "" Then
            stMessage.Append("* Vendor City Required").AppendLine()
        End If

        If txtVendorState.Text = "" Then
            stMessage.Append("* Vendor State Required").AppendLine()
        End If
        Return stMessage.ToString

        If txtVendorZip.Text = "" Then
            stMessage.Append("* Vendor Zip Code Required").AppendLine()
        End If

        If txtPlumStoreNo.Text <> "" And Integer.TryParse(txtPlumStoreNo.Text, 0) Then
            stMessage.Append("* Plum Store Number must be Integer").AppendLine()
        End If

        If txtPSIStoreNo.Text <> "" And Integer.TryParse(txtPSIStoreNo.Text, 0) Then
            stMessage.Append("* PSI Store Number must be Integer").AppendLine()
        End If

        If txtPeopleSoftID.Text <> "" And Integer.TryParse(txtPeopleSoftID.Text, 0) Then
            stMessage.Append("* PeopleSoft ID must be Integer").AppendLine()
        End If

    End Function
    Private Function Save(ByVal rebind As Boolean) As Boolean
        Try
            mStoreAdd.Save()
        Catch ex As Exception
            MsgBox(ex.ToString, MsgBoxStyle.Critical)
        End Try
        Return True
    End Function
#End Region

#Region "Event Handlers"
    Private Sub frmStoreAdd_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim sjDAO As New StoreJurisdictionDAO
        Me.cboStoreJurisdiction.DataSource = sjDAO.GetJurisdictionList()
        Me.cboStoreJurisdiction.DisplayMember = "StoreJurisdictionDesc"
        Me.cboStoreJurisdiction.ValueMember = "StoreJurisdictionID"

        Dim tjDAO As New TaxJurisdictionAdminDAO
        Me.cboTaxJurisdiction.DataSource = tjDAO.GetJurisdictionList
        Me.cboTaxJurisdiction.DisplayMember = "TaxJurisdictionDesc"
        Me.cboTaxJurisdiction.ValueMember = "TaxJurisdictionID"

        Me.cboZone.DataSource = ZoneDAO.GetZones
        Me.cboZone.DisplayMember = "Zone_Name"
        Me.cboZone.ValueMember = "Zone_ID"

        Me.cboPOSWriter.DataSource = POSWriterDAO.GetFileWriters("POS").Tables(0)
        Me.cboPOSWriter.DisplayMember = "POSFileWriterCode"
        Me.cboPOSWriter.ValueMember = "POSFileWriterKey"

        Me.cboISSPriceChgType.DataSource = StoreAddDAO.GetSalePriceChgTypeList
        Me.cboISSPriceChgType.DisplayMember = "pricechgtypedesc"
        Me.cboISSPriceChgType.ValueMember = "pricechgtypeid"

        BindcboSourceStore()
        BindcboPOSSubTeam()
        BindcboSubStore()

        Dim index As Integer
        index = Me.cboISSPriceChgType.FindString("ISS")
        Me.cboISSPriceChgType.SelectedIndex = index

    End Sub
    Private Sub btnCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub cboPOSWriter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPOSWriter.SelectedIndexChanged

        If TypeOf cboPOSWriter.SelectedValue Is DataRowView Then
            Exit Sub
        End If

        BindcboSourceStore()
        BindcboPOSSubTeam()
        BindcboSubStore()

    End Sub
    Private Sub cboSourceStore_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSourceStore.SelectedIndexChanged

        If TypeOf cboPOSWriter.SelectedValue Is DataRowView Then
            Exit Sub
        End If
        BindcboSubStore()

    End Sub
    Private Sub cboPOSSubTeam_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPOSSubTeam.SelectedIndexChanged

        If TypeOf cboPOSWriter.SelectedValue Is DataRowView Then
            Exit Sub
        End If
        BindcboSubStore()

    End Sub

    Private Sub btnAddSubStore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddSubStore.Click

        If IsNothing(StoreSubTeamSubstitution) Then
            StoreSubTeamSubstitution = SubTeamSubstitutionList.NewList
        End If

        Dim subStore As New SubStoreInfo
        subStore.SubStoreID = cboSubStore.SelectedValue
        subStore.SubStoreName = cboSubStore.Text

        Dim posSubteam As New POSSubteamInfo
        posSubteam.POSSubteamID = cboPOSSubTeam.SelectedValue
        posSubteam.POSSubteamName = cboPOSSubTeam.Text

        StoreSubTeamSubstitution.Add(SubTeamSubstitutionInfo.NewInfo(subStore, posSubteam))
        BindSubStoreGrid()

    End Sub

    Private Sub btnRemoveStoreSub_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRemoveStoreSub.Click

        For Each item As DataGridViewRow In dgvSubStoreSubteam.SelectedRows
            Dim remItem As New SubTeamSubstitutionInfo
            With remItem
                .SubSubTeamID = item.Cells(0).Value
                .SubSubTeamName = item.Cells(1).Value
                .SubStoreID = item.Cells(2).Value
                .SubStoreName = item.Cells(3).Value
            End With
            StoreSubTeamSubstitution.Remove(remItem)
            BindSubStoreGrid()
        Next

    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click

        Dim stValid As String
        stValid = ValidateInput()
        If stValid <> "" Then
            MsgBox(stValid)
            Exit Sub
        End If

        Dim varStoreSubTeamSubstitutions As String
        If IsNothing(StoreSubTeamSubstitution) Then
            varStoreSubTeamSubstitutions = ""
        Else
            varStoreSubTeamSubstitutions = StoreSubTeamSubstitution.ToString
        End If

        Dim varPSIStoreNo As Integer
        If Len(Me.txtPSIStoreNo.Text) > 0 Then
            varPSIStoreNo = CInt(Me.txtPSIStoreNo.Text)
        End If

        Dim varPlumStoreNo As Integer
        If Len(Me.txtPlumStoreNo.Text) > 0 Then
            varPlumStoreNo = CInt(Me.txtPlumStoreNo.Text)
        End If


        Dim varSourceStoreNo As Integer
        If Len(Me.cboSourceStore.SelectedValue) > 0 Then
            varSourceStoreNo = CInt(Me.cboSourceStore.SelectedValue)
        End If

        Dim varStoreNumber As Integer
        If Len(Me.txtStoreNumber.Text) > 0 Then
            varStoreNumber = CInt(Me.txtStoreNumber.Text)
        End If

        Dim varBusinessUnitID As Integer
        If Len(Me.txtBusinessUnitID.Text) > 0 Then
            varBusinessUnitID = CInt(Me.txtBusinessUnitID.Text)
        End If

        Dim varPeopleSoftID As Integer
        If Len(Me.txtPeopleSoftID.Text) > 0 Then
            varPeopleSoftID = CInt(Me.txtPeopleSoftID.Text)
        End If

        mStoreAdd = StoreAddBO.NewStore(varStoreNumber, Me.txtStoreAbbrev.Text, Me.txtStoreName.Text, CInt(Me.cboStoreJurisdiction.SelectedValue),
        CInt(Me.cboZone.SelectedValue), CInt(Me.cboTaxJurisdiction.SelectedValue), varBusinessUnitID, varPlumStoreNo,
        varPSIStoreNo, varSourceStoreNo, CInt(Me.cboISSPriceChgType.SelectedValue), varStoreSubTeamSubstitutions,
        Me.txtVendorName.Text, Me.txtVendorAddress.Text, Me.txtVendorCity.Text, Me.txtVendorState.Text, Me.txtVendorZip.Text, varPeopleSoftID,
        CByte(Me.chkIncSlim.Checked), CByte(Me.chkIncFutureSale.Checked), CByte(Me.chkIncPromoPlanner.Checked), txtGeoCode.Text)

        btnRemoveStoreSub.Enabled = False
        btnAddSubStore.Enabled = False
        btnCancel.Enabled = False
        btnOK.Enabled = False
        _BackgroundWorker.WorkerReportsProgress = True
        _BackgroundWorker.WorkerSupportsCancellation = True
        _BackgroundWorker.RunWorkerAsync()
        Save(False)
        Me.Close()
    End Sub
    Private Sub txtStoreNumber_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtStoreNumber.Validating
        If txtStoreNumber.Text = "" Then
            ErrorProvider1.SetError(txtStoreNumber, "Required")
        Else
            If Not Integer.TryParse(txtStoreNumber.Text, 0) Then
                ErrorProvider1.SetError(txtStoreNumber, "Must be integer.")
            Else
                ErrorProvider1.SetError(txtStoreNumber, "")
            End If
        End If
    End Sub

    Private Sub txtStoreAbbrev_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtStoreAbbrev.Validating
        If txtStoreAbbrev.Text = "" Then
            ErrorProvider1.SetError(txtStoreAbbrev, "Required")
        Else
            ErrorProvider1.SetError(txtStoreAbbrev, "")
        End If
    End Sub

    Private Sub txtStoreName_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtStoreName.Validating
        If txtStoreName.Text = "" Then
            ErrorProvider1.SetError(txtStoreName, "Required")
        Else
            ErrorProvider1.SetError(txtStoreName, "")
        End If
    End Sub

    Private Sub txtBusinessUnitID_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtBusinessUnitID.Validating
        If txtBusinessUnitID.Text = "" Then
            ErrorProvider1.SetError(txtBusinessUnitID, "Required")
        Else
            If Not Integer.TryParse(txtBusinessUnitID.Text, 0) Then
                ErrorProvider1.SetError(txtBusinessUnitID, "Must be integer.")
            Else
                ErrorProvider1.SetError(txtBusinessUnitID, "")
            End If
        End If
    End Sub

    Private Sub txtVendorName_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtVendorName.Validating
        If txtVendorName.Text = "" Then
            ErrorProvider1.SetError(txtVendorName, "Required")
        Else
            ErrorProvider1.SetError(txtVendorName, "")
        End If
    End Sub

    Private Sub txtVendorAddress_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtVendorAddress.Validating
        If txtVendorAddress.Text = "" Then
            ErrorProvider1.SetError(txtVendorAddress, "Required")
        Else
            ErrorProvider1.SetError(txtVendorAddress, "")
        End If
    End Sub

    Private Sub txtVendorCity_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtVendorCity.Validating
        If txtVendorCity.Text = "" Then
            ErrorProvider1.SetError(txtVendorCity, "Required")
        Else
            ErrorProvider1.SetError(txtVendorCity, "")
        End If
    End Sub

    Private Sub txtVendorState_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtVendorState.Validating
        If txtVendorState.Text = "" Then
            ErrorProvider1.SetError(txtVendorState, "Required")
        Else
            ErrorProvider1.SetError(txtVendorState, "")
        End If
    End Sub

    Private Sub txtVendorZip_Validating(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtVendorZip.Validating
        If txtVendorZip.Text = "" Then
            ErrorProvider1.SetError(txtVendorZip, "Required")
        Else
            ErrorProvider1.SetError(txtVendorZip, "")
        End If
    End Sub
#End Region

End Class