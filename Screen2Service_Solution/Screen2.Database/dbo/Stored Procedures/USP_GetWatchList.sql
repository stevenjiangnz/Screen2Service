CREATE PROCEDURE [dbo].[USP_GetWatchList]
	@UserID varchar(128)
	
AS

SELECT ID,
	Name,
	[Description],
	[Owner],
	IsSystem, 
	c.MemberCount,
	DisplayOrder,
	[Status],
	ZoneId from Watchlists 
	left join (select watchlistid, count(shareid) MemberCount from watchlistdetails
				where UserID= @UserID
				group by watchlistid) c
	on id = c.watchlistid
	WHERE IsSystem = 1 or (IsSystem = 0 and [owner]= @UserID)
	Order by Name


RETURN 0
