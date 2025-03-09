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
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace WPF_ZooManager
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;
        public MainWindow()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["WPF_ZooManager.Properties.Settings.CSharpTutorialDBConnectionString"].ConnectionString;

            sqlConnection = new SqlConnection(connectionString);

            ShowZoos();
            ShowAllAnimals();
        }

        private void ShowZoos()
        {
            try
            {
                string query = "select * from Zoo";

                // Running the query on the SQL Connection
                // SQLDataAdapter can be imagined like an interface to make tables usable by C#-objects
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable zooTable = new DataTable();

                    sqlDataAdapter.Fill(zooTable);

                    // Telling the programm which information of the table should be shown in the ListBox
                    listZoos.DisplayMemberPath = "Location";
                    // The value behind a selected item in the list is going to be the Id
                    listZoos.SelectedValuePath = "Id";
                    // The reference to the data the ListBox will show
                    listZoos.ItemsSource = zooTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void ShowAssociatedAnimals()
        {
            try
            {
                string query = "select * from Animal a inner join ZooAnimal za on a.Id = za.Animalid where za.Zooid = @ZooId";

                // To use @ZooId as a variable, the following sql command is needed
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    // Gives the variable @ZooId the value of the item that is selected in the ListBox
                    sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                    
                    DataTable animalTable = new DataTable();

                    sqlDataAdapter.Fill(animalTable);

                    listAssociatedAnimals.DisplayMemberPath = "Name";
                    listAssociatedAnimals.SelectedValuePath = "Id";
                    listAssociatedAnimals.ItemsSource = animalTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                // MessageBox.Show(e.ToString());
            }
        }

        
        private void ShowAllAnimals()
        {
            try
            {
                string query = "select * from Animal";

                // Using this it is not neccesary to write code that opens and closes the connection (as seen in method DeleteZoo_Click)
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable animalTable = new DataTable();

                    sqlDataAdapter.Fill(animalTable);

                    listAllAnimals.DisplayMemberPath = "Name";
                    listAllAnimals.SelectedValuePath = "Id";
                    listAllAnimals.ItemsSource = animalTable.DefaultView;
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }

        private void listZoos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowAssociatedAnimals();
            ShowSelectedZooInTextBox();
        }

        private void DeleteZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string queryDelete = "delete from Zoo where id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(queryDelete, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                // Exectuting the SQL Command
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                // Closing the SQL Connection
                sqlConnection.Close();

                // Updating the display
                ShowZoos();
            }
        }

        private void AddZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string queryAddZoo = "insert into Zoo values (@Location)";
                SqlCommand sqlCommand = new SqlCommand(queryAddZoo, sqlConnection);

                sqlConnection.Open();
                // Adding the text that was written into the TextBox to the database 
                sqlCommand.Parameters.AddWithValue("@Location", myTextBox.Text);
                // Exectuting the SQL Command
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
            }
        }

        private void AddAnimalToZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string queryAddAnimalToZoo = "insert into ZooAnimal values (@Zooid, @Animalid)";
                SqlCommand sqlCommand = new SqlCommand(queryAddAnimalToZoo, sqlConnection);

                sqlConnection.Open();
                // Adding the selected Animal and the selected Zoo to the ZooAnimal table
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Animalid", listAllAnimals.SelectedValue);
                // Exectuting the SQL Command
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAssociatedAnimals();
            }
        }

        private void RemoveAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string queryRemoveAnimal = "delete from ZooAnimal where Zooid = @ZooId and AnimalId = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(queryRemoveAnimal, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAssociatedAnimals.SelectedValue);
                // Exectuting the SQL Command
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                // Closing the SQL Connection
                sqlConnection.Close();

                // Updating the display
                ShowAssociatedAnimals();
            }
        }

        private void AddAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string queryAddAnimal = "insert into Animal values (@AnimalName)";
                SqlCommand sqlCommand = new SqlCommand(queryAddAnimal, sqlConnection);

                sqlConnection.Open();
                // Adding the Animal that was written in the TexBox to the Animal table
                sqlCommand.Parameters.AddWithValue("@AnimalName", myTextBox.Text);
                // Exectuting the SQL Command
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAllAnimals();
            }
        }

        private void DeleteAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string queryDeleteAnimal = "delete from Animal where id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(queryDeleteAnimal, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                // Exectuting the SQL Command
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                // Closing the SQL Connection
                sqlConnection.Close();

                // Updating the display
                ShowAllAnimals();
            }
        }

        private void ShowSelectedZooInTextBox()
        {
            try
            {
                string queryShowSelectedZoo = "select location from Zoo where Id = @ZooId";

                // To use @ZooId as a variable, the following sql command is needed
                SqlCommand sqlCommand = new SqlCommand(queryShowSelectedZoo, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    // Gives the variable @ZooId the value of the item that is selected in the ListBox
                    sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);

                    DataTable zooDataTable = new DataTable();

                    sqlDataAdapter.Fill(zooDataTable);

                    // Updating the TextBox with the very first entry at the "Location" of the zooDataTable
                    myTextBox.Text = zooDataTable.Rows[0]["Location"].ToString();
                }
            }
            catch (Exception e)
            {
                // MessageBox.Show(e.ToString());
            }
        }

        private void ShowSelectedAnimalInTextBox()
        {
            try
            {
                string queryShowSelectedAnimal = "select Name from Animal where Id = @AnimalId";

                // To use @ZooId as a variable, the following sql command is needed
                SqlCommand sqlCommand = new SqlCommand(queryShowSelectedAnimal, sqlConnection);

                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

                using (sqlDataAdapter)
                {
                    // Gives the variable @ZooId the value of the item that is selected in the ListBox
                    sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);

                    DataTable animalDataTable = new DataTable();

                    sqlDataAdapter.Fill(animalDataTable);

                    // Updating the TextBox with the very first entry at the "Location" of the zooDataTable
                    myTextBox.Text = animalDataTable.Rows[0]["Name"].ToString();
                }
            }
            catch (Exception e)
            {
                // MessageBox.Show(e.ToString());
            }
        }

        private void listAllAnimals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ShowSelectedAnimalInTextBox();
        }

        private void UpdateZoo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string queryUpdateZoo = "update Zoo Set Location = @Location where Id = @ZooId";
                SqlCommand sqlCommand = new SqlCommand(queryUpdateZoo, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ZooId", listZoos.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Location", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowZoos();
            }
        }

        private void UpdateAnimal_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string queryUpdateAnimal = "update Animal Set Name = @Name where Id = @AnimalId";
                SqlCommand sqlCommand = new SqlCommand(queryUpdateAnimal, sqlConnection);

                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@AnimalId", listAllAnimals.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@Name", myTextBox.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                ShowAllAnimals();
            }
        }
    }
}
