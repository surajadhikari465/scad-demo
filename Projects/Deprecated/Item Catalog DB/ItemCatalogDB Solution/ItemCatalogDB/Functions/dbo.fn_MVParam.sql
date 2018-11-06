SET ANSI_NULLS ON

GO

SET QUOTED_IDENTIFIER ON

GO
IF EXISTS (SELECT * FROM   sysobjects WHERE  name = N'fn_MVParam') 
    DROP FUNCTION fn_MVParam
GO

-- =============================================

-- Author:                  Hussain Hashim

-- Create date:				8/16/2007

-- Description:				Multi Value Parameter

-- =============================================

CREATE FUNCTION [dbo].[fn_MVParam] 

(           
            -- Add the parameters for the function here
            @RepParam nvarchar(4000), 
            @Delim char(1)=','
)

RETURNS @Values TABLE (Param nvarchar(4000)) 
AS
Begin

            Declare @chrind INT
            Declare @Piece nvarchar(4000)

            Select @chrind=1

            While @chrind > 0
                        Begin
                                    Select @chrind=CharIndex(@Delim,@RepParam)
                                    If @chrind > 0
                                                Begin
                                                            Select @Piece = Left(@RepParam,@chrind-1)
                                                End
                                    Else
                                                Begin
                                                            Select @Piece=@RepParam
                                                End
                                    

                                    Insert @Values(Param) Values(@Piece)

                                    Select @RepParam = Right(@RepParam,Len(@RepParam)-@chrind)
                                    
                                    If Len(@RepParam) = 0 Break
                        End
Return
End

 


GO