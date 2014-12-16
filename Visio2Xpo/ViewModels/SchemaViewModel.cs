using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.UI.ViewModels
{
    public class SchemaViewModel : Conductor<PropertyChangedBase>
    {
        private Schema _Schema;
        public Schema Schema
        {
            get
            {
                return _Schema;
            }
            private set
            {
                if (_Schema == value) return;
                _Schema = value;
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

        public ObservableCollection<TableViewModel> Tables { get; private set; }

        public SchemaViewModel(Schema schema)
        {
            Tables = new ObservableCollection<TableViewModel>();
            Schema = schema;

            AOTObject = "AOT";
            AOTObjectType = "AOT";

            LoadTables(schema.Tables);
        }

        private void LoadTables(IEnumerable<Table> tables)
        {
            Tables.Clear();
            tables.ToList().ForEach(t => Tables.Add(new TableViewModel(t)));
        }
    }
}