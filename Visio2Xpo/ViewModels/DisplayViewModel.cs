using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using cvo.buyshans.Visio2Xpo.Data;
using cvo.buyshans.Visio2Xpo.UI.Messages;

namespace cvo.buyshans.Visio2Xpo.UI.ViewModels
{
    [Export]
    public class DisplayViewModel : Conductor<PropertyChangedBase>, IHandle<SchemaChanged>
    {
        private readonly IEventAggregator _EventAggregator;

        public ObservableCollection<SchemaViewModel> Entities { get; private set; }

        [ImportingConstructor]
        public DisplayViewModel(IEventAggregator eventAggregator)
        {
            Entities = new ObservableCollection<SchemaViewModel>();

            _EventAggregator = eventAggregator;
            _EventAggregator.Subscribe(this);
        }

        private void LoadSchema(Schema schema)
        {
            Entities.Clear();
            if (schema != null)
            {
                Entities.Add(new SchemaViewModel(schema));
            }
        }

        #region "Handles"
        public void Handle(SchemaChanged message)
        {
            LoadSchema(message.Schema);
        }
        #endregion "Handles"
    }
}