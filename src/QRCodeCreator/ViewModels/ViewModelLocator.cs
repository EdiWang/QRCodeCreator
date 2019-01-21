using System;
using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;
using QRCodeCreator.Services;
using QRCodeCreator.Views;
using ZXing.Mobile;

namespace QRCodeCreator.ViewModels
{
    [Windows.UI.Xaml.Data.Bindable]
    public class ViewModelLocator
    {
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register(() => new NavigationServiceEx());
            SimpleIoc.Default.Register<ShellViewModel>();
            Register<MainViewModel, MainPage>();
            Register<TotpViewModel, TotpPage>();
            Register<WiFiViewModel, WiFiPage>();
            Register<QRScannerViewModel, QRScannerPage>();
            Register<SettingsViewModel, SettingsPage>();
            Register<ScanPageViewModel, ScanPage>();
        }

        public SettingsViewModel SettingsViewModel => ServiceLocator.Current.GetInstance<SettingsViewModel>();

        public QRScannerViewModel QRScannerViewModel => ServiceLocator.Current.GetInstance<QRScannerViewModel>();

        public WiFiViewModel WiFiViewModel => ServiceLocator.Current.GetInstance<WiFiViewModel>();

        public TotpViewModel TotpViewModel => ServiceLocator.Current.GetInstance<TotpViewModel>();

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        public ShellViewModel ShellViewModel => ServiceLocator.Current.GetInstance<ShellViewModel>();

        public NavigationServiceEx NavigationService => ServiceLocator.Current.GetInstance<NavigationServiceEx>();

        public void Register<VM, V>()
            where VM : class
        {
            SimpleIoc.Default.Register<VM>();

            NavigationService.Configure(typeof(VM).FullName, typeof(V));
        }
    }
}
