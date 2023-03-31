using ModernDesign.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernDesign
{
    class MainViewModel : ObservableObject
    {

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand DiscoveryViewCommand { get; set; }

        public RelayCommand SudokuViewCommand { get; set; }

        private object _currentView;
        public object CurrentView 
        {
            get { return _currentView; }
            set 
            { 
                _currentView = value;
                OnPropertyChanged();
            } 
        }

        public HomeView HomeView { get; set; }
        public WebCamViewModel WebCamVM { get; set; }

        public SudokuView SudokuView { get; set; }
        public MainViewModel()
        {
            var HomeVM = new HomeViewModel();
            HomeView = new HomeView(HomeVM);

            WebCamVM = new WebCamViewModel();

            var SudokuVM = new SudokuViewModel();
            SudokuView = new SudokuView(SudokuVM);

            HomeViewCommand = new RelayCommand(o => { CurrentView = HomeView; });
            DiscoveryViewCommand = new RelayCommand(o => { CurrentView = WebCamVM; });
            SudokuViewCommand = new RelayCommand(o => { CurrentView = SudokuView; });

            CurrentView = WebCamVM;

        }

        
    }


}
