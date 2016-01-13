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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DatabaseApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //default constructor
        public MainWindow()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.SetData("DataDirectory", System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Databases"));
        }//end default constructor

        //fire this event immediateley after the grid is loaded
        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            //call the function that refreshes the grid data
            refreshGrid();

            //set the active row to row 1
            CustomerDataGrid.SelectedIndex = 0;
        }//end Grid_Loaded

        //function to refresh the data in the grid
        //this is not only called here, but from the Add and Edit Record forms 
        //and from the Delete Record method 
        public void refreshGrid()
        {
            //create an instance of a data connection to the database customer table
            CustomerDataDataContext con = new CustomerDataDataContext();

            //create a customer list
            List<Customer> customers = (from c in con.Customers select c).ToList();
            CustomerDataGrid.ItemsSource = customers;
        }//end refreshGrid

        //menu item click event for Add Record
        private void mnuitemAddRecord_Click(object sender, RoutedEventArgs e)
        {
            AddRecord addRecordWindow = new AddRecord();
            addRecordWindow.ShowDialog();
        }//end mnuitemAddRecord_Click

        //menu item click event for Edit Record
        private void mnuitemEditRecord_Click(object sender, RoutedEventArgs e)
        {
            //local variables
            string rowID;

            //figure out which row in the datagrid is selected
            object item = CustomerDataGrid.SelectedItem;

            //try block to prevent an error found during coding.  The delete method throws an error if no row
            //is selected in the datagrid.
            try
            {
                rowID = (CustomerDataGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            }
            catch
            {
                //no rows are highlighted in the grid
                MessageBox.Show("Please select a row in the datagrid to edit and try again.");
                return;
            }

            //check to make sure we received a valid rowID
            if (rowID != null)
            {
                //show the Edit Record window
                EditRecord editRecordWindow = new EditRecord(rowID);
                editRecordWindow.ShowDialog();
            }
        }//end mnuitemEditRecord_Click

        //menu item click event for Delete Record
        private void mnuitmDeleteRecord_Click(object sender, RoutedEventArgs e)
        {
            //local variables
            string rowID;

            //create an instance of a data connection to the database customer table
            CustomerDataDataContext con = new CustomerDataDataContext();

            //figure out which row in the datagrid is selected
            object item = CustomerDataGrid.SelectedItem;
            //try block to prevent an error found during coding.  The delete method throws an error if no row
            //is selected in the datagrid.
            try
            {
                rowID = (CustomerDataGrid.SelectedCells[0].Column.GetCellContent(item) as TextBlock).Text;
            }
            catch
            {
                //no rows are highlighted in the grid
                MessageBox.Show("Please select a row in the datagrid to delete and try again.");
                return;
            }
            
            //check to make sure we received a valid rowID
            if (rowID != null)
            {
                //main try/catch block
                try
                {
                    //get the target row 
                    var cust =
                        (from c in con.Customers
                         where c.CustomerID == rowID
                         select c).First();

                    //delete this record from the database
                    con.Customers.DeleteOnSubmit(cust);
                    con.SubmitChanges();

                    //refresh the datagrid
                    refreshGrid();

                    //tell the user this was successful
                    MessageBox.Show("The item '" + rowID + "' was deleted successfully.");
                }
                catch (Exception ex)
                {
                    //tell the user this was successful
                    MessageBox.Show("The item '" + rowID + "' failed to delete.  " + ex.ToString());
                }
            }


        }//end mnuitmDeleteRecord_Click

        //menu item click event for Exit
        private void mnuitmExit_Click(object sender, RoutedEventArgs e)
        {
            //shutdown
            Application.Current.Shutdown();
        }//end mnuitmExit_Click
    }
}
