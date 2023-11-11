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
using System.Xml;

namespace Loan_System
{
    /// <summary>
    /// Interaction logic for ApplicationReview.xaml
    /// </summary>
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
            this.WindowState = WindowState.Maximized;
        }
        private void closeBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void windowBorder_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }
        private void approvedBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void waitapprovalBtn_Click(object sender, RoutedEventArgs e)
        {

        }
        private void rejectBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        //set the data
        public void SetData(Applicant selectedApplicant)
        {
            // Use the selected data to populate your window's controls

            namelabel.Text = selectedApplicant.Official_Name;
            idlabel.Text = selectedApplicant.Government_ID.ToString();
            timelabel.Text = selectedApplicant.Timestamp;
        }
    }
}
