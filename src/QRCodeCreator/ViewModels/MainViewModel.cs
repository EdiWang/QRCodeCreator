using System;

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace QRCodeCreator.ViewModels
{
    public class MainViewModel : QRCodeGeneratorViewModelBase
    {
        public MainViewModel()
        {
            SourceText = "https://edi.wang";
            CommandGetQRCode = new RelayCommand(async () => await GetQrCode(), () => !string.IsNullOrEmpty(SourceText));
        }
    }
}
