/*
   This example code is in the Public Domain

   This software is distributed on an "AS IS" BASIS, 
   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
   either express or implied.

   Este código de exemplo é de uso publico,

   Este software é distribuido na condição "COMO ESTÁ",
   e NÃO SÃO APLICÁVEIS QUAISQUER GARANTIAS, implicitas 
   ou explicitas
*/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TEX;
using System.IO;
using System.IO.Ports;

namespace FlowTEX
{
    public partial class frmFlowTex : Form
    {
        cFlowTEX FlowTEX;

        const string defaultFlowFormat = "0.000";
        const string defaultTemperatureFormat = "0.000";
        const string defaultFlowUnit = "Sccm";
        const string defaultTemperatureUnit = "°C";

        public frmFlowTex()
        {
            InitializeComponent();
            FlowTEX = new cFlowTEX();
            lblFlow.Text = FlowTEX.getFlow().ToString(defaultFlowFormat) + defaultFlowUnit;
            lblTemperature.Text = FlowTEX.getTemperature().ToString(defaultTemperatureFormat) + defaultTemperatureUnit;

            comboSerialFlowTex.DropDown += ComboSerialFlowTex_DropDown;
            comboSerialFlowTex.Items.Add("AUTO");
            comboSerialFlowTex.SelectedIndex = 0;
        }

        private void ComboSerialFlowTex_DropDown(object sender, EventArgs e)
        {
            ComboBox combo = sender as ComboBox;
            List<string> ComList = new List<string>();

            string value = "";
            if(combo.SelectedIndex >= 0)
            {
                value = combo.SelectedItem.ToString();
            }

            ComList.Clear();
            ComList.AddRange(SerialPort.GetPortNames());
            ComList.Sort();

            combo.Items.Clear();
            combo.Items.Add("AUTO");
            combo.Items.AddRange(ComList.ToArray());
            
            if(value != "")
            {
                foreach(object item in combo.Items)
                {
                    if(item.ToString() == value)
                    {
                        combo.SelectedItem = item;
                        break;
                    }
                }
            }
        }

        private void tmrRefresh_Tick(object sender, EventArgs e)
        {
            if(FlowTEX.isConnected())
            {
                lblFlow.Text = FlowTEX.getFlow().ToString(defaultFlowFormat) + defaultFlowUnit;
                lblTemperature.Text = FlowTEX.getTemperature().ToString(defaultTemperatureFormat) + defaultTemperatureUnit;
            }

            if(FlowTEX.hasError())
                icoFlowTEX.Image = imgStatus.Images[1];
            else
                icoFlowTEX.Image = imgStatus.Images[0];
        }

        private void btnAbrirFlowTEX_Click(object sender, EventArgs e)
        {
            if(!FlowTEX.isConnected())
            {
                if(!FlowTEX.isActive())
                {
                    if((comboSerialFlowTex.SelectedItem != null) && (comboSerialFlowTex.SelectedIndex > 0))
                    {
                        FlowTEX.setSerialPort(comboSerialFlowTex.SelectedItem.ToString());
                    }
                    else
                    {
                        FlowTEX.setSerialPort(null);
                    }

                    FlowTEX.init();
                    tmrRefresh.Enabled = true;
                }
                else
                {
                    if((comboSerialFlowTex.SelectedItem != null) && (comboSerialFlowTex.SelectedIndex > 0))
                    {
                        FlowTEX.setSerialPort(comboSerialFlowTex.SelectedItem.ToString());
                    }
                    else
                    {
                        FlowTEX.setSerialPort(null);
                    }

                    FlowTEX.connect();
                }
            }
            else
            {
                FlowTEX.disconnect();
            }

            if(!FlowTEX.isConnected())
            {
                btnAbrirFlowTEX.Text = "Abrir";
            }
            else
            {
                btnAbrirFlowTEX.Text = "Fechar";
            }
        }
    }
}
