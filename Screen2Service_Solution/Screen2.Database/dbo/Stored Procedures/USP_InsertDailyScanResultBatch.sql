CREATE PROCEDURE [dbo].[USP_InsertDailyScanResultBatch]
	@DailyScanResultsXML XML
AS
--IF (EXISTS (SELECT * 
--                 FROM INFORMATION_SCHEMA.TABLES 
--                 WHERE TABLE_SCHEMA = 'dbo' 
--                 AND  TABLE_NAME = 'TempDailyScanResult'))
--DROP TABLE TempDailyScanResult 

	INSERT INTO dbo.DailyScanResults
	(
		DailyScanId,
		TradingDate,
		IsMatch,
		[Message],
		ProcessDT,
		ShareId
	)
	SELECT 
	CONVERT(int, cast(colx.query('data(DailyScanId) ') as varchar)) as [DailyScanId],
	CONVERT(int, cast(colx.query('data(TradingDate) ') as varchar)) as [TradingDate],
	CONVERT(bit, cast(colx.query('data(IsMatch) ') as varchar)) as [IsMatch],
	CASE WHEN cast(colx.query('data(Message)') as nvarchar(max)) = '' THEN null ELSE cast(colx.query('data(Message)') as nvarchar(max)) END as [Message],
	GetDate() as ProcessDT,
	CONVERT(int, cast(colx.query('data(ShareId) ') as varchar)) as [ShareId]

	--INTO TempDailyScanResult
	FROM @DailyScanResultsXML.nodes('ArrayOfDailyScanResult/DailyScanResult') AS Tabx(Colx)
RETURN 0
