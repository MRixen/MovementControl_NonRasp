using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication6
{
    class RobotControl
    {
        private GlobalDataSet globalDataSet;
        byte[] position, ids, msgStart, msgEnd, speed;
        byte[] byteArray = new byte[64];
        int maxRotAngle = 300;
        int maxIncrements = 1024;
        byte positionReached = 2;
        int SMOOTH_ZONE = 3; // Zone to set position reached bit
        int counter = 0;

        public RobotControl(GlobalDataSet globalDataSet)
        {
            this.globalDataSet = globalDataSet;
        }

        public void moveForward(int stepsize, int velocity, int steps)
        {
            // Set start of message
            msgStart = BitConverter.GetBytes((short)9999);
            for (int i = 0; i < msgStart.Length; i++) byteArray[i] = msgStart[i];

            // Set ids
            // Binär (i.e. 0000 0011 for motor 1 and 2)
            ids = BitConverter.GetBytes((short)3);
            for (int i = 0; i < ids.Length; i++) byteArray[i + 2] = ids[i];

            // Set speed for dynamixel 1
            speed = BitConverter.GetBytes((short)velocity);
            for (int i = 0; i < speed.Length; i++) byteArray[i + 4] = speed[i];

            // Set speed for dynamixel 2
            speed = BitConverter.GetBytes((short)velocity);
            for (int i = 0; i < speed.Length; i++) byteArray[i + 6] = speed[i];

            for (int k = 0; k < steps; k++)
            {
                counter = 0;
                // TODO: Add length variable for the count of rows per table (not hardcoded 23!!!)
                while (counter < 23)
                {
                    // Set position from database for dxl 1
                    int dataT = (int)globalDataSet.DataSet.Tables[0].Rows[counter].ItemArray.GetValue(2);

                    position = BitConverter.GetBytes((short)Math.Round((dataT / 100) * globalDataSet.Factor, 0));
                    //position = BitConverter.GetBytes((short)dataArray[counter]);
                    for (int i = 0; i < position.Length; i++) byteArray[i + 8] = position[i];

                    // Set position from database for dxl 2
                    DataRow dataRow2 = globalDataSet.DataSet.Tables[0].Rows[counter];

                    position = BitConverter.GetBytes((short)Math.Round((dataT / 100) * globalDataSet.Factor, 0));
                    //position = BitConverter.GetBytes((short)dataArray[counter]);
                    for (int i = 0; i < position.Length; i++) byteArray[i + 10] = position[i];

                    counter++;
                    sendToPort(byteArray);
                }
            }
        }

        private void sendToPort(byte[] byteArray)
        {
            int retVal = 0;
            globalDataSet.SerialPort.Write(byteArray, 0, 64);
            do
            {
                retVal = globalDataSet.SerialPort.ReadByte();
            }
            while (retVal != 1);
        }

    }
}

