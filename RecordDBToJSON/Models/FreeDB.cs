using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordDBToJSON.Models
{
    public class FreeDB
    {
		public int Id { get; set; }

		public string Artist { get; set; }
		
		public int RecordId { get; set; }
		
		public string Record { get; set; }
		
		public int DiscId { get; set; }
		
		public string FreeDbId { get; set; }
		
		public string OtherFreeDbId { get; set; }
		
		public string Genre { get; set; }
		
		public int Revision { get; set; }
		
		public string Review { get; set; }
    }
}
