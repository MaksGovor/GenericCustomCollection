using System;
using System.Collections;
using System.Collections.Generic;
using CustomSortedList;
using Xunit;

namespace CustomSortedList.Test
{
    public class CustromSortedListTest
    {
        private class Box<T> {
            public T Value;

            public Box(T value) {
                Value = value;
            }
        }

        [Fact]
        public void AddingItems_NotNullKeys_CheckKeyValueMatching()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>();
            string[] keys = new string[] { "one", "two", "three" };
            int[] values = new int[] { 1, 2, 3 };

            //act
            for (int i = 0; i < keys.Length; i++)
            {
                numbers.Add(keys[i], values[i]);
            }

            //assert
            for (int i = 0; i < keys.Length; i++)
            {
                Assert.Equal(values[i], numbers[keys[i]]);
            }
        }

        [Fact]
        public void AddingItems_NotNullKeys_CheckCount()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>();
            string[] keys = new string[] { "one", "two", "three" };
            int[] values = new int[] { 1, 2, 3 };
            const int expectedCount = 3;

            //act
            for (int i = 0; i < keys.Length; i++)
            {
                numbers.Add(keys[i], values[i]);
            }

            //assert
            Assert.Equal(expectedCount, numbers.Count);
        }

        [Fact]
        public void AddingItems_NotNullKeys_CheckIncreaseCapacity()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>();
            string[] keys = new string[] { "one", "two", "three", "four" };
            int[] values = new int[] { 1, 2, 3, 4 };
            const int expectedCapacity = 8;

            //act
            for (int i = 0; i < keys.Length; i++)
            {
                numbers.Add(keys[i], values[i]);
            }

            //assert
            Assert.Equal(expectedCapacity, numbers.Capacity);
        }

        [Fact]
        public void AddingItem_NullKey_ThrowsArgumentNullException()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>();
            const string expectedExceptionMessage = "Key should be not null";

            //act
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => numbers.Add(null, 24));

            //assert
            Assert.NotNull(exception);
            Assert.Equal(expectedExceptionMessage, exception.ParamName);
        }

        [Fact]
        public void AddingItem_ExictingKey_ThrowsArgumentException()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>();
            const string key = "seven";
            string expectedExceptionMessage = $"Key {key} already exists";

            //act
            ArgumentException exception = Assert.Throws<ArgumentException>(() => {
                numbers.Add(key, 7);
                numbers.Add(key, 7);
            });
            
            //assert
            Assert.NotNull(exception);
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Fact]
        public void InitByDict_NotNullKeys_CheckCount()
        {
            //arrange
            Dictionary<string, int> numbersDict = new Dictionary<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };
            const int expectedCount = 3;

            //act
            MySortedList<string, int> numbers = new MySortedList<string, int>(numbersDict);

            //assert
            Assert.NotNull(numbers);
            Assert.Equal(expectedCount, numbers.Count);
        }

        [Fact]
        public void InitByDict_NotNullKeys_CheckIncreaseCapacity()
        {
            //arrange
            Dictionary<string, int> numbersDict = new Dictionary<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4},
            };
            const int expectedCapacity = 8;

            //act
            MySortedList<string, int> numbers = new MySortedList<string, int>(numbersDict);

            //assert
            Assert.NotNull(numbers);
            Assert.Equal(expectedCapacity, numbers.Capacity);
        }

        [Fact]
        public void InitWithCapacity_IntCapacity_CheckCapacity()
        {
            //arrange
            const int expectedCapacity = 8;

            //act
            MySortedList<string, int> numbers = new MySortedList<string, int>(expectedCapacity);

            //assert
            Assert.NotNull(numbers);
            Assert.Equal(numbers.Capacity, expectedCapacity);
        }


        [Fact]
        public void Clear_CheckCount_ThrowsNullReferenceException()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>() 
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };

            //act
            numbers.Clear();
            NullReferenceException exception = Assert.Throws<NullReferenceException>(() => numbers["one"]);

            //assert
            Assert.NotNull(exception);
            Assert.Empty(numbers);
            Assert.Equal("MySortedList is empty", exception.Message);
        }

        [Fact]
        public void ContainsKey_NotNullExistingKey_ReturnsTrue()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };

            //act
            bool containsOne = numbers.ContainsKey("one");
            bool containsTwo = numbers.ContainsKey("two");
            bool containsThree = numbers.ContainsKey("three");

            //assert
            Assert.True(containsOne && containsTwo && containsThree);
        }

        [Fact]
        public void ContainsKey_NotNullNoExistingKey_ReturnsFalse()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };

            //act
            bool containsFour = numbers.ContainsKey("four");

            //assert
            Assert.False(containsFour);
        }

        [Fact]
        public void ContainsKey_NullKey_ThrowsArgumentNullException()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };
            const string expectedExceptionMessage = "Key should be not null";

            //act
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => numbers.ContainsKey(null));

            //assert
            Assert.NotNull(exception);
            Assert.Equal(expectedExceptionMessage, exception.ParamName);
        }

        [Fact]
        public void ContainsValue_ExistingComparableValue_ReturnsTrue()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };

            //act
            bool containsOne = numbers.ContainsValue(1);
            bool containsTwo = numbers.ContainsValue(2);
            bool containsThree = numbers.ContainsValue(3);

            //assert
            Assert.True(containsOne && containsTwo && containsThree);
        }

        [Fact]
        public void ContainsValue_NoExistingComparableValue_ReturnsFalse()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };

            //act
            bool containsFour = numbers.ContainsValue(4);


            //assert
            Assert.False(containsFour);
        }

        [Fact]
        public void ContainsValue_ExistingNoComparableValue_ReturnsTrueByLink()
        {
            //arrange
            Box<int> one = new Box<int>(1);
            MySortedList<string, Box<int>> numbers = new MySortedList<string, Box<int>>()
            {
                { "one",   one },
                { "two",   new Box<int>(2) },
                { "three", new Box<int>(3) },
            };

            //act
            bool containsOneByLink = numbers.ContainsValue(one);
            bool containsOne = numbers.ContainsValue(new Box<int>(1));

            //assert
            Assert.True(containsOneByLink);
            Assert.False(containsOne);
        }

        [Fact]
        public void ContainsValue_ExistingNullValue_ReturnsTrue()
        {
            //arrange
            MySortedList<int, string> numbers = new MySortedList<int, string>()
            {
                { 1, "one" },
                { 2, "two" },
                { 3, "three" },
            };

            numbers.Add(123, null);
            //act
            bool containsNull = numbers.ContainsValue(null);

            //assert
            Assert.True(containsNull);
        }

        [Fact]
        public void GetEnumerator_SortedList_ReturnsCorrectEnumeratorObject()
        {
            //arrange
            MySortedList<int, string> numbersExpected = new MySortedList<int, string>()
            {
                { 1, "one" },
                { 2, "two" },
                { 3, "three" },
            };
            MySortedList<int, string> numbersActual = new MySortedList<int, string>();
            MySortedList<int, string> numbersActualForEach = new MySortedList<int, string>();
            IEnumerator<KeyValuePair<int, string>> enumerator = numbersExpected.GetEnumerator();
            //act
            while (enumerator.MoveNext())
            {
                numbersActual.Add(enumerator.Current.Key, enumerator.Current.Value);
            }

            foreach (KeyValuePair<int, string> kvp in numbersExpected)
                numbersActualForEach.Add(kvp.Key, kvp.Value);

            //assert
            Assert.Equal(numbersExpected, numbersActual);
            Assert.Equal(numbersExpected, numbersActualForEach);
        }

        [Fact]
        public void IndexOfKey_NotNullExistingKey_ReturnsIndex()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };

            //act
            int indexOfKeyOne = numbers.IndexOfKey("one");
            int indexOfKeyTwo = numbers.IndexOfKey("two");
            int indexOfKeyThree = numbers.IndexOfKey("three");

            //assert
            Assert.Equal(0, indexOfKeyOne);
            Assert.Equal(2, indexOfKeyTwo);
            Assert.Equal(1, indexOfKeyThree);
        }

        [Fact]
        public void IndexOfKey_NullKey_ThrowsArgumentNullException()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };
            const string expectedExceptionMessage = "Key should be not null";

            //act
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => numbers.IndexOfKey(null));

            //assert
            Assert.NotNull(exception);
            Assert.Equal(expectedExceptionMessage, exception.ParamName);
        }

        [Fact]
        public void IndexOfKey_NotNullExistingKey_ReturnsMinus1()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };

            //act
            int indexOfKeyFour = numbers.IndexOfKey("four");

            //assert
            Assert.Equal(-1, indexOfKeyFour);
        }

        [Fact]
        public void IndexOfValue_NotNullExistingComparableValue_ReturnsIndex()
        {
            //arrange
            MySortedList<int, string> numbers = new MySortedList<int, string>()
            {
                { 1, "one" },
                { 2, "two" },
                { 3, "three" },
            };

            //act
            int indexOfValueOne = numbers.IndexOfValue("one");
            int indexOfValueTwo = numbers.IndexOfValue("two");
            int indexOfValueThree = numbers.IndexOfValue("three");

            //assert
            Assert.Equal(0, indexOfValueOne);
            Assert.Equal(1, indexOfValueTwo);
            Assert.Equal(2, indexOfValueThree);
        }

        [Fact]
        public void IndexOfValue_NotNullExistingNoComparableValue_ReturnsIndexByLink()
        {
            //arrange
            Box<int> one = new Box<int>(1);
            Box<int> two = new Box<int>(2);
            Box<int> three = new Box<int>(3);
            MySortedList<string, Box<int>> numbers = new MySortedList<string, Box<int>>()
            {
                { "one",   one },
                { "two",   two },
                { "three", three },
            };

            //act
            int indexOfValueOne = numbers.IndexOfValue(one);
            int indexOfValueTwo = numbers.IndexOfValue(two);
            int indexOfValueThree = numbers.IndexOfValue(three);

            //assert
            Assert.Equal(0, indexOfValueOne);
            Assert.Equal(2, indexOfValueTwo);
            Assert.Equal(1, indexOfValueThree);
        }

        [Fact]
        public void IndexOfValue_ExistingNullValue_ReturnsFirstIndex()
        {
            //arrange
            MySortedList<int, string> numbers = new MySortedList<int, string>()
            {
                { 1, "one" },
                { 2, "two" },
                { 7, "seven" },
            };

            numbers.Add(4, null);
            numbers.Add(5, null);
            //act
            int indexOfValueNull = numbers.IndexOfValue(null);

            //assert
            Assert.Equal(2, indexOfValueNull);
        }

        [Fact]
        public void IndexOfValue_NotNullNoExistingComparableValue_ReturnsMinus1()
        {
            //arrange
            MySortedList<int, string> numbers = new MySortedList<int, string>()
            {
                { 1, "one" },
                { 2, "two" },
                { 7, "seven" },
            };

            //act
            int indexOfValueNull = numbers.IndexOfValue("four");

            //assert
            Assert.Equal(-1, indexOfValueNull);
        }



    }
}
