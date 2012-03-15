using System;
using System.Linq;
using Ninject;
using Ninject.Activation;
using Ninject.Extensions.Conventions;
using Ninject.Planning.Bindings;

namespace efsession
{
    internal class OverridableBindingGenerator<TService> : IBindingGenerator
    {
        private readonly string _serviceAssembly;

        public OverridableBindingGenerator()
        {
            _serviceAssembly = Assembly(typeof(TService));
        }

        public void Process(Type type, Func<IContext, object> scopeCallback, IKernel kernel)
        {
            var service = typeof(TService);

            if (NoBindingsIn(kernel))
            {
                kernel.Rebind(service).To(type).InScope(scopeCallback).WithMetadata("assembly", Assembly(type));
                return;
            }

            if (DefaultBindingIn(kernel) && Assembly(type) != _serviceAssembly)
            {
                kernel.Rebind(service).To(type).InScope(scopeCallback).WithMetadata("assembly", Assembly(type));
                return;
            }

            kernel.Bind(service).To(type).InScope(scopeCallback).WithMetadata("assembly", Assembly(type));
        }

        private bool NoBindingsIn(IKernel kernel)
        {
            return !kernel.GetBindings(typeof(TService)).Any(HasAssemblyKey);
        }

        private bool DefaultBindingIn(IKernel kernel)
        {
            return kernel.GetBindings(typeof(TService)).Any(IsServiceAssemblyBinding);

        }

        private bool HasAssemblyKey(IBinding b)
        {
            var satisifies = b.Metadata.Has("assembly");
            return satisifies;
        }

        private bool IsServiceAssemblyBinding(IBinding b)
        {
            var haskey = HasAssemblyKey(b);
            if (!haskey) return false;
            var satisfies = b.Metadata.Get<string>("assembly") == _serviceAssembly;
            return satisfies;
        }

        private string Assembly(Type type)
        {
            return type.Assembly.FullName;
        }
    }
}
