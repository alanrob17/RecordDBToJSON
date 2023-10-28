// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="ArtistData.cs" company="Software Inc.">
//   A.Robson
// </copyright>
// <summary>
//   Defines the Disc Data type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace RecordDBToJSON.Data
{
    using RecordDBToJSON.Components;
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using RecordDBToJSON.Models;

    public class DiscData
    {
        #region " Methods "

        /// <summary>
        /// Select all Disc records.
        /// </summary>
        /// <returns>The <see cref="List"/>list of artists.</returns>
        public List<Disc> Select()
        {
            var dt = new DataTable();

            using (var cn = new SqlConnection(AppSettings.Instance.ConnectString))
            {
                var sql = "adm_SelectAllDiscs";
                var cmd = new SqlCommand(sql, cn) { CommandType = CommandType.StoredProcedure };

                var da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            var query = from dr in dt.AsEnumerable()
                        select new Disc
                        {
                            DiscId = Convert.ToInt32(dr["DiscId"]),
                            RecordId = Convert.ToInt32(dr["RecordId"]),
                            DiscNo = Convert.ToInt32(dr["DiscNo"]),
                            FreeDbDiscId = (dr["FreeDbDiscId"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["FreeDbDiscId"]),
                            FreeDbId = dr["FreeDbId"].ToString(),
                            Length = (dr["Length"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["Length"])
                        };

            return query.ToList();
        }

        #endregion
    }
}
