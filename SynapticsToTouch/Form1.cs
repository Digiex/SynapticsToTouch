using SYNCTRLLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SynapticsToTouch
{
    public partial class Form1 : Form
    {
        SynAPICtrl synAPI;
        SynDeviceCtrl synDev;
        SynPacketCtrl synPacket;
        int DeviceHandle = -1;
        public Form1()
        {
            InitializeComponent();
            synAPI = new SynAPICtrlClass();
            synDev = new SynDeviceCtrlClass();
            synPacket = new SynPacketCtrlClass();
        }

        void synDev_OnPacket()
        {
            synDev.LoadPacket(synPacket);
            if(synPacket.X != 0 || synPacket.Y != 0)
            Debug.WriteLine(synPacket.X + ", " + synPacket.Y + ", " + synPacket.FingerState);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            synAPI.Initialize();
            synAPI.Activate();
            DeviceHandle = synAPI.FindDevice(SynConnectionType.SE_ConnectionAny, SynDeviceType.SE_DeviceTouchPad, -1);
            if (DeviceHandle == -1)
            {
                MessageBox.Show("Unable to find a Synaptics TouchPad");

            }
            synDev.Select(DeviceHandle);
            synDev.Activate();
            synDev.OnPacket += synDev_OnPacket;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            synDev.Unacquire();
        }
    }
}
