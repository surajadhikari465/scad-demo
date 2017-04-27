Imports log4net
Imports System.Configuration
Imports WholeFoods.Utility
Imports System.Data
Imports System.Text
Imports WholeFoods.IRMA.Administration.ReasonCodes.BusinessLogic
Imports WholeFoods.IRMA.Administration.Common.BusinessLogic

Public Class ReasonCodeDetailAdd
    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Public Sub New()

        InitializeComponent()

    End Sub

    Private Sub ReasonCodeDetailAdd_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        CenterForm(Me)

    End Sub

    Private Sub cmdExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExit.Click

        logger.Debug("ReasonCodeDetailAdd.cmdExit_Click Entry")
        Me.Close()
        logger.Debug("ReasonCodeDetailAdd.cmdExit_Click Exit")

    End Sub

    Private Sub cmdSubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSubmit.Click
        logger.Debug("ReasonCodeDetailAdd.cmdSubmit_Click entry")

        If txtCode.Text <> "" And txtDescription.Text <> "" Then
            Dim bo As ReasonCodeMaintenanceBO = New ReasonCodeMaintenanceBO

            If bo.checkIfReasonCodeDetailExists(txtCode.Text) Then
                MsgBox("This Reason Code Detail already exists, please enter a new code!", MsgBoxStyle.Exclamation, Me.Text)
            Else
                bo.createReasonCodeDetail(txtCode.Text, txtDescription.Text, txtExtDescription.Text)
                MsgBox("Reason Code Detail added successfully!", MsgBoxStyle.Exclamation, Me.Text)
                Me.Close()
            End If
        Else
            MsgBox("Please enter a Code and Description for Reason Code Detail!", MsgBoxStyle.Exclamation, Me.Text)
        End If

        logger.Debug("ReasonCodeDetailAdd.cmdSubmit_Click exit")
    End Sub
End Class