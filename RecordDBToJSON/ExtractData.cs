// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExtractData.cs" company="Software Inc.">
//   A.Robson
// </copyright>
// <summary>
//   Create JSON from my Record collection.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace RecordDBToJSON
{
    using BLL;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Extract data.
    /// </summary>
    public class ExtractData
    {
        public static int Main(string[] args)
        {
            var artists = SelectArtists();
            var azureArtists = artists;
            var records = SelectRecords();
            var azureRecords = SelectAzureRecords();

            Azure azure = new Azure();
            var discs = azure.SelectDiscs();
            azure.WriteDiscText(discs);

            var tracks = azure.SelectTracks();
            azure.WriteTrackText(tracks);

            var items = azure.SelectFreeDBItems();
            azure.WriteFreeDBText(items);

            var postgres = new Postgres();
            postgres.WritePostgresText(artists, records);

            // Artist-Record list for Netlify
            WriteArtistRecordJson(artists, records);
            CleanJson("artists-records.json");
            ReformatJsonToJavaScript("artists-records.json");

            WriteArtistJson(artists);
            CleanJson("artists.json");

            WriteRecordJson(records);
            CleanJson("records.json");

            WriteArtistText(artists);

            // Note: this method is only used to fix up a problem in the Azure Artist table
            // UpdateArtistText(artists);
            WriteAzureArtistText(azureArtists);

            WriteRecordText(records);

            WriteAzureRecordText(azureRecords);

            WriteMysqlArtistText(artists);
            WriteMysqlRecordText(records);

            Console.WriteLine("Finished...");

            return 0;
        }

        private static void WriteMysqlRecordText(List<Record> records)
        {
            var outFile = Environment.CurrentDirectory + "\\mysql-python-records.py";
            var outStream = File.Create(outFile);
            var sw = new StreamWriter(outStream);

            var recordText = FormatRecordHeader();

            foreach (var currentRecord in records)
            {
                var record = currentRecord;

                string recordLine = FormatRecordMySqlText(record);
                recordText.AppendLine(recordLine);
            }

            recordText = FormatMySqlFooter(recordText);

            sw.WriteLine(recordText.ToString());

            // flush and close
            sw.Flush();
            sw.Close();
        }

        private static string FormatRecordMySqlText(Record record)
        {
            string review = record.Review;
            review = review.Replace("\"", "\\\"");
            review = review.Replace(@"''", @"\'");
            review = review.Replace(@"‘", @"\'");
            review = review.Replace(@"’", @"\'");
            review = review.Replace(@"“", "\\\"");
            review = review.Replace(@"”", "\\\"");
            string name = record.Name;
            name = name.Replace(@"''", @"\'");
            string bought = record.Bought;
            bought = ReformatDateString(bought);

            string recordText = $"\t(\"{record.RecordId}\",\"{record.ArtistId}\", \"{name}\", \"{record.Field}\", \"{record.Recorded}\", \"{record.Label}\", \"{record.Pressing}\", \"{record.Rating}\", \"{record.Discs}\", \"{record.Media}\", \"{bought}\", \"{record.Cost}\", \"{review}\"),";

            return recordText;
        }

        private static string ReformatDateString(string bought)
        {
            string dateString = string.Empty;

            string[] dateArray = bought.Split('-');
            if (!string.IsNullOrEmpty(bought))
            {
                dateString = $"{dateArray[2]}-{dateArray[1]}-{dateArray[0]} 12:00:00";
            }
            else
            {
                dateString = "1900-01-01 12:00:00";
            }

            return dateString;
        }

        private static StringBuilder FormatRecordHeader()
        {
            var sb = new StringBuilder();
            sb.AppendLine("import mysql.connector");
            sb.AppendLine("");
            sb.AppendLine("db = mysql.connector.connect(");
            sb.AppendLine("\thost=\"localhost\", user=\"root\", password=\"london@1\", database=\"RecordDB\"");
            sb.AppendLine(")");
            sb.AppendLine("");
            sb.AppendLine("cursor = db.cursor()");
            sb.AppendLine("");
            sb.AppendLine("sql = \"INSERT INTO Record (RecordId, ArtistId, Name, Field, Recorded, Label, Pressing, Rating, Discs, Media, Bought, Cost, Review) VALUES (%s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s, %s)\"");
            sb.AppendLine("");
            sb.AppendLine("items = [");

            return (sb);
        }

        private static void WriteMysqlArtistText(List<Artist> artists)
        {
            var outFile = Environment.CurrentDirectory + "\\mysql-python-artists.py";
            var outStream = File.Create(outFile);
            var sw = new StreamWriter(outStream);

            var artistText = FormatArtistHeader();
            
            foreach (var currentArtist in artists)
            {
                var artist = currentArtist;

                string artistLine = FormatArtistMySqlText(artist);
                artistText.AppendLine(artistLine);
            }

            artistText = FormatMySqlFooter(artistText);

            sw.WriteLine(artistText.ToString());

            // flush and close
            sw.Flush();
            sw.Close();
        }

        private static string FormatArtistMySqlText(Artist artist)
        {
            string biography = artist.Biography;
            biography = biography.Replace("\"", "\\\"");
            biography = biography.Replace(@"''''", @"\'");
            string lastName = artist.LastName;
            lastName = lastName.Replace(@"''''", @"\'");
            string name = artist.Name;
            name = name.Replace(@"''''", @"\'");

            string artistText = $"\t(\"{artist.ArtistId}\", \"{artist.FirstName}\", \"{lastName}\", \"{name}\", \"{biography}\"),";

            return artistText;
        }

        private static StringBuilder FormatArtistHeader()
        {
            var sb = new StringBuilder();
            sb.AppendLine("import mysql.connector");
            sb.AppendLine("");
            sb.AppendLine("db = mysql.connector.connect(");
            sb.AppendLine("\thost=\"localhost\", user=\"root\", password=\"london@1\", database=\"RecordDB\"");
            sb.AppendLine(")");
            sb.AppendLine("");
            sb.AppendLine("cursor = db.cursor()");
            sb.AppendLine("");
            sb.AppendLine("sql = \"INSERT INTO Artist (ArtistId, FirstName, LastName, Name, Biography) VALUES (%s, %s, %s, %s, %s)\"");
            sb.AppendLine("");
            sb.AppendLine("items = [");

            return (sb);
        }

        private static StringBuilder FormatMySqlFooter(StringBuilder sb)
        {
            sb.AppendLine("]");
            sb.AppendLine("");
            sb.AppendLine("cursor.executemany(sql, items)");
            sb.AppendLine("");
            sb.AppendLine("db.commit()");
            sb.AppendLine("");
            sb.AppendLine("print(f\"{cursor.rowcount} were inserted.\")");

            return (sb);
        }

        /// <summary>
        /// Add JavaScript code to JSON file for Netlify
        /// </summary>
        /// <param name="fileName">The JSON file.</param>
        private static void ReformatJsonToJavaScript(string fileName)
        {
            if (File.Exists(Environment.CurrentDirectory + "\\" + fileName))
            {
                var lines = File.ReadAllLines(Environment.CurrentDirectory + "\\" + fileName);

                List<string> newLines = new List<string>();

                newLines.Add("'use strict';");
                newLines.Add("const artistList = [");

                for (int i = 0; i < lines.Length - 1; i++)
                {
                    if (i > 0)
                    {
                        newLines.Add(lines[i]);
                    }
                }

                newLines.Add("];");

                var newFile = "record-data.js";

                File.WriteAllLines(Environment.CurrentDirectory + "\\" + newFile, newLines, Encoding.UTF8);
            }
        }

        /// <summary>
        /// Write records text file for Azure.
        /// </summary>
        /// <param name="records">The Records list for azure.</param>
        private static void WriteAzureRecordText(List<FullRecord> records)
        {
            var outFile = Environment.CurrentDirectory + "\\azure-records.txt";
            var outStream = File.Create(outFile);
            var sw = new StreamWriter(outStream);

            foreach (var currentRecord in records)
            {
                var record = currentRecord;

                var recordText = FormatAzureFullRecordText(record);

                sw.WriteLine(recordText);
            }

            // flush and close
            sw.Flush();
            sw.Close();
        }

        /// <summary>
        /// Write records text file for Sqlite.
        /// </summary>
        /// <param name="records">The Records list for Sqlite.</param>
        private static void WriteRecordText(List<Record> records)
        {
            var outFile = Environment.CurrentDirectory + "\\records.txt";
            var outStream = File.Create(outFile);
            var sw = new StreamWriter(outStream);

            foreach (var currentRecord in records)
            {
                var record = currentRecord;

                var recordText = FormatRecordText(record);

                sw.WriteLine(recordText);
            }

            // flush and close
            sw.Flush();
            sw.Close();
        }

        /// <summary>
        /// Format subset of record data for insert command.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns>The <see cref="string"/>record in SQL query format.</returns>
        private static string FormatRecordText(Record record)
        {
            var r = new StringBuilder();

            record.Name = record.Name.Replace("'", "''");
            record.Label = record.Label.Replace("'", "''");
            record.Review = record.Review.Replace("'", "''");
            record.Review = record.Review.Replace('~', '"');
            record.Review = record.Review.Replace("\r\n", string.Empty);

            r.Append($"INSERT INTO [Record] ([RecordId],[ArtistId],[Name],[Field],[Recorded],[Label],[Pressing],[Rating],[Discs],[Media],[Bought],[Cost],[Review]) VALUES ({record.RecordId}, {record.ArtistId}, '{record.Name}', '{record.Field}', {record.Recorded}, '{record.Label}', '{record.Pressing}', '{record.Rating}', {record.Discs}, '{record.Media}', '{record.Bought}', {record.Cost}, '{record.Review}');");

            return r.ToString();
        }

        /// <summary>
        /// Format full record data for insert command.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns>The <see cref="string"/>record in SQL query format.</returns>
        private static string FormatFullRecordText(FullRecord record)
        {
            var r = new StringBuilder();

            record.Name = record.Name.Replace("'", "''");
            record.Label = record.Label.Replace("'", "''");
            record.Review = record.Review.Replace("'", "''");
            record.Review = record.Review.Replace('~', '"');
            record.Review = record.Review.Replace("\r\n", string.Empty);

            r.Append($"INSERT INTO [Record] ([RecordId],[ArtistId],[Name],[Field],[Recorded],[Label],[Pressing],[Rating],[Discs],[Media],[Bought],[Cost],[CoverName],[Review],[FreeDbId]) VALUES ({record.RecordId}, {record.ArtistId}, '{record.Name}', '{record.Field}', {record.Recorded}, '{record.Label}', '{record.Pressing}', '{record.Rating}', {record.Discs}, '{record.Media}', '{record.Bought}', {record.Cost}, '{record.CoverName}', '{record.Review}', {record.FreeDbId});");

            return r.ToString();
        }

        /// <summary>
        /// Format Azure full record data for insert command.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns>The <see cref="string"/>record in SQL query format.</returns>
        private static string FormatAzureFullRecordText(FullRecord record)
        {
            var r = new StringBuilder();

            record.Name = record.Name.Replace("'", "''");
            record.Label = record.Label.Replace("'", "''");
            record.Review = record.Review.Replace("'", "''");
            record.Review = record.Review.Replace('~', '"');
            record.Review = record.Review.Replace("\r\n", string.Empty);

            r.Append($"SET IDENTITY_INSERT Record ON\nINSERT INTO [Record] ([RecordId],[ArtistId],[Name],[Field],[Recorded],[Label],[Pressing],[Rating],[Discs],[Media],[Bought],[Cost],[CoverName],[Review],[FreeDbId]) VALUES ({record.RecordId}, {record.ArtistId}, '{record.Name}', '{record.Field}', {record.Recorded}, '{record.Label}', '{record.Pressing}', '{record.Rating}', {record.Discs}, '{record.Media}', '{record.Bought}', {record.Cost}, '{record.CoverName}', '{record.Review}', {record.FreeDbId});\nSET IDENTITY_INSERT Record OFF\nGO");

            return r.ToString();
        }

        /// <summary>
        /// Write azure Artist text file.
        /// </summary>
        /// <param name="artists">The Artists list for azure.</param>
        private static void WriteAzureArtistText(List<Artist> artists)
        {
            var outFile = Environment.CurrentDirectory + "\\azure-artists.txt";
            var outStream = File.Create(outFile);
            var sw = new StreamWriter(outStream);

            foreach (var currentArtist in artists)
            {
                var artist = currentArtist;

                var artistText = FormatAzureArtistText(artist);

                sw.WriteLine(artistText);
            }

            // flush and close
            sw.Flush();
            sw.Close();
        }

        /// <summary>
        /// Write Artist text file.
        /// </summary>
        /// <param name="artists">The Artists.</param>
        private static void WriteArtistText(List<Artist> artists)
        {
            var outFile = Environment.CurrentDirectory + "\\artists.txt";
            var outStream = File.Create(outFile);
            var sw = new StreamWriter(outStream);

            foreach (var currentArtist in artists)
            {
                var artist = currentArtist;

                var artistText = FormatArtistText(artist);
                
                sw.WriteLine(artistText);
            }

            // flush and close
            sw.Flush();
            sw.Close();
        }

        /// <summary>
        /// Format artist data for insert command.
        /// </summary>
        /// <param name="artist">The artist.</param>
        /// <returns>The <see cref="string"/>artist in SQL query format.</returns>
        private static string FormatArtistText(Artist artist)
        {
            var a = new StringBuilder();

            artist.FirstName = artist.FirstName.Replace("'", "''");

            artist.LastName = artist.LastName.Replace("'", "''");
            artist.Name = artist.Name.Replace("'", "''");
            artist.Biography = artist.Biography.Replace("'", "''");
            artist.Biography = artist.Biography.Replace('~', '"');
            artist.Biography = artist.Biography.Replace("\r\n", string.Empty);

            a.Append($"INSERT INTO Artist (ArtistId, FirstName, LastName, Name, Biography) VALUES ({artist.ArtistId}, '{artist.FirstName}', '{artist.LastName}', '{artist.Name}', '{artist.Biography}');");

            return a.ToString();
        }

        /// <summary>
        /// Format Azure artist data for insert command.
        /// </summary>
        /// <param name="artist">The artist.</param>
        /// <returns>The <see cref="string"/>artist in SQL query format.</returns>
        private static string FormatAzureArtistText(Artist artist)
        {
            var a = new StringBuilder();

            artist.FirstName = artist.FirstName.Replace("'", "''");

            artist.LastName = artist.LastName.Replace("'", "''");
            artist.Name = artist.Name.Replace("'", "''");
            // artist.Biography = artist.Biography.Replace("'", "''");
            artist.Biography = artist.Biography.Replace('~', '"');
            artist.Biography = artist.Biography.Replace("\r\n", string.Empty);

            a.Append($"SET IDENTITY_INSERT Artist ON\nINSERT INTO Artist (ArtistId, FirstName, LastName, Name, Biography) VALUES ({artist.ArtistId}, '{artist.FirstName}', '{artist.LastName}', '{artist.Name}', '{artist.Biography}');\nSET IDENTITY_INSERT Artist OFF\nGO");

            return a.ToString();
        }

        /// <summary>
        /// Clean JSON remove comma from second last line.
        /// </summary>
        /// <param name="fileName">The file Name.</param>
        private static void CleanJson(string fileName)
        {
            if (File.Exists(Environment.CurrentDirectory + "\\" + fileName))
            {
                var lines = File.ReadAllLines(Environment.CurrentDirectory + "\\" + fileName);

                lines[lines.Length - 2] = lines[lines.Length - 2].Replace(",", string.Empty);

                for (var x = 0; x < lines.Length; x++)
                {
                    lines[x] = lines[x].Replace("~", "\\\"");
                }

                File.WriteAllLines(Environment.CurrentDirectory + "\\" + fileName, lines, Encoding.UTF8);
            }
        }

        /// <summary>
        /// Write Record JSON to file.
        /// </summary>
        /// <param name="artists">The artist list.</param>
        /// <param name="records">The record list.</param>
        private static void WriteArtistRecordJson(IEnumerable<Artist> artists, List<Record> records)
        {
            var outFile = Environment.CurrentDirectory + "\\artists-records.json";
            var outStream = File.Create(outFile);
            var sw = new StreamWriter(outStream);

            sw.WriteLine("["); // opening JSON bracket

            foreach (var currentArtist in artists)
            {
                var artist = currentArtist;
                var recordList = from r in records where artist.ArtistId == r.ArtistId select r;

                var artistJson = FormatArtist(artist);

                artistJson = artistJson.Replace("/images/", "/assets/images/");

                sw.WriteLine(artistJson);

                var recordJson = FormatArtistRecord(recordList);

                sw.WriteLine(recordJson);
            }

            sw.WriteLine("]"); // closing JSON bracket

            // flush and close
            sw.Flush();
            sw.Close();
        }

        /// <summary>
        /// Write Artist JSON for MongoDB.
        /// </summary>
        /// <param name="artists">The Artists.</param>
        private static void WriteArtistJson(IEnumerable<Artist> artists)
        {
            var outFile = Environment.CurrentDirectory + "\\artists.json";
            var outStream = File.Create(outFile);
            var sw = new StreamWriter(outStream);

            sw.WriteLine("["); // opening JSON bracket

            foreach (var currentArtist in artists)
            {
                var artist = currentArtist;

                var artistJson = FormatArtist(artist);

                artistJson = artistJson.Replace("/images/", "/assets/images/");
                artistJson = artistJson.Substring(0, artistJson.Length - 1);
                artistJson += "\n\t},";

                sw.WriteLine(artistJson);
            }

            sw.WriteLine("]"); // closing JSON bracket

            // flush and close
            sw.Flush();
            sw.Close();
        }

        /// <summary>
        /// Write Record JSON for MongoDB.
        /// </summary>
        /// <param name="records">The Records.</param>
        private static void WriteRecordJson(IEnumerable<Record> records)
        {
            var outFile = Environment.CurrentDirectory + "\\records.json";
            var outStream = File.Create(outFile);
            var sw = new StreamWriter(outStream);

            sw.WriteLine("["); // opening JSON bracket

            foreach (var currentRecord in records)
            {
                var record = currentRecord;

                var recordJson = FormatRecord(record);
                recordJson = recordJson.Substring(0, recordJson.Length - 1);
                recordJson += "\n\t},";

                sw.WriteLine(recordJson);
            }

            sw.WriteLine("]"); // closing JSON bracket

            // flush and close
            sw.Flush();
            sw.Close();
        }

        /// <summary>
        /// Format record data into JSON.
        /// </summary>
        /// <param name="record">The record.</param>
        /// <returns>The <see cref="string"/>record in JSON.</returns>
        private static string FormatRecord(Record record)
        {
            var r = new StringBuilder();

            record.Review = record.Review.Replace('"', '~');
            record.Review = record.Review.Replace("\r\n", string.Empty);

            r.Append("\t{\n");
            r.Append("\t\t\"recordid\": " + record.RecordId + ",\n");
            r.Append("\t\t\"artistid\": " + record.ArtistId + ",\n");
            r.Append("\t\t\"name\": \"" + record.Name + "\",\n");
            r.Append("\t\t\"field\": \"" + record.Field + "\",\n");
            r.Append("\t\t\"recorded\": " + record.Recorded + ",\n");
            r.Append("\t\t\"label\": \"" + record.Label + "\",\n");
            r.Append("\t\t\"pressing\": \"" + record.Pressing + "\",\n");
            r.Append("\t\t\"rating\": \"" + record.Rating + "\",\n");
            r.Append("\t\t\"discs\": " + record.Discs + ",\n");
            r.Append("\t\t\"media\": \"" + record.Media + "\",\n");
            r.Append("\t\t\"bought\": \"" + record.Bought + "\",\n");
            r.Append("\t\t\"cost\": \"" + record.Cost + "\",\n");
            r.Append("\t\t\"review\": \"" + record.Review + "\",");

            return r.ToString();
        }

        /// <summary>
        /// Format record into JSON.
        /// </summary>
        /// <param name="records">The records.</param>
        /// <returns>The <see cref="string"/>record list in JSON format.</returns>
        private static string FormatArtistRecord(IEnumerable<Record> records)
        {
            var r = new StringBuilder();

            r.Append("\t\t\"record\": [\n");

            foreach (var record in records)
            {
                var name = record.Name;

                if (name.Contains("\""))
                {
                    name = name.Replace("\"", "~");
                }

                record.Review = record.Review.Replace('"', '~');
                record.Review = record.Review.Replace("\r\n", string.Empty);

                r.Append("\t\t\t{\n");
                r.Append("\t\t\t\t\"recordid\": " + record.RecordId + ",\n");
                r.Append("\t\t\t\t\"artistid\": " + record.ArtistId + ",\n");
                r.Append("\t\t\t\t\"name\": \"" + name + "\",\n");
                r.Append("\t\t\t\t\"field\": \"" + record.Field + "\",\n");
                r.Append("\t\t\t\t\"recorded\": " + record.Recorded + ",\n");
                r.Append("\t\t\t\t\"label\": \"" + record.Label + "\",\n");
                r.Append("\t\t\t\t\"pressing\": \"" + record.Pressing + "\",\n");
                r.Append("\t\t\t\t\"rating\": \"" + record.Rating + "\",\n");
                r.Append("\t\t\t\t\"discs\": " + record.Discs + ",\n");
                r.Append("\t\t\t\t\"media\": \"" + record.Media + "\",\n");
                r.Append("\t\t\t\t\"bought\": \"" + record.Bought + "\",\n");
                r.Append("\t\t\t\t\"cost\": \"" + record.Cost.ToString("C") + "\",\n");
                r.Append("\t\t\t\t\"review\": \"" + record.Review + "\"\n");
                r.Append("\t\t\t},\n");
            }

            var recordList = r.ToString();
            recordList = recordList.Substring(0, recordList.Length - 2) + "\n";
            recordList += "\t\t]\n\t},";

            return recordList;
        }

        /// <summary>
        /// Format artist record into JSON.
        /// </summary>
        /// <param name="artist">The artist.</param>
        /// <returns>The <see cref="string"/>artist in JSON format.</returns>
        private static string FormatArtist(Artist artist)
        {
            var a = new StringBuilder();

            artist.Biography = artist.Biography.Replace('"', '~');
            artist.Biography = artist.Biography.Replace("\r\n", string.Empty);

            a.Append("\t{\n");
            a.Append("\t\t\"artistid\": " + artist.ArtistId + ",\n");
            a.Append("\t\t\"firstname\": \"" + artist.FirstName + "\",\n");
            a.Append("\t\t\"lastname\": \"" + artist.LastName + "\",\n");
            a.Append("\t\t\"name\": \"" + artist.Name + "\",\n");
            a.Append("\t\t\"biography\": \"" + artist.Biography + "\",");

            return a.ToString();
        }

        /// <summary>
        /// Select records.
        /// </summary>
        /// <returns>The <see cref="List"/> records.</returns>
        private static List<Record> SelectRecords()
        {
            var recordData = new RecordData();
            var records = recordData.Select();

            return records;
        }

        /// <summary>
        /// Select artists.
        /// </summary>
        /// <returns>The <see cref="List"/>artists.</returns>
        private static List<Artist> SelectArtists()
        {
            var artistData = new ArtistData();
            var artists = artistData.Select();

            return artists;
        }

        /// <summary>
        /// Select full record list.
        /// </summary>
        /// <returns>The <see cref="List"/>records for Azure.</returns>
        private static List<FullRecord> SelectAzureRecords()
        {
            var recordData = new FullRecordData();
            var records = recordData.Select();

            return records;
        }

        /// <summary>
        /// // Note: this method is only used to fix up a problem in the Azure Artist table
        /// </summary>
        /// <param name="artists">The Artists.</param>
        private static void UpdateArtistText(List<Artist> artists)
        {
            var outFile = Environment.CurrentDirectory + "\\updateartists.txt";
            var outStream = File.Create(outFile);
            var sw = new StreamWriter(outStream);

            foreach (var currentArtist in artists)
            {
                var artist = currentArtist;

                var artistText = FormatUpdateArtistText(artist);

                sw.WriteLine(artistText);
            }

            // flush and close
            sw.Flush();
            sw.Close();
        }

        // Note: this method is only used to fix up a problem in the Azure Artist table
        private static string FormatUpdateArtistText(Artist artist)
        {
            var a = new StringBuilder();

            // artist.Biography = artist.Biography.Replace("'", "''");
            artist.Biography = artist.Biography.Replace('~', '"');
            artist.Biography = artist.Biography.Replace("\r\n", string.Empty);

            if (artist.Biography.Length > 0 )
            {
                a.Append($"UPDATE Artist SET Biography = '{artist.Biography}' WHERE ArtistId = {artist.ArtistId};\nGO");
            }
            else
            {
                a.Append($"UPDATE Artist SET Biography = NULL WHERE ArtistId = {artist.ArtistId};\nGO");
            }

            return a.ToString();
        }
    }
}
