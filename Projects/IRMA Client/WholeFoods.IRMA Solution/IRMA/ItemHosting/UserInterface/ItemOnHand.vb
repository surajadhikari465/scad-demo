Option Strict Off
Option Explicit On

Imports System.Data.SqlClient
Imports WholeFoods.Utility.DataAccess
Imports WholeFoods.IRMA.ItemHosting.DataAccess
Imports WholeFoods.Utility

Friend Class frmItemOnHand
    Inherits System.Windows.Forms.Form

    Private mds As DataSet
    Private mdtOnHandSummary As DataTable
    Private mdtOnHandDetail As DataTable

    Private Sub cmdExit_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdExit.Click
        Me.Close()
    End Sub

    Private Sub frmItemOnHand_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load
        Call SetupDataTable()
        Call LoadDataTable()
    End Sub

    Private Sub SetupDataTable()

        Dim mdtOnHandSummaryKeys(1) As DataColumn

        ' Create a data table to store the summary On Hand information.
        mdtOnHandSummary = New DataTable("GetAllOnHand")

        With mdtOnHandSummary.Columns
            .Add(New DataColumn("Store No", GetType(String)))
            .Add(New DataColumn("Store Name", GetType(String)))
            .Add(New DataColumn("Subteam", GetType(String)))
            .Add(New DataColumn("Avg Weight", GetType(String)))
            .Add(New DataColumn("TTL Quantity", GetType(String)))
            .Add(New DataColumn("TTL Weight", GetType(String)))
            .Add(New DataColumn("Avg Cost", GetType(String)))
            .Add(New DataColumn("Last Cost", GetType(String)))

            ' Setup the primary key.
            mdtOnHandSummaryKeys(0) = .Item("Store No")
            mdtOnHandSummaryKeys(1) = .Item("Subteam")

        End With

        mdtOnHandSummary.PrimaryKey = mdtOnHandSummaryKeys
    End Sub

    Private Sub LoadDataTable()

        Dim reader As SqlDataReader = Nothing
        Dim row As DataRow
        Dim paramList As New DBParamList()

        Try

            ' clear everything before loading the dataset
            ugrdItemOnHand.DataSource = Nothing
            mdtOnHandSummary.Rows.Clear()

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            paramList.Add(New DBParam("Item_Key", DBParamType.Int, glItemID))

            ' get the store/item/pack details
            reader = GetDataReader("dbo.GetAllOnHand", paramList)

            Dim _retailUOM As String = String.Empty

            ' loop through and setup the store/item quantities
            With reader
                While .Read
                    row = mdtOnHandSummary.NewRow

                    row("Store No") = .GetInt32(.GetOrdinal("Store_No"))
                    row("Store Name") = .GetString(.GetOrdinal("Store_Name")).Trim
                    row("SubTeam") = .GetInt32(.GetOrdinal("SubTeam_No"))

                    _retailUOM = .GetString(.GetOrdinal("RetailUOM"))

                    If .GetDecimal(.GetOrdinal("AvgCost")) > 0 Then
                        row("Avg Cost") = String.Format("{0} {1}", .GetDecimal(.GetOrdinal("AvgCost")).ToString("####0.00"), _retailUOM)
                    Else
                        row("Avg Cost") = String.Format("{0}", .GetDecimal(.GetOrdinal("AvgCost")).ToString("####0.00"))
                    End If

                    If .GetDecimal(.GetOrdinal("LastOrderedCost")) > 0 Then
                        row("Last Cost") = String.Format("{0} {1}", .GetDecimal(.GetOrdinal("LastOrderedCost")).ToString("####0.00"), _retailUOM)
                    Else
                        row("Last Cost") = String.Format("{0}", .GetDecimal(.GetOrdinal("LastOrderedCost")).ToString("####0.00"))
                    End If

                    If .GetDecimal(.GetOrdinal("TotalWeight")) > 0 Then
                        row("TTL Weight") = String.Format("{0} {1}", .GetDecimal(.GetOrdinal("TotalWeight")).ToString("####0.0###"), _retailUOM)
                    Else
                        row("TTL Weight") = 0
                    End If

                    If .GetDecimal(.GetOrdinal("TotalUnits")) > 0 Then
                        row("TTL Quantity") = String.Format("{0} {1}", .GetDecimal(.GetOrdinal("TotalUnits")).ToString("####0.0###"), .GetString(.GetOrdinal("LastCostUnit")))
                    Else
                        row("TTL Quantity") = 0
                    End If

                    If .GetDecimal(.GetOrdinal("AvgWeight")) > 0 Then
                        row("Avg Weight") = String.Format("{0} {1}", .GetDecimal(.GetOrdinal("AvgWeight")).ToString("####0.0###"), _retailUOM)
                    Else
                        row("Avg Weight") = 0
                    End If

                    mdtOnHandSummary.Rows.Add(row)
                End While

                .Close()
            End With

            mdtOnHandSummary.AcceptChanges()

            ' setup the dataset we're binding to the grid
            mds = New DataSet
            mds.Tables.Add(mdtOnHandSummary)

            ' bind the dataset to the grid
            ugrdItemOnHand.DataSource = mds

            ' Hide the Store_No columns and whichever cost column won't be used by the region.
            ugrdItemOnHand.DisplayLayout.Bands(0).Columns(0).Hidden = True

            If ConfigurationServices.AppSettings("UseAvgCostforCostandMargin") Then
                ugrdItemOnHand.DisplayLayout.Bands(0).Columns("Last Cost").Hidden = True
            Else
                ugrdItemOnHand.DisplayLayout.Bands(0).Columns("Avg Cost").Hidden = True
            End If

        Catch ex As Exception

            MessageBox.Show(ex.Message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error)

        Finally
            paramList = Nothing
            reader = Nothing

            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default

        End Try

    End Sub

    Private Function GetDataReader(ByVal procedureName As String, ByVal paramList As DBParamList) As SqlClient.SqlDataReader

        Dim factory As New DataFactory(DataFactory.ItemCatalog)
        Dim reader As SqlDataReader = Nothing

        Try
            reader = factory.GetStoredProcedureDataReader(procedureName, paramList)

        Catch ex As Exception
            Throw ex

        Finally
            factory = Nothing

        End Try

        Return reader

    End Function

End Class