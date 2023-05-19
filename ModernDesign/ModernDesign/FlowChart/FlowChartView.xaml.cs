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


        private LineWithId selectedLineHighLight = null;

        public FlowChartView(FlowChartViewModel _viewModel)
        {
            InitializeComponent();
            this.flowchartVM = _viewModel;
            DataContext = flowchartVM;
        }

        int CurrentBlockIndex = -1;
        int CurrentDrawingLineId = -1;

        int startBlockIndex = -1;

        int LineIndex = 0;
        private void BlockPreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (CurrentBlockIndex != -1)
            {
                
                if (CurrentDrawingLineId != -1)
                {
                    var endBlock = sender as SingleBlockView;
                    var endBlockIndex = flowchartVM.Blocks.IndexOf(endBlock);

                    //取消連接
                    if (startBlockIndex == endBlockIndex || endBlock.OutputButtonClicked)
                    {
                        CancelDrawLine();
                        endBlock.OutputButtonClicked = false;
                        endBlock.viewModel.DrawingLine = false;
                    }
                    else//連接完成，更新末端點
                    {
                        var index = FindLineIndexById(CurrentDrawingLineId);
                        if (index != -1)
                        {
                            flowchartVM.Lines[index].PrevBlockIndex = startBlockIndex;
                            flowchartVM.Lines[index].NextBlockIndex = endBlockIndex;
                        }
                        DrawLineEnd(endBlockIndex);
                        endBlock.InputButtonClicked = false;
                        RegistAction(startBlockIndex, endBlockIndex);
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
            if (block.viewModel.DrawingLine && CurrentDrawingLineId == -1)
            {
                var X1 = flowchartVM.Blocks[CurrentBlockIndex].viewModel.OutputX;
                var Y1 = flowchartVM.Blocks[CurrentBlockIndex].viewModel.OutputY;
                var l = new LineWithId();
                l.SetStart(X1, Y1);
                flowchartVM.Lines.Add(l);
                l.ID = LineIndex;
                LineIndex++;
                CurrentDrawingLineId = l.ID;
                flowchartVM.Blocks[CurrentBlockIndex].viewModel.OutputLineIds.Add(CurrentDrawingLineId);
                //出發的block index
                startBlockIndex = CurrentBlockIndex;
            }
        }

        private void CanvasMouseMove(object sender, MouseEventArgs e)
        {
            Point mousePoint = e.GetPosition(FlowChartCanvas);
            if (CurrentDrawingLineId != -1 && CurrentBlockIndex != -1 && flowchartVM.Blocks[CurrentBlockIndex].viewModel.DrawingLine)
            {
                //-10避免擋到InputButton
                DrawLine(mousePoint.X - 10, mousePoint.Y - 10, CurrentDrawingLineId);
            }
            if (CurrentBlockIndex != -1 && e.LeftButton == MouseButtonState.Pressed)
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
                var parent = (e.OriginalSource as Line).Parent;
                var selectedLine = parent as LineWithId;
                AddLineHighLight(selectedLine);
            }

            bool isClickBlock = e.OriginalSource is SingleBlockView;
            //連接失敗
            if (!isClickBlock && CurrentBlockIndex != -1 && CurrentDrawingLineId != -1)
            {
                flowchartVM.Blocks[CurrentBlockIndex].viewModel.OutputLineIds.Remove(CurrentDrawingLineId);
                CancelDrawLine();
            }

        }
        private void CanvasMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void CanvasMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            bool isClickLine = e.OriginalSource is Line;
            if (isClickLine)
            {
                var parent = (e.OriginalSource as Line).Parent;
                var selectedLine = parent as LineWithId;


                AddLineHighLight(selectedLine);


                // 建立右鍵菜單
                var contextMenu = new ContextMenu();
                var menuItem = new MenuItem();
                menuItem.Header = "Delete";
                menuItem.Click += (s, args) =>
                {
                    // 從ViewModel中移除該Line的HighLight
                    RemoveLineHighLight();

                    // 從ViewModel中移除該Line
                    DeleteLine(selectedLine.ID);
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
            var inputLine = flowchartVM.Lines.FirstOrDefault(l => l.ID == movingBlock.InputLineId);
            if (movingBlock.InputLineId != -1 && inputLine != null)
            {
                var inputLineindex = flowchartVM.Lines.IndexOf(inputLine);
                flowchartVM.Lines[inputLineindex].SetEnd(movingBlock.InputX, movingBlock.InputY);
            }
            if (movingBlock.OutputLineIds.Count != 0)
            {
                foreach (var Id in movingBlock.OutputLineIds)
                {
                    var index = FindLineIndexById(Id);
                    if (index != -1)
                        flowchartVM.Lines[index].SetStart(movingBlock.OutputX, movingBlock.OutputY);
                }

            }
        }

        private void CancelDrawLine()
        {
            if (CurrentDrawingLineId != -1)
            {
                var index = FindLineIndexById(CurrentDrawingLineId);
                if (index != -1)
                    flowchartVM.Lines.RemoveAt(index);
                CurrentDrawingLineId = -1;
                flowchartVM.Blocks[CurrentBlockIndex].viewModel.DrawingLine = false;
                flowchartVM.Blocks[CurrentBlockIndex].viewModel.OutputLineIds.Remove(CurrentDrawingLineId);
            }
        }
        private void DrawLine(double x2, double y2, int Id)
        {
            var index = FindLineIndexById(Id);
            if (index != -1)
                flowchartVM.Lines[index].SetEnd(x2, y2);
        }
        private void DrawLineEnd(int EndBlockIndex)
        {
            var X2 = flowchartVM.Blocks[EndBlockIndex].viewModel.InputX;
            var Y2 = flowchartVM.Blocks[EndBlockIndex].viewModel.InputY;
            DrawLine(X2, Y2, CurrentDrawingLineId);
            flowchartVM.Blocks[CurrentBlockIndex].viewModel.DrawingLine = false;
            flowchartVM.Blocks[EndBlockIndex].viewModel.InputLineId = CurrentDrawingLineId;
            CurrentDrawingLineId = -1;
        }
        private void DeleteLine(int deleteLineId)
        {
            foreach (var block in flowchartVM.Blocks)
            {
                if (block.viewModel.InputLineId == deleteLineId)
                {
                    block.viewModel.InputLineId = -1;
                }
                for (int i = 0; i < block.viewModel.OutputLineIds.Count; i++)
                {
                    if (block.viewModel.OutputLineIds[i] == deleteLineId)
                    {
                        block.viewModel.OutputLineIds.RemoveAt(i);
                    }
                }
            }
            var index = FindLineIndexById(deleteLineId);
            if (index != -1)
            {
                var PrevBlockId = flowchartVM.Lines[index].PrevBlockIndex;
                var NextBlockId = flowchartVM.Lines[index].NextBlockIndex;
                flowchartVM.Lines.RemoveAt(index);
                UnRegistAction(PrevBlockId, NextBlockId);
            }
                
        }
        private void RemoveLineHighLight()
        {
            if (selectedLineHighLight != null)
            {
                var lineHighLight = flowchartVM.Lines.FirstOrDefault(l => l.ID == 999);
                if (lineHighLight != null)
                {
                    var index = flowchartVM.Lines.IndexOf(lineHighLight);
                    flowchartVM.Lines.RemoveAt(index);
                    selectedLineHighLight = null;
                }

            }
        }
        private void AddLineHighLight(LineWithId selectedLine)
        {
            if (selectedLineHighLight == null)
            {
                selectedLineHighLight = new LineWithId();
                selectedLineHighLight.SetStartEnd(selectedLine.X1, selectedLine.Y1, selectedLine.X2, selectedLine.Y2);
                selectedLineHighLight.SetDefaultHighLightLine();
                selectedLineHighLight.ID = 999;
                flowchartVM.Lines.Add(selectedLineHighLight);
            }

        }

        private bool CheckLine(LineWithId line1, Line line2)
        {
            if (Math.Abs(line1.X1 - line2.X1) < 5 && Math.Abs(line1.Y1 - line2.Y1) < 5 &&
               Math.Abs(line1.X2 - line2.X2) < 5 && Math.Abs(line1.Y2 - line2.Y2) < 5)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private int FindLineIndexById(int Id)
        {
            int index = -1;
            var line = flowchartVM.Lines.FirstOrDefault(l => l.ID == Id);
            if (line != null)
            {
                index = flowchartVM.Lines.IndexOf(line);
            }
            return index;
        }


        private void RegistAction(int PrevBlockId, int NextBlockId)
        {
            flowchartVM.Blocks[PrevBlockId].viewModel.BlockAction.OutputAction += flowchartVM.Blocks[NextBlockId].viewModel.BlockAction.InputAction;
        }
        private void UnRegistAction(int PrevBlockId, int NextBlockId)
        {
            flowchartVM.Blocks[PrevBlockId].viewModel.BlockAction.OutputAction -= flowchartVM.Blocks[NextBlockId].viewModel.BlockAction.InputAction;
        }
    }
}