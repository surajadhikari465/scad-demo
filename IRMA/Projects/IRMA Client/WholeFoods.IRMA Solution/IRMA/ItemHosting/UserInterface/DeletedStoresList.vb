Option Strict Off
Option Explicit On

Imports System.Text
Imports VB = Microsoft.VisualBasic
Imports log4net

Public Class DeletedStoresList
    Public Sub LoadForm(ByVal dt As DataTable, ByVal msgtop As String, ByVal msgbottom As String)
        ugrdSelectList.DataSource = dt
        ugrdSelectList.DataBind()

        lblMsgTop.Text = msgtop
        lblMsgBottom.Text = msgbottom
        Me.ShowDialog()
    End Sub
    Private Sub btnOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOK.Click
        Me.Close()
    End Sub
End Class