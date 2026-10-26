# EventEase

An ASP.NET Core MVC application for an event-management company to manage venues, events and bookings. Staff maintain venue and event details, book venues for events without double-booking, and search bookings from a filterable dashboard. Data is stored in Azure SQL, with venue and event images in Azure Blob Storage.

Built for CLDV6211 at Emeris Durban North (2025).

## Features

- **Venues:** create, edit and remove venues with location, capacity, availability and an image.
- **Events:** manage events with a date, description, event type and image.
- **Bookings:** book a venue for an event on a date; the app rejects a booking when that venue is already booked for the event on the same day.
- **Booking dashboard:** search by booking ID or event name, and filter by event type, venue availability and date range.
- **Safe deletes:** venues and events with active bookings cannot be deleted.
- **Validation:** required fields and positive capacities are enforced with data annotations and shown on the forms.

## Tech stack

| Area | Technologies |
| --- | --- |
| Web app | C#, ASP.NET Core MVC (.NET 8), Razor, Bootstrap |
| Data | Azure SQL Database with Entity Framework Core |
| Storage | Azure Blob Storage |
| Hosting | Azure App Service |

## Getting started

### Prerequisites

- .NET 8 SDK
- A SQL Server or Azure SQL database
- An Azure Storage account with a blob container for images

### Run locally

1. Create the database tables to match the Entity Framework models in `TheEventEaseWebApp/Models` (venues, events, event types and bookings).
2. In `TheEventEaseWebApp`, copy `appsettings.Local.example.json` to `appsettings.Local.json` and set `ConnectionStrings:DefaultConnection` (SQL) and `ConnectionStrings:ContainerConnection` (Blob Storage).
3. Run the app:

```bash
dotnet run --project TheEventEaseWebApp
```

The environment variables `ConnectionStrings__DefaultConnection` and `ConnectionStrings__ContainerConnection` override the local JSON file, which is ignored by Git.

## Project structure

```text
TheEventEaseWebApp/
  Controllers/   Venue, event, booking and home controllers
  Models/        EF Core entities, view models and database context
  Views/         Razor views, including the booking dashboard
```

## Acknowledgements

Course material, documentation and adapted code patterns are credited in [CODE_ATTRIBUTION.txt](CODE_ATTRIBUTION.txt).
