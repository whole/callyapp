using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CallyApp.db
{
    class ContactContext: DataContext
    {
        public ContactContext(string connstring) : base(connstring) { }

        public Table<Contact> Contacts;
    }
}
