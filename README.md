<h1>Flight Information</h1>

This is an ASP.NET Core Web Application representing a flight information system. The home page of this app provides a list of flights where each flight can be selected to display a corresponding list of passengers. Flights can be added to the flight list and passengers can be added to each flight. Data is stored in 3 tables (Flights, Passengers, and Tickets). There is a many-to-many relationship between Flights and Passengers so Tickets serves as a many-to-many join table.

<h2>Technologies used</h2>
<ul>
     <li>Visual Studio 2019 16.9.6</li>
     <li>.NET 5.0 SDK</li>
     <li>SQL Server LocalDB</li>
     <li>Entity Framework Core</li>
</ul>

<h2>Usage</h2>

The home page of the app (*Flights/Index*) displays a table of flights. Flights can be added to this table with the "<strong>Create New</strong>" button.

Each flight in the table can be selected with the "<strong>Select</strong>" button to display a table of passengers registered for the flight.

Passengers can be created with the "<strong>Create New</strong>" button on the Passengers page (*Passengers/Index*). 

Passengers can be added to a flight when creating or editing an existing flight on the *Flights/Index* page by checking the box that corresponds to the passengers name and ID.

The *DbInitializer.Initialize* method populates the database with some test data if there are no flights in the database.


  

