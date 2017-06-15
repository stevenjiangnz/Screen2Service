CREATE PROCEDURE [dbo].[USP_GetAccountBalanceJourneyByAccount]
	@AccountId int,
	@Take int = -1	
AS
IF(@Take < 0)
    BEGIN
	SELECT Id,
	FundAmount,
	TotalBalance,
	AvailableFund,
	Margin,
	Reserve,
	FeeSum,
	PositionValue,
	TradingDate,
	UpdateDT,
	[Action],
	TransactionId,
	TradeSetId,
	OrderId,
	AccountId
	FROM AccountBalanceJourneys
	WHERE AccountId = @AccountId
	Order By Id desc
	END
	ELSE
	Begin
		Select top (@Take)
	Id,
	FundAmount,
	TotalBalance,
	AvailableFund,
	Margin,
	Reserve,
	FeeSum,
	PositionValue,
	TradingDate,
	UpdateDT,
	[Action],
	TransactionId,
	TradeSetId,
	OrderId,
	AccountId
	FROM AccountBalanceJourneys
	WHERE AccountId = @AccountId
	Order By Id desc
	End
RETURN 0
