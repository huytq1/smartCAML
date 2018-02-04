﻿using System;
using System.Windows.Controls;
using KoS.Apps.SharePoint.SmartCAML.Editor.Builder.Filters;
using KoS.Apps.SharePoint.SmartCAML.Editor.Enums;
using KoS.Apps.SharePoint.SmartCAML.Model;

namespace KoS.Apps.SharePoint.SmartCAML.Editor.Builder.QueryFilters
{
    interface IQueryFilterController : IDisposable
    {
        Field Field { get; }
        FilterOperator? FilterOperator { get; }

        event EventHandler ValueChanged;

        void Initialize(Panel parent, string oldValue);
        string GetValue();
        IFilter GetFilter(QueryOperator queryOperator);
        void Refresh(IFilter filter);
    }
}
