--=============================================================================================================================================            
-- Author: <JAIME REYES VEREA>            
-- Date:  <2021-12-06>          
-- Desc: <GET USER>       
--=============================================================================================================================================            
CREATE PROCEDURE [dbo].[usp_Users_GETL]      
@pi_UserId		INT			= NULL,
@pc_userName	VARCHAR(100)= NULL,  
@pc_PassWord	VARCHAR(100)= NULL
AS                                                                          
BEGIN      

 SET TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;               
 SET NOCOUNT ON;

	SELECT 
		 Identifier
		,UserName
		,[PassWord]
		,UserStatusId 
	FROM 
		Users
	WHERE
		Identifier	= COALESCE(@pi_UserId, Identifier) 
	AND UserName	= COALESCE(@pc_userName, UserName) 
	AND [PassWord]	= COALESCE(@pc_PassWord, [PassWord])
		
		      
 SET TRANSACTION ISOLATION LEVEL READ COMMITTED;               
 SET NOCOUNT OFF;              
END