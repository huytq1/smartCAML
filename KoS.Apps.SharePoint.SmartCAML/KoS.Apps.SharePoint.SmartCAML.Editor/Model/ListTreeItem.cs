﻿using System.ComponentModel;
using System.Runtime.CompilerServices;
using KoS.Apps.SharePoint.SmartCAML.Editor.Annotations;
using KoS.Apps.SharePoint.SmartCAML.Model;

namespace KoS.Apps.SharePoint.SmartCAML.Editor.Model
{
    public class ListTreeItem : INotifyPropertyChanged
    {
        private bool _isExpanded = true;
        private bool _isSelected = true;
        public ISharePointProvider Client { get; set; }

        public bool IsExpanded
        {
            get { return _isExpanded; }
            set
            {
                if (value == _isExpanded) return;
                _isExpanded = value;
                OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get { return _isSelected; }
            set
            {
                if (value == _isSelected) return;
                _isSelected = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
