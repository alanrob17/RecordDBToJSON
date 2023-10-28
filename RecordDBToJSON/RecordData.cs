// --------------------------------------------------------------------------------------------------------------------
// <copyright file="RecordData.cs" company="Software Inc.">
//   A.Robson
// </copyright>
// <summary>
//   Defines the Record type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------
namespace RecordDBToJSON
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using RecordDBToJSON.Components;

    /// <summary>
    /// The record data.
    /// </summary>
    public class RecordData
    {
        /// <summary>
        /// Select all Record records.
        /// </summary>
        /// <returns>The <see cref="List"/>list of all record objects.</returns>
        public List<Record> Select()
        {
            var dt = new DataTable();

            using (var cn = new SqlConnection(AppSettings.Instance.ConnectString))
            {
                var sql = "json_RecordSelectAll";
                var cmd = new SqlCommand(sql, cn) { CommandType = CommandType.StoredProcedure };

                var da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            // ArtistName = dr["FirstName"].ToString(),
            var query = from dr in dt.AsEnumerable()
                        select new Record
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
                            Review = dr["Review"].ToString()
                        };

            return query.ToList();
        }
    }
}
