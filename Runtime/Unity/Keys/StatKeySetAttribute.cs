using System;

namespace Deucarian.GameplayFoundation
{
    /// <summary>Marks an authoritative set of named StatKey fields or properties for the Inspector.</summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public sealed class StatKeySetAttribute : Attribute { }
}
