// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ArtistData.cs" company="Software Inc.">
//   A.Robson
// </copyright>
// <summary>
//   Defines the Artist Data type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
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
    /// The artist data.
    /// </summary>
    public class ArtistData
    {
        #region " Methods "

        /// <summary>
        /// Select all Artist records.
        /// </summary>
        /// <returns>The <see cref="List"/>list of artists.</returns>
        public List<Artist> Select()
        {
            var dt = new DataTable();

            using (var cn = new SqlConnection(AppSettings.Instance.ConnectString))
            {
                var sql = "json_ArtistSelectAll";
                var cmd = new SqlCommand(sql, cn) { CommandType = CommandType.StoredProcedure };

                var da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            var query = from dr in dt.AsEnumerable()
                        select new Artist
                        {
                            ArtistId = Convert.ToInt32(dr["ArtistId"]),
                            FirstName = dr["FirstName"].ToString(),
                            LastName = dr["LastName"].ToString(),
                            Name = dr["Name"].ToString(),
                            Biography = dr["Biography"].ToString()
                        };

            return query.ToList();
        }

        #endregion
    }
}
