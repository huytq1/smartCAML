﻿using System;
using System.IO;
using System.Net;
using System.Windows;

namespace KoS.Apps.SharePoint.SmartCAML.Editor.Utils
{
    static class ExceptionHandler
    {
        public static NLog.Logger Log = NLog.LogManager.GetCurrentClassLogger();

        public static void HandleConnection(Exception ex)
        {
            Telemetry.Instance.Native.TrackException(ex);
            if (ex is FileNotFoundException) HandleConnection((FileNotFoundException)ex);
            else if (ex is WebException) HandleConnection((WebException)ex);
            else
            {
                Log.Error(ex);
                StatusNotification.Notify("Connection failed");
                MessageBox.Show(ex.Message, "Connection failed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private static void HandleConnection(FileNotFoundException ex)
        {
            Log.Error(ex);
            StatusNotification.Notify("Connection failed");

            if (ex.Message.Contains("Microsoft.SharePoint"))
                MessageBox.Show("Could not load file or assembly 'Microsoft.SharePoint, Version=15.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c'.\n\n" +
                    "Make shure you are running application on SharePoint sever or change the connection type to 'Client' in 'advance settings'.", "Connection failed", MessageBoxButton.OK, MessageBoxImage.Information);
            else
                MessageBox.Show(ex.Message, "Connection failed", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private static void HandleConnection(WebException ex)
        {
            Log.Error(ex);
            StatusNotification.Notify("Connection failed");

            if (ex.Status == WebExceptionStatus.NameResolutionFailure)
            {
                MessageBox.Show("Could not find the server. Please check the URL.", "Connection failed", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if ( ex.Response is HttpWebResponse)
            {
                var response = (HttpWebResponse)ex.Response;

                switch (response.StatusCode)
                {
                    case HttpStatusCode.BadGateway:
                        MessageBox.Show("Could not find the server. Please check the URL.", "Connection failed", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    case HttpStatusCode.Unauthorized:
                    case HttpStatusCode.Forbidden:
                        MessageBox.Show("You are not authorized to open this site", "Connection failed", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                    default:
                        MessageBox.Show(ex.Message, "Connection failed", MessageBoxButton.OK, MessageBoxImage.Information);
                        break;
                }
            }
            else
                MessageBox.Show(ex.ToString(), "Connection failed", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public static void Handle(Exception ex, string message = null)
        {
            Telemetry.Instance.Native.TrackException(ex);
            Log.Error(ex);
            MessageBox.Show($"{message}\n\n{ex.Message}", "SmartCAML", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
