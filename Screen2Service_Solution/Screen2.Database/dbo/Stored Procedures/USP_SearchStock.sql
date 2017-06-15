CREATE PROCEDURE [dbo].[USP_SearchStock]
	@StartDate  int = 0,
	@EndDate  int = 0
AS
	SELECT i.ShareId, 
		i.TradingDate,
		s.Symbol,
		s.Name,
		s.Industry,
		s.Sector,
		s.ShareType,
		s.IsCFD,
		s.IsActive,
		t.[Open],
		t.[Close],
		t.High,
		t.Low,
		t.Volumn,
		t.AdjustedClose,
		i.SMA5,
		i.SMA10,
		i.SMA30,
		i.SMA50,
		i.SMA200,
		i.EMA10,
		i.EMA20,
		i.EMA50,
		i.BB_Middle,
		i.BB_Low,
		i.BB_High,
		i.ADX,
		i.ADX_Plus,
		i.ADX_Minus,
		i.MACD,
		i.MACD_Hist,
		i.MACD_Signal,
		i.Heikin_Open,
		i.Heikin_Close,
		i.Heikin_High,
		i.Heikin_Low,
		i.Stochastic_K,
		i.Stochastic_D,
		i.RSI,
		i.RSI2,
		i.WR,
		i.Delt_Price,
		i.Delt_SMA5,
		i.Delt_SMA10,
		i.Delt_SMA50,
		i.Delt_EMA20,
		i.Delt_Diff_ADX,
		i.Delt_MACD,
		i.Delt_MACD_Hist,
		i.Delt_MACD_Signal,
		i.Delt_K,
		i.Delt_D,
		i.Vol_AVG5,
		i.Vol_AVG10,
		i.Vol_AVG20,
		i.Prev_Heikin
	From [Indicators] i Inner join 
		[Shares] s on i.ShareId = s.Id inner join
		[Tickers] t on (i.ShareId = t.ShareId and i.TradingDate = t.TradingDate)
	Where (@StartDate = 0 or i.TradingDate >= @StartDate) And
	(@EndDate = 0 or i.TradingDate <= @EndDate)

		

RETURN 0
