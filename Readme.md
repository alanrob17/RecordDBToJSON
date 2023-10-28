## Record DB TO JSON

A project to export my SQL Server record database into a JSON format.

I create two JSON files

``artists-records.json`` is all of my artists with a nested list of their records. I use this file as the data for the website:

[recordlist.netlify.app](https://recordlist.netlify.app "recordlist.netlify.app")

``records.json`` is the list of records that have been joined to my list of artists from MongoDB. I have added the artist objectId to each record.

I then copy this file to another project named **record-db-to-mongo** that is used to import my data into MongoDB Compass.

### To-Do

I have expanded this program to create versions of my RecordDB that can be used in a number of other databases.

In the process I haven't planned this project properly. I am going to have to refactor this code.
