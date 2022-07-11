using Microsoft.SqlServer.Management.IntegrationServices;
using Microsoft.SqlServer.Management.Smo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TourneyKeeperCommon;
using TourneyKeeperCommon.SSIS;

namespace DataImporterGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool running = false;
        private static bool isRunning = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            running = !running;
            if (running)
            {
                importButton.Content = "Stop";
                RunImport();
            }
            else
            {
                importButton.Content = "Start";
            }
        }

        private async Task RunImport()
        {
            try
            {
                if (!isRunning)
                {
                    isRunning = true;
                    Backup();

                    var res = Task.Factory.StartNew<Tuple<Operation.ServerOperationStatus, string>>(() => SSISLib.RunSSIS(ConfigurationManager.ConnectionStrings["TargetConnectionString"].ConnectionString));
                    Output.Text = "Running import";
                    await res;
                    Output.Text = res.Result.Item1 == Operation.ServerOperationStatus.Success ? "Success!" : res.Result.Item2;
                    lastRun.Content = DateTime.Now;

                    var wait = Task.Factory.StartNew<bool>(() => Wait());
                    await wait;
                    if (running)
                    {
                        isRunning = false;
                        RunImport();
                    }
                }
            }
            catch (Exception e)
            {
                importButton.Content = "Start";
                Output.Text = $"Crash! {e.Message}";
            }
            finally
            {
                isRunning = false;
            }
        }

        private static bool Wait()
        {
            int waitTime = int.Parse(ConfigurationManager.AppSettings["waitTimeInMinutes"]) * 60 * 1000;
            Thread.Sleep(waitTime);
            return true;
        }

        private void Backup()
        {
            Server server = new Server(ConfigurationManager.AppSettings["serverName"]);
            Database database = server.Databases[ConfigurationManager.AppSettings["databaseName"]];
            Backup backup = new Backup();
            backup.Action = BackupActionType.Database;
            backup.Database = database.Name;
            backup.Devices.AddDevice(System.IO.Path.Combine(ConfigurationManager.AppSettings["backupLocation"], ConfigurationManager.AppSettings["backupName"]), DeviceType.File);
            backup.BackupSetName = ConfigurationManager.AppSettings["backupName"];
            backup.Incremental = false;
            backup.SqlBackup(server);
        }
    }
}
