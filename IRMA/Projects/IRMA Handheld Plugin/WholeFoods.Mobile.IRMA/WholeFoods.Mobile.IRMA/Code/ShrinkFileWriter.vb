Imports System.IO
Imports System.Data


Public Class ShrinkFileWriter

    Public ReadOnly SHRINK_DIR_NAME As String = "Shrink"
    Public ReadOnly CS As String = "CASE"
    Public ReadOnly UNIT As String = "UNIT"
    Public PREVIOUS_SESSION As String
    Protected session As Session
    Protected sessionUtility As SessionUtility = New SessionUtility

    Public Sub New(ByVal userSession As Session)
        Me.session = userSession
    End Sub

    ''' <summary>
    ''' Saves a shrink item to the shrink session file.
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
    Public Sub SaveItem(ByVal sessionName As String, ByVal upc As String, ByVal itemKey As String, ByVal qty As String, ByVal uom As String, ByVal desc As String, ByVal CostedByWeight As Boolean, _
                        ByVal createFile As Boolean, ByVal shrinkSubTypeID As Integer, ByVal shrinkSubType As String, _
                        ByVal shrinkAdjId As Integer, ByVal shrinkTypeId As String, ByVal shrinkType As String)
        ' cbw is a new field <=> CostedByWeight. 
        If ((sessionUtility.IsEmpty(sessionName) = False) And createFile = False) Then
            'get file and save listbox contents and update xml file
            If (session.hasSubTypeUpdated) Then
                ReplaceShrinkInfo(upc, session.ShrinkTypeId, session.ShrinkType, session.ShrinkAdjId, session.InventoryAdjustmentCodeID, shrinkSubTypeID, shrinkSubType)
                session.hasSubTypeUpdated = False

            End If

            UpdateTempXmlFile(sessionName, upc, itemKey, qty, uom, desc, CostedByWeight, shrinkSubTypeID, shrinkSubType, shrinkAdjId, shrinkTypeId, ShrinkType, Nothing)
            ' ReplaceShrinkInfo(upc,session.ShrinkTypeId,session.ShrinkType,session.ShrinkAdjId,

        Else
            WriteTempXmlFile(sessionName)
            UpdateTempXmlFile(sessionName, upc, itemKey, qty, uom, desc, CostedByWeight, shrinkSubTypeID, shrinkSubType, shrinkAdjId, shrinkTypeId, ShrinkType, Nothing)
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
    Public Sub UpdateTempXmlFile(ByVal sessionName As String, ByVal upc As String, ByVal itemKey As String, ByVal qty As Double, _
                                 ByVal uom As String, ByVal desc As String, ByVal cbw As Boolean, ByVal shrinkSubTypeID As Integer, _
                                 ByVal shrinkSubType As String, ByVal shrinkAdjId As Integer, _
                                 ByVal shrinkTypeId As String, ByVal shrinkType As String, ByVal inventoryAdjustmentCodeID As Integer)

        'load the XML file
        Dim docXML As XDocument = New XDocument()
        docXML = XDocument.Load(MakeFilePath(sessionName))
        Dim shrink As XElement = docXML.Root

        'check if UPC already saved

        Dim xmlTree As XElement = _
               <Items><%= From el As XElement In docXML.Elements().<Items>.<Item> _
                          Where el.Name.LocalName() = "Item" And (el.Attribute("UPC") <> upc Or (el.Attribute("UPC") = upc And el.Attribute("SHRINK_SUB_TYPE_ID") <> shrinkSubTypeID.ToString)) _
                          Select el %>
               </Items>


        Dim item As New XElement("Item", _
            New XAttribute("UPC", upc), _
            New XAttribute("ITEM_KEY", itemKey), _
            New XAttribute("QTY", qty), _
            New XAttribute("UOM", uom), _
            New XAttribute("DESC", desc), _
            New XAttribute("COSTED_BY_WEIGHT", cbw), _
            New XAttribute("SHRINK_SUB_TYPE", shrinkSubType), _
            New XAttribute("SHRINK_SUB_TYPE_ID", shrinkSubTypeID), _
            New XAttribute("SHRINK_ADJ_ID", shrinkAdjId), _
            New XAttribute("SHRINK_TYPE_ID", shrinkTypeId), _
            New XAttribute("SHRINK_TYPE", shrinkType))
        xmlTree.Add(item)

        shrink.Element("Items").Remove()
        shrink.Add(xmlTree)

        docXML.Save(MakeFilePath(sessionName))

    End Sub

    Public Sub ReplaceShrinkSubTypeId(ByVal upc As String, ByVal shrinkSubTypeId As String, ByVal oldShrinkSubTypeId As String)

        'load the XML file
        Dim docXML As XDocument = New XDocument()
        docXML = XDocument.Load(MakeFilePath(session.SessionName))
        Dim shrink As XElement = docXML.Root

        Dim isUpdating As Boolean = False

        Dim xmlTreeRemainingItems As XElement = _
        <Items><%= From el As XElement In docXML.Elements().<Items>.<Item> _
                   Where el.Name.LocalName() = "Item" And (el.Attribute("UPC") <> upc Or (el.Attribute("UPC") = upc And el.Attribute("SHRINK_SUB_TYPE_ID") <> oldShrinkSubTypeId)) _
                   Select el %>
        </Items>

        Dim xmlTree As XElement = _
               <Items><%= From el As XElement In docXML.Elements().<Items>.<Item> _
                          Where el.Name.LocalName() = "Item" And el.Attribute("UPC") = upc And el.Attribute("SHRINK_SUB_TYPE_ID") = oldShrinkSubTypeId _
                          Select el %>
               </Items>

        If xmlTree.Elements.Count > 0 Then
            If xmlTree.Element("Item").Attribute("UPC").Value = upc Then
                xmlTree.Element("Item").Attribute("SHRINK_SUB_TYPE_ID").SetValue(shrinkSubTypeId)
                isUpdating = True
            End If
        End If
        If isUpdating Then
            Dim updatedItem As XElement = xmlTree.Element("Item")
            xmlTreeRemainingItems.Add(updatedItem)

            shrink.Element("Items").Remove()
            shrink.Add(xmlTreeRemainingItems)

            docXML.Save(MakeFilePath(session.SessionName))
        End If
    End Sub

    Public Sub ReplaceShrinkInfo(ByVal upc As String, ByVal shrinkTypeId As String, ByVal shrinkType As String, ByVal shrinkTypeAdjId As Integer, _
                                 ByVal inventoryAdjustmentCodeID As Integer, ByVal shrinkSubTypeId As Integer, ByVal shrinkSubType As String)

        Dim docXML As XDocument = New XDocument()

        docXML = XDocument.Load(MakeFilePath(session.SessionName))
        Dim shrink As XElement = docXML.Root

        Dim shrinkSubTypeIdFromXml As XElement = shrink.Elements("ShrinkSubTypeID").First
        Dim shrinkSubTypeFromXml As XElement = shrink.Elements("ShrinkSubType").First
        Dim inventoryAdjustmentCodeIDFromXml As XElement = shrink.Elements("InventoryAdjustmentCodeID").First
        Dim shrinkTypeAdjIdFromXml As XElement = shrink.Elements("ShrinkTypeAdjId").First
        Dim shrinkTypeIdFromXml As XElement = shrink.Elements("ShrinkTypeId").First
        Dim shrinkTypeFromXml As XElement = shrink.Elements("ShrinkType").First

        shrinkSubTypeIdFromXml.Value = shrinkSubTypeId
        shrink.Elements("ShrinkSubTypeID").Remove()
        shrink.Add(shrinkSubTypeIdFromXml)

        shrinkSubTypeFromXml.Value = shrinkSubType
        shrink.Elements("ShrinkSubType").Remove()
        shrink.Add(shrinkSubTypeFromXml)

        inventoryAdjustmentCodeIDFromXml.Value = inventoryAdjustmentCodeID
        shrink.Elements("InventoryAdjustmentCodeID").Remove()
        shrink.Add(inventoryAdjustmentCodeIDFromXml)

        shrinkTypeAdjIdFromXml.Value = shrinkTypeAdjId
        shrink.Elements("ShrinkTypeAdjId").Remove()
        shrink.Add(shrinkTypeAdjIdFromXml)

        shrinkTypeIdFromXml.Value = shrinkTypeId
        shrink.Elements("ShrinkTypeId").Remove()
        shrink.Add(shrinkTypeIdFromXml)

        shrinkTypeFromXml.Value = shrinkType
        shrink.Elements("ShrinkType").Remove()
        shrink.Add(shrinkTypeFromXml)

        docXML.Save(MakeFilePath(session.SessionName))

    End Sub

    Public Sub ReplaceShrinkSubType(ByVal upc As String, ByVal shrinkSubType As String, _
                                    ByVal shrinkAdjId As Integer, ByVal shrinkTypeId As String, ByVal shrinkType As String, ByVal shrinkSubtypeId As String)

        'load the XML file
        Dim docXML As XDocument = New XDocument()
        docXML = XDocument.Load(MakeFilePath(session.SessionName))
        Dim shrink As XElement = docXML.Root

        Dim isUpdating As Boolean = False

        Dim xmlTreeRemainingItems As XElement = _
        <Items><%= From el As XElement In docXML.Elements().<Items>.<Item> _
                   Where el.Name.LocalName() = "Item" And (el.Attribute("UPC") <> upc Or (el.Attribute("UPC") = upc And el.Attribute("SHRINK_SUB_TYPE_ID") <> shrinkSubtypeId)) _
                   Select el %>
        </Items>

        Dim xmlTree As XElement = _
               <Items><%= From el As XElement In docXML.Elements().<Items>.<Item> _
                          Where el.Name.LocalName() = "Item" And el.Attribute("UPC") = upc And el.Attribute("SHRINK_SUB_TYPE_ID") = shrinkSubtypeId _
                          Select el %>
               </Items>

        If xmlTree.Elements.Count > 0 Then
            If xmlTree.Element("Item").Attribute("UPC").Value = upc Then
                xmlTree.Element("Item").Attribute("SHRINK_SUB_TYPE").SetValue(shrinkSubType)
                xmlTree.Element("Item").Attribute("SHRINK_ADJ_ID").SetValue(shrinkAdjId)
                xmlTree.Element("Item").Attribute("SHRINK_TYPE_ID").SetValue(shrinkTypeId)
                xmlTree.Element("Item").Attribute("SHRINK_TYPE").SetValue(shrinkType)
                isUpdating = True
            End If
        End If
        If isUpdating Then

            Dim updatedItem As XElement = xmlTree.Element("Item")
            xmlTreeRemainingItems.Add(updatedItem)

            shrink.Element("Items").Remove()
            shrink.Add(xmlTreeRemainingItems)

            docXML.Save(MakeFilePath(session.SessionName))
        End If

    End Sub


    Public Sub ReplaceQuantityBasedOnType(ByVal upc As String, ByVal qty As String, ByVal oldShrinkSubTypeId As String)

        'load the XML file
        Dim docXML As XDocument = New XDocument()
        docXML = XDocument.Load(MakeFilePath(session.SessionName))
        Dim shrink As XElement = docXML.Root

        Dim isUpdating As Boolean = False

        Dim xmlTreeRemainingItems As XElement = _
        <Items><%= From el As XElement In docXML.Elements().<Items>.<Item> _
                   Where el.Name.LocalName() = "Item" And (el.Attribute("UPC") <> upc Or (el.Attribute("UPC") = upc And el.Attribute("SHRINK_SUB_TYPE_ID") <> oldShrinkSubTypeId)) _
                   Select el %>
        </Items>

        Dim xmlTree As XElement = _
               <Items><%= From el As XElement In docXML.Elements().<Items>.<Item> _
                          Where el.Name.LocalName() = "Item" And el.Attribute("UPC") = upc And el.Attribute("SHRINK_SUB_TYPE_ID") = oldShrinkSubTypeId _
                          Select el %>
               </Items>

        If xmlTree.Elements.Count > 0 Then
            If xmlTree.Element("Item").Attribute("UPC").Value = upc Then
                Dim sDiscountType As String = CStr(xmlTree.Element("Item").Attribute("DISCOUNT_TYPE"))
                If Not sDiscountType Is Nothing Then
                    If xmlTree.Element("Item").Attribute("DISCOUNT_TYPE").Value = 3 Then xmlTree.Element("Item").Attribute("DISCOUNT_AMOUNT").SetValue(qty)
                End If
                xmlTree.Element("Item").Attribute("QTY").SetValue(qty)
                isUpdating = True
            End If
        End If

        If isUpdating Then
            Dim updatedItem As XElement = xmlTree.Element("Item")
            xmlTreeRemainingItems.Add(updatedItem)

            shrink.Element("Items").Remove()
            shrink.Add(xmlTreeRemainingItems)

            docXML.Save(MakeFilePath(session.SessionName))
        End If
    End Sub

    Public Sub ReplaceQuantity(ByVal upc As String, ByVal qty As String)

        'load the XML file
        Dim docXML As XDocument = New XDocument()
        docXML = XDocument.Load(MakeFilePath(session.SessionName))
        Dim shrink As XElement = docXML.Root

        Dim isUpdating As Boolean = False

        Dim xmlTreeRemainingItems As XElement = _
        <Items><%= From el As XElement In docXML.Elements().<Items>.<Item> _
                   Where el.Name.LocalName() = "Item" And el.Attribute("UPC") <> upc _
                   Select el %>
        </Items>

        Dim xmlTree As XElement = _
               <Items><%= From el As XElement In docXML.Elements().<Items>.<Item> _
                          Where el.Name.LocalName() = "Item" And el.Attribute("UPC") = upc _
                          Select el %>
               </Items>

        If xmlTree.Elements.Count > 0 Then
            If xmlTree.Element("Item").Attribute("UPC").Value = upc Then
                Dim sDiscountType As String = CStr(xmlTree.Element("Item").Attribute("DISCOUNT_TYPE"))
                If Not sDiscountType Is Nothing Then
                    If xmlTree.Element("Item").Attribute("DISCOUNT_TYPE").Value = 3 Then xmlTree.Element("Item").Attribute("DISCOUNT_AMOUNT").SetValue(qty)
                End If
                xmlTree.Element("Item").Attribute("QTY").SetValue(qty)
                isUpdating = True
            End If
        End If

        If isUpdating Then
            Dim updatedItem As XElement = xmlTree.Element("Item")
            xmlTreeRemainingItems.Add(updatedItem)

            shrink.Element("Items").Remove()
            shrink.Add(xmlTreeRemainingItems)

            docXML.Save(MakeFilePath(session.SessionName))
        End If
    End Sub

    ''' <summary>
    ''' Creates the order file on initial add of item
    ''' </summary>
    ''' <param name="sessionName"></param>
    ''' <remarks></remarks>
    Public Overridable Sub WriteTempXmlFile(ByVal sessionName As String)

        If (Not Directory.Exists(GetDirectoryPath())) Then
            Directory.CreateDirectory(GetDirectoryPath())
        End If

        Dim XmlFileName As String = MakeFilePath(sessionName)

        Dim shrink As XElement = New XElement("Shrink", _
                New XElement("UserName", session.UserName), _
                New XElement("UserID", session.UserID), _
                New XElement("Store", session.StoreNo), _
                New XElement("StoreName", session.StoreName), _
                New XElement("Subteam", session.SubteamKey), _
                New XElement("SubteamName", session.Subteam), _
                New XElement("SessionId", session.SessionName), _
                New XElement("ShrinkType", session.ShrinkType), _
                New XElement("ShrinkTypeId", session.ShrinkTypeId), _
                New XElement("ShrinkTypeAdjId", session.ShrinkAdjId), _
                New XElement("ShrinkSubTypeID", session.ShrinkSubTypeID), _
                New XElement("InventoryAdjustmentCodeID", session.InventoryAdjustmentCodeID), _
                New XElement("ShrinkSubType", session.ShrinkSubType), _
                New XElement("Items"))

        Dim shrinkSession As XDocument = New XDocument(New XDeclaration("1.0", "UTF-8", "yes"), shrink)

        shrinkSession.Save(XmlFileName)


    End Sub

    ''' <summary>
    ''' Gets shrink file directory path.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Overridable Function GetDirectoryPath() As String

        Dim path As String = Environment.GetFolderPath(Environment.SpecialFolder.Personal) & "\" & SHRINK_DIR_NAME

        Return (path)

    End Function

    ''' <summary>
    ''' Gets shrink file path.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function GetFilePath() As String
        'this function should probably be deprecated in the receivedoc module and use MakeFilePath(Me.mySession.SessionName) instead
        Dim filename As String
        filename = session.SessionName.Replace("/"c, "_"c)                     'TFS 6606, 06/26/2012, Faisal Ahmed
        Dim path As String = GetDirectoryPath() & "\" & filename & ".txt"
        Return path

    End Function

    Function MakeFilePath(ByVal filename As String) As String
        filename = filename.Replace("/"c, "_"c)
        Dim path As String = GetDirectoryPath() & "\" & filename & ".txt"
        Return path
    End Function

    ''' <summary>
    ''' GenerateSessionName
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Overridable Function GenerateSessionName() As String

        Dim myName As String = Me.session.StoreName.Trim & "_" & _
                                Me.session.Subteam.Replace("/", "").Replace("\", "").Trim & "_" & _
                                Me.session.UserName.Replace(".", "").Trim

        Return myName

    End Function

    ''' <summary>
    ''' Removes specified upc from file
    ''' </summary>
    ''' <param name="sessionName"></param>
    ''' <param name="upc"></param>
    ''' <remarks></remarks>
    Public Sub DeleteItem(ByVal sessionName As String, ByVal upc As String)

        DeleteItemFromXmlFile(sessionName, upc)

    End Sub

    ''' <summary>
    ''' Deletes session file, this is called from finish command on success
    ''' </summary>
    ''' <param name="path"></param>
    ''' <remarks></remarks>
    Public Sub DeleteFile(ByVal path As String)

        File.Delete(path)

    End Sub

    ''' <summary>
    ''' Deletes only the specified upc item from xml file
    ''' </summary>
    ''' <param name="sessionName"></param>
    ''' <param name="upc"></param>
    ''' <remarks></remarks>
    Private Sub DeleteItemFromXmlFile(ByVal sessionName As String, ByVal upc As String)

        'load the XML file
        Dim docXML As XDocument = New XDocument()
        docXML = XDocument.Load(MakeFilePath(sessionName))
        Dim shrink As XElement = docXML.Root

        'check if UPC already saved

        Dim xmlTree As XElement = _
               <Items><%= From el As XElement In docXML.Elements().<Items>.<Item> _
                          Where el.Name.LocalName() = "Item" And el.Attribute("UPC") <> upc _
                          Select el %>
               </Items>


        shrink.Element("Items").Remove()
        shrink.Add(xmlTree)


        docXML.Save(MakeFilePath(sessionName))

    End Sub

    ''' <summary>
    ''' Gets the list of items in the current shrink session.
    ''' </summary>
    ''' <param name="myDataList"></param>
    ''' <param name="sessionName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetShrinkList(ByRef myDataList As List(Of XNode), ByVal sessionName As String)

        Try

            'load the XML file
            Dim docXML As XDocument = New XDocument()
            docXML = XDocument.Load(MakeFilePath(sessionName))

            Dim xmlTree As XElement = _
                  <Items><%= From el As XElement In docXML.Elements().<Items>.<Item> _
                             Where el.Name.LocalName() = "Item" _
                             Select el %>
                  </Items>

            myDataList = xmlTree.Nodes.ToList()

        Catch ex As Exception

        End Try

        Return myDataList

    End Function

    ''' <summary>
    ''' Populates a shrink session from XML file contents.
    ''' </summary>
    ''' <param name="filename"></param>
    ''' <param name="session"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overridable Function GetFileSession(ByVal filename As String, ByVal session As Session) As Session

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
            ElseIf (str.Equals("ShrinkType")) Then
                session.ShrinkType = el.Value
            ElseIf (str.Equals("ShrinkTypeId")) Then
                session.ShrinkTypeId = el.Value
            ElseIf (str.Equals("ShrinkTypeAdjId")) Then
                session.ShrinkAdjId = el.Value
            ElseIf (str.Equals("SessionId")) Then
                session.SessionName = el.Value
            ElseIf (str.Equals("InventoryAdjustmentCodeID")) Then
                session.InventoryAdjustmentCodeID = el.Value
            ElseIf (str.Equals("ShrinkSubType")) Then
                session.ShrinkSubType = el.Value
            ElseIf (str.Equals("ShrinkSubTypeID")) Then
                session.ShrinkSubTypeID = el.Value
                End If

        Next

        Return session
    End Function

    ''' <summary>
    ''' Checks to see if saved shrink sessions exist.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SavedSessionExists() As Boolean

        If (Directory.Exists(GetDirectoryPath())) Then
            Dim userName As String = session.UserName.Replace(".", "")
            Dim file As String() = Directory.GetFiles(GetDirectoryPath(), "*" + userName + ".txt")

            If (file.Length = 1) Then
                Dim param As Char() = {"\\", "."}
                Dim fullPath As String() = file(0).Split(param)
                PREVIOUS_SESSION = fullPath(3)
                Return True
            Else
                Return False
            End If

        End If

    End Function

    ''' <summary>
    ''' Checks to see if a current shrink session file exists.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SessionFileExists() As Boolean
        Try
            Dim fileExists As Boolean = File.Exists(MakeFilePath(session.SessionName))
            Return fileExists
        Catch ex As Exception
            Return False
        End Try

    End Function

    ''' <summary>
    ''' Returns a list of sesssion items in a hashtable.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetScannedItemHash() As Hashtable

        Dim myHashTable As Hashtable = New Hashtable()
        Try
            Dim myDataList As List(Of XNode)

            'load the XML file
            Dim docXML As XDocument = New XDocument()
            docXML = XDocument.Load(MakeFilePath(session.SessionName))

            Dim xmlTree As XElement = _
                  <Items><%= From el As XElement In docXML.Elements().<Items>.<Item> _
                             Where el.Name.LocalName() = "Item" _
                             Select el %>
                  </Items>

            myDataList = xmlTree.Nodes.ToList()

            'loop thru list
            Dim tmpUpcStr, tmpQtyStr As String
            Dim tmpUpc, tmpQty As String()
            Dim charArray As Char() = {"/"c}

            For Each item As XElement In myDataList
                tmpUpcStr = item.Attribute("UPC").ToString()
                tmpUpcStr = tmpUpcStr.Replace("""", "/")
                tmpUpc = tmpUpcStr.Split(charArray)
                tmpQtyStr = item.Attribute("QTY").ToString()
                tmpQtyStr = tmpQtyStr.Replace("""", "/")
                tmpQty = tmpQtyStr.Split(charArray)
                myHashTable.Add(tmpUpc(1), Integer.Parse(tmpQty(1)))
            Next

        Catch ex As Exception

        End Try

        Return myHashTable

    End Function

    Public Function isPreviouslyScanned(ByVal upc As String, ByVal shrinkSubTypeID As String) As Boolean

        'load the XML file
        Dim docXML As XDocument = New XDocument()
        docXML = XDocument.Load(MakeFilePath(session.SessionName))
        Dim shrink As XElement = docXML.Root

        Dim xmlTree As XElement = _
               <Items><%= From el As XElement In docXML.Elements().<Items>.<Item> _
                          Where el.Name.LocalName() = "Item" And el.Attribute("UPC") = upc And el.Attribute("SHRINK_SUB_TYPE_ID") = shrinkSubTypeID _
                          Select el %>
               </Items>

        If xmlTree.Elements.Count > 0 Then
            If xmlTree.Element("Item").Attribute("UPC").Value = upc Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If

    End Function
    Public Function isPreviouslyScanned(ByVal upc As String) As Boolean

        'load the XML file
        Dim docXML As XDocument = New XDocument()
        docXML = XDocument.Load(MakeFilePath(session.SessionName))
        Dim shrink As XElement = docXML.Root

        Dim xmlTree As XElement = _
               <Items><%= From el As XElement In docXML.Elements().<Items>.<Item> _
                          Where el.Name.LocalName() = "Item" And el.Attribute("UPC") = upc _
                          Select el %>
               </Items>

        If xmlTree.Elements.Count > 0 Then
            If xmlTree.Element("Item").Attribute("UPC").Value = upc Then
                Return True
            Else
                Return False
            End If
        Else
            Return False
        End If

    End Function
End Class