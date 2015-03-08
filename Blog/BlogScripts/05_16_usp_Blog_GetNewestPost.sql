

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Blog_GetNewestPost]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Blog_GetNewestPost]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:		Description:
**		2015-Feb-16	ToantN		Get 10 newest post
*******************************************************************************/

CREATE PROCEDURE [dbo].[usp_Blog_GetNewestPost]
	@Take int
AS
BEGIN

	If @Take is null
		Set @Take = 12

	SELECT top (@Take) [PostId]
      ,[Title]
      ,[ShortDescription]
      ,[Description]
      ,[Meta]
      ,[UrlSlug]
      ,[Status]
      ,[CreatedDate]
      ,[UpdatedDate]
      ,[DataRowVersion]
      ,[CategoryId]
      ,[PublishDate]
      ,[IsPrivate]
      ,[SearchValue]
      ,[CountView]
  FROM [dbo].[Posts]
  ORDER BY [PublishDate] DESC, [DataRowVersion] DESC
END
GO