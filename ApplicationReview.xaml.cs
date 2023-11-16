using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
using System.Xml;

namespace Loan_System
{

    public partial class ApplicationReview : Window
    {
        public ApplicationReview()
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
        //write to line
        private void WriteToLine(string filePath, int lineNumber, string content)
        {
            // Read all lines from the file
            string[] lines = File.ReadAllLines(filePath);

            // Update the specified line
            if (lineNumber > 0 && lineNumber <= lines.Length)
            {
                lines[lineNumber - 1] = content;
            }
            else
            {
                return;
            }

            // Write the updated content back to the file
            File.WriteAllLines(filePath, lines);
        }
        //tab actions
        private async void approvedBtn_Click(object sender, RoutedEventArgs e)
        {
            //loading start
            loadingIndicator.Visibility = Visibility.Visible;

            rejectBtn.Visibility = Visibility.Collapsed;
            approvedBtn.Visibility = Visibility.Collapsed;
            waitapprovalBtn.Visibility = Visibility.Collapsed;

            //update txt file line 14
            WriteToLine("C:\\Users\\mule\\source\\repos\\Loan System\\text\\selected_applicant_details.txt", 14, "accepted");

            await RunPythonScript(@"""C:\Users\mule\source\repos\Loan System\py\accepted_smtp.py""", "C:\\Users\\mule\\source\\repos\\Loan System\\text\\output_emails.txt");
            await RunPythonScript(@"""C:\Users\mule\source\repos\Loan System\py\update_status.py""", "C:\\Users\\mule\\source\\repos\\Loan System\\text\\output_update.txt");

            //loading end
            loadingIndicator.Visibility = Visibility.Collapsed;

            string email_output = File.ReadAllText("C:\\Users\\mule\\source\\repos\\Loan System\\text\\output_emails.txt");
            string update_output = File.ReadAllText("C:\\Users\\mule\\source\\repos\\Loan System\\text\\output_update.txt");
            MessageBox.Show(email_output);
            MessageBox.Show(update_output);
        }
        private async void waitapprovalBtn_Click(object sender, RoutedEventArgs e)
        {
            //loading start
            loadingIndicator.Visibility = Visibility.Visible;

            rejectBtn.Visibility = Visibility.Collapsed;
            approvedBtn.Visibility = Visibility.Collapsed;
            waitapprovalBtn.Visibility = Visibility.Collapsed;

            await RunPythonScript(@"""C:\Users\mule\source\repos\Loan System\py\pending_smtp.py""", "C:\\Users\\mule\\source\\repos\\Loan System\\text\\output_emails.txt");
            
            //loading end
            loadingIndicator.Visibility = Visibility.Collapsed;

            string email_output = File.ReadAllText("C:\\Users\\mule\\source\\repos\\Loan System\\text\\output_emails.txt");
            MessageBox.Show(email_output);
        }
        private async void rejectBtn_Click(object sender, RoutedEventArgs e)
        {
            //loading start
            loadingIndicator.Visibility = Visibility.Visible;

            rejectBtn.Visibility = Visibility.Collapsed;
            approvedBtn.Visibility = Visibility.Collapsed;
            waitapprovalBtn.Visibility = Visibility.Collapsed;

            //update txt file line 14
            WriteToLine("C:\\Users\\mule\\source\\repos\\Loan System\\text\\selected_applicant_details.txt", 14, "rejected");


            await RunPythonScript(@"""C:\Users\mule\source\repos\Loan System\py\rejecteded_smtp.py""", "C:\\Users\\mule\\source\\repos\\Loan System\\text\\output_emails.txt");
            await RunPythonScript(@"""C:\Users\mule\source\repos\Loan System\py\update_status.py""", "C:\\Users\\mule\\source\\repos\\Loan System\\text\\output_update.txt");

            //loading end
            loadingIndicator.Visibility = Visibility.Collapsed;

            string email_output = File.ReadAllText("C:\\Users\\mule\\source\\repos\\Loan System\\text\\output_emails.txt");
            string update_output = File.ReadAllText("C:\\Users\\mule\\source\\repos\\Loan System\\text\\output_update.txt");
            MessageBox.Show(email_output);
            MessageBox.Show(update_output);
        }

        //set the data
        public void SetData(Applicant selectedApplicant)
        {
            // Use the selected data to populate your window's controls

            namelabel.Text = selectedApplicant.Official_Name;
            idlabel.Text = selectedApplicant.Government_ID.ToString();
            timelabel.Text = selectedApplicant.Timestamp;
            updatelabel.Text = selectedApplicant.Time_of_Update;
            agelabel.Text = selectedApplicant.Age.ToString();
            sexlabel.Text = selectedApplicant.Sex;
            essaylabel.Text = selectedApplicant.ApplicantEssay;
            durationlabel.Text = selectedApplicant.LoanDuration;
            amountlabel.Text = selectedApplicant.LoanAmount.ToString();
            statuslabel.Text = selectedApplicant.Status;
            phonelabel.Text = selectedApplicant.PhoneNo.ToString();
            emaillabel.Text = selectedApplicant.Email;
            occupationlabel.Text = selectedApplicant.Occupation;
            idphotolabel.Text = selectedApplicant.Government_ID_Photo;
            paysliphotolabel.Text = selectedApplicant.PaySlipImage;
        }

        //write text file
        public void WriteToFile(Applicant selectedApplicant)
        {
            string filePath = "C:\\Users\\mule\\source\\repos\\Loan System\\text\\selected_applicant_details.txt";

            try
            {
                using (StreamWriter writer = new StreamWriter(filePath))
                {
                    // Write data line by line
                    writer.WriteLine(selectedApplicant.Timestamp);
                    writer.WriteLine(selectedApplicant.Official_Name);
                    writer.WriteLine(selectedApplicant.Age);
                    writer.WriteLine(selectedApplicant.Sex);
                    writer.WriteLine(selectedApplicant.Government_ID);
                    writer.WriteLine(selectedApplicant.Government_ID_Photo);
                    writer.WriteLine(selectedApplicant.Occupation);
                    writer.WriteLine(selectedApplicant.PaySlipImage);
                    writer.WriteLine(selectedApplicant.LoanDuration);
                    writer.WriteLine(selectedApplicant.LoanAmount);
                    writer.WriteLine(selectedApplicant.PhoneNo);
                    writer.WriteLine(selectedApplicant.Email);
                    writer.WriteLine(selectedApplicant.ApplicantEssay);
                    writer.WriteLine(selectedApplicant.Status);
                    writer.WriteLine(selectedApplicant.Time_of_Update);

                    // You can add more lines here as needed
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        //links to images
        private void payslip_click(object sender, RoutedEventArgs e)
        {
            dynamic url = paysliphotolabel.Text.Trim().Trim('"');

            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    }
                    );
                }
                catch (Exception ex)
                {
                    // Handle the exception, e.g., display an error message
                    MessageBox.Show($"Error opening URL: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Handle the case where the URI is not well-formed
                MessageBox.Show("Invalid URL format", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void idphoto_click(object sender, RoutedEventArgs e)
        {
            dynamic url = paysliphotolabel.Text.Trim().Trim('"');

            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                try
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = url,
                        UseShellExecute = true
                    }
                    );
                }
                catch (Exception ex)
                {
                    // Handle the exception, e.g., display an error message
                    MessageBox.Show($"Error opening URL: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                // Handle the case where the URI is not well-formed
                MessageBox.Show("Invalid URL format", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
