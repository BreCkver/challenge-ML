--=============================================================================================================================================              
-- Author: <Jaime Reyes Verea>              
-- Date:  <2021-12-06>              
-- Desc: <INSERT NEW USER>                  
--=============================================================================================================================================     
CREATE PROCEDURE [dbo].[usp_User_INS]  
@pc_userName	VARCHAR(100),  
@pc_PassWord	VARCHAR(100),
@pi_StatusId	INT
AS                                                                      
BEGIN  
	
	DECLARE @userIdentifier INT
	
	INSERT INTO Users 
	(	
		UserName, 
		[PassWord], 
		UserStatusId, 
		CreatedDate
	)
	VALUES
	(
		@pc_userName, 
		@pc_PassWord,
		@pi_StatusId,
		GETDATE()
	)
	
	SET @userIdentifier = SCOPE_IDENTITY()
	SELECT @userIdentifier
END