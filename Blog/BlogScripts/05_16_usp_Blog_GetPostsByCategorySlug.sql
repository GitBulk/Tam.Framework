

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Blog_GetPosts]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Blog_GetPosts]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
/******************************************************************************
**		Change History
*******************************************************************************
**		Date:		Author:		Description:
**		2015-Feb-16	ToantN		Get posts (pagination)
*******************************************************************************/

CREATE PROCEDURE [dbo].[usp_Blog_GetPosts]
	@Skip int,
	@Take int
AS
BEGIN
	If @Skip is null
		Set @Skip = 12

	If @Take is null
		Set @Take = 12

	
SELECT top (@Take)
	   F.[PostId]
      ,F.[Title]
      ,F.[ShortDescription]
      ,F.[Description]
      ,F.[Meta]
      ,F.[UrlSlug]
      ,F.[Status]
      ,F.[CreatedDate]
      ,F.[UpdatedDate]
      ,F.[DataRowVersion]
      ,F.[CategoryId]
      ,F.[PublishDate]
      ,F.[IsPrivate]
      ,F.[SearchValue]
      ,F.[CountView]
FROM
(
	SELECT ROW_NUMBER() OVER (ORDER BY PublishDate DESC, DataRowVersion DESC) AS [Num]
	  ,[PostId]
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
 ) F
 WHERE F.[Num] > @Skip
 
END
GO