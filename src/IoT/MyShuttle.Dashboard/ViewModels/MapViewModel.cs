using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Data.Json;
using Bing.Maps;

namespace MyShuttle.Dashboard.Views
{
    public class MapViewModel : INotifyPropertyChanged
    {
        private string selectedDate;
        private List<RouteViewModel> routes;
        private ObservableCollection<RouteViewModel> selectedRoutes;
        private Location center;
        private double zoom;

        private bool isLoading;
        
        public string SelectedDate
        {
            get { return selectedDate; }
            set
            {
                if (selectedDate != value)
                {
                    selectedDate = value;
                    OnPropertyChanged();
                }
            }
        }

        public List<RouteViewModel> Routes
        {
            get { return routes; }
            set
            {
                if (routes != value)
                {
                    routes = value;
                    OnPropertyChanged();
                }
            }
        }

        public Location Center
        {
            get { return center; }
            set
            {
                if (center != value)
                {
                    center = value;
                    OnPropertyChanged();
                }
            }
        }

        public double Zoom
        {
            get { return zoom; }
            set
            {
                if (zoom != value)
                {
                    zoom = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsLoading
        {
            get { return isLoading; }
            set
            {
                if (isLoading != value)
                {
                    isLoading = value;
                    OnPropertyChanged();
                }
            }
        }

        public MapViewModel()
        {
            Center = new Location();
            Zoom = 0;
        }

        public async void LoadData(TimeSpan mapLoadDelay)
        {
            this.IsLoading = true;
            SelectedDate = "October 8, 2014";
            await Task.Delay(mapLoadDelay);
            Center = new Location(40.717682971847694, -73.8929183113467);
            Zoom = 13.5;
            Routes = new List<RouteViewModel>
            {
                new RouteViewModel
                {
                    From = "68 Bushwick Ave, Brooklyn",
                    To = "76 Queens Blvd, Flushing",
                    Date = "Oct 8, 2014",
                    Time = "16:42",
                    Locations = new LocationCollection()
                },
                new RouteViewModel
                {
                    From = "76 Queens Blvd, Flushing",
                    To = "89 69th Rd, Forest Hills",
                    Date = "Oct 8, 2014",
                    Time = "18:19",
                    Locations = new LocationCollection(),
                    Invoiced = true
                }
            };
            Routes[0].AddLocationsFromJson("[[40.712808,-73.941107],[40.71193,-73.940681],[40.712301,-73.939189],[40.714409,-73.930917],[40.714441,-73.930612],[40.713958,-73.926299],[40.71399,-73.923708],[40.713502,-73.915677],[40.71362,-73.915441],[40.715262,-73.913011],[40.71561,-73.91234],[40.717402,-73.910409],[40.719339,-73.906831],[40.723099,-73.90078],[40.723201,-73.900582],[40.72517,-73.896698],[40.726457,-73.894407],[40.727509,-73.892771],[40.728271,-73.89128],[40.728228,-73.891151],[40.72849,-73.888721],[40.729022,-73.88172],[40.729172,-73.880851],[40.734268,-73.87384],[40.734,-73.872858],[40.733898,-73.872499],[40.733807,-73.872164]]");
            Routes[1].AddLocationsFromJson("[[40.733807,-73.872163],[40.733769,-73.872022],[40.73342,-73.870868],[40.733329,-73.870568],[40.733157,-73.870659],[40.73143,-73.871571],[40.730878,-73.871678],[40.730062,-73.871651],[40.729209,-73.87149],[40.727879,-73.871077],[40.724601,-73.869549],[40.719119,-73.864527],[40.714811,-73.860048],[40.71398,-73.859758],[40.71185,-73.85985],[40.711818,-73.859651],[40.711078,-73.854791],[40.711434,-73.854571]]");
            
            this.IsLoading = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class RouteViewModel
    {
        public string From { get; set; }

        public string To { get; set; }

        public bool Invoiced { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public LocationCollection Locations { get; set; }

        public void AddLocationsFromJson(string json)
        {
            JsonArray obj = JsonArray.Parse(json);
            foreach (var location in obj)
            {
                var locationarray = location.GetArray();
                Locations.Add(new Location(locationarray[0].GetNumber(), locationarray[1].GetNumber()));
            }
        }
    }
}
