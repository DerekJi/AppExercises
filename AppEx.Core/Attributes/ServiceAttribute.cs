using AppEx.Core.Models;
using System;

namespace AppEx.Core.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ServiceAttribute : Attribute
    {
        public ServiceAttribute()
        {
        }

        public ServiceAttribute(Type serviceType)
        {
            ServiceType = serviceType;
        }

        public Lifetime Lifetime { get; set; } = Lifetime.Scoped;
        public Type ServiceType { get; set; }
    }
}
