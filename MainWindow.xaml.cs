using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace Loan_System
{
    //applicant class
    public class Applicant
    {
        public string? Timestamp {  get; set; }
        public string? Official_Name {  get; set; }
        public int? Age { get; set; }
        public string? Sex { get; set; }
        public int? Government_ID { get; set; }
        public string? Government_ID_Photo { get; set; }
        public string? Occupation { get; set; }
        public string? PaySlipImage { get; set; }
        public string? LoanDuration { get; set; }
        public int? LoanAmount { get; set; }
        public int? PhoneNo { get; set; }
        public string? Email { get; set; }
        public string? ApplicantEssay { get; set; }
        public string? Status { get; set; }
        public string? Time_of_Update { get; set; }
    }
    
    //main window class
    public partial class MainWindow : Window
    {
        //initialize app
        public MainWindow()
        {
            InitializeComponent();
        }

        //ui control functions
        private void minBtn_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void maxBtn_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
            }
            else
            {
                this.WindowState = WindowState.Maximized;
            }
            
        }
        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void windowBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        
        //run python 
        private async Task RunPythonScript(string filepath, string textpath)
        {
            try
            { 
                string scriptPath = filepath;


                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "python", // runs python application
                    Arguments = scriptPath,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                };

                using (Process process = new Process { StartInfo = psi })
                {
                    process.Start();

                    // If you want to read the output of the Python script, you can do so
                    string output = await process.StandardOutput.ReadToEndAsync();
                    // Read the error output of the Python script
                    string error = await process.StandardError.ReadToEndAsync();
                    File.WriteAllText(textpath, output);

                    process.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred while running the Python script: {ex.Message}");
            };
        }
        
        //read output
        private List<Applicant> ReadDataFromFile(string filePath)
        {
            List<Applicant> applicants = new List<Applicant>();

            // Read all lines from the file into an array
            string[] lines = File.ReadAllLines(filePath);

            //MessageBox.Show($"{lines.Length}");

            foreach (string line in lines)
            {
                // Define the regex pattern for 15 values separated by pipes, allowing for empty values
                string pattern = @"\|([^|]*)\|([^|]*)\|([^|]*)\|([^|]*)\|([^|]*)\|([^|]*)\|([^|]*)\|([^|]*)\|([^|]*)\|([^|]*)\|([^|]*)\|([^|]*)\|([^|]*)\|([^|]*)\|([^|]*)\|";

                // Match the pattern in the input string
                Match match = Regex.Match(line, pattern);
                if (match.Success)
                {
                    applicants.Add(new Applicant
                    {
                        Timestamp = match.Groups[1].Value,
                        Official_Name = match.Groups[2].Value,
                        Age = int.Parse(match.Groups[3].Value),
                        Sex = match.Groups[4].Value,
                        Government_ID = int.Parse(match.Groups[5].Value),
                        Government_ID_Photo = match.Groups[6].Value,
                        Occupation = match.Groups[7].Value,
                        PaySlipImage = match.Groups[8].Value,
                        LoanDuration = match.Groups[9].Value,
                        LoanAmount = int.Parse(match.Groups[10].Value),
                        PhoneNo = int.Parse(match.Groups[11].Value),
                        Email = match.Groups[12].Value,
                        ApplicantEssay = match.Groups[13].Value,
                        Status = match.Groups[14].Value,
                        Time_of_Update = match.Groups[15].Value
                    });
                }
                else
                {
                    MessageBox.Show("match unsuccessful");
                }
            }

            return applicants;
        }
        
        //clicks
        private void pendingBtn_Click(object sender, RoutedEventArgs e)
        {
            //set pending grid visible
                pendingGrid.Visibility = Visibility.Visible;
                acceptedGrid.Visibility = Visibility.Collapsed;
                rejectedGrid.Visibility = Visibility.Collapsed;
            
        }
        private void acceptedBtn_Click(object sender, RoutedEventArgs e)
        {
            //set accepted grid visible
            pendingGrid.Visibility = Visibility.Collapsed;
            acceptedGrid.Visibility = Visibility.Visible;
            rejectedGrid.Visibility = Visibility.Collapsed;
            
        }
        private void rejectedBtn_Click(object sender, RoutedEventArgs e)
        {
            //set rejected grid visible
            pendingGrid.Visibility = Visibility.Collapsed;
            acceptedGrid.Visibility = Visibility.Collapsed;
            rejectedGrid.Visibility = Visibility.Visible;
            
        }

        //refresh applications
        private async void refreshBtn_Click(object sender, RoutedEventArgs e)
        {
            //loading start
            loadingIndicator.Visibility = Visibility.Visible;
            refreshBtn.Visibility = Visibility.Collapsed;

            //run the all python API's
            await RunPythonScript(@"""C:\Users\mule\source\repos\Loan System\py\pending.py""", "C:\\Users\\mule\\source\\repos\\Loan System\\text\\pending_applications.txt");
            await RunPythonScript(@"""C:\Users\mule\source\repos\Loan System\py\accepted.py""", "C:\\Users\\mule\\source\\repos\\Loan System\\text\\accepted_applications.txt");
            await RunPythonScript(@"""C:\Users\mule\source\repos\Loan System\py\rejected.py""", "C:\\Users\\mule\\source\\repos\\Loan System\\text\\rejected_applications.txt");

            //loading end
            loadingIndicator.Visibility = Visibility.Collapsed;
            refreshBtn.Visibility = Visibility.Visible;

            //display the data
            try
            {
                pending_ListView.ItemsSource = ReadDataFromFile("C:\\Users\\mule\\source\\repos\\Loan System\\text\\pending_applications.txt");
                accepted_ListView.ItemsSource = ReadDataFromFile("C:\\Users\\mule\\source\\repos\\Loan System\\text\\accepted_applications.txt");
                rejected_ListView.ItemsSource = ReadDataFromFile("C:\\Users\\mule\\source\\repos\\Loan System\\text\\rejected_applications.txt");
            }
            catch (Exception ex)
            {
                string excep = ex.Message.ToString();
                MessageBox.Show(excep);
            }
        }

        //open new window
        private void userinfo_pendingListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Applicant selectedApplicant = (Applicant)pending_ListView.SelectedItem;

            OpenNewWindowWithData(selectedApplicant);
            
        }
        private void userinfo_acceptedListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Applicant selectedApplicant = (Applicant)accepted_ListView.SelectedItem;

            OpenNewWindowWithData(selectedApplicant);

        }
        private void userinfo_rejectedListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Applicant selectedApplicant = (Applicant)rejected_ListView.SelectedItem;

            OpenNewWindowWithData(selectedApplicant);

        }
        private void OpenNewWindowWithData(Applicant selectedApplicant)
        {
            // Create a new window
            ApplicationReview newWindow = new ApplicationReview();

            // Pass the data to the new window (assuming MyNewWindow has a method to accept data)
            newWindow.SetData(selectedApplicant);
            
            //write data to a txt file
            newWindow.WriteToFile(selectedApplicant);

            // Show the new window
            newWindow.Show();
        }

        //searchbar
        private void Pending_SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = pending_searchTextBox.Text.ToLower(); // Convert to lowercase for case-insensitive search

            foreach (Applicant item in pending_ListView.Items)
            {
                if (item.Official_Name.ToLower().Contains(searchText) ||
                    item.ApplicantEssay.ToLower().Contains(searchText) ||
                    item.Age.ToString().ToLower().Contains(searchText) ||
                    item.Email.ToLower().Contains(searchText) ||
                    item.Timestamp.ToLower().Contains(searchText) ||
                    item.Time_of_Update.ToLower().Contains(searchText) ||
                    item.Sex.ToLower().Contains(searchText) ||
                    item.Government_ID.ToString().ToLower().Contains(searchText) ||
                    item.Government_ID_Photo.ToLower().Contains(searchText) ||
                    item.PaySlipImage.ToLower().Contains(searchText) ||
                    item.Occupation.ToLower().Contains(searchText) ||
                    item.LoanAmount.ToString().ToLower().Contains(searchText) ||
                    item.LoanDuration.ToLower().Contains(searchText) ||
                    item.Status.ToLower().Contains(searchText) ||
                    item.PhoneNo.ToString().ToLower().Contains(searchText) 
                    )
                {
                    // Item matches the search string, make it visible
                    ((ListViewItem)pending_ListView.ItemContainerGenerator.ContainerFromItem(item)).Visibility = Visibility.Visible;
                }
                else
                {
                    // Item doesn't match the search string, make it not visible
                    ((ListViewItem)pending_ListView.ItemContainerGenerator.ContainerFromItem(item)).Visibility = Visibility.Collapsed;
                }
            }
        }
        private void Rejected_SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = rejected_searchTextBox.Text.ToLower(); // Convert to lowercase for case-insensitive search

            foreach (Applicant item in rejected_ListView.Items)
            {
                if (item.Official_Name.ToLower().Contains(searchText) ||
                    item.ApplicantEssay.ToLower().Contains(searchText) ||
                    item.Age.ToString().ToLower().Contains(searchText) ||
                    item.Email.ToLower().Contains(searchText) ||
                    item.Timestamp.ToLower().Contains(searchText) ||
                    item.Time_of_Update.ToLower().Contains(searchText) ||
                    item.Sex.ToLower().Contains(searchText) ||
                    item.Government_ID.ToString().ToLower().Contains(searchText) ||
                    item.Government_ID_Photo.ToLower().Contains(searchText) ||
                    item.PaySlipImage.ToLower().Contains(searchText) ||
                    item.Occupation.ToLower().Contains(searchText) ||
                    item.LoanAmount.ToString().ToLower().Contains(searchText) ||
                    item.LoanDuration.ToLower().Contains(searchText) ||
                    item.Status.ToLower().Contains(searchText) ||
                    item.PhoneNo.ToString().ToLower().Contains(searchText)
                    )
                {
                    // Item matches the search string, make it visible
                    ((ListViewItem)rejected_ListView.ItemContainerGenerator.ContainerFromItem(item)).Visibility = Visibility.Visible;
                }
                else
                {
                    // Item doesn't match the search string, make it not visible
                    ((ListViewItem)rejected_ListView.ItemContainerGenerator.ContainerFromItem(item)).Visibility = Visibility.Collapsed;
                }
            }
        }
        private void Accepted_SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = accepted_searchTextBox.Text.ToLower(); // Convert to lowercase for case-insensitive search

            foreach (Applicant item in accepted_ListView.Items)
            {
                if (item.Official_Name.ToLower().Contains(searchText) ||
                    item.ApplicantEssay.ToLower().Contains(searchText) ||
                    item.Age.ToString().ToLower().Contains(searchText) ||
                    item.Email.ToLower().Contains(searchText) ||
                    item.Timestamp.ToLower().Contains(searchText) ||
                    item.Time_of_Update.ToLower().Contains(searchText) ||
                    item.Sex.ToLower().Contains(searchText) ||
                    item.Government_ID.ToString().ToLower().Contains(searchText) ||
                    item.Government_ID_Photo.ToLower().Contains(searchText) ||
                    item.PaySlipImage.ToLower().Contains(searchText) ||
                    item.Occupation.ToLower().Contains(searchText) ||
                    item.LoanAmount.ToString().ToLower().Contains(searchText) ||
                    item.LoanDuration.ToLower().Contains(searchText) ||
                    item.Status.ToLower().Contains(searchText) ||
                    item.PhoneNo.ToString().ToLower().Contains(searchText)
                    )
                {
                    // Item matches the search string, make it visible
                    ((ListViewItem)accepted_ListView.ItemContainerGenerator.ContainerFromItem(item)).Visibility = Visibility.Visible;
                }
                else
                {
                    // Item doesn't match the search string, make it not visible
                    ((ListViewItem)accepted_ListView.ItemContainerGenerator.ContainerFromItem(item)).Visibility = Visibility.Collapsed;
                }
            }
        }

    }

}
