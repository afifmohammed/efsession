using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Text;
using Ninject;
using Ninject.Extensions.Conventions;

namespace efsession
{
    internal static class Extensions
    {
        public static string ToFriendlyMessage(this DbEntityValidationException ex)
        {
            var message = new StringBuilder("Validation errors from Entity Framework");
            ex.EntityValidationErrors.ForEach(er => er.ValidationErrors.ForEach(e => message.AppendLine("Property: {0} Error: {1}".For(e.PropertyName, e.ErrorMessage))));
            return message.ToString();
        }

        /// <summary>
        /// enumerates each item on the <paramref name="items"/> collection and will apply the <paramref name="action"/> on it.
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> items, Action<T> action)
        {
            foreach (var item in items)
            {
                action(item);
            }
        }

        /// <summary>
        /// extension method to invoke <see cref="string.Format(string,object[])"/>
        /// </summary>
        public static string For(this string input, params object[] args)
        {
            return string.Format(input, args);
        }

        public static IKernel BindPluggable<TPluggableService>(this IKernel kernel, params Action<AssemblyScanner>[] actions) where TPluggableService : class
        {
            kernel.Scan(scanner =>
                            {
                                scanner.FromAssembliesInPath(AppDomain.CurrentDomain.ExecutingAssmeblyPath());
                                actions.ForEach(a => a(scanner));
                                scanner.WhereTypeInheritsFrom<TPluggableService>();
                                scanner.Where(target => !target.IsAbstract && !target.IsInterface && target.IsClass);
                                scanner.BindWith<OverridableBindingGenerator<TPluggableService>>();
                            });

            return kernel;
        }

        public static string ExecutingAssmeblyPath(this AppDomain appDomain)
        {
            return string.IsNullOrEmpty(appDomain.RelativeSearchPath)
                       ? appDomain.BaseDirectory
                       : appDomain.RelativeSearchPath;
        }
    }
}