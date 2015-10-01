﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace KoS.Apps.SharePoint.SmartCAML.Editor.Controls
{
    /// <summary>
    /// Interaction logic for ClosableTabHeader.xaml
    /// </summary>
    public partial class ClosableTabHeader : UserControl
    {
        public string HeaderText
        {
            get { return (string) ucTabName.Content; }
            set { ucTabName.Content = value; }
        }

        public bool CloseButtonVisible
        {
            get { return ucCloseButton.Visibility == Visibility.Visible; }
            set { ucCloseButton.Visibility = value ? Visibility.Visible : Visibility.Collapsed; }
        }

        public ClosableTabHeader()
        {
            InitializeComponent();
        }

        private void UcCloseButton_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ucCloseButton.Foreground = Brushes.Crimson;
        }

        private void UcCloseButton_OnMouseLeave(object sender, MouseEventArgs e)
        {
            ucCloseButton.Foreground = Brushes.Gray;
        }

        private void UcCloseButton_OnClick(object sender, RoutedEventArgs e)
        {
            var tabItem = (TabItem) this.Parent;
            ((TabControl)tabItem.Parent).Items.Remove(tabItem);
        }

        private void UcTabName_OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ucCloseButton.Margin = new Thickness(ucTabName.ActualWidth + 5, 3, 4, 0);
        }
    }
}
