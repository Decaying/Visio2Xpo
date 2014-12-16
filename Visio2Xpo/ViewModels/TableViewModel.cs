using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.UI.ViewModels
{
    public class TableViewModel : Conductor<PropertyChangedBase>
    {
        public ObservableCollection<FieldViewModel> Fields { get; private set; } 

        private Table _Table;
        public Table Table
        {
            get
            {
                return _Table;
            }
            private set
            {
                if (_Table == value) return;
                _Table = value;
                NotifyOfPropertyChange();
            }
        }

        private String _AOTObject;
        public String AOTObject
        {
            get
            {
                return _AOTObject;
            }
            private set
            {
                if (_AOTObject == value) return;
                _AOTObject = value;
                NotifyOfPropertyChange();
            }
        }

        private String _AOTObjectType;

        public String AOTObjectType
        {
            get
            {
                return _AOTObjectType;
            }
            set
            {
                if (_AOTObjectType == value) return;
                _AOTObjectType = value;
                NotifyOfPropertyChange();
            }
        }
    
        public TableViewModel(Table table)
        {
            Table = table;
            Fields = new ObservableCollection<FieldViewModel>();

            AOTObject = table.Name;
            AOTObjectType = "Table";

            Fields.Clear();
            LoadFields(table.Fields);
            if (table.PrimaryKey != null)
            {
                LoadPrimaryKeys(table.PrimaryKey.Fields);
            }
        }

        private void LoadPrimaryKeys(IEnumerable<Field> fields)
        {
            fields.ToList().ForEach(f => Fields.Add(new FieldViewModel(f)));
        }

        private void LoadFields(IEnumerable<Field> fields)
        {
            fields.ToList().ForEach(f => Fields.Add(new FieldViewModel(f)));
        }
    }
}