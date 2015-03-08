
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Blog_GetInActiveUsers]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Blog_GetInActiveUsers]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:		Description:
**		2015-Feb-07	ToantN		Get in active user.
*******************************************************************************/

CREATE PROCEDURE [dbo].[usp_Blog_GetInActiveUsers]
	@OnDay datetime = null
AS
BEGIN
	SELECT [Id]
		  ,[UserName]
		  ,[Email]
		  ,[Password]
		  ,[PasswordSalt]
		  ,[CreatedDate]
		  ,[UpdatedDate]
		  ,[DataRowVersion]
		  ,[Status]
		  ,[UserToken]
		  ,[LastLoginDate]
	  FROM [dbo].[Users]
	  WHERE [Status] = 0
	  And (DATEDIFF(DAY, @OnDay, CreatedDate) = 0 Or ISNULL(@Onday, '') = '')
END
GO
-- select * from users
-- exec usp_Blog_GetInActiveUser '2015-02-07'