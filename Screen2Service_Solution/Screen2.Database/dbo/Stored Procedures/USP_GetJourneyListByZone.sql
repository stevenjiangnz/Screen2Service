CREATE PROCEDURE [dbo].[USP_GetJourneyListByZone]
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
	StartDay
	from Journeys
	where ((@ZoneId is null and ZoneId is null) or 
	(@ZoneId =ZoneId)) 
	and
	[Owner] = @Owner
	order by id desc
RETURN 0
