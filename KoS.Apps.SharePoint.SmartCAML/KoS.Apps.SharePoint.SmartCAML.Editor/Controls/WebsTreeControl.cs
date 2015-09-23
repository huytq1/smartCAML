﻿using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;
using KoS.Apps.SharePoint.SmartCAML.Editor.Model;
using KoS.Apps.SharePoint.SmartCAML.Model;
using KoS.Apps.SharePoint.SmartCAML.SharePointProvider;

namespace KoS.Apps.SharePoint.SmartCAML.Editor.Controls
{
    /// <summary>
    /// Interaction logic for WebsTreeControl.xaml
    /// </summary>
    public partial class WebsTreeControl : UserControl
    {
        public ObservableCollection<ListTreeItem> Webs = new ObservableCollection<ListTreeItem>();

        public SList SelectedList
            => ucLists.ItemsSource != null ?
            (ucLists.SelectedItem as SList) :
            null;

        public WebsTreeControl()
        {
            InitializeComponent();
            ucLists.ItemsSource = Webs;
        }

        public event EventHandler ListExecute;

        private void ucLists_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if ((ucLists.SelectedItem as SList) == null) return;
            ListExecute?.Invoke(this, EventArgs.Empty);
        }

        public void Add(ISharePointProvider client)
        {
            if (client == null) return;

            Webs.Add(new ListTreeItem { Client = client} );
        }

        public ISharePointProvider GetClient(Web web)
        {
            return Webs.FirstOrDefault(client => client.Client.Web == web)?.Client;
        }
    }
}
