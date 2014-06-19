using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiftDotNet;
using SharpDX;
using System.Threading;

namespace OculusOrientationLibrary
{
    public class OculusOrientation
    {
        private static OculusOrientation instance;

        private static IHMDDevice device = null;
        private static IDeviceManager manager;
        private static ISensorFusion fusion;
        private Thread updateThread;

        /// <summary>
        /// the double[] contains the most current YAW, PITCH, & ROLL in that order.
        /// </summary>
        /// <param name="newOrientation"></param>
        /// <param name="e"></param>
        public delegate void OrientationChangedEventHandler(double[] newOrientation);
        public event OrientationChangedEventHandler orientationChanged;

        private OculusOrientation()
        {
            manager = Factory.CreateDeviceManager();
            fusion = Factory.CreateSensorFusion();
            var devices = manager.HMDDevices;
            if (devices.Length <= 0)
            {
                throw new Exception("No Oculus Hardware found");
            }
            var handle = devices[0];
            Console.WriteLine("Found an HMD device: " + handle.DeviceInfo.DisplayDevice);

            device = handle.CreateDevice();

            fusion.AttachedDevice = device.Sensor;

            fusion.IsPredictionEnabled = true;

            updateThread = new Thread(new ThreadStart(updateOrientation));
            updateThread.Start();
        }

        private void updateOrientation(){
            Quaternion q;
            double[] results;
            while (true)
            {
                q = fusion.PredictedOrientation;

                results = new double[] { q.Y * 180 + 180, q.X * 180, q.Z * 180 }; //YAW, PITCH, ROLL
                if (orientationChanged != null)
                {
                    orientationChanged(results);
                }
                Thread.Sleep(33); //will update ~30 times per second
            }
        }

        /// <summary>
        /// Using a factory model, returns an instance (the only one actually) of the Oculus. The device will start firing events with an updated orientation immediately.
        /// </summary>
        /// <returns></returns>
        public static OculusOrientation getInstance()
        {
            if (instance == null)
            {
                instance = new OculusOrientation();
            }
            return instance;
        }

        public void resetOrientation()
        {
            fusion.Reset();
        }
    }
}
