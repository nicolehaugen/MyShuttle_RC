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
    public sealed partial class MainHub : Page
    {
        private TimeSpan TopDriversLoadDelay
        {
            get { return TimeSpan.FromMilliseconds(rand.Next(2000, 3000)); }
        }
        private TimeSpan VehiclesLoadDelay
        {
            get { return TimeSpan.FromMilliseconds(rand.Next(4000, 5000)); }
        }
        private TimeSpan ServicesLoadDelay
        {
            get { return TimeSpan.FromMilliseconds(rand.Next(3000, 4000)); }
        }
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        bool isLoadedHub;

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

        public MainHub()
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
                DriversHubSection_Loaded(sender, null);
                Vehicles_Loaded(sender, null);
                Services_Loaded(sender, null);
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
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            if (e.PageState == null)
            {
                MainHubHub.ScrollToSection(MainHubHub.Sections[0]);
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

            Frame.CacheSize = 0;
            Frame.CacheSize = 10;

            TopDrivers.Header = "top drivers";
            Vehicles.Header = "vehicles";
            if (isLoadedHub)
            {
                DriversHubSection_Loaded(this, null);
                Vehicles_Loaded(this, null);
                Services_Loaded(this, null);
            }

        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion


        private void imgTop1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(DriversHub), "Guthrie");
            }
        }

        private void imgTop3_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(DriversHub), "Hanselman");
            }
        }

        private void Image_Tapped(object sender, TappedRoutedEventArgs e)
        {
            if (this.Frame != null)
            {
                this.Frame.Navigate(typeof(MillesHub));
            }
        }

        private void DriversHubSection_Loaded(object sender, RoutedEventArgs e)
        {
            isLoadedHub = true;
            var imgTop1 = ControlTreeHelper.FindNameInVisualTree<Image>(MainHubHub, "imgTop1");
            var imgTop2 = ControlTreeHelper.FindNameInVisualTree<Image>(MainHubHub, "imgTop2");
            var imgTop3 = ControlTreeHelper.FindNameInVisualTree<Image>(MainHubHub, "imgTop3");
            var imgTop4 = ControlTreeHelper.FindNameInVisualTree<Image>(MainHubHub, "imgTop4");
            var imgTop5 = ControlTreeHelper.FindNameInVisualTree<Image>(MainHubHub, "imgTop5");

            if (Window.Current.Bounds.Width < 1920)
            {
                imgTop1.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/MainHub/top1.png", UriKind.Absolute));
                imgTop2.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/MainHub/top2.png", UriKind.Absolute));
                imgTop3.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/MainHub/top3.png", UriKind.Absolute));
                imgTop4.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/MainHub/top4.png", UriKind.Absolute));
                imgTop5.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/MainHub/top5.png", UriKind.Absolute));
            }
            else
            {
                imgTop1.Source = new BitmapImage(new Uri("ms-appx:///Assets/1920/MainHub/hd-top1.png", UriKind.Absolute));
                imgTop2.Source = new BitmapImage(new Uri("ms-appx:///Assets/1920/MainHub/hd-top2.png", UriKind.Absolute));
                imgTop3.Source = new BitmapImage(new Uri("ms-appx:///Assets/1920/MainHub/hd-top3.png", UriKind.Absolute));
                imgTop4.Source = new BitmapImage(new Uri("ms-appx:///Assets/1920/MainHub/hd-top4.png", UriKind.Absolute));
                imgTop5.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/MainHub/top5.png", UriKind.Absolute));
            }
        }
        private void Vehicles_Loaded(object sender, RoutedEventArgs e)
        {
            var imgSta1 = ControlTreeHelper.FindNameInVisualTree<Image>(MainHubHub, "imgSta1");
            var imgSta2 = ControlTreeHelper.FindNameInVisualTree<Image>(MainHubHub, "imgSta2");
            var imgSta3 = ControlTreeHelper.FindNameInVisualTree<Image>(MainHubHub, "imgSta3");
            var imgSta4 = ControlTreeHelper.FindNameInVisualTree<Image>(MainHubHub, "imgSta4");

            if (Window.Current.Bounds.Width < 1920)
            {
                imgSta1.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/MainHub/sta1.png", UriKind.Absolute));
                imgSta2.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/MainHub/sta2.png", UriKind.Absolute));
                imgSta3.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/MainHub/sta3.png", UriKind.Absolute));
                imgSta4.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/MainHub/sta4.png", UriKind.Absolute));
            }
            else
            {
                imgSta1.Source = new BitmapImage(new Uri("ms-appx:///Assets/1920/MainHub/hd-sta1.png", UriKind.Absolute));
                imgSta2.Source = new BitmapImage(new Uri("ms-appx:///Assets/1920/MainHub/hd-sta2.png", UriKind.Absolute));
                imgSta3.Source = new BitmapImage(new Uri("ms-appx:///Assets/1920/MainHub/hd-sta3.png", UriKind.Absolute));
                imgSta4.Source = new BitmapImage(new Uri("ms-appx:///Assets/1920/MainHub/hd-sta4.png", UriKind.Absolute));
            }
        }

        private void Services_Loaded(object sender, RoutedEventArgs e)
        {
            var imgCha1 = ControlTreeHelper.FindNameInVisualTree<Image>(MainHubHub, "imgCha1");
            var imgCha2 = ControlTreeHelper.FindNameInVisualTree<Image>(MainHubHub, "imgCha2");
            var imgCha1Emp = ControlTreeHelper.FindNameInVisualTree<Image>(MainHubHub, "imgCha1Emp");
            var imgCha2Emp = ControlTreeHelper.FindNameInVisualTree<Image>(MainHubHub, "imgCha2Emp");

            if (Window.Current.Bounds.Width < 1920)
            {

                imgCha1.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/MainHub/cha1.png", UriKind.Absolute));
                imgCha2.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/MainHub/cha2.png", UriKind.Absolute));
                imgCha1Emp.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/MainHub/cha1-emp.png", UriKind.Absolute));
                imgCha2Emp.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/MainHub/cha2-emp.png", UriKind.Absolute));
            }
            else
            {
                imgCha1.Source = new BitmapImage(new Uri("ms-appx:///Assets/1920/MainHub/hd-cha1.png", UriKind.Absolute));
                imgCha2.Source = new BitmapImage(new Uri("ms-appx:///Assets/1920/MainHub/hd-cha2.png", UriKind.Absolute));
                imgCha1Emp.Source = new BitmapImage(new Uri("ms-appx:///Assets/1920/MainHub/hd-cha1-emp.png", UriKind.Absolute));
                imgCha2Emp.Source = new BitmapImage(new Uri("ms-appx:///Assets/1920/MainHub/hd-cha2-emp.png", UriKind.Absolute));
            }
        }

        private Random rand = new Random((int)DateTime.Now.Ticks);

        private async void TopDrivers_OnLoaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(TopDriversLoadDelay);
            TopDrivers.Header = "top drivers (212)";
            ((Storyboard)((FrameworkElement)sender).Resources["Load1StoryBoard"]).Begin();
        }

        private async void Vehicles_OnLoaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(VehiclesLoadDelay);
            Vehicles.Header = "vehicles (190)";
            ((Storyboard)((FrameworkElement)sender).Resources["LoadVehicle1StoryBoard"]).Begin();
        }

        private async void Services_OnLoaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(ServicesLoadDelay);
            ((Storyboard)((FrameworkElement)sender).Resources["Cha1Storyboard"]).Begin();
        }
    }
}
