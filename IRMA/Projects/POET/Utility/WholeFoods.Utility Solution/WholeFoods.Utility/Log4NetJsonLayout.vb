Imports System.IO
Imports log4net.Core
Imports log4net.Layout
Imports Newtonsoft.Json

Namespace WholeFoods.Utility.Layout.Json
    Public Class Log4NetJsonLayout
        Inherits LayoutSkeleton

        Public Sub New()
            IgnoresException = False
        End Sub

        Public Overrides Sub ActivateOptions()
        End Sub

        Public Overrides Sub Format(ByVal writer As TextWriter, ByVal e As LoggingEvent)
            Dim dic As New Dictionary(Of String, Object) From
            {
                {"date", e.TimeStamp.ToUniversalTime().ToString("O")},
                {"level", e.Level.DisplayName},
                {"message", e.RenderedMessage},
                {"exception", e.ExceptionObject},
                {"logger", e.LoggerName},
                {"userName", e.UserName}
            }

            writer.WriteLine(JsonConvert.SerializeObject(dic))
        End Sub

    End Class
End Namespace
