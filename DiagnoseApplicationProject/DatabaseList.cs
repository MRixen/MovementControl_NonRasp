using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication6
{
    public partial class DatabaseList : Form
    {
        private FormMain formBaseContext;
        private DataSet dataSet;
        private DatabaseConnection databaseConnection;
        private String[] element = new String[4];
        private int tableID;
        private System.ComponentModel.BackgroundWorker backgroundWorker_readDataset;
        private HelperFunctions helperFunctions;
        private int databaseId;

        public DatabaseList(FormMain context, DataSet dataSet, DatabaseConnection databaseConnection, int databaseId)
        {
            InitializeComponent();

            this.formBaseContext = context;
            this.databaseConnection = databaseConnection;
            this.dataSet = dataSet;
            this.databaseId = databaseId;
            helperFunctions = new HelperFunctions();

            backgroundWorker_readDataset.DoWork += new DoWorkEventHandler(backgroundWorker_readDataset_DoWork);
            backgroundWorker_readDataset.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_readDataset_RunWorkerCompleted);

            this.tableID = Int32.Parse(numericUpDown_tableSelector.Text);
            helperFunctions.changeElementText(labelListEntries, "List entries: " + dataSet.Tables[tableID].Rows.Count.ToString(), false);
            helperFunctions.changeElementText(labelDatabaseId, "Database ID: " + databaseId, false);
        }

        private void backgroundWorker_readDataset_DoWork(object sender, DoWorkEventArgs e)
        {
            readDataset();
        }

        private void backgroundWorker_readDataset_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            helperFunctions.changeElementText(labelListEntries, "List entries: " + dataSet.Tables[tableID].Rows.Count.ToString(), false);
        }

        private void FormDatabaseList_Closing(object sender, FormClosingEventArgs e)
        {
            formBaseContext.setCheckboxUnchecked_DbList = CheckState.Unchecked;
        }

        private void readDataset()
        {
            if (dataSet != null)
            {
                int[] maxTableRows = databaseConnection.getTableSizeForDb(dataSet);

                for (int j = 0; j < maxTableRows[tableID]; j++)
                    {
                        DataRow dataRow = dataSet.Tables[tableID].Rows[j];
                        ListViewItem listViewItemsTemp = new ListViewItem();                       

                        element[0] = dataRow.ItemArray.GetValue(1).ToString();
                        listViewItemsTemp = new ListViewItem(element[0]);

                        for (int k = 1; k < 4; k++)
                        {
                            element[k] = dataRow.ItemArray.GetValue(k).ToString();
                            listViewItemsTemp.SubItems.Add(element[k]);
                        }

                        listViewDatabaseContent.BeginInvoke((MethodInvoker)delegate() { listViewDatabaseContent.Items.AddRange(new ListViewItem[] { listViewItemsTemp }); });
                    }
            }
        }

        private void FormDatabase_Load(object sender, EventArgs e)
        {        
            backgroundWorker_readDataset.RunWorkerAsync();
        }

        private void numericUpDown_valueChanged(object sender, EventArgs e)
        {
            this.tableID = Convert.ToInt32(((NumericUpDown)sender).Value);
            helperFunctions.clearElement(listViewDatabaseContent);
            backgroundWorker_readDataset.RunWorkerAsync();
        }

    }
}
