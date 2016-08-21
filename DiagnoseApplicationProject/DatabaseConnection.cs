using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication6
{
    public class DatabaseConnection
    {
        private int DATABASE_SIZE = 1;
        private string strCon;
        System.Data.SqlClient.SqlDataAdapter dataAdapter, dataAdapter1, dataAdapter2, dataAdapter3, dataAdapterX;
        private DataSet dataSet, dataSetX;
        private int[] maxTableRows;
        private System.Data.SqlClient.SqlConnection dataBase_connection;
        private GlobalDataSet globalDataSet;

        public DatabaseConnection(GlobalDataSet globalDataSet)
        {
            this.globalDataSet = globalDataSet;
        }

        public void UpdateDatabase(DataSet dataSet, int tableID, string connString)
        {
            try
            {
                if (globalDataSet.DebugMode) Debug.Write("connString: " + connString);
                // Create and open connection to specific database
                dataBase_connection = new SqlConnection(connString);
                dataBase_connection.Open();

                dataAdapter = new SqlDataAdapter("SELECT * FROM tbl_rl_j0", dataBase_connection);

                //dataAdapter1 = new SqlDataAdapter("SELECT * FROM tbl_rl_j1", dataBase_connection);

                //dataAdapter2 = new SqlDataAdapter("SELECT * FROM tbl_rl_j2", dataBase_connection);

                //dataAdapter3 = new SqlDataAdapter("SELECT * FROM tbl_rl_j3", dataBase_connection);

                dataBase_connection.Close();

                SqlCommandBuilder commandBuilder = new SqlCommandBuilder(dataAdapter);
                commandBuilder.DataAdapter.Update(dataSet.Tables[0]);

                //System.Data.SqlClient.SqlCommandBuilder commandBuilder1 = new System.Data.SqlClient.SqlCommandBuilder(dataAdapter1);
                //commandBuilder1.DataAdapter.Update(dataSet.Tables[1]);

                //System.Data.SqlClient.SqlCommandBuilder commandBuilder2 = new System.Data.SqlClient.SqlCommandBuilder(dataAdapter2);
                //commandBuilder2.DataAdapter.Update(dataSet.Tables[2]);

                //System.Data.SqlClient.SqlCommandBuilder commandBuilder3 = new System.Data.SqlClient.SqlCommandBuilder(dataAdapter3);
                //commandBuilder3.DataAdapter.Update(dataSet.Tables[3]);
            }
            catch (DBConcurrencyException e)
            {
                if (globalDataSet.DebugMode) Debug.Write("Exception in UpdateDatabase():" + e);
            }
        }

        public int[] getTableSizeForDb(DataSet dataSet)
        {
            maxTableRows = new int[DATABASE_SIZE];
            for (int i = 0; i < DATABASE_SIZE; i++)
            {
                maxTableRows[i] = dataSet.Tables[i].Rows.Count;
            }
            return maxTableRows;
        }

        public void deleteDatabaseContent(string dBdescription)
        {
            dataBase_connection = new System.Data.SqlClient.SqlConnection(dBdescription);
            int MAX_TABLE_AMOUNT = Properties.Settings.Default.MAX_TABLE_AMOUNT;
            if (dataBase_connection != null)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = dataBase_connection;

                for (int i = 0; i <= MAX_TABLE_AMOUNT; i++)
                {
                    cmd.CommandText = "DELETE tbl_rl_j" + i;
                    dataBase_connection.Open();
                    cmd.ExecuteNonQuery();
                    dataBase_connection.Close();
                }
            }
        }

        public void deleteDatabaseContent(string dBdescription, int tableId)
        {
            dataBase_connection = new System.Data.SqlClient.SqlConnection(dBdescription);
            int MAX_TABLE_AMOUNT = Properties.Settings.Default.MAX_TABLE_AMOUNT;
            if (dataBase_connection != null)
            {
                SqlCommand cmd = new SqlCommand();
                cmd.CommandType = System.Data.CommandType.Text;
                cmd.Connection = dataBase_connection;

                cmd.CommandText = "DELETE tbl_rl_j" + tableId;
                dataBase_connection.Open();
                cmd.ExecuteNonQuery();
                dataBase_connection.Close();
            }
        }

    }
}
