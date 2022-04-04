using System;
using System.Collections.Generic;
using CustomSortedList;

namespace SimpleConsoleApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            /*IDictionary<string, int> numberNames = new Dictionary<string, int>();
            numberNames.Add("One", 1);
            numberNames.Add("Two", 2);
            numberNames.Add("Three", 3);*/

            MySortedList<string, int> mySortedList = new MySortedList<string, int>();
            IList<string> keys = mySortedList.Keys;
            IList<int> values = mySortedList.Values;

            mySortedList.Addition += (sender, mySortedListEventArgs) =>
                Console.WriteLine($"{DateTime.Now.TimeOfDay} LOG: {mySortedListEventArgs.Message}\t" +
                $"Key: {mySortedListEventArgs.Element.Key}\t" +
                $"Value: {mySortedListEventArgs.Element.Value}");
            mySortedList.Removal += (sender, mySortedListEventArgs) =>
                Console.WriteLine($"{DateTime.Now.TimeOfDay} LOG: {mySortedListEventArgs.Message}\t" +
                $"Key: {mySortedListEventArgs.Element.Key}\t" +
                $"Value: {mySortedListEventArgs.Element.Value}");
            mySortedList.Clearing += (sender, mySortedListEventArgs) =>
                Console.WriteLine($"{DateTime.Now.TimeOfDay} LOG: {mySortedListEventArgs.Message}");

            bool flag = true;
            while (flag)
            {
                Console.Clear();
                Console.WriteLine("----------------------------------");
                Console.WriteLine("|1.\tview the list");
                Console.WriteLine("|2.\tview keys");
                Console.WriteLine("|3.\tview values");
                Console.WriteLine("|4.\tcheck contains key");
                Console.WriteLine("|5.\tcheck contains value");
                Console.WriteLine("|6.\tget value by key");
                Console.WriteLine("|7.\tget index of key");
                Console.WriteLine("|8.\tget index of value");
                Console.WriteLine("|9.\tget key by index");
                Console.WriteLine("|10.\tget value by index");
                Console.WriteLine("|11.\tupdate value by key");
                Console.WriteLine("|12.\tupdate value by index");
                Console.WriteLine("|13.\tadd element");
                Console.WriteLine("|14.\tremove element by key");
                Console.WriteLine("|15.\tremove element by index");
                Console.WriteLine("|16.\tclear");
                Console.WriteLine("|17.\tcount");
                Console.WriteLine("|0.\texit");
                Console.WriteLine("----------------------------------\n");

                string action = Console.ReadLine();

                try
                {
                    switch (action)
                    {
                        case "0":
                            {
                                Console.Clear();
                                flag = false;
                                Environment.Exit(0);
                                Console.ReadLine();
                                break;
                            }
                        case "1":
                            {
                                Console.Clear();
                                foreach (KeyValuePair<string, int> kvp in mySortedList)
                                    Console.WriteLine("key: {0}\tvalue: {1}", kvp.Key, kvp.Value);
                                Console.ReadLine();
                                break;
                            }
                        case "2":
                            {
                                Console.Clear();
                                foreach (string key in keys)
                                    Console.WriteLine("key: {0}", key);
                                Console.ReadLine();
                                break;
                            }
                        case "3":
                            {
                                Console.Clear();
                                foreach (int value in values)
                                    Console.WriteLine("value: {0}", value);
                                Console.ReadLine();
                                break;
                            }
                        case "4":
                            {
                                Console.Clear();
                                Console.WriteLine("Enter the key: ");
                                string key = Console.ReadLine();

                                Console.WriteLine(mySortedList.ContainsKey(key) ? "element present" : "no element");
                                Console.ReadLine();
                                break;
                            }
                        case "5":
                            {
                                Console.Clear();
                                Console.WriteLine("Enter the value: ");
                                string value = Console.ReadLine();
                                int parsed = Int32.Parse(value);

                                Console.WriteLine(mySortedList.ContainsValue(parsed) ? "element present" : "no element");
                                Console.ReadLine();
                                break;
                            }
                        case "6":
                            {
                                Console.Clear();
                                Console.WriteLine("Enter the key: ");
                                string key = Console.ReadLine();

                                Console.WriteLine(mySortedList[key]);
                                Console.ReadLine();
                                break;
                            }
                        case "7":
                            {
                                Console.Clear();
                                Console.WriteLine("Enter the key: ");
                                string key = Console.ReadLine();

                                Console.WriteLine(mySortedList.IndexOfKey(key));
                                Console.ReadLine();
                                break;
                            }
                        case "8":
                            {
                                Console.Clear();
                                Console.WriteLine("Enter the value: ");
                                string value = Console.ReadLine();
                                int parsed = Int32.Parse(value);

                                Console.WriteLine(mySortedList.IndexOfValue(parsed));
                                Console.ReadLine();
                                break;
                            }
                        case "9":
                            {
                                Console.Clear();
                                Console.WriteLine("Enter the index: ");
                                string value = Console.ReadLine();
                                int index = Int32.Parse(value);

                                Console.WriteLine(mySortedList.GetKey(index));
                                Console.ReadLine();
                                break;
                            }
                        case "10":
                            {
                                Console.Clear();
                                Console.WriteLine("Enter the index: ");
                                string value = Console.ReadLine();
                                int index = Int32.Parse(value);

                                Console.WriteLine(mySortedList.GetByIndex(index));
                                Console.ReadLine();
                                break;
                            }
                        case "11":
                            {
                                Console.Clear();
                                Console.WriteLine("Enter the key: ");
                                string key = Console.ReadLine();
                                Console.WriteLine("Enter the value: ");
                                int value = Int32.Parse(Console.ReadLine());
                                mySortedList[key] = value;
                                Console.ReadLine();
                                break;
                            }
                        case "12":
                            {
                                Console.Clear();
                                Console.WriteLine("Enter the index: ");
                                int index = Int32.Parse(Console.ReadLine());
                                Console.WriteLine("Enter the value: ");
                                int value = Int32.Parse(Console.ReadLine());

                                mySortedList.SetByIndex(index, value);
                                Console.ReadLine();
                                break;
                            }
                        case "13":
                            {
                                Console.Clear();
                                Console.WriteLine("Enter the key: ");
                                string key = Console.ReadLine();
                                Console.WriteLine("Enter the value: ");
                                int value = Int32.Parse(Console.ReadLine());
                                mySortedList.Add(key, value);
                                Console.ReadLine();
                                break;
                            }
                        case "14":
                            {
                                Console.Clear();
                                Console.WriteLine("Enter the key: ");
                                string key = Console.ReadLine();
                                mySortedList.Remove(key);

                                Console.ReadLine();
                                break;
                            }
                        case "15":
                            {
                                Console.Clear();
                                Console.WriteLine("Enter the key: ");
                                int index = Int32.Parse(Console.ReadLine());
                                mySortedList.RemoveAt(index);

                                Console.ReadLine();
                                break;
                            }
                        case "16":
                            {
                                Console.Clear();
                                mySortedList.Clear();
                                Console.ReadLine();
                                break;
                            }
                        case "17":
                            {
                                Console.Clear();
                                Console.WriteLine($"Count: {mySortedList.Count}");
                                Console.ReadLine();
                                break;
                            }
                    }
                } catch (Exception e)
                {
                    Console.WriteLine("Something went wrong. Error message:");
                    Console.WriteLine(e.Message);
                    Console.ReadLine();
                }

            }
        }
    }
}
