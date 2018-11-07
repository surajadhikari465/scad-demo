''' <summary>
''' Dialog to execute the auto-allocation process with available options.
''' </summary>
''' <remarks></remarks>
Public Class OrdersAutoAllocate

#Region " Public Properties"

    ''' <summary>
    ''' Returns the Checked value of the chkCasePackMoves checkbox control.
    ''' </summary>
    ''' <returns>Boolean</returns>
    ''' <remarks></remarks>
    Public ReadOnly Property DoCasePackMoves() As Boolean
        Get
            Return True
        End Get
    End Property

#End Region

#Region " Control Methods"

    Private Sub cmdStart_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdStart.Click

        DialogResult = Windows.Forms.DialogResult.OK

    End Sub

#End Region

End Class
