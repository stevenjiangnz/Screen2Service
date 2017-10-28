CREATE PROCEDURE [dbo].[USP_GetAccountFull]
AS
	SELECT 
	a.Id, 
	a.BrokerId,
	a.CreateDate,
	a.CreatedBy,
	a.[Description],
	a.Name,
	a.[Owner],
	a.[Status],
	a.ZoneId,
	a.IsTrackingAccount,
	ab.AccountId,
	ab.FundAmount,
	ab.AvailableFund,
	ab.FeeSum,
	ab.Id as AccountBalanceId,
	ab.TradingDate,
	ab.UpdateDT
	from Accounts a
	Left Join AccountBalances ab on a.id=ab.AccountId 
RETURN 0
