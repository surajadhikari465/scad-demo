Option Explicit On
Option Strict On

Imports System.IO
Imports System.Data.SqlClient
Imports System.Text
Imports WholeFoods.IRMA.Replenishment.Common.Writers
Imports WholeFoods.IRMA.Replenishment.POSPush.Controller
Imports WholeFoods.IRMA.Replenishment.POSPush.BusinessLogic
Imports WholeFoods.Utility

' NOTE: If this Namespace is updated, the StoreUpdatesBO constructor should also be updated.
Namespace WholeFoods.IRMA.Replenishment.POSPush.Writers

    Public Class PIRUS_Writer
        Inherits POSWriter

        ' POSFilename is the name of the file placed on the FTP server
        Private _POSFilename As String = "FILEUPDT.DAT"
        Private _outputFileFormat As FileFormat = FileFormat.Text
        Protected Overrides_fileEncoding As Encoding = Encoding.GetEncoding(1252)

#Region "Writer Constructors"

        Public Sub New(ByVal FileWriterKey As Integer)
            MyBase.New(FileWriterKey)
        End Sub

#End Region

#Region "Writer specific methods to add a single record to the POS Push file"

        ''' <summary>
        ''' The PIRUS Writer does not use the common tax flag enabled logic.  Instead, the POSID for
        ''' the active tag flag is displayed. 
        ''' </summary>
        ''' <param name="currentColumn"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overrides Function BuildTaxDataString(ByRef currentColumn As POSDataElementBO) As String
            Dim dataContent As String = Nothing
            ' get all of the TaxFlagBO objects for this item in this store
            Dim currentItemKey As Integer = CType(_currentChange.GetValue(_currentChange.GetOrdinal("Item_Key")), Integer)
            Dim itemTaxValues As Hashtable = Nothing
            Dim taxEnum As IDictionaryEnumerator = Nothing
            Dim currentTaxFlag As TaxFlagBO

            If _currentStoreUpdate.ItemTaxFlags.Item(currentItemKey) IsNot Nothing Then
                itemTaxValues = CType(_currentStoreUpdate.ItemTaxFlags.Item(currentItemKey), Hashtable)
                taxEnum = itemTaxValues.GetEnumerator()
            End If

            ' iterate the list of tax flags until we find the active value
            If taxEnum IsNot Nothing Then
                While taxEnum.MoveNext()
                    currentTaxFlag = CType(taxEnum.Value, TaxFlagBO)
                    If currentTaxFlag.TaxFlagValue Then
                        ' the active tax flag has been found
                        dataContent = currentTaxFlag.POSId.ToString
                        Exit While
                    End If
                End While
            End If

            Return dataContent
        End Function

        ''' <summary>
        ''' Add footer specific to a change type
        ''' </summary>
        ''' <param name="chgType"></param>
        ''' <remarks></remarks>
        Protected Overrides Sub AddSectionFooterTextToFile(ByVal chgType As ChangeType, ByVal footerInfo As POSBatchFooterBO)
            Select Case chgType
                'add promo offer footer: XA 8 line
                Case ChangeType.PromoOffer
                    Dim footer As New StringBuilder

                    footer.Append("XA 03")
                    If footerInfo IsNot Nothing Then
                        footer.Append(footerInfo.PIRUS_StartDate)
                    End If
                    footer.Append("8")

                    WriteLine(footer.ToString)
            End Select
        End Sub

#End Region


#Region "Property Definitions"
        Overrides Property WriterFilename(ByVal currentStore As StoreUpdatesBO) As String
            Get
                Return _POSFilename
            End Get
            Set(ByVal value As String)
                _POSFilename = value
            End Set
        End Property

        Public Overrides Property OutputFileFormat() As FileFormat
            Get
                Return _outputFileFormat
            End Get
            Set(ByVal value As FileFormat)
                _outputFileFormat = value
            End Set
        End Property

        'Overrides Property IsBinary() As Boolean
        '    Get
        '        Return _isBinary
        '    End Get
        '    Set(ByVal value As Boolean)
        '        _isBinary = value
        '    End Set
        'End Property
#End Region

    End Class
End Namespace

