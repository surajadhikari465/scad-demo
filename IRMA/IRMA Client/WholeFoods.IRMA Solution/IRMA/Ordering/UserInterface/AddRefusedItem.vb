Option Strict Off
Option Explicit On

Imports log4net
Imports WholeFoods.IRMA.Common.DataAccess

Imports WholeFoods.IRMA.Ordering.DataAccess
Imports WholeFoods.IRMA.Ordering.BusinessLogic
Imports WholeFoods.IRMA.Administration.ReasonCodes.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.BusinessLogic
Imports WholeFoods.IRMA.ItemHosting.DataAccess


Friend Class frmAddRefusedItem
    Inherits System.Windows.Forms.Form

    Private ItemUnitsData As List(Of ItemUnitBO)
    Private DA As ItemUnitDAO = New ItemUnitDAO

    Private m_OrderHeaderId As Integer = 0

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Property OrderHeaderId() As Integer
        Get
            OrderHeaderId = m_OrderHeaderId
        End Get
        Set(ByVal value As Integer)
            m_OrderHeaderId = value
        End Set
    End Property

    Private Sub LoadReceivingDiscrepanyReasonCodes()

        Dim bo As ReasonCodeMaintenanceBO = New ReasonCodeMaintenanceBO
        Dim dt As DataTable = bo.getReasonCodeDetailsForType(enumReasonCodeType.RD.ToString)

        ucmbReasonCode.DataSource = dt
        ucmbReasonCode.DisplayMember = "ReasonCode"
        ucmbReasonCode.ValueMember = "ReasonCodeDetailID"

        Dim dr As DataRow = dt.NewRow()
        dr("ReasonCodeDetailID") = -1
        dr("ReasonCode") = ""
        dr("ReasonCodeDesc") = ""
        dr("ReasonCodeExtDesc") = ""
        dt.Rows.InsertAt(dr, 0)

        ucmbReasonCode.DisplayLayout.Bands(0).Columns("ReasonCode").Header.Caption = "Code"
        ucmbReasonCode.DisplayLayout.Bands(0).Columns("ReasonCodeDesc").Header.Caption = "Description"
        ucmbReasonCode.DisplayLayout.Bands(0).Columns("ReasonCode").Width = 50
        ucmbReasonCode.DisplayLayout.Bands(0).Columns("ReasonCodeDesc").Width = 200
        ucmbReasonCode.DisplayLayout.Bands(0).Columns("ReasonCodeDetailID").Hidden = True
        ucmbReasonCode.DisplayLayout.Bands(0).Columns("ReasonCodeExtDesc").Hidden = True

    End Sub
    
    Private Sub frmAddRefusedItem_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmAddRefusedItem_Load Entry")

        CenterForm(Me)
        LoadReceivingDiscrepanyReasonCodes()
        ucmbReasonCode.Text = "II"

        logger.Debug("frmAddRefusedItem_Load Exit")
    End Sub

    Private Sub cmdAdd_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdAdd.Click
        logger.Debug("cmdAdd_Click Entry")

        If (Trim(txtIdentifier.Text) = String.Empty And Trim(txtVIN.Text) = String.Empty) Then
            MsgBox("Please enter an identifier or VIN.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        If (Trim(txtDescription.Text) = String.Empty) Then
            MsgBox("Please enter item description.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        If (Trim(cmbUnits.Text) = String.Empty) Then
            MsgBox("Please enter item unit.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        If (CDbl(txtInvoiceCost.Value) < 0.0) Then
            MsgBox("Please enter a valid invoice cost.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
            Exit Sub
        ElseIf (CDbl(txtInvoiceCost.Value) = 0.0) Then
            If (MsgBox("Invoice cost is 0.0.  Do you want to keep this value?", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No) Then
                Exit Sub
            End If
        End If

        If (CDbl(txtInvoiceQuantity.Value) <= 0.0) Then
            MsgBox("Please enter a valid invoice quantity.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        If (ucmbReasonCode.Text = String.Empty) Then
            MsgBox("Please select a reason code.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        'Dim insertSQLString As String = "EXEC InsertOrderItemRefused " _
        '                        & m_OrderHeaderId & ", null,'" _
        '                        & Trim(txtIdentifier.Text) & "', '" _
        '                        & Trim(txtVIN.Text) & "', '" _
        '                        & Trim(txtDescription.Text) & "', '" _
        '                        & Trim(cmbUnits.Text) & "', " _
        '                        & txtInvoiceQuantity.Value & ", " _
        '                        & txtInvoiceCost.Value & ", " _
        '                        & txtInvoiceQuantity.Value & ", " _
        '                        & ucmbReasonCode.Value & ", " _
        '                        & "1, " _
        '                        & "NULL"

        'SQLExecute(insertSQLString, DAO.RecordsetOptionEnum.dbSQLPassThrough)

        OrderingDAO.InsertOrderItemRefused(m_OrderHeaderId, Trim(txtIdentifier.Text), Trim(txtVIN.Text), Trim(txtDescription.Text), Trim(cmbUnits.Text), txtInvoiceQuantity.Value, txtInvoiceCost.Value, txtInvoiceQuantity.Value, ucmbReasonCode.Value, True)

        MsgBox("The refused item has been added successfully.", MsgBoxStyle.Information, "Add Refused Item")
        cmdClear.PerformClick()

        Me.Close()

        logger.Debug("cmdAdd_Click Exit")
    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        logger.Debug("cmdExit_Click Entry")

        Me.Close()

        logger.Debug("cmdExit_Click Exit")
    End Sub

    Private Sub cmdClear_Click(sender As System.Object, e As System.EventArgs) Handles cmdClear.Click
        logger.Debug("cmdClear_Click Entry")

        txtIdentifier.Text = String.Empty
        txtVIN.Text = String.Empty
        txtDescription.Text = String.Empty
        txtInvoiceCost.Value = 0.0
        txtInvoiceQuantity.Value = 0.0

        cmbUnits.SelectedIndex = -1

        logger.Debug("cmdClear_Click Entry")
    End Sub
End Class