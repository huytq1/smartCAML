﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using KoS.Apps.SharePoint.SmartCAML.Editor.BindingConverters;
using KoS.Apps.SharePoint.SmartCAML.Model;

namespace KoS.Apps.SharePoint.SmartCAML.Editor.Controls
{
    /// <summary>
    /// Interaction logic for ItemsGrid.xaml
    /// </summary>
    public partial class ItemsGrid : UserControl
    {
        private SmartCAML.Model.SList _list;

        #region Dependency Property

        public static readonly DependencyProperty DisplayColumnsByTitleProperty = DependencyProperty.Register(nameof(DisplayColumnsByTitle), typeof(bool), typeof(ItemsGrid), new PropertyMetadata(DisplayColumnsByTitlePropertyChanged));
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

        private static void DisplayColumnsByTitlePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            foreach (var column in ((ItemsGrid) d).ucItems.Columns)
            {
                var header = column.Header;
                column.Header = null;
                column.Header = header;
            }
        }

        #endregion

        public ItemsGrid()
        {
            InitializeComponent();
        }

        public SList List
        {
            get { return _list; }
            set
            {
                _list = value;

                foreach (var column in List.Fields.Select( c => new
                    {
                        Header = new ColumnHeader { Title = c.Title, InternalName = c.InternalName},
                        Bind = c.InternalName,
                        Field = c
                    }).OrderBy( c => DisplayColumnsByTitle ? c.Header.Title : c.Header.InternalName))
                {
                    var gridColumn = new DataGridTextColumn
                    {
                        IsReadOnly = column.Field.IsReadonly,
                        Header = column.Header,
                        Width = 100,
                        Binding = new Binding($"[{column.Bind}]") {Mode = BindingMode.TwoWay}
                    };

                    BindColumnHeaderFormat(gridColumn);
                    ucItems.Columns.Add(gridColumn);
                }
            }
        }

        public bool HasChanges => ucItems.ItemsSource?.Cast<ListItem>().Any(item => item.IsDirty) ?? false;

        internal void QueryResult(List<ListItem> items)
        {
            ucItems.ItemsSource = items;
        }

        private void BindColumnHeaderFormat(DataGridTextColumn column)
        {
            BindingOperations.SetBinding(column, DataGridTextColumn.HeaderStringFormatProperty, new Binding
            {
                Source = this,
                Path = new PropertyPath(nameof(DisplayColumnsByTitle)),
                Mode = BindingMode.TwoWay,
                Converter = new BoolToStringConverter { True= "Title", False = "InternalName"}
            });
        }

        private void ucItems_RowEditEnding(object sender, DataGridRowEditEndingEventArgs e)
        {
            //if (e.EditAction == DataGridEditAction.Cancel) return;

            //var item = (ListItem)e.Row.DataContext;
            //try
            //{
            //    item.Update();
            //}
            //catch (Exception)
            //{
            //    item.CancelChanges();
            //    e.Cancel = true;
            //    ucItems.CancelEdit();
            //}
        }

        public List<ListItem> GetDirtyItems()
            => ucItems.ItemsSource.Cast<ListItem>().Where(item => item.IsDirty).ToList();

        public void SetFields()
        {
            

        }

        public class ColumnHeader : IFormattable
        {
            public string Title { get; set; }
            public string InternalName { get; set; }
            
            public override string ToString()
            {
                return Title;
            }

            public string ToString(string format, IFormatProvider formatProvider)
            {
                return format?.ToLower() == nameof(InternalName).ToLower()
                    ? InternalName
                    : Title;
            }
        }

        private void MenuItem_OnClick(object sender, RoutedEventArgs e)
        {
            var mi = sender as MenuItem;
            var cx = mi.Parent as ContextMenu;
            var c = cx.PlacementTarget;
            var g = c as System.Windows.Controls.Primitives.DataGridColumnHeader;
            g.Column.Visibility = Visibility.Collapsed;
        }
    }
}
