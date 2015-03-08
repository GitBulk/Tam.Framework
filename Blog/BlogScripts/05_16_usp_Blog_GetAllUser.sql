

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Blog_GetUserByName]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Blog_GetUserByName]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:		Description:
**		2015-Feb-07	ToantN		Get user by user name.
*******************************************************************************/

CREATE PROCEDURE [dbo].[usp_Blog_GetUserByName]
	@UserName nvarchar(15)
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
	  WHERE [UserName] = @UserName
END
GO