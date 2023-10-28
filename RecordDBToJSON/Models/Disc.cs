// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="Disc.cs" company="Software Inc.">
//   A.Robson
// </copyright>
// <summary>
//   Defines the Disc type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace RecordDBToJSON.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Disc
    {
        public int DiscId { get; set; }

        public int RecordId { get; set; }

        public int DiscNo { get; set; }

        public int FreeDbDiscId { get; set; }

        public string FreeDbId { get; set; }

        public int Length { get; set; }
    }
}
