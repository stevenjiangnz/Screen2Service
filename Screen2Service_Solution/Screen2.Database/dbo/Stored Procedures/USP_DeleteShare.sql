-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE USP_DeleteShare
	-- Add the parameters for the stored procedure here
	@ShareID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DELETE [ShareInfoes]
	WHERE ShareID = @ShareID

	DELETE [Shares]
	WHERE id = @ShareID
END
GO
