using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using MySql.Data.MySqlClient;
using System.Threading;
using System.IO.Ports;
using System.Management;
using System.Text.RegularExpressions;
using Microsoft.Win32;

namespace WindowsFormsApplication6
{
    public partial class FormMain : Form
    {
        public delegate void UpdateSavedRowCounter(string text);

        DatabaseConnection databaseConnection;
        string conString;
        DataSet dataSet_Db1, dataSet_Db2;
        DataRow dRow;
        private int[] maxTableRows_Db1;
        int inc = 0;
        private bool recordIsActive = false;
        private bool calibrationIsActive = false;

        private System.IO.StreamWriter writer = new System.IO.StreamWriter("DiagnoseDebugLog.log", true);
        private RBC.TcpIpCommunicationUnit tcpDiagnoseClient = null;
        private String dllConfigurationFileName = "";
        private int writeCycle;
        private int MAX_WRITE_CYCLE;
        private int sampleStep;
        private CheckBox xSenderCheckbox;
        private CheckBox ySenderCheckbox;
        private CheckBox zSenderCheckbox;
        private DatabaseList dataBaseList;
        private CheckBox senderCheckbox;
        private int savedRowCounter;
        private int tablePresentID = -1;
        private int MAX_TABLE_AMOUNT = Properties.Settings.Default.MAX_TABLE_AMOUNT;
        private int MIN_TABLE_AMOUNT = Properties.Settings.Default.MIN_TABLE_AMOUNT;
        private float SAMPLE_TIME = Properties.Settings.Default.SAMPLE_TIME;
        private int DEFAULT_SAMPLE_TIME_FACTOR = Properties.Settings.Default.DEFAULT_SAMPLE_TIME_FACTOR;
        private int MAX_ALIVE_SIGNAL_PAUSE = Properties.Settings.Default.MAX_ALIVE_SIGNAL_PAUSE;
        private string FILE_SAVE_PATH = Properties.Settings.Default.FILE_SAVE_PATH;
        private int SENSOR_AMOUNT = Properties.Settings.Default.sensorAmount;
        double[] Rx_x = new double[3];
        double[] Rx_y = new double[3];
        double[] Rx_z = new double[3];

        private float recordDuration;
        //private RBC.Configuration dllConfiguration = null;

        private BackgroundWorker backgroundWorker_CreateLocalDb, backgroundWorker_DeleteDb, backgroundWorker_InitComPort;
        private HelperFunctions helperFunctions;
        private Stopwatch aliveStopWatch = new Stopwatch();

        GlobalDataSet globalDataSet;

        long timeStamp_startTime;
        Stopwatch timer_timeStamp = new Stopwatch();
        private bool notInUseByGraph, notInUseByDatabase;

        private string db_name;
        private Thread movementControlThread;
        private bool startTransfer = false;
        private RobotControl robotControl;

        #region FORM
        public FormMain()
        {
            InitializeComponent();
            globalDataSet = new GlobalDataSet();
            helperFunctions = new HelperFunctions(globalDataSet);
            databaseConnection = new DatabaseConnection(globalDataSet);
            robotControl = new RobotControl(globalDataSet);

            globalDataSet.DebugMode = false;
            globalDataSet.ShowProgramDuration = false;

            backgroundWorker_DeleteDb.DoWork += new DoWorkEventHandler(backgroundWorker_DeleteDb_DoWork);
            backgroundWorker_DeleteDb.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_DeleteDb_RunWorkerCompleted);

            backgroundWorker_InitComPort.DoWork += new DoWorkEventHandler(backgroundWorker_InitComPort_DoWork);
            backgroundWorker_InitComPort.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_InitComPort_RunWorkerCompleted);

