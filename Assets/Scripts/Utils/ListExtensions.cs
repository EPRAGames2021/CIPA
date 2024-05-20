using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EPRA.Utilities
{
    public static class ListExtensions
    {
        public static bool HaveSameElements<T>(List<T> list1, List<T> list2)
        {
            var set1 = new HashSet<T>(list1);
            var set2 = new HashSet<T>(list2);

            return set1.SetEquals(set2);
        }
    }
}
