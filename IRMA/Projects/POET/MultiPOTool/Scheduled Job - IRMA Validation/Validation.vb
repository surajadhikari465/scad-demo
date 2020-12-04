Imports Microsoft.VisualBasic
Imports WholeFoods.Utility.DataAccess
Imports System.Text
Imports WholeFoods.Utility

Module Validation

    Sub Main()

        log4net.Config.XmlConfigurator.Configure()

        Try

            Dim r As New BOValidateByRegion
            Dim RegionList As ArrayList = r.ListRegions

            Dim RegionID As Integer
            Dim jobStartTime As DateTime
            Dim jobDuration As TimeSpan
            Dim regionStartTime As DateTime
            Dim regionDuration As TimeSpan
            Dim send As New Email

            ' **** Get Order Info for Email Notifications
            Dim dt As DataTable = r.ValidationEmailInfo

            jobStartTime = System.DateTime.Now
            Console.WriteLine("starting validation job at " & jobStartTime.ToShortTimeString() & "...")
            Console.WriteLine()
            Logger.LogInfo("Starting POET Scheduled Job - IRMA Validation", GetType(Validation))

            For Each RegionID In RegionList
                regionStartTime = System.DateTime.Now

                Console.WriteLine("     validating region " & RegionID.ToString())
                Logger.LogInfo("Validating region " & RegionID.ToString(), GetType(Validation))

                Try
                    r.ValidateByRegion(RegionID)
                Catch ex As Exception
                    'Dim send As New Email
                    send.SendEmail("There was an error running the POET validation process for region  " _
                        & RegionID.ToString() _
                        & ", check the ErrorLog table for SQL errors..." _
                        & vbCrLf & vbCrLf & ex.ToString())
                    Logger.LogError("There was an error running the POET validation process for region  " & RegionID.ToString(), GetType(Validation), ex)
                End Try

                regionDuration = System.DateTime.Now - regionStartTime
                jobDuration = System.DateTime.Now - jobStartTime

                Console.WriteLine("     done validating region " & RegionID.ToString)
                Console.WriteLine("     it took " & regionDuration.ToString())
                Console.WriteLine()
                Console.WriteLine("   job time so far: " & jobDuration.ToString.ToString())
                Console.WriteLine()
            Next

            jobDuration = System.DateTime.Now - jobStartTime
            Console.WriteLine()
            Console.WriteLine("...done with all regions. total job time: " & jobDuration.ToString())
            Console.WriteLine()
            ' *** Sending Email Notifications ***
            Console.WriteLine("...sending Email notifications ")
            For Each dr As DataRow In dt.Rows
                If Not Trim(dr.Item("Email")) = String.Empty Then
                    If r.SessionValidated(dr.Item("UploadSessionHistoryID")) = True Then
                        send.SendEmail(dr.Item("UploadSessionHistoryID"), BuildMsgBody(True, dr), dr.Item("Email"), dr.Item("CCEmail"))
                    Else
                        send.SendEmail(dr.Item("UploadSessionHistoryID"), BuildMsgBody(False, dr), dr.Item("Email"), dr.Item("CCEmail"), True)
                    End If
                End If
            Next
            Logger.LogInfo("Completed POET Scheduled Job - IRMA Validation", GetType(Validation))
            Console.WriteLine("...done")
            Console.WriteLine("...FAIT ACCOMPLI!!!")
        Catch ex As Exception
            Logger.LogError("Error during POET Scheduled Job - IRMA Validation", GetType(Validation), ex)
        End Try

    End Sub

    Private Function BuildMsgBody(ByVal success As Boolean, ByVal SessionInfo As DataRow) As String

        Dim msg As New StringBuilder
        Dim successMsg As String = ""

        ' *** Create Message ***
        msg.AppendLine("******************")
        msg.AppendFormat("Dear {0} ", SessionInfo("UserName"))
        If success = False Then
            msg.AppendFormat("{0}Data validation exceptions " & _
            "were found in your POET Session Number {1}   " & _
            "{0}Proceed to POET exception page " & _
            "for details.{0}", ControlChars.NewLine, SessionInfo("UploadSessionHistoryID"))
        Else
            msg.AppendFormat("{0}Your Purchase orders from Session " & _
            "{1} have been successfully validated and will be pushed to IRMA on the scheduled Auto Push Date.{0}" & _
            "{0}If the Auto Push is scheduled for today, then the POs will be pushed to IRMA immediately.{0}" & _
            "{0}If the Auto Push is scheduled for a later date, then the POs will be pushed to IRMA automatically on that specified day. You can manually push these POs from the 'Ready to Create POs' page, at any time prior to the scheduled auto push date. {0}", _
            ControlChars.NewLine, SessionInfo("UploadSessionHistoryID"))
        End If
        msg.AppendLine("******************")
        msg.AppendFormat("User Region - {0}{1}{2}", ControlChars.Tab, SessionInfo("RegionName"), ControlChars.NewLine)
        msg.AppendFormat("Total Number of Orders - {0}{1}{2}", ControlChars.Tab, SessionInfo("NumberOfOrders"), ControlChars.NewLine)
        msg.AppendFormat("Total Number of Items - {0}{1}{2}", ControlChars.Tab, SessionInfo("NumberOfItems"), ControlChars.NewLine)
        msg.AppendFormat("FileName - {0}{1}{2}", ControlChars.Tab, SessionInfo("FileName"), ControlChars.NewLine)
        msg.AppendFormat("Date/Time Uploaded - {0}{1}{2}", ControlChars.Tab, SessionInfo("UploadedDate"), ControlChars.NewLine)
        msg.AppendLine("******************")

        Return msg.ToString
    End Function

End Module


