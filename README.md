# Monster Trading Card Game
Monster Trading Card Game REST API in C# by Nico Lerchl for the lecture "Software Engineering 1" at the University of Applied Sciences Technikum Wien.

## Approach
I started off writing the `ServerSocket` class to get going with the http part of the project as this was the greatest unknown I had about the project. I then created the first endpoint to get a feel for how the rest of the infrastructure might pan out.

Writing my own small OR-Mapper was then next thing I then sought after.
This is where I abstracted the `Entity` and `Repository` classes. The SQL statements where built together with a `StringBuilder` in the beginning.
This however made my programm vulnerable to SQL-Injections. Switching to `NpgsqlParameter`s solved this problem.
There have been lots of small changes on how entities are constructed and how and which properties are mapped to the database during the creation of the rest of the application.

Having the before mentioned constructs allowed me to implement most of the endpoints.
_Battles_ and _Trades_ however required more logic, which I did not just want to put in the endpoint classes.
I therefore wrote the abstract `Logic` class which works with the abstract `Repository` class.
Using this made it really simple to craft `Logic`-Classes for all entities, as I wanted to keep the code uniform.

The last bigger decision I made was to use custom Exceptions, equivilant to HTTP-Statuscodes, to fully encapsulate the http parts from the logic parts.

## Architecture
I am going to describe the infrastructure of the application top down, as if we were an http request coming in.  
A big part of how the endpoints work is done right when the application starts however, long before any requests are made, so I will start there.

### `ServerSocket`
The programm creates once instance of the `ServerSocket` class which also creates once instance of the `EndpointRegister` class.
It passes all classes that contain endpoints to its constructure.
The `EndpointRegister` then registers all endpoints to be able to access them easily later on.

An endpoint is represented by a static method that is annotated with the `ApiEndpoint` attribute.
The attribute contains the http method, the path of the endpoint and whether the endpoint requires authentication or not.
The method itself has to return a `Reponse` and declares parameters which are then filled with the values from the request.

Now that the `EndpointRegister` knows about all endpoints, it can be used to find the correct endpoint for a request.
The incoming request reaches the `ServerSocket`, parses it and then tells the `EndpointRegister` to execute the corresponding endpoint. 

### `EndpointRegister`
