using ModernDesign.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace ModernDesign
{
    public class FlowChartViewModel : ObservableObject
    {
        public FlowChartViewModel()
        {
            var a = new SingleBlockView();
            a.viewModel.BackGround = new SolidColorBrush(Colors.LightPink);
            a.viewModel.BlockAction = new RedBlockAction();
            var b = new SingleBlockView();
            b.viewModel.BackGround = new SolidColorBrush(Colors.LightGreen);
            b.viewModel.BlockAction = new GreenBlockAction();
            var c = new SingleBlockView();
            c.viewModel.BackGround = new SolidColorBrush(Colors.LightBlue);
            c.viewModel.BlockAction = new BlueBlockAction();

            Blocks.Add(a);
            Blocks.Add(b);
            Blocks.Add(c);

            RunFlowThread = new Thread(StartFlow);
            StartCommand = new RelayCommand(o => 
            { 
                if(RunFlowThread.ThreadState == ThreadState.Unstarted || 
                RunFlowThread.ThreadState == ThreadState.Stopped ||
                RunFlowThread.ThreadState == ThreadState.Aborted)
                {
                    RunFlowThread = new Thread(StartFlow);
                    RunFlowThread.Start();
                }
            });

            StopCommand = new RelayCommand(o =>
            {
                
                if (RunFlowThread.ThreadState == ThreadState.Running ||
                RunFlowThread.ThreadState == ThreadState.WaitSleepJoin)
                {
                    RunFlowThread.Abort();
                }
                else
                {
                    Console.WriteLine(RunFlowThread.ThreadState.ToString());
                }
                    
            });
        }
        Thread RunFlowThread;
        public RelayCommand StartCommand { get; set; }
        public RelayCommand StopCommand { get; set; }
        public void StartFlow()
        {
            Blocks[0].viewModel.BlockAction.InputAction?.Invoke(new DataFlow());
        }

        private ObservableCollection<SingleBlockView> _blocks = new ObservableCollection<SingleBlockView>();
        public ObservableCollection<SingleBlockView> Blocks
        {
            get
            {
                return _blocks;
            }
            set
            {
                _blocks = value;
                OnPropertyChanged();
            }
        }

        private ObservableCollection<LineWithId> _lines = new ObservableCollection<LineWithId>();
        public ObservableCollection<LineWithId> Lines
        {
            get
            {
                return _lines;
            }
            set
            {
                _lines = value;
                OnPropertyChanged();
            }
        }
    }
}