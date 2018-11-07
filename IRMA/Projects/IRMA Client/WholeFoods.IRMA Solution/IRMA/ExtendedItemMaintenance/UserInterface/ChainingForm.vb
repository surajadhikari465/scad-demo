Imports WholeFoods.IRMA.ModelLayer.DataAccess
Imports Infragistics.Win.UltraWinGrid

Public Class ChainingForm

    Dim _chainNameList As String = ""

    ''' <summary>
    ''' Sets and gets the selection of chain names in the chain name list
    ''' in the form of a comma delimeted list of chain names for an item.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChainNameList() As String
        Get
            Dim theChainNameList As String = ""
            Dim theRowChainID As Integer
            Dim theRowChainName As String

            For Each theRow As UltraGridRow In Me.UltraGridChains.Selected.Rows

                theRowChainID = CInt(theRow.Cells(0).Value)
                theRowChainName = CStr(theRow.Cells(1).Value)

                If theChainNameList.Length > 0 Then
                    theChainNameList = theChainNameList + ", "
                End If
                theChainNameList = theChainNameList + theRowChainID.ToString() + ": " + theRowChainName
            Next

            Return theChainNameList
        End Get
        Set(ByVal value As String)
            _chainNameList = value
        End Set
    End Property

    Private Sub Initialize()
        Dim theDataSet As DataSet = ItemChainDAO.Instance.GetAllItemChains()

        Me.UltraGridChains.SetDataBinding(theDataSet, "", True)

        Dim theChainNameList As String = ", " + Me._chainNameList + ", "
        Dim theRowChainID As Integer

        For Each theRow As UltraGridRow In Me.UltraGridChains.Rows

            theRowChainID = CInt(theRow.Cells(0).Value)

            If theChainNameList.IndexOf(", " + theRowChainID.ToString() + ": ") > -1 Then
                theRow.Selected = True
            End If
        Next
    End Sub

    Private Sub ChainingForm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Initialize()
    End Sub

    Private Sub ButtonClear_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonClear.Click

        Me.UltraGridChains.Selected.Rows.Clear()

    End Sub
End Class