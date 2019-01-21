using System;
using System.Threading.Tasks;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace QRCodeCreator.ViewModels
{
    public class TotpViewModel : QRCodeGeneratorViewModelBase
    {
        private string _totpKey;
        private int _totpLength;
        private string _issuer;
        private string _totpTitle;

        public string TotpTitle
        {
            get => _totpTitle;
            set { _totpTitle = value; RaisePropertyChanged(); }
        }

        public string TotpKey
        {
            get => _totpKey;
            set { _totpKey = value; RaisePropertyChanged(); }
        }

        public int TotpLength
        {
            get => _totpLength;
            set { _totpLength = value; RaisePropertyChanged(); }
        }

        public string Issuer
        {
            get => _issuer;
            set { _issuer = value; RaisePropertyChanged(); }
        }

        public TotpViewModel()
        {
            TotpLength = 6;
            Issuer = Environment.MachineName;
            CommandGetQRCode = new RelayCommand(async () => await GetQrCode());
        }

        public override Task GetQrCode()
        {
            SourceText = $"otpauth://totp/{Issuer}:{TotpTitle}?secret={TotpKey}&digits={TotpLength}&issuer={Issuer}";
            return base.GetQrCode();
        }
    }
}
