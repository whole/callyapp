using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using CallyApp.Resources;
using CallyApp.db;
using System.Collections.ObjectModel;
using Microsoft.Phone.Tasks;
using System.Threading.Tasks;
using Telerik.Windows.Controls;

namespace CallyApp
{
    
    public partial class MainPage : PhoneApplicationPage
    {
        ContactContext context;
        ObservableCollection<Contact> contactList;
        /// <summary>
        /// Double Tap event always preceded by Tap so we wait for some time before calling functions on tap
        /// </summary>
        bool singleTap;
        public MainPage()
        {
            InitializeComponent();
            CallyApp.AppData.HelperClass.CreateDatabase();
            context = new ContactContext("isostore:/Contacts.sdf");
            RefreshDataSource();
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            RefreshDataSource();
        }

        #region Events
        private async void llsContacts_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var contact = llsContacts.SelectedItem as Contact;
            this.singleTap = true;
            await Task.Delay(200);
            if (contact != null && singleTap)
            {
                contact.Visits = contact.Visits + 1;
                context.SubmitChanges();
                CallContact(contact.Title, contact.Number);

            }
        }

        private void llsContacts_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            var contact = llsContacts.SelectedItem as Contact;
            singleTap = false;
            if (contact != null)
            {
                //contact.Visits = contact.Visits + 1;
                //context.SubmitChanges();
                SendSms(contact.Number);

            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshDataSource();
        }
        #endregion

        #region PhoneFunctions
        private void CallContact(string name, string number)
        {
            PhoneCallTask phoneCallTask = new PhoneCallTask();
            phoneCallTask.PhoneNumber = number;
            phoneCallTask.DisplayName = name;
            phoneCallTask.Show();
        }

        private void SendSms(string number)
        {
            SmsComposeTask smsComposeTask = new SmsComposeTask();
            smsComposeTask.To = number;
            smsComposeTask.Show();
        }
        
        #endregion

        #region Private Methods
        private void RefreshDataSource()
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
            {
                var query = from Contact c in context.Contacts
                            orderby c.Visits descending
                            select c;
                contactList = new ObservableCollection<Contact>(query);
              
                llsContacts.ItemsSource = contactList;
      
            }

            else
            {
                var query = from Contact c in context.Contacts
                            where c.Title.Contains(txtSearch.Text) || c.Number.Contains(txtSearch.Text)
                            orderby c.Visits descending
                            select c;
                contactList = new ObservableCollection<Contact>(query);
                llsContacts.ItemsSource = contactList;
            }
        }

        #endregion
    }
}