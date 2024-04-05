using RecordDBToJSON.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RecordDBToJSON.BLL
{
    public class Postgres
    {
        internal void WritePostgresText(List<Artist> artists, List<Record> records)
        {
            var outFile = Environment.CurrentDirectory + "\\postgres-data.txt";
            var outStream = File.Create(outFile);
            var sw = new StreamWriter(outStream);

            var artistCreateText = GetArtistCreateText();
            sw.WriteLine(artistCreateText);

            var artistList = BuildArtistText(artists);
            sw.WriteLine(artistList);

            string recordCreateText = GetRecordCreateText();
            sw.WriteLine(recordCreateText);

            var recordList = BuildRecordText(records);
            sw.WriteLine(recordList);

            // flush and close
            sw.Flush();
            sw.Close();

        }

        private static string BuildArtistText(List<Artist> artists)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var artist in artists)
            {
                var artistText = FormatArtistText(artist);

                sb.Append(artistText);
            }

            // replace last comma with a ;
            var artistList = sb.ToString();
            var iposn = artistList.LastIndexOf(",");
            artistList = artistList.Substring(0, iposn);
            artistList = artistList + ";\n";

            return artistList;
        }

        private static string BuildRecordText(List<Record> records)
        {
            StringBuilder sb = new StringBuilder();

            foreach (var record in records)
            {
                var recordText = FormatRecordText(record);

                sb.Append(recordText);
            }

            // replace last comma with a ;
            var recordList = sb.ToString();
            var iposn = recordList.LastIndexOf(",");
            recordList = recordList.Substring(0, iposn);
            recordList = recordList + ";\n";

            return recordList;
        }

        private string GetArtistCreateText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("CREATE TABLE artist (\n");
            sb.Append("artistid SERIAL PRIMARY KEY,\n");
            sb.Append("firstname VARCHAR(50),\n");
            sb.Append("lastname VARCHAR(50) NOT NULL,\n");
            sb.Append("name VARCHAR(100),\n");
            sb.Append("biography TEXT\n");
            sb.Append(");\n");
            sb.Append("\n");
            sb.Append("INSERT INTO artist (artistid, firstname, lastname, name, biography) VALUES");

            return sb.ToString();
        }

        private static string FormatArtistText(Artist artist)
        {
            var sb = new StringBuilder();
            var firstName = artist.FirstName;
            firstName = firstName.Replace("'", "''");
            var lastName = artist.LastName;
            lastName = lastName.Replace("'", "''");
            var name = artist.Name;
            name = name.Replace("'", "''");

            var biography = artist.Biography.Replace("\r\n", string.Empty);
            biography = biography.Replace("'", "''");
            sb.Append($"({artist.ArtistId}, '{firstName}', '{lastName}', '{name}', '{biography}'),\n");

            return sb.ToString();
        }

        private string GetRecordCreateText()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append("CREATE TABLE record (\n");
            sb.Append("recordid SERIAL PRIMARY KEY,\n");
            sb.Append("artistid INT NOT NULL,\n");
            sb.Append("name VARCHAR(80) NOT NULL,\n");
            sb.Append("field VARCHAR(50) NOT NULL,\n");
            sb.Append("recorded INT NOT NULL,\n");
            sb.Append("label VARCHAR(50) NOT NULL,\n");
            sb.Append("pressing VARCHAR(50) NOT NULL,\n");
            sb.Append("rating VARCHAR(4) NOT NULL,\n");
            sb.Append("discs INT NOT NULL,\n");
            sb.Append("media VARCHAR(50) NOT NULL,\n");
            sb.Append("bought DATE,\n");
            sb.Append("cost MONEY,\n");
            sb.Append("review TEXT,\n");
            sb.Append("FOREIGN KEY (artistid) REFERENCES artist(artistid) ON DELETE CASCADE\n");
            sb.Append(");\n\n");
            sb.Append("INSERT INTO record (recordid, artistid, name, field, recorded, label, pressing, rating, discs, media, bought, cost, review) VALUES");

            return sb.ToString();
        }

        private static string FormatRecordText(Record record)
        {
            var sb = new StringBuilder();
            var review = record.Review.Replace("\r\n", string.Empty);
            review = review.Replace("'", "''");
            var name = record.Name;
            name = name.Replace("'", "''");
            var label = record.Label;
            label = label.Replace("'", "''");

            var bought = record.Bought;

            if (bought != string.Empty)
            {
                var year = record.Bought.Substring(6, 4);
                var month = record.Bought.Substring(3, 2);
                var day = record.Bought.Substring(0, 2);
                bought = $"{year}-{month}-{day}";
            }
            else
            {
                bought = "1900-01-01";
            }

            var cost = record.Cost;
            cost = Math.Round(cost, 2);

            sb.Append($"({record.RecordId}, {record.ArtistId}, '{name}', '{record.Field}', {record.Recorded}, '{label}', '{record.Pressing}', '{record.Rating}', {record.Discs}, '{record.Media}', '{bought}', {record.Cost}, '{review}'),\n");
            return sb.ToString();
        }
    }
}
