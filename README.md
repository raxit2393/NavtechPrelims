# Navtech Prelims (.NET Coding Assignment)

## Requirements

Develop entities metadata configuration API. Create two endpoints to read and save the configuration

## Tools & Technologies
- Microsoft Visual Studio
- SQL Server Express Edition
- Postman
- ASP .NET Web API .NET Core (Framework .NET Core 3.1)
- ADO .NET

## Endpoint 1 - Read configuration requirement

Source 1 : GET {{localhost}}/api/DefaultFields/{entity}
Source 2 : GET {{localhost}}/api/CustomFields/{entity}

Approach 1:
GET {{localhost}}/api/Entity
Fetch the data from two sources (i.e Source 1 and Source 2) and then merge the response. Once it is merged then join with the configuration that is available in the database and return the result.

Approach 2: 
GET {{localhost}}/api/Entity?isAlternateApproach=true
Fetch the data from two sources (i.e Source 1 and Source 2) and then merge the response. Once it is merged then save the configuration to database. When saved, if the configuration is available then it will update them else add new configuration
Once it's is saved then fetch the configuration that is available in the database and return the result. 


## Endpoint 2 - Save configuration requirement
System shall be able to perform bulk insert/update operations. If a field specific entry is available then update that entry
otherwise insert.

