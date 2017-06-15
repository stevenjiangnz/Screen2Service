CREATE PROCEDURE [dbo].[USP_UpdateIndicatorBatch]
	@IndicatorsXML XML
AS
	
	SELECT CONVERT(int, cast(colx.query('data(TradingDate) ') as varchar)) as [xTradingDate],
	CONVERT(int, cast(colx.query('data(ShareId) ') as varchar)) as [xShareId],
	CASE WHEN cast(colx.query('data(Delt_Price)') as varchar) = '' THEN null ELSE CONVERT(float, cast(colx.query('data(Delt_Price) ') as varchar)) END as xDelt_Price,
	CASE WHEN cast(colx.query('data(Delt_SMA5)') as varchar) = '' THEN null ELSE CONVERT(float, cast(colx.query('data(Delt_SMA5) ') as varchar)) END as xDelt_SMA5,
	CASE WHEN cast(colx.query('data(Delt_SMA10)') as varchar) = '' THEN null ELSE CONVERT(float, cast(colx.query('data(Delt_SMA10) ') as varchar)) END as xDelt_SMA10,
	CASE WHEN cast(colx.query('data(Delt_SMA50)') as varchar) = '' THEN null ELSE CONVERT(float, cast(colx.query('data(Delt_SMA50) ') as varchar)) END as xDelt_SMA50,
	CASE WHEN cast(colx.query('data(Delt_EMA20)') as varchar) = '' THEN null ELSE CONVERT(float, cast(colx.query('data(Delt_EMA20) ') as varchar)) END as xDelt_EMA20,
	CASE WHEN cast(colx.query('data(Delt_MACD)') as varchar) = '' THEN null ELSE CONVERT(float, cast(colx.query('data(Delt_MACD) ') as varchar)) END as xDelt_MACD,
	CASE WHEN cast(colx.query('data(Delt_MACD_Hist)') as varchar) = '' THEN null ELSE CONVERT(float, cast(colx.query('data(Delt_MACD_Hist) ') as varchar)) END as xDelt_MACD_Hist,
	CASE WHEN cast(colx.query('data(Delt_MACD_Signal)') as varchar) = '' THEN null ELSE CONVERT(float, cast(colx.query('data(Delt_MACD_Signal) ') as varchar)) END as xDelt_MACD_Signal,
	CASE WHEN cast(colx.query('data(Delt_K)') as varchar) = '' THEN null ELSE CONVERT(float, cast(colx.query('data(Delt_K) ') as varchar)) END as xDelt_K,
	CASE WHEN cast(colx.query('data(Delt_D)') as varchar) = '' THEN null ELSE CONVERT(float, cast(colx.query('data(Delt_D) ') as varchar)) END as xDelt_D,
	CASE WHEN cast(colx.query('data(Delt_Diff_ADX)') as varchar) = '' THEN null ELSE CONVERT(float, cast(colx.query('data(Delt_Diff_ADX) ') as varchar)) END as xDelt_Diff_ADX,
	CASE WHEN cast(colx.query('data(Vol_AVG5)') as varchar) = '' THEN null ELSE CONVERT(float, cast(colx.query('data(Vol_AVG5) ') as varchar)) END as xVol_AVG5,
	CASE WHEN cast(colx.query('data(Vol_AVG10)') as varchar) = '' THEN null ELSE CONVERT(float, cast(colx.query('data(Vol_AVG10) ') as varchar)) END as xVol_AVG10,
	CASE WHEN cast(colx.query('data(Vol_AVG20)') as varchar) = '' THEN null ELSE CONVERT(float, cast(colx.query('data(Vol_AVG20) ') as varchar)) END as xVol_AVG20,
	CASE WHEN cast(colx.query('data(Prev_Heikin)') as varchar) = '' THEN null ELSE CONVERT(bit, cast(colx.query('data(Prev_Heikin) ') as varchar)) END as xPrev_Heikin
	into #tempIndicator

FROM @IndicatorsXML.nodes('ArrayOfIndicator/Indicator') AS Tabx(Colx)

update Indicators 
set 
	Delt_Price = t.xDelt_Price,
	Delt_SMA5 = t.xDelt_SMA5,
	Delt_SMA10 = t.xDelt_SMA10,
	Delt_SMA50 = t.xDelt_SMA50,
	Delt_EMA20 =t.xDelt_EMA20,
	Delt_MACD = t.xDelt_MACD,
	Delt_MACD_Hist = t.xDelt_MACD_Hist,
	Delt_MACD_Signal = t.xDelt_MACD_Signal,
	Delt_K = t.xDelt_K,
	Delt_D = t.xDelt_D,
	Delt_Diff_ADX = t.xDelt_Diff_ADX,
	Vol_AVG5 = t.xVol_AVG5,
	Vol_AVG10 = t.xVol_AVG10,
	Vol_AVG20 = t.xVol_AVG20,
	Prev_Heikin = t.xPrev_Heikin
from Indicators i inner join
	#tempIndicator t
	on (i.ShareId = t.xShareId and i.TradingDate =t.xTradingDate)

RETURN 0
