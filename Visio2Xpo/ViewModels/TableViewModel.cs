using System;
using Caliburn.Micro;
using cvo.buyshans.Visio2Xpo.Data;

namespace cvo.buyshans.Visio2Xpo.UI.ViewModels
{
    public class TableViewModel : PropertyChangedBase
    {

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


        public TableViewModel(Table table)
        {
            AOTObject = table.Name;
            Table = table;
        }
    }
}