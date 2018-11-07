CREATE PROCEDURE [dbo].[IconPosItemRefresh]
	@Identifiers varchar(max)
as
begin

UPDATE si

SET Refresh = 1

FROM 
	dbo.StoreItem si
	JOIN ItemIdentifier ii (Nolock) on si.Item_Key = ii.Item_Key
	JOIN ValidatedScanCode vsc (Nolock) on ii.Identifier = vsc.ScanCode
	JOIN fn_ParseStringList(@Identifiers, '|') list on ii.Identifier = list.Key_Value
	JOIN Price p (Nolock) on ii.Item_Key = p.Item_Key AND si.Store_No = p.Store_No
	JOIN StorePOSConfig spc (Nolock) on si.Store_No = spc.Store_No
	JOIN POSWriter pos (Nolock) on spc.POSFileWriterKey = pos.POSFileWriterKey
WHERE 
	p.Price <> 0 AND 
	si.Authorized = 1 AND 
	pos.POSFileWriterCode = 'R10'

end