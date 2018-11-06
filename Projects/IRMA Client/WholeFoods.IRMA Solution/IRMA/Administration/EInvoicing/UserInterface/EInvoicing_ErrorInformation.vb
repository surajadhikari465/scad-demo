
Imports WholeFoods.IRMA.Replenishment.EInvoicing.DataAccess
Public Class EInvoicing_ErrorInformation



    Private _EInvoiceId As Integer
    Public Property EInvoiceId() As Integer
        Get
            Return _EInvoiceId
        End Get
        Set(ByVal value As Integer)
            _EInvoiceId = value
        End Set
    End Property

    Private _ErrorHistory As Hashtable = New Hashtable()
    Private _isPrintOk As Boolean = False



    Private Sub EInvoicing_ErrorInformation_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ClearBrowser()



        LoadHistory()
        While WB_ErrorHistory.ReadyState <> WebBrowserReadyState.Complete
            Application.DoEvents()
        End While

    End Sub


    Private Sub ClearBrowser()
        WB_ErrorHistory.Navigate("about:blank")
        While WB_ErrorHistory.ReadyState <> WebBrowserReadyState.Complete
            Application.DoEvents()
        End While


    End Sub
    Private Sub LoadHistory()
        Dim dt As DataTable
        Dim dao As EInvoicingDAO = New EInvoicingDAO
        dt = dao.GetErrorHistory(_EInvoiceId)

        If dt.Rows.Count = 0 Then
            ' no recods found.
            WB_ErrorHistory.Document.Write("<html><div align='center' style='font-family: Calibri; font-size=11px; font-weight: bold;'>No Error History Information Exists for this EInvoice</div></html>")
        Else
            WB_ErrorHistory.Document.Write("<html><div align='center' style='font-family: Calibri; font-size=11px; font-weight: bold;'><-- Choose an Error History Report from the left</div></html>")
            _ErrorHistory.Clear()
            For Each dr As DataRow In dt.Rows
                _ErrorHistory.Add(dr("TimeStamp").ToString(), dr("ErrorInformation").ToString())
            Next
        End If

        Dim root As TreeNode = TreeView_ErrorHistory.Nodes.Add("root", "Error History")
        For Each dr As DataRow In dt.Rows
            root.Nodes.Add(dr("ErrorHistoryId").ToString(), dr("Timestamp").ToString())
        Next

        TreeView_ErrorHistory.ExpandAll()

    End Sub

    Private Sub TreeView_NodeMouseClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeNodeMouseClickEventArgs) Handles TreeView_ErrorHistory.NodeMouseClick
        ClearBrowser()
        If e.Node.Level <> 0 Then ' not the root node.
            Dim timestamp As String = e.Node.Text
            WB_ErrorHistory.Document.Write(_ErrorHistory(timestamp))
            _isPrintOk = True
        Else
            WB_ErrorHistory.Document.Write("<html><div align='center' style='font-family: Calibri; font-size=11px; font-weight: bold;'><-- Choose an Error History Report from the left</div></html>")
            _isPrintOk = False
        End If

        While WB_ErrorHistory.ReadyState <> WebBrowserReadyState.Complete
            Application.DoEvents()

        End While


    End Sub

    Private Sub ExitToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ExitToolStripMenuItem.Click
        Me.Close()
    End Sub

    Private Sub PrintToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles PrintToolStripMenuItem.Click
        If _isPrintOk Then
            WB_ErrorHistory.ShowPrintDialog()
        Else
            MessageBox.Show("No error information has been selected. Please choose a timestamp from the tree to the left.", "Nothing to print", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub
End Class