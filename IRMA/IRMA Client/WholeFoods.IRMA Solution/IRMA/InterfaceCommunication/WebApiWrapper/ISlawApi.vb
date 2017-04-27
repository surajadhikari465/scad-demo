Imports WholeFoods.IRMA.InterfaceCommunication.WebApiModel
Namespace WholeFoods.IRMA.InterfaceCommunication.WebApiWrapper
    Public Interface ISlawApi

        Sub GetSlawData()
        Sub PostPrintHeader(slawPrintBatchModel As SlawPrintBatchModel)

    End Interface
End Namespace