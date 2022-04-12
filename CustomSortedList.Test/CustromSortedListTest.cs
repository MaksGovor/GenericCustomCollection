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

        public class ReverseIntComparer : IComparer<int>
        {
            public int Compare(int value1, int value2)
            {
                return -(value1.CompareTo(value2));
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
        public void Init_NotNullKeys_CheckRightSeq()
        {
            //arrange
            const int expectedCount = 3;

            //act
            MySortedList<int, int> numbers = new MySortedList<int, int>()
            {
                { 12, 1 },
                { 3, 2 },
                { 14, 3 },
            };

            bool rightSeq = numbers.IndexOfKey(3) < numbers.IndexOfKey(12) && numbers.IndexOfKey(12) < numbers.IndexOfKey(14);
            
            //assert
            Assert.NotNull(numbers);
            Assert.True(rightSeq);
            Assert.Equal(expectedCount, numbers.Count);
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
        public void InitByComparer_ValidComparer_CheckCorrectSequencing()
        {
            //arrange
            IComparer<int> reverseComparer = new ReverseIntComparer();
            int[] keysReverseSort = new int[] { 4, 3, 2, 1 };
            string[] valuesReverseSort = new string[] { "four", "three", "two", "one" };

            //act
            MySortedList<int, string> numbers = new MySortedList<int, string>(reverseComparer);
            numbers.Add(2, "two");
            numbers.Add(4, "four");
            numbers.Add(1, "one");
            numbers.Add(3, "three");

            //assert
            Assert.Equal(reverseComparer, numbers.Comparer);
            for (int i = 0; i < numbers.Count; i++)
            {
                Assert.Equal(numbers.GetKey(i), keysReverseSort[i]);
                Assert.Equal(numbers.GetByIndex(i), valuesReverseSort[i]);
            }
        }

        [Fact]
        public void InitByComparerAndCapacity_ValidComparer_CheckCorrectSetting()
        {
            //arrange
            IComparer<int> reverseComparer = new ReverseIntComparer();
            const int expectedCapacity = 64;

            //act
            MySortedList<int, string> numbers = new MySortedList<int, string>(reverseComparer, expectedCapacity);

            //assert
            Assert.Equal(reverseComparer, numbers.Comparer);
            Assert.Equal(expectedCapacity, numbers.Capacity);
        }

        [Fact]
        public void InitByComparerAndDictionary_ValidComparerNotNullKeys_CheckCorrectSequencing()
        {
            //arrange
            IComparer<int> reverseComparer = new ReverseIntComparer();
            Dictionary<int, string> numbersDict = new Dictionary<int, string>()
            {
                { 2, "two" },
                { 4, "four" },
                { 1, "one" },
                { 3, "three" },
            };
            int[] keysReverseSort = new int[] { 4, 3, 2, 1 };
            string[] valuesReverseSort = new string[] { "four", "three", "two", "one" };

            //act
            MySortedList<int, string> numbers = new MySortedList<int, string>(reverseComparer, numbersDict);

            //assert
            Assert.Equal(reverseComparer, numbers.Comparer);
            for (int i = 0; i < numbers.Count; i++)
            {
                Assert.Equal(numbers.GetKey(i), keysReverseSort[i]);
                Assert.Equal(numbers.GetByIndex(i), valuesReverseSort[i]);
            }
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
        public void SetCapacity_ValidValue()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 },
            };
            const int expectedCapacityBeforeSet = 8;
            const int expectedCapacityAfterSet = 64;

            //act
            int capacityBeforeSet = numbers.Capacity;
            numbers.Capacity = expectedCapacityAfterSet;
            int capacityAfterSet = numbers.Capacity;

            //assert
            Assert.Equal(expectedCapacityBeforeSet, capacityBeforeSet);
            Assert.Equal(expectedCapacityAfterSet, capacityAfterSet);
        }

        [Fact]
        public void SetCapacity_NoValidValue_ThrowsArgumentOutOfRangeException()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 },
            };
            const int expectedCapacityBeforeSet = 8;
            const string expectedExceptionMessage = "Capacity must be greater than Count";

            //act
            int capacityBeforeSet = numbers.Capacity;
            ArgumentOutOfRangeException exception1 = Assert.Throws<ArgumentOutOfRangeException>(() => numbers.Capacity = 4);
            int capacityAfterSet = numbers.Capacity;

            //assert
            Assert.Equal(expectedCapacityBeforeSet, capacityBeforeSet);
            Assert.Equal(expectedCapacityBeforeSet, capacityAfterSet);
            Assert.NotNull(exception1);
            Assert.Equal(expectedExceptionMessage, exception1.ParamName);
        }

        [Fact]
        public void GetAndSetValueByKey_NullKey_ThrowsArgumentNullException()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>();
            const string expectedExceptionMessage = "Key should be not null";

            //act
            ArgumentNullException exception1 = Assert.Throws<ArgumentNullException>(() => numbers[null]);
            ArgumentNullException exception2 = Assert.Throws<ArgumentNullException>(() => numbers[null] = 5);

            //assert
            Assert.NotNull(exception1);
            Assert.NotNull(exception2);
            Assert.Equal(expectedExceptionMessage, exception1.ParamName);
            Assert.Equal(expectedExceptionMessage, exception2.ParamName);
        }

        [Fact]
        public void GetValueByKey_NotNullNoExistingKey_ThrowsKeyNotFoundException()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>();
            const string key = "ten";
            string expectedExceptionMessage = $"Key {key} does not exists";

            //act
            KeyNotFoundException exception = Assert.Throws<KeyNotFoundException>(() => numbers[key]);

            //assert
            Assert.NotNull(exception);
            Assert.Equal(expectedExceptionMessage, exception.Message);
        }

        [Fact]
        public void SetValueByKey_NotNullNoExistingKey_AddNewItem()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>();
            const string key = "ten";

            //act
            numbers[key] = 10;

            //assert
            Assert.Single(numbers);
            Assert.Equal(10, numbers[key]);
        }

        [Fact]
        public void GetAndSetValueByKey_NotNullExistingKey_ThrowsArgumentNullException()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>() 
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "five", 4}
            };
            const int expectedOldValue = 4;
            const int expectedNewValue = 5;

            //act
            int oldValue = numbers["five"];
            numbers["five"] = expectedNewValue;
            int newValue = numbers["five"];

            //assert
            Assert.Equal(expectedOldValue, oldValue);
            Assert.Equal(expectedNewValue, newValue);
        }

        [Fact]
        public void GetKeysAndValueList_NoSupportedOperations_ThrowsNotSupportedException()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "five", 4}
            };
            const string expectedExceptionMessage = "This operation is not supported";

            //act
            IList<string> keys = numbers.Keys;
            IList<int> values = numbers.Values;
            NotSupportedException exceptionKeys1 = Assert.Throws<NotSupportedException>(() => keys.Add(""));
            NotSupportedException exceptionKeys2 = Assert.Throws<NotSupportedException>(() => keys.Clear());
            NotSupportedException exceptionKeys3 = Assert.Throws<NotSupportedException>(() => keys.Insert(1, ""));
            NotSupportedException exceptionKeys4 = Assert.Throws<NotSupportedException>(() => keys[1] = "");
            NotSupportedException exceptionKeys5 = Assert.Throws<NotSupportedException>(() => keys.Remove(""));
            NotSupportedException exceptionKeys6 = Assert.Throws<NotSupportedException>(() => keys.RemoveAt(1));

            NotSupportedException exceptionValues1 = Assert.Throws<NotSupportedException>(() => values.Add(1));
            NotSupportedException exceptionValues2 = Assert.Throws<NotSupportedException>(() => values.Clear());
            NotSupportedException exceptionValues3 = Assert.Throws<NotSupportedException>(() => values.Insert(1, 1));
            NotSupportedException exceptionValues4 = Assert.Throws<NotSupportedException>(() => values[1] = 1);
            NotSupportedException exceptionValues5 = Assert.Throws<NotSupportedException>(() => values.Remove(1));
            NotSupportedException exceptionValues6 = Assert.Throws<NotSupportedException>(() => values.RemoveAt(1));

            //assert
            Assert.Equal(expectedExceptionMessage, exceptionKeys1.Message);
            Assert.Equal(expectedExceptionMessage, exceptionKeys2.Message);
            Assert.Equal(expectedExceptionMessage, exceptionKeys3.Message);
            Assert.Equal(expectedExceptionMessage, exceptionKeys4.Message);
            Assert.Equal(expectedExceptionMessage, exceptionKeys5.Message);
            Assert.Equal(expectedExceptionMessage, exceptionKeys6.Message);
            Assert.Equal(expectedExceptionMessage, exceptionValues1.Message);
            Assert.Equal(expectedExceptionMessage, exceptionValues2.Message);
            Assert.Equal(expectedExceptionMessage, exceptionValues3.Message);
            Assert.Equal(expectedExceptionMessage, exceptionValues4.Message);
            Assert.Equal(expectedExceptionMessage, exceptionValues5.Message);
            Assert.Equal(expectedExceptionMessage, exceptionValues6.Message);
        }

        [Fact]
        public void GetKeysAndValueList_IsReadOnly_ReturnsFalse()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "five", 4}
            };

            //act
            IList<string> keys = numbers.Keys;
            IList<int> values = numbers.Values;

            //assert
            Assert.True(keys.IsReadOnly);
            Assert.True(values.IsReadOnly);
        }

        [Fact]
        public void GetKeysAndValueList_Count_EqualToCountOfSortedList()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "five", 4}
            };

            //act
            IList<string> keys = numbers.Keys;
            IList<int> values = numbers.Values;

            //assert
            Assert.Equal(numbers.Count, keys.Count);
            Assert.Equal(numbers.Count, values.Count);
        }

        [Fact]
        public void GetKeysAndValueList_GetKeyAndValueByIndex_EqualToSortedListPair()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };

            //act
            IList<string> keys = numbers.Keys;
            IList<int> values = numbers.Values;

            //assert
            foreach (KeyValuePair<string, int> kvp in numbers)
            {
                Assert.Equal(keys[numbers.IndexOfKey(kvp.Key)], kvp.Key);
                Assert.Equal(values[numbers.IndexOfValue(kvp.Value)], kvp.Value);
            }
        }

        [Fact]
        public void GetKeysAndValueList_TestEnumerator()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };
            int keysAcc = 0;
            int valuesAcc = 0;

            //act
            IList<string> keys = numbers.Keys;
            IList<int> values = numbers.Values;

            //assert
            foreach (string key in keys)
            {
                Assert.Equal(key, keys[keysAcc]);
                keysAcc++;
            }

            foreach (int value in values)
            {
                Assert.Equal(value, values[valuesAcc]);
                valuesAcc++;
            }

            Assert.Equal(numbers.Count, keysAcc);
            Assert.Equal(numbers.Count, valuesAcc);
        }

        [Fact]
        public void GetKeysAndValueList_CopyTo_ValidArraySize()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };
            IList<string> keys = numbers.Keys;
            IList<int> values = numbers.Values;

            string[] keysArr = new string[3];
            int[] valuesArr = new int[3];

            //act
            keys.CopyTo(keysArr, 0);
            values.CopyTo(valuesArr, 0);

            //assert
            Assert.Equal(keys, keysArr);
            Assert.Equal(values, valuesArr);
        }

        [Fact]
        public void GetKeysAndValueList_CopyTo_NonValidStartIndex_ThrowsArgumentOutOfRangeException()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };
            const string expectedExceptionMessage = "Number was less than the array's lower bound in the first dimension";

            IList<string> keys = numbers.Keys;
            IList<int> values = numbers.Values;

            string[] keysArr = new string[3];
            int[] valuesArr = new int[3];

            //act
            ArgumentOutOfRangeException exception1 = Assert.Throws<ArgumentOutOfRangeException>(() => keys.CopyTo(keysArr, -1));
            ArgumentOutOfRangeException exception2 = Assert.Throws<ArgumentOutOfRangeException>(() => values.CopyTo(valuesArr, -1));

            //assert
            Assert.NotNull(exception1);
            Assert.NotNull(exception2);
            Assert.Equal(expectedExceptionMessage, exception1.ParamName);
            Assert.Equal(expectedExceptionMessage, exception2.ParamName);
        }

        [Fact]
        public void GetKeysAndValueList_CopyTo_SmallArraySizeFromIndex_ThrowsArgumentException()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };
            const string expectedExceptionMessage = "Destination array was not long enough";

            IList<string> keys = numbers.Keys;
            IList<int> values = numbers.Values;

            string[] keysArr = new string[3];
            int[] valuesArr = new int[3];

            //act
            ArgumentException exception1 = Assert.Throws<ArgumentException>(() => keys.CopyTo(keysArr, 1));
            ArgumentException exception2 = Assert.Throws<ArgumentException>(() => values.CopyTo(valuesArr, 1));

            //assert
            Assert.NotNull(exception1);
            Assert.NotNull(exception2);
            Assert.Equal(expectedExceptionMessage, exception1.Message);
            Assert.Equal(expectedExceptionMessage, exception2.Message);
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
            const string key = "one";
            string expectedExceptionMessage = $"Key {key} does not exists";

            //act
            numbers.Clear();
            KeyNotFoundException exception = Assert.Throws<KeyNotFoundException>(() => numbers["one"]);

            //assert
            Assert.NotNull(exception);
            Assert.Empty(numbers);
            Assert.Equal(expectedExceptionMessage, exception.Message);
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

        [Fact]
        public void GetKey_ValidIndex_ReturnsKey()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };

            //act
            string key0 = numbers.GetKey(0);
            string key1 = numbers.GetKey(1);
            string key2 = numbers.GetKey(2);

            //assert
            Assert.Equal("one", key0);
            Assert.Equal("three", key1);
            Assert.Equal("two", key2);
        }

        [Fact]
        public void GetKey_NoValidIndex_ThrowsArgumentOutOfRangeException()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };
            const string expectedExceptionMessage = "Index out of range";

            //act
            ArgumentOutOfRangeException exception1 = Assert.Throws<ArgumentOutOfRangeException>(() => numbers.GetKey(-1));
            ArgumentOutOfRangeException exception2 = Assert.Throws<ArgumentOutOfRangeException>(() => numbers.GetKey(3));

            //assert
            Assert.NotNull(exception1);
            Assert.NotNull(exception2);
            Assert.Equal(expectedExceptionMessage, exception1.ParamName);
            Assert.Equal(expectedExceptionMessage, exception2.ParamName);
        }


        [Fact]
        public void GetByIndex_ValidIndex_ReturnsValue()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };

            //act
            int value0 = numbers.GetByIndex(0);
            int value1 = numbers.GetByIndex(1);
            int value2 = numbers.GetByIndex(2);

            //assert
            Assert.Equal(1, value0);
            Assert.Equal(3, value1);
            Assert.Equal(2, value2);
        }

        [Fact]
        public void GetByIndex_NoValidIndex_ThrowsArgumentOutOfRangeException()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };
            const string expectedExceptionMessage = "Index out of range";

            //act
            ArgumentOutOfRangeException exception1 = Assert.Throws<ArgumentOutOfRangeException>(() => numbers.GetByIndex(-1));
            ArgumentOutOfRangeException exception2 = Assert.Throws<ArgumentOutOfRangeException>(() => numbers.GetByIndex(3));

            //assert
            Assert.NotNull(exception1);
            Assert.NotNull(exception2);
            Assert.Equal(expectedExceptionMessage, exception1.ParamName);
            Assert.Equal(expectedExceptionMessage, exception2.ParamName);
        }

        [Fact]
        public void SetByIndex_ValidIndex_SetCorrectValueByIndex()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };
            const int newValueForIndex0 = 11;
            const int newValueForIndex1 = 22;

            //act
            numbers.SetByIndex(0, newValueForIndex0);
            numbers.SetByIndex(1, newValueForIndex1);
            int value0 = numbers.GetByIndex(0);
            int value1 = numbers.GetByIndex(1);

            //assert
            Assert.Equal(newValueForIndex0, value0);
            Assert.Equal(newValueForIndex1, value1);
        }

        [Fact]
        public void SetByIndex_NoValidIndex_ThrowsArgumentOutOfRangeException()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };
            const string expectedExceptionMessage = "Index out of range";

            //act
            ArgumentOutOfRangeException exception1 = Assert.Throws<ArgumentOutOfRangeException>(() => numbers.SetByIndex(-1, 212));
            ArgumentOutOfRangeException exception2 = Assert.Throws<ArgumentOutOfRangeException>(() => numbers.SetByIndex(3, 212));

            //assert
            Assert.NotNull(exception1);
            Assert.NotNull(exception2);
            Assert.Equal(expectedExceptionMessage, exception1.ParamName);
            Assert.Equal(expectedExceptionMessage, exception2.ParamName);
        }

        [Fact]
        public void Remove_NonNullExistingKey_ReturnsTrue()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 },
            };
            const int expectedCountAfterRemove = 3;

            //act
            bool removedOne = numbers.Remove("two");
            bool removedFive = numbers.Remove("five");

            //assert
            Assert.True(removedOne && removedFive);
            Assert.Equal(expectedCountAfterRemove, numbers.Count);
        }

        [Fact]
        public void Remove_NonNullNotExistingKey_ReturnsFalse()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 },
            };
            const int expectedCountAfterRemove = 4;

            //act
            numbers.Remove("two");
            bool removedOne = numbers.Remove("two");
            bool removedFive = numbers.Remove("six");

            //assert
            Assert.False(removedOne || removedFive);
            Assert.Equal(expectedCountAfterRemove, numbers.Count);
        }

        [Fact]
        public void Remove_NullKey_ThrowsArgumentNullException()
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
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => numbers.Remove(null));

            //assert
            Assert.NotNull(exception);
            Assert.Equal(expectedExceptionMessage, exception.ParamName);
        }

        [Fact]
        public void RemoveAt_ValidIndex_RemoveItems()
        {
            //arrange
            MySortedList<int, string> numbers = new MySortedList<int, string>()
            {
                { 1, "one" },
                { 2, "two" },
                { 7, "seven" },
            };
            const int expectedCountAfterRemove = 2;

            //act
            numbers.RemoveAt(0); // {1, "one"}
            bool containsOne = numbers.ContainsKey(1);

            //assert
            Assert.Equal(expectedCountAfterRemove, numbers.Count);
            Assert.False(containsOne);
        }

        [Fact]
        public void RemoveAt_NoValidIndex_RemoveItems()
        {
            //arrange
            MySortedList<int, string> numbers = new MySortedList<int, string>()
            {
                { 1, "one" },
                { 2, "two" },
                { 7, "seven" },
            };
            const int expectedCountAfterRemove = 3;
            const string expectedExceptionMessage = "Index out of range";

            //act
            ArgumentOutOfRangeException exception1 = Assert.Throws<ArgumentOutOfRangeException>(() => numbers.RemoveAt(-1));
            ArgumentOutOfRangeException exception2 = Assert.Throws<ArgumentOutOfRangeException>(() => numbers.RemoveAt(3));

            //assert
            Assert.Equal(expectedCountAfterRemove, numbers.Count);
            Assert.NotNull(exception1);
            Assert.NotNull(exception2);
            Assert.Equal(expectedExceptionMessage, exception1.ParamName);
            Assert.Equal(expectedExceptionMessage, exception2.ParamName);
        }

        [Fact]
        public void IsEmpty_EmptyAndNonEmptySL_ReturnsBool()
        {
            //arrange
            MySortedList<int, string> numbers = new MySortedList<int, string>();

            //act
            bool isEmptyBeforeAdd = numbers.IsEmpty();
            numbers.Add(1, "one");
            bool isEmptyAfterAdd = numbers.IsEmpty();
            numbers.Remove(1);
            bool isEmptyAfterRemoveLast = numbers.IsEmpty();
            numbers.Add(1, "one");
            numbers.Add(2, "two");
            numbers.Clear();
            bool isEmptyAfterClear = numbers.IsEmpty();


            //assert
            Assert.True(isEmptyBeforeAdd);
            Assert.False(isEmptyAfterAdd);
            Assert.True(isEmptyAfterRemoveLast);
            Assert.True(isEmptyAfterClear);
        }

        [Fact]
        public void Clone_MySortedList_ReturnsEqualList()
        {
            //arrange
            MySortedList<int, string> numbers = new MySortedList<int, string>()
            {
                { 1, "one" },
                { 2, "two" },
                { 7, "seven" },
            };
            int numbersHash = numbers.GetHashCode();

            //act
            MySortedList<int, string> numbersClone = numbers.Clone();
            int numbersCloneHash = numbersClone.GetHashCode();

            //assert
            Assert.Equal(numbers, numbersClone);
            Assert.NotEqual(numbersHash, numbersCloneHash);
        }

        [Fact]
        public void TrimExcess_CountDivideCapacityLessThan90Percent()
        {
            //arrange
            MySortedList<int, string> numbers = new MySortedList<int, string>()
            {
                { 1, "one" },
                { 2, "two" },
                { 7, "seven" },
                { 5, "five" },
                { 4, "four" },
            };
            const int expectedCapacityBeforeTrim = 8;
            const int expectedCapacityAfterTrim = 5;

            //act
            int capacityBeforeTrim = numbers.Capacity;
            numbers.TrimExcess();
            int capacityAfterTrim = numbers.Capacity;

            //assert
            Assert.Equal(expectedCapacityBeforeTrim, capacityBeforeTrim);
            Assert.Equal(expectedCapacityAfterTrim, capacityAfterTrim);
        }

        [Fact]
        public void TrimExcess_CountDivideCapacityGreterThan90Percent()
        {
            //arrange
            MySortedList<int, string> numbers = new MySortedList<int, string>()
            {
                { 1, "one" },
                { 2, "two" },
                { 7, "seven" },
                { 5, "five" },
                { 4, "four" },
                { 6, "six" },
                { 9, "nine" },
                { 11, "eleven" },
                { 12, "twelve" },
                { 13, "thirdteen" },
                { 100, "100" },
                { 101, "101" },
                { 102, "102" },
                { 103, "103" },
                { 104, "104" },
            };
            const int expectedCapacityBeforeTrim = 16;
            const int expectedCapacityAfterTrim = 16;

            //act
            int capacityBeforeTrim = numbers.Capacity;
            numbers.TrimExcess();
            int capacityAfterTrim = numbers.Capacity;

            //assert
            Assert.Equal(expectedCapacityBeforeTrim, capacityBeforeTrim);
            Assert.Equal(expectedCapacityAfterTrim, capacityAfterTrim);
        }

        [Fact]
        public void TryGetValue_NonNullExistingKey_ReturnsTrue()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 },
            };

            //act
            int value;
            bool result = numbers.TryGetValue("four", out value);

            //assert
            Assert.True(result);
            Assert.Equal(4, value);
        }

        [Fact]
        public void TryGetValue_NonNullNoExistingKey_ReturnsFalse()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 },
            };

            //act
            int value;
            bool result = numbers.TryGetValue("ten", out value);

            //assert
            Assert.False(result);
            Assert.Equal(0, value);
        }

        [Fact]
        public void TryGetValue_NullKey_ArgumentNullException()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
                { "four", 4 },
                { "five", 5 },
            };
            const string expectedExceptionMessage = "Key should be not null";

            //act
            int value;
            ArgumentNullException exception = Assert.Throws<ArgumentNullException>(() => numbers.TryGetValue(null, out value));

            //assert
            Assert.NotNull(exception);
            Assert.Equal(expectedExceptionMessage, exception.ParamName);
        }

        [Fact]
        public void ToString_TestValidString()
        {
            //arrange
            MySortedList<string, int> numbers = new MySortedList<string, int>()
            {
                { "one", 1 },
                { "two", 2 },
                { "three", 3 },
            };
            const string expectedString = "{one: 1, three: 3, two: 2}";

            //act
            string actualString = numbers.ToString();

            //assert
            Assert.Equal(expectedString, actualString);
        }
    }
}
