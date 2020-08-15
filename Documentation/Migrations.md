# Migrations

1. `appsettings.json` contains connection strings. The auth db (ApplicationDbContext) has to use the Identity connection string. 
This should already be set up. Then run this command after cd-ing into Dataflow:

2. Now we need to create a migration. This will look at the whole Identity structure set up by IdentityServer by default (IdentityUser, IdentityRole, etc...), along with our custom implementations (ApplicationUser, ApplicationRole), and custom table remapping (removing AspNet, i.e. AspNetUsers just becomes Users so we don't have AspNet all over our database). Most of the tables are set up automatically by Asp.Net, so we just really need to remap tables names. 

- CD into ./Database
- Run the following command: `dotnet ef --startup-project ../Webapp migrations add Initial  --context ApplicationDbContext --verbose`

We need to run this from Database because all the db info is there, but need to specify that Webapp is the startup project. This will add an "Initial" migration for `ApplicationDbContext`. Note that this won't create the database yet. It'll just set up all the information that Entity Framework migrations need to create that database. In general, we will not be using migrations. All the models will instead be reverse engineered from existing databases. EF has a nice reverse-engineer feature to handle this, with a few manual tweaks. Generally, each time a database upgrade needs to happen, you create a new migration and update the database. However, EF migrations have no elegant way to handle dynamically dropping/adding foreign/primary keys when updating tables. Since many of the tables are "properly" linked this way, making changes to the tables involving any of these relations or keys is a huge hassle. The only real way to do it is to drop/recreate the keys on each migrations, which you have to set up manually. To avoid this, we will only be working off of one "Master" migrations (the one we just created). That is, if we want to change the Identity db schema, we will do so by dropping the entire database, rolling back all migrations, and recreating a new "Master" migration that will recreate the db from scratch. As a result, updates to prod dbs can only involve additions, not deletions or updates. This let's us manually add the column to prod via an update script, avoid's the hassle of migrations, and avoids breaking reverse compatibility. Ideally there will be minimal changes to Identity tables. 

3. Now we need to actually create the database. This can be done using the following:

- CD into ./Database
- Run the following command: `dotnet ef --startup-project ../Webapp database update --context ApplicationDbContext --verbose`

You should have a database that matches your model now.
