using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using Bing.Maps;
using Bing.Maps.Directions;
using MyShuttle.Dashboard.Common;

namespace MyShuttle.Dashboard.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MapPage : Page
    {
        private TimeSpan MapLoadDelay
        {
            get { return TimeSpan.FromMilliseconds(rand.Next(2000, 3000)); }
        }

        private NavigationHelper navigationHelper;
        private bool centered = false;
        private Random rand = new Random((int)DateTime.Now.Ticks);

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public MapPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.NavigationCacheMode = NavigationCacheMode.Disabled;
            var viewModel = (MapViewModel)this.DataContext;
            viewModel.PropertyChanged += viewModel_PropertyChanged;

            viewModel.LoadData(MapLoadDelay);
        }

        void viewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var viewModel = (MapViewModel)this.DataContext;
            if (!centered && !viewModel.IsLoading)
            {
                MainMap.SetView(viewModel.Center, viewModel.Zoom);
                foreach (var route in viewModel.Routes.Take(5))
                {
                    ListView.SelectedItems.Add(route);
                }
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

        private Dictionary<RouteViewModel, Tuple<Pushpin, MapPolyline, Pushpin>> routes = new Dictionary<RouteViewModel, Tuple<Pushpin, MapPolyline, Pushpin>>(); 

        private async void Selector_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var shapeLayer = MainMap.ShapeLayers.FirstOrDefault();
            if (shapeLayer == null)
            {
                shapeLayer = new MapShapeLayer();
                MainMap.ShapeLayers.Add(shapeLayer);
            }

            foreach (var routeViewModel in e.RemovedItems.Cast<RouteViewModel>())
            {
                var item = routes[routeViewModel];
                if (item != null)
                {
                    shapeLayer.Shapes.Remove(item.Item2);
                    MainMap.Children.Remove(item.Item1);
                    MainMap.Children.Remove(item.Item3);
                }

            }

            foreach (var routeViewModel in e.AddedItems.Cast<RouteViewModel>())
            {
                var pushpin1 = new Pushpin
                {
                    Background = new SolidColorBrush(Colors.White)
                };
                MapLayer.SetPosition(pushpin1, routeViewModel.Locations.First());
                var routeLine = new MapPolyline();
                routeLine.Color = routeViewModel.Invoiced ? Color.FromArgb(255, 255, 127, 101) : Color.FromArgb(255, 67, 82, 102);
                routeLine.Width = 5;
                routeLine.Locations = new LocationCollection();
                var pushpin2 = new Pushpin
                {
                    Background = new SolidColorBrush(Colors.White)
                };
                MapLayer.SetPosition(pushpin2, routeViewModel.Locations.Last());

                routes[routeViewModel] = new Tuple<Pushpin, MapPolyline, Pushpin>(pushpin1, routeLine, pushpin2);


                MainMap.Children.Add(pushpin1);
                shapeLayer.Shapes.Add(routeLine);
                foreach (var routeLocation in routeViewModel.Locations)
                {
                    await Task.Delay(100);
                    routeLine.Locations.Add(routeLocation);
                }
                if (routes.ContainsKey(routeViewModel))
                {
                    MainMap.Children.Add(pushpin2);
                }
            }
            Debug.WriteLine("End");
        }
    }
}
