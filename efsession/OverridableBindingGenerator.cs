using System;
using System.Linq;
using Ninject;
using Ninject.Activation;
using Ninject.Extensions.Conventions;
using Ninject.Planning.Bindings;

namespace efsession
{
    internal class OverridableBindingGenerator : IBindingGenerator
    {
        private readonly string _serviceAssembly;
        private readonly Type _service;
        public OverridableBindingGenerator(Type type)
        {
            _service = type;
            _serviceAssembly = Assembly(_service);
        }

        public void Process(Type type, Func<IContext, object> scopeCallback, IKernel kernel)
        {
            if (NoBindingsIn(kernel))
            {
                kernel.Rebind(_service).To(type).InScope(scopeCallback).WithMetadata("assembly", Assembly(type));
                return;
            }

            if (DefaultBindingIn(kernel) && Assembly(type) != _serviceAssembly)
            {
                kernel.Rebind(_service).To(type).InScope(scopeCallback).WithMetadata("assembly", Assembly(type));
                return;
            }

            kernel.Bind(_service).To(type).InScope(scopeCallback).WithMetadata("assembly", Assembly(type));
        }

        private bool NoBindingsIn(IKernel kernel)
        {
            return !kernel.GetBindings(_service).Any(HasAssemblyKey);
        }

        private bool DefaultBindingIn(IKernel kernel)
        {
            return kernel.GetBindings(_service).Any(IsServiceAssemblyBinding);

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
