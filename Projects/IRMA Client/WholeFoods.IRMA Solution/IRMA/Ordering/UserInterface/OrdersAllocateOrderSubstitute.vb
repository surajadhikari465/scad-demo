Imports System.IO

Imports log4net

Imports WholeFoods.IRMA.Ordering.BusinessLogic
Imports WholeFoods.IRMA.Ordering.DataAccess

Public Class OrdersAllocateOrderSubstitute

    ' Define the log4net logger for this class.
    Private Shared logger As ILog = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType)

    Private iIdentifier As String
    Private iSubIdentifier As String
    Private bNonRetail As Boolean
    Private sSubTeam As String

    Public Property Identifier() As String
        Get
            Return iIdentifier
        End Get
        Set(ByVal value As String)
            iIdentifier = value
        End Set
    End Property

    Public Property SubIdentifier() As String
        Get
            Return iSubIdentifier
        End Get
        Set(ByVal value As String)
            iSubIdentifier = value
        End Set
    End Property

    Public Property NonRetail() As Boolean
        Get
            Return bNonRetail
        End Get
        Set(ByVal value As Boolean)
            bNonRetail = value
        End Set
    End Property

    Public Property SubTeam() As String
        Get
            Return sSubTeam
        End Get
        Set(ByVal value As String)
            sSubTeam = value
        End Set
    End Property

    Private Sub OrdersAllocateOrderSubstitute_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        gridOrderList.DataSource = FairShareAllocationDAO.GetOrdersMissingSubstitutionItem(Identifier, SubIdentifier, NonRetail)

        If gridOrderList.Rows.Count > 0 Then
            cmdExport.Enabled = True
        End If

    End Sub

    Private Sub cmdExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdExport.Click

        Dim fName As String = My.Computer.FileSystem.SpecialDirectories.Desktop & "\" & SubTeam.ToUpper & "-SUBSTITUTIONS FOR " & Identifier & (IIf(NonRetail, ".NON-RETAIL.", ".RETAIL.")).ToString & Date.Today.Month.ToString & Date.Today.Day.ToString & Date.Today.Year.ToString & ".xls"

        Try

            If File.Exists(fName) Then
                If MessageBox.Show("A file with the same name already exists on the desktop." & vbNewLine & "Would you like to replace it?", "Replace File", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = Windows.Forms.DialogResult.Yes Then
                    File.Delete(fName)
                Else
                    Exit Sub
                End If
            End If

            gridExport.Export(Me.gridOrderList, fName)

            MessageBox.Show("Export Complete", "Export Finished", MessageBoxButtons.OK, MessageBoxIcon.Information)

        Catch ex As Exception

            MessageBox.Show("Unable to complete export: " & ex.InnerException.Message.ToString, "Export Failed", MessageBoxButtons.OK, MessageBoxIcon.Error)

        End Try

    End Sub

End Class
