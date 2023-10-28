// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FullRecord.cs" company="Software Inc.">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class FullRecord
    {
        #region " Properties "

        /// <summary>
        /// Gets or sets Record Unique Identifier
        /// </summary>
        public int RecordId { get; set; } // identity field

        /// <summary>
        /// Gets or sets Artist Id
        /// </summary>
        public int ArtistId { get; set; } // relate to the artist entity

        /// <summary>
        /// Gets or sets Record Name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets Record Field
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// Gets or sets Record Recorded
        /// </summary>
        public int Recorded { get; set; }

        /// <summary>
        /// Gets or sets Record Label
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets Record Pressing
        /// </summary>
        public string Pressing { get; set; }

        /// <summary>
        /// Gets or sets Record Rating
        /// </summary>
        public string Rating { get; set; }

        /// <summary>
        /// Gets or sets Record Discs
        /// </summary>
        public int Discs { get; set; }

        /// <summary>
        /// Gets or sets Record Media
        /// </summary>
        public string Media { get; set; }

        /// <summary>
        /// Gets or sets Record Bought
        /// </summary>
        public string Bought { get; set; }

        /// <summary>
        /// Gets or sets Record Cost
        /// </summary>
        public decimal Cost { get; set; }

        /// <summary>
        /// Gets or sets Cover name
        /// </summary>
        public string CoverName { get; set; }

        /// <summary>
        /// Gets or sets the review.
        /// </summary>
        public string Review { get; set; }

        /// <summary>
        /// Gets or sets FreeDB Id
        /// </summary>
        public int FreeDbId { get; set; }

        #endregion

    }
}
