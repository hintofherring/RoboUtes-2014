using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RiftDotNet;
using SharpDX;
using System.Threading;

using commSockServer;
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

        public delegate void YawChangedEventHandler(double newOrientation);
        public event YawChangedEventHandler yawChanged;

        public delegate void PitchChangedEventHandler(double newOrientation);
        public event PitchChangedEventHandler pitchChanged;


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
                if (yawChanged != null) {
                    yawChanged(results[0]);
                }
                if (pitchChanged != null) {
                    pitchChanged(results[1]);
                }
                Thread.Sleep(20); //will update ~50 times per second
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

    public class OculusTransmitter {

        private commSockReceiver CSR;
        private OculusOrientation OcuOrient;
        private Timer sendTimer;
        private int YAW = 180; //just safe starting values
        private int PITCH = 180; //just safe starting values
        object sync = 1;
        private volatile bool orientationChanged = false;

        public OculusTransmitter(commSockReceiver _CSR, OculusOrientation _OcuOrient) {
            CSR = _CSR;
            OcuOrient = _OcuOrient;
            OcuOrient.yawChanged += OcuOrient_yawChanged;
            OcuOrient.pitchChanged += OcuOrient_pitchChanged;
            sendTimer = new Timer(sendTimerCallback, null,100, 50); //can send up to 20 times per second
        }

        void OcuOrient_pitchChanged(double newOrientation) {
            int possibleNewOrientation = ((int)newOrientation).Map(-120, 120, 0, 180);
            lock (sync) {
                if (possibleNewOrientation != PITCH) {
                    //Console.WriteLine("New PITCH: " + possibleNewOrientation);
                    orientationChanged = true;
                    PITCH = possibleNewOrientation;
                }
            }
        }

        void OcuOrient_yawChanged(double newOrientation) {
            int possibleNewOrientation = ((int)newOrientation).Map(300, 50, 0, 180);
            lock (sync) {
                if (possibleNewOrientation != YAW) { //if the new value is sufficiently different (oculus picks up tiny movement after the decimal all the time)
                    //Console.WriteLine("New YAW: " + possibleNewOrientation);
                    orientationChanged = true;
                    YAW = possibleNewOrientation;
                }
            }
        }

        private void sendTimerCallback(object state) {
                lock (sync) {
                    if (orientationChanged) {
                        CSR.write("P:" + PITCH);
                        CSR.write("Y:" + YAW);
                        orientationChanged = false;
                    }
                }
        }


    }

    public static class ExtensionMethods {
        public static int Map(this int value, int fromSource, int toSource, int fromTarget, int toTarget) {
            return (value - fromSource) * (toTarget - fromTarget) / (toSource - fromSource) + fromTarget;
        }
    }
}
