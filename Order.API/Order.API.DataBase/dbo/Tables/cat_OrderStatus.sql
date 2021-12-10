
CREATE TABLE [dbo].[cat_OrderStatus]
(
	 [Identifier]		INT NOT NULL
	,[Name]			    VARCHAR(100)
	,[IsActive]			BIT
	,[CreatedDate]		DATETIME
	,[UpdatedDate]		DATETIME
	CONSTRAINT [PK_OrderStatus] PRIMARY KEY ([Identifier] ASC)
)
GO