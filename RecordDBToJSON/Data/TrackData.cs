// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="TrackData.cs" company="Software Inc.">
//   A.Robson
// </copyright>
// <summary>
//   Defines the Track Data type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace RecordDBToJSON.Data
{
    using RecordDBToJSON.Components;
    using RecordDBToJSON.Models;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class TrackData
    {
        #region " Properties "

        /// <summary>
        /// Select all Track records.
        /// </summary>
        /// <returns>The <see cref="List"/>list of tracks.</returns>
        public List<Track> Select()
        {
            var dt = new DataTable();

            using (var cn = new SqlConnection(AppSettings.Instance.ConnectString))
            {
                var sql = "adm_SelectAllTracks";
                var cmd = new SqlCommand(sql, cn) { CommandType = CommandType.StoredProcedure };

                var da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            var query = from dr in dt.AsEnumerable()
                        select new Track
                        {
                            TrackId = Convert.ToInt32(dr["TrackId"]),
                            DiscId = Convert.ToInt32(dr["DiscId"]),
                            TrackNo = (dr["TrackNo"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["TrackNo"]),
                            Name = dr["Name"].ToString(),
                            TrackLength = (dr["TrackLength"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["TrackLength"]),
                            Extended = dr["Extended"].ToString()
                        };

            return query.ToList();
        }

        #endregion
    }
}
