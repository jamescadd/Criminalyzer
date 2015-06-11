using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using PropertyChanged;

namespace Criminalyzer
{
    [ImplementPropertyChanged]
    class MainViewModel
    {
        public RelayCommand GetMugshotsCommand {  get { return new RelayCommand(OnGetMugshots); } }

        public ObservableCollection<Record> Records = new ObservableCollection<Record>();

        public MainViewModel()
        {

        }

        protected void OnGetMugshots()
        {

        }
    }
}
