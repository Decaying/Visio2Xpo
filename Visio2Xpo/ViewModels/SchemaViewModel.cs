using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Caliburn.Micro;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.UI.ViewModels
{
    public class SchemaViewModel : PropertyChangedBase
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

        public ObservableCollection<TableViewModel> TableViewModel;

        public SchemaViewModel(Schema schema)
        {
            Schema = schema;
            AOTObject = "AOT";
            TableViewModel = new ObservableCollection<TableViewModel>();
            LoadTables(schema.Tables);
        }

        private void LoadTables(IEnumerable<Table> tables)
        {
            TableViewModel.Clear();
            tables.ToList().ForEach(t => TableViewModel.Add(new TableViewModel(t)));
        }
    }
}