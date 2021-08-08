# ***Endpoint 1 Read Configuration:***
**Approach 1:**

##### GET {{localhost}}/api/Entity/

Fetch the data from two sources (i.e Source 1 and Source 2) and then merge the response. Once it is merged then join with the configuration that is available in the database and return the result.

**Approach 2:**

##### GET {{localhost}}/api/Entity?isAlternateApproach=true

Fetch the data from two sources (i.e Source 1 and Source 2) and then merge the response. Once it is merged then save the configuration to database. When saved, if the configuration is available then it will update them else add new configuration

Once it’s is saved then fetch the configuration that is available in the database and return the result.

We have coded the alternate approach as it was ambiguous that when we are saving/updating no source was provided. So initially the data would have been hardcoded.

Now, with this approach the configuration available from different sources can be saved in the database and in case the configuration is updated from the source the same will be updated/inserted in our database.

**Note:** The endpoint for both the approaches are same and is differentiated with a Boolean.

The response format is in below format:

```
{
    "responseCode": "OK",
    "message": "Configurations fetched successfully.",
    "data": [
        {
            "entityName": "Product",
            "fields": [
                {
                    "fieldName": "F1",
                    "isRequired": true,
                    "maxLength": 10,
                    "endPointUrl": "Source1"
                },
                {
                    "fieldName": "F2",
                    "isRequired": false,
                    "maxLength": 14,
                    "endPointUrl": "Source1"
                },
                {
                    "fieldName": "Field1",
                    "isRequired": false,
                    "maxLength": 0,
                    "endPointUrl": "Source1"
                },
                {
                    "fieldName": "Field2",
                    "isRequired": false,
                    "maxLength": 0,
                    "endPointUrl": "Source1"
                },
                {
                    "fieldName": "Field3",
                    "isRequired": false,
                    "maxLength": 0,
                    "endPointUrl": "Source1"
                }
                }
            ]
        },
        .....
    ]
}
```

**Steps:**

- The method calls the two mock api to get the configuration
- Both the responses are merged
- Fetch the configurations from database
- Get the distinct list of all the entities from mock response and db configuration
- Loop over the distinct entities and the filter the fields from both the lists
- If the configuration is present in the database, then skip those configuration 
- Convert the filtered output to the expected format

# ***Endpoint 2 Save Configuration:***
System shall be able to perform bulk insert/update operations. If a field specific entry is available then update that entry otherwise insert. 

The POST endpoint accepts the json in below format. In the stored procedure a MERGE statement is used for bulk insert/update.

##### POST {{localhost}}/api/Entity/

```
[
    {
        "EntityName": "Laptop1",
        "Fields": [
            {
                "FieldName": "123",
                "IsRequired": true,
                "MaxLength": 10
            }
        ]
    },
    {
        "EntityName": "Mouse",
        "Fields": [
            {
                "FieldName": "Field1",
                "IsRequired": true,
                "MaxLength": 10
            }
        ]
    },
    ....
]
```

# ***Clean up***
**Describe how you can clean up the configured data that's already saved in the database if any fields are removed from source2 later time. System shall not need to maintain configuration if fields are not available in either of the sources.**

We can write a merge statement to clean up the unused configuration.

The merge statement would be written such that if the conditions match then update the data and if not matched then delete the configurations.

