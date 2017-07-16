CREATE PROCEDURE [dbo].[USP_GetTickerByShare]
	@ShareId int = 0,
	@StartDate int =-1,
	@EndDate int = -1
AS
	SELECT [Id]
      ,[TradingDate]
      ,[Open]
      ,[Close]
      ,[High]
      ,[Low]
      ,[Volumn]
      ,[AdjustedClose]
      ,[ShareId]
	  ,[JSTicks]
  FROM [Tickers]
  Where [ShareId] = @ShareId AND
	([TradingDate]>= @StartDate OR @StartDate =-1) AND
	([TradingDate]<= @EndDate OR @EndDate =-1)
	order by [TradingDate]
	
RETURN 0
