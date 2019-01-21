using System;
using System.Collections.Generic;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using QRCodeCreator.ViewModels;

using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using QRCodeCreator.Printing;

namespace QRCodeCreator.Views
{
    public sealed partial class TotpPage : Page
    {
        private TotpViewModel ViewModel => DataContext as TotpViewModel;

        private PhotosPrintHelper _printHelper;

        DataTransferManager _dtm = DataTransferManager.GetForCurrentView();

        private StorageFile _tempExportFile;

        public TotpPage()
        {
            InitializeComponent();
            _dtm.DataRequested += DtmOnDataRequested;
        }

        private async void DtmOnDataRequested(DataTransferManager sender, DataRequestedEventArgs args)
        {
            try
            {
                DataPackage requestData = args.Request.Data;
                requestData.Properties.Title = "My TOTP QR Code";
                requestData.Properties.Description = "by Simple QR Code Creator";

                List<IStorageItem> imageItems = new List<IStorageItem> { _tempExportFile };
                requestData.SetStorageItems(imageItems);

                RandomAccessStreamReference imageStreamRef = RandomAccessStreamReference.CreateFromFile(_tempExportFile);
                requestData.Properties.Thumbnail = imageStreamRef;
                requestData.SetBitmap(imageStreamRef);
            }
            catch (Exception ex)
            {
                await new MessageDialog(ex.Message, "ERROR").ShowAsync();
            }
        }

        private async void BtnShare_OnClick(object sender, RoutedEventArgs e)
        {
            StorageFile tempFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("QRCode_Temp.png", CreationCollisionOption.ReplaceExisting);
            _tempExportFile = await ViewModel.SaveToStorageFile(tempFile);
            DataTransferManager.ShowShareUI();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _printHelper = new PhotosPrintHelper(this);
            _printHelper.RegisterForPrinting();
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            base.OnNavigatedFrom(e);
            _printHelper.UnregisterForPrinting();
        }

        private async void BtnPrint_OnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                _printHelper.RBitmap = ViewModel.SaveToBitmap();
                await _printHelper.ShowPrintUIAsync();
            }
            catch (Exception ex)
            {
                var msg = $"{ex.Message}";
                MessageDialog dig = new MessageDialog(msg, "ERROR");
                await dig.ShowAsync();
            }
        }

        private async void BtnPickColor_OnClick(object sender, RoutedEventArgs e)
        {
            await DigForegroundColorPicker.ShowAsync();
        }

        private async void BtnPickBackgroundColor_OnClick(object sender, RoutedEventArgs e)
        {
            await DigBackgroundColorPicker.ShowAsync();
        }

        private async void BtnCopy_OnClick(object sender, RoutedEventArgs e)
        {
            StorageFile tempFile = await ApplicationData.Current.TemporaryFolder.CreateFileAsync("QRCode_Copy_Temp.png", CreationCollisionOption.ReplaceExisting);
            _tempExportFile = await ViewModel.SaveToStorageFile(tempFile);
            RandomAccessStreamReference imageStreamRef = RandomAccessStreamReference.CreateFromFile(_tempExportFile);
            var dp = new DataPackage();
            dp.SetBitmap(imageStreamRef);
            Clipboard.SetContent(dp);
        }
    }
}
