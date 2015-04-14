using System.Threading;
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
    public sealed partial class MillesHub : Page
    {
        private TimeSpan YearChartLoadDelay
        {
            get { return TimeSpan.FromMilliseconds(rand.Next(2200, 2800)); }
        }
        private TimeSpan MonthChartLoadDelay
        {
            get { return TimeSpan.FromMilliseconds(rand.Next(2200, 2800)); }
        }
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private bool isLoadedHub = false;

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

        public MillesHub()
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
                FrameworkElement_Loaded(null, null);
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
            // TODO: Assign a collection of bindable groups to this.DefaultViewModel["Groups"]

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
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void imgCha1_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }

        private void imgCha2_Tapped(object sender, TappedRoutedEventArgs e)
        {
        }

        private async void ShowYearChart()
        {
            var year = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "Year");
            var yearactive = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "YearActive");
            var month = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "Month");
            var monthactive = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "MonthActive");
            ControlTreeHelper.FindNameInVisualTree<Grid>(CarHub, "imgCha1G").Visibility = Visibility.Visible;
            ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "imgCha1A").Visibility = Visibility.Visible;
            ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "imgCha1B").Visibility = Visibility.Collapsed;
            ControlTreeHelper.FindNameInVisualTree<Grid>(CarHub, "imgCha2G").Visibility = Visibility.Collapsed;
            ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "imgCha2A").Visibility = Visibility.Collapsed;
            ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "imgCha2B").Visibility = Visibility.Collapsed;
            ControlTreeHelper.FindNameInVisualTree<TextBlock>(CarHub, "Actual").Text = "2014";

            year.Visibility = Visibility.Collapsed;
            yearactive.Visibility = Visibility.Visible;
            month.Visibility = Visibility.Visible;
            monthactive.Visibility = Visibility.Collapsed;

            await Task.Delay(YearChartLoadDelay);
            ((Storyboard)(ControlTreeHelper.FindNameInVisualTree<Grid>(CarHub, "StackPanel").Resources["Cha1LoadStoryBoard"])).Begin();
        }

        private async void ShowMonthChart()
        {
            var year = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "Year");
            var yearactive = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "YearActive");
            var month = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "Month");
            var monthactive = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "MonthActive");
            ControlTreeHelper.FindNameInVisualTree<Grid>(CarHub, "imgCha2G").Visibility = Visibility.Visible;
            ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "imgCha2A").Visibility = Visibility.Visible;
            ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "imgCha2B").Visibility = Visibility.Collapsed;
            ControlTreeHelper.FindNameInVisualTree<Grid>(CarHub, "imgCha1G").Visibility = Visibility.Collapsed;
            ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "imgCha1A").Visibility = Visibility.Collapsed;
            ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "imgCha1B").Visibility = Visibility.Collapsed;
            ControlTreeHelper.FindNameInVisualTree<TextBlock>(CarHub, "Actual").Text = "January 2014";

            year.Visibility = Visibility.Visible;
            yearactive.Visibility = Visibility.Collapsed;
            month.Visibility = Visibility.Collapsed;
            monthactive.Visibility = Visibility.Visible;

            await Task.Delay(MonthChartLoadDelay);
            ((Storyboard)(ControlTreeHelper.FindNameInVisualTree<Grid>(CarHub, "StackPanel").Resources["Cha2LoadStoryBoard"])).Begin();
        }

        private Random rand = new Random((int)DateTime.Now.Ticks);

        private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            ShowYearChart();
        }

        private void FrameworkElement_Loaded(object sender, RoutedEventArgs e)
        {
            isLoadedHub = true;

            var ima1a = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "imgCha1A");
            var ima1b = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "imgCha1B");
            var ima2a = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "imgCha2A");
            var ima2b = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "imgCha2B");

            var hdItems = ControlTreeHelper.FindNameInVisualTree<StackPanel>(CarHub, "HdItems");
            var hdItems2 = ControlTreeHelper.FindNameInVisualTree<StackPanel>(CarHub, "HdItems2");

            if (Window.Current.Bounds.Width < 1920)
            {
                ima1a.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Milles/chaA.png", UriKind.Absolute));
                ima1b.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Milles/chaB.png", UriKind.Absolute));
                ima2a.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Milles/cha2A.png", UriKind.Absolute));
                ima2b.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Milles/cha2B.png", UriKind.Absolute));
                hdItems.Visibility = Visibility.Collapsed;
                hdItems2.Visibility = Visibility.Collapsed;
                ControlTreeHelper.FindNameInVisualTree<Grid>(CarHub, "ButtonsGrid").Margin = new Thickness(80, 25, 0, 10);
            }
            else
            {
                ima1a.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Milles/hd-chaA.png", UriKind.Absolute));
                ima1b.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Milles/hd-chaB.png", UriKind.Absolute));
                ima2a.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Milles/hd-cha2A.png", UriKind.Absolute));
                ima2b.Source = new BitmapImage(new Uri("ms-appx:///Assets/1280/Milles/hd-cha2B.png", UriKind.Absolute));
                hdItems.Visibility = Visibility.Visible;
                hdItems2.Visibility = Visibility.Visible;
                ControlTreeHelper.FindNameInVisualTree<Grid>(CarHub, "ButtonsGrid").Margin = new Thickness(90, 25, 0, 10);
            }
        }

        private void Year_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ShowYearChart();
        }

        private void Month_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ShowMonthChart();
        }

        private void FirstElementTapped(object sender, TappedRoutedEventArgs e)
        {
            if (ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "Year").Visibility == Visibility.Collapsed)
            {
                this.Frame.Navigate(typeof(CarsHub), "Hanselman");
            }
        }

        private void ThirdElementTapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(CarsHub), "Guthrie");
        }

        private void FourthElementTapped(object sender, TappedRoutedEventArgs e)
        {
            if (ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "Year").Visibility == Visibility.Visible)
            {
                this.Frame.Navigate(typeof(CarsHub), "Hanselman");
            }
        }
    }
}