            backgroundWorker_CreateLocalDb.DoWork += new DoWorkEventHandler(backgroundWorker_CreateLocalDb_DoWork);
            backgroundWorker_CreateLocalDb.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_CreateLocalDb_RunWorkerCompleted);

            movementControlThread = new Thread(movementControl);

            // Initialize
            notInUseByGraph = true;
            notInUseByDatabase = true;
            sampleStep = DEFAULT_SAMPLE_TIME_FACTOR;
            recordIsActive = false;
            writeCycle = 0;
            savedRowCounter = 0;
        }

        private void movementControl()
        {
            int stepsize = 100; // In percentage of max stepsize
            int velocity = 400; // In percentage of max velocity
            int steps = 1;

            while (!globalDataSet.StopAllOperations)
            {
                // Send control data to openCM via usb
                if (startTransfer)
                {
                    // Condition one is set - move forward
                    if (true)
                    {
                        //Debug.WriteLine("Execute moveforward...");
                        robotControl.moveForward(stepsize, velocity, steps);
                    }

                    // Condition two is set - move backward
                    if (true) ;

                }
                Task.Delay(-1).Wait(2000);
            }
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                backgroundWorker_CreateLocalDb.RunWorkerAsync();
            }
            catch (Exception err)
            {
                MessageBox.Show(err.Message);
            }
        }

        private void FormMain_Closed(object sender, FormClosedEventArgs e)
        {
            closeApplication();
        }

        private void FormMain_Closing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.ApplicationExitCall)
            {
                tcpDiagnoseClient.closeAllConnections();
                closeApplication();
            }
            else
            {
                // DO nothing
            }
        }

        private void checkBox_showDatabase_CheckedChanged(object sender, EventArgs e)
        {
            senderCheckbox = (CheckBox)sender;

            if (((CheckBox)sender).Checked)
            {
                dataBaseList = new DatabaseList(this, globalDataSet.DataSet, databaseConnection, 1);
                dataBaseList.Show();
            }
            else
            {
                try
                {
                    dataBaseList.Close();
                }
                catch (Exception)
                {

                }
            }
        }

        public CheckState setCheckboxUnchecked_Y
        {
            set { ySenderCheckbox.CheckState = CheckState.Unchecked; }
        }

        public CheckState setCheckboxUnchecked_X
        {
            set { xSenderCheckbox.CheckState = CheckState.Unchecked; }
        }

        public CheckState setCheckboxUnchecked_Z
        {
            set { zSenderCheckbox.CheckState = CheckState.Unchecked; }
        }

        private void startControlAlgorithm_Click(object sender, EventArgs e)
        {
            movementControlThread.Start();

            button_startControlAlgorithm.Enabled = false;
        }

        public CheckState setCheckboxUnchecked_DbList
        {
            set { senderCheckbox.CheckState = CheckState.Unchecked; }
        }
        #endregion

        #region BACKGROUND WORKER
        private void backgroundWorker_DeleteDb_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            BackgroundWorker worker = sender as BackgroundWorker;

            // Delete the content of all tables
            databaseConnection.deleteDatabaseContent(Properties.Settings.Default.ConnectionString_DataBase);

            e.Result = databaseConnection.getTableSizeForDb(dataSet_Db1);
        }

        private void backgroundWorker_DeleteDb_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //helperFunctions.changeElementText(labelSavedRows, "");
            // Show duration of measurement
            Debug.WriteLine("MAX_WRITE_CYCLE: " + MAX_WRITE_CYCLE);
            recordIsActive = true;
            helperFunctions.changeElementText(button_startControlAlgorithm, "Stop recording", false);
            helperFunctions.changeElementEnable(checkBox_showDatabase, false);
            maxTableRows_Db1 = (int[])e.Result;
        }

        private void backgroundWorker_InitComPort_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            BackgroundWorker worker = sender as BackgroundWorker;

            // Find openCM device
            string[] portnames;
            int counter = 0;
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
            {
                portnames = SerialPort.GetPortNames();
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
                var tList = (from n in portnames
                             join p in ports on n equals p["DeviceID"].ToString()
                             select n + " - " + p["Caption"]).ToList();

                foreach (string s in tList)
                {
                    if (s.Contains("ROBOTIS Virtual COM Port"))
                    {
                        Debug.WriteLine("Device found");
                        startTransfer = true;
                        break;
                    }
                    else counter++;
                }
                if (!startTransfer) Debug.WriteLine("No device found");
            }

            SerialPort serialPort = new SerialPort();

            try
            {
                serialPort.BaudRate = 115200;
                serialPort.DataBits = 8;
                serialPort.Handshake = Handshake.None;
                serialPort.Parity = Parity.None;
                serialPort.StopBits = StopBits.One;
                serialPort.PortName = portnames[counter];
                serialPort.Open();
                globalDataSet.SerialPort = serialPort;
            }
            catch {
                Debug.WriteLine("Exception in init com port");
            }
        }

        private void backgroundWorker_InitComPort_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

        }
       
        private void backgroundWorker_CreateLocalDb_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            BackgroundWorker worker = sender as BackgroundWorker;


            e.Result = xampp_getContent("moveforward", 0);
        }

        private void backgroundWorker_CreateLocalDb_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            DataSet dataSet = (DataSet)e.Result;
            globalDataSet.DataSet = dataSet;
            writeToDb((DataSet)e.Result, "moveforward");
            enableVisualComponents();

            backgroundWorker_InitComPort.RunWorkerAsync();
        }

        private void enableVisualComponents()
        {
            checkBox_showDatabase.Enabled = true;
            button_startControlAlgorithm.Enabled = true;
        }

        #endregion

        #region HELP FUNCTIONS
        private void getPortInfo()
        {
            string[] portNames = SerialPort.GetPortNames();
            string sInstanceName = string.Empty;
            string sPortName = string.Empty;
            bool bFound = false;

            for (int y = 0; y < portNames.Length; y++)
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("root\\WMI", "SELECT * FROM MSSerial_PortName");
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    sInstanceName = queryObj["InstanceName"].ToString();

                    if (sInstanceName.IndexOf("Vid_xxxx&Pid_xxxx") > -1)
                    {
                        sPortName = queryObj["PortName"].ToString();
                        break;
                    }
                }
            }
        }

        private void xampp_removeContent(string dbName, int tableId)
        {
            string conStringXampp = "Server=localhost;Database=" + dbName + "; Uid=root;Pwd=rbc;";
            MySqlConnection connection = new MySqlConnection(conStringXampp);
            MySqlCommand cmd;
            connection.Open();

            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "DELETE FROM s"+tableId+" WHERE 1";
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        private DataSet xampp_getContent(string dbName, int tableId)
        {
            DataSet dataSet = new DataSet();
            string conStringXampp = "Server=localhost;Database=" + dbName + "; Uid=root;Pwd=rbc;";
            MySqlConnection connection = new MySqlConnection(conStringXampp);
            MySqlCommand cmd;
            connection.Open();

            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "SELECT * FROM s"+tableId;
                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                adapter.Fill(dataSet);
            }
            catch (Exception)
            {

                throw;
            }

            return dataSet;
        }

        private void xampp_addContent(string dbName, int tableId, Decimal[] data)
        {
            string conStringXampp = "Server=localhost;Database="+ dbName+"; Uid=root;Pwd=rbc;";
            MySqlConnection connection = new MySqlConnection(conStringXampp);
            MySqlCommand cmd;
            connection.Open();

            try
            {
                cmd = connection.CreateCommand();
                cmd.CommandText = "INSERT INTO s"+ tableId + "(x,y,z,timestamp)VALUES(@x,@y,@z,@timestamp)";
                cmd.Parameters.AddWithValue("@x", data[0]);
                cmd.Parameters.AddWithValue("@y", data[1]);
                cmd.Parameters.AddWithValue("@z", data[2]);
                cmd.Parameters.AddWithValue("@timestamp", data[3]);
                cmd.ExecuteNonQuery();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

        public void startApplication()
        {

        }

        private void closeApplication()
        {
            // Stop timer to measure program execution
            if (globalDataSet.ShowProgramDuration) globalDataSet.Timer_programExecution.Stop();

            globalDataSet.StopAllOperations = true;
            if(globalDataSet.SerialPort.IsOpen) globalDataSet.SerialPort.Close();

            if (System.Windows.Forms.Application.MessageLoop)
            {
                // WinForms app
                System.Windows.Forms.Application.Exit();
            }
            else
            {
                // Console app
                System.Environment.Exit(1);
            }
        }

        private void button_clearInfo_Clicked(object sender, EventArgs e)
        {
            textBox_Info.Clear();
        }

        private void writeToDb(DataSet dataSet, string dbName)
        {
            if (dataSet != null)
            {
                try
                {
                    databaseConnection.UpdateDatabase(dataSet, 0, Properties.Settings.Default.ConnectionString_DataBase);
                    //maxTableRows_Db1[0]++;
                }
                catch (Exception err)
                {
                    MessageBox.Show(err.Message);
                }
            }

        }
        #endregion
    }
}
