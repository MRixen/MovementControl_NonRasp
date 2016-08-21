using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication6
{
    class HelperFunctions
    {
        String elementText = "";
        int elementTextInt = 0;
        private GlobalDataSet globalDataSet;

        public HelperFunctions(GlobalDataSet globalDataSet)
        {
            this.globalDataSet = globalDataSet;
        }

        public HelperFunctions()
        {
        }

        public void changeElementText(object element, String text, bool add)
        {
            if ((element.GetType() == typeof(TextBox)) && ((TextBox)element).InvokeRequired) ((TextBox)element).BeginInvoke((MethodInvoker)delegate() { ((TextBox)element).Text = text; ((TextBox)element).Refresh(); });
            else if ((element.GetType() == typeof(TextBox)))
            {
                if (add) ((TextBox)element).Text = ((TextBox)element).Text + "\n" + text;
                else ((TextBox)element).Text = text;
                ((TextBox)element).Refresh();
            }

            if ((element.GetType() == typeof(Label)) && ((Label)element).InvokeRequired) ((Label)element).BeginInvoke((MethodInvoker)delegate() { ((Label)element).Text = text; ((Label)element).Refresh(); });
            else if ((element.GetType() == typeof(Label)))
            {
                if (add) ((Label)element).Text = ((Label)element).Text + "\n" + text;
                else ((Label)element).Text = text;
                ((Label)element).Refresh();
            }

            if ((element.GetType() == typeof(Button)) && ((Button)element).InvokeRequired) ((Button)element).BeginInvoke((MethodInvoker)delegate() { ((Button)element).Text = text; ((Button)element).Refresh(); });
            else if ((element.GetType() == typeof(Button)))
            {
                if (add) ((Button)element).Text = ((Button)element).Text + "\n" + text;
                else ((Button)element).Text = text;
                ((Button)element).Refresh();
            }
        }

        public void changeElementEnable(object element, bool enabled)
        {
            if ((element.GetType() == typeof(TextBox)) && ((TextBox)element).InvokeRequired) ((TextBox)element).BeginInvoke((MethodInvoker)delegate() { ((TextBox)element).Enabled = enabled; ((TextBox)element).Refresh(); });
            else if ((element.GetType() == typeof(TextBox)))
            {
                ((TextBox)element).Enabled = enabled;
                ((TextBox)element).Refresh();
            }

            if ((element.GetType() == typeof(Label)) && ((Label)element).InvokeRequired) ((Label)element).BeginInvoke((MethodInvoker)delegate() { ((Label)element).Enabled = enabled; ((Label)element).Refresh(); });
            else if ((element.GetType() == typeof(Label)))
            {
                ((Label)element).Enabled = enabled;
                ((Label)element).Refresh();
            }

            if ((element.GetType() == typeof(Button)) && ((Button)element).InvokeRequired) ((Button)element).BeginInvoke((MethodInvoker)delegate() { ((Button)element).Enabled = enabled; ((Button)element).Refresh(); });
            else if ((element.GetType() == typeof(Button)))
            {
                ((Button)element).Enabled = enabled;
                ((Button)element).Refresh();
            }
        }


        public int getElementText(object element)
        {
            if ((element.GetType() == typeof(NumericUpDown)) && ((NumericUpDown)element).InvokeRequired) ((NumericUpDown)element).BeginInvoke((MethodInvoker)delegate() { this.elementTextInt = Convert.ToInt32(((NumericUpDown)element).Value); });
            else if ((element.GetType() == typeof(NumericUpDown))) this.elementText = ((NumericUpDown)element).Text;

            return this.elementTextInt;
        }

        public void clearElement(object element)
        {
            if ((element.GetType() == typeof(TextBox)) && ((TextBox)element).InvokeRequired) ((TextBox)element).BeginInvoke((MethodInvoker)delegate() { ((TextBox)element).Clear(); });
            else if ((element.GetType() == typeof(TextBox))) ((TextBox)element).Text = "";

            if ((element.GetType() == typeof(Label)) && ((Label)element).InvokeRequired) ((Label)element).BeginInvoke((MethodInvoker)delegate() { ((Label)element).Text = ""; });
            else if ((element.GetType() == typeof(Label))) ((Label)element).Text = "";

            if ((element.GetType() == typeof(Button)) && ((Button)element).InvokeRequired) ((Button)element).BeginInvoke((MethodInvoker)delegate() { ((Button)element).Text = ""; });
            else if ((element.GetType() == typeof(Button))) ((Button)element).Text = "";

            if ((element.GetType() == typeof(ListView)) && ((ListView)element).InvokeRequired) ((ListView)element).BeginInvoke((MethodInvoker)delegate() { ((ListView)element).Items.Clear(); });
            else if ((element.GetType() == typeof(ListView))) ((ListView)element).Items.Clear();
        
        }
       

    }
}
