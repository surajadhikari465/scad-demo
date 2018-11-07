Option Explicit On
Option Strict On

Imports log4net
Imports System.IO
Imports System.Net

Namespace WholeFoods.Utility
    ''' <summary>
    ''' Provides logging for the IRMA applications.  The log levels and output destinations are configured
    ''' in the app.config files distributed with each application, using log4net.
    ''' 
    ''' It is recommended that all new logging define a logger for each class, as opposed to using this
    ''' Utility logger.
    ''' </summary>
    ''' <remarks></remarks>
    Public Class Logger
        ' Define the log4net logger for this class.
        Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

        Public Shared Sub LogError(ByVal stmt As String, ByVal classname As System.Type, ByVal e As Exception)
            If logger.IsErrorEnabled Then
                logger.Error(GetTypeString(classname) & " " & stmt, e)
            End If
        End Sub

        Public Shared Sub LogError(ByVal stmt As String, ByVal classname As System.Type)
            If logger.IsErrorEnabled Then
                logger.Error(GetTypeString(classname) & " " & stmt)
            End If
        End Sub

        Public Shared Sub LogWarning(ByVal stmt As String, ByVal classname As System.Type, ByVal e As Exception)
            If logger.IsWarnEnabled Then
                logger.Warn(GetTypeString(classname) & " " & stmt, e)
            End If
        End Sub

        Public Shared Sub LogWarning(ByVal stmt As String, ByVal classname As System.Type)
            If logger.IsWarnEnabled Then
                logger.Warn(GetTypeString(classname) & " " & stmt)
            End If
        End Sub

        Public Shared Sub LogInfo(ByVal stmt As String, ByVal classname As System.Type)
            If logger.IsInfoEnabled Then
                logger.Info(GetTypeString(classname) & " " & stmt)
            End If
        End Sub

        Public Shared Sub LogDebug(ByVal stmt As String, ByVal classname As System.Type)
            If logger.IsDebugEnabled Then
                logger.Debug(GetTypeString(classname) & " " & stmt)
            End If
        End Sub

        Private Shared Function GetTypeString(ByVal classname As System.Type) As String
            Dim returnStr As String = ""
            If (classname IsNot Nothing) Then
                returnStr = classname.ToString()
            End If
            Return returnStr
        End Function

        Public Shared Sub WriteLog(ByVal filename As String, ByVal format As String, ByVal ParamArray arg() As Object)

            Dim oWrite As System.IO.StreamWriter
            oWrite = File.AppendText(filename)

            oWrite.WriteLine(format, arg)

            oWrite.Close()
        End Sub
    End Class

End Namespace

