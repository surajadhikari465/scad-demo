IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[calcCheckDigit]') AND xtype in (N'FN', N'IF', N'TF'))
DROP FUNCTION [dbo].[calcCheckDigit]
GO

create function [dbo].[calcCheckDigit](@upc varchar(12))
   -- **************************************************************************
   --  Procedure: calcCheckDigit()
   --     Author: Ron Savage, CEN, 512-542-0361
   --       Date: 09/21/2006
   --
   --  Description:
   --  This function takes a UPC string, and calculates the MOD10 check digit
   --  for that upc. The UPC must be left padded with 0s out to 12 characters.
   -- **************************************************************************
   returns integer
begin

   declare @checkDigit       as integer;
   declare @DigitTotal       as integer;
   declare @upcChar          as integer;
   declare @upcLength        as integer;
   declare @lastDigit        as integer;
   declare @DigitTotalLength as integer;
   declare @DigitTotalString as varchar(12);

   select @checkDigit = 0;
   select @DigitTotal = 0;
   select @upcChar    = 1;
   select @upcLength  = len(@upc);

   -- Check that the UPC sent is 12 characters in length
   if ( @upcLength =12 )
   begin
      while ( @upcChar <= @upcLength )
         begin
            if ( @upcChar % 2 = 0 )
               select @DigitTotal = @DigitTotal + ( 3 * cast( substring(@upc, @upcChar, 1) as integer) );
            else
               select @DigitTotal = @DigitTotal + ( 1 * cast( substring(@upc, @upcChar, 1) as integer) );

            select @upcChar = @upcChar + 1;
         end

      -- Get the last digit of the total
      select @DigitTotalString  = cast(@DigitTotal as varchar);
      select @DigitTotalLength  = len(@DigitTotalString);

      select @lastDigit         = cast(substring(@DigitTotalString, @DigitTotalLength, 1) as integer);

      -- Subtract it from 10, unless it's 0
      if ( @lastDigit = 0 )
         select @checkDigit = 0;
      else
         select @checkDigit = 10 - @lastDigit;
   end

   return (@checkDigit);
end
GO
 