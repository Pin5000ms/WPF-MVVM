using ModernDesign.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModernDesign.MVVM.ViewModel
{
    class MainViewModel : ObservableObject
    {

        public RelayCommand HomeViewCommand { get; set; }
        public RelayCommand DiscoveryViewCommand { get; set; }

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

        public HomeViewModel HomeVM { get; set; }
        public DiscoveryViewModel DiscoveryVM { get; set; }
        public MainViewModel()
        {
            HomeVM = new HomeViewModel();
            DiscoveryVM = new DiscoveryViewModel();

            HomeViewCommand = new RelayCommand(o => { CurrentView = HomeVM; });
            DiscoveryViewCommand = new RelayCommand(o => { CurrentView = DiscoveryVM; });

            CurrentView = HomeVM;
        }
    }
}
