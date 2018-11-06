Option Explicit On
Option Strict On

Imports System.IO
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.TagPush.Controller
Imports WholeFoods.IRMA.Replenishment.TagPush.BusinessLogic
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.Utility

' NOTE: If this Namespace is updated, the StoreUpdatesBO constructor should also be updated.
Namespace WholeFoods.IRMA.Replenishment.TagPush.Writers

    Public Class FileMakerPro_Writer
        Inherits TagWriter

#Region "Writer Constructors"
        Public Sub New(ByVal FileWriterKey As Integer)
            MyBase.New(FileWriterKey)
            MyBase.OutputFileFormat = FileFormat.Text
            MyBase.ExemptTagFile = False
        End Sub
#End Region

#Region "Property Definitions"
#End Region

#Region "Extend Methods"
#End Region

    End Class

End Namespace

