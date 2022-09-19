using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Blazor.UI.Services.Exstensions
{
    internal sealed class NamedType : TypeDelegator
    {
        object name;
        public NamedType(object name)
        {
            this.name = name;
        }
        public NamedType(object name, Type delegatingType) : base(delegatingType)
        {
            this.name = name;
        }


        public override string Name { get => (name?.GetType() ?? typeof(NamedType)).Name; }
        public override Guid GUID { get; } = Guid.NewGuid();
        public override bool Equals(object obj) => Equals(obj as NamedType);
        public override int GetHashCode() => name.GetHashCode();
        public override bool Equals(Type o) => o is NamedType t && object.Equals(t.name, this.name) && (t.ServiceType == null || ServiceType == null || t.ServiceType == ServiceType);
        public override string FullName => (name?.GetType() ?? typeof(NamedType)).FullName;

        public Type ServiceType => typeImpl;
    }
}
