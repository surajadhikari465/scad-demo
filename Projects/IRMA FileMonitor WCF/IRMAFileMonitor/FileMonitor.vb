' NOTE: You can use the "Rename" command on the context menu to change the interface name "IService1" in both code and config file together.
<ServiceContract()> _
Public Interface IFileMonitor

    ' TODO: Add your service operations here
    <OperationContract()> _
    Function GetUnprocessedPushFiles(ByVal region As String) As DataTable

End Interface
