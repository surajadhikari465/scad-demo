Imports log4net
Imports WholeFoods.IRMA.Ordering.BusinessLogic
Imports WholeFoods.IRMA.Ordering.DataAccess

Public Class ApproveInvoice
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private approvalTypeSelected As Boolean = False

#Region "Events raised by this form"
    ''' <summary>
    ''' Notify the calling form of the approval option selected by the user.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event ApproveByPOSelected()

    ''' <summary>
    ''' Notify the calling form of the approval option selected by the user.
    ''' </summary>
    ''' <remarks></remarks>
    Public Event ApproveByInvoiceSelected()
#End Region

    ''' <summary>
    ''' The user selected to approve the invoice.  Notify the calling form of the approval
    ''' option so that it can be processed.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button_Approve_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Approve.Click
        logger.Debug("Button_Approve_Click entry")
        ' Verify the user selected an option.
        If Not RadioButton_PayInvoice.Checked AndAlso Not RadioButton_PayPO.Checked Then
            MsgBox(String.Format(ResourcesIRMA.GetString("Required"), GroupBox_ApprovalOptions.Text), MsgBoxStyle.Critical, Me.Text)
        Else
            ' Raise the appropriate event, based on the option selected, so the calling form can 
            ' process the approval.
            If RadioButton_PayInvoice.Checked Then
                logger.Info("Button_Approve_Click - user selected the Pay by Invoice option")
                Me.ApprovalInfo.ApprovalType = ApprovalInformationBO.ApprovalTypeEnum.PayByInvoice
                approvalTypeSelected = True
            ElseIf RadioButton_PayPO.Checked Then
                logger.Info("Button_Approve_Click - user selected the Pay by PO option")
                Me.ApprovalInfo.ApprovalType = ApprovalInformationBO.ApprovalTypeEnum.PayByPO
                approvalTypeSelected = True
            End If

            ' Close the form.
            Me.Close()
        End If
        logger.Debug("Button_Approve_Click exit")
    End Sub

    ''' <summary>
    ''' Process the form closing.  
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ApproveInvoice_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
        logger.Debug("ApproveInvoice_FormClosing entry")
        ' If an approval event was not raised, verify the user wants to close the form without
        ' approving the invoice.
        If Not approvalTypeSelected Then
            If MsgBox("Do you want to exit without approving the invoice for upload?", MsgBoxStyle.YesNo, Me.Text) = MsgBoxResult.No Then
                ' Cancel the form closing
                e.Cancel = True
            End If
        End If
        logger.Debug("ApproveInvoice_FormClosing exit")
    End Sub

    Private Sub ApproveInvoice_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' Enable pay-agreed-cost radio in approval form only if user is PO Approval Admin.
        Me.RadioButton_PayPO.Enabled = gbPOApprovalAdmin
    End Sub

    Public Sub New(ByRef ApprovalInfo As ApprovalInformationBO)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.ApprovalInfo = ApprovalInfo
    End Sub


    Private _approvalInfo As ApprovalInformationBO
    Public Property ApprovalInfo() As ApprovalInformationBO
        Get
            Return _approvalInfo
        End Get
        Set(ByVal value As ApprovalInformationBO)
            _approvalInfo = value
        End Set
    End Property



End Class