using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using Caliburn.Micro;
using cvo.buyshans.Visio2Xpo.Communication.DynamicsAX;
using cvo.buyshans.Visio2Xpo.Communication.Visio;
using cvo.buyshans.Visio2Xpo.UI.Messages;
using cvo.buyshans.Visio2Xpo.UI.ViewModels;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;

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
            //IoC.Get<IEventAggregator>().PublishOnUIThread(new ThemeChanged("Office2013DarkGray"));
        }
        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            var assemblies = base.SelectAssemblies().ToList();

            assemblies.Add(typeof(VisioReader).GetTypeInfo().Assembly);
            assemblies.Add(typeof(XpoFormatter).GetTypeInfo().Assembly);

            return assemblies;
        }

        protected override void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var exception = e.Exception.InnerException;

            while (exception.InnerException != null)
            {
                exception = exception.InnerException;
            }

            WinUIMessageBox.Show(
               exception.Message,
               "Something happened",
               MessageBoxButton.OK,
               MessageBoxImage.Error);

            e.Handled = true;
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