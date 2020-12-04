Imports System.Web.SessionState
Imports WholeFoods.Utility

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        log4net.Config.XmlConfigurator.Configure()
        Logger.LogInfo("POET MultiPOTool Started", GetType(Global_asax))
        ' Fires when the application is started
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the session is started
        Logger.LogInfo("POET MultiPOTool Session Start", GetType(Global_asax))
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires at the beginning of each request
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires upon attempting to authenticate the use
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        Dim ex As HttpUnhandledException = Server.GetLastError()
        Logger.LogError("Unhadled exception", GetType(Global_asax), ex.GetBaseException())
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        Logger.LogInfo("POET MultiPOTool Session End", GetType(Global_asax))
        ' Fires when the session ends
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Fires when the application ends
        Logger.LogInfo("POET MultiPOTool Shutdown", GetType(Global_asax))
    End Sub

End Class