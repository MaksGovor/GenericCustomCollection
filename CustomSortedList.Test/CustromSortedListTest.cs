using System;
using CustomSortedList;
using Xunit;

namespace CustomSortedList.Test
{
    public class CustromSortedListTest
    {
        [Fact]
        public void AddingKeysAndValueMatching()
        {
            MySortedList<string, int> nubmers = new MySortedList<string, int>();
            string[] keys = new string[] { "one", "two", "three" };
            int[] values = new int[] { 1, 2, 3};

            for (int i = 0; i < keys.Length; i++)
            {
                string key = keys[i];
                int value = values[i];
                nubmers.Add(key, value);
                Assert.Equal(nubmers[key], value);
            }
        }
    }
}
