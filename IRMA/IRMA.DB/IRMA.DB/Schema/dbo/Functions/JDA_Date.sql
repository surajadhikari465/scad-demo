Create Function dbo.JDA_Date(@cent int, @date int)
        returns datetime
Begin
        DECLARE @res as datetime;
        DECLARE @workdate as int 
        DECLARE @maxdate as datetime    
        DECLARE @mindate as datetime    

        SELECT @mindate = CAST('01/01/1900' as datetime),@maxdate = CAST('12/31/2075' as datetime)

        Select @workdate = Case @cent 
                                        when  0 then
                                                19000000 + @date
                                        else 
                                                20000000 + @date
                                  end 

        Select @res = case ISDATE(@workdate)
                                                when 1 then CONVERT(datetime,STR(@workdate,8),112) -- yymmdd input
                                                else case 
                                                                when @date = 999999 then @maxdate
                                                                else @mindate
                                                         end
                                  end

        RETURN @res;

--- REPLACED LOGIC
--      Select @res = Case @cent 
--                                      when  0 then
--                                              '19'+ LTRIM(STR(@date))
--                                      else 
--                                              case LEN(@date)
--                                                      when 3 then
--                                                              CAST('20000'+LTRIM(STR(@date)) as Datetime)
--                                                      when 4 then
--                                                              CAST('2000'+LTRIM(STR(@date)) as Datetime)
--                                                      when 5 then 
--                                                              CAST('200'+LTRIM(STR(@date)) as Datetime)
--                                                      when 6 then
--                                                              CAST('20'+LTRIM(STR(@date)) as Datetime)
--                                                      else
--                                                              CAST('01/01/01' as datetime)
--                                                      end
--                              end 

--      RETURN @res;
End