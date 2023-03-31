using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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

namespace ModernDesign
{
    /// <summary>
    /// HomeView.xaml 的互動邏輯
    /// </summary>
    public partial class HomeView : UserControl
    {
        private HomeViewModel HomeVM;

        private bool _isMoving;
        private Point? _buttonPosition;
        private double deltaX;
        private double deltaY;
        private TranslateTransform _currentTT;

        public HomeView(HomeViewModel _homeVM)
        {
            InitializeComponent();
            DataContext = _homeVM;
            HomeVM = _homeVM;
            CompositionTarget.Rendering += OnRendering;
        }
        private void Drop(object sender, DragEventArgs e)
        {
            var source = (e.Data.GetData("Source") as Button).DataContext as ModernDesign.Block;
            if (source != null)
            {
                //ver.1
                int newIndex = blocklistview.Items.IndexOf((sender as Button).DataContext);
                var list = (blocklistview.ItemsSource) as ObservableCollection<ModernDesign.Block>;
                int oldIndex = list.IndexOf(source);
                list.RemoveAt(oldIndex);
                list.Insert(newIndex, source);

                //ver.2
                //int newIndex = HomeVM.HomePageBlocks.IndexOf((sender as Button).DataContext as ModernDesign.MVVM.ViewModel.Block);
                //int oldIndex = HomeVM.HomePageBlocks.IndexOf(source);
                //HomeVM.HomePageBlocks.RemoveAt(oldIndex);
                //HomeVM.HomePageBlocks.Insert(newIndex, source);
            }
        }

        private void DragLeave(object sender, DragEventArgs e)
        {

        }

        private void PreviewMouseMove(object sender, MouseEventArgs e)
        {
            //if (_isMoving)
            //{
            //    var mousePoint = Mouse.GetPosition(blocklistview);

            //    var offsetX = (_currentTT == null ? _buttonPosition.Value.X : _buttonPosition.Value.X - _currentTT.X) + deltaX - mousePoint.X;
            //    var offsetY = (_currentTT == null ? _buttonPosition.Value.Y : _buttonPosition.Value.Y - _currentTT.Y) + deltaY - mousePoint.Y;

            //    (sender as Button).RenderTransform = new TranslateTransform(-offsetX, -offsetY);
            //};
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                
                Task.Factory.StartNew(new Action(() =>
                {
                    Thread.Sleep(100);
                    App.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (e.LeftButton == MouseButtonState.Pressed)
                        { 
                            //GiveFeedbackEventHandler handler = new GiveFeedbackEventHandler(DragSource_GiveFeedback);
                            //GiveFeedback += handler;
                            var data = new DataObject();
                            data.SetData("Source", (sender as Button));
                            DragDrop.DoDragDrop(sender as DependencyObject, data, DragDropEffects.Move);
                            //GiveFeedback -= handler;
                            e.Handled = true;
                        }
                    }), null);
                }), CancellationToken.None);
            }
        }

        void DragSource_GiveFeedback(object sender, GiveFeedbackEventArgs e)
        {
            try
            {
                //This loads the cursor from a stream .. 
                //if (_allOpsCursor == null)
                //{
                //    using (Stream cursorStream = System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceStream("SimplestDragDrop.DDIcon.cur"))
                //    {
                //        _allOpsCursor = new Cursor(cursorStream);
                //    }
                //}
                //Mouse.SetCursor(customCursor);
                e.UseDefaultCursors = false;
                e.Handled = true;
            }
            finally { }
        }

        private void OnRendering(object sender, EventArgs e)
        {
            
            
        }

        private void PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //if (_buttonPosition == null)
            //    _buttonPosition = (sender as Button).TransformToAncestor(blocklistview).Transform(new Point(0, 0));
            //var mousePosition = Mouse.GetPosition(blocklistview);
            //deltaX = mousePosition.X - _buttonPosition.Value.X;
            //deltaY = mousePosition.Y - _buttonPosition.Value.Y;
            //_isMoving = true;
        }

        private void PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            //_currentTT = (sender as Button).RenderTransform as TranslateTransform;
            //_isMoving = false;
        }
    }

}
