Option Strict Off
Option Explicit On

Imports System.Text
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.IRMA.TaxHosting.BusinessLogic
Imports WholeFoods.IRMA.TaxHosting.DataAccess
Imports log4net


Friend Class frmStore
    Inherits System.Windows.Forms.Form

    Public m_lStore_No As Integer
    Private _originalStoreJurisdictionID As Integer
    Private _hasChanges As Boolean
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


    Public Property Store_No() As Integer
        Get
            Return m_lStore_No
        End Get
        Set(ByVal Value As Integer)
            m_lStore_No = Value
        End Set
    End Property

    Private Sub frmStore_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        logger.Debug("frmStore_Load Entry")

        'bind data to drop downs
        BindData()

        'fill in existing data for current store
        FillFormData()

        If Not IsDBNull(glStore_Limit) Then SetActive(cmdSelStore, False)

        SetControls()

        CenterForm(Me)
        _hasChanges = False

        logger.Debug("frmStore_Load Exit")

    End Sub

    ''' <summary>
    ''' get data from database for current store and set controls on form w/ data
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub FillFormData()

        logger.Debug("FillFormData Entry")

        Dim rsStoreData As DAO.Recordset = Nothing

        Try
            rsStoreData = SQLOpenRecordSet("EXEC GetStore " & m_lStore_No, DAO.RecordsetTypeEnum.dbOpenSnapshot, DAO.RecordsetOptionEnum.dbSQLPassThrough + DAO.RecordsetOptionEnum.dbForwardOnly)
            txtName.Text = rsStoreData.Fields("Store_Name").Value
            txtAbbr.Text = IIf(IsDBNull(rsStoreData.Fields("StoreAbbr").Value), "", rsStoreData.Fields("StoreAbbr").Value)
            txtPSIStoreNo.Text = IIf(IsDBNull(rsStoreData.Fields("PSI_Store_No").Value), "", rsStoreData.Fields("PSI_Store_No").Value)
            txtPhone.Text = IIf(IsDBNull(rsStoreData.Fields("Phone_Number").Value), "", rsStoreData.Fields("Phone_Number").Value)
            cmbZone.SelectedValue = IIf(IsDBNull(rsStoreData.Fields("Zone_ID").Value), -1, rsStoreData.Fields("Zone_ID").Value)
            txtBusinessUnit.Text = IIf(IsDBNull(rsStoreData.Fields("BusinessUnit_ID").Value), "", rsStoreData.Fields("BusinessUnit_ID").Value)
            txtUNFI.Text = IIf(IsDBNull(rsStoreData.Fields("UNFI_Store").Value), "", rsStoreData.Fields("UNFI_Store").Value)
            Me.txtEXEWarehouse.Text = IIf(IsDBNull(rsStoreData.Fields("EXEWarehouse").Value), "", rsStoreData.Fields("EXEWarehouse").Value)
            ComboBox_TaxJurisdiction.SelectedValue = IIf(IsDBNull(rsStoreData.Fields("TaxJurisdictionID").Value), -1, rsStoreData.Fields("TaxJurisdictionID").Value)
            Me.chkInternal.CheckState = System.Math.Abs(CShort(rsStoreData.Fields("Internal").Value))
            Me.chkRegional.CheckState = System.Math.Abs(CShort(rsStoreData.Fields("Regional").Value))
            Select Case True
                Case Is = rsStoreData.Fields("Mega_Store").Value '365 store
                    Me.optType(0).Checked = True
                Case Is = rsStoreData.Fields("WFM_Store").Value
                    Me.optType(1).Checked = True
                Case Is = rsStoreData.Fields("Distribution_Center").Value
                    Me.optType(2).Checked = True
                Case Is = rsStoreData.Fields("Manufacturer").Value
                    Me.optType(3).Checked = True
            End Select

            ' There is validation logic that only runs if the store jurisdiction ID changes, so we must keep track
            ' of the original value.
            ComboBox_StoreJurisdiction.SelectedValue = IIf(IsDBNull(rsStoreData.Fields("StoreJurisdictionID").Value), -1, rsStoreData.Fields("StoreJurisdictionID").Value)
            _originalStoreJurisdictionID = CInt(ComboBox_StoreJurisdiction.SelectedValue)

            Me.txtGeoCode.Text = IIf(IsDBNull(rsStoreData.Fields("GeoCode").Value), "", rsStoreData.Fields("GeoCode").Value)
            Me.TextBox_ScaleStoreNo.Text = IIf(IsDBNull(rsStoreData.Fields("PLUMStoreNo").Value), "", rsStoreData.Fields("PLUMStoreNo").Value)
        Finally
            If rsStoreData IsNot Nothing Then
                rsStoreData.Close()
                rsStoreData = Nothing
            End If
        End Try

        logger.Debug("FillFormData Exit")
    End Sub

    Private Sub frmStore_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing

        logger.Debug("frmStore_FormClosing Entry")

        If _hasChanges Then
            'prompt the user to save changes
            Dim result As DialogResult = MessageBox.Show(ResourcesCommon.GetString("msg_confirmSaveData"), Me.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question)

            If result = Windows.Forms.DialogResult.Yes Then
                If ApplyChanges() Then
                    'close form
                    Me.Close()
                Else
                    e.Cancel = True
                End If
            End If
        End If

        logger.Debug("frmStore_FormClosing Exit")
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")
        Me.Close()
        logger.Debug("cmdExit_Click Exit")
    End Sub

    Private Sub cmdItems_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdItems.Click

        logger.Debug("cmdItems_Click Entry")

        Dim fSIV As frmStoreItemVendors

        frmItemSearch.ShowDialog()
        If frmItemSearch.SelectedItems.Count > 0 Then
            fSIV = New frmStoreItemVendors(frmItemSearch.SelectedItems.Item(0).Item_Key, m_lStore_No, (Me.txtName).Text, (frmItemSearch.SelectedItems.Item(0).ItemDescription), (frmItemSearch.SelectedItems.Item(0).ItemIdentifier))
            If Not (fSIV Is Nothing) Then
                fSIV.ShowDialog()
                fSIV.Close()
                fSIV.Dispose()
            End If
        End If
        frmItemSearch.Close()

        logger.Debug("cmdItems_Click Exit")

    End Sub

    Private Sub cmdSelStore_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSelStore.Click
        logger.Debug("cmdSelStore_Click Entry")

        Dim lStore_No As Integer
        Dim fSelStore As frmSelStore
        fSelStore = New frmSelStore(False)
        fSelStore.ShowDialog()

        lStore_No = fSelStore.Store_No
        fSelStore.Close()
        fSelStore.Dispose()

        If lStore_No > 0 Then Me.Store_No = lStore_No

        logger.Debug("cmdSelStore_Click Exit")

    End Sub

    ''' <summary>
    ''' bind data to form elements that need data bound to them
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub BindData()

        logger.Debug("BindData Entry")

        'bind data to zone drop down box
        cmbZone.DataSource = ZoneDAO.GetZoneList
        cmbZone.DisplayMember = "ZoneName"
        cmbZone.ValueMember = "ZoneID"

        'bind data to store jurisdiction drop down box
        Dim storeJurisdiction As New StoreJurisdictionDAO
        ComboBox_StoreJurisdiction.DataSource = storeJurisdiction.GetJurisdictionList
        ComboBox_StoreJurisdiction.DisplayMember = "StoreJurisdictionDesc"
        ComboBox_StoreJurisdiction.ValueMember = "StoreJurisdictionID"

        'bind data to tax jurisdiction drop down box
        Dim taxJurisdiction As New TaxJurisdictionDAO
        ComboBox_TaxJurisdiction.DataSource = taxJurisdiction.GetJurisdictionList
        ComboBox_TaxJurisdiction.DisplayMember = "TaxJurisdictionDesc"
        ComboBox_TaxJurisdiction.ValueMember = "TaxJurisdictionID"

        logger.Debug("BindData Exit")

    End Sub

    ''' <summary>
    ''' validate the data and save the changes if it is valid
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ApplyChanges() As Boolean

        logger.Debug("ApplyChanges Entry")

        Dim success As Boolean = False
        Dim storeData As New StoreBO
        Dim storeDAO As New StoreDAO
        Dim statusList As ArrayList
        Dim statusEnum As IEnumerator
        Dim message As New StringBuilder
        Dim currentStatus As StoreStatus

        ' Populate the store business object with the user data
        storeData.StoreNo = Me.Store_No
        storeData.StoreName = txtName.Text
        'storeData.PSIStoreNo = IIf(txtPSIStoreNo.Text = "", vbNull, txtPSIStoreNo.Text)
        storeData.StoreAbbr = txtAbbr.Text
        storeData.PhoneNumber = txtPhone.Text
        storeData.ZoneID = cmbZone.SelectedValue
        storeData.BusinessUnitID = txtBusinessUnit.Text
        storeData.UNFIStore = txtUNFI.Text
        storeData.OriginalStoreJurisdictionID = _originalStoreJurisdictionID
        storeData.UpdatedStoreJurisdictionID = ComboBox_StoreJurisdiction.SelectedValue
        storeData.TaxJurisdictionID = ComboBox_TaxJurisdiction.SelectedValue
        storeData.IsInternal = chkInternal.CheckState
        storeData.IsRegional = chkRegional.CheckState
        storeData.PSIStoreNo = IIf(txtPSIStoreNo.Text = "", 0, txtPSIStoreNo.Text)
        storeData.PLUMStoreNo = IIf(TextBox_ScaleStoreNo.Text = "", -1, CInt(TextBox_ScaleStoreNo.Text))

        Select Case True
            Case Is = optType(0).Checked '365 store
                storeData.IsMegaStore = True
            Case Is = optType(1).Checked
                storeData.IsWFMStore = True
            Case Is = optType(2).Checked
                storeData.IsDistributionCenter = True
            Case Is = optType(3).Checked
                storeData.IsManufacturer = True
        End Select

        storeData.GeoCode = Trim(txtGeoCode.Text)

        ' Validate the store data before saving the changes
        statusList = storeData.ValidateData(txtEXEWarehouse.Text)

        'loop through possible validation errors and build message string containing all errors
        statusEnum = statusList.GetEnumerator
        While statusEnum.MoveNext
            currentStatus = CType(statusEnum.Current, StoreStatus)
            Select Case currentStatus
                Case StoreStatus.Error_InvalidEXEWarehouse
                    message.Append(String.Format(ResourcesItemHosting.GetString("msg_warning_StoreExeWarehouseInvalid"), _lblLabel_5.Text))
                    message.Append(Environment.NewLine)
                Case StoreStatus.Error_StoreJurisdictionChangeAndBatchExists
                    message.Append(ResourcesItemHosting.GetString("msg_error_ChgStoreJurisdictionWhenBatchPending"))
                    message.Append(Environment.NewLine)
            End Select
        End While

        If storeDAO.CheckPLUMStoreNoExists(storeData.StoreNo, storeData.PLUMStoreNo) Then
            message.Append("PLUM Store No already exists in the database.")
            message.Append(Environment.NewLine)
        End If

        If message.Length <= 0 Then
            'save data to database
            storeDAO.UpdateStore(storeData)
            success = True
            _hasChanges = False
        Else
            'display error msg
            MessageBox.Show(message.ToString, Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning)
            logger.Info(message.ToString)
        End If

        logger.Debug("ApplyChanges Exit")
        Return success
    End Function

    ''' <summary>
    ''' apply changes to database
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub cmdSelect_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSelect.Click
        logger.Debug("cmdSelect_Click Entry")
        ApplyChanges()
        logger.Debug("cmdSelect_Click Exit")
    End Sub

    Private Sub AdjustWidthComboBox_DropDown(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox_TaxJurisdiction.DropDown

        logger.Debug("AdjustWidthComboBox_DropDown Entry")

        Dim senderComboBox As ComboBox = CType(sender, ComboBox)
        Dim width As Integer = senderComboBox.DropDownWidth
        Dim g As Graphics = senderComboBox.CreateGraphics()
        Dim font As Font = senderComboBox.Font
        Dim vertScrollBarWidth As Integer = CType(IIf((senderComboBox.Items.Count > senderComboBox.MaxDropDownItems), SystemInformation.VerticalScrollBarWidth, 0), Integer)

        Dim newWidth As Integer
        Dim itemEnum As IEnumerator = CType(sender, ComboBox).Items.GetEnumerator
        Dim currentItem As String

        While itemEnum.MoveNext
            currentItem = CType(itemEnum.Current, TaxJurisdictionBO).TaxJurisdictionDesc
            newWidth = CType(g.MeasureString(currentItem, font).Width, Integer) + vertScrollBarWidth

            If width < newWidth Then
                width = newWidth
            End If
        End While

        senderComboBox.DropDownWidth = width

        logger.Debug("AdjustWidthComboBox_DropDown Exit")

    End Sub

    Private Sub Button_StoreSubteam_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_StoreSubTeam.Click

        logger.Debug("Button_StoreSubteam_Click Entry")

        Dim frm As StoreSubTeam = New StoreSubTeam(m_lStore_No)
        frm.ShowDialog(Me.Parent)
        frm.Dispose()

        logger.Debug("Button_StoreSubteam_Click Exit")


    End Sub

    Private Sub _lblPSIStore_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lblPSIStore.Click

    End Sub

    Private Sub txtBusinessUnit_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtBusinessUnit.TextChanged

    End Sub

    Private Sub txtPSIStoreNo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPSIStoreNo.TextChanged

        logger.Debug("txtPSIStoreNo_TextChanged Entry")

        If Not (IsNumeric(txtPSIStoreNo.Text) Or (txtPSIStoreNo.Text = "")) Then
            MsgBox(ResourcesItemHosting.GetString("InvalidPsiStoreNo"), MsgBoxStyle.Critical, Me.Text)

            logger.Info(ResourcesItemHosting.GetString("InvalidPsiStoreNo"))
            txtPSIStoreNo.Clear()
            txtPSIStoreNo.Focus()
            logger.Debug("txtPSIStoreNo_TextChanged Exit")
            Exit Sub
        End If

        logger.Debug("txtPSIStoreNo_TextChanged Exit")

    End Sub

    Private Sub txtName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtName.TextChanged
        _hasChanges = True
    End Sub

    Private Sub txtAbbr_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtAbbr.TextChanged
        _hasChanges = True
    End Sub

    Private Sub txtPhone_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPhone.TextChanged
        _hasChanges = True
    End Sub

    Private Sub cmbZone_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmbZone.SelectedIndexChanged
        _hasChanges = True
    End Sub

    Private Sub txtUNFI_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtUNFI.TextChanged
        _hasChanges = True
    End Sub

    Private Sub txtEXEWarehouse_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtEXEWarehouse.TextChanged
        _hasChanges = True
    End Sub

    Private Sub ComboBox_StoreJurisdiction_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_StoreJurisdiction.SelectedIndexChanged
        _hasChanges = True
    End Sub

    Private Sub ComboBox_TaxJurisdiction_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ComboBox_TaxJurisdiction.SelectedIndexChanged
        _hasChanges = True
    End Sub

    Private Sub _optType_0_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _optType_0.CheckedChanged
        _hasChanges = True
    End Sub

    Private Sub _optType_1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _optType_1.CheckedChanged
        _hasChanges = True
    End Sub

    Private Sub _optType_2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _optType_2.CheckedChanged
        _hasChanges = True
    End Sub

    Private Sub _optType_3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _optType_3.CheckedChanged
        _hasChanges = True
    End Sub

    Private Sub chkInternal_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkInternal.CheckedChanged
        _hasChanges = True
    End Sub

    Private Sub chkRegional_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkRegional.CheckedChanged
        _hasChanges = True
    End Sub

    Private Sub txtGeoCode_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtGeoCode.TextChanged
        _hasChanges = True
    End Sub

    Private Sub SetControls()
        SetActive(txtName, gbApplicationConfigurationAdministrator Or Not gbTaxAdministrator)
        SetActive(txtAbbr, gbApplicationConfigurationAdministrator Or Not gbTaxAdministrator)
        SetActive(txtPSIStoreNo, gbApplicationConfigurationAdministrator Or Not gbTaxAdministrator)
        SetActive(txtPhone, gbApplicationConfigurationAdministrator Or Not gbTaxAdministrator)
        SetActive(chkInternal, gbApplicationConfigurationAdministrator Or Not gbTaxAdministrator)
        SetActive(chkRegional, gbApplicationConfigurationAdministrator Or Not gbTaxAdministrator)
        SetActive(cmbZone, gbApplicationConfigurationAdministrator Or Not gbTaxAdministrator)
        SetActive(txtBusinessUnit, gbApplicationConfigurationAdministrator Or Not gbTaxAdministrator)
        SetActive(txtUNFI, gbApplicationConfigurationAdministrator Or Not gbTaxAdministrator)
        SetActive(txtEXEWarehouse, gbApplicationConfigurationAdministrator Or Not gbTaxAdministrator)
        SetActive(ComboBox_StoreJurisdiction, InstanceDataDAO.IsFlagActive("UseStoreJurisdictions") And (gbApplicationConfigurationAdministrator Or Not gbTaxAdministrator))
        SetActive(Button_StoreSubTeam, gbApplicationConfigurationAdministrator Or Not gbTaxAdministrator)
        SetActive(fraType, gbApplicationConfigurationAdministrator Or Not gbTaxAdministrator)
        SetActive(cmdItems, gbApplicationConfigurationAdministrator Or Not gbTaxAdministrator)
    End Sub
End Class