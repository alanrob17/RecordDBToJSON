// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FullRecordData.cs" company="Software Inc.">
//   A.Robson
// </copyright>
// <summary>
//   Defines the Full Record type.
//   This contains all of the fields in the database and also the correct data types.
//   These were dummed down for the Record class to use in MongoDB.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace RecordDBToJSON
{
    using RecordDBToJSON.Components;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FullRecordData
    {
        /// <summary>
        /// Select all Full Record records.
        /// </summary>
        /// <returns>The <see cref="List"/>list of all full record objects.</returns>
        public List<FullRecord> Select()
        {
            var dt = new DataTable();

            using (var cn = new SqlConnection(AppSettings.Instance.ConnectString))
            {
                var sql = "json_InternationalRecordSelectAll";
                var cmd = new SqlCommand(sql, cn) { CommandType = CommandType.StoredProcedure };

                var da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            // ArtistName = dr["FirstName"].ToString(),
            var query = from dr in dt.AsEnumerable()
                        select new FullRecord
                        {
                            ArtistId = Convert.ToInt32(dr["ArtistId"]),
                            RecordId = Convert.ToInt32(dr["RecordId"]),
                            Name = dr["Name"].ToString(),
                            Field = dr["Field"].ToString(),
                            Recorded = Convert.ToInt32(dr["Recorded"]),
                            Label = dr["Label"].ToString(),
                            Pressing = dr["Pressing"].ToString(),
                            Rating = dr["Rating"].ToString(),
                            Discs = Convert.ToInt32(dr["Discs"]),
                            Media = dr["Media"].ToString(),
                            Bought = dr["Bought"].ToString(),
                            Cost = DataConvert.ConvertTo<decimal>(dr["Cost"], default(decimal)),
                            Review = dr["Review"].ToString(),
                            FreeDbId = Convert.ToInt32(dr["FreeDbId"])
                        };

            return query.ToList();
        }
    }
}
