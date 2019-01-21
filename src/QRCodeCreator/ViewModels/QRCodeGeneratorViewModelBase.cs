using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Provider;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Xaml.Media.Imaging;
using Edi.UWP.Helpers.Extensions;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using QRCodeCreator.Helpers;
using QRCodeCreator.QRCoderCore;

namespace QRCodeCreator.ViewModels
{
    public class QRCodeGeneratorViewModelBase : ViewModelBase
    {
        private WriteableBitmap _bitmap;
        private string _selectedEcc;
        private Color _foregroundColor;
        private Color _backgroundColor;
        private string _sourceText;

        public QRCodeGeneratorViewModelBase()
        {
            SelectedEcc = "M";
            ForegroundColor = Colors.Black;
            BackgroundColor = Colors.White;
            CommandSave = new RelayCommand(async () => await SaveToPic());
        }

        public Color ForegroundColor
        {
            get => _foregroundColor;
            set { _foregroundColor = value; RaisePropertyChanged(); }
        }

        public Color BackgroundColor
        {
            get => _backgroundColor;
            set { _backgroundColor = value; RaisePropertyChanged(); }
        }

        public WriteableBitmap Bitmap
        {
            get => _bitmap;
            set
            {
                _bitmap = value;
                RaisePropertyChanged();
            }
        }

        public string SourceText
        {
            get => _sourceText;
            set
            {
                _sourceText = value;
                RaisePropertyChanged();
            }
        }

        public RelayCommand CommandGetQRCode { get; set; }

        public RelayCommand CommandSave { get; set; }

        public List<string> EccModes
        {
            get
            {
                return Enum.GetValues(typeof(QRCodeGenerator.ECCLevel)).Cast<Enum>().Select(x => x.ToString()).ToList();
            }
        }

        public string SelectedEcc
        {
            get => _selectedEcc;
            set
            {
                _selectedEcc = value;
                RaisePropertyChanged();
            }
        }

        public async Task SaveToPic()
        {
            try
            {
                var fileName = $"QRCODE_{DateTime.Now:yyyy-MM-dd-HHmmss}";
                var result = await Bitmap.SaveToPngImage(PickerLocationId.PicturesLibrary, fileName);
                if (result != FileUpdateStatus.Complete)
                {
                    var dig = new MessageDialog($"{result}", "FAIL");
                    await dig.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                var dig = new MessageDialog($"{ex.Message}", "FAIL");
                await dig.ShowAsync();
            }
        }

        public WriteableBitmap SaveToBitmap()
        {
            return Bitmap;
        }

        public async Task<StorageFile> SaveToStorageFile(StorageFile tempFile)
        {
            await Bitmap.SaveStorageFile(ImageFormat.Png, tempFile);
            return tempFile;
        }

        public virtual async Task GetQrCode()
        {
            var level = SelectedEcc;
            var eccLevel = (QRCodeGenerator.ECCLevel)(level == "L" ? 0 : level == "M" ? 1 : level == "Q" ? 2 : 3);
            var qrGenerator = new QRCodeGenerator();
            var qrCodeData = qrGenerator.CreateQrCode(SourceText, eccLevel);
            var qrCode = new BitmapByteQRCode(qrCodeData);
            var qrCodeImage = qrCode.GetGraphic(20, this.ForegroundColor, this.BackgroundColor);

            using (var stream = new InMemoryRandomAccessStream())
            {
                using (var writer = new DataWriter(stream.GetOutputStreamAt(0)))
                {
                    writer.WriteBytes(qrCodeImage);
                    await writer.StoreAsync();
                }
                Bitmap = new WriteableBitmap(1024, 1024);
                await Bitmap.SetSourceAsync(stream);
            }
        }
    }
}
