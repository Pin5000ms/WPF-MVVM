using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
            this.CaptureMouse();
        }

        private void OnPreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (this.isDragging && e.LeftButton == MouseButtonState.Pressed)
            {
                var canvas = FindCanvas(this);
                Point newPosition = e.GetPosition(canvas);

                var left = newPosition.X - startPosition.X;
                var top = newPosition.Y - startPosition.Y;

                if(left < 0)
                {
                    left = 0;
                }
                if (left > canvas.ActualWidth - this.ActualWidth)
                {
                    left = canvas.ActualWidth - this.ActualWidth;
                }
                if(top < 0)
                {
                    top = 0;
                }
                if(top > canvas.ActualHeight - this.ActualHeight)
                {
                    top = canvas.ActualHeight - this.ActualHeight;
                }

                Canvas.SetLeft(this, left);
                Canvas.SetTop(this, top);
                viewModel.Left = left;
                viewModel.Top = top;
                
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
