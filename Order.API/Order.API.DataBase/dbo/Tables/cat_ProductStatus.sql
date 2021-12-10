CREATE TABLE [dbo].[cat_ProductStatus]
(
	 [Identifier]		INT NOT NULL
	,[Name]			    VARCHAR(100)
	,[IsActive]			BIT
	,[CreatedDate]		DATETIME
	,[UpdatedDate]		DATETIME
	CONSTRAINT [PK_ProductStatus] PRIMARY KEY ([Identifier] ASC)
)
