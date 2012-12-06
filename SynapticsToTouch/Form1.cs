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
        int lastFingerState = 0;
        PointerTouchInfo[] contacts = new PointerTouchInfo[1];
        void synDev_OnPacket()
        {
            synDev.LoadPacket(synPacket);
            if (calibrateState == 0)
            {
                int X, Y;
                X = Clamp(Clamp(synPacket.X - XMin, XMax, 0) * wWidth / (XMax - XMin), wWidth, 0);
                Y = Clamp(Clamp(YMax - synPacket.Y, YMax, 0) * wHeight / (YMax - YMin), wHeight, 0);
                if (contacts[0].PointerInfo.PointerId == 0 && synPacket.FingerState != 0)
                {
                    contacts[0] = MakePointerTouchInfo(X, Y, synPacket.W, 1);
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
                touchLabel.Text = "X: " + X + ", Y: " + Y + ", W: " + synPacket.W;
            }
            else
            {
                if (synPacket.FingerState != 0 && lastFingerState == 0)
                {
                    switch (calibrateState)
                    {
                        case 1:
                            XMin = synPacket.X;
                            calibrateLabel.Text = "Swipe from top.";
                            calibrateState = 2;
                            break;
                        case 2:
                            YMax = synPacket.Y;
                            calibrateLabel.Text = "Swipe from right.";
                            calibrateState = 3;
                            break;
                        case 3:
                            XMax = synPacket.X;
                            calibrateLabel.Text = "Swipe from bottom.";
                            calibrateState = 4;
                            break;
                        case 4:
                            YMin = synPacket.Y;
                            calibrateLabel.Text = "Done.";
                            calibrateState = 5;
                            break;
                        case 5:
                            calibrateState = 0;
                            break;
                    }
                    updateCalibrationStatusLabel();
                }
            }
            lastFingerState = synPacket.FingerState;
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
                bool s = TouchInjector.InitializeTouchInjection(256, TouchFeedback.INDIRECT);//initialize with default settings

                XMin = synDev.GetLongProperty(SynDeviceProperty.SP_XLoSensor);
                XMax = synDev.GetLongProperty(SynDeviceProperty.SP_XHiSensor);
                YMin = synDev.GetLongProperty(SynDeviceProperty.SP_YLoSensor);
                YMax = synDev.GetLongProperty(SynDeviceProperty.SP_YHiSensor);
                ZTouchThreshold = synDev.GetLongProperty(SynDeviceProperty.SP_ZTouchThreshold) + 20;
                wHeight = GetScreen().Height;
                wWidth = GetScreen().Width;
                synDev.Acquire(0);
                updateCalibrationStatusLabel();

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
        int calibrateState = 0;
        private void calibrateButton_Click(object sender, EventArgs e)
        {
            calibrateLabel.Text = "Swipe from left edge.";
            calibrateState = 1;
        }
        public static T Clamp<T>(T value, T max, T min)
         where T : System.IComparable<T>
        {
            T result = value;
            if (value.CompareTo(max) > 0)
                result = max;
            if (value.CompareTo(min) < 0)
                result = min;
            return result;
        }
        public void updateCalibrationStatusLabel()
        {
            calibrationStatusLabel.Text = "Left: " + XMin + ", Top: " + YMax + ", Right: " + XMax + ", Bottom:" + YMin;
        }
    }
}
