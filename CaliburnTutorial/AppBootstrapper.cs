using System;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.Linq;
using System.Windows;
using Caliburn.Micro;
using CaliburnTutorial.ViewModels;

namespace CaliburnTutorial
{
    public class AppBootstrapper : BootstrapperBase
    {
        private CompositionContainer _Container;

        public AppBootstrapper()
        {
            Initialize();
        }

        protected override object GetInstance(Type service, string key)
        {
            var contract = String.IsNullOrEmpty(key) ? AttributedModelServices.GetContractName(service) : key;
            var exports = _Container.GetExportedValues<Object>(contract).ToList();

            if (exports.Any())
            {
                return exports.First();
            }

            throw new Exception(String.Format("Could not locate any instances of contract {0}.", contract));
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
    }
}