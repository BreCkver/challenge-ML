--=============================================================================================================================================              
-- Author: <Jaime Reyes Verea>              
-- Date:  <2021-12-06>              
-- Desc: <INSERT NEW ORDER>                  
--=============================================================================================================================================     
CREATE PROCEDURE [dbo].[usp_Order_INS]  
@pi_UserId			INT,  
@pc_Name			VARCHAR(100),
@pi_OrderTypeId		INT,
@pi_OrderStatusId	INT
AS                                                                      
BEGIN  
	
	DECLARE @orderIdentifier INT
	
	INSERT INTO Orders 
	(	
		UserIdentifier,
		Name,
		OrderTypeId,
		OrderStatusId,
		CreatedDate
	)
	VALUES
	(
		@pi_UserId, 
		@pc_Name,
		@pi_OrderTypeId,
		@pi_OrderStatusId,
		GETDATE()
	)
	
	SET @orderIdentifier = SCOPE_IDENTITY()
	SELECT @orderIdentifier
END