CREATE TABLE EntityConfiguration (
Id BIGINT IDENTITY,
EntityName VARCHAR(200) NOT NULL,
FieldName VARCHAR(200) NOT NULL,
IsRequired BIT NULL,
[MaxLength] BIGINT NULL,
EndpointUrl VARCHAR(2000) NULL,
IsActive BIT NULL DEFAULT 1,
CreatedDate DATETIME DEFAULT GETDATE(),
UpdatedDate DATETIME DEFAULT GETDATE()
)

--DROP TABLE EntityConfiguration
--insert into Product VAlues  ('QW', 'desc')

INSERT INTO EntityConfiguration (EntityName, FieldName, IsRequired, [MaxLength], EndpointUrl) VALUES ('Product', 'F1', 1, 10, 'Source1')
INSERT INTO EntityConfiguration (EntityName, FieldName, IsRequired, [MaxLength], EndpointUrl) VALUES ('Product', 'F2', 0, 14, 'Source1')
INSERT INTO EntityConfiguration (EntityName, FieldName, IsRequired, [MaxLength], EndpointUrl) VALUES ('Order', 'CF1', 1, 100, 'Source2')
INSERT INTO EntityConfiguration (EntityName, FieldName, IsRequired, [MaxLength], EndpointUrl) VALUES ('Order', 'CF2', 0, 100, 'Source2')
INSERT INTO EntityConfiguration (EntityName, FieldName, IsRequired, [MaxLength], EndpointUrl) VALUES ('Order', 'CF3', 1, 1000, 'Source2')
INSERT INTO EntityConfiguration (EntityName, FieldName, IsRequired, [MaxLength], EndpointUrl) VALUES ('Product', 'Field1', 1, 1000, 'Source1')