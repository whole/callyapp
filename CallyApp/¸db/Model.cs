using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace CallyApp.db
{
    [Table(Name = "Contacts")]
    class Contact
    {
        public Contact()
        {
           
        }

        [Column(IsPrimaryKey = true)]
        public Guid ContactId { get; set; } // this is also contact call number
        [Column]
        public string Title { get; set; }

        [Column]
        public string Number { get; set; }
        /// <summary>
        /// How much time did we visit (call) contact
        /// </summary>
        [Column]
        public int Visits { get; set; }

        public string TitleVisits { get { return Title + "(" + Visits + ")"; } } 
    }
}
