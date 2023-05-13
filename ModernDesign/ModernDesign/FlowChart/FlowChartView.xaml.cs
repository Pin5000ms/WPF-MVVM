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

namespace ModernDesign
{
    /// <summary>
    /// FlowChartView.xaml 的互動邏輯
    /// </summary>
    public partial class FlowChartView : UserControl
    {
        private FlowChartViewModel flowchartVM;


        private Line selectedLineHighLight = null;

        public FlowChartView(FlowChartViewModel _viewModel)
        {
            InitializeComponent();
            this.flowchartVM = _viewModel;
            DataContext = flowchartVM;
        }

        int CurrentBlockIndex = -1;
        int CurrentDrawingLineIndex = -1;

        int startBlockIndex = -1;
        

        private void BlockPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentBlockIndex != -1)
            {
                //連接完成，更新末端點
                if(CurrentDrawingLineIndex != -1)
                {
                    var endBlock = sender as SingleBlockView;
                    var endBlockIndex = flowchartVM.Blocks.IndexOf(endBlock);

                    if (startBlockIndex == endBlockIndex || endBlock.OutputButtonClicked)
                    {
                        CancelDrawLine();
                        endBlock.OutputButtonClicked = false;
                        endBlock.viewModel.DrawingLine = false;
                    }
                    else
                    {
                        DrawLineEnd(endBlockIndex);

                        
                        endBlock.InputButtonClicked = false;
                    }
                }
            }

