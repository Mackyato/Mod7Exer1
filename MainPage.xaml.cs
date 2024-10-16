﻿using MySql.Data.MySqlClient;
using Mod7Exer1.Services;

namespace Mod7Exer1
{
    public partial class MainPage : ContentPage
    {
        private readonly DatabaseConnectionService _dbConnectionService;

        public MainPage()
        {
            InitializeComponent();
            _dbConnectionService = new DatabaseConnectionService();
        }

        private async void OnTestConnectionClicked(object sender, EventArgs e)
        {
            var connectionString = _dbConnectionService.GetConnectionString();
            try
            {
                using (var connection = new MySqlConnection(connectionString))
                {
                    await connection.OpenAsync();
                    ConnectionStatuslLabel.Text = "Connection Successful!";
                    ConnectionStatuslLabel.TextColor = Colors.Green;
                }
            }
            catch (Exception ex)
            {
                ConnectionStatuslLabel.Text = $"Connection Failed: {ex.Message}";
                ConnectionStatuslLabel.TextColor = Colors.Red;
            }
        }

        private async void OpenViewEmployee(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//ViewEmployee");
        }
    }

}