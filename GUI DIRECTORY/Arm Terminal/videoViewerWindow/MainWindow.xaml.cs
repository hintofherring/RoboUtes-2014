using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.IO;
using videoSocketToolsV2;
using System.Net;
using System.Net.Sockets;
using System.Drawing;
using System.Runtime.InteropServices;


namespace videoViewerWindow {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class videoWindow : Window {

        private videoSocketReceiver VSR;
        private int port;
        private monitors targetMonitor;
        JpegBitmapDecoder JpegDec;
        private string name;

        public videoWindow(int _port, monitors monitorToDisplayOn, string _name) {
            InitializeComponent();
            port = _port;
            VSR = new videoSocketReceiver(port, connectionCallback);
            targetMonitor = monitorToDisplayOn;
            name = _name;
        }

        private void connectionCallback(bool connectionStatus) {
            if (connectionStatus) {
                VSR.frameReceived += VSR_frameReceived;
                VSR.connectionLost += VSR_connectionLost;
            }
            else {
                VSR.close();
                VSR = new videoSocketReceiver(port, connectionCallback);
            }
        }

        private void VSR_connectionLost() {
            Console.WriteLine("video connection Loss Detected on Server Side, autoreconnect routine engaged");
            VSR.close();
            VSR = new videoSocketReceiver(port, connectionCallback);
        }

        private void VSR_frameReceived(byte[] newFrame) {
            if (newFrame.Length != null && newFrame.Length > 0) {
                displayFrame(newFrame);
            }
        }

        public void displayFrame(byte[] frame) {
            try {
                Action work = delegate
                {
                    imageBox.Source = ByteImageConverter.ByteToImage(frame);
                };
                Dispatcher.Invoke(work);
            }
            catch {
                return; //do nothing...
            }
        }


        private void Window_Loaded(object sender, RoutedEventArgs e) {
            switch (targetMonitor)
            {
                case monitors.secondMonitor:
                    this.MaximizeToSecondaryMonitor();
                    break;
                case monitors.thirdMonitor:
                    this.MaximizeToThirdMonitor();
                    break;
            }
            this.Title = name;
        }

        private void MaximizeToSecondaryMonitor() {
            var secondaryScreen = System.Windows.Forms.Screen.AllScreens.Where(s => !s.Primary).FirstOrDefault();

            if (secondaryScreen != null) {
                if (!this.IsLoaded)
                    this.WindowStartupLocation = WindowStartupLocation.Manual;

                var workingArea = secondaryScreen.WorkingArea;
                this.Left = workingArea.Left;
                this.Top = workingArea.Top;
                this.Width = workingArea.Width;
                this.Height = workingArea.Height;
                // If window isn't loaded then maxmizing will result in the window displaying on the primary monitor
                if (this.IsLoaded)
                    this.WindowState = WindowState.Normal;
            }
        }

        private void MaximizeToThirdMonitor()
        {
            System.Windows.Forms.Screen secondaryScreen = System.Windows.Forms.Screen.AllScreens.Where(s => !s.Primary).FirstOrDefault();
            System.Windows.Forms.Screen tertiaryScreen = System.Windows.Forms.Screen.AllScreens.Where(s => s != secondaryScreen).FirstOrDefault();

            if (tertiaryScreen != null)
            {
                if (!this.IsLoaded)
                    this.WindowStartupLocation = WindowStartupLocation.Manual;

                var workingArea = tertiaryScreen.WorkingArea;
                this.Left = workingArea.Left;
                this.Top = workingArea.Top;
                this.Width = workingArea.Width;
                this.Height = workingArea.Height;
                // If window isn't loaded then maxmizing will result in the window displaying on the primary monitor
                if (this.IsLoaded)
                    this.WindowState = WindowState.Normal;
            }

            else if (secondaryScreen != null)
            {
                if (!this.IsLoaded)
                    this.WindowStartupLocation = WindowStartupLocation.Manual;

                var workingArea = secondaryScreen.WorkingArea;
                this.Left = workingArea.Left;
                this.Top = workingArea.Top;
                this.Width = workingArea.Width;
                this.Height = workingArea.Height;
                // If window isn't loaded then maxmizing will result in the window displaying on the primary monitor
                if (this.IsLoaded)
                    this.WindowState = WindowState.Normal;
            }
        }

        public enum monitors
        {
            secondMonitor,
            thirdMonitor
        }

    }

    public class ByteImageConverter {
        public static ImageSource ByteToImage(byte[] imageData)
        {
            BitmapImage biImg = new BitmapImage();
            WrappingStream WS;
            using (MemoryStream ms = new MemoryStream(imageData))
            {
               WS = new WrappingStream(ms);
                biImg.BeginInit();
                biImg.CacheOption = BitmapCacheOption.OnLoad;
                biImg.StreamSource = WS;
                biImg.EndInit();
                biImg.Freeze();
            }
            WS.Dispose();
            ImageSource imgSrc = biImg as ImageSource;
            
            return imgSrc;
        }
    }

