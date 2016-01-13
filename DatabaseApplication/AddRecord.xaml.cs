using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DatabaseApplication
{
    /// <summary>
    /// Interaction logic for AddRecord.xaml
    /// </summary>
    public partial class AddRecord : Window
    {
        //default constructor
        public AddRecord()
        {
            InitializeComponent();
        }//end default constructor

        //code to handle the Cancel button click event
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            //close the add window
            DialogResult = false;
        }//end Cancel_Click

        //code to handle the Add button click event
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //add the record to the database
            //the ID must be unique and specified or do not proceed with the add
            if (txtCustomerID.Text.Trim() == "")
            {
                MessageBox.Show("Missing unique Customer ID", "Entry error",
                    MessageBoxButton.OK, MessageBoxImage.Error);

                return;
            }

            //add the record
            Customer cust = new Customer();
            cust.CustomerID = txtCustomerID.Text.Trim();
            cust.CompanyName = txtCompanyName.Text.Trim();
            cust.ContactName = txtContactName.Text.Trim();
            cust.ContactTitle = txtContactTitle.Text.Trim();
            cust.Country = txtCountry.Text.Trim();
            cust.City = txtCity.Text.Trim();
            cust.Region = txtRegionState.Text.Trim();
            cust.PostalCode = txtPostalCode.Text.Trim();
            cust.Address = txtAddress.Text.Trim();
            cust.Phone = txtPhone.Text.Trim();
            cust.Fax = txtFax.Text.Trim();

            //main try block
            try
            {
                //create an instance of a data connection to the database customer table
                CustomerDataDataContext con = new CustomerDataDataContext();

                //add new record to the database
                con.Customers.InsertOnSubmit(cust);
                con.SubmitChanges();

                //tell the user this was added successfully
                MessageBox.Show("Customer " + cust.CustomerID + " was added successfully.");

                //refresh the data grid
                Application app = Application.Current;
                MainWindow main = (MainWindow)app.MainWindow;
                main.refreshGrid();

                //exit the dialog window with a success result
                DialogResult = true;
            }
            catch (Exception ex)
            {
                //tell the user this was successful
                MessageBox.Show("The item '" + cust.CustomerID + "' could not be added.  " + ex.ToString());

                //exit the dialog window without doing anything.
                DialogResult = false;
            }
        }//end btnAdd_Click

        private void txtCustomerID_Loaded(object sender, RoutedEventArgs e)
        {
            //put the cursor at the end of the customer ID textbox
            txtCustomerID.Focus();
        }//end txtCustomerID_Loaded
        
    }
}
