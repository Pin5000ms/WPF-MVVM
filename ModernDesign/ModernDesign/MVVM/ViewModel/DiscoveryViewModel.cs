using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using ModernDesign.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace ModernDesign.MVVM.ViewModel
{
    public class DiscoveryViewModel : ObservableObject
    {
        public DiscoveryViewModel()
        {
            Capture = new Capture(0);
            MatFrame = Capture.QueryFrame();
            Frame = ToBitmapSource(MatFrame);
            Capture.ImageGrabbed += Update;
            StartCapture();
        }

        private BitmapSource frame;

        public BitmapSource Frame
        {
            get { return frame; }
            set 
            { 
                frame = value;
                OnPropertyChanged();//必須加 才會觸發變更
            }
        }


        public Mat MatFrame { get; set; }
        public Capture Capture { get; set; }

        private Thread cap;

        public Action OnFrameGet;

        public void StartCapture()
        {
            if (cap == null)
            {
                cap = new Thread(CaptureThread);
                cap.Start();
            }

        }
        public void StopCapture()
        {
            cap.Abort();
            cap = null;
        }

        public void SaveImage(string fileName)
        {
            MatFrame.Save(fileName);
        }


        /// <summary>
        /// Delete a GDI object
        /// </summary>
        /// <param name="o">The poniter to the GDI object to be deleted</param>
        /// <returns></returns>
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        /// <summary>
        /// Convert an IImage to a WPF BitmapSource. The result can be used in the Set Property of Image.Source
        /// </summary>
        /// <param name="image">The Emgu CV Image</param>
        /// <returns>The equivalent BitmapSource</returns>
        public static BitmapSource ToBitmapSource(IImage image)
        {
            using (System.Drawing.Bitmap source = image.Bitmap)
            {
                IntPtr ptr = source.GetHbitmap(); //obtain the Hbitmap

                BitmapSource bs = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    ptr,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    System.Windows.Media.Imaging.BitmapSizeOptions.FromEmptyOptions());

                DeleteObject(ptr); //release the HBitmap
                return bs;
            }
        }

        public void Update(object sender, EventArgs e)
        {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                Frame = ToBitmapSource(MatFrame);
            }));
        }
        void CaptureThread()
        {
            
            while (true)
            {
                MatFrame = Capture.QueryFrame();
            }
        }
    }
    
}
