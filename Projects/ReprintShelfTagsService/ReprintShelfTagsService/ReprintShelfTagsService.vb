Public Class ReprintShelfTagsService
    Inherits System.ServiceProcess.ServiceBase

    Private WithEvents tmrChecker As System.Timers.Timer
    Private _reprintTags As ReprintShelfTags = Nothing

    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.

        Try
            _reprintTags = New ReprintShelfTags()

            tmrChecker = New System.Timers.Timer(_reprintTags.TimerInterval)
            tmrChecker.Enabled = True

        Catch ex As Exception
            Diagnostics.EventLog.WriteEntry(Me.ServiceName, ex.ToString, EventLogEntryType.Error)
            MsgBox(ex.ToString, MsgBoxStyle.Critical, "Error starting service")

        End Try

    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.

        Try
            tmrChecker.Enabled = False

        Catch ex As Exception
            Diagnostics.EventLog.WriteEntry(Me.ServiceName, ex.ToString, EventLogEntryType.Error)
            MsgBox(ex.ToString, MsgBoxStyle.Critical, "Error stoping service")

        Finally
            tmrChecker.Dispose()
            tmrChecker = Nothing
            _reprintTags = Nothing

        End Try

    End Sub

    Private Sub tmrChecker_Elapsed(ByVal sender As Object, ByVal e As System.Timers.ElapsedEventArgs) Handles tmrChecker.Elapsed

        Try
            'disable the timer while processing
            tmrChecker.Enabled = False

            Call _reprintTags.ProcessReprintRequests()

        Catch ex As Exception
            Call _reprintTags.HandleError(ex)
            'write to event log 
            Diagnostics.EventLog.WriteEntry(Me.ServiceName, ex.ToString, EventLogEntryType.Error)

        Finally
            tmrChecker.Enabled = True

        End Try

    End Sub

End Class
