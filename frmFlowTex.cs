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

        bool wasConnected = false;

        Binding I2CaddressBinding;
        public byte I2CAddress
        {
            get;
            set;           
        }

        public frmFlowTex()
        {
            InitializeComponent();
            FlowTEX = new cFlowTEX();
            lblFlow.Text = FlowTEX.getFlow().ToString(defaultFlowFormat) + defaultFlowUnit;
            lblTemperature.Text = FlowTEX.getTemperature().ToString(defaultTemperatureFormat) + defaultTemperatureUnit;

            comboSerialFlowTex.DropDown += ComboSerialFlowTex_DropDown;
            comboSerialFlowTex.Items.Add("AUTO");
            comboSerialFlowTex.SelectedIndex = 0;

            lblSerialNumber.Text = "";
            lblVersion.Text = "";
            lblModel.Text = "";

            I2CaddressBinding = edtI2CAddress.DataBindings.Add("Text",this,"I2CAddress");
            I2CaddressBinding.Format += I2CaddressBinding_Format;
            I2CaddressBinding.Parse += I2CaddressBinding_Parse;            
        }

        private void I2CaddressBinding_Parse(object sender, ConvertEventArgs e)
        {
            string s = (string)e.Value;
            byte result = 0;
            bool bSuccess = false;

            if((s != null) &&
                ((s.StartsWith("0x")) || (s.StartsWith("0X"))) &&
                byte.TryParse(s.Substring(2), System.Globalization.NumberStyles.AllowHexSpecifier, null, out result))
            {
                if((result > 0) && (result <= 0x7F))
                {
                    e.Value = result;
                    bSuccess = true;
                }
                else
                {
                    e.Value = I2CAddress;
                }
            }
            else if(byte.TryParse(s, out result))
            {
                if((result > 0) && (result <= 0x7F))
                {
                    e.Value = result;
                    bSuccess = true;
                }
                else
                {
                    e.Value = I2CAddress;
                }
            }

            if(!bSuccess)
            { MessageBox.Show("Valor inválido!\n Valores permitidos 0x01 a 0x7F", "Valor Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void I2CaddressBinding_Format(object sender, ConvertEventArgs e)
        {
            if(e.DesiredType == typeof(string))
            {
                e.Value = "0x" + (string)((byte)e.Value).ToString("X2");
            }
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
                if(!wasConnected)
                {
                    wasConnected = true;

                    if(FlowTEX.getModel(out string model))
                    {
                        lblModel.Text = model;
                    }

                    if(FlowTEX.getSerialNumber(out string serialNumber))
                    {
                        lblSerialNumber.Text = serialNumber;
                    }

                    if(FlowTEX.getVersion(out string version))
                    {
                        lblVersion.Text = version;
                    }

                    if(FlowTEX.getI2CAddress(out byte Address  ))
                    {                        
                        I2CAddress = Address;
                        I2CaddressBinding.ReadValue();
                    }
                }

                lblFlow.Text = FlowTEX.getFlow().ToString(defaultFlowFormat) + defaultFlowUnit;
                lblTemperature.Text = FlowTEX.getTemperature().ToString(defaultTemperatureFormat) + defaultTemperatureUnit;
            }
            else
            {
                wasConnected = false;
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

        private void edtI2CAddress_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Return)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                Validate();
            }
        }

        private void btnChangeI2CAddress_Click(object sender, EventArgs e)
        {
            if(  MessageBox.Show("Deseja alterar o endereço de I2C?", "Alteração de endereço de I2C", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes )
            {
                if(FlowTEX.setI2CAddress(I2CAddress))
                { MessageBox.Show("Endereço alterado com sucesso!", "Alteração de endereço de I2C", MessageBoxButtons.OK, MessageBoxIcon.Information); }
                else
                { MessageBox.Show("Falha na  alteração do endereço", "Alteração de endereço de I2C", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            }
        }
    }
}