            //更新當前選擇block
            var block = sender as SingleBlockView;
            CurrentBlockIndex = flowchartVM.Blocks.IndexOf(block);
        }

        private void BlockMouseLeave(object sender, MouseEventArgs e)
        {
            var block = sender as SingleBlockView;
            if (block.viewModel.DrawingLine && CurrentDrawingLineIndex == -1)
            {
                var currentDrawingLine = new Line();
                currentDrawingLine.X1 = flowchartVM.Blocks[CurrentBlockIndex].viewModel.OutputX;
                currentDrawingLine.Y1 = flowchartVM.Blocks[CurrentBlockIndex].viewModel.OutputY;
                flowchartVM.Lines.Add(currentDrawingLine);
                CurrentDrawingLineIndex = flowchartVM.Lines.Count - 1;
                flowchartVM.Blocks[CurrentBlockIndex].viewModel.OutputLineIndex = CurrentDrawingLineIndex;
                //出發的block index
                startBlockIndex = CurrentBlockIndex;
            }
        }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            Point mousePoint = e.GetPosition(FlowChartCanvas);
            if (CurrentDrawingLineIndex != -1 && CurrentBlockIndex != -1 && flowchartVM.Blocks[CurrentBlockIndex].viewModel.DrawingLine)
            {
                //-10避免擋到InputButton
                DrawLine(mousePoint.X -10 , mousePoint.Y -10 , CurrentDrawingLineIndex);
            }
            if(CurrentBlockIndex != -1 && e.LeftButton == MouseButtonState.Pressed)
            {
                UpdateLineLocation();
            }
        }

        private void CanvasMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool isClickLine = e.OriginalSource is Line;

            if (!isClickLine && selectedLineHighLight != null)
            {
                RemoveLineHighLight();
            }

            if (isClickLine)
            {
                RemoveLineHighLight();
                var selectedLine = (Line)e.OriginalSource;
                AddLineHighLight(selectedLine);
            }

            bool isClickBlock = e.OriginalSource is SingleBlockView;
            //連接失敗
            if (!isClickBlock && CurrentBlockIndex != -1 && CurrentDrawingLineIndex != -1)
            {
                flowchartVM.Blocks[CurrentBlockIndex].viewModel.OutputLineIndex = -1;
                CancelDrawLine();
            }
            
        }
        private void CanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            
        }

        private void CanvasMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Line)
            {
                var selectedLine = (Line)e.OriginalSource;


                AddLineHighLight(selectedLine);



                // 建立右鍵菜單
                var contextMenu = new ContextMenu();
                var menuItem = new MenuItem();
                menuItem.Header = "Delete";
                menuItem.Click += (s, args) =>
                {
                    var lineIndex = flowchartVM.Lines.IndexOf(selectedLine);
                    // 從ViewModel中移除該Line
                    DeleteLine(lineIndex);

                    // 從ViewModel中移除該Line的HighLight
                    RemoveLineHighLight();
                };
                contextMenu.Items.Add(menuItem);

                // 顯示右鍵菜單
                contextMenu.IsOpen = true;


                e.Handled = true;
            }
        }

        private void CanvasMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void UpdateLineLocation()
        {
            var movingBlock = flowchartVM.Blocks[CurrentBlockIndex].viewModel;
            if(movingBlock.InputLineIndex != -1)
            {
                flowchartVM.Lines[movingBlock.InputLineIndex].X2 = movingBlock.InputX;
                flowchartVM.Lines[movingBlock.InputLineIndex].Y2 = movingBlock.InputY;
            }
            if(movingBlock.OutputLineIndex != -1)
            {
                flowchartVM.Lines[movingBlock.OutputLineIndex].X1 = movingBlock.OutputX;
                flowchartVM.Lines[movingBlock.OutputLineIndex].Y1 = movingBlock.OutputY;
            }
        }

        private void CancelDrawLine()
        {
            if (CurrentDrawingLineIndex != -1)
            {
                flowchartVM.Lines.RemoveAt(CurrentDrawingLineIndex);
                CurrentDrawingLineIndex = -1;
                flowchartVM.Blocks[CurrentBlockIndex].viewModel.DrawingLine = false;
                flowchartVM.Blocks[CurrentBlockIndex].viewModel.OutputLineIndex = -1;
            }
        }
        private void DrawLine(double x2, double y2, int currentIdx)
        {
            flowchartVM.Lines[currentIdx].X2 = x2;
            flowchartVM.Lines[currentIdx].Y2 = y2;
        }
        private void DrawLineEnd(int EndBlockIndex)
        {
            var X2 = flowchartVM.Blocks[EndBlockIndex].viewModel.InputX;
            var Y2 = flowchartVM.Blocks[EndBlockIndex].viewModel.InputY;
            flowchartVM.DrawLine(X2, Y2, CurrentDrawingLineIndex);
            flowchartVM.Blocks[CurrentBlockIndex].viewModel.DrawingLine = false; 
            flowchartVM.Blocks[EndBlockIndex].viewModel.InputLineIndex = CurrentDrawingLineIndex;
            CurrentDrawingLineIndex = -1;
        }

        private void DeleteLine(int deleteLineIndex)
        {
            foreach (var block in flowchartVM.Blocks)
            {
                if(block.viewModel.InputLineIndex == deleteLineIndex)
                {
                    block.viewModel.InputLineIndex = -1;
                }
                if (block.viewModel.OutputLineIndex == deleteLineIndex)
                {
                    block.viewModel.OutputLineIndex = -1;
                }
            }
            flowchartVM.Lines.RemoveAt(deleteLineIndex);
        }
        private void RemoveLineHighLight()
        {
            if (selectedLineHighLight != null)
            {
                var lineHighLightIndex = flowchartVM.Lines.IndexOf(selectedLineHighLight);
                flowchartVM.Lines.RemoveAt(lineHighLightIndex);
                selectedLineHighLight = null;
            }
        }
        private void AddLineHighLight(Line selectedLine)
        {
            if(selectedLineHighLight == null)
            {
                selectedLineHighLight = new Line()
                {
                    X1 = selectedLine.X1,
                    X2 = selectedLine.X2,
                    Y1 = selectedLine.Y1,
                    Y2 = selectedLine.Y2,
                    Stroke = Brushes.OrangeRed,
                    StrokeThickness = 7,
                    Opacity = 0.5,
                    IsHitTestVisible = false//避免點擊到該效果
                };
                flowchartVM.Lines.Add(selectedLineHighLight);
            }
            
        }
    }
}
