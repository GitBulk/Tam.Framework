

IF  EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[usp_Blog_GetPostsAndTotalRecords]') AND type in (N'P', N'PC'))
DROP PROCEDURE [dbo].[usp_Blog_GetPostsAndTotalRecords]
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

CREATE PROCEDURE [dbo].[usp_Blog_GetPostsAndTotalRecords]
	@Skip int,
	@Take int,
	@TotalRecord int output
AS
BEGIN
	If @Skip is null
		Set @Skip = 12

	If @Take is null
		Set @Take = 12
		
	CREATE TABLE #Posts12
	(
		[PostId] [int] primary key,
		[Title] [nvarchar](150) NOT NULL,
		[ShortDescription] [nvarchar](500) NOT NULL,
		[Description] [nvarchar](max) NOT NULL,
		[Meta] [nvarchar](300) NULL,
		[UrlSlug] [nvarchar](100) NOT NULL,
		[Status] [int] NOT NULL,
		[CreatedDate] [datetime] NOT NULL,
		[UpdatedDate] [datetime] NOT NULL,
		[DataRowVersion] [timestamp] NOT NULL,
		[CategoryId] [int] NULL,
		[PublishDate] [date] NULL,
		[IsPrivate] [bit] NOT NULL,
		[SearchValue] [nvarchar](200) NULL,
		[CountView] [int] NOT NULL,
		[Num] BIGINT  NOT NULL
	)
	
	INSERT into #Posts12 
	SELECT [PostId]
		  ,[Title]
		  ,[ShortDescription]
		  ,[Description]
		  ,[Meta]
		  ,[UrlSlug]
		  ,[Status]
		  ,[CreatedDate]
		  ,[UpdatedDate]
		  ,NULL
		  ,[CategoryId]
		  ,[PublishDate]
		  ,[IsPrivate]
		  ,[SearchValue]
		  ,[CountView]
		  ,ROW_NUMBER() OVER (ORDER BY PublishDate DESC, DataRowVersion DESC) AS [Num]
		FROM [dbo].[Posts]
	
	SET @TotalRecord = (SELECT COUNT(1) FROM #Posts12)
	
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
	FROM #Posts12 F
	 WHERE F.[Num] > @Skip
 
	DROP TABLE #Posts12
	
END



GO