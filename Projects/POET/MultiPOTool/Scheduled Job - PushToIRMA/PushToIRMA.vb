Imports Microsoft.VisualBasic
Imports WholeFoods.Utility.DataAccess

Module PushToIRMA

    Sub Main()

        Dim r As New BOPushToIRMA
        Dim RegionList As ArrayList = r.ListRegions

        Dim RegionID As Integer
        Dim jobStartTime As DateTime
        Dim jobDuration As TimeSpan
        Dim regionStartTime As DateTime
        Dim regionDuration As TimeSpan

        jobStartTime = System.DateTime.Now
        Console.WriteLine("starting push job at " & jobStartTime.ToShortTimeString() & "...")
        Console.WriteLine()

        For Each RegionID In RegionList
            regionStartTime = System.DateTime.Now

            Console.WriteLine("     pushing to region " & RegionID.ToString())

            Try
                r.PushByRegion(RegionID)
            Catch ex As Exception
                'Console.WriteLine("!!!error pushing to region " & RegionID.ToString())
                Dim send As New Email
                send.SendEmail("There was an error pushing POET orders to region  " & RegionID.ToString() & ", check the ErrorLog table for SQL errors..." & vbCrLf & vbCrLf & ex.ToString())
            End Try

            regionDuration = System.DateTime.Now - regionStartTime
            jobDuration = System.DateTime.Now - jobStartTime

            Console.WriteLine("     done pushing to region " & RegionID.ToString)
            Console.WriteLine("     it took " & regionDuration.ToString())
            Console.WriteLine()
            Console.WriteLine("   job time so far: " & jobDuration.ToString.ToString())
            Console.WriteLine()
        Next

        jobDuration = System.DateTime.Now - jobStartTime
        Console.WriteLine()
        Console.WriteLine("...done with all regions. total job time: " & jobDuration.ToString())
        Console.WriteLine()
        'Console.WriteLine("hit enter to exit")
        'Console.ReadLine()
    End Sub

End Module


