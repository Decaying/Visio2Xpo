using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using cvo.buyshans.Visio2Xpo.UI.ViewModels;

namespace cvo.buyshans.Visio2Xpo.UI
{
    public class Bootstrapper : BootstrapperBase
    {
        private CompositionContainer _Container;

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _Container = new CompositionContainer(
                new AggregateCatalog(
                    AssemblySource.Instance.Select(x => new AssemblyCatalog(x))
                )
            );
            
            var batch = new CompositionBatch();

            batch.AddExportedValue<IWindowManager>(new WindowManager());
            batch.AddExportedValue<IEventAggregator>(new EventAggregator());
            batch.AddExportedValue(_Container);

            _Container.Compose(batch);
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<MainWindowViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            var contract = String.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(service) : key;
            var exports = _Container.GetExportedValues<Object>(contract).ToList();

            if (!exports.Any())
                throw new Exception(String.Format("No instances found of contract: {0}!", contract));
            
            return exports.First();
        }
    }
}