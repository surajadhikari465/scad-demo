Imports System.Xml
Imports System.Deployment.Application
Imports Microsoft.Build.Tasks.Deployment
Imports System.IO
Imports System.Data.SqlClient
Imports System.Security.Cryptography
Imports System.Net

Partial Public Class _Default
    Inherits System.Web.UI.Page

    Dim blnIRMAClientUsed As Boolean = False
    Dim blnPOETUsed As Boolean = False
    Dim blnPromoPlannerUsed As Boolean = False
    Dim blnReportManagerUsed As Boolean = False
    Dim blnSLIMUsed As Boolean = False
    Dim blnStoreOpsUsed As Boolean = False
    Dim blnStoreOrderGuideUsed As Boolean = False
    Dim sIRMAClientVersion As String = String.Empty
    Dim sSystemVersion As String = String.Empty
    Dim SqlConn As SqlConnection = New SqlConnection("Data Source=idt-ce\ced;Initial Catalog=BuildConfiguration;User Id=BuildConfigUser;Password=BuildConfigUser;")

    Private Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        lblServer.Text = "Server: " & Dns.GetHostName.ToString
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        button_generagelinks.Attributes.Add("onClick", "return GenerateLinks();")
        cboVersion.Attributes.Add("onChange", "return InvalidateLinks();")

        SqlConn.Open()

        If Not Page.IsPostBack Then
            pnlLinks.Visible = False
            pnlNoConfig.Visible = False
            LoadCombos()
        End If
    End Sub

    Private Sub LoadVersion()
        Dim Data As DataSet = New DataSet
        Dim da As SqlDataAdapter = New SqlDataAdapter("select * from Regions", SqlConn)
        Dim sql As String = String.Empty

        sql = String.Format("select version from ValidConfigurations where Environment = '{0}' and Region = '{1}'", cboEnvironment.SelectedValue.ToString(), cboRegion.SelectedValue.ToString())

        If Not cboEnvironment.SelectedValue.Equals(String.Empty) And Not cboRegion.SelectedValue.Equals(String.Empty) Then
            cboVersion.Items.Clear()
            da.SelectCommand = New SqlCommand(sql, SqlConn)
            da.Fill(Data, "Versions")

            If Data.Tables("Versions").Rows.Count > 0 Then
                For Each dr As DataRow In Data.Tables("Versions").Rows
                    cboVersion.Items.Add(New ListItem(dr("Version").ToString(), dr("Version").ToString()))
                Next
                cboVersion.Items.Insert(0, New ListItem("", ""))
            End If
        End If

        da.Dispose()
        Data.Dispose()

        pnlLinks.Visible = False
        pnlNoConfig.Visible = False
    End Sub

    Public Function GetServerName(ByVal sEnvironment As String) As String
        Dim sTestServers As String = ConfigurationManager.AppSettings.Get("TestServers")
        Dim sQAServers As String = ConfigurationManager.AppSettings.Get("QAServers")
        Dim sProdServers As String = ConfigurationManager.AppSettings.Get("ProdServers")
        Dim blnIsDev As Boolean = False

        blnIsDev = CBool(Mid(cboVersion.SelectedValue, InStr(cboVersion.SelectedValue, "^") + 1))

        If blnIsDev Then
            Select Case sEnvironment.ToUpper
                Case "TEST"
                    Return Mid(sTestServers, InStr(sTestServers, "|") + 1)
                Case "QA"
                    Return Mid(sQAServers, InStr(sQAServers, "|") + 1)
                Case Else
                    Return sProdServers
            End Select
        Else
            Select Case sEnvironment.ToUpper
                Case "TEST"
                    Return Mid(sTestServers, 1, InStr(sTestServers, "|") - 1)
                Case "QA"
                    Return Mid(sQAServers, 1, InStr(sQAServers, "|") - 1)
                Case Else
                    Return sProdServers
            End Select
        End If
    End Function

    Private Function GetManifestVersion(ByVal sPath As String) As String
        Dim _mr As ManifestUtilities.Manifest = Nothing

        Try
            _mr = ManifestUtilities.ManifestReader.ReadManifest(sPath, False)
            Dim _ai As ManifestUtilities.AssemblyIdentity = _mr.AssemblyIdentity
            Return Mid(_ai.Version, 1, InStrRev(_ai.Version, ".") - 1)
        Catch ex As Exception
            Return "Not Found"
        End Try
    End Function

    Private Function GetLastWriteTime(ByVal sPath As String) As String
        If Not File.Exists(sPath) Then
            Return "Application Not Installed"
        Else
            Return "Last Updated: " & My.Computer.FileSystem.GetFileInfo(sPath).LastWriteTime
        End If
    End Function

    Private Sub SetURL(ByVal sURL As String, ByVal blnIsUsedByRegion As Boolean, ByVal sApplication As String, ByVal lnkCtrl As HyperLink)
        lnkCtrl.Enabled = blnIsUsedByRegion
        lnkCtrl.NavigateUrl = sURL
        lnkCtrl.ForeColor = IIf(blnIsUsedByRegion, Drawing.Color.Blue, Drawing.Color.Black)
    End Sub

    Private Function GetApplicationURL(ByVal sApplicationName As String, ByVal AppServer As String, ByVal WebAppSuffix As String) As String
        Dim sURL As String = String.Empty
        Dim blnIsDev As Boolean = False

        If (AppServer.Contains("iat") Or AppServer.Contains("iax")) Then blnIsDev = True

        Select Case sApplicationName.ToUpper
            Case "IRMA CLIENT"
                If blnIsDev Then
                    sURL = "http://" & AppServer & "/deployment/" & cboRegion.SelectedValue.ToUpper & "/IRMA_Dev/IRMA.application"
                Else
                    sURL = "http://" & AppServer & "/deployment/" & cboRegion.SelectedValue.ToUpper & "/IRMA/IRMA.application"
                End If
            Case "DVO"
                Select Case cboEnvironment.SelectedItem.Text.ToUpper
                    Case "TEST"
                        sURL = "http://dvotst7/dvo-tst/Login.do"
                    Case "QA"
                        sURL = "http://dvo-qa.wholefoods.com/dvo-qa"
                    Case Else
                        sURL = "http://dvo"
                End Select
            Case "POET"
                Select Case cboEnvironment.SelectedItem.Text.ToUpper
                    Case "TEST"
                        sURL = "http://poet_test"
                    Case "QA"
                        sURL = "http://poet_qa"
                    Case Else
                        sURL = "http://poet"
                End Select
            Case "PROMO PLANNER"
                sURL = String.Format("http://{0}/PromoPlanner{1}", AppServer, WebAppSuffix)
            Case "REPORT MANAGER"
                sURL = String.Format("http://{0}/ReportManager{1}", AppServer, WebAppSuffix)
            Case "SLIM"
                sURL = String.Format("http://{0}/SLIM{1}", AppServer, WebAppSuffix)
            Case "STORE OPS"
                sURL = "http://" & AppServer & "/StoreOps"
                If AppServer.ToLower.Contains("iax") Then
                    sURL = "http://" & AppServer.ToLower.Replace("iax", "iaq") & "/StoreOps"
                End If
            Case "STORE ORDER GUIDE"
                sURL = "http://" & AppServer & "/StoreOrderGuide"
        End Select

        Return sURL
    End Function

    Public Function Encrypt(ByVal encryptValue As String) As String
        Dim encryptedString As String = ""
        Dim rijndael As New RijndaelManaged()
        Dim rijndaelEncrypt As ICryptoTransform = Nothing
        Dim encryptStream As CryptoStream = Nothing
        Dim memStream As New MemoryStream()
        Dim _key(23) As Byte
        Dim _IV(15) As Byte

        Try
            If encryptValue.Length > 0 Then
                ' Create the crypto objects
                rijndael.Key = _key
                rijndael.IV = _IV
                rijndaelEncrypt = rijndael.CreateEncryptor()
                encryptStream = New CryptoStream(memStream, rijndaelEncrypt, CryptoStreamMode.Write)

                ' Write the encrytped value to memory
                Dim input As Byte() = Encoding.UTF8.GetBytes(encryptValue)
                encryptStream.Write(input, 0, input.Length)
                encryptStream.FlushFinalBlock()

                ' Retrieve the encrypted value to return
                encryptedString = Convert.ToBase64String(memStream.ToArray())
            End If
        Finally
            If rijndael IsNot Nothing Then
                rijndael.Clear()
            End If
            If rijndaelEncrypt IsNot Nothing Then
                rijndaelEncrypt.Dispose()
            End If
            If memStream IsNot Nothing Then
                memStream.Close()
            End If
        End Try

        Return encryptedString
    End Function

    Public Function Decrypt(ByVal encryptedValue As String) As String
        Dim decryptedString As String = String.Empty
        Dim rijndael As New RijndaelManaged()
        Dim rijndaelDecrypt As ICryptoTransform = Nothing
        Dim decryptStream As CryptoStream = Nothing
        Dim memStream As New MemoryStream()
        Dim _key(23) As Byte
        Dim _IV(15) As Byte

        Try
            If encryptedValue.Length > 0 Then

                ' Create the crypto objects
                rijndaelDecrypt = rijndael.CreateDecryptor(_key, _IV)
                Dim input As Byte() = Convert.FromBase64String(encryptedValue)
                decryptStream = New CryptoStream(memStream, rijndaelDecrypt, CryptoStreamMode.Write)

                ' Write the decrytped value to memory
                decryptStream.Write(input, 0, input.Length)
                decryptStream.FlushFinalBlock()
                memStream.Position = 0
                Dim result(CType(memStream.Length - 1, System.Int32)) As Byte
                memStream.Read(result, 0, CType(result.Length, System.Int32))

                ' Retrieve the decrypted value to return
                Dim utf8 As New UTF8Encoding()
                decryptedString = utf8.GetString(result)
            End If
        Finally
            If rijndael IsNot Nothing Then
                rijndael.Clear()
            End If
            If rijndaelDecrypt IsNot Nothing Then
                rijndaelDecrypt.Dispose()
            End If
            If memStream IsNot Nothing Then
                memStream.Close()
            End If
        End Try

        Return decryptedString
    End Function

    Private Sub SetLinks(ByRef AppServer As String, ByRef DBServer As String, ByRef WebAppSuffix As String)
        Dim itm As ListItem = Nothing
        Dim blnIsDev As Boolean = False

        lblDatabase.Text = String.Format("Database Server: {0}", DBServer.ToUpper)
        SetURL(GetApplicationURL("IRMA CLIENT", AppServer, WebAppSuffix), blnIRMAClientUsed, "IRMA CLIENT", lnkClient)
        SetURL(GetApplicationURL("DVO", AppServer, WebAppSuffix), True, "DVO", lnkDVO) ' Forcing True for is-used... all regions use DVO...
        lnkDVO.Attributes.Add("target", "_blank") ' Open in new tab/window (other links already do, by default).
        SetURL(GetApplicationURL("POET", AppServer, WebAppSuffix), blnPOETUsed, "POET", lnkPOET)
        SetURL(GetApplicationURL("PROMO PLANNER", AppServer, WebAppSuffix), blnPromoPlannerUsed, "PROMO PLANNER", lnkPromoPlanner)
        SetURL(GetApplicationURL("REPORT MANAGER", AppServer, WebAppSuffix), blnReportManagerUsed, "REPORT MANAGER", lnkReportManager)
        SetURL(GetApplicationURL("SLIM", AppServer, WebAppSuffix), blnSLIMUsed, "SLIM", lnkSLIM)
        SetURL(GetApplicationURL("STORE OPS", AppServer, WebAppSuffix), blnStoreOpsUsed, "STORE OPS", lnkStoreOps)
        SetURL(GetApplicationURL("STORE ORDER GUIDE", AppServer, WebAppSuffix), blnStoreOrderGuideUsed, "STORE ORDER GUIDE", lnkStoreOrderGuide)

        If blnIsDev Then
            lblClientInfo.Text = GetLastWriteTime("\\" & Dns.GetHostName.ToString & "\WebApps$\Deployment\" & cboRegion.SelectedValue.ToUpper & "\IRMA_Dev\IRMA.application")
        Else
            lblClientInfo.Text = GetLastWriteTime("\\" & Dns.GetHostName.ToString & "\WebApps$\Deployment\" & cboRegion.SelectedValue.ToUpper & "\IRMA\IRMA.application")
        End If

        If cboRegion.SelectedItem.Text = "CE" And (cboEnvironment.SelectedItem.Text = "Production" Or cboEnvironment.SelectedItem.Text = "QA") Then
            lblIRMASystemVersion.Text = "No IRMA System Version"
        Else
            lblIRMASystemVersion.Text = "IRMA System Version " & sSystemVersion
        End If
    End Sub

    Private Sub SetApplicationUse(ByRef DBServer As String, ByRef Database As String, ByRef UserName As String, ByRef Password As String)
        Dim reader As SqlDataReader = Nothing
        Dim sConnStr As String
        Dim conn As SqlConnection
        Dim cmd As SqlCommand
        Dim sDBServer As String = String.Empty
        Dim blnIsDev As Boolean = False

        sConnStr = String.Format("Data Source={0};Initial Catalog={1};User Id={2};Password={3};", DBServer, Database, UserName, Password)

        If cboRegion.SelectedValue.ToUpper = "CE" And cboEnvironment.SelectedItem.Text.ToUpper <> "TEST" Then
            blnIRMAClientUsed = False
            blnPOETUsed = True
            blnPromoPlannerUsed = False
            blnReportManagerUsed = False
            blnSLIMUsed = False
            blnStoreOpsUsed = False
            blnStoreOrderGuideUsed = False

            sSystemVersion = "No IRMA System Version"
            sIRMAClientVersion = "version not found"

            Exit Sub
        End If

        conn = New SqlConnection(sConnStr)
        cmd = New SqlCommand("SELECT ApplicationName, Version, UsedByRegion FROM Version", conn)

        conn.Open()

        Try
            reader = cmd.ExecuteReader
            While reader.Read
                Select Case reader("ApplicationName").ToString.ToUpper
                    Case "IRMA CLIENT"
                        blnIRMAClientUsed = CBool(reader("UsedByRegion"))
                        sIRMAClientVersion = reader("Version").ToString
                    Case "POET"
                        blnPOETUsed = CBool(reader("UsedByRegion"))
                    Case "PROMO PLANNER"
                        blnPromoPlannerUsed = CBool(reader("UsedByRegion"))
                    Case "REPORT MANAGER"
                        blnReportManagerUsed = CBool(reader("UsedByRegion"))
                    Case "SLIM"
                        blnSLIMUsed = CBool(reader("UsedByRegion"))
                    Case "STORE OPS"
                        blnStoreOpsUsed = CBool(reader("UsedByRegion"))
                    Case "STORE ORDER GUIDE"
                        blnStoreOrderGuideUsed = CBool(reader("UsedByRegion"))
                    Case "SYSTEM"
                        sSystemVersion = reader("Version").ToString
                End Select
            End While
        Catch ex As Exception
            Debug.WriteLine("sfd")
        Finally
            If Not reader Is Nothing Then
                If Not reader.IsClosed Then reader.Close()
            End If

            If Not conn Is Nothing Then
                If conn.State <> ConnectionState.Closed Then conn.Close()
                conn.Dispose()
            End If
        End Try
    End Sub

    Private Sub LoadCombos()
        Dim Data As DataSet = New DataSet
        Dim da As SqlDataAdapter = New SqlDataAdapter("select * from Regions", SqlConn)

        da.Fill(Data, "regions")

        If Data.Tables(0).Rows.Count > 0 Then
            For Each dr As DataRow In Data.Tables("regions").Rows
                cboRegion.Items.Add(New ListItem(dr("Region").ToString(), dr("Region").ToString()))
            Next
            cboRegion.Items.Insert(0, New ListItem("", ""))
        End If

        da.SelectCommand = New SqlCommand("select * from Environments", SqlConn)
        da.Fill(Data, "Environments")

        If Data.Tables("Environments").Rows.Count > 0 Then
            For Each dr As DataRow In Data.Tables("Environments").Rows
                cboEnvironment.Items.Add(New ListItem(dr("Environment").ToString(), dr("Environment").ToString()))
            Next
            cboEnvironment.Items.Insert(0, New ListItem("", ""))
        End If

        da.Dispose()
        Data.Dispose()
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        If Not SqlConn Is Nothing Then
            If SqlConn.State <> ConnectionState.Closed Then SqlConn.Close()
            SqlConn.Dispose()
        End If
    End Sub

    Protected Sub button_generagelinks_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles button_generagelinks.Click
        GenerateLinks()
    End Sub

    Private Sub GenerateLinks()
        Dim sRegion As String
        Dim sVersion As String
        Dim sEnvironment As String
        Dim sql As String
        Dim AppServer As String
        Dim DBServer As String
        Dim database As String
        Dim username As String
        Dim password As String
        Dim bEncrypted As Boolean
        Dim UseWebAppSuffix As Boolean = False
        Dim WebAppSuffix As String = String.Empty

        sRegion = cboRegion.SelectedValue.ToString()
        sVersion = cboVersion.SelectedValue.ToString()
        sEnvironment = cboEnvironment.SelectedValue.ToString()
        sql = String.Format("select * from IRMASuiteConfig where Region = '{0}' and Version = '{1}' and Environment = '{2}'", sRegion, sVersion, sEnvironment)

        Dim data As DataSet = New DataSet
        Dim da As SqlDataAdapter = New SqlDataAdapter(sql, SqlConn)

        da.Fill(data, "IRMASuiteConfig")

        If data.Tables("IRMASuiteConfig").Rows.Count = 0 Then
            pnlLinks.Visible = False
            pnlNoConfig.Visible = True
        Else
            AppServer = data.Tables("IRMASuiteConfig").Rows(0)("Appserver")
            DBServer = data.Tables("IRMASuiteConfig").Rows(0)("DBServer")
            database = data.Tables("IRMASuiteConfig").Rows(0)("database")
            username = data.Tables("IRMASuiteConfig").Rows(0)("username")
            password = data.Tables("IRMASuiteConfig").Rows(0)("password")
            WebAppSuffix = data.Tables("IRMASuiteConfig").Rows(0)("webappsuffix") & ""

            Select Case data.Tables("IRMASuiteConfig").Rows(0)("usewebappsuffix").ToString.ToLower()
                Case "true", 1
                    UseWebAppSuffix = True
                Case "false", 0
                    UseWebAppSuffix = False
            End Select

            Select Case data.Tables("IRMASuiteConfig").Rows(0)("encrypted").ToString().ToLower()
                Case "true", 1
                    bEncrypted = True
                Case "false", 0
                    bEncrypted = False
            End Select

            If bEncrypted Then password = Decrypt(password)
            If Not UseWebAppSuffix Then WebAppSuffix = String.Empty

            SetApplicationUse(DBServer, database, username, password)
            SetLinks(AppServer, DBServer, WebAppSuffix)

            pnlLinks.Visible = True
            pnlNoConfig.Visible = False
        End If

        da.Dispose()
        data.Dispose()
    End Sub

    Protected Sub cboEnvironment_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboEnvironment.SelectedIndexChanged
        LoadVersion()
    End Sub

    Protected Sub cboRegion_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboRegion.SelectedIndexChanged
        LoadVersion()
    End Sub
End Class