using System;
using System.Data.Entity;
using Ninject.Extensions.ContextPreservation;
using Ninject.Extensions.Conventions;
using Ninject.Modules;

namespace efsession
{
    public class EfSession : NinjectModule
    {
        private readonly Action<AssemblyScanner>[] _scanActions;

        public EfSession() : this(s => s.InRequestScope()) { }

        public EfSession(params Action<AssemblyScanner>[] scanActions)
        {
            _scanActions = scanActions;
        }

        public override void Load()
        {
            Kernel.BindPluggable<IConfigureModelBuilder>(x => x.InSingletonScope());

            Kernel.BindPluggable<DbContext>(_scanActions);

            Kernel.BindPluggable<ISession>(_scanActions);
            Kernel.BindInterfaceToBinding<IQueryStore, ISession>();
            Kernel.BindInterfaceToBinding<ICommandStore, ISession>();
            Kernel.BindInterfaceToBinding<IStore, ISession>();
        }
    }
}