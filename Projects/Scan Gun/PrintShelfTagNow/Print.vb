Imports WholeFoods.Utility

Public Class Print
    Public Enum SignType
        Grocery = 0
        Nutrition = 1
    End Enum
    Public Sub New()
    End Sub
    Public Sub PrintSignNow(ByVal Item As ItemCatalog.StoreItem, ByVal Store As ItemCatalog.Store, ByVal Type As SignType, ByVal PrinterNetworkName As String, ByVal Copies As Integer)
        'Assumes that Item is instantiated with the user's selected item
        Try
            Dim crwreport As New CrystalDecisions.CrystalReports.Engine.ReportDocument
            Dim reportName As String

            'Determine report to print
            If Item.IsOnSale Then
                If Item.IsSaleEDLP Then
                    reportName = IIf(Type = 0, "EDLPGrocery", "EDLPNutrition")
                Else
                    reportName = IIf(Type = 0, "GrocerySale", "NutritionSale")
                End If
            Else
                reportName = IIf(Type = 0, "Grocery", "Nutrition")
            End If

            crwreport.FileName = String.Format("{0}\{1}SignHand{2}.rpt", _
                My.Application.Info.DirectoryPath, _
                IIf(Store.WFM_Store, "WFM", "HFM"), _
                reportName)
            crwreport.SetParameterValue(0, Item.Item_Key)
            crwreport.SetParameterValue(1, Item.StoreNo)
            Dim config As New System.Configuration.AppSettingsReader
            'Uses ODBC to connect, so the server and database parameters are not needed (and do not work if set)
            crwreport.SetDatabaseLogon(CType(ConfigurationServices.AppSettings("crystalUser"), String), CType(ConfigurationServices.AppSettings("crystalPassword"), String))
            crwreport.PrintOptions.PrinterName = PrinterNetworkName
            crwreport.PrintToPrinter(Copies, False, 0, 0)
        Catch ex As System.Exception
            Throw ex
        End Try
    End Sub
End Class
