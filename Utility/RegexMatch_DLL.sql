ALTER DATABASE ItemCatalog SET TRUSTWORTHY ON
GO

use ItemCatalog
go

exec sp_changedbowner 'sa'
go

/****** Object:  UserDefinedFunction [dbo].[RegExMatch]    Script Date: 10/22/2009 17:32:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RegExReplace]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[RegExReplace]
go

/****** Object:  UserDefinedFunction [dbo].[RegExMatch]    Script Date: 10/22/2009 17:32:26 ******/
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RegExMatch]') AND type in (N'FN', N'IF', N'TF', N'FS', N'FT'))
DROP FUNCTION [dbo].[RegExMatch]
go

IF EXISTS (SELECT * FROM sys.assemblies WHERE [name] = 'RegexProject')
DROP ASSEMBLY RegexProject
go

CREATE ASSEMBLY RegexProject from 'S:\IRMA\RegexProject.dll' WITH PERMISSION_SET = EXTERNAL_ACCESS
go

CREATE FUNCTION [dbo].[RegExReplace](@expression [nvarchar](4000), @pattern [nvarchar](4000), @replace [nvarchar](4000))
RETURNS [nvarchar](4000) WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [RegexProject].[RegexProject.UserDefinedFunctions].[RegExReplace]
GO
EXEC sys.sp_addextendedproperty @name=N'AutoDeployed', @value=N'yes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'RegExReplace'
GO
EXEC sys.sp_addextendedproperty @name=N'SqlAssemblyFile', @value=N'UserDefinedFunctions.vb' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'RegExReplace'
GO
EXEC sys.sp_addextendedproperty @name=N'SqlAssemblyFileLine', @value=16 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'RegExReplace'
go

CREATE FUNCTION [dbo].[RegExMatch](@Input [nvarchar](4000), @Pattern [nvarchar](4000))
RETURNS [bit] WITH EXECUTE AS CALLER
AS 
EXTERNAL NAME [RegexProject].[RegexProject.UserDefinedFunctions].[RegExMatch]
GO
EXEC sys.sp_addextendedproperty @name=N'AutoDeployed', @value=N'yes' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'RegExMatch'
GO
EXEC sys.sp_addextendedproperty @name=N'SqlAssemblyFile', @value=N'UserDefinedFunctions.vb' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'RegExMatch'
GO
EXEC sys.sp_addextendedproperty @name=N'SqlAssemblyFileLine', @value=16 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'FUNCTION',@level1name=N'RegExMatch'
go
