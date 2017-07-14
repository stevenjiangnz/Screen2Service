CREATE PROCEDURE [dbo].[USP_GetLatestAsxTradingDate]
AS
	SELECT max(TradingDate) as tradingDate
	From AsxEods
RETURN 0
