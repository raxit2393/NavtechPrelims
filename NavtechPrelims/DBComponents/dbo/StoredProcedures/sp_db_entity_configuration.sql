
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO  
ALTER PROCEDURE [dbo].[sp_db_entity_configuration]  
@Operation VARCHAR(20)=NULL,  
 
 @XMLConfig XML='' 
 
AS

BEGIN  
  
	IF UPPER(@Operation)='SELECT-CONIFG'  
	BEGIN  
		SELECT EntityName, FieldName, ISNULL(IsRequired, 0) AS IsRequired, [MaxLength], EndPointUrl FROM EntityConfiguration where IsActive = 1;
	END
  
	ELSE IF UPPER(@Operation)='SAVE-CONIFG'  
	BEGIN
	BEGIN TRANSACTION [ConfigurationTran]      
	BEGIN TRY
		DECLARE @EntityConfig TABLE (EntityName VARCHAR(200) NOT NULL, FieldName VARCHAR(200) NOT NULL, IsRequired BIT NULL, [MaxLength] BIGINT NULL, EndPointUrl VARCHAR(2000) NULL);

		INSERT INTO @EntityConfig (EntityName, FieldName, EndPointUrl, IsRequired, [MaxLength])
			(SELECT x.y.value('(EntityName/text())[1]','VARCHAR(200)') AS EntityName,      
				x.y.value('(FieldName/text())[1]','VARCHAR(200)') AS FieldName,      
				x.y.value('(EndPointUrl/text())[1]','VARCHAR(2000)') AS EndPointUrl,      
				x.y.value('(IsRequired/text())[1]','BIT') AS IsRequired,
			  x.y.value('(MaxLength/text())[1]','BIGINT') AS [MaxLength]
			  FROM @XMLConfig.nodes('/Fields/Field') AS x (y));
		
		MERGE EntityConfiguration t
		USING @EntityConfig s
		ON t.EntityName = s.EntityName AND t.FieldName = s.FieldName AND ISNULL(t.EndPointUrl, '') = ISNULL(s.EndPointUrl, '')
		WHEN MATCHED      
		THEN UPDATE SET       
			t.IsRequired = s.IsRequired,      
			t.[MaxLength] = s.[MaxLength], 
			t.EndPointUrl = s.EndPointUrl,
			t.UpdatedDate = GETDATE()	
		WHEN NOT MATCHED BY TARGET THEN 
			INSERT (EntityName, FieldName, IsRequired, [MaxLength], EndPointUrl)   
				VALUES(s.EntityName, s.FieldName, s.IsRequired, s.[MaxLength], s.EndPointUrl);

	COMMIT TRANSACTION [ConfigurationTran]      
	END TRY      
	BEGIN CATCH      
	ROLLBACK TRANSACTION [ConfigurationTran]      
	DECLARE @ErrorMessage NVARCHAR(4000);      
	DECLARE @ErrorSeverity INT;      
	DECLARE @ErrorState INT;      
      
	SELECT @ErrorMessage = ERROR_MESSAGE(),      
    @ErrorSeverity = ERROR_SEVERITY(),      
    @ErrorState = ERROR_STATE();      
      
	RAISERROR (@ErrorMessage, @ErrorSeverity, @ErrorState);      
	END CATCH  
	END
END  
  
  
/*    ==Scripting Parameters==  
  
    Source Server Version : SQL Server 2014 (12.0.2000)  
    Source Database Engine Edition : Microsoft SQL Server Express Edition  
    Source Database Engine Type : Standalone SQL Server  
  
    Target Server Version : SQL Server 2017  
    Target Database Engine Edition : Microsoft SQL Server Standard Edition  
    Target Database Engine Type : Standalone SQL Server  
*/  
  
  