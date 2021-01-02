# IMDb
My own IMDb dataset importer - loads into a Marten DB document store.

This is just extracted from a private project, since it can be open-sourced to help you get started using the IMDb datasets.

## Solution Overview

### IMDb.Core

* Contains the core models representing rows in each dataset
* Provides the parsers that can parse the dataset into models (includes built in batching, as to not run out of memory)
* Repository interfaces to abstract the infrastructure layer

### IMDb.Infrastructure

* Implements the repository interfaces with Marten DB document stores
    * Marten uses Postgres underneath
    * The batch sync uses Postgres' `COPY` function internally for faster bulk inserts than establishing a connection per operation - doing it one by one would cause 2 connection lifetimes per row. The default batching (for loading into memory, and then sending to bulk insert) is 50k
* Provides an extension method to add the Marten Identities `StoreOptions.SetupIMDbIdentities()` to reduce duplicated code
    * For example, in an ASP.NET Core project you would run:
```cs
services.AddMarten(options =>
{
    options.AutoCreateSchemaObjects = AutoCreate.All;

    options.Connection(Configuration.GetConnectionString("Marten"));

    options.SetupIMDbIdentities();
});
```

### IMDb.Importer

* Upon startup it runs a full sync of the IMDb datasets found [here](https://datasets.imdbws.com/)
* There is no scheduling for now, to rerun, restart the container/console app
  * To run every day/week etc, look at a project like [Quartz.NET](https://www.quartz-scheduler.net/)
  
## Docker

This project contains out of the box Docker + `docker-compose` support.

To orchestrate the postgres database, and IMDb importer, simply run `docker-compose up -d --build` (`-d` makes it run in the background, `--build` builds the Docker images)

Feel free to edit the environment variables found in the `docker-compose.yml` file to configure to your needs

### Environment Variables

* `POSTGRES_USER`, `POSTGRES_PASSWORD`, and `POSTGRES_DB` - setup the Postgres database with these default settings - only sets up on the first run.
* `MARTEN_CONNECTION_STRING` - the connection string the the `postgres` service, this uses the Postgres configuration from above, feel free to change if the database is not in the docker-compose services
* `BATCH_SIZE` - the amount of rows to load at a time, this can be lowered if RAM is an issue
* `IMDB_BASE_PATH` - the directory to save IMDb raw dataset files to. Remove this entirely to save to the `current working directory/imdb`. By default in the docker compose project this is set to `/imdb` as this folder is volume mounted
* `DATABASE_NAME` - the name of the database, used for some direct queries after the import for statistics

## Credits

* [IMDb](https://www.imdb.com/) - for providing the datasets
* [Marten](https://martendb.io/) - for the document store abstracting Postgres
* [Postgres](https://www.postgresql.org/) - the database
* [Docker](https://www.docker.com/) - containerization