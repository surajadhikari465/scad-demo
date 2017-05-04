Option Strict Off
Option Explicit On
Imports log4net
Imports WholeFoods.IRMA.Common.DataAccess
Imports WholeFoods.Utility

Friend Class frmOrderSend
    Inherits System.Windows.Forms.Form

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Dim mbFax As Boolean
    Dim mbEmail As Boolean
    Dim mbManual As Boolean
    Dim mbElectronic As Boolean
    Dim msFaxNbr As String
    Dim msEmailAddr As String
    Dim mbSend As Boolean

    Public Property Fax() As Boolean
        Get
            Fax = mbFax
        End Get
        Set(ByVal Value As Boolean)
            mbFax = Value
            If Value Then
                Me._optPOTrans_0.Checked = System.Windows.Forms.CheckState.Checked
            Else
                Me._optPOTrans_0.Checked = System.Windows.Forms.CheckState.Unchecked
            End If
        End Set
    End Property

    Public Property Email() As Boolean
        Get
            Email = mbEmail
        End Get
        Set(ByVal Value As Boolean)
            mbEmail = Value
            If Value Then
                Me._optPOTrans_1.Checked = System.Windows.Forms.CheckState.Checked
            Else
                Me._optPOTrans_1.Checked = System.Windows.Forms.CheckState.Unchecked
            End If
        End Set
    End Property

    Public Property Manual() As Boolean
        Get
            Manual = mbManual
        End Get
        Set(ByVal Value As Boolean)
            mbManual = Value
            If Value Then
                Me._optPOTrans_2.Checked = System.Windows.Forms.CheckState.Checked
            Else
                Me._optPOTrans_2.Checked = System.Windows.Forms.CheckState.Unchecked
            End If
        End Set
    End Property

    Public Property Electronic() As Boolean
        Get
            Electronic = mbElectronic
        End Get
        Set(ByVal Value As Boolean)
            mbElectronic = Value
            If Value Then
                Me._optPOTrans_3.Checked = System.Windows.Forms.CheckState.Checked
            Else
                Me._optPOTrans_3.Checked = System.Windows.Forms.CheckState.Unchecked
            End If
        End Set
    End Property

    Public ReadOnly Property Send() As Boolean
        Get
            Send = mbSend
        End Get
    End Property

    Public Property FaxNbr() As String
        Get
            FaxNbr = msFaxNbr
        End Get

        Set(ByVal Value As String)
            Me._txtField_0.Text = Value
        End Set
    End Property

    Public Property EmailAddr() As String
        Get
            EmailAddr = msEmailAddr
        End Get

        Set(ByVal Value As String)
            Me._txtField_1.Text = Value
        End Set
    End Property



    Public Property IsDropShipment() As Boolean
        Get
            Return CheckBox_DropShip.Checked
        End Get
        Set(ByVal value As Boolean)
            CheckBox_DropShip.Checked = value
        End Set
    End Property


    Public ReadOnly Property TransmissionMethod() As Integer
        Get
            If Me._optPOTrans_0.Checked Then
                Return 1
            ElseIf Me._optPOTrans_1.Checked Then
                Return 2
            ElseIf Me._optPOTrans_2.Checked Then
                Return 3
            ElseIf Me._optPOTrans_3.Checked Then
                Return 4
            End If
        End Get
    End Property

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click

        logger.Debug("cmdExit_Click Entry")

        Me.Hide()
        logger.Debug("cmdExit_Click Exit")

    End Sub

    Private Sub cmdSendOrder_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdSendOrder.Click

        logger.Debug("cmdSendOrder_Click Entry")

        mbSend = True
        mbFax = Me._optPOTrans_0.Checked
        mbEmail = Me._optPOTrans_1.Checked
        mbManual = Me._optPOTrans_2.Checked
        mbElectronic = Me._optPOTrans_3.Checked
        msFaxNbr = Me._txtField_0.Text
        msEmailAddr = Me._txtField_1.Text

        Me.Hide()

        logger.Debug("cmdSendOrder_Click Exit")

    End Sub

    Private Sub frmOrderSend_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        logger.Debug("frmOrderSend_Load Entry")

        CenterForm(Me)

        ' if not in production, remind the user of that fact
        If Not My.Application.IsProduction Then
            Me._labelTestEnvironment.Visible = True
        End If

        'Look at InstanceDataFlags, figure out if Fax transmission is allowed.
        Dim allowFaxOption = InstanceDataDAO.IsFlagActive("AllowVendorWithFax") AndAlso gbSuperUser
        'If InstanceDataDAO.IsFlagActive("AllowVendorWithFax") AndAlso (cmbLabelType.Text Is Nothing Or cmbLabelType.Text = "") Then
        _optPOTrans_0.Enabled = allowFaxOption
        _optPOTrans_0.Visible = allowFaxOption
        _txtField_0.Enabled = allowFaxOption
        _txtField_0.Visible = allowFaxOption

        logger.Debug("frmOrderSend_Load Exit")
    End Sub


    Private Sub frmOrderSend_FormClosing(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        logger.Debug("frmOrderSend_FormClosing Entry")
        Dim Cancel As Boolean = eventArgs.Cancel
        Dim UnloadMode As System.Windows.Forms.CloseReason = eventArgs.CloseReason

        If UnloadMode = System.Windows.Forms.CloseReason.UserClosing Then
            Cancel = True

            Me.Hide()
        End If
        eventArgs.Cancel = Cancel
        logger.Debug("frmOrderSend_FormClosing Exit")
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub optPOTrans_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles _optPOTrans_3.CheckedChanged
        CheckBox_DropShip.Enabled = _optPOTrans_3.Checked
    End Sub
End Class