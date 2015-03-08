
IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Blog_GetUserByEmail]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Blog_GetUserByEmail]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:		Description:
**		2015-Feb-07	ToantN		Get user by email.
*******************************************************************************/

CREATE PROCEDURE [dbo].[usp_Blog_GetUserByEmail]
	@Email varchar(64)
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
	  WHERE [UserName] = @Email
END
GO