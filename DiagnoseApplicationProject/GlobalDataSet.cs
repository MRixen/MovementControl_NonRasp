using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication6
{
    public class GlobalDataSet
    {
        private int MAX_TABLE_AMOUNT = Properties.Settings.Default.MAX_TABLE_AMOUNT;
        private bool stopAllOperations = false;
        private long timerValue;
        private bool debugMode;
        private Stopwatch timer_programExecution = new Stopwatch();
        private bool showProgramDuration;
        private DataSet dataSet;
        private bool nextPosition = true;
        private bool firstWriteExecuted = false;
        private float factor = (1023f / 300f);
        private SerialPort serialPort;
        private int[] maxTableRows;


        public bool NextPositionRequest
        {
            get
            {
                return nextPosition;
            }

            set
            {
                nextPosition = value;
            }
        }

        public bool FirstWriteExecuted
        {
            get
            {
                return firstWriteExecuted;
            }

            set
            {
                firstWriteExecuted = value;
            }
        }

        public bool DebugMode
        {
            get
            {
                return debugMode;
            }

            set
            {
                debugMode = value;
            }
        }

        public Stopwatch Timer_programExecution
        {
            get
            {
                return timer_programExecution;
            }

            set
            {
                timer_programExecution = value;
            }
        }

        public long TimerValue
        {
            get
            {
                return timerValue;
            }

            set
            {
                timerValue = value;
            }
        }

        internal bool ShowProgramDuration
        {
            get
            {
                return showProgramDuration;
            }

            set
            {
                showProgramDuration = value;
            }
        }

        public DataSet DataSet
        {
            get
            {
                return dataSet;
            }

            set
            {
                dataSet = value;
            }
        }

        public bool StopAllOperations
        {
            get
            {
                return stopAllOperations;
            }

            set
            {
                stopAllOperations = value;
            }
        }

        public float Factor
        {
            get
            {
                return factor;
            }

            set
            {
                factor = value;
            }
        }

        public SerialPort SerialPort
        {
            get
            {
                return serialPort;
            }

            set
            {
                serialPort = value;
            }
        }

        public int MaxTableAmount
        {
            get
            {
                return MAX_TABLE_AMOUNT;
            }

            set
            {
                MAX_TABLE_AMOUNT = value;
            }
        }

        public int[] MaxTableRows
        {
            get
            {
                return maxTableRows;
            }

            set
            {
                maxTableRows = value;
            }
        }
    }
}
