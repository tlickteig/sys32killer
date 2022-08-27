using System;
using System.Collections.Generic;
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

namespace System32Killer
{
    /// <summary>
    /// Interaction logic for DeletionWindow.xaml
    /// </summary>
    public partial class DeletionWindow : Window
    {
        private const string DIRECTORY_NAME = "F:\\Programs\\UniServerZ\\www\\website";

        public DeletionWindow()
        {
            InitializeComponent();
        }

        public override void EndInit()
        {
            base.EndInit();

            string[] files = Directory.GetFiles(DIRECTORY_NAME);
            if (files?.Count() > 0)
            {
                prgProgressBar.Maximum = files.Count();
                prgProgressBar.Value = 0;


            }
        }
    }
}
