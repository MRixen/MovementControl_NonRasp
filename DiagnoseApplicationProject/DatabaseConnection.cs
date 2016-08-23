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

        public int[] getTableSizeForDb(DataSet dataSet)
        {
            maxTableRows = new int[globalDataSet.MaxTableAmount];
            for (int i = 0; i < globalDataSet.MaxTableAmount; i++)
            {
                maxTableRows[i] = dataSet.Tables[i].Rows.Count;
            }
            return maxTableRows;
        }

        public void UpdateLocalDatabase(DataSet dataSet, string connString)
        {
            try
            {
                if (globalDataSet.DebugMode) Debug.Write("connString: " + connString);
                dataBase_connection = new SqlConnection(connString);
                dataBase_connection.Open();
                for (int i = 0; i < globalDataSet.MaxTableAmount; i++)
                {
                    SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM tbl_rl_j"+i, dataBase_connection);
                    SqlCommandBuilder cmdBuilder = new SqlCommandBuilder(adapter);
                    cmdBuilder.DataAdapter.Update(dataSet.Tables[i]);
                }
                dataBase_connection.Close();
            }
            catch (DBConcurrencyException e)
            {
                if (globalDataSet.DebugMode) Debug.Write("Exception in UpdateDatabase(): " + e);
            }
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
