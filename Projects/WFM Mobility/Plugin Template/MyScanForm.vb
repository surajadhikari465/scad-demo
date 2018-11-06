Public Class MyScanForm
    Inherits HandheldHardware.ScanForm
    Private MyScanner As HandheldHardware.HandheldScanner
    Public Shared Sub Main()
        Application.Run(New MyScanForm())
    End Sub
    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.KeyPreview = True

        'init HH scanner check to see if scanner is NULL
        If Not MyScanner Is Nothing Then
            MyScanner.restoreScannerSettings()
        End If

        MyScanner = New HandheldHardware.HandheldScanner(Me)
        If (MyScanner.HHType = HandheldHardware.Scanner.UNKNOWN) Then
            MessageBox.Show("Scanner Init Error, scanner hardware not recognized.", "Error")
        End If


    End Sub

    Public Overrides Sub updateControlsOnScanCompleteEvent()

        Try

            StatusBar1.Text = "Scan complete!"

            For Each c As Control In Me.Controls
                c.Refresh()
            Next

            StatusBar1.Visible = False

            Me.SuccessLabel.Text = "Scan successful, UPC: " & Me.UPCTextBox.Text


        Catch e As Exception
        End Try

    End Sub


    Public Overrides Sub updateControlsScanFailedEvent()

        Try

            StatusBar1.Visible = True
            StatusBar1.Text = "*** Scan failed ***"

            For Each c As Control In Me.Controls
                c.Refresh()
            Next

        Catch ex As ObjectDisposedException
        End Try

    End Sub


    Public Overrides Sub updateControlsOnScanTriggerEvent()

        Try

            If (Not (StatusBar1.Text.Equals("*** Scan failed ***")) And (Not (StatusBar1.Text.Equals("Scan complete!")))) Then

                StatusBar1.Visible = True
                StatusBar1.Text = "User abandoned"

            End If
            For Each c As Control In Me.Controls
                c.Refresh()
            Next

        Catch ex As ObjectDisposedException
        End Try

    End Sub

    Public Overrides Sub updateControlsOnScanFailedEvent()

        Try

            StatusBar1.Visible = True
            StatusBar1.Text = "*** Scan failed ***"

            For Each c As Control In Me.Controls
                c.Refresh()
            Next

        Catch ex As ObjectDisposedException
        End Try

    End Sub


    Public Overrides Sub updateUPCText(ByVal upc As String)
        Dim rweightStr As String
        rweightStr = upc.Substring(upc.Length - 11, 1)

        Try
            'This will replace the last 5 digits with zeros if the first digit is "2"
            If rweightStr = 2 Then   ' Value of the first digit on the UPC needs to be "2"
                Dim str1 As String = "00000"  ' Digits replacing the last 5 digits on the original UPC
                Dim remStr As String = upc.Remove(6, 5) ' When first digit = 2, remove the last 5 digits of the orginal UPC 
                Dim conStr As String = String.Concat(remStr, str1) ' Assign UPC value to 'conStr' variable then concatenate "00000" at the end
                upc = conStr ' Assign the new UPC (with zeros at the end) back to the variable 'upc'

                'This populates the scanned upc into the UPC textbox
                Me.UPCTextBox.Text = upc

            Else

            End If

            'This populates the scanned upc into the UPC textbox
            Me.UPCTextBox.Text = upc

        Catch ex As Exception

        End Try
 
    End Sub

    Public Overrides Sub isTriggerDown(ByVal isDown As Boolean)

        Try

            If (isDown) Then

                StatusBar1.Visible = True
                StatusBar1.Text = "Scan now..."

            End If

        Catch ex As Exception

        End Try
    End Sub

    Private Sub MyScanForm_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If (e.KeyCode = System.Windows.Forms.Keys.Up) Then
            'Up
        End If
        If (e.KeyCode = System.Windows.Forms.Keys.Down) Then
            'Down
        End If
        If (e.KeyCode = System.Windows.Forms.Keys.Left) Then
            'Left
        End If
        If (e.KeyCode = System.Windows.Forms.Keys.Right) Then
            'Right
        End If
        If (e.KeyCode = System.Windows.Forms.Keys.Enter) Then
            'Enter


        End If

    End Sub
End Class
