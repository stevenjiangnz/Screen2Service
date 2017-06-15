CREATE PROCEDURE [dbo].[USP_GetPlanListByZone]
	@Owner nvarchar(max),
	@ZoneId int = null
	AS
	select 
	ID,
	ZoneId,
	Created,
	Modified,
	[Owner],
	[Status],
	TradingDate
	from Plans
	where ((@ZoneId is null and ZoneId is null) or 
	(@ZoneId =ZoneId)) 
	and
	[Owner] = @Owner
	order by id desc
RETURN 0
