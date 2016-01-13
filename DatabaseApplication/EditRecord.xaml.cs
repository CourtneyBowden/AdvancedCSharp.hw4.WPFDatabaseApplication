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
    public partial class EditRecord : Window
    {
        //class variables
        protected string rowID; //stores the ID of the row being edited

        //constructor to accept the Row ID from the grid on the main window
        public EditRecord(string RowID)
        {
            InitializeComponent();
            rowID = RowID;
        }//end constructor with RowID

        //code to handle the Cancel button click event
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            //close the edit window
            DialogResult = false;
        }//end Cancel_Click

        //code to handle the Save button click event
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //main try/catch block
            try
            {
                //create an instance of a data connection to the database customer table
                CustomerDataDataContext con = new CustomerDataDataContext();

                // Query the database for the row to be updated. 
                var cust = con.Customers.SingleOrDefault(c => c.CustomerID == rowID);

                //set all of the field values the record
                cust.CustomerID = rowID;
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

                //save record to the database
                con.SubmitChanges();

                //tell the user this was updated successfully
                MessageBox.Show("Customer '" + rowID + "' was successfully updated.");

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
                MessageBox.Show("The item '" + rowID + "' could not be saved.  " + ex.ToString());

                //exit the dialog window without doing anything.
                DialogResult = false;
            }
        }//end btnSave_Click

        //pull the data from the database and place the initial values in each control on this form
        private void EditRecord_Loaded(object sender, RoutedEventArgs e)
        {
            //create an instance of a data connection to the database customer table
            CustomerDataDataContext con = new CustomerDataDataContext();

            // Query the database for the row to be updated. 
            var cust = con.Customers.SingleOrDefault(c => c.CustomerID == rowID);

            //the query should have returned 1 row
            if(cust != null)
            {
                //set all of the field values the record
                txtCustomerID.Text = cust.CustomerID;
                txtCompanyName.Text = cust.CompanyName;
                txtContactName.Text = cust.ContactName;
                txtContactTitle.Text = cust.ContactTitle;
                txtCountry.Text = cust.Country;
                txtCity.Text = cust.City;
                txtRegionState.Text = cust.Region;
                txtPostalCode.Text = cust.PostalCode;
                txtAddress.Text = cust.Address;
                txtPhone.Text = cust.Phone;
                txtFax.Text = cust.Fax;

                //put the cursor at the end of the company name textbox
                txtCompanyName.Focus();
                txtCompanyName.SelectionStart = txtCompanyName.Text.Length;
            }
        }//end EditRecord_Loaded
    }
}
