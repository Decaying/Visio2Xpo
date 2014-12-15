using System;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using Caliburn.Micro;
using cvo.buyshans.Visio2Xpo.Data;
using cvo.buyshans.Visio2Xpo.UI.Messages;

namespace cvo.buyshans.Visio2Xpo.UI.ViewModels
{
    [Export]
    public class DisplayViewModel : PropertyChangedBase, IHandle<SchemaChanged>
    {
        private readonly IEventAggregator _EventAggregator;

        public ObservableCollection<SchemaViewModel> SchemaViewModel { get; private set; }

        [ImportingConstructor]
        public DisplayViewModel(IEventAggregator eventAggregator)
        {
            SchemaViewModel = new ObservableCollection<SchemaViewModel>();

            _EventAggregator = eventAggregator;
            _EventAggregator.Subscribe(this);
        }

        #region "Handles"
        public void Handle(SchemaChanged message)
        {
            LoadSchema(message.Schema);
        }

        private void LoadSchema(Schema schema)
        {
            SchemaViewModel.Clear();
            SchemaViewModel.Add(new SchemaViewModel(schema));
        }

        #endregion "Handles"
    }
}