using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AForge.Video;
using AForge.Video.DirectShow;
using System.Net;
using System.Net.Sockets;

namespace videoSocketTools
{
    public class managedVideoTransmitter
    {
        private VideoCaptureDevice cameraSource;
        private IPAddress IP;
        private int port;
        videoSocketSender VSS;
        private volatile bool ON = false;
        private volatile bool connected = false;

        public delegate void actualFPSChangedEventHandler(int newFPS);
        public event actualFPSChangedEventHandler actualFPSRecalculated;


        public managedVideoTransmitter(VideoCaptureDevice _cameraSource, IPAddress _IP, int _port)
        {
            cameraSource = _cameraSource;
            IP = _IP;
            port = _port;
            VSS = new videoSocketSender(cameraSource);
            VSS.connectionLost += VSS_connectionLost;
            VSS.actualFPSRecalculated += VSS_acualFPSRecalculated;
        }

        void VSS_acualFPSRecalculated(int newFPS)
        {
            if (actualFPSRecalculated != null)
            {
                actualFPSRecalculated(newFPS);
            }
        }

        public void stop()
        {
            ON = false;
            VSS.stop();
        }

        /// <summary>
        /// set the video quality to be transmitted. Accepts values between 0 and 100.
        /// </summary>
        /// <param name="quality"></param>
        public void setQuality(int quality)
        {
            if (quality > 100 || quality < 0)
            {
                //do nothing... Its out of range...
            }
            else
            {
                VSS.transmissionQuality = (long)quality;
            }

        }

        public void setFPS(int FPS)
        {
            if (FPS < 1)
            {
                VSS.transmissionFPS = 1;
            }
            else if (FPS > 1000)
            {
                VSS.transmissionFPS = 1000;
            }
            else
            {
                VSS.transmissionFPS = FPS;
            }
        }

        public void startTransmitting()
        {
            ON = true;
            if (connected)
            {
                VSS.beginTransmitting();
            }
            else
            {
                VSS.beginConnect(IP, port, connectedCallback);
            }
        }

        private void VSS_connectionLost()
        {
            VSS.stop();
            connected = false;
            VSS.beginConnect(IP, port, connectedCallback);
        }

        private void connectedCallback(bool connectionStatus)
        {
            if (connectionStatus)
            {
                connected = true;
                VSS.beginTransmitting();
            }
            else
            {
                connected = false;
                VSS.beginConnect(IP, port, connectedCallback);
            }
        }
    }

    public class managedDualVideoTransmitter
    {
        private VideoCaptureDevice leftCameraSource;
        private VideoCaptureDevice rightCameraSource;
        private IPAddress IP;
        private int port;
        dualVideoSocketSender VSS;
        private volatile bool ON = false;
        private volatile bool connected = false;

        public delegate void actualFPSChangedEventHandler(int newFPS);
        public event actualFPSChangedEventHandler actualFPSRecalculated;


        public managedDualVideoTransmitter(VideoCaptureDevice _leftCameraSource, VideoCaptureDevice _rightCameraSource, IPAddress _IP, int _port)
        {
            leftCameraSource = _leftCameraSource;
            rightCameraSource = _rightCameraSource;
            IP = _IP;
            port = _port;
            VSS = new dualVideoSocketSender(leftCameraSource, rightCameraSource);
            VSS.connectionLost += VSS_connectionLost;
            VSS.actualFPSRecalculated += VSS_acualFPSRecalculated;
        }

        void VSS_acualFPSRecalculated(int newFPS)
        {
            if (actualFPSRecalculated != null)
            {
                actualFPSRecalculated(newFPS);
            }
        }

        public void stop()
        {
            ON = false;
            VSS.stop();
        }

        /// <summary>
        /// set the video quality to be transmitted. Accepts values between 0 and 100.
        /// </summary>
        /// <param name="quality"></param>
        public void setQuality(int quality)
        {
            if (quality > 100 || quality < 0)
            {
                //do nothing... Its out of range...
            }
            else
            {
                VSS.transmissionQuality = (long)quality;
            }

        }

        public void setFPS(int FPS)
        {
            if (FPS < 1)
            {
                VSS.transmissionFPS = 1;
            }
            else if (FPS > 1000)
            {
                VSS.transmissionFPS = 1000;
            }
            else
            {
                VSS.transmissionFPS = FPS;
            }
        }

        public void startTransmitting()
        {
            ON = true;
            if (connected)
            {
                VSS.beginTransmitting();
            }
            else
            {
                VSS.beginConnect(IP, port, connectedCallback);
            }
        }

        private void VSS_connectionLost()
        {
            VSS.stop();
            connected = false;
            VSS.beginConnect(IP, port, connectedCallback);
        }

        private void connectedCallback(bool connectionStatus)
        {
            if (connectionStatus)
            {
                connected = true;
            }
            else
            {
                connected = false;
                VSS.beginConnect(IP, port, connectedCallback);
            }
        }
    }
}
