CREATE PROCEDURE [dbo].[USP_GetShareByWatch]
	@WatchID int,
	@Reverse bit = 0
AS
	Select 
	* from Shares s
	where ((@Reverse =0 and s.Id in ( select shareID from WatchListDetails w
				where w.WatchListId = @WatchID)) or
		 (@Reverse = 1 and s.Id  not in ( select shareID from WatchListDetails w
				where w.WatchListId = @WatchID)))
		 and s.IsActive = 1

RETURN 0
