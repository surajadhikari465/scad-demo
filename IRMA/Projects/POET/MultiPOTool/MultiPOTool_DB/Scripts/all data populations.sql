  --update to Regions table
  update Regions set CentralTimeZoneOffset = 1 where RegionID=1
   update Regions set CentralTimeZoneOffset = 0 where RegionID=2
    update Regions set CentralTimeZoneOffset = 1 where RegionID=3
     update Regions set CentralTimeZoneOffset = -2 where RegionID=4
      update Regions set CentralTimeZoneOffset = 1 where RegionID=5
       update Regions set CentralTimeZoneOffset = -2 where RegionID=6
        update Regions set CentralTimeZoneOffset = -1 where RegionID=7
         update Regions set CentralTimeZoneOffset = -2 where RegionID=8
          update Regions set CentralTimeZoneOffset = 0 where RegionID=9
           update Regions set CentralTimeZoneOffset = 1 where RegionID=10
           update Regions set CentralTimeZoneOffset = 0 where RegionID=11
           update Regions set CentralTimeZoneOffset = 1 where RegionID=12

-- update to Version table

           update [version] set [Version] = 2.0 ,[AppVersion_Client] = 2.0

--insert into helplinks

INSERT INTO [MultiPOTool].[dbo].[HelpLinks]
           ([LinkDescription]
           ,[LinkURL]
           ,[UpdatedDate]
           ,[UpdatedUserID]
           ,[OrderOfAppearance])
     VALUES
           ('Guide to POET v2.0'
           ,'http://connect/team/IRMA/IRMA%20Training/Guide%20to%20POET%20v1.2.3.docx'
           )
GO

INSERT INTO [MultiPOTool].[dbo].[HelpLinks]
           ([LinkDescription]
           ,[LinkURL]
           ,[UpdatedDate]
           ,[UpdatedUserID]
           ,[OrderOfAppearance])
     VALUES
           ('POET Errors and Troubleshooting'
           ,'http://connect/team/IRMA/IRMA%20Training/POET%20Errors%20and%20Troubleshooting.docx'
           )
GO

INSERT INTO [MultiPOTool].[dbo].[HelpLinks]
           ([LinkDescription]
           ,[LinkURL]
           ,[UpdatedDate]
           ,[UpdatedUserID]
           ,[OrderOfAppearance])
     VALUES
           ('POET v2.0 Template'
           ,'http://connect/team/IRMA/IRMA%20Training/POET%20v1.2%20template.xlsx'
           )
GO

INSERT INTO [MultiPOTool].[dbo].[HelpLinks]
           ([LinkDescription]
           ,[LinkURL]
           ,[UpdatedDate]
           ,[UpdatedUserID]
           ,[OrderOfAppearance])
     VALUES
           ('POET Spreadsheet format guide v2.0'
           ,'http://connect/team/IRMA/IRMA%20Training/POET_Spreadsheet%20format%20guide%20v1.2.docx'
           )
GO

INSERT INTO [MultiPOTool].[dbo].[HelpLinks]
           ([LinkDescription]
           ,[LinkURL]
           ,[UpdatedDate]
           ,[UpdatedUserID]
           ,[OrderOfAppearance])
     VALUES
           ('Quick Guide to POET'
           ,'http://connect/team/IRMA/IRMA%20Training/Quick%20Guide%20to%20POET.docx'
           )
GO



