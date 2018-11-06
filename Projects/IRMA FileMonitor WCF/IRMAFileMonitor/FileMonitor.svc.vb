' NOTE: You can use the "Rename" command on the context menu to change the class name "Service1" in code, svc and config file together.
Public Class FileMonitor
    Implements IFileMonitor

    Public Sub New()
    End Sub

    Public Function GetUnprocessedPushFiles(ByVal region As String) As DataTable Implements IFileMonitor.GetUnprocessedPushFiles
        Dim dtFiles As DataTable = New DataTable("tehFilez")
        Dim dr As DataRow
        Dim strFileSize As String = ""
        Dim di As New IO.DirectoryInfo(ConfigurationManager.AppSettings("PushFileDirectory").ToString)
        Dim aryFi As IO.FileInfo() = di.GetFiles(region & "*")
        Dim fi As IO.FileInfo

        Dim column As New DataColumn("Files", GetType(System.String))
        dtFiles.Columns.Add(column)

        column = New DataColumn("Created", GetType(System.DateTime))
        dtFiles.Columns.Add(column)

        column = New DataColumn("Send", GetType(System.Boolean))
        column.DefaultValue = False
        dtFiles.Columns.Add(column)

        For Each fi In aryFi
            dr = dtFiles.NewRow
            dr.Item("Files") = fi.Name
            dr.Item("Created") = fi.CreationTime
            dtFiles.Rows.Add(dr)
        Next

        Return dtFiles
    End Function

End Class
