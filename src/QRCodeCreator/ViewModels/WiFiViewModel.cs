using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Command;
using QRCoder;

namespace QRCodeCreator.ViewModels
{
    public class WiFiViewModel : QRCodeGeneratorViewModelBase
    {
        private bool _isHiddenSsid;
        private string _password;
        private string _ssid;
        private PayloadGenerator.WiFi.Authentication _authenticationMode;
        private IEnumerable<PayloadGenerator.WiFi.Authentication> _authenticationModes;

        public string Ssid
        {
            get => _ssid;
            set { _ssid = value; RaisePropertyChanged(); }
        }

        public string Password
        {
            get => _password;
            set { _password = value; RaisePropertyChanged(); }
        }


        public IEnumerable<PayloadGenerator.WiFi.Authentication> AuthenticationModes
        {
            get => _authenticationModes;
            set { _authenticationModes = value; RaisePropertyChanged(); }
        }

        public PayloadGenerator.WiFi.Authentication AuthenticationMode
        {
            get => _authenticationMode;
            set { _authenticationMode = value; RaisePropertyChanged(); }
        }

        public bool IsHiddenSsid
        {
            get => _isHiddenSsid;
            set { _isHiddenSsid = value; RaisePropertyChanged(); }
        }

        public WiFiViewModel()
        {
            Ssid = "A-MSFT-GUEST";
            Password = "P@ssw0rd1";
            IsHiddenSsid = false;
            CommandGetQRCode = new RelayCommand(async () => await GetQrCode());

            AuthenticationModes = Enum.GetValues(typeof(PayloadGenerator.WiFi.Authentication)).Cast<PayloadGenerator.WiFi.Authentication>();
        }

        public override Task GetQrCode()
        {
            var str = new PayloadGenerator.WiFi(Ssid, Password, AuthenticationMode, IsHiddenSsid);
            SourceText = str.ToString();
            return base.GetQrCode();
        }
    }
}
