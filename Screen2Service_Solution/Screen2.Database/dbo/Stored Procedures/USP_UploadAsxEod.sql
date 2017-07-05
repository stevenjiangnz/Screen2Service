-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE USP_UploadAsxEod
	@TickersXML XML
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	INSERT INTO Tickers
	([TradingDate],[Open],[Close], [Low], [High], [Volumn],[AdjustedClose],[ShareId],[JSTicks])
	SELECT
    CONVERT(int, cast(colx.query('data(TradingDate) ') as varchar)) as [TradingDate],
    CONVERT(float, cast(colx.query('data(Open) ') as varchar))  as [Open],
    CONVERT(float, cast(colx.query('data(Close) ') as varchar)) as [Close],
    CONVERT(float, cast(colx.query('data(Low) ') as varchar)) as [Low],
	CONVERT(float, cast(colx.query('data(High) ') as varchar)) as [High],
	CONVERT(bigint, cast(colx.query('data(Volumn) ') as varchar)) as [Volumn],
	CONVERT(float, cast(colx.query('data(AdjustedClose) ') as varchar)) as [AdjustedClose],
	CONVERT(int, cast(colx.query('data(ShareId) ') as varchar)) as [ShareId],
	CONVERT(bigint, cast(colx.query('data(JSTicks) ') as varchar)) as [JSTicks]
	
FROM @TickersXML.nodes('ArrayOfAsxEod/AsxEod') AS Tabx(Colx)

END