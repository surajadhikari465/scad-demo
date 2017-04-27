if exists (select * from dbo.sysobjects where  name = 'pivot_example') drop procedure pivot_example;
go

create procedure dbo.pivot_example
as
--**************************************************************************
-- Procedure: pivot_example()
--    Author: Ron Savage
--      Date: 08/02/2007
--
-- Description:
-- This is an example of how you can use the pivot_query procedure to save
-- a ton of time making pivoted summary reports.
--
-- Change History:
-- Date        Init. Description
-- 08/02/2007  RS    Created.
-- **************************************************************************
begin
   --**************************************************************************
   -- Define the query to pull the raw data
   --**************************************************************************
   declare @myQuery    varchar(1000);

   set @myQuery = '
         select
            s.SubTeam_No,
            subt.SubTeam_Name,
            rtrim(st.StoreAbbr) StoreAbbr,
            s.Sales_Amount,
            s.Sales_Quantity
         from
            Sales_sumByItem s

            JOIN Store st
               ON (st.Store_No = s.Store_No)

            JOIN SubTeam subt
               ON ( subt.SubTeam_No = s.SubTeam_No )

         where
            s.SubTeam_No between 1 and 1000
            and s.Store_No between 100 and 1000
            and s.Date_Key > ''02/10/2007''';

   --**************************************************************************
   -- Pivot the crap out of it.
   --**************************************************************************
   EXEC dbo.pivot_query @myQuery, 'SubTeam_No,SubTeam_Name', 'StoreAbbr', 'sum(Sales_Amount) Sales,sum(Sales_Quantity) Qty';
end
go
