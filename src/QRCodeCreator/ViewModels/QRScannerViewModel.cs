using System;

using GalaSoft.MvvmLight;

namespace QRCodeCreator.ViewModels
{
    public class QRScannerViewModel : ViewModelBase
    {
        private string _resultText;

        public string ResultText
        {
            get => _resultText;
            set { _resultText = value; RaisePropertyChanged(); }
        }

        public QRScannerViewModel()
        {
        }
    }
}
