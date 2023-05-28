using CuoiKi.Enum;
using System;

namespace CompanyManagement.HelperClasses
{
    public class FilterKey
    {
        public FilterName Name { get; set; }
        public FilterLogicType LogicType { get; set; }
        public FilterKey(FilterName name, FilterLogicType logic)
        {
            Name = name;
            LogicType = logic;
        }
        public bool Equals(FilterKey other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (this.GetHashCode() != other.GetHashCode()) return false;
            System.Diagnostics.Debug.Assert(base.GetType() != typeof(object));
            if (!base.Equals(other)) return false;
            return this.Name == other.Name && this.LogicType == other.LogicType;
        }
        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (this.GetType() != obj.GetType()) return false;
            return Equals((FilterKey)obj);
        }
        public override int GetHashCode()
        {
            return HashCode.Combine(Name.GetHashCode(), LogicType.GetHashCode());
        }
    }
}
