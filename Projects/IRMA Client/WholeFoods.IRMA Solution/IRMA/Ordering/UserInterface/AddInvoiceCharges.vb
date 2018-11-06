
Imports log4net
Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Ordering.BusinessLogic
Imports WholeFoods.IRMA.Ordering.DataAccess
Imports WholeFoods.IRMA.Replenishment.EInvoicing.DataAccess
Imports WholeFoods.Utility.DataAccess
Imports System.ComponentModel

Public Class AddInvoiceCharges



    Private _parentForm As frmOrderStatus
    Private _OrderId As Integer
 


    Sub New(ByVal OrderId As Integer, ByRef parentform As Form)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _parentForm = CType(parentform, frmOrderStatus)
        _OrderId = OrderId
    End Sub

    Private Sub AddInvoiceCharges_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        LoadSubTeamGLAcct(_OrderId)
        LoadAllocatedCharges()

    End Sub


    Private Sub LoadSubTeamGLAcct(ByVal OrderHeader_Id As Integer)
        Dim subteamDAO As New OrderingDAO
        Dim subteamList As BindingList(Of SubTeamBO) = OrderingDAO.LoadSubTeamGLAcct(OrderHeader_Id)

        ComboBox_SubTeamGLAcct.DataSource = subteamList
        ComboBox_SubTeamGLAcct.ValueMember = "SubTeamNo"
        ComboBox_SubTeamGLAcct.DisplayMember = "SubTeamName"

    End Sub

    Private Sub LoadAllocatedCharges()
        Dim allocatedDAO As New EInvoicingDAO
        Dim AllocatedChargesList As DataTable
        AllocatedChargesList = allocatedDAO.getAllocatedCharges()

        ComboBox_AllocatedCharges.DataSource = AllocatedChargesList
        ComboBox_AllocatedCharges.DisplayMember = "AllocatedCharge"
        ComboBox_AllocatedCharges.ValueMember = "ChargeOrAllowance"


    End Sub


    Private Sub rdoNonAllocated_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rdoNonAllocated.CheckedChanged, rdoAllocated.CheckedChanged
        If rdoNonAllocated.Checked Then
            ComboBox_SubTeamGLAcct.Visible = True
            ComboBox_AllocatedCharges.Visible = False
            Label_ChargeOrAllowance.Visible = False
        Else
            ComboBox_SubTeamGLAcct.Visible = False
            ComboBox_AllocatedCharges.Visible = True
            Label_ChargeOrAllowance.Visible = True

        End If
    End Sub

    Private Sub Button_Cancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Cancel.Click
        Me.Close()

    End Sub

    Private Sub Button_Ok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button_Ok.Click
        If saveData() Then
            _parentForm.LoadOrderInvoiceSpecCharges(glOrderHeaderID)
            Me.Close()
        End If

    End Sub

    Public Function saveData() As Boolean

        Dim value As Decimal
        Dim sDescription As String
        Dim subteamGLAcct As Integer
        Dim specChargeNew As Decimal
        Dim isAllowance As Boolean
        Dim returnValue As Boolean = False

        If TextBox_value.Text.Trim().Equals(String.Empty) Then
            MessageBox.Show("You must enter a dollar value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox_value.Focus()
            Return returnValue
            Exit Function

        End If

        If Not Decimal.TryParse(TextBox_value.Text.Trim(), value) Then
            MessageBox.Show("You must enter a valid dollar value.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            TextBox_value.Focus()
            Return returnValue
            Exit Function
        End If

        If rdoAllocated.Checked Then
            If ComboBox_AllocatedCharges.SelectedItem Is Nothing Then
                MessageBox.Show("You must select an Allocated Charge type.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ComboBox_SubTeamGLAcct.Focus()
                Return returnValue
                Exit Function
            End If

            If value < 0 Then
                MessageBox.Show("Values for Allocated charges must be entered as positive values. Charge vs. Allowance is handled through eInvoicing configuration.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return returnValue
                Exit Function
            End If

        ElseIf rdoNonAllocated.Checked Then
            If ComboBox_SubTeamGLAcct.SelectedItem Is Nothing Then
                MessageBox.Show("You must select a Subteam/GLAccount", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                ComboBox_SubTeamGLAcct.Focus()
                Return returnValue
                Exit Function
            End If
        End If

        sDescription = IIf(rdoAllocated.Checked, CType(ComboBox_AllocatedCharges.SelectedItem, DataRowView).Item("AllocatedCharge").ToString().Trim(), String.Empty).ToString()
        subteamGLAcct = CType(IIf(rdoNonAllocated.Checked, ComboBox_SubTeamGLAcct.SelectedValue, -1), Integer)
        specChargeNew = value
        If rdoAllocated.Checked Then
            If ComboBox_AllocatedCharges.SelectedValue.ToString().Equals("-1") Then  ' Allowance.
                specChargeNew = specChargeNew * -1
                isAllowance = True
            Else
                isAllowance = False
            End If
        End If
        Try
            OrderingDAO.InsertControlGroupInvoiceCharge(glOrderHeaderID, CType(IIf(rdoAllocated.Checked, 1, 2), Integer), sDescription, subteamGLAcct, isAllowance, specChargeNew)
            returnValue = True
        Catch Ex As Exception
            MessageBox.Show(String.Format("An error occurred when saving your changes.{0}{1}", vbCrLf & vbCrLf, Ex.Message))
            returnValue = False
        End Try

        Return returnValue
    End Function

    Private Sub ComboBox_AllocatedCharges_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox_AllocatedCharges.SelectedIndexChanged
        UpdateChargesOrAllowanceLabel()
    End Sub

    Private Sub ComboBox_AllocatedCharges_VisibleChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ComboBox_AllocatedCharges.VisibleChanged
        UpdateChargesOrAllowanceLabel()
    End Sub

    Private Sub UpdateChargesOrAllowanceLabel()
        With ComboBox_AllocatedCharges
            If .Visible Then
                If Not .SelectedItem Is Nothing Then
                    If .SelectedValue.ToString().Equals("-1") Then
                        Label_ChargeOrAllowance.Text = "-"
                    Else
                        Label_ChargeOrAllowance.Text = "+"
                    End If

                End If
            End If
        End With
    End Sub

End Class