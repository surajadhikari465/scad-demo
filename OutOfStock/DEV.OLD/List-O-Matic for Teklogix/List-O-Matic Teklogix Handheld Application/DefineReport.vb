Imports System
Imports System.IO
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Data
Imports System.Data.SqlClient
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
Imports System.Xml
Imports System.Collections.Specialized
Imports System.Reflection
Imports Symbol
Imports Symbol.Barcode
Imports Scanner.quiver_tst


Public Class DefineReport

    Private splashScreen As SplashScreenForm
    Public RegionCode As String
    Public ServiceURI As String
    Public UserAuthenticated As String
    Public UserName As String
    Public UserEmail As String


    Dim regionsDataSet As New DataSet
    Dim MySettingsDataSet As New DataSet
    Dim fs As IO.FileStream
    Dim regionID As String
    Dim regionName As String
    Dim myRegion As String
    Dim myStore As String
    Dim myStoreName As String
    Dim UsingDefaultStore As Boolean

    Dim RegionsFile As String
    Dim MySettingsFile As String

    Private Sub DefineReport_Closing(ByVal sender As Object, ByVal e As System.ComponentModel.CancelEventArgs) Handles MyBase.Closing

        If MsgBox("Exit SMORe List-O-Matic?", MsgBoxStyle.OkCancel, "Exit?") = MsgBoxResult.Cancel Then

            e.Cancel = True

        End If
    End Sub

    Function GenerateStoreList(ByVal RegionID As String) As Boolean



        'Dim client As New SmoreServiceClient(SmoreServiceClient.CreateDefaultBinding(), New System.ServiceModel.EndpointAddress(My.Resources.WebServiceAddress))

        'Moved this to MySettings.xml ...

        Dim client As New SmoreServiceClient(SmoreServiceClient.CreateDefaultBinding(), New System.ServiceModel.EndpointAddress(WcfAddress))

        'Dim storelistgenerated As Boolean

        Try



            Dim mystorelist As New StoreList

            mystorelist.Region = RegionID



            'ComboBox1.DisplayMember = "Name"
            'ComboBox1.ValueMember = "BusinessUnit"



            ComboBox1.DisplayMember = "Name"
            ComboBox1.ValueMember = "Name"


            'ComboBox1.DataSource = client.StoreNameWithBU(mystorelist)


            ComboBox1.DataSource = client.StoreName(mystorelist)


        Catch ex As Exception



            MsgBox("Error: " & ex.Message, MsgBoxStyle.Exclamation, "Store List Error")

        End Try


        Return True



    End Function

    Private Sub DefineReport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load




        Me.Enabled = True
        Me.Visible = True


        splashScreen.Close()



        If File.Exists(RegionsFile) Then


            BuildRegionList()


        Else


            MsgBox("Missing File: Regions.xml", MsgBoxStyle.Exclamation, "Error.")


        End If







    End Sub

    Public Sub New()

        ' create and display the splash screen form and make the main form it's owner
        splashScreen = New SplashScreenForm()
        splashScreen.Owner = Me
        splashScreen.Show()

        ' process the message queue - this is done to allow the splash screen to be painted
        Application.DoEvents()

        ' disable the main form to avoid the title bar being drawn over the splash screen during initialization
        Me.Enabled = False


        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        AppPath()

        MySettingsFile = OOSAppPath & "\MySettings.xml"
        RegionsFile = OOSAppPath & "\Regions.xml"






        If File.Exists(MySettingsFile) Then


            GetMySettings()


        Else


            MsgBox("Missing File: MySettings.xml", MsgBoxStyle.Exclamation, "Error.")


        End If








    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

        LoadScanForm()

    End Sub   


    Private Sub MenuItem6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem6.Click

        Dim iam As String

        iam = System.Reflection.Assembly.GetExecutingAssembly.FullName

        MsgBox(iam, MsgBoxStyle.Information, "OOS")
    End Sub

    Private Sub MenuItem5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MenuItem6.Click


        If MsgBox("Exit OOS?", MsgBoxStyle.OkCancel, "Exit?") = MsgBoxResult.Ok Then







            Application.[Exit]()






            Me.Dispose()
            Me.Close()



        End If

    End Sub








    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click

        Button2.Enabled = False
        Dim vim As New VimLookup()
        Dim mystorelist As New StoreList

        'GenerateStoreList(ComboBox2.SelectedItem)
        ComboBox1.DataSource = vim.GetAllStoreNamesByRegion(ComboBox2.SelectedItem)
        Button2.Enabled = True

        UsingDefaultStore = False

    End Sub






    Function BuildRegionList() As Boolean




        Try

            fs = New IO.FileStream(RegionsFile, IO.FileMode.Open)

        Catch err As Exception


            MsgBox("Unable to Open File: " & RegionsFile & err.Message, MsgBoxStyle.Exclamation, "Error.")


        End Try






        Try

            regionsDataSet.ReadXml(fs)

        Catch ex As Exception


            MsgBox("Unable to Read: " & RegionsFile & ex.Message, MsgBoxStyle.Exclamation, "Error.")



        Finally

            If System.IO.File.Exists(RegionsFile) = True Then

                fs.Close()

            End If

        End Try










        For Each DataRow As DataRow In regionsDataSet.Tables(0).Rows


            regionID = DataRow("regionid").ToString

            regionName = DataRow("regionname").ToString

            ComboBox2.Items.Add(regionID)


        Next





        ComboBox2.SelectedItem = myRegion



        Return True



    End Function



    Function GetMySettings() As Boolean





        Try

            fs = New IO.FileStream(MySettingsFile, IO.FileMode.Open)

        Catch err As Exception



            MsgBox("Unable to Open File: " & MySettingsFile & err.Message, MsgBoxStyle.Exclamation, "Error.")


        End Try

        Try

            MySettingsDataSet.ReadXml(fs)

        Catch ex As Exception


            MsgBox("Unable to Read: " & MySettingsFile & ex.Message, MsgBoxStyle.Exclamation, "Error.")

        Finally


            fs.Close()

        End Try




        For Each DataRow As DataRow In MySettingsDataSet.Tables(0).Rows


            myRegion = DataRow("MyRegion").ToString

            myStore = DataRow("MyStore").ToString

            MaxItems = Convert.ToInt32(DataRow("MaxItems").ToString)

            WcfAddress = DataRow("ServiceAddress").ToString


        Next


        If myStore <> "" Then


            ComboBox1.Items.Add(Trim(myStore))
            ComboBox1.SelectedItem = Trim(myStore)
            ComboBox1.SelectedValue = Trim(myStore)
            Module1.storeBUandName = myStore
            UsingDefaultStore = True



            'Module1.storeName = Trim(Split(myStore, "|")(1))
            'Module1.storeBU = Trim(Split(myStore, "|")(0))


            Module1.storeName = Trim(myStore)

        Else


            UsingDefaultStore = False


        End If


        Return True

    End Function



    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs)


        If e.KeyCode = Keys.Enter Then


            LoadScanForm()

        End If


    End Sub




    Function LoadScanForm() As Boolean



        If ComboBox2.SelectedItem = "" Then



            MsgBox("Select a Region", MsgBoxStyle.Exclamation, "Region Required.")



        ElseIf ComboBox1.SelectedItem Is Nothing Then



            MsgBox("Select a Store", MsgBoxStyle.Exclamation, "Store Required.")


        Else


            Button1.Enabled = False

            Module1.regionID = Trim(Split(ComboBox2.SelectedItem, "|")(0))


            If UsingDefaultStore = False Then

                'Module1.storeBUandName = ComboBox1.SelectedValue
                'Module1.storeName = Trim(Split(ComboBox1.SelectedValue, "|")(1))
                'Module1.storeBU = Trim(Split(ComboBox1.SelectedValue, "|")(0))


                Module1.storeName = ComboBox1.SelectedValue


            End If






            Dim myXmlTextWriter As XmlTextWriter = New XmlTextWriter(MySettingsFile, System.Text.Encoding.UTF8)
            myXmlTextWriter.Formatting = System.Xml.Formatting.Indented
            myXmlTextWriter.WriteStartDocument(False)

            'Create the main document element ...

            myXmlTextWriter.WriteStartElement("MySettings")
            myXmlTextWriter.WriteStartElement("MyRegion")
            myXmlTextWriter.WriteString(ComboBox2.SelectedItem)
            myXmlTextWriter.WriteEndElement()
            myXmlTextWriter.WriteStartElement("MyStore")
            myXmlTextWriter.WriteString(storeName)
            myXmlTextWriter.WriteEndElement()
            myXmlTextWriter.WriteStartElement("MaxItems")
            myXmlTextWriter.WriteString(MaxItems)
            myXmlTextWriter.WriteEndElement()
            myXmlTextWriter.WriteStartElement("ServiceAddress")
            myXmlTextWriter.WriteString(WcfAddress)
            myXmlTextWriter.WriteEndElement()
            myXmlTextWriter.WriteEndElement()
            myXmlTextWriter.Flush()
            myXmlTextWriter.Close()




            'MsgBox("BB")



            HandHeldDataCollect.Label2.Text = "(" & Module1.regionID & ")" & " " & Module1.storeName

            ' MsgBox("AA")



            HandHeldDataCollect.Show()

            Button1.Enabled = True

        End If



    End Function






End Class