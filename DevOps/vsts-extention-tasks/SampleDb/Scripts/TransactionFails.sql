declare @scriptKey varchar(128) -- Create the variable
set @scriptKey = 'Add_Bob_and_Frank_Script' -- Set the variable value
:r .\..\Templates\StartDataTransformation.sql -- Reletive path to the start script template
-- Start custom logic
		Insert Into Users (Id, UserName) values(10,'bob')
		Insert Into Users (Id, UserName) values(10,'frank')
-- End custom logic
:r .\..\Templates\EndDataTransformation.sql -- Relative path to the end script template