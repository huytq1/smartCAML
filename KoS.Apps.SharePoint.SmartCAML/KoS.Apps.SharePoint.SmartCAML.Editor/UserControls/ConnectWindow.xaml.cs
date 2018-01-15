﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using KoS.Apps.SharePoint.SmartCAML.SharePointProvider;
using System.Windows;
using System.Windows.Controls;
using KoS.Apps.SharePoint.SmartCAML.Editor.Model;
using KoS.Apps.SharePoint.SmartCAML.Editor.Utils;
using KoS.Apps.SharePoint.SmartCAML.Model;

namespace KoS.Apps.SharePoint.SmartCAML.Editor.UserControls
{
    /// <summary>
    /// Interaction logic for ConnectWindow.xaml
    /// </summary>
    public partial class ConnectWindow : UserControl
    {
        public ISharePointProvider Client { get; private set; }
        public ConnectWindowModel Model => (ConnectWindowModel)this.DataContext;
        public event Action<ConnectWindow, ISharePointProvider> DialogResult;

        public ConnectWindow()
        {
            Telemetry.Instance.Native.TrackPageView("Connect");
            InitializeComponent();

            this.Width = Config.ConnectWindowWidth;
            this.DataContext = new ConnectWindowModel();
        }

        private async void ucConnectButton_Click(object sender, RoutedEventArgs e)
        {
            Telemetry.Instance.Native.TrackEvent("Connect.OK", new Dictionary<string, string>
            {
                {"ProviderType", Model.ProviderType.ToString() },
            });

            var client = SharePointProviderFactory.Create(Model.ProviderType);

            try
            {
                Model.IsConnecting = true;
                StatusNotification.NotifyWithProgress("Connecting...");

                if (await client.Connect(Model.SharePointWebUrl, Model.UserName, Model.UserPassword) != null)
                {
                    Client = client;
                    Model.AddNewUrl(Model.SharePointWebUrl);
                    Model.AddUserToHistory();
                    Model.Save();

                    DialogResult(this, client);
                }
                StatusNotification.Notify("Connected");
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleConnection(ex);
            }

            Model.IsConnecting = false;
        }

        private void ucCancelButton_Click(object sender, RoutedEventArgs e)
        {
            Telemetry.Instance.Native.TrackEvent("Connect.Cancel");
            DialogResult(this, null);
        }

        private void AdvanceOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            Telemetry.Instance.Native.TrackEvent("Connect.AdvanceOptions");
            Model.ShowAdvanceOptions = true;
        }

        private void HideAdvanceOptionsButton_Click(object sender, RoutedEventArgs e)
        {
            Telemetry.Instance.Native.TrackEvent("Connect.AdvanceOptions");
            Model.ShowAdvanceOptions = false;
        }

        private void PasswordBox_OnPasswordChanged(object sender, RoutedEventArgs e)
        {
            Model.UserPassword = ((PasswordBox)sender).Password;
        }

        private void ConnectWindow_OnClosing(object sender, CancelEventArgs e)
        {
            Config.ConnectWindowWidth = this.Width;
        }
    }
}
