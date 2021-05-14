using System;

namespace AppEx.Core.Models
{
    [Flags]
    public enum Lifetime
    {
        Transient,
        Scoped,
        Singleton
    }
}
