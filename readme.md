## Crud API based on Mongo Db and Dotnet Core and Ef core 

This project also uses [Agoda IOC](https://github.com/agoda-com/Agoda.IoC) for dependency registration.

Am trying to cover multiple scenarios associated with migration when we use this setup in production below

Below is the current schema and data before we make any change to the schema 

Student schema 
```C#
 public class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Email { get; set; }

    }
```
Student Data

```json
[
  {
    "id": 1,
    "name": "string",
    "department": "string",
    "email": "string"
  },
  {
    "id": 2,
    "name": "Student Before model change",
    "department": "string",
    "email": "string"
  }
]
```
### Scenario 1 adding nullable types

```C#
 public class Student
 {
     public int Id { get; set; }
     public string Name { get; set; }
     public string Department { get; set; }
     public string Email { get; set; }
     public string Phone { get; set; } // newly added type 

 }
```

Student Data
```json
[
  {
    "id": 1,
    "name": "string",
    "department": "string",
    "email": "string",
    "phone": null
  },
  {
    "id": 2,
    "name": "Student Before model change",
    "department": "string",
    "email": "string",
    "phone": null
  }
]
```
### Scenario 2 adding non-nullable types

```C#
 public class Student
 {
     public int Id { get; set; }
     public string Name { get; set; }
     public string Department { get; set; }
     public string Email { get; set; }
     public string Phone { get; set; }
     public int Age { get; set; } // int is non nullable 
 }
```

we will get Serialisation exception , since the serialser doesnt know what to do with the new Non nullable `int Age`
```C#
System.Collections.Generic.KeyNotFoundException: The given key was not present in the dictionary.
   at MongoDB.EntityFrameworkCore.Serializers.SerializationHelper.ReadElementValue[T](BsonDocument document, BsonSerializationInfo elementSerializationInfo)
   at lambda_method5(Closure, QueryContext, BsonDocument)
```

- make it nullable in easy way
    ```C#
        public int? Age { get; set; }
    ```

- But there are high chances there will be breaking change to schema down the line , this can be controlled using versioing 
Lets again take the example of `int Age`
