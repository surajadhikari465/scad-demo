 /****** Object:  StoredProcedure [dbo].[Administration_POSPush_GetStoreScaleWriterConfigurations]    Script Date: 08/24/2006 16:33:09 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Administration_POSPush_GetStoreScaleWriterConfigurations]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Administration_POSPush_GetStoreScaleWriterConfigurations]
GO

/****** Object:  StoredProcedure [dbo].[Administration_POSPush_GetStoreScaleWriterConfigurations]    Script Date: 08/24/2006 16:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE dbo.Administration_POSPush_GetStoreScaleWriterConfigurations(
	@Store_No int,
	@ScaleType varchar(100)
)	
AS 

-- Joins the Store, StoreScaleConfig, and POSWriter tables to retrieve the Scale Push configuration data for each 
-- store with an entry in the StoreScaleConfig table. 
-- If a @Store_No is passed in then only data for that store is retrieved

BEGIN

	SELECT ST.Store_No, 
		ST.Store_Name,  
		POS_Scale.POSFileWriterKey AS ScaleFileWriterKey, 
		POS_Scale.POSFileWriterCode AS ScaleFileWriterCode, 
		POS_Scale.POSFileWriterClass AS ScaleFileWriterClass,
		POS_Scale.FileWriterType,
		POS_Scale.ScaleWriterType,
		SType.ScaleWriterTypeDesc 
	FROM StoreScaleConfig ST_Scale
	INNER JOIN
		Store ST
		ON ST.Store_No = ST_Scale.Store_No
	INNER JOIN
		POSWriter POS_Scale 
		ON ST_Scale.ScaleFileWriterKey = POS_Scale.POSFileWriterKey 
		AND ((@ScaleType IS NULL) OR 
			 (@ScaleType IS NOT NULL AND POS_Scale.ScaleWriterType = (SELECT ScaleWriterTypeKey FROM ScaleWriterType WHERE ScaleWriterTypeDesc=@ScaleType)))
	LEFT JOIN
		ScaleWriterType SType
		ON POS_Scale.ScaleWriterType = SType.ScaleWriterTypeKey
	WHERE POS_Scale.Disabled = 0 
		AND ((@Store_No IS NULL) OR 
			 (@Store_No IS NOT NULL AND ST.Store_No = @Store_No))
	ORDER BY ST.Store_No 

END
GO

 