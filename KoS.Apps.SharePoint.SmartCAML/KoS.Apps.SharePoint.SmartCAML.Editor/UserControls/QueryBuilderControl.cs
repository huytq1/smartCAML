﻿using System;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using KoS.Apps.SharePoint.SmartCAML.Editor.Core;
using System.Collections.Generic;
using KoS.Apps.SharePoint.SmartCAML.Editor.Builder.Filters;

namespace KoS.Apps.SharePoint.SmartCAML.Editor.Controls
{
    /// <summary>
    /// Interaction logic for QueryBuilderControl.xaml
    /// </summary>
    public partial class QueryBuilderControl : UserControl
    {
        public event EventHandler Changed;
        public OrderedList<QueryFilterControl> Controller { get; }

        public static readonly DependencyProperty DisplayColumnsByTitleProperty = DependencyProperty.Register(nameof(DisplayColumnsByTitle), typeof(bool), typeof(QueryBuilderControl), null);
        [Bindable(true)]
        public bool DisplayColumnsByTitle
        {
            get
            {
                return (bool)this.GetValue(DisplayColumnsByTitleProperty);
            }
            set
            {
                this.SetValue(DisplayColumnsByTitleProperty, value);
            }
        }

        public QueryBuilderControl()
        {
            InitializeComponent();
            Controller = new OrderedList<QueryFilterControl>(ucFilters);
            AddFilter();
        }

        private void AddFilterButton_Click(object sender, RoutedEventArgs e)
        {
            AddFilter();
        }

        private void AddFilter()
        {
            var filter = Controller.Add();

            filter.SetBinding(QueryFilterControl.DisplayColumnsByTitleProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(DisplayColumnsByTitle)),
                Mode = BindingMode.TwoWay
            });

            filter.Changed += (sender,  e) => Changed?.Invoke(sender, e);
        }

        public IEnumerable<IFilter> GetFilters()
        {
            return ucFilters
                .Children
                .OfType<QueryFilterControl>()
                .Select( c => c.GetFilter())
                .Where( f => f != null);
        }
    }
}
