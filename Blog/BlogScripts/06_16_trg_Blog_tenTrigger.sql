IF OBJECT_ID ('[dbo].[trg_tenTrgiger]','TR') IS NOT NULL
    DROP TRIGGER trg_tenTrgiger;
GO

/*******************************************************************************************
**		Change History
********************************************************************************************
**		Date:		Author:					Description:
********************************************************************************************/

CREATE TRIGGER [dbo].[trg_tenTrgiger]
ON [dbo].[ApplyForSecondaryCustomer]
FOR DELETE 
AS
	BEGIN
	END
GO
