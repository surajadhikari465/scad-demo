Imports System.IO
Imports System.Data


Public Class ReceiveDocumentFileWriter
    Inherits ShrinkFileWriter

    Public ReadOnly ReceiveDocument_DIR_NAME As String = "ReceiveDocument"
    'Public ReadOnly CS As String = "CASE"
    'Public ReadOnly UNIT As String = "UNIT"

    'Private session As Session
    'Private sessionUtility As SessionUtility = New SessionUtility


    Public Sub New(ByVal userSession As Session)
        MyBase.New(userSession)
    End Sub

    ''' <summary>
    ''' Saves a receive document item to the receive document session file.
    ''' </summary>
    ''' <param name="sessionName"></param>
    ''' <param name="upc"></param>
    ''' <param name="itemKey"></param>
    ''' <param name="qty"></param>
    ''' <param name="uom"></param>
    ''' <param name="desc"></param>
    ''' <param name="CostedByWeight"></param>
    ''' <param name="createFile"></param>
    ''' <remarks></remarks>
    Public Overloads Sub SaveItem(ByVal sessionName As String, ByVal upc As String, ByVal itemKey As String, ByVal qty As String, ByVal uom As String, ByVal desc As String, _
                        ByVal CostedByWeight As Boolean, ByVal createFile As Boolean, ByVal qUnit As Integer, _
                        ByVal discountAmount As Double, ByVal discountType As Integer, ByVal discountReasonCode As String)
        ' cbw is a new field <=> CostedByWeight. 
        If ((sessionUtility.IsEmpty(sessionName) = False) And createFile = False) Then
            'get file and save listbox contents and update xml file
            UpdateTempXmlFile(sessionName, upc, itemKey, qty, uom, desc, CostedByWeight, qUnit, discountAmount, discountType, discountReasonCode)

        Else

            WriteTempXmlFile(sessionName)
            UpdateTempXmlFile(sessionName, upc, itemKey, qty, uom, desc, CostedByWeight, qUnit, discountAmount, discountType, discountReasonCode)

        End If

    End Sub

    ''' <summary>
    ''' Updates only xml's items node when a upc is added
    ''' </summary>
    ''' <param name="sessionName"></param>
    ''' <param name="upc"></param>
    ''' <param name="itemKey"></param>
    ''' <param name="qty"></param>
    ''' <param name="uom"></param>
    ''' <param name="desc"></param>
    ''' <remarks></remarks>
    Public Overloads Sub UpdateTempXmlFile( _
            ByVal sessionName As String, ByVal upc As String, ByVal itemKey As String, ByVal qty As Double, ByVal uom As String, ByVal desc As String, ByVal cbw As Boolean, ByVal qUnit As Integer, _
            ByVal discountAmount As Double, ByVal discountType As Integer, ByVal discountReasonCode As String)

        'load the XML file
        Dim docXML As XDocument = New XDocument()
        docXML = XDocument.Load(GetFilePath())
        Dim receiveDocument As XElement = docXML.Root

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
            New XAttribute("UOM", uom), _
            New XAttribute("DESC", desc), _
            New XAttribute("COSTED_BY_WEIGHT", cbw), _
            New XAttribute("QUANTITY_UNIT", qUnit), _
            New XAttribute("DISCOUNT_AMOUNT", discountAmount), _
            New XAttribute("DISCOUNT_TYPE", discountType), _
            New XAttribute("DISCOUNT_REASON_CODE", discountReasonCode))

        'New XAttribute("DISCOUNT_REASON_CODE", discountReasonCode), _
        'New XAttribute("RETURN_ORDER", IIf(session.ActionType = Enums.ActionType.ReceiveDocumentCredit, True, False)))


        xmlTree.Add(item)

        receiveDocument.Element("Items").Remove()
        receiveDocument.Add(xmlTree)

        docXML.Save(GetFilePath())

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

        Dim XmlFileName As String = GetFilePath()

        Dim receiveDocument As XElement = New XElement("ReceiveDocument", _
                New XElement("UserName", session.UserName), _
                New XElement("UserID", session.UserID), _
                New XElement("Store", session.StoreNo), _
                New XElement("StoreName", session.StoreName), _
                New XElement("Subteam", session.SubteamKey), _
                New XElement("SubteamName", session.Subteam), _
                New XElement("SessionId", session.SessionName), _
                New XElement("VendorID", session.DSDVendorID), _
                New XElement("VendorName", session.DSDVendorName), _
                New XElement("InvoiceNum", session.DSDInvoice), _
                New XElement("EffectiveDate", Now()), _
                New XElement("StoreVendorID", session.StoreVendorID), _
                New XElement("Return_Order", IIf(session.ActionType = Enums.ActionType.ReceiveDocumentCredit, True, False)), _
                New XElement("Items"))

        Dim receiveDocuSession As XDocument = New XDocument(New XDeclaration("1.0", "UTF-8", "yes"), receiveDocument)

        receiveDocuSession.Save(XmlFileName)


    End Sub

    ''' <summary>
    ''' Gets receive document file directory path.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Overrides Function GetDirectoryPath() As String

        Dim path As String = Environment.GetFolderPath(Environment.SpecialFolder.Personal) & "\ReceiveDocument"

        Return (path)

    End Function
    ''' <summary>
    ''' Creates the session receiveDocument file name
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Overrides Function GenerateSessionName() As String

        Dim dt As DateTime = Me.session.StartTime

        Dim myName As String = Me.session.StoreName & "_" & _
                                Me.session.Subteam & "_" & _
                                Me.session.DSDVendorName & "_" & _
                                Me.session.DSDInvoice & "_" & _
                                dt.Year.ToString() & _
                                dt.Month.ToString() & _
                                dt.Day.ToString() & "_" & _
                                dt.Hour.ToString() & _
                                dt.Minute.ToString() & _
                                dt.Second.ToString()

        Return myName

    End Function



    ''' <summary>
    ''' Populates a receive document session from XML file contents.
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
            If (str.Equals("Store")) Then
                session.StoreNo = el.Value
            ElseIf (str.Equals("StoreName")) Then
                session.StoreName = el.Value
            ElseIf (str.Equals("UserName")) Then
                session.UserName = el.Value
            ElseIf (str.Equals("UserID")) Then
                session.UserID = el.Value
            ElseIf (str.Equals("Subteam")) Then
                session.SubteamKey = el.Value
            ElseIf (str.Equals("SubteamName")) Then
                session.Subteam = el.Value
            ElseIf (str.Equals("VendorID")) Then
                session.DSDVendorID = el.Value
            ElseIf (str.Equals("VendorName")) Then
                session.DSDVendorName = el.Value
            ElseIf (str.Equals("InvoiceNum")) Then
                session.DSDInvoice = el.Value
            ElseIf (str.Equals("SessionId")) Then
                session.SessionName = el.Value
            ElseIf (str.Equals("StoreVendorID")) Then
                session.StoreVendorID = el.Value
            ElseIf (str.Equals("Return_Order")) Then
                session.ActionType = IIf(el.Value = True, Enums.ActionType.ReceiveDocumentCredit, Enums.ActionType.ReceiveDocument) '//TODO
            End If

        Next

        Return session

    End Function
End Class
