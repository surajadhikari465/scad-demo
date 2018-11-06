Imports System.IO
Imports System.Data

Public Class TransferFileWriter
    Inherits ShrinkFileWriter
    Private ReadOnly TRANSFER_DIR_NAME As String = "Transfer"

    Public Sub New(ByVal userSession As Session)
        MyBase.New(userSession)
    End Sub

    ''' <summary>
    ''' Saves a transfer item to the transfer session file.
    ''' </summary>
    ''' <param name="sessionName"></param>
    ''' <param name="upc"></param>
    ''' <param name="itemKey"></param>
    ''' <param name="qty"></param>
    ''' <param name="retailUom"></param>
    ''' <param name="desc"></param>
    ''' <param name="SoldByWeight"></param>
    ''' <param name="createFile"></param>
    ''' <remarks></remarks>
    Public Overloads Sub SaveItem(ByVal sessionName As String, ByVal upc As String, ByVal itemKey As String, ByVal qty As String, ByVal retailUomId As Integer, ByVal retailUom As String, ByVal vendorPack As String, ByVal vendorCost As Double, ByVal adjustedCost As Double, ByVal adjustedReason As Integer, ByVal totalCost As Double, ByVal desc As String, ByVal SoldByWeight As Boolean, ByVal createFile As Boolean)
        ' cbw is a new field: CostedByWeight. 
        If ((sessionUtility.IsEmpty(sessionName) = False) And createFile = False) Then
            'get file and save listbox contents and update xml file
            UpdateTempXmlFile(upc, itemKey, qty, retailUomId, retailUom, vendorPack, vendorCost, adjustedCost, adjustedReason, totalCost, desc, SoldByWeight)
        Else
            WriteTempXmlFile(sessionName)
            UpdateTempXmlFile(upc, itemKey, qty, retailUomId, retailUom, vendorPack, vendorCost, adjustedCost, adjustedReason, totalCost, desc, SoldByWeight)
        End If
    End Sub

    ''' <summary>
    ''' Updates only xml's LineItems node when a upc is added
    ''' </summary>
    ''' <param name="upc"></param>
    ''' <param name="itemKey"></param>
    ''' <param name="qty"></param>
    ''' <param name="retailUom"></param>
    ''' <param name="desc"></param>
    ''' <remarks></remarks>
    Public Overloads Sub UpdateTempXmlFile(ByVal upc As String, ByVal itemKey As String, ByVal qty As Double, ByVal retailUomId As Integer, ByVal retailUom As String, ByVal vendorPack As String, ByVal vendorCost As Double, ByVal adjustedCost As Double, ByVal adjustedReason As Integer, ByVal totalCost As Double, ByVal desc As String, ByVal sbw As Boolean)

        'load the XML file
        Dim docXML As XDocument = New XDocument()
        docXML = XDocument.Load(MakeFilePath(session.SessionName))
        Dim transfer As XElement = docXML.Root

        'check if UPC already saved

        Dim xmlTree As XElement = _
            <Items><%= From el As XElement In docXML.Elements().<Items>.<Item> _
                       Where el.Name.LocalName() = "Item" And el.Attribute("UPC") <> upc _
                       Select el %>
            </Items>


        Dim item As New XElement("Item", _
            New XAttribute("UPC", upc), _
            New XAttribute("ITEM_KEY", itemKey), _
            New XAttribute("QTY", qty), _
            New XAttribute("TOTAL_COST", totalCost), _
            New XAttribute("RETAIL_UOMID", retailUomId), _
            New XAttribute("RETAIL_UOM", retailUom), _
            New XAttribute("VENDOR_PACK", vendorPack), _
            New XAttribute("VENDOR_COST", vendorCost), _
            New XAttribute("ADJUSTED_COST", adjustedCost), _
            New XAttribute("ADJUSTED_REASON", adjustedReason), _
            New XAttribute("DESC", desc), _
            New XAttribute("SOLD_BY_WEIGHT", sbw))

        xmlTree.Add(item)

        transfer.Element("Items").Remove()
        transfer.Add(xmlTree)

        docXML.Save(MakeFilePath(session.SessionName))

    End Sub

    ''' <summary>
    ''' Creates the order file on initial add of item
    ''' </summary>
    ''' <param name="sessionName"></param>
    ''' <remarks></remarks>
    Public Overrides Sub WriteTempXmlFile(ByVal sessionName As String)

        If (Not Directory.Exists(GetDirectoryPath())) Then
            Directory.CreateDirectory(GetDirectoryPath())
        End If

        Dim XmlFileName As String = MakeFilePath(sessionName)

        Dim transfer As XElement = New XElement("OrderHeader", _
                New XElement("FromStore_VendorID", session.transferVendorId), _
                New XElement("FromStore_No", session.TransferFromStoreNo), _
                New XElement("FromStore_Name", session.TransferFromStoreName), _
                New XElement("ProductType_ID", session.SelectedProductType), _
                New XElement("ToStore_VendorID", session.TransferToStoreNo), _
                New XElement("ToStore_Name", session.TransferToStoreName), _
                New XElement("TransferSubTeam", session.TransferFromSubteamKey), _
                New XElement("TransferSubTeamName", session.TransferFromSubteam), _
                New XElement("TransferToSubTeam", session.TransferToSubteamKey), _
                New XElement("TransferToSubTeamName", session.TransferToSubteam), _
                New XElement("ExpectedDate", session.TransferExpectedDate), _
                New XElement("CreatedBy", session.UserID), _
                New XElement("SessionId", session.SessionName), _
                New XElement("Items"))

        Dim transferSession As XDocument = New XDocument(New XDeclaration("1.0", "UTF-8", "yes"), transfer)

        transferSession.Save(XmlFileName)


    End Sub

    ''' <summary>
    ''' Gets transfer file directory path.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Overrides Function GetDirectoryPath() As String

        Dim path As String = Environment.GetFolderPath(Environment.SpecialFolder.Personal) & "\" & TRANSFER_DIR_NAME

        Return (path)

    End Function

    ''' <summary>
    ''' Creates the session transfer file name
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Overrides Function GenerateSessionName() As String

        Dim myName As String = Me.session.TransferFromStoreName.Trim & "_" & _
                                Me.session.Subteam.Replace("/", "").Replace("\", "").Trim & "_" & _
                                Me.session.TransferToStoreName.Trim & "_" & _
                                Me.session.TransferToSubteam.Trim & "_" & _
                                Me.session.UserName.Replace(".", "").Trim

        Return myName

    End Function

    ''' <summary>
    ''' Populates a transfer session from XML file contents.
    ''' </summary>
    ''' <param name="filename"></param>
    ''' <param name="session"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetFileSession(ByVal filename As String, ByVal session As Session) As Session

        Dim str As String

        Dim docXML As XElement = XElement.Load(filename)
        Dim childList As IEnumerable(Of XElement) = From el In docXML.Elements() _
                                                    Select el

        For Each el As XElement In childList

            str = el.Name.LocalName()
            If (str.Equals("FromStore_VendorID")) Then
                session.transferVendorId = el.Value
            ElseIf (str.Equals("FromStore_No")) Then
                session.TransferFromStoreNo = el.Value
            ElseIf (str.Equals("FromStore_Name")) Then
                session.TransferFromStoreName = el.Value
            ElseIf (str.Equals("ProductType_ID")) Then
                session.SelectedProductType = el.Value
            ElseIf (str.Equals("ToStore_VendorID")) Then
                session.TransferToStoreNo = el.Value
            ElseIf (str.Equals("ToStore_Name")) Then
                session.TransferToStoreName = el.Value
            ElseIf (str.Equals("TransferSubTeam")) Then
                session.TransferFromSubteamKey = el.Value
            ElseIf (str.Equals("TransferSubTeamName")) Then
                session.TransferFromSubteam = el.Value
            ElseIf (str.Equals("TransferToSubTeam")) Then
                session.TransferToSubteamKey = el.Value
            ElseIf (str.Equals("TransferToSubTeamName")) Then
                session.TransferToSubteam = el.Value
            ElseIf (str.Equals("ExpectedDate")) Then
                session.TransferExpectedDate = el.Value
            ElseIf (str.Equals("CreatedBy")) Then
                session.UserID = el.Value
            ElseIf (str.Equals("SessionId")) Then
                session.SessionName = el.Value
            End If

        Next

        Return session

    End Function

    ''' <summary>
    ''' Checks to see if saved shrink sessions exist.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub DeleteOlderSessions(ByVal sessionKeptDays As Double, ByVal todaysDate As Date)

        If (Directory.Exists(GetDirectoryPath())) Then
            Dim directory As New DirectoryInfo(GetDirectoryPath())
            Dim allFiles As FileInfo() = directory.GetFiles("*.txt")

            For Each File As FileInfo In allFiles
                If File.LastWriteTime < DateAdd(DateInterval.Day, -sessionKeptDays, todaysDate) Then
                    File.Delete()
                End If
            Next
        End If

    End Sub
End Class
