Imports System.Data.SqlClient
Imports WholeFoods.IRMA.Ordering.DataAccess

Imports log4net

Namespace WholeFoods.IRMA.Administration.UserInterface
    Public Class CostAdjustmentReasonAdd

        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)


#Region "Members"

        Private _reasonId As Integer
        Private _description As String

#End Region

#Region "Properties"

        Public ReadOnly Property ReasonId() As Integer
            Get
                Return _reasonId
            End Get
        End Property

        Public ReadOnly Property Description() As String
            Get
                Return _description
            End Get
        End Property

#End Region

        Private Sub cmdAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAdd.Click
            logger.Debug("cmdAdd_Click Entry")

            _description = txtReason.Text.Trim()

            Try
                _reasonId = CostAdjustmentReasonDAO.NewCostAdjustmentReason(_description)
                Me.Close()
                logger.Debug("cmdAdd_Click Exit")
            Catch ex As SqlException
                ErrorProvider1.SetError(txtReason, "Reason already exists")
                logger.Error("Reason already exists" & ex.Message.ToString)
                logger.Debug("cmdAdd_Click Exit")
            Catch ex As Exception
                ErrorProvider1.SetError(txtReason, "Unknown error")
                logger.Error("Unknown error" & ex.Message.ToString)
                logger.Debug("cmdAdd_Click Exit")
            End Try
        End Sub

        Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click
            Close()
        End Sub

        Private Sub txtReason_Validating(ByVal sender As System.Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles txtReason.Validating
            logger.Debug("txtReason_Validating Entry")
            If txtReason.Text.Trim().Length = 0 Then
                ErrorProvider1.SetError(txtReason, "Reason must not be blank")
                txtReason.Focus()
            End If
            logger.Debug("txtReason_Validating Exit")
        End Sub
    End Class
End Namespace