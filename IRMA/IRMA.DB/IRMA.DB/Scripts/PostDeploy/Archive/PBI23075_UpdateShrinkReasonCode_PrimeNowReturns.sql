RAISERROR('Updating ''Recall'' ShrinkSubType to be ''Prime Now Returns''', 0, 1) WITH NOWAIT

UPDATE	dbo.ShrinkSubType
SET		ReasonCodeDescription = 'Prime Now Returns',
		LastUpdateDateTime = getdate()
WHERE	ReasonCodeDescription = 'Recall'