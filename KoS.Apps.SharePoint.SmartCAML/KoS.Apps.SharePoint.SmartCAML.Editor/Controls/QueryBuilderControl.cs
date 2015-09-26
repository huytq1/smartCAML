﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using KoS.Apps.SharePoint.SmartCAML.Editor.Builder;

namespace KoS.Apps.SharePoint.SmartCAML.Editor.Controls
{
    /// <summary>
    /// Interaction logic for QueryBuilderControl.xaml
    /// </summary>
    public partial class QueryBuilderControl : UserControl
    {
        public event EventHandler Changed;

        public QueryBuilderControl()
        {
            InitializeComponent();
            AddFilter();
        }

        private void AddFilterButton_Click(object sender, RoutedEventArgs e)
        {
            AddFilter();
        }

        private void QueryFilterControl_OnRemoveClick(object sender, EventArgs e)
        {
            ucFilters.Children.Remove((UIElement)sender);
            Changed?.Invoke(this, EventArgs.Empty);
        }

        private void AddFilter()
        {
            var filterControl = new QueryFilterControl();
            filterControl.RemoveClick += QueryFilterControl_OnRemoveClick;
            filterControl.Changed += (sender, args) => Changed?.Invoke(this, EventArgs.Empty);
            filterControl.Up += MoveFilterUp;
            filterControl.Down += MoveFilterDown;

            ucFilters.Children.Add(filterControl);
        }

        private void MoveFilterDown(object control, EventArgs eventArgs)
        {
            var filterControl = (UIElement)control;
            var index = ucFilters.Children.IndexOf(filterControl);

            if (index > 0)
                MoveFilter(filterControl, index + 1);
        }

        private void MoveFilterUp(object control, EventArgs eventArgs)
        {
            var filterControl = (UIElement)control;
            var index = ucFilters.Children.IndexOf(filterControl);

            if (index > 0)
                MoveFilter(filterControl, index - 1);
        }

        private void MoveFilter(UIElement filterControl, int newIndex)
        {
            ucFilters.Children.Remove(filterControl);
            ucFilters.Children.Insert(newIndex, filterControl);

            Changed?.Invoke(this, EventArgs.Empty);
        }

        public QueryBuilder Build()
        {
            var builder = new QueryBuilder();
            foreach (var control in ucFilters.Children.OfType<QueryFilterControl>())
            {
                control.BuildQuery(builder);
            }

            return builder;
        }
    }
}
