using CuoiKi.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CompanyManagement.HelperClasses
{
    public class FilterChain<T>
    {
        private Dictionary<FilterKey, Predicate<T>> _predicates;
        public FilterChain()
        {
            _predicates = new Dictionary<FilterKey, Predicate<T>>();
        }

        public void AddPredicate(FilterName name, FilterLogicType logic, Predicate<T> predicate)
        {
            FilterKey key = new FilterKey(name, logic);
            _predicates.Add(key, predicate);
        }

        public void RemovePredicate(FilterName keyName)
        {
            // Find all predicates with the given filter key
            var matchingPredicates = _predicates.Where(p => p.Key.Name == keyName).ToList();

            // Remove all matching predicates
            foreach (var predicate in matchingPredicates)
            {
                _predicates.Remove(predicate.Key);
            }
        }
        public IEnumerable<T> ApplyAndLogic(IEnumerable<T> data)
        {
            foreach (var predicate in _predicates.Where(p => p.Key.LogicType == FilterLogicType.And).Select(p => p.Value))
            {
                data = data.Where(new Func<T, bool>(predicate));
            }
            return data;
        }

        public IEnumerable<T> ApplyOrLogic(IEnumerable<T> data)
        {
            IEnumerable<T> result = Enumerable.Empty<T>();
            foreach (var predicate in _predicates.Where(p => p.Key.LogicType == FilterLogicType.Or).Select(p => p.Value))
            {
                result = result.Concat(data.Where(new Func<T, bool>(predicate)));
            }
            return result;
        }
        public IEnumerable<T> ApplyOrThenAnd(IEnumerable<T> data)
        {
            var orResult = ApplyOrLogic(data);
            var andResult = ApplyAndLogic(orResult);
            return andResult;
        }
    }
}
