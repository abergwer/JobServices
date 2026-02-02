 ## Getting Started

### Prerequisites

- .NET 8 SDK or later
- MongoDB running and accessible

### Installation
 
1. Clone the repository
2. Update MongoDB connection settings in `appsettings.json`
3. Restore dependencies: `dotnet restore`
4. Build the project: `dotnet build`
5. Run the application: `dotnet run' <port>


# JobServices

A .NET 8 backend service for scheduling and managing asynchronous jobs with MongoDB integration. This project provides a robust job scheduling system with REST API endpoints for job management.

## Features

- **Job Scheduling**: Schedule jobs with configurable intervals (in seconds)
- **Job Management**: Create, update, delete, and retrieve jobs via REST API
- **Job Status Control**: Pause and resume job execution
- **Background Processing**: Dedicated background service for job execution
- **MongoDB Integration**: Persistent job storage with MongoDB
- **Logging**: Comprehensive logging throughout the application

## Technology Stack

- **.NET 8**: Latest .NET framework
- **ASP.NET Core**: Web API framework
- **MongoDB Driver**: Database operations
- **Swagger/OpenAPI**: API documentation

## Project Structure

### Core Services

#### `JobService`
Main service for job CRUD operations and database interactions.
- `CreateJob()`: Create a new scheduled job
- `GetAllJobs()`: Retrieve all jobs from the database
- `UpdateJob()`: Update an existing job
- `DeleteJob()`: Remove a job by name
- `StopJob()`: Pause job execution
- `ResumeJob()`: Resume paused job
- `CheckJobStatus()`: Check the status of a specific job

#### `JobSchedulerService`
Background service that runs continuously to execute scheduled jobs.
- Monitors job schedules every second
- Executes jobs when their `nextRun` time is reached
- Updates `nextRun` based on the job's schedule
- Logs job execution events

#### `MongoDbContext`
Database context for MongoDB collections management.
- Handles MongoDB connection
- Provides collection access for job persistence

### Models

#### `Job`
Represents a scheduled job with the following properties:
- `Id`: Unique identifier (GUID)
- `Name`: Job name
- `Type`: Job type/category
- `Payload`: Job data/parameters
- `Schedule`: Execution interval in seconds
- `Status`: Current status (Active/Paused)
- `nextRun`: Next scheduled execution time

#### `CreateJobRequest`
DTO for job creation requests:
- `Name`: Job name
- `Type`: Job type
- `Payload`: Job data
- `Schedule`: Interval in seconds (must be a valid integer)

## API Endpoints

The application exposes REST API endpoints through controllers for:
- Creating new jobs
- Retrieving all jobs or specific jobs
- Updating job details
- Deleting jobs
- Controlling job execution (start/stop/resume)

Access the Swagger UI at `/swagger` to explore and test all endpoints.

## Configuration

### Dependency Injection

Services are registered with the following lifetimes:
- **MongoDbContext**: Singleton (database connection pool)
- **JobService**: Singleton (used by the background scheduler)
- **IJobService**: Singleton interface implementation
- **JobSchedulerService**: Hosted service (application-scoped singleton)

### Logging

Configure logging in `appsettings.json`:

Logging settings are specified in the `Logging` section of `appsettings.json`. You can configure the log level and log destination (console, file, etc.) according to your preferences. For example:

```json
"Logging": {
  "LogLevel": {
    "Default": "Information",
    "Microsoft": "Warning",
    "Microsoft.Hosting.Lifetime": "Information"
  }
}
