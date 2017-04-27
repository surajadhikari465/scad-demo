Namespace WholeFoods.IRMA.Replenishment.Common.Writers

    Public Class Constants

        'FileWriterType table values
        Public Const FileWriterType_POS As String = "POS"
        Public Const FileWriterType_TAG As String = "TAG"
        Public Const FileWriterType_POSPULL As String = "POSPull"
        Public Const FileWriterType_REPRINTTAGS As String = "REPRINTTAGS"
        Public Const FileWriterType_SCALE As String = "SCALE"
        Public Const FileWriterType_PLUMSTORE As String = "PLUMStore"
        Public Const FileWriterType_ELECTRONICSHELFTAG As String = "EST"

        'POSWriter.ScaleWriterType --> ScaleWriterType.ScaleWriterTypeDesc values
        Public Const ScaleWriterType_Store As String = "STORE"
        Public Const ScaleWriterType_Corporate As String = "CORPORATE"
        Public Const ScaleWriterType_Zone As String = "ZONE"
        Public Const ScaleWriterType_SmartXZone As String = "SMARTX ZONE"

        'action codes reside in GetPLUMCorpChg stored proc
        'they indicate what type of change is being sent to the scale system
        Public Const ActionCode_ItemChange As Char = "C"c
        Public Const ActionCode_ItemIdAdd As Char = "A"c
        Public Const ActionCode_ItemIdDelete As Char = "D"c
        Public Const ActionCode_FullScaleFile As Char = "F"c

    End Class

End Namespace