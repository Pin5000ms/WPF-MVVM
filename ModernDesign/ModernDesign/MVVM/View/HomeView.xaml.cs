using ModernDesign.MVVM.ViewModel;
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

namespace ModernDesign.MVVM.View
{
    /// <summary>
    /// HomeView.xaml 的互動邏輯
    /// </summary>
    public partial class HomeView : UserControl
    {
        private HomeViewModel HomeVM;
        public HomeView(HomeViewModel _homeVM)
        {
            InitializeComponent();
            DataContext = _homeVM;
            HomeVM = _homeVM;
        }
        private void Drop(object sender, DragEventArgs e)
        {
            var source = (e.Data.GetData("Source") as Button).DataContext as ModernDesign.MVVM.ViewModel.Block;
            if (source != null)
            {
                //ver.1
                int newIndex = blocklistview.Items.IndexOf((sender as Button).DataContext);
                var list = (blocklistview.ItemsSource) as ObservableCollection<ModernDesign.MVVM.ViewModel.Block>;
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

        private void PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                Task.Factory.StartNew(new Action(() =>
                {
                    Thread.Sleep(100);
                    App.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (e.LeftButton == MouseButtonState.Pressed)
                        {
                            var data = new DataObject();
                            data.SetData("Source", (sender as Button));
                            DragDrop.DoDragDrop(sender as DependencyObject, data, DragDropEffects.Move);
                            e.Handled = true;
                        }
                    }), null);
                }), CancellationToken.None);
            }
        }
    }

}
