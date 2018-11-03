using System;
using System.Collections.Generic;
using System.Linq;
using System.Dynamic;

namespace DynamicObjectStudy
{
    public class DynamicDataRecord : DynamicObject  
    {
        private readonly IDictionary<string, object> dictionary;

        public DynamicDataRecord(object _object)
        {
            dictionary = _object.GetType()
                .GetProperties()
                .Where(x => x.CanRead)
                .ToDictionary(x => x.Name, x => x.GetValue(_object));
        }

        public DynamicDataRecord(IDictionary<string, object> dictionary)
            => this.dictionary = dictionary;

        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            if (dictionary == null) return false;
            var index = indexes[0];
            if (index is string)
            {
                return TrySetValue((string)index, value);
            }
            if (index is int)
            {
                var elem = dictionary.ElementAtOrDefault((int)index);
                if (CheckValueType(value.GetType(), elem.Value.GetType()))
                {
                    dictionary[elem.Key] = value;
                    return true;
                }
            }
            return false;
        }

        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            if (dictionary == null) return false;
            return TrySetValue(binder.Name, value);
        }

        private bool TrySetValue(string key, object value)
        {
            if (!dictionary.TryGetValue(key, out var result)) return false;
            if (CheckValueType(value.GetType(), result.GetType()))
            {
                dictionary[key] = value;
                return true;
            }
            return false;
        }

        private bool CheckValueType(Type valueType, Type baseType)
            => valueType.Equals(baseType) || valueType.IsSubclassOf(baseType);

        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            var index = indexes[0];
            if (index is string) return dictionary.TryGetValue((string)index, out result);
            else if (index is int) result = dictionary.ElementAtOrDefault((int)index).Value;
            else result = null;
            return result != null;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
            => dictionary.TryGetValue(binder.Name, out result);
        

        public override IEnumerable<string> GetDynamicMemberNames()
            => dictionary.Keys;
    }
}
