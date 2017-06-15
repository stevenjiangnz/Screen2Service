-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE USP_GetLastTicker
	@ShareID int,
	@TradingDate int = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON; 

    -- Insert statements for procedure here
	SELECT top 1 
	 [Id] , 
    [TradingDate],
    [Open],
    [Close],
    [High]  ,
    [Low]    ,
    [Volumn]  ,
    [AdjustedClose],
	[JSTicks],
    [ShareId]   
	from tickers where shareid = @ShareID
	AND (@TradingDate is null OR TradingDate <= @TradingDate)
	order by TradingDate desc
END