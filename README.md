With the introduction of .Net we got a chance to build a console application using C# for creating a Movie Theatre Application for Movie Plex Theatre.
Part of their success comes from the fact that their business is automated and no human intervention is needed for any of their business processes.

From the project view we just have to make basic working of movie plex in which admin add on movies with their respective age limit/rate
and on the other side the user can access to list of movies. The user can choose between movies after validation.

The objective was to understand the concept of C# language and how implement it to the real world scenario. We have to make sure that the application has a flow to it. It cannot get stuck with weird or invalid user inputs.

We put our best efforts to Avoid crashing the application, because there is usually something shown to the user as to what went wrong like it sometimes contains stack trace. This kind of information enables a hacker to gain valuable knowledge into what kind of backend systems are being used. It is your job to catch any form of exceptions thrown and handle them in a proper way

The console application  was created using C# having Admin module and Guest module :

The Admin can login using predefined password up to 5 attempts

Admin can add  movies and their corresponding rating or age limit from list of movie ratings which are then stored in external file

The list of movies from the file are displayed to the Guest for selecting and to catch any form of exceptions thrown for invalid inputs 

The guest has to enter appropriate rating or age limit defined in the code

The application is free of stack trace which forbid a hacker to gain valuable knowledge into what kind of backend systems are being used
