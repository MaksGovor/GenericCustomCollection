using System;
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
            MySortedList<string, int> nubmers = new MySortedList<string, int>();
            string[] keys = new string[] { "one", "two", "three" };
            int[] values = new int[] { 1, 2, 3 };

            //act
            for (int i = 0; i < keys.Length; i++)
            {
                nubmers.Add(keys[i], values[i]);
            }

            //assert
            for (int i = 0; i < keys.Length; i++)
            {
                Assert.Equal(values[i], nubmers[keys[i]]);
            }
        }

        [Fact]
        public void AddingItems_NotNullKeys_CheckCount()
        {
            //arrange
            MySortedList<string, int> nubmers = new MySortedList<string, int>();
            string[] keys = new string[] { "one", "two", "three" };
            int[] values = new int[] { 1, 2, 3 };
            const int expectedCount = 3;

            //act
            for (int i = 0; i < keys.Length; i++)
            {
                nubmers.Add(keys[i], values[i]);
            }

            //assert
            Assert.Equal(expectedCount, nubmers.Count);
        }

        [Fact]
        public void AddingItems_NotNullKeys_CheckIncreaseCapacity()
        {
            //arrange
            MySortedList<string, int> nubmers = new MySortedList<string, int>();
            string[] keys = new string[] { "one", "two", "three", "four" };
            int[] values = new int[] { 1, 2, 3, 4 };
            const int expectedCapacity = 8;

            //act
            for (int i = 0; i < keys.Length; i++)
            {
                nubmers.Add(keys[i], values[i]);
            }

            //assert
            Assert.Equal(expectedCapacity, nubmers.Capacity);
        }

        [Fact]
        public void AddingItem_NullKey_ThrowsException()
        {
            //arrange
            MySortedList<string, int> nubmers = new MySortedList<string, int>();
            const string expectedExceptionMessage = "Key should be not null";

            //act
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => nubmers.Add(null, 24));

            //assert
            Assert.NotNull(exception);
            Assert.Equal(expectedExceptionMessage, exception.ParamName);
        }

        [Fact]
        public void AddingItem_ExictingKey_ThrowsException()
        {
            //arrange
            MySortedList<string, int> nubmers = new MySortedList<string, int>();
            const string key = "seven";
            string expectedExceptionMessage = $"Key {key} already exists";

            //act
            ArgumentException exception = Assert.Throws<ArgumentException>(() => {
                nubmers.Add(key, 7);
                nubmers.Add(key, 7);
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
            MySortedList<string, int> nubmers = new MySortedList<string, int>(numbersDict);

            //assert
            Assert.NotNull(nubmers);
            Assert.Equal(expectedCount, nubmers.Count);
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
            MySortedList<string, int> nubmers = new MySortedList<string, int>(numbersDict);

            //assert
            Assert.NotNull(nubmers);
            Assert.Equal(expectedCapacity, nubmers.Capacity);
        }

        [Fact]
        public void InitWithCapacity_IntCapacity_CheckCapacity()
        {
            //arrange
            const int expectedCapacity = 8;

            //act
            MySortedList<string, int> nubmers = new MySortedList<string, int>(expectedCapacity);

            //assert
            Assert.NotNull(nubmers);
            Assert.Equal(nubmers.Capacity, expectedCapacity);
        }


        [Fact]
        public void Clear_CheckCount_ThrowsException()
        {
            //arrange
            MySortedList<string, int> nubmers = new MySortedList<string, int>() 
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };

            //act
            nubmers.Clear();
            NullReferenceException exception = Assert.Throws<NullReferenceException>(() => nubmers["one"]);

            //assert
            Assert.NotNull(exception);
            Assert.Empty(nubmers);
            Assert.Equal("MySortedList is empty", exception.Message);
        }

        [Fact]
        public void ContainsKey_NotNullExistingKey_ReturnsTrue()
        {
            //arrange
            MySortedList<string, int> nubmers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };

            //act
            bool containsOne = nubmers.ContainsKey("one");
            bool containsTwo = nubmers.ContainsKey("two");
            bool containsThree = nubmers.ContainsKey("three");

            //assert
            Assert.True(containsOne && containsTwo && containsThree);
        }

        [Fact]
        public void ContainsKey_NotNullNoExistingKey_ReturnsFalse()
        {
            //arrange
            MySortedList<string, int> nubmers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };

            //act
            bool containsFour = nubmers.ContainsKey("four");

            //assert
            Assert.False(containsFour);
        }

        [Fact]
        public void ContainsKey_NullKey_ThrowsException()
        {
            //arrange
            MySortedList<string, int> nubmers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };
            const string expectedExceptionMessage = "Key should be not null";

            //act
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => nubmers.ContainsKey(null));

            //assert
            Assert.NotNull(exception);
            Assert.Equal(expectedExceptionMessage, exception.ParamName);
        }

        [Fact]
        public void ContainsValue_ExistingComparableValue_ReturnsTrue()
        {
            //arrange
            MySortedList<string, int> nubmers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };

            //act
            bool containsOne = nubmers.ContainsValue(1);
            bool containsTwo = nubmers.ContainsValue(2);
            bool containsThree = nubmers.ContainsValue(3);

            //assert
            Assert.True(containsOne && containsTwo && containsThree);
        }

        [Fact]
        public void ContainsValue_NoExistingComparableValue_ReturnsFalse()
        {
            //arrange
            MySortedList<string, int> nubmers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };

            //act
            bool containsFour = nubmers.ContainsValue(4);


            //assert
            Assert.False(containsFour);
        }

        [Fact]
        public void ContainsValue_ExistingNoComparableValue_ReturnsTrueByLink()
        {
            //arrange
            Box<int> one = new Box<int>(1);
            MySortedList<string, Box<int>> nubmers = new MySortedList<string, Box<int>>()
            {
                { "one",   one },
                { "two",   new Box<int>(2) },
                { "three", new Box<int>(3) },
            };

            //act
            bool containsOneByLink = nubmers.ContainsValue(one);
            bool containsOne = nubmers.ContainsValue(new Box<int>(1));

            //assert
            Assert.True(containsOneByLink);
            Assert.False(containsOne);
        }

        [Fact]
        public void ContainsValue_ExistingNullValue_ReturnsTrue()
        {
            //arrange
            MySortedList<int, string> nubmers = new MySortedList<int, string>()
            {
                { 1, "one" },
                { 2, "two" },
                { 3, "three" },
            };

            nubmers.Add(123, null);
            //act
            bool containsNull = nubmers.ContainsValue(null);

            //assert
            Assert.True(containsNull);
        }
    }
}
