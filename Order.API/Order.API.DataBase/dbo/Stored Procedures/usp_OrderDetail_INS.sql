--=============================================================================================================================================              
-- Author: <Jaime Reyes Verea>              
-- Date:  <2021-12-06>              
-- Desc: <INSERT NEW ORDER>                  
--=============================================================================================================================================     
CREATE PROCEDURE [dbo].[usp_OrderDetail_INS]  
@pi_OrderIdentifier		INT,
@pc_ExternalIdentifier	VARCHAR(50),
@pc_Description			VARCHAR(500),
@pc_KeyWords			VARCHAR(30),
@pc_Title				VARCHAR(50),
@pc_Author				VARCHAR(200),
@pc_Publisher			VARCHAR(100),
@pi_OrderStatusId		INT
AS                                                                      
BEGIN  
	
	DECLARE @orderIdentifier INT
	
	INSERT INTO OrderDetail 
	(	
		 OrderIdentifier
		,ExternalIdentifier
		,[Description]
		,KeyWords
		,Title
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
		@pi_OrderStatusId,
		GETDATE()
	)
	
	SET @orderIdentifier = SCOPE_IDENTITY()
	SELECT @orderIdentifier
END