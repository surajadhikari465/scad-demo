Imports System.Threading
Imports Core

Imports System.Runtime.InteropServices
Imports System.ComponentModel
Imports System.IO


Public Class MainForm

    Private frmUpdateSplash As UpdateSplash

    Dim updateManifest As String = "\Temp\WFMMobile\Updates.xml"
    Dim updatePath As String = "\Temp\WFMMobile\Updates"
    Dim pluginPath As String = "\Program Files\WFM Mobile\Plugins"

    Dim updateCount As Integer = 0
    Dim currentUpdate As String
    Dim currentItemCount As Integer = 0

    Private WithEvents wc As WebClient = WebClient.Instance

    Public Sub New()
        MyBase.New()

        ' initialize and show the plash screen
        Me.frmUpdateSplash = New UpdateSplash
        Me.frmUpdateSplash.Owner = Me
        Me.frmUpdateSplash.Show()

        Application.DoEvents()

        Me.Enabled = False

        InitializeComponent()

    End Sub

    Private Sub MainForm_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles MyBase.Load

        If Not IO.Directory.Exists(updatePath) Then
            IO.Directory.CreateDirectory(updatePath)
        End If

        Dim xDoc As XDocument = XDocument.Load(updateManifest)

        Dim list = From Update In xDoc.<Updates>.<Update>

        updateCount = list.Count

        If list.Count > 0 Then
            currentItemCount = 1
        End If

        StartDownloads()

    End Sub

    Private Sub UpdateProgressDownload(ByVal progress As Core.ProgressData) Handles wc.OnFileDownloadProgress

        If Me.InvokeRequired Then
            ' we were called on a worker thread
            ' marshal the call to the user interface thread
            Me.Invoke(New WebClient.ProgressIndicatorDelegate(AddressOf UpdateProgressDownload), progress)
            Return
        End If

        Dim curr As String = CStr(Math.Round((progress.BytesReceived / 1000000), 2))

        Dim currStr As String() = curr.Split(".")

        If currStr.Length > 1 Then
            If Len(currStr(1)) = 1 Then
                curr = curr & "0"
            End If
        End If

        Dim tot As Decimal = Math.Round((progress.TotalBytesToReceive / 1000000), 2)

        Me.frmUpdateSplash.pbrUpdate.Maximum = progress.TotalBytesToReceive
        Me.frmUpdateSplash.pbrUpdate.Value = progress.BytesReceived
        Me.frmUpdateSplash.lblStepMessage.Text = String.Format("{0} of {1} MB downloaded", curr, tot)

        Me.frmUpdateSplash.Focus()
        Me.frmUpdateSplash.Refresh()

    End Sub

    Private Sub UpdateProgressDownloadComplete() Handles wc.OnProcessCompleteEvent

        If Me.InvokeRequired Then
            ' we were called on a worker thread
            ' marshal the call to the user interface thread
            Me.Invoke(New WebClient.ProcessCompleteDelegate(AddressOf UpdateProgressDownloadComplete))
            Return
        End If

        wc.Dispose()

        Dim xDoc As XDocument = XDocument.Load(updateManifest)

        Dim list = From Update In xDoc.<Updates>.<Update> _
                   Where Update.@name = currentUpdate

        list.@downloaded = "1"

        xDoc.Save(updateManifest)

        If currentItemCount = updateCount Then

            ApplyUpdates()

        Else

            currentItemCount = currentItemCount + 1

            StartDownloads()

        End If

    End Sub

    Private Sub StartDownloads()

        Dim xDoc As XDocument = XDocument.Load(updateManifest)

        Dim list = From Update In xDoc.<Updates>.<Update> _
                   Where Update.@downloaded = False

        If list.Count = 0 Then

            MessageBox.Show("There are no updates to apply.", "No Updates Available", MessageBoxButtons.OK, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1)
            Application.Exit()

        Else

            Dim item = list.First

            currentUpdate = item.@name

            Me.frmUpdateSplash.lblStep.Text = String.Format("Downloading {0} update " & Environment.NewLine & "({1} of {2})", currentUpdate, currentItemCount, updateCount)
            Me.frmUpdateSplash.lblStepMessage.Text = String.Empty
            Me.frmUpdateSplash.pbrUpdate.Maximum = updateCount
            Me.frmUpdateSplash.pbrUpdate.Minimum = 1
            Me.frmUpdateSplash.pbrUpdate.Value = 1

            Me.frmUpdateSplash.Refresh()

            Dim fName As String = String.Empty

            If item.<File>.Value.Contains(".GAC.") Then

                ' strip off the .txt extension for GAC files included in the downloads for CAB deployments
                fName = item.<File>.Value.Substring(0, Len(item.<File>.Value) - 4)

            Else

                fName = item.<File>.Value

            End If

            wc.DownloadFileAsync(New Uri(item.<Location>.Value & item.<File>.Value), fName, updatePath)

        End If

    End Sub

    Private Sub ApplyUpdates()

        Dim i As Int16 = 1

        Dim xDoc As XDocument = XDocument.Load(updateManifest)

        Dim list = From Update In xDoc.<Updates>.<Update> _
                    Where Update.@downloaded = True

        Me.frmUpdateSplash.lblStep.Text = String.Format("Preparing to apply updates...")
        Me.frmUpdateSplash.lblStepMessage.Text = String.Empty
        Me.frmUpdateSplash.pbrUpdate.Minimum = 1
        Me.frmUpdateSplash.pbrUpdate.Maximum = list.Count
        Me.frmUpdateSplash.Refresh()

        Cursor.Current = Cursors.WaitCursor

        Application.DoEvents()

        For Each item In list

            'for debugging
            'item.@downloaded = "0"
            'xDoc.Save(updateManifest)

            Me.frmUpdateSplash.lblStep.Text = String.Format("Applying {0} update " & Environment.NewLine & "({1} of {2})", item.@name, i, list.Count)
            Me.frmUpdateSplash.pbrUpdate.Value = i
            Me.frmUpdateSplash.Refresh()

            If item.@type = "Assembly" Then

                If Not IO.Directory.Exists(pluginPath) Then IO.Directory.CreateDirectory(pluginPath)

                IO.File.Copy(updatePath & "\" & item.<File>.Value, pluginPath & "\" & item.<File>.Value, True)

            ElseIf item.@type = "Executable" Then

                Dim p As Process = New Process

                Try

                    p.StartInfo.UseShellExecute = True
                    p.StartInfo.FileName = "\Windows\wceload.exe"
                    p.StartInfo.Arguments = "/silent /noui " & """" & updatePath & "\" & item.<File>.Value + """"
                    p.Start()
                    Application.DoEvents()
                    p.WaitForExit()

                Catch w As System.ComponentModel.Win32Exception

                    Dim e As New Exception()
                    e = w.GetBaseException()
                    MessageBox.Show(e.Message)

                    Application.Exit()

                End Try

            End If

            i = i + 1

        Next

        Cursor.Current = Cursors.Default

        Me.frmUpdateSplash.lblStep.Text = "Updates complete. Cleaning up..."
        Me.frmUpdateSplash.Refresh()

        For Each f In IO.Directory.GetFiles(updatePath)

            IO.File.Delete(f)

        Next

        Me.frmUpdateSplash.lblStep.Text = "Restarting WFM Mobile..."
        Me.frmUpdateSplash.pbrUpdate.Value = frmUpdateSplash.pbrUpdate.Maximum
        Me.frmUpdateSplash.Refresh()

        Process.Start("\Program Files\WFM Mobile\Client.exe", Nothing)

        Application.Exit()

    End Sub

End Class

