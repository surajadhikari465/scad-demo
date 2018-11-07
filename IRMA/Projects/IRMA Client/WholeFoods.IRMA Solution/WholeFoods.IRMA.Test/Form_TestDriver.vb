Option Explicit On
Option Strict On

Imports WholeFoods.IRMA.Administration
Imports WholeFoods.Utility
Imports WholeFoods.Utility.DataAccess

Public Class Form_TestDriver
    Inherits System.Windows.Forms.Form

    ''' <summary>
    ''' Launch the POS Admin application.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim adminForm As New Form_IrmaAdministration
        adminForm.Show()
    End Sub

End Class