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

        public int XMin;
        public int XMax;
        public int YMin;
        public int YMax;
        public int ZTouchThreshold;

        public int wWidth;
        public int wHeight;

        public Form1()
        {
            InitializeComponent();
            synAPI = new SynAPICtrlClass();
            synDev = new SynDeviceCtrlClass();
            synPacket = new SynPacketCtrlClass();
        }

        PointerTouchInfo[] contacts = new PointerTouchInfo[1];
        void synDev_OnPacket()
        {
            int X, Y;
            synDev.LoadPacket(synPacket);
            X = (synPacket.X - XMin) * wWidth / (XMax - XMin);
            Y = (YMax - synPacket.Y) * wHeight / (YMax - YMin);
            //Finger = synPacket.FingerState & (int)SynFingerFlags.SF_FingerPresent;
            if (contacts[0].PointerInfo.PointerId == 0 && synPacket.FingerState != 0)
            {
                contacts[0] = MakePointerTouchInfo(X, Y, 2, 1);
                //contacts[1] = MakePointerTouchInfo(650, 500, 2, 2);
                bool success = TouchInjector.InjectTouchInput(1, contacts);
            }
            else if (synPacket.FingerState != 0)
            {
                contacts[0].PointerInfo.PtPixelLocation.X = X;
                contacts[0].PointerInfo.PtPixelLocation.Y = Y;
                contacts[0].PointerInfo.PointerFlags = PointerFlags.UPDATE | PointerFlags.INRANGE | PointerFlags.INCONTACT;
                //contacts[1].PointerInfo.PointerFlags = PointerFlags.UPDATE | PointerFlags.INRANGE | PointerFlags.INCONTACT;

                bool s = TouchInjector.InjectTouchInput(1, contacts);
            }
            else
            {
                //release them
                contacts[0].PointerInfo.PointerFlags = PointerFlags.UP;
                //contacts[1].PointerInfo.PointerFlags = PointerFlags.UP;

                bool success2 = TouchInjector.InjectTouchInput(1, contacts);
                contacts[0].PointerInfo.PointerId = 0;
            }
        }
        public Rectangle GetScreen()
        {
            return Screen.FromControl(this).Bounds;
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
            else
            {
                synDev.Select(DeviceHandle);
                synDev.Activate();
                bool s = TouchInjector.InitializeTouchInjection();//initialize with default settings

                XMin = synDev.GetLongProperty(SynDeviceProperty.SP_XLoSensor);
                XMax = synDev.GetLongProperty(SynDeviceProperty.SP_XHiSensor);
                YMin = synDev.GetLongProperty(SynDeviceProperty.SP_YLoSensor);
                YMax = synDev.GetLongProperty(SynDeviceProperty.SP_YHiSensor);
                ZTouchThreshold = synDev.GetLongProperty(SynDeviceProperty.SP_ZTouchThreshold) + 20;
                wHeight = GetScreen().Height;
                wWidth = GetScreen().Width; 
                synDev.Acquire(0);

                synDev.OnPacket += synDev_OnPacket;
            }
        }

        private PointerTouchInfo MakePointerTouchInfo(int x, int y, int radius, uint id, uint orientation = 90, uint pressure = 32000)
        {
            PointerTouchInfo contact = new PointerTouchInfo();
            contact.PointerInfo.pointerType = PointerInputType.TOUCH;
            contact.TouchFlags = TouchFlags.NONE;
            contact.Orientation = orientation;
            contact.Pressure = pressure;
            contact.PointerInfo.PointerFlags = PointerFlags.DOWN | PointerFlags.INRANGE | PointerFlags.INCONTACT;
            contact.TouchMasks = TouchMask.CONTACTAREA | TouchMask.ORIENTATION | TouchMask.PRESSURE;
            contact.PointerInfo.PtPixelLocation.X = x;
            contact.PointerInfo.PtPixelLocation.Y = y;
            contact.PointerInfo.PointerId = id;
            contact.ContactArea.left = x - radius;
            contact.ContactArea.right = x + radius;
            contact.ContactArea.top = y - radius;
            contact.ContactArea.bottom = y + radius;
            return contact;
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (DeviceHandle != -1)
            {
                synDev.Unacquire();
            }
        }
    }
}
