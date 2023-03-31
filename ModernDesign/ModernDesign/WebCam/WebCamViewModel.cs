using Emgu.CV;
using ModernDesign.Core;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using System.Drawing.Imaging;

namespace ModernDesign
{
    public class WebCamViewModel : ObservableObject
    {
        public WebCamViewModel()
        {
            Capture = new VideoCapture(0);
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
        public VideoCapture Capture { get; set; }

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
        //
        // 摘要:
        //     Delete a GDI object
        //
        // 參數:
        //   o:
        //     The pointer to the GDI object to be deleted
        [DllImport("gdi32")]
        private static extern int DeleteObject(IntPtr o);

        //
        // 摘要:
        //     Convert an IImage to a WPF BitmapSource. The result can be used in the Set Property
        //     of Image.Source
        //
        // 參數:
        //   image:
        //     The Emgu CV Image
        //
        // 傳回:
        //     The equivalent BitmapSource
        public static BitmapSource ToBitmapSource(Mat image)
        {
            Bitmap val = BitmapExtension.ToBitmap(image, false);
            try
            {
                IntPtr hbitmap = val.GetHbitmap();
                BitmapSource result = Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                DeleteObject(hbitmap);
                return result;
            }
            finally
            {
                ((IDisposable)val)?.Dispose();
            }
        }
        void CaptureThread()
        {
            while (true)
            {
                MatFrame = Capture.QueryFrame();
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
                {
                    Frame = ToBitmapSource(MatFrame);
                }));
            }
        }
    }
    
}
