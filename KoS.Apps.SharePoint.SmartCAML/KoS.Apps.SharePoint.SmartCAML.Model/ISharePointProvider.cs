﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace KoS.Apps.SharePoint.SmartCAML.Model
{
    public interface ISharePointProvider
    {
        Web Web { get; }

        Task<Web> Connect(string url);
        Task<Web> Connect(string url, string userName, string password);

        Task<List<ListItem>> ExecuteQuery(ListQuery query, int? pageSize);
        Task FillListFields(SList list);
        Task SaveItem(ListItem item);
        Task FillContentTypes(SList list, bool fillAlsoWeb = true);
        Task<List<KeyValuePair<string, string>>> GetLookupItems(FieldLookup lookup);
    }
}
