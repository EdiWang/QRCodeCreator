using System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using QRCodeCreator.ViewModels;

using Windows.UI.Xaml.Controls;
using ZXing.Mobile;

namespace QRCodeCreator.Views
{
    public sealed partial class QRScannerPage : Page
    {
        private QRScannerViewModel ViewModel => DataContext as QRScannerViewModel;

        readonly MobileBarcodeScanner _scanner;

        public QRScannerPage()
        {
            InitializeComponent();
            _scanner = new MobileBarcodeScanner(this.Dispatcher)
            {
                Dispatcher = this.Dispatcher
            };
        }

        private void BtnScan_OnClick(object sender, RoutedEventArgs e)
        {
            _scanner.UseCustomOverlay = false;
            _scanner.TopText = "Hold camera up to QR Code";
            _scanner.BottomText = "Camera will automatically scan QR Code\r\n\r\nPress the 'Back' button to Cancel";

            _scanner.Scan().ContinueWith(async t =>
            {
                if (t.Result != null)
                {
                    string msg = "";
                    msg = !string.IsNullOrEmpty(t.Result?.Text) ? t.Result.Text : "Scanning Canceled!";

                    await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
                    {
                        ViewModel.ResultText += msg + Environment.NewLine;
                    });
                }
            });
        }
    }
}
