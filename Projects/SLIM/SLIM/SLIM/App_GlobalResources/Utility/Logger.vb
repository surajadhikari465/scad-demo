Option Explicit On
Option Strict On

Imports System.IO
Imports System.Net

' Utility class - should be reorganized.  Currently provides logging and ftp services.
Namespace WholeFoods.Utility

    Public Class Logger

        Public Shared irmaSwitch As New TraceSwitch("IRMASwitch", "General IRMA Application Switch")
        Public Shared posPushSwitch As New TraceSwitch("POSPushSwitch", "POS Push Switch")

        Public Shared Sub LogError(ByVal stmt As String, ByVal classname As System.Type, ByVal e As Exception)
            Trace.WriteLineIf(WholeFoods.Utility.Logger.irmaSwitch.TraceError, Now & " " & GetTypeString(classname) & " " & stmt)
            Trace.WriteLineIf(WholeFoods.Utility.Logger.irmaSwitch.TraceError, e.Message)
            Trace.WriteLineIf(WholeFoods.Utility.Logger.irmaSwitch.TraceError, e.StackTrace)
        End Sub

        Public Shared Sub LogError(ByVal stmt As String, ByVal classname As System.Type)
            Trace.WriteLineIf(WholeFoods.Utility.Logger.irmaSwitch.TraceError, Now & " " & GetTypeString(classname) & " " & stmt)
        End Sub

        Public Shared Sub LogWarning(ByVal stmt As String, ByVal classname As System.Type, ByVal e As Exception)
            Trace.WriteLineIf(WholeFoods.Utility.Logger.irmaSwitch.TraceWarning, Now & " " & GetTypeString(classname) & " " & stmt)
            Trace.WriteLineIf(WholeFoods.Utility.Logger.irmaSwitch.TraceWarning, e.Message)
            Trace.WriteLineIf(WholeFoods.Utility.Logger.irmaSwitch.TraceWarning, e.StackTrace)
        End Sub

        Public Shared Sub LogWarning(ByVal stmt As String, ByVal classname As System.Type)
            Trace.WriteLineIf(WholeFoods.Utility.Logger.irmaSwitch.TraceWarning, Now & " " & GetTypeString(classname) & " " & stmt)
        End Sub

        Public Shared Sub LogInfo(ByVal stmt As String, ByVal classname As System.Type)
            Trace.WriteLineIf(WholeFoods.Utility.Logger.irmaSwitch.TraceInfo, Now & " " & GetTypeString(classname) & " " & stmt)
        End Sub

        Public Shared Sub LogDebug(ByVal stmt As String, ByVal classname As System.Type)
            Trace.WriteLineIf(WholeFoods.Utility.Logger.irmaSwitch.TraceVerbose, Now & " " & GetTypeString(classname) & " " & stmt)
        End Sub

        Private Shared Function GetTypeString(ByVal classname As System.Type) As String
            Dim returnStr As String = ""
            If (classname IsNot Nothing) Then
                returnStr = classname.ToString()
            End If
            Return returnStr
        End Function

    End Class

End Namespace

