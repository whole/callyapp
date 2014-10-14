using CallyApp.db;
using Microsoft.Phone.UserData;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
namespace CallyApp.AppData
{
    public static class HelperClass
    {

        public static void CreateDatabase()
        {
            using (var ctx = new ContactContext("isostore:/Contacts.sdf"))
            {
                //for testing

                //if (ctx.DatabaseExists())
                //    ctx.DeleteDatabase();

                if (!ctx.DatabaseExists())
                {
                    ctx.CreateDatabase();
                    FetchContacts(ctx);
                }

                // if database exists update contacts
                if (ctx.DatabaseExists())
                {
                   // TODO Update baze novim kontaktima
                    UpdateDatabaseContacts();
                }
            }
        }

        private static void UpdateDatabaseContacts()
        {
            
                Contacts cons = new Contacts();
                cons.SearchCompleted += new EventHandler<ContactsSearchEventArgs>(UpdateContacts_SearchCompleted);
                cons.SearchAsync("", FilterKind.None, "");

        }

        private static void UpdateContacts_SearchCompleted(object sender, ContactsSearchEventArgs e)
        {
            using (var context = new ContactContext("isostore:/Contacts.sdf"))
            {

                var query = from CallyApp.db.Contact c in context.Contacts
                            orderby c.Visits descending
                            select c;
                var contactList = new ObservableCollection<CallyApp.db.Contact>(query);
                Contacts cons = new Contacts();
                // samo ako je broj kontakata razliciti onda updejtaj kontakte
                var brojKontaktaUAdresaru = e.Results.Count();
                if (brojKontaktaUAdresaru!= contactList.Count)
                {
                    // za svaki kontakt provjeri je li postoje brojevi
                    foreach (var contactAddressBook in e.Results)
                    {
                        string broj = contactAddressBook.PhoneNumbers.ToList().Count() > 0 ? contactAddressBook.PhoneNumbers.ToList()[0].ToString() : string.Empty;
                        string broj2 = contactAddressBook.PhoneNumbers.ToList().Count() > 1 ? contactAddressBook.PhoneNumbers.ToList()[1].ToString() : string.Empty;
                        string broj3 = contactAddressBook.PhoneNumbers.ToList().Count() > 2 ? contactAddressBook.PhoneNumbers.ToList()[2].ToString() : string.Empty;

                        if (!string.IsNullOrEmpty(broj) && !string.IsNullOrEmpty(contactAddressBook.DisplayName))
                        {
                            var jeLiPostojiKontakt = contactList.Where(c => c.Number == broj).Count() == 0;
                            if (jeLiPostojiKontakt)
                            {
                                // TODO gledati je li ime postoji, a ako postoji dodati ga pod Ime (2)
                                context.Contacts.InsertOnSubmit(new db.Contact { ContactId = Guid.NewGuid(), Number = broj, Title = contactAddressBook.DisplayName });
                            }
                        }
                    }
                    context.SubmitChanges();
                }
            }
        }
                      
         
              
        

        private static void FetchContacts(ContactContext ctx)
        {

            Contacts cons = new Contacts();
            cons.SearchCompleted += new EventHandler<ContactsSearchEventArgs>(Contacts_SearchCompleted);
            cons.SearchAsync(String.Empty, FilterKind.None, "");
        }

        static void Contacts_SearchCompleted(object sender, ContactsSearchEventArgs e)
        {
            using (var ctx = new ContactContext("isostore:/Contacts.sdf"))
            {
                //ctx.Contacts.InsertOnSubmit(new db.Contact { ContactId = Guid.NewGuid(), Number = "111", Title = "desana" });
                //ctx.Contacts.InsertOnSubmit(new db.Contact { ContactId = Guid.NewGuid(), Number = "222", Title = "isus" });

                // nakon sto se dobave kontakti dodaj ih u nasu listu
                foreach (Microsoft.Phone.UserData.Contact i in e.Results)
                {

                    string broj = i.PhoneNumbers.ToList().Count() > 0 ? i.PhoneNumbers.ToList()[0].ToString() : string.Empty;
                    string broj2 = i.PhoneNumbers.ToList().Count() > 1 ? i.PhoneNumbers.ToList()[1].ToString() : string.Empty;
                    string broj3 = i.PhoneNumbers.ToList().Count() > 2 ? i.PhoneNumbers.ToList()[2].ToString() : string.Empty;
                    if (i.DisplayName != null)
                    {
                        if (!string.IsNullOrEmpty(broj))
                        {
                            ctx.Contacts.InsertOnSubmit(new db.Contact { ContactId = Guid.NewGuid(), Number = broj, Title = i.DisplayName });

                            if (!string.IsNullOrEmpty(broj2))
                            {
                                ctx.Contacts.InsertOnSubmit(new db.Contact { ContactId = Guid.NewGuid(), Number = broj2, Title = i.DisplayName + " " + 2 });
                                if (!string.IsNullOrEmpty(broj3))
                                {
                                    ctx.Contacts.InsertOnSubmit(new db.Contact { ContactId = Guid.NewGuid(), Number = broj3, Title = i.DisplayName + " " + 3 });
                                }
                            }
                        }
                    }
                }
                ctx.SubmitChanges();
            }
        }
    }
}
    

    

