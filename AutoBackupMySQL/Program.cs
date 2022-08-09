//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using MySql.Data.MySqlClient;
//using System.Reflection;
//using System.Configuration;
//using System.Timers;
//using NCrontab;
//using System.ServiceProcess;

//namespace AutoBackupMySQL
//{
//    class Program
//    {
//        static void Main(string[] args)
//        {
//#if DEBUG
//            //While debugging this section is used.
//            Service1 myService = new Service1();
//            myService.onDebug();
//            System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);

//#else
//            ServiceBase[] ServicesToRun;
//            ServicesToRun = new ServiceBase[]
//            {
//                new Service1()
//            };
//            ServiceBase.Run(ServicesToRun);
//#endif
//        }

//    }
//}

using System;
using NCrontab.Advanced;
using NCrontab.Advanced.Enumerations;
using System.Reflection;
using MySql.Data.MySqlClient;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AutoBackupMySQL
{
    public class Program
    {
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

        public static void Main()
        {
            Console.WriteLine("Backup MySQL are running.....");
            WorkerProcess();
        }

        public static void WorkerProcess()
        {
            try
            {
                var listTaskes = new List<Task>();
                var listDatabase = ListDatabase.Trim().Split(';');
                var date = DateTime.Now.ToString("yyyyMMdd");
                for (int i = 0; i < listDatabase.Length; i++)
                {
                    var db = listDatabase[i];
                    //var thread = new Task(() =>
                    //{
                    //    string constring = ConnectionString + "database=" + db + ";";
                    //    string file = BackupFolderPath + date + "\\" + db + ".sql";

                    //    bool isBackupSuccess = BackupMySQL(constring, file);
                    //    if (isBackupSuccess)
                    //        WriteLog(MethodBase.GetCurrentMethod().Name, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ": " + db + ": Backup successfully!");
                    //    else
                    //        WriteLog(MethodBase.GetCurrentMethod().Name, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ": " + db + ": Backup failed!");
                    //});
                    //thread.Start();


                    var thread = Task.Factory.StartNew(() =>
                    {
                        string constring = ConnectionString + "database=" + db + ";";
                        string file = BackupFolderPath + date + "\\" + db + ".sql";

                        bool isBackupSuccess = BackupMySQL(constring, file);
                        if (isBackupSuccess)
                            WriteLog(MethodBase.GetCurrentMethod().Name, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ": " + db + ": Backup successfully!");
                        else
                            WriteLog(MethodBase.GetCurrentMethod().Name, DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ": " + db + ": Backup failed!");
                    });
             
                    listTaskes.Add(thread);
                }
                Task.WaitAll(listTaskes.ToArray());
                Console.WriteLine("Backup MySQL done.");
            }
            catch (Exception ex)
            {
                WriteLog(MethodBase.GetCurrentMethod().Name, ex.ToString());
            }
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
                string strErrorLogFile = LogPath + "log" + DateTime.Now.ToString("ddMMyyyy") + ".txt"; // File ghi log lỗi
                System.IO.File.AppendAllText(strErrorLogFile, string.Format("{0}{1} {2}: {3}", Environment.NewLine, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), functionName, strMessage));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}