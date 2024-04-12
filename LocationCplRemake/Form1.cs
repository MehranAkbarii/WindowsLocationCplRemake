using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LocationCplRemake {
    public partial class Form1 : Form {

        String currentLocationAccess;
        public Form1() {
            InitializeComponent();
            GetLocationAccessStatusFromRegistry();
        }

        private void buttonCancel_Click(object sender, EventArgs e) {
            this.Close();
        }

        private void buttonOK_Click(object sender, EventArgs e) {
            if (checkBoxLocation.Checked) {
                updateLanguagesRegistry("Allow");
            } else updateLanguagesRegistry("Deny");
            this.Close();
        }
        private void GetLocationAccessStatusFromRegistry() {
            RegistryKey key = Registry.LocalMachine.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\CapabilityAccessManager\\ConsentStore\\location");
            if (key != null) {
                if (key.GetValueNames().Contains("Value")) {
                    string currentLocationAccess = (string)key.GetValue("Value");
                    if (currentLocationAccess == "Allow")
                        checkBoxLocation.Checked = true;
                    else
                        checkBoxLocation.Checked = false;
                }
            }
        }
        private void updateLanguagesRegistry(string inputValue) {
            string command = "Reg Add HKLM\\SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\CapabilityAccessManager\\ConsentStore\\location /v Value /t REG_SZ /d \"" + inputValue + "\" /f";

            ProcessStartInfo processStartInfo = new ProcessStartInfo {
                FileName = "cmd.exe",
                Arguments = $"/s /c {command}",
                Verb = "runas", // Run as administrator
                WindowStyle = ProcessWindowStyle.Hidden // Hide the command prompt window
            };

            Process.Start(processStartInfo);
        }
    }
}
