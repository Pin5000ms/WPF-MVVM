using Emgu.CV;
using OpenTK.Input;
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
using MouseButton = System.Windows.Input.MouseButton;
using MouseButtonEventArgs = System.Windows.Input.MouseButtonEventArgs;
using MouseEventArgs = System.Windows.Input.MouseEventArgs;

namespace ModernDesign
{
    /// <summary>
    /// SingleBlock.xaml 的互動邏輯
    /// </summary>
    public partial class SingleBlockView : UserControl
    {
        private bool isDragging = false;
        private Point startPosition;
        public SingleBlock viewModel;

        public bool InputButtonClicked = false;
        public bool OutputButtonClicked = false;

        public SingleBlockView()
        {
            InitializeComponent();
            viewModel = new SingleBlock();
            DataContext = viewModel;
        }

        private void OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.isDragging = true;
            this.startPosition = e.GetPosition(this);
            //Console.WriteLine(startPosition.X + "," + startPosition.Y);
            this.CaptureMouse();
        }

        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (this.isDragging && e.LeftButton == MouseButtonState.Pressed)
            {
                var canvas = FindCanvas(this);
                Point newPosition = e.GetPosition(canvas);

                //if(newPosition.X < startPosition.X || newPosition.Y < startPosition.Y||
                //    newPosition.X > canvas.ActualWidth- startPosition.X || newPosition.Y > canvas.ActualHeight - startPosition.Y)
                //{
                //    return;
                //}

                var left = newPosition.X - startPosition.X;
                var top = newPosition.Y - startPosition.Y;


                if (newPosition.X > startPosition.X && newPosition.X < canvas.ActualWidth - (this.ActualWidth - startPosition.X))
                {
                    Canvas.SetLeft(this, left);
                    viewModel.Left = left;
                }

                if (newPosition.Y > startPosition.Y && newPosition.Y < canvas.ActualHeight - (this.ActualHeight - startPosition.Y))
                {
                    Canvas.SetTop(this, top);
                    viewModel.Top = top;
                }

                //Console.WriteLine(newPosition.X + "," + newPosition.Y);

            }
        }

        private void OnPreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            this.isDragging = false;
            this.ReleaseMouseCapture();
        }

        private Canvas FindCanvas(DependencyObject obj)
        {
            while (obj != null)
            {
                var canvas = obj as Canvas;
                if (canvas != null && canvas.Name == "FlowChartCanvas")
                {
                    return canvas;
                }

                obj = VisualTreeHelper.GetParent(obj);
            }

            return null;
        }

        private void InputMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            viewModel.DrawingLine = false;
            InputButtonClicked = true;
        }

        private void OutputMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            viewModel.DrawingLine = true;
            OutputButtonClicked = true;
        }

    }
}