    /// <summary>
    /// A <see cref="Stream"/> that wraps another stream. The major feature of <see cref="WrappingStream"/> is that it does not dispose the
    /// underlying stream when it is disposed; this is useful when using classes such as <see cref="BinaryReader"/> and
    /// <see cref="System.Security.Cryptography.CryptoStream"/> that take ownership of the stream passed to their constructors.
    /// </summary>
    public class WrappingStream : Stream
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WrappingStream"/> class.
        /// </summary>
        /// <param name="streamBase">The wrapped stream.</param>
        public WrappingStream(Stream streamBase)
        {
            // check parameters
            if (streamBase == null)
                throw new ArgumentNullException("streamBase");

            m_streamBase = streamBase;
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports reading.
        /// </summary>
        /// <returns><c>true</c> if the stream supports reading; otherwise, <c>false</c>.</returns>
        public override bool CanRead
        {
            get { return m_streamBase == null ? false : m_streamBase.CanRead; }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports seeking.
        /// </summary>
        /// <returns><c>true</c> if the stream supports seeking; otherwise, <c>false</c>.</returns>
        public override bool CanSeek
        {
            get { return m_streamBase == null ? false : m_streamBase.CanSeek; }
        }

        /// <summary>
        /// Gets a value indicating whether the current stream supports writing.
        /// </summary>
        /// <returns><c>true</c> if the stream supports writing; otherwise, <c>false</c>.</returns>
        public override bool CanWrite
        {
            get { return m_streamBase == null ? false : m_streamBase.CanWrite; }
        }

        /// <summary>
        /// Gets the length in bytes of the stream.
        /// </summary>
        public override long Length
        {
            get { ThrowIfDisposed(); return m_streamBase.Length; }
        }

        /// <summary>
        /// Gets or sets the position within the current stream.
        /// </summary>
        public override long Position
        {
            get { ThrowIfDisposed(); return m_streamBase.Position; }
            set { ThrowIfDisposed(); m_streamBase.Position = value; }
        }

        /// <summary>
        /// Begins an asynchronous read operation.
        /// </summary>
        public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            ThrowIfDisposed();
            return m_streamBase.BeginRead(buffer, offset, count, callback, state);
        }

        /// <summary>
        /// Begins an asynchronous write operation.
        /// </summary>
        public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            ThrowIfDisposed();
            return m_streamBase.BeginWrite(buffer, offset, count, callback, state);
        }

        /// <summary>
        /// Waits for the pending asynchronous read to complete.
        /// </summary>
        public override int EndRead(IAsyncResult asyncResult)
        {
            ThrowIfDisposed();
            return m_streamBase.EndRead(asyncResult);
        }

        /// <summary>
        /// Ends an asynchronous write operation.
        /// </summary>
        public override void EndWrite(IAsyncResult asyncResult)
        {
            ThrowIfDisposed();
            m_streamBase.EndWrite(asyncResult);
        }

        /// <summary>
        /// Clears all buffers for this stream and causes any buffered data to be written to the underlying device.
        /// </summary>
        public override void Flush()
        {
            ThrowIfDisposed();
            m_streamBase.Flush();
        }

        /// <summary>
        /// Reads a sequence of bytes from the current stream and advances the position
        /// within the stream by the number of bytes read.
        /// </summary>
        public override int Read(byte[] buffer, int offset, int count)
        {
            ThrowIfDisposed();
            return m_streamBase.Read(buffer, offset, count);
        }

        /// <summary>
        /// Reads a byte from the stream and advances the position within the stream by one byte, or returns -1 if at the end of the stream.
        /// </summary>
        public override int ReadByte()
        {
            ThrowIfDisposed();
            return m_streamBase.ReadByte();
        }

        /// <summary>
        /// Sets the position within the current stream.
        /// </summary>
        /// <param name="offset">A byte offset relative to the <paramref name="origin"/> parameter.</param>
        /// <param name="origin">A value of type <see cref="T:System.IO.SeekOrigin"/> indicating the reference point used to obtain the new position.</param>
        /// <returns>The new position within the current stream.</returns>
        public override long Seek(long offset, SeekOrigin origin)
        {
            ThrowIfDisposed();
            return m_streamBase.Seek(offset, origin);
        }

        /// <summary>
        /// Sets the length of the current stream.
        /// </summary>
        /// <param name="value">The desired length of the current stream in bytes.</param>
        public override void SetLength(long value)
        {
            ThrowIfDisposed();
            m_streamBase.SetLength(value);
        }

        /// <summary>
        /// Writes a sequence of bytes to the current stream and advances the current position
        /// within this stream by the number of bytes written.
        /// </summary>
        public override void Write(byte[] buffer, int offset, int count)
        {
            ThrowIfDisposed();
            m_streamBase.Write(buffer, offset, count);
        }

        /// <summary>
        /// Writes a byte to the current position in the stream and advances the position within the stream by one byte.
        /// </summary>
        public override void WriteByte(byte value)
        {
            ThrowIfDisposed();
            m_streamBase.WriteByte(value);
        }

        /// <summary>
        /// Gets the wrapped stream.
        /// </summary>
        /// <value>The wrapped stream.</value>
        protected Stream WrappedStream
        {
            get { return m_streamBase; }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="WrappingStream"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            // doesn't close the base stream, but just prevents access to it through this WrappingStream
            if (disposing)
                m_streamBase = null;

            base.Dispose(disposing);
        }

        private void ThrowIfDisposed()
        {
            // throws an ObjectDisposedException if this object has been disposed
            if (m_streamBase == null)
                throw new ObjectDisposedException(GetType().Name);
        }

        Stream m_streamBase;
    }
}
