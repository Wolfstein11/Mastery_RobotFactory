using Grpc.Net.Client;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TuFabricaDDD.Domain.Types;
using TuFabricaDDD.Domain.ValueObjects;
using TuFabricaDDD.GrpcContracts.Units;

namespace TuFabricaDDD.UI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<RobotViewModel> Robots { get; set; } = new();

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Connect to the gRPC service
            using var channel = GrpcChannel.ForAddress("https://localhost:5126"); // Adjust URL as needed
            var client = new UnitService.UnitServiceClient(channel);

            // Fetch all units
            var response = await client.GetAllUnitsAsync(new GetAllUnitsRequest());

            Robots.Clear();
            foreach (var unit in response.Items)
            {
                Robots.Add(new RobotViewModel
                {
                    Id = Guid.Parse(unit.Id),
                    SerialNumber = unit.SerialNumber,
                    Category = (RobotCategory)unit.Category,
                    Status = (RobotStatus)unit.Status,
                    // Map other properties as needed
                });
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            RobotsDataGrid.ItemsSource = Robots;
            RobotsDataGrid.LoadingRow += RobotsDataGrid_LoadingRow;
        }

        private void RobotsDataGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            if (e.Row.Item is RobotViewModel robot)
            {
                // Color rows based on status
                switch (robot.Status)
                {
                    case RobotStatus.Operational:
                        e.Row.Background = Brushes.LightGreen;
                        break;
                    case RobotStatus.Busy:
                        e.Row.Background = Brushes.LightYellow;
                        break;
                    case RobotStatus.UnderMaintenance:
                        e.Row.Background = Brushes.LightBlue;
                        break;
                    case RobotStatus.Broken:
                        e.Row.Background = Brushes.IndianRed;
                        break;
                    case RobotStatus.Idle:
                    default:
                        e.Row.Background = Brushes.White;
                        break;
                }
            }
        }

        private void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            // For demo: create a robot with random values
            var robot = new RobotViewModel
            {
                Id = Guid.NewGuid(),
                SerialNumber = $"SN-{DateTime.Now.Ticks}",
                Category = RobotCategory.Humanoid,
                Status = RobotStatus.Operational,
                CurrentLocation = new Location("AreaA", 1, 2, 0),
                NetworkLocation = new NetworkLocation("192.168.1.1", new AccessPoint("FactoryWiFi", 6))
            };
            Robots.Add(robot);
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (RobotsDataGrid.SelectedItem is RobotViewModel selected)
            {
                // For demo: cycle status
                selected.Status = selected.Status switch
                {
                    RobotStatus.Operational => RobotStatus.Busy,
                    RobotStatus.Busy => RobotStatus.UnderMaintenance,
                    RobotStatus.UnderMaintenance => RobotStatus.Broken,
                    RobotStatus.Broken => RobotStatus.Idle,
                    _ => RobotStatus.Operational
                };
                RobotsDataGrid.Items.Refresh();
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (RobotsDataGrid.SelectedItem is RobotViewModel selected)
            {
                Robots.Remove(selected);
            }
        }
    }

    public class RobotViewModel
    {
        public Guid Id { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public RobotCategory Category { get; set; }
        public RobotStatus Status { get; set; }
        public Location CurrentLocation { get; set; } = new Location("", 0, 0, 0);
        public NetworkLocation NetworkLocation { get; set; } = new NetworkLocation("0.0.0.0", new AccessPoint("", 1));
        public string LocationDisplay => CurrentLocation.ToString();
        public string NetworkDisplay => NetworkLocation.ToString();
    }
}