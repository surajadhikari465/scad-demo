Imports System.Reflection

Public Class NoNetwork
    Private _hasValidNetwork As Boolean = False
    Private ReadOnly _parentForm As Form

    Sub New(ByVal parentForm As Form)

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _parentForm = parentForm
    End Sub

    Private Sub NoNetwork_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = String.Format("OOS {0}", AppVersion)
        ValidateNetworkStatus()
    End Sub


    Private Sub ValidateNetworkStatus()
        If Not _hasValidNetwork Then
            If TcpSocketTest() Then
                With Label_NetworkStatus
                    .Text = "Network Available"
                    .ForeColor = Color.ForestGreen
                End With
                _hasValidNetwork = True
            Else
                With Label_NetworkStatus
                    .Text = "No Network Available"
                    .ForeColor = Color.Firebrick
                End With
                _hasValidNetwork = False
            End If

            SetNetworkStatus()
        End If
    End Sub

    Public Sub SetNetworkStatus()
        MenuItem_Continue.Enabled = _hasValidNetwork
    End Sub

    Private Sub MenuItem_Exit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles MenuItem_Exit.Click
        'set force close, so we skip the "close OOS" confirmation.
        'may be creating NoNetwork Form as a child of different types of ParentForms. 
        'So do this as in a way that will only set ForceClose=True if it exists on that form. #reflection

        Dim t As Type = _parentForm.GetType()

        For Each prop As FieldInfo In t.GetFields()
            If prop.Name.Equals("ForceClose") Then
                prop.SetValue(_parentForm, True)
            End If
        Next
        DialogResult = DialogResult.Abort
        
    End Sub

    Private Sub MenuItem_Continue_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) _
        Handles MenuItem_Continue.Click
        Timer1.Enabled = False
        Close()
    End Sub

    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        ValidateNetworkStatus()
    End Sub


End Class