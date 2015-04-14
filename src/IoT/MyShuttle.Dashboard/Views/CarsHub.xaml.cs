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
    public sealed partial class CarsHub : Page
    {
        private TimeSpan VehicleLoadDelay
        {
            get { return TimeSpan.FromMilliseconds(rand.Next(2000, 3000)); }
        }
        private TimeSpan AlarmsLoadDelay
        {
            get { return TimeSpan.FromMilliseconds(rand.Next(2000, 3000)); }
        }
        private TimeSpan ChartSectionLoadDelay
        {
            get { return TimeSpan.FromMilliseconds(rand.Next(2000, 3000)); }
        }
        private TimeSpan YearChartLoadDelay
        {
            get { return TimeSpan.FromMilliseconds(rand.Next(1200, 2800)); }
        }
        private TimeSpan MonthChartLoadDelay
        {
            get { return TimeSpan.FromMilliseconds(rand.Next(1200, 2800)); }
        }
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private bool isLoadedHub = false;

        private string Driver = null;

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

        public CarsHub()
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
                HubSection_Loaded_1(sender, null);
                CarSection_Loaded(sender, null);
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
                Driver = (string)e.NavigationParameter;
            }
            else
            {
                Driver = "Hanselman";
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

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            if (this.Frame.BackStack.Last().SourcePageType == typeof(MillesHub))
            {
                await Task.Delay(60);
                this.CarHub.ScrollToSection(this.CarHub.Sections[2]);
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void HubSection_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private async void HubSection_Loaded_1(object sender, RoutedEventArgs e)
        {
            isLoadedHub = true;

            var stc1 = ControlTreeHelper.FindNameInVisualTree<StackPanel>(CarHub, "stc1");
            var stc2 = ControlTreeHelper.FindNameInVisualTree<StackPanel>(CarHub, "stc2");
            var imgAla3 = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "imgAla3");
            var imgAla5 = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "imgAla5");
            // 1280x768 Resolution
            if (Window.Current.Bounds.Width < 1920)
            {
                stc1.Orientation = Orientation.Horizontal;
                stc2.Orientation = Orientation.Horizontal;
                imgAla3.Margin = new Thickness(5, 0, 0, 0);
                imgAla5.Margin = new Thickness(5, 0, 0, 0);
            }
            // 1920x1080 Resolution
            else
            {
                stc1.Orientation = Orientation.Vertical;
                stc2.Orientation = Orientation.Vertical;
                imgAla3.Margin = new Thickness(0, 5, 0, 0);
                imgAla5.Margin = new Thickness(0, 5, 0, 0);
            }
        }

        private async void CarSection_Loaded(object sender, RoutedEventArgs e)
        {
            isLoadedHub = true;

            var gridSke = ControlTreeHelper.FindNameInVisualTree<Grid>(CarHub, "gridSke");

            // 1280x768 Resolution
            if (Window.Current.Bounds.Width < 1920)
            {
                gridSke.Margin = new Thickness(0, 0, 0, 0);
            }
            // 1920x1080 Resolution
            else
            {
                gridSke.Margin = new Thickness(0, 100, 0, 0);
            }
        }
        private void imgCha1_Tapped(object sender, TappedRoutedEventArgs e)
        {
            ShowMonthChart();
        }

        private async void imgCha2_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(MapPage));
        }

        private async void ShowYearChart()
        {
            var year = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "Year");
            var yearactive = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "YearActive");
            var month = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "Month");
            var monthactive = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "MonthActive");
            var next = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "Next");
            ControlTreeHelper.FindNameInVisualTree<Grid>(CarHub, "imgCha1G").Visibility = Visibility.Visible;
            ControlTreeHelper.FindNameInVisualTree<UIElement>(CarHub, "imgCha1A").Visibility = Visibility.Visible;
            ControlTreeHelper.FindNameInVisualTree<Grid>(CarHub, "imgCha2G").Visibility = Visibility.Collapsed;
            ControlTreeHelper.FindNameInVisualTree<UIElement>(CarHub, "imgCha1B").Visibility = Visibility.Collapsed;
            ControlTreeHelper.FindNameInVisualTree<TextBlock>(CarHub, "Actual").Text = "2014";

            year.Visibility = Visibility.Collapsed;
            yearactive.Visibility = Visibility.Visible;
            month.Visibility = Visibility.Visible;
            monthactive.Visibility = Visibility.Collapsed;
            next.Visibility = Visibility.Collapsed;

            await Task.Delay(YearChartLoadDelay);
            
            ((Storyboard)(ControlTreeHelper.FindNameInVisualTree<StackPanel>(CarHub, "StackPanel").Resources["Cha1LoadStoryBoard"])).Begin();
        }

        private async void ShowMonthChart()
        {
            var year = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "Year");
            var yearactive = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "YearActive");
            var month = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "Month");
            var monthactive = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "MonthActive");
            var next = ControlTreeHelper.FindNameInVisualTree<Image>(CarHub, "Next");
            ControlTreeHelper.FindNameInVisualTree<Grid>(CarHub, "imgCha2G").Visibility = Visibility.Visible;
            ControlTreeHelper.FindNameInVisualTree<UIElement>(CarHub, "imgCha2A").Visibility = Visibility.Visible;
            ControlTreeHelper.FindNameInVisualTree<Grid>(CarHub, "imgCha1G").Visibility = Visibility.Collapsed;
            ControlTreeHelper.FindNameInVisualTree<UIElement>(CarHub, "imgCha2B").Visibility = Visibility.Collapsed;
            ControlTreeHelper.FindNameInVisualTree<TextBlock>(CarHub, "Actual").Text = "October 2014";

            year.Visibility = Visibility.Visible;
            yearactive.Visibility = Visibility.Collapsed;
            month.Visibility = Visibility.Collapsed;
            monthactive.Visibility = Visibility.Visible;
            next.Visibility = Visibility.Visible;

            await Task.Delay(MonthChartLoadDelay);
            
            ((Storyboard)(ControlTreeHelper.FindNameInVisualTree<StackPanel>(CarHub, "StackPanel").Resources["Cha2LoadStoryBoard"])).Begin();
        }

        private Random rand = new Random((int)DateTime.Now.Ticks);

        private async void Car_OnLoaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(VehicleLoadDelay);
            if (Driver == "Hanselman")
            {
                CarSection.Header = "Chevrolet Suburban";
            }
            else
            {
                CarSection.Header = "Dodge Caliber";
            }
            ((Storyboard)((FrameworkElement)sender).Resources["CarLoadStoryBoard"]).Begin();
        }

        private async void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
        {
            ControlTreeHelper.FindNameInVisualTree<Grid>(CarHub, "imgCha1G").Visibility = Visibility.Visible;
            await Task.Delay(ChartSectionLoadDelay);
            ((Storyboard)((FrameworkElement)sender).Resources["Cha1LoadStoryBoard"]).Begin();
        }

        private async void AlarmSection_OnLoaded(object sender, RoutedEventArgs e)
        {
            await Task.Delay(AlarmsLoadDelay);
            ((Storyboard)((FrameworkElement)sender).Resources["AlarmSectionLoadStoryBoard"]).Begin();
        }

        private void Year_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ShowYearChart();
        }

        private void Month_OnTapped(object sender, TappedRoutedEventArgs e)
        {
            ShowMonthChart();
        }
    }
}
