CREATE PROCEDURE [dbo].[USP_InsertAlertResultBatch]
	@AlertResultsXML XML
AS

--IF (EXISTS (SELECT * 
--                 FROM INFORMATION_SCHEMA.TABLES 
--                 WHERE TABLE_SCHEMA = 'dbo' 
--                 AND  TABLE_NAME = 'TempAlertResult'))
--DROP TABLE TempAlertResult 
	INSERT INTO dbo.AlertResults
	(
		AlertId,
		TradingDate,
		IsMatch,
		[Message],
		ProcessDT,
		ZoneId,
		ShareId
	)
	SELECT 
	CONVERT(int, cast(colx.query('data(AlertId) ') as varchar)) as [AlertId],
	CONVERT(int, cast(colx.query('data(TradingDate) ') as varchar)) as [TradingDate],
	CONVERT(bit, cast(colx.query('data(IsMatch) ') as varchar)) as [IsMatch],
	CASE WHEN cast(colx.query('data(Message)') as varchar) = '' THEN null ELSE cast(colx.query('data(Message)') as varchar) END as [Message],
	GetDate() as ProcessDT,
	CASE WHEN cast(colx.query('data(ZoneId)') as varchar) = '' THEN null ELSE CONVERT(int, cast(colx.query('data(ZoneId)') as varchar)) END as ZoneId,
	CONVERT(int, cast(colx.query('data(ShareId) ') as varchar)) as [ShareId]


	--Into ProductVendor1
FROM @AlertResultsXML.nodes('ArrayOfAlertResult/AlertResult') AS Tabx(Colx)

RETURN 0
