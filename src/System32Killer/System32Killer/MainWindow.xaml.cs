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

namespace System32Killer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public void Event_StartButtonClicked(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to continue?", "Confirm System32 deletion",
                MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                DeletionWindow deletionWindow = new DeletionWindow();
                deletionWindow.ShowDialog();

                switch (deletionWindow.StopReason)
                {
                    case StopReason.Success:
                        MessageBox.Show("System32 deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case StopReason.Error:
                        MessageBox.Show("An error occurred when deleting System32. Please check the Event Viewer for details", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        break;
                }
            }
        }

        public void Event_CancelButtonClicked(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
