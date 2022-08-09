using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace AutoBackupMySQL
{
    partial class Service1 : ServiceBase
    {

        private Timer timer = new Timer();
        private static string LogPath = ConfigurationManager.AppSettings["LogPath"];
        private static string BackupFolderPath = ConfigurationManager.AppSettings["BackupFolderPath"];
        private static string ListDatabase = ConfigurationManager.AppSettings["ListDatabase"];
        private static string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
        private static bool IsDaily = bool.Parse(ConfigurationManager.AppSettings["IsDaily"]);
        private static bool IsWeekly = bool.Parse(ConfigurationManager.AppSettings["IsWeekly"]);
        private static int NumberDay = int.Parse(ConfigurationManager.AppSettings["NumberDay"]);
        private static DateTime StartDate = DateTime.Parse(ConfigurationManager.AppSettings["StartDate"]);
        private static int Hour = int.Parse(ConfigurationManager.AppSettings["Hour"]);
        private static int Minute = int.Parse(ConfigurationManager.AppSettings["Minute"]);
        private static int Second = int.Parse(ConfigurationManager.AppSettings["Second"]);

        public Service1()
        {
            InitializeComponent();
        }

        public void OnDebug()
        {
            OnStart(null);
        }

        protected override void OnStart(string[] args)
        {
            timer.Elapsed += new ElapsedEventHandler(WorkerProcess);
            timer.Start();
            // TODO: Add code here to start your service.
        }

        private void WorkerProcess(object sender, EventArgs e)
        {
            try
            {
                timer.Interval = 1000;  // Khoảng thời gian giữa các tiến trình
                timer.Stop(); // Stop timer => thực hiện công việc => Start trở lại

                if (IsDaily)
                {
                    var now = DateTime.Now;
                    if (now.Hour == Hour && now.Minute == Minute && now.Second == Second)
                    {
                        var listDatabase = ListDatabase.Trim().Split(';');
                        var date = DateTime.Now.ToString("yyyyMMdd");
                        for (int i = 0; i < listDatabase.Length; i++)
                        {
                            var db = listDatabase[i];
                            string constring = ConnectionString + "database=" + db + ";";
                            string file = BackupFolderPath + date + "\\" + db + ".sql";

                            bool isBackupSuccess = BackupMySQL(constring, file);
                            if (isBackupSuccess)
                                WriteLog(MethodBase.GetCurrentMethod().Name, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ": " + db + ": Backup successfully!");
                            else
                                WriteLog(MethodBase.GetCurrentMethod().Name, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ": " + db + ": Backup failed!");
                        }
                    }
                }

                if (IsWeekly)
                {
                    var now = DateTime.Now;
                    if (now.Day == StartDate.Day && now.Hour == Hour && now.Minute == Minute && now.Second == Second)
                    {
                        var listDatabase = ListDatabase.Trim().Split(';');
                        var date = DateTime.Now.ToString("yyyyMMdd");
                        for (int i = 0; i < listDatabase.Length; i++)
                        {
                            var db = listDatabase[i];
                            string constring = ConnectionString + "database=" + db + ";";
                            string file = BackupFolderPath + date + "\\" + db + ".sql";

                            bool isBackupSuccess = BackupMySQL(constring, file);
                            if (isBackupSuccess)
                                WriteLog(MethodBase.GetCurrentMethod().Name, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ": " + db + ": Backup successfully!");
                            else
                                WriteLog(MethodBase.GetCurrentMethod().Name, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ": " + db + ": Backup failed!");
                        }

                        StartDate = StartDate.AddDays(NumberDay);
                    }
                }

                timer.Start();
            }
            catch (Exception ex)
            {
                WriteLog(MethodBase.GetCurrentMethod().Name, ex.ToString());
            }
        }

        protected override void OnStop()
        {
            // TODO: Add code here to perform any tear-down necessary to stop your service.
            WriteLog("OnStop", "OnStop");
        }

        public static bool BackupMySQL(string connection, string filePath)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connection))
                {
                    using (MySqlCommand cmd = new MySqlCommand())
                    {
                        using (MySqlBackup mb = new MySqlBackup(cmd))
                        {
                            cmd.Connection = conn;
                            conn.Open();
                            mb.ExportToFile(filePath);
                            conn.Close();
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                WriteLog(MethodBase.GetCurrentMethod().Name, ex.ToString());
                return false;
            }
        }

        public static void WriteLog(string functionName, string strMessage)
        {
            try
            {
                string strErrorLogFile = LogPath + "log-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt"; // File ghi log lỗi
                File.AppendAllText(strErrorLogFile, string.Format("{0}{1} {2}: {3}", Environment.NewLine, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), functionName, strMessage));
            }
            catch
            {

            }
        }
    }
}
