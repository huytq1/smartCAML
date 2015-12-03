﻿using System;
using System.Deployment.Application;
using KoS.Apps.SharePoint.SmartCAML.Editor.Utils;
using System.Windows;
using System.Windows.Media;

namespace KoS.Apps.SharePoint.SmartCAML.Editor.Dialogs
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        private static bool _pendingRestart;

        public AboutWindow()
        {
            InitializeComponent();
            ucVersion.Text = VersionUtil.GetVersion();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/konradsikorski/smartCAML");
        }

        private async void AboutWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            if (_pendingRestart)
            {
                UpdateStatusSuccess("Update completed. Restart the application.");
                return;
            }

            try
            {
                var updateInfo = await ClickOnceHelper.CheckNewVersion();

                if (updateInfo.UpdateAvailable)
                {
                    UpdateStatusMessage($"New version '{updateInfo.AvailableVersion.ToString(4)}' is available.", true);
                    ucUpdateButton.Visibility = Visibility.Visible;
                }
                else
                {
                    UpdateStatusMessage("Application is up to date.");
                }
            }
            catch(Exception ex)
            {
                UpdateStatusError("Could not check updates, try again later.", ex);
            }
        }

        private void UcUpdateButton_OnClick(object sender, RoutedEventArgs e)
        {
            UpdateStatusMessage("Updating...", false);

            ClickOnceHelper.DoUpdateAsync(
                (o, args) =>
                {
                    if (args.Error != null)
                    {
                        UpdateStatusError("Update failed.", args.Error);
                    }
                    else if (args.Cancelled)
                    {
                        UpdateStatusSuccess("Update canceled.");
                    }
                    else
                    {
                        _pendingRestart = true;
                        UpdateStatusSuccess("Update completed. Restart the application.");
                    }
                },
                (o, args) =>
                {
                    ucUpdateMessage.Text = $"Updating {args.ProgressPercentage}%...";
                }
            );

        }

        private void UpdateStatusSuccess(string message, bool? installButtonVisible = null)
        {
            ucUpdateMessage.Foreground = Brushes.ForestGreen;
            ucUpdateMessage.Text = message;

            if (installButtonVisible.HasValue)
                ucUpdateButton.Visibility = installButtonVisible == true 
                    ? Visibility.Visible 
                    : Visibility.Collapsed;
        }

        private void UpdateStatusError(string message, Exception ex)
        {
            ucUpdateMessage.Foreground = Brushes.DarkRed;
            ucUpdateMessage.Text = message;
        }

        private void UpdateStatusMessage(string message, bool? installButtonVisible = null)
        {
            ucUpdateMessage.Foreground = Brushes.Gray;
            ucUpdateMessage.Text = message;

            if (installButtonVisible.HasValue)
                ucUpdateButton.Visibility = installButtonVisible == true
                    ? Visibility.Visible
                    : Visibility.Collapsed;
        }
    }
}
