# Monster Trading Card Game
Monster Trading Card Game REST API in C# by Nico Lerchl for the lecture "Software Engineering 1" at the University of Applied Sciences Technikum Wien.

1. [Monster Trading Card Game](#monster-trading-card-game)
   1. [Approach](#approach)
   2. [Architecture](#architecture)
      1. [`ServerSocket`](#serversocket)
      2. [`ApiEndpointRegister`](#apiendpointregister)
      3. [`ApiEndpoint`](#apiendpoint)
      4. [`Logic`](#logic)
      5. [`Repository`](#repository)
   3. [Unit Tests](#unit-tests)
      1. [`ApiEndpointRegisterTest`](#apiendpointregistertest)
      2. [Logic](#logic-1)
   4. [Time spent](#time-spent)

## Approach
I started off writing the `ServerSocket` class to get going with the http part of the project as this was the greatest unknown I had. I then created the first endpoint to get a feel for how the rest of the infrastructure might pan out.

Writing my own small OR-Mapper was then next thing I then sought after.
This is where I abstracted the `Entity` and `Repository` classes. The SQL statements where built with a `StringBuilder` in the beginning.
This however made my programm vulnerable to SQL-Injections. Switching to `NpgsqlParameter`s solved this problem.
There have been lots of small changes on how entities are constructed and how and which properties are mapped to the database during the creation of the rest of the application.

Having the before mentioned constructs allowed me to implement most of the endpoints.
_Battles_ and _Trades_ however required more logic, which I did not just want to put in the endpoint classes.
I therefore wrote the abstract `Logic` class which works with the abstract `Repository` class.
Using this made it really simple to craft `Logic` classes for all entities, as I wanted to keep the code uniform.

The last bigger decision I made was to use custom exceptions, equivilant to HTTP-Statuscodes, to fully encapsulate the http parts from the logic parts.

## Architecture
I am going to describe the infrastructure of the application top down, as if we were an http request coming in.  
A big part of how the endpoints work is done right when the application starts however, long before any requests are made, so I will start there.

### `ServerSocket`
The programm creates once instance of the `ServerSocket` class which also creates once instance of the `ApiEndpointRegister` class.
It passes all classes that contain endpoints to its constructure.
The `ApiEndpointRegister` then registers all endpoints to be able to access them easily later on.

An endpoint is represented by a static method that is annotated with the `ApiEndpoint` attribute.
The attribute contains the http method, the path of the endpoint and whether the endpoint requires authentication or not.
The method itself has to return a `Reponse` and declares parameters which are then filled with the values from the request.

Now that the `ApiEndpointRegister` knows about all endpoints, it can be used to find the correct endpoint for a request.
The incoming request reaches the `ServerSocket`, which parses it and then tells the `ApiEndpointRegister` to execute the corresponding endpoint.

### `ApiEndpointRegister`
The `ApiEndpointRegister` will then look for the correct method to execute.
It will also check for the Bearer token, if the endpoint requires authentication.
Additionally it will try to pass all of the parameters that the endpoint requires.

There are four possible attributes a parameter can be annotated with:

- `Bearer`: The parameter will be filled with the Bearer token.
- `PathParam`: The parameter will be filled with the value of the path parameter. This is done through having a regex group in the url of the endpoint and naming the parameter the same or supplying the name to the attribute.
- `QueryParam`: The parameter will be filled with the value of the query parameter. This is done by looking for a query parameter with the same name as the parameter or the name supplied to the attribute.
- `Body`: This parameter will be filled with the body of the request. The parameter's datatype is used to deserialize the body.

The method can then then be executed with its parameters.

### `ApiEndpoint`
`ApiEndpoint` methods use `Logic`s according to the entity they are working with.
They always return a `Response` object.

### `Logic`
`Logic`s use `Repository`s to access the database.
They can also use other `Logic`s to access other entities.

`Logic`s use exceptions to signal errors to the `ApiEndpoint`s.

### `Repository`
`Repostitory`s access the database.
The abstract `Repository` class uses reflection to map the properties of an entity to the database.

These are the attributes used for mapping:

- `Column`: Supplies the name of the column in the database, if it is different from the property name.
- `Table`: Supplies the name of the table in the database, if it is different from the class name.
- `Id`: Used to mark the property that is the primary key of the table. It will therefore be ignored when saving the entity.
- `Transient`: Used to mark properties that should not be mapped to the database.

All properties that are not marked with `Transient` will be mapped to the database.
Using their class and property names if no `Table` or `Column` attributes are present.

## Unit Tests
### `ApiEndpointRegisterTest`
I chose to write unit tests for the `ApiEndpointRegister` as it is a crucial part of the application.
The class consists of the following three tests:

- Positive test to check if the correct endpoint is found and all types of parameters are correctly filled.
- Negative test for when the `ApiEndpoint`'s url has not been set.
- Negative test for when the with `ApiEndpoint` annotated method does not return a `Response`.

### Logic
All other tests are written for either the `TradeLogic` or `UserLogic` classes.

`UserLogic` as it also contains crucial parts of the application such as the registration and login. And I picked `TradeLogic` because its logic is probably the most complex.

## Time spent
I started developing on the 13th of September, as visible in the git history.
The project has therefore spanned over 4 months and I would estimate to have spent roughly 90 hours working on it.
Most of that time was probably spent on building the abstract infrastructure.
The project could therefore have been finished quicker but I am very pleased with how it turned out to be just like a little framework.
Further adaptions to the application would now be very easy to make.
