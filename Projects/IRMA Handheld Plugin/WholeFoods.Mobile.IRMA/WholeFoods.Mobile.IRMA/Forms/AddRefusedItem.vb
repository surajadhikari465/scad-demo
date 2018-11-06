Imports System.Windows.Forms
Imports System.Collections.Generic
Imports System.Data
Imports System.Text
Imports Microsoft.WindowsCE.Forms
Imports System.ServiceModel

Public Class AddRefusedItem

    Private serviceFault As ParsedCFFaultException = Nothing
    Private serviceCallSuccess As Boolean
    Private RefusedItemsReasonCodeList As DataTable
    Private ItemUnitList As DataTable
    Private reasonCodes As ReasonCode()
    Private mySession As Session
    Private dtsTypes As DataTable
    Private _orderHeaderID As Integer
    
    Public WriteOnly Property OrderHeaderID() As Integer
        Set(ByVal value As Integer)
            _orderHeaderID = value
        End Set
    End Property

#Region " Constructors"
    Public Sub New(ByVal session As Session)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.mySession = session
        AlignText()

    End Sub

#End Region

    Private Sub AddRefusedItem_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'populates reason codes

        Try
            ' Attempt a service call to get the refusal reason codes and UOMs.
            serviceCallSuccess = True

            PopulateReasonCodes()
            'PopulateUnits()

            ' Explicitly handle service faults, timeouts, and connection failures.  If this initialization block fails, the user will
            ' fall back to the last form she was on.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            serviceCallSuccess = False
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "AddRefusedItem_Load")
            serviceCallSuccess = False
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "AddRefusedItem_Load")
            serviceCallSuccess = False
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "AddRefusedItem_Load")
            serviceCallSuccess = False
            Me.DialogResult = Windows.Forms.DialogResult.Abort

        End Try

        If Not serviceCallSuccess Then
            ' The initialization failed.  End the method and return to the previous form.
            Exit Sub
        End If


        cmbReasonCode.BindingContext = New BindingContext()
        cmbReasonCode.DataSource = RefusedItemsReasonCodeList
        cmbReasonCode.DisplayMember = "Description"
        cmbReasonCode.ValueMember = "Code"
        cmbReasonCode.Text = "II"

        cmbUnit.BindingContext = New BindingContext()
        cmbUnit.DataSource = ItemUnitList
        cmbUnit.DisplayMember = "UnitName"
        cmbUnit.ValueMember = "UnitName"

        Cursor.Current = Cursors.Default
    End Sub

    Public Sub PopulateUnits()

        Dim dtUnits As DataTable
        Dim dr As DataRow

        Dim units As ListsItemUnit() = mySession.WebProxyClient.GetItemUnits()

        reasonCodes = mySession.WebProxyClient.GetReasonCodesByType(Enums.ReasonCodeType.RI.ToString())

        If (units.Length > 0) Then

            dtUnits = New DataTable

            dtUnits.Columns.Add(New DataColumn("UnitName"))

            For Each unit In units
                dr = dtUnits.NewRow()

                dr.Item("UnitName") = unit.Unit_Name

                dtUnits.Rows.Add(dr)
            Next

            ItemUnitList = dtUnits

        End If
    End Sub

    Public Sub PopulateReasonCodes()

        Dim dtReasonCodes As DataTable
        Dim dr As DataRow

        Dim reasonCodesResult As ListsReasonCode() = Me.mySession.WebProxyClient.GetRefusedItemsReasonCodes()
        reasonCodes = mySession.WebProxyClient.GetReasonCodesByType(Enums.ReasonCodeType.RI.ToString())

        If (reasonCodesResult.Length > 0) Then

            dtReasonCodes = New DataTable

            dtReasonCodes.Columns.Add(New DataColumn("Code"))
            dtReasonCodes.Columns.Add(New DataColumn("Description"))
            dr = dtReasonCodes.NewRow()
            dr.Item("Code") = -1
            dr.Item("Description") = ""
            dtReasonCodes.Rows.Add(dr)

            For Each reasonCode In reasonCodesResult
                dr = dtReasonCodes.NewRow()

                dr.Item("Code") = reasonCode.ReasonCodeID
                dr.Item("Description") = reasonCode.ReasonCode

                dtReasonCodes.Rows.Add(dr)
            Next

            RefusedItemsReasonCodeList = dtReasonCodes

        End If
    End Sub

    Private Sub mmuCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mmuCancel.Click
        Me.DialogResult = Windows.Forms.DialogResult.None
        Me.Close()
    End Sub

    Private Sub mmuAccept_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mmuAccept.Click
        Dim invCost As Double

        If Trim(txtIdentifier.Text) = String.Empty And Trim(txtVIN.Text) = String.Empty Then
            MsgBox("Please enter an identifier or VIN.", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        If Trim(txtDesc.Text) = String.Empty Then
            MsgBox("Please enter an item description.", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        If Trim(cmbUnit.Text) = String.Empty Then
            MsgBox("Please enter the item UOM.", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        If Not IsNumeric(Trim(txtInvoiceCost.Text)) Then
            MsgBox("Please enter a valid invoice cost.", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        Else
            invCost = CDbl(Trim(txtInvoiceCost.Text))
            If invCost < 0.0 Then
                MsgBox("Please enter a valid invoice cost.", MsgBoxStyle.Information, Me.Text)
                Exit Sub
            ElseIf invCost = 0.0 Then
                If (MsgBox("Invoice cost is 0.0.  Do you want to keep this value?", MsgBoxStyle.YesNo + MsgBoxStyle.Question, Me.Text) = MsgBoxResult.No) Then
                    Exit Sub
                End If
            End If
        End If

        If Not IsNumeric(Trim(txtInvoiceQuantity.Text)) Then
            MsgBox("Please enter a valid invoice quantity.", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        If Trim(cmbReasonCode.Text) = String.Empty Then
            MsgBox("Please select a reason code.", MsgBoxStyle.Information, Me.Text)
            Exit Sub
        End If

        Try
            ' Attempt a service call to insert a refused item.
            serviceCallSuccess = True

            Dim result As WholeFoods.Mobile.IRMA.Result = Me.mySession.WebProxyClient.InsertOrderItemRefused( _
                                                            _orderHeaderID, _
                                                            0, _
                                                            Trim(txtIdentifier.Text), _
                                                            Trim(txtVIN.Text), _
                                                            Trim(txtDesc.Text), _
                                                            Trim(cmbUnit.Text), _
                                                            CDec(Trim(txtInvoiceQuantity.Text)), _
                                                            CDec(Trim(txtInvoiceCost.Text)), _
                                                            cmbReasonCode.SelectedValue)

            ' Explicitly handle service faults, timeouts, and connection failures.  If this call fails, allow the user to retry.
        Catch ex As Microsoft.Tools.ServiceModel.CFFaultException
            serviceFault = New ParsedCFFaultException(ex.FaultMessage)
            Dim err As New ErrorHandler(serviceFault)
            err.ShowErrorNotification()
            serviceCallSuccess = False

        Catch ex As TimeoutException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.TIMEOUT_EXCEPTION, "mmuAccept_Click")
            serviceCallSuccess = False

        Catch ex As CommunicationException
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(Messages.COMM_EXCEPTION, "mmuAccept_Click")
            serviceCallSuccess = False

        Catch ex As Exception
            Dim err As New ErrorHandler()
            err.DisplayErrorMessage(ex.Message, "mmuAccept_Click")
            serviceCallSuccess = False

        End Try

        If Not serviceCallSuccess Then
            ' The call to InsertOrderItemRefused failed.  End the method and allow the user to retry.
            Exit Sub
        End If

        Me.DialogResult = Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub LinkLabelReasonCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles LinkLabelReasonCode.Click
        Dim reasonCodeDescription = New ReasonCodeDescription(reasonCodes)
        reasonCodeDescription.ShowDialog()
    End Sub

    Private Sub AlignText()
        lblIdentifier.TextAlign = ContentAlignment.TopRight
        lblVIN.TextAlign = ContentAlignment.TopRight
        lblDesc.TextAlign = ContentAlignment.TopRight
        lblUnit.TextAlign = ContentAlignment.TopRight
        lblInvCost.TextAlign = ContentAlignment.TopRight
        lblInvQty.TextAlign = ContentAlignment.TopRight
        LinkLabelReasonCode.TextAlign = ContentAlignment.TopRight
    End Sub

End Class