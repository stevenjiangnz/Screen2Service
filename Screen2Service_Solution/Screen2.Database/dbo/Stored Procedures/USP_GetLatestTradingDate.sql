CREATE PROCEDURE [dbo].[USP_GetLatestTradingDate]
AS
	SELECT max(TradingDate) as tradingDate
	From Indicators
RETURN 0
