Public Class Enums

    Public Enum enumOrderType
        Purchase = 1
        Distribution = 2
        Transfer = 3
        Flowthru = 4
    End Enum

    Public Enum SubTeamType
        Retail = 1
        Manufacturing = 2
        RetailManufacturing = 3
        Expense = 4
        Packaging = 5
        Suplies = 6
    End Enum

    Public Enum ActionType
        Order = 1
        Credit = 2
        Transfer = 3
        ItemCheck = 4
        PrintSign = 5
        Receive = 6
        CycleCountTeam = 7
        CycleCountLocation = 8
        ReceiveDocument = 9
        ReceiveDocumentCredit = 10
    End Enum

    Public Enum ProductType
        Product = 1
        PackagingSupplies = 2
        OtherSupplies = 3
    End Enum

    Public Enum ReasonCodeType
        CA
        RR
        RD
        DR
        SP
        RI
    End Enum

    Public Enum DocumentType
        Invoice = 1
        Other = 2
        None = 3
    End Enum

End Class