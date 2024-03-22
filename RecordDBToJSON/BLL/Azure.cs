// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Azure.cs" company="Software Inc.">
//   A.Robson
// </copyright>
// <summary>
//   Routines to create Azure insert scripts.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace RecordDBToJSON.BLL
{
    using RecordDBToJSON.Data;
    using RecordDBToJSON.Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Azure
    {
        #region " Discs "

        /// <summary>
        /// Select disc list.
        /// </summary>
        /// <returns>The <see cref="List"/>list of discs.</returns>
        public List<Disc> SelectDiscs()
        {
            var discData = new DiscData();
            var discs = discData.Select();

            return discs;
        }

        /// <summary>
        /// Write discs text file for Azure.
        /// </summary>
        /// <param name="records">The discs list for Azure.</param>
        internal void WriteDiscText(List<Disc> discs)
        {
            var outFile = Environment.CurrentDirectory + "\\azure-discs.txt";
            var outStream = File.Create(outFile);
            var sw = new StreamWriter(outStream);

            foreach (var disc in discs)
            {
                var discText = FormatDiscText(disc);

                sw.WriteLine(discText);
            }

            // flush and close
            sw.Flush();
            sw.Close();
        }

        /// <summary>
        /// Format disc data for insert command.
        /// </summary>
        /// <param name="disc">The disc.</param>
        /// <returns>The <see cref="string"/>disc in SQL query format.</returns>
        internal static string FormatDiscText(Disc disc)
        {
            var d = new StringBuilder();

            d.Append($"SET IDENTITY_INSERT Disc ON\nINSERT INTO [Disc] ([DiscId], [RecordId],[DiscNo],[FreeDbDiscId],[FreeDbId],[Length]) VALUES ({disc.DiscId}, {disc.RecordId}, {disc.DiscNo}, {disc.FreeDbDiscId}, '{disc.FreeDbId}', {disc.Length});\nSET IDENTITY_INSERT Disc OFF\nGO");

            return d.ToString();
        }

        #endregion

        #region " Tracks "

        /// <summary>
        /// Select track list.
        /// </summary>
        /// <returns>The <see cref="List"/>list of tracks.</returns>
        public List<Track> SelectTracks()
        {
            var trackData = new TrackData();
            var tracks = trackData.Select();

            return tracks;
        }

        /// <summary>
        /// Write tracks text file for Azure.
        /// </summary>
        /// <param name="records">The tracks list for Azure.</param>
        internal void WriteTrackText(List<Track> tracks)
        {
            var outFile = Environment.CurrentDirectory + "\\azure-tracks.txt";
            var outStream = File.Create(outFile);
            var sw = new StreamWriter(outStream);

            foreach (var track in tracks)
            {
                var trackText = FormatTrackText(track);

                sw.WriteLine(trackText);
            }

            // flush and close
            sw.Flush();
            sw.Close();
        }

        private string FormatTrackText(Track track)
        {
            var t = new StringBuilder();

            track.Name = track.Name.Replace("'", "''");
            track.Name = track.Name.Replace('~', '"');
            track.Name = track.Name.Replace("\r\n", string.Empty);
            track.Extended = track.Extended.Replace("'", "''");
            track.Extended = track.Extended.Replace('~', '"');
            track.Extended = track.Extended.Replace("\r\n", string.Empty);

            t.Append($"SET IDENTITY_INSERT Track ON\nINSERT INTO [Track] ([TrackId],[DiscId],[TrackNo],[Name],[TrackLength],[Extended]) VALUES ({track.TrackId}, {track.DiscId}, {track.TrackNo}, '{track.Name}', {track.TrackLength}, '{track.Extended}');\nSET IDENTITY_INSERT Track OFF\nGO");

            return t.ToString();
        }

        #endregion

        #region " FreeDB "

        /// <summary>
        /// Select FreeDB list.
        /// </summary>
        /// <returns>The <see cref="List"/>list of FreeDB items.</returns>
        public List<FreeDB> SelectFreeDBItems()
        {
            var freeDBData = new FreeDBData();
            var items = freeDBData.Select();

            return items;
        }

        internal void WriteFreeDBText(List<FreeDB> items)
        {
            var outFile = Environment.CurrentDirectory + "\\azure-freedb.txt";
            var outStream = File.Create(outFile);
            var sw = new StreamWriter(outStream);

            foreach (var item in items)
            {
                var itemText = FormatFreeDBText(item);

                sw.WriteLine(itemText);
            }

            // flush and close
            sw.Flush();
            sw.Close();

        }

        private string FormatFreeDBText(FreeDB item)
        {
            var f = new StringBuilder();

            item.Artist = item.Artist.Replace("'", "''");
            item.Artist = item.Artist.Replace('~', '"');
            item.Artist = item.Artist.Replace("\r\n", string.Empty);
            item.Record = item.Record.Replace("'", "''");
            item.Record = item.Record.Replace('~', '"');
            item.Record = item.Record.Replace("\r\n", string.Empty);
            item.Genre = item.Genre.Replace("'", "''");
            item.Genre = item.Genre.Replace('~', '"');
            item.Genre = item.Genre.Replace("\r\n", string.Empty);
            item.Review = item.Review.Replace("'", "''");
            item.Review = item.Review.Replace('~', '"');
            item.Review = item.Review.Replace("\r\n", string.Empty);

            f.Append($"SET IDENTITY_INSERT FreeDB ON\nINSERT INTO [FreeDB] ([Id],[Artist],[RecordId],[Record],[DiscId],[FreeDbId],[OtherFreeDbId],[Genre],[Revision],[Review]) VALUES ({item.Id}, '{item.Artist}', {item.RecordId}, '{item.Record}', {item.DiscId}, '{item.FreeDbId}','{item.OtherFreeDbId}','{item.Genre}', {item.Revision}, '{item.Review}');\nSET IDENTITY_INSERT FreeDB OFF\nGO");

            return f.ToString();
        }

        #endregion
    }
}
