using System;
using System.ComponentModel.Composition;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using Caliburn.Micro;
using cvo.buyshans.Visio2Xpo.Communication.Visio;
using cvo.buyshans.Visio2Xpo.Communication.Visio.Factories;
using cvo.buyshans.Visio2Xpo.Data;
using cvo.buyshans.Visio2Xpo.UI.Messages;
using DevExpress.Mvvm;
using DevExpress.Mvvm.UI;

namespace cvo.buyshans.Visio2Xpo.UI.ViewModels
{
    [Export]
    public class ActionViewModel : PropertyChangedBase, IHandle<LoadMessage>, IHandle<SaveMessage>
    {
        private Boolean _CanSave;
        private Boolean _CanLoad = true;

        private Schema _Schema;
        
        private readonly IEventAggregator _EventAggregator;
        private readonly IOpenFileDialogService _VisioOpenFileDialogService;
        private readonly ISaveFileDialogService _XPOOpenFileDialogService;

        [ImportingConstructor]
        public ActionViewModel(IEventAggregator eventAggregator)
        {
            _EventAggregator = eventAggregator;
            _VisioOpenFileDialogService = new OpenFileDialogService { Filter = "Visio files (.vsdx)|*.vsdx" };
            _XPOOpenFileDialogService = new SaveFileDialogService { Filter = "XPO files (.xpo)|*.xpo" };

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
            _EventAggregator.PublishOnUIThreadAsync(new SaveMessage(_Schema));
        }

        public void Load()
        {
            _EventAggregator.PublishOnUIThreadAsync(new LoadMessage());
        }
        #endregion "UI Actions"

        public void Handle(LoadMessage message)
        {
            if (_VisioOpenFileDialogService.ShowDialog())
            {
                var file = _VisioOpenFileDialogService.Files.First();
                var resultFileName = file.DirectoryName + "\\" + file.Name;
                
                using (var reader = IoC.Get<IVisioReader>().Initialize(resultFileName))
                {
                    ReadVisioSchema(reader);
                    CanSave = true;
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

            _Schema = schema;
            _EventAggregator.PublishOnUIThreadAsync(new SchemaChanged(schema));
        }

        public void Handle(SaveMessage message)
        {
            if (_XPOOpenFileDialogService.ShowDialog())
            {
                var file = _XPOOpenFileDialogService.File;
                var resultFileName = file.DirectoryName + "\\" + file.Name;

                IoC.Get<IFormatter>()
                    .Serialize(new FileStream(resultFileName, FileMode.Create),
                               message.Schema);
            }
        }
    }
}