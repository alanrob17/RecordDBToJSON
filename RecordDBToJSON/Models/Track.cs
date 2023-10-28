// --------------------------------------------------------------------------------------------------------------------- 
// <copyright file="Track.cs" company="Software Inc.">
//   A.Robson
// </copyright>
// <summary>
//   Defines the Track type.
// </summary>
// ---------------------------------------------------------------------------------------------------------------------
namespace RecordDBToJSON.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Track
    {
        public int TrackId { get; set; }

        public int DiscId { get; set; }

        public int TrackNo { get; set; }

        public string Name { get; set; }

        public int TrackLength { get; set; }

        public string Extended { get; set; }
    }
}
