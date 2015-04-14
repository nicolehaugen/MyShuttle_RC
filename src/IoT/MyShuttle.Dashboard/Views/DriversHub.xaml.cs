using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Animation;
using MyShuttle.Dashboard.Common;
using MyShuttle.Dashboard.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Hub Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=321224

namespace MyShuttle.Dashboard.Views
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class DriversHub : Page
    {
        private TimeSpan RatingsLoadDelay
        {
            get { return TimeSpan.FromMilliseconds(rand.Next(3500, 5000)); }
        }
        private TimeSpan ValuationsLoadDelay
        {
            get { return TimeSpan.FromMilliseconds(rand.Next(3500, 5000)); }
        }
        private TimeSpan DriverCardLoadDelay
        {
            get { return TimeSpan.FromMilliseconds(rand.Next(3000, 5000)); }
        }
        private TimeSpan GeneralStatisticsLoadDelay
        {
            get { return TimeSpan.FromMilliseconds(rand.Next(3500, 5000)); }
        }

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private bool isLoadedHub = false;

        public bool isFullHd = false;

        public string Driver = null;

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public DriversHub()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.NavigationCacheMode = NavigationCacheMode.Disabled;

            this.SizeChanged += this.Page_SizeChanged;
        }

        public void Page_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (isLoadedHub)
            {
                driverCardHub_Loaded(sender, null);
                GeneralHub_Loaded(sender, null);
                RatingsHub_Loaded(sender, null);
                ValorationHub_Loaded(sender, null);
            }
        }


        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (e.NavigationParameter != null)
            {
                Driver = (string) e.NavigationParameter;
            }
            else
            {
                Driver = "Hanselman";
            }
            if (e.PageState == null)
            {
                await Task.Delay(100);
                DriverHub.ScrollToSection(DriverHub.Sections[0]);
            }
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);

            Frame.CacheSize = 1;
            Frame.CacheSize = 10;
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void imgCar_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(CarsHub), Driver);
            }
        }

        private void driverCardHub_Loaded(object sender, RoutedEventArgs e)
        {
            isLoadedHub = true;
            var imgDriver = ControlTreeHelper.FindNameInVisualTree<Image>(DriverHub, "imgDriver");
            var imgCar = ControlTreeHelper.FindNameInVisualTree<Image>(DriverHub, "imgCar");
            var stcDriverCard = ControlTreeHelper.FindNameInVisualTree<StackPanel>(DriverHub, "stcDriverCard");

            // 1280x768 Resolution
            if (Window.Current.Bounds.Width < 1920)
            {
                imgDriver.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Driver/" + Driver + "/driver-file.png", UriKind.Absolute));
                imgCar.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Driver/" + Driver + "/car-file.png", UriKind.Absolute));
                stcDriverCard.Orientation = Orientation.Horizontal;
                imgCar.Margin = new Thickness(50, 0, 0, 0);
            }
            // 1920x1080 Resolution
            else
            {
                imgDriver.Source = new BitmapImage(new Uri("ms-appx:///Assets/1920/Driver/" + Driver + "/hd-driver-file.png", UriKind.Absolute));
                imgCar.Source = new BitmapImage(new Uri("ms-appx:///Assets/1920/Driver/" + Driver + "/hd-car-file.png", UriKind.Absolute));
                stcDriverCard.Orientation = Orientation.Vertical;
                imgCar.Margin = new Thickness(0, 10, 0, 0);
            }
        }

        private void GeneralHub_Loaded(object sender, RoutedEventArgs e)
        {
            var imgSta1 = ControlTreeHelper.FindNameInVisualTree<Image>(DriverHub, "imgSta1");
            var imgSta2 = ControlTreeHelper.FindNameInVisualTree<Image>(DriverHub, "imgSta2");
            var imgSta3 = ControlTreeHelper.FindNameInVisualTree<Image>(DriverHub, "imgSta3");

            // 1280x768 Resolution
            if (Window.Current.Bounds.Width < 1920)
            {
                isFullHd = false;

                imgSta1.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Driver/" + Driver + "/sta1.png", UriKind.Absolute));
                imgSta2.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Driver/" + Driver + "/sta2.png", UriKind.Absolute));
                imgSta3.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Driver/" + Driver + "/sta3.png", UriKind.Absolute));
            }
            // 1920x1080 Resolution
            else
            {
                isFullHd = true;

                imgSta1.Source = new BitmapImage(new Uri("ms-appx:///Assets/1920/Driver/" + Driver + "/hd-sta1.png", UriKind.Absolute));
                imgSta2.Source = new BitmapImage(new Uri("ms-appx:///Assets/1920/Driver/" + Driver + "/hd-sta2.png", UriKind.Absolute));
                imgSta3.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Driver/" + Driver + "/sta3.png", UriKind.Absolute));
            }
        }

        private void RatingsHub_Loaded(object sender, RoutedEventArgs e)
        {
            var imgCha1 = ControlTreeHelper.FindNameInVisualTree<Image>(DriverHub, "imgCha1");
            var imgCha1D = ControlTreeHelper.FindNameInVisualTree<Image>(DriverHub, "imgCha1D");

            // 1280x768 Resolution
            if (Window.Current.Bounds.Width < 1920)
            {
                isFullHd = false;

                imgCha1.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Driver/chaA.png", UriKind.Absolute));
                imgCha1D.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Driver/" + Driver + "/chaB.png", UriKind.Absolute));
                imgCha1.Margin = new Thickness(0, 0, 0, 0);
                imgCha1D.Margin = new Thickness(0, 0, 0, 0);
            }
            // 1920x1080 Resolution
            else
            {
                isFullHd = true;

                imgCha1.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Driver/chaA.png", UriKind.Absolute));
                imgCha1D.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Driver/" + Driver + "/chaB.png", UriKind.Absolute));
                imgCha1.Margin = new Thickness(0, 100, 0, 0);
                imgCha1D.Margin = new Thickness(0, 100, 0, 0);
            }
        }

        private async void ValorationHub_Loaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(150);
            var imgCha2 = ControlTreeHelper.FindNameInVisualTree<Image>(DriverHub, "imgCha2");
            var imgCha2D = ControlTreeHelper.FindNameInVisualTree<Image>(DriverHub, "imgCha2D");

            // 1280x768 Resolution
            if (Window.Current.Bounds.Width < 1920)
            {
                isFullHd = false;

                imgCha2.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Driver/cha2A.png", UriKind.Absolute));
                imgCha2D.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Driver/cha2B.png", UriKind.Absolute));
                imgCha2.Margin = new Thickness(0, 0, 0, 0);
                imgCha2D.Margin = new Thickness(0, 0, 0, 0);
            }
            // 1920x1080 Resolution
            else
            {
                isFullHd = true;

                imgCha2.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Driver/cha2A.png", UriKind.Absolute));
                imgCha2D.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Driver/cha2B.png", UriKind.Absolute));
                imgCha2.Margin = new Thickness(0, 100, 0, 0);
                imgCha2D.Margin = new Thickness(0, 100, 0, 0);
            }
        }

        private Random rand = new Random((int)DateTime.Now.Ticks);

        private async void Ratings_OnLoaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(RatingsLoadDelay);
            ((Storyboard)((FrameworkElement)sender).Resources["RatingsLoadStoryBoard"]).Begin();
        }

        private async void Valoration_OnLoaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(ValuationsLoadDelay);
            ((Storyboard)((FrameworkElement)sender).Resources["ValorationLoadStoryBoard"]).Begin();
        }

        private async void driverCarHub_OnLoaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(DriverCardLoadDelay);
            ((Storyboard)((FrameworkElement)sender).Resources["DriverCardLoadStoryBoard"]).Begin();
        }

        private async void GeneralHub_OnLoaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(GeneralStatisticsLoadDelay);
            ((Storyboard)((FrameworkElement)sender).Resources["GeneralLoadStoryBoard"]).Begin();
        }
    }
}
