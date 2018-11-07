Imports WholeFoods.IRMA.Administration.ReasonCodes.DataAccess

Public Class frmDeletePOReasons

    Private Sub DeletePOReasons_Load(sender As System.Object, e As System.EventArgs) Handles MyBase.Load
        LoadReasonCodesUltraCombo(ulDeleteReasonCode, enumReasonCodeType.DP)
    End Sub


    Private Sub ulDeleteReasonCode_ValueChanged(sender As System.Object, e As System.EventArgs) Handles ulDeleteReasonCode.ValueChanged
        btnYes.Enabled = True
    End Sub
End Class
