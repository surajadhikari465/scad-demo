/****** Object:  StoredProcedure [dbo].[Replenishment_ScalePush_GetStoreWriterConfigurations]    Script Date: 09/06/2006 16:33:09 ******/
IF  EXISTS (SELECT * FROM dbo.sysobjects WHERE id = OBJECT_ID(N'[dbo].[Replenishment_ScalePush_GetStoreWriterConfigurations]') AND OBJECTPROPERTY(id,N'IsProcedure') = 1)
DROP PROCEDURE [dbo].[Replenishment_ScalePush_GetStoreWriterConfigurations]
GO

/****** Object:  StoredProcedure [dbo].[Replenishment_ScalePush_GetStoreWriterConfigurations]    Script Date: 09/06/2006 16:33:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER OFF
GO

CREATE PROCEDURE dbo.Replenishment_ScalePush_GetStoreWriterConfigurations(
	@FileWriterType varchar(10)
)
AS 

BEGIN
	-- Joins the Store, StoreScaleConfig and POSWriter tables to
	-- retrieve the Scale Push configuration data for each 
	-- Store with an entry in the StoreScaleConfig table.
		
	SELECT ST.Store_No, ST.Store_Name, 
		ST.BatchID, ST.BatchRecords, 
		POSW.POSFileWriterKey, POSW.POSFileWriterCode, POSW.POSFileWriterClass, POSW.DelimChar, 
		POSW.FixedWidth, POSW.EnforceDictionary, POSW.TaxFlagTrueChar, POSW.TaxFlagFalseChar,
		POSW.LeadingDelim, POSW.TrailingDelim, POSW.AppendToFile, POSW.OutputByIrmaBatches, 
		(SELECT COUNT(1) FROM POSWriterEscapeChars WHERE POSFileWriterKey = POSW.POSFileWriterKey) AS EscapeCharCount,
		POSW.FileWriterType, SWT.ScaleWriterTypeDesc AS ScaleWriterType,
		POSW.FieldIdDelim
	FROM StoreScaleConfig SSC
	INNER JOIN
		Store ST
		ON ST.Store_No = SSC.Store_No
	LEFT JOIN
		POSWriter POSW
		ON SSC.ScaleFileWriterKey = POSW.POSFileWriterKey	--JOIN IS 'SCALE' SPECIFIC
	LEFT JOIN
		ScaleWriterType SWT
		ON SWT.ScaleWriterTypeKey = POSW.ScaleWriterType
	WHERE POSW.Disabled = 0
		AND POSW.FileWriterType = @FileWriterType
	ORDER BY ST.Store_No  

END
GO

 