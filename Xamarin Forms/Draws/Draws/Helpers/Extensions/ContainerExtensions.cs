using System;
using System.Linq;
using System.Reflection;
using Draws.Helpers.Attributes;
using Prism.Ioc;
using Prism.Navigation;
using Xamarin.Forms;

namespace Draws.Helpers.Extensions
{
    public static class ContainerExtensions
    {
        public static IContainerRegistry AutoRegisterMvvmComponents(this IContainerRegistry container, Assembly assembly)
        {
            var pageBaseTypeInfo = typeof(Page).GetTypeInfo();
            var pageTypesInfos = assembly.DefinedTypes.Where(x => x.IsClass && pageBaseTypeInfo.IsAssignableFrom(x));

            foreach (var page in pageTypesInfos)
            {
                var pageName = GetPageName(page.AsType());
                container.RegisterForNavigation(page.AsType(), pageName);
                PageNavigationRegistry.Register(pageName, page.AsType());
            }

            return container;
        }

        private static string GetPageName(Type pageType)
        {
            var attrs = pageType.GetTypeInfo().GetCustomAttributes();

            foreach (var attr in attrs)
            {
                if (attr is PageAttribute pageAttribute)
                    return pageAttribute.Name;
            }

            return pageType.Name;
        }
    }
}