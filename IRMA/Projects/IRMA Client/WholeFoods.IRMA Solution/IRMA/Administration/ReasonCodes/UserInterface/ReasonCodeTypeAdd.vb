
Imports log4net
Imports System.Configuration
Imports WholeFoods.Utility
Imports System.Data
Imports System.Text
Imports WholeFoods.IRMA.Administration.ReasonCodes.BusinessLogic
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic

Public Class ReasonCodeTypeAdd

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Sub New()

        InitializeComponent()

    End Sub

    Private Sub ReasonCodeMaintenance_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        CenterForm(Me)

    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click

        logger.Debug("ReasonCodeTypeAdd.cmdSubmit_Click entry")

        If txtReasonCodeTypeAbbr.Text <> "" And txtReasonCodeTypeDesc.Text <> "" Then
            Dim bo As ReasonCodeMaintenanceBO = New ReasonCodeMaintenanceBO

            If bo.checkIfReasonCodeTypeExists(txtReasonCodeTypeAbbr.Text) Then
                MsgBox("This Reason Code Type already exists, please enter a new code!", MsgBoxStyle.Exclamation, Me.Text)
            Else
                bo.createReasonCodeType(txtReasonCodeTypeAbbr.Text, txtReasonCodeTypeDesc.Text)
                MsgBox("Reason Code Type added successfully!", MsgBoxStyle.Exclamation, Me.Text)
                Me.Close()
            End If
        Else
            MsgBox("Please enter an Abbreviation and Description for Reason Code Type!", MsgBoxStyle.Exclamation, Me.Text)
        End If

        logger.Debug("ReasonCodeTypeAdd.cmdSubmit_Click exit")

    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click

        logger.Debug("ReasonCodeTypeAdd.cmdExit_Click Entry")
        Me.Close()
        logger.Debug("ReasonCodeTypeAdd.cmdExit_Click Exit")

    End Sub
End Class