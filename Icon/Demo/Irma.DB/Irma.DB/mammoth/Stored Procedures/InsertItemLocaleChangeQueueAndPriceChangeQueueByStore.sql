CREATE PROCEDURE [mammoth].[InsertItemLocaleChangeQueueAndPriceChangeQueueByStore]
	@Store_No int
AS
BEGIN	

declare @result table(successCode bit, [message] varchar(max))
begin transaction
begin try
			declare @eventTypeID int
			declare @successCode bit
			declare @message varchar(max) = ''
			declare @excludedStoreNo varchar(250);
			set @excludedStoreNo = (select dbo.fn_GetAppConfigValue('LabAndClosedStoreNo','IRMA Client'));

			If EXISTS (select Key_Value from dbo.fn_Parse_List(@ExcludedStoreNo, '|') WHERE Key_Value = @Store_No)
			begin
				set @successCode = 0
				set @message = @message + ' This store is configured as a Lab or Closed Store. '
				insert into @result (successCode, [message]) values(@successCode, @message)
				select successCode, [message] from @result
			end
			else
			begin

			--************************************************************************************************************************************
				declare @priceConfigValue varchar(350);
				declare @priceRowCount int = 0

				set @priceConfigValue = (select dbo.fn_GetAppConfigValue('MammothPriceChanges','IRMA Client'));
			
				if @priceConfigValue != 1
				begin
					set @successCode = 1
					set @message = @message + ' Mammoth Price changes are disabled. '
				end
				else
				begin
					set @eventTypeID = (select EventTypeID from mammoth.ItemChangeEventType (nolock) where EventTypeName = 'Price')
				
					insert into mammoth.PriceChangeQueue with (tablock) (Item_Key, Store_No, Identifier, EventTypeID, EventReferenceID, InsertDate)
					select
						P.Item_Key,
						P.Store_No,
						II.Identifier,
						@eventTypeID AS EventTypeID,
						null as EventReferenceID,
						getdate() AS InsertDate
					from
					Price p
					inner join Item i on p.Item_Key = i.Item_Key
					inner join ItemIdentifier ii on p.Item_Key = ii.Item_Key
					inner join StoreItem si on p.Item_Key = si.Item_Key and p.Store_No = si.Store_No
					inner join ValidatedScanCode vsc (NOLOCK) on ii.Identifier = vsc.ScanCode 
					where 
					i.Deleted_Item = 0
					and ii.Deleted_Identifier = 0
					and ii.Remove_Identifier = 0
					and si.Authorized = 1
					and p.Store_No = @Store_No
				
					set @priceRowCount = @@rowcount
				end
			--************************************************************************************************************************************
				declare @itemLocaleConfigValue varchar(350)
				declare @itemLocaleRowCount int = 0

				set @itemLocaleConfigValue = (select dbo.fn_GetAppConfigValue('MammothItemLocaleChanges','IRMA Client'));
			
				if @itemLocaleConfigValue != 1
				begin
					set @successCode = 1
					set @message = @message + ' Mammoth ItemLocale changes are disabled. '
				end
				else
				begin

					set @eventTypeId = (select EventTypeID from mammoth.ItemChangeEventType WHERE EventTypeName = 'ItemLocaleAddOrUpdate')

					insert into mammoth.ItemLocaleChangeQueue with (tablock) (Item_Key, Store_No, Identifier, EventTypeID, EventReferenceID, InsertDate)
					select
						i.Item_Key,
						@Store_No,
						ii.Identifier,
						@eventTypeId,
						null,
						getdate()
					from
						dbo.ItemIdentifier	ii
						inner join dbo.Item		i	on ii.Item_Key = i.Item_Key
						inner join StoreItem si on i.Item_Key = si.Item_Key and si.Store_No = @Store_No
						inner join ValidatedScanCode vsc on vsc.ScanCode = ii.Identifier
					where
						i.Deleted_Item = 0
						and ii.Deleted_Identifier = 0
						AND ii.Remove_Identifier = 0
						and si.Authorized = 1

					set @itemLocaleRowCount = @@rowcount
				end

			if @@TRANCOUNT > 0
			begin commit transaction
				set @successCode = 1
				set @message = @message + ' ' + cast(@priceRowCount as varchar(10)) + ' Price change records queued. ' + cast(@itemLocaleRowCount as varchar(10)) + ' ItemLocale change records queued. '
				insert into @result (successCode, [message]) values(@successCode, @message)
			end
			else
			begin
				set @successCode = 1
				set @message = @message + ' No records queued. '
				insert into @result (successCode, [message]) values(@successCode,  @message)
			end
		end
end try

begin catch
		IF @@TRANCOUNT > 0
		begin
			rollback transaction
		end
		set @successCode = 0

		set @message = @message + 
			' An error occurred.  Database changes were rolled back:'
			+ ' ErrorNumber = ' + cast(ERROR_NUMBER() as varchar(max))
			+ ' ErrorSeverity = ' + cast(ERROR_SEVERITY() as varchar(max))
			+ ' ErrorState = ' + cast(ERROR_STATE() as varchar(max))
			+ ' ErrorProcedure = ' +  ERROR_PROCEDURE()
			+ ' ErrorLine = ' +  cast(ERROR_LINE() as varchar(max))
			+ ' ErrorMessage = ' +  ERROR_MESSAGE()

		insert into @result (successCode, [message]) values(@successCode, @message)
	end catch
	select successCode, [message] from @result
END
GO
GRANT EXECUTE
    ON OBJECT::[mammoth].[InsertItemLocaleChangeQueueAndPriceChangeQueueByStore] TO [IRSUser]
    AS [dbo];


GO
GRANT EXECUTE
    ON OBJECT::[mammoth].[InsertItemLocaleChangeQueueAndPriceChangeQueueByStore] TO [IRMAClientRole]
    AS [dbo];

