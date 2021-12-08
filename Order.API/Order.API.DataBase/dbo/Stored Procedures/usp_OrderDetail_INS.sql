--=============================================================================================================================================              
-- Author: <Jaime Reyes Verea>              
-- Date:  <2021-12-06>              
-- Desc: <INSERT NEW PRODUCT TO ORDER>                  
--=============================================================================================================================================     
CREATE PROCEDURE [dbo].[usp_OrderDetail_INS]  
@pi_OrderIdentifier		INT,
@pc_ExternalIdentifier	VARCHAR(50),
@pc_Description			VARCHAR(500) = NULL,
@pc_KeyWords			VARCHAR(30) = NULL,
@pc_Title				VARCHAR(50),
@pc_Author				VARCHAR(200),
@pc_Publisher			VARCHAR(100),
@pi_OrderStatusId		INT
AS                                                                      
BEGIN  

	DECLARE @orderDetailIdentifier INT
	
	INSERT INTO OrderDetail 
	(	
		 OrderIdentifier
		,ExternalIdentifier
		,[Description]
		,KeyWords
		,Title
		,Author
		,Publisher
		,OrderStatusId
		,CreatedDate
	)
	VALUES
	(
		@pi_OrderIdentifier,
		@pc_ExternalIdentifier,
		@pc_Description,
		@pc_KeyWords,
		@pc_Title,
		@pc_Author,
		@pc_Publisher,
		@pi_OrderStatusId,
		GETDATE()
	)
	
	SET @orderDetailIdentifier = SCOPE_IDENTITY()
	SELECT @orderDetailIdentifier
END