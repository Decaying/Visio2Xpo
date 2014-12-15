using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows;
using Caliburn.Micro;
using cvo.buyshans.Visio2Xpo.Communication.Visio;
using cvo.buyshans.Visio2Xpo.Communication.Visio.Factories;
using cvo.buyshans.Visio2Xpo.Data;
using cvo.buyshans.Visio2Xpo.UI.Messages;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors.Helpers;

namespace cvo.buyshans.Visio2Xpo.UI.ViewModels
{
    [Export]
    public class ActionViewModel : PropertyChangedBase, IHandle<LoadMessage>, IHandle<SaveMessage>
    {
        private Boolean _CanSave;
        private Boolean _CanLoad = true;
        
        private readonly IEventAggregator _EventAggregator;
        private readonly IOpenFileDialogService _OpenFileDialogService;

        [ImportingConstructor]
        public ActionViewModel(IEventAggregator eventAggregator)
        {
            _EventAggregator = eventAggregator;
            _OpenFileDialogService = new OpenFileDialogService {Filter = "Visio files (.vsdx)|*.vsdx"};

            _EventAggregator.Subscribe(this);
        }

        #region "UI Properties"
        public Boolean CanSave
        {
            get
            {
                return _CanSave;
            }
            private set
            {
                if (_CanSave == value) return;
                _CanSave = value;
                NotifyOfPropertyChange();
            }
        }
        
        public Boolean CanLoad
        {
            get
            {
                return _CanLoad;
            }
            private set
            {
                if (_CanLoad != value)
                {
                    _CanLoad = value;
                    NotifyOfPropertyChange();
                }
            }
        }
        #endregion "UI Properties"
        #region "UI Actions"
        public void Save()
        {
            _EventAggregator.PublishOnUIThread(new SaveMessage());
        }

        public void Load()
        {
            _EventAggregator.PublishOnUIThread(new LoadMessage());
        }
        #endregion "UI Actions"

        public void Handle(LoadMessage message)
        {

            if (_OpenFileDialogService.ShowDialog())
            {
                var file = _OpenFileDialogService.Files.First();
                var resultFileName = file.DirectoryName + "\\" + file.Name;
                
                using (var reader = IoC.Get<IVisioReader>().Initialize(resultFileName))
                {
                    ReadVisioSchema(reader);
                }
            }
        }

        private void ReadVisioSchema(IVisioReader reader)
        {
            IFactory<Schema> factory = new SchemaFactory(reader);

            var schema = factory.Create();

            if (factory.HasErrors())
            {
                var sb = new StringBuilder();

                factory.GetErrors().ToList().ForEach(e =>
                    sb.AppendLine(e)
                    );

                throw new Exception(sb.ToString());
            }

            _EventAggregator.PublishOnUIThreadAsync(new SchemaChanged(schema));
        }

        public void Handle(SaveMessage message)
        {
        }
    }
}