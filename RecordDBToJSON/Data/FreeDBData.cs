// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="FreeDBData.cs" company="Software Inc.">
//   A.Robson
// </copyright>
// <summary>
//   Defines the FrreeDB item Data type.
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

    public class FreeDBData
    {
        #region " Methods "

        /// <summary>
        /// Select all FreeDB records.
        /// </summary>
        /// <returns>The <see cref="List"/>list of FreeDB items.</returns>
        public List<FreeDB> Select()
        {
            var dt = new DataTable();

            using (var cn = new SqlConnection(AppSettings.Instance.ConnectString))
            {
                var sql = "adm_SelectAllFreeDBItems";
                var cmd = new SqlCommand(sql, cn) { CommandType = CommandType.StoredProcedure };

                var da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }

            var query = from dr in dt.AsEnumerable()
                        select new FreeDB
                        {
                            Id = Convert.ToInt32(dr["Id"]),
                            Artist = dr["Artist"].ToString(),
                            RecordId = Convert.ToInt32(dr["RecordId"]),
                            Record = dr["Record"].ToString(),
                            DiscId = Convert.ToInt32(dr["DiscId"]),
                            FreeDbId = dr["FreeDbId"].ToString(),
                            OtherFreeDbId = dr["OtherFreeDbId"].ToString(),
                            Genre = dr["Genre"].ToString(),
                            Revision = (dr["Revision"] == DBNull.Value) ? 0 : Convert.ToInt32(dr["Revision"]),
                            Review = dr["Review"].ToString(),

                        };

            return query.ToList();
        }

        #endregion
    }
}
