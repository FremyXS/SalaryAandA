using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exe
{
    public class MediumSorting
    {
        // Внешняя сортировка.
        public static void SortByFiles(string pathToSort)
        {
            int numOfLines = 0;
            using (var reader = new StreamReader(pathToSort))
                while (reader.ReadLine() != null)
                    numOfLines++;

            int runTo = (int)Math.Ceiling(Math.Log(numOfLines, 2));

            for (int i = 0; i < runTo; i++)
                TakeElements((int)Math.Pow(2, i));


            void TakeElements(int switching)
            {
                //Наполнение временных файлов
                #region
                var writerA = new StreamWriter("A.txt");
                var writerB = new StreamWriter("B.txt");

                int counter = 0;
                bool writeToA = true;
                using (var reader = new StreamReader(pathToSort))
                {
                    string? line = reader.ReadLine();
                    while (line != null)
                    {
                        if (counter % switching == 0)
                            writeToA = !writeToA;

                        if (writeToA)
                            writerA.WriteLine(line);
                        else
                            writerB.WriteLine(line);
                        counter++;
                        line = reader.ReadLine();
                    }
                }
                writerB.Dispose();
                writerA.Dispose();
                #endregion
                //Сбрасывание в файл
                File.Create(pathToSort).Dispose();
                using (var writer = new StreamWriter(pathToSort))
                {
                    var readerA = new StreamReader("A.txt");
                    var readerB = new StreamReader("B.txt");

                    while (!readerA.EndOfStream || !readerB.EndOfStream)
                    {
                        PopTo(readerA, readerB, switching);
                    }

                    readerA.Dispose();
                    readerB.Dispose();

                    void PopTo(StreamReader readerA, StreamReader readerB, int popFromFile)
                    {
                        int stepsA = 0, stepsB = 0;
                        string? a_line = readerA.ReadLine();
                        string? b_line = readerB.ReadLine();
                        while ((stepsA < popFromFile || stepsB < popFromFile) && (a_line != null || b_line != null))
                        {
                            int compare;
                            if (a_line == null) compare = 1;
                            else if (b_line == null) compare = -1;
                            else compare = int.Parse(a_line).CompareTo(int.Parse(b_line));

                            if (compare < 0 || compare == 0)
                            {
                                writer.WriteLine(a_line);
                                stepsA++;
                                a_line = stepsA >= popFromFile ? null : readerA.ReadLine();
                            }
                            if (compare > 0 || compare == 0)
                            {
                                writer.WriteLine(b_line);
                                stepsB++;
                                b_line = stepsB >= popFromFile ? null : readerB.ReadLine();
                            }
                        }
                    }
                }
            }
        }
        // ABC-сортировка.
        public static ICollection<string> ABSSort(ICollection<string> words, int rank = 0)
        {
            if (words.Count <= 1)
                return words;

            var square = new Dictionary<char, List<string>>(62);
            var result = new List<string>();
            int shortWordsCounter = 0;
            foreach (var word in words)
            {
                if (rank < word.Length)
                {
                    if (square.ContainsKey(word[rank]))
                        square[word[rank]].Add(word);
                    else
                        square.Add(word[rank], new List<string> { word });
                }
                else
                {
                    result.Add(word);
                    shortWordsCounter++;
                }
            }
            if (shortWordsCounter == words.Count)
                return words;

            for (char i = 'А'; i <= 'я'; i++)
            {
                if (square.ContainsKey(i))
                {
                    foreach (var word in ABSSort(square[i], rank + 1))
                        result.Add(word);
                }
            }
            return result;
        }
        // Сортировка двоичным деревом. Сложность O(n*log2 n). Элементы располагаются деревом, слева < число, справа >= число
        private class TreeNode
        {
            public TreeNode(int num) => number = num;
            int number;
            TreeNode? left;
            TreeNode? right;
            internal void Insert(int num)
            {
                if (num < number)
                {
                    if (left == null)
                        left = new TreeNode(num);
                    else
                        left.Insert(num);
                }
                else
                {
                    if (right == null)
                        right = new TreeNode(num);
                    else
                        right.Insert(num);
                }
            }
            internal List<int> Parse(List<int> array)
            {
                if (left != null)
                    left.Parse(array);
                array.Add(number);
                if (right != null)
                    right.Parse(array);
                return array;
            }
        }
        public static void TreeSort(int[] arr)
        {
            var enumerator = arr.GetEnumerator();
            enumerator.MoveNext();
            var root = new TreeNode((int)enumerator.Current);
            while (enumerator.MoveNext())
                root.Insert((int)enumerator.Current);
            Array.Copy(root.Parse(new List<int>()).ToArray(), arr, arr.Length);
        }
        // Radix sort. Сортировка чисел с помощью сдвига
        public static void RadixSort(int[] arr)
        {
            int i, j;
            int[] tmp = new int[arr.Length];
            for (int shift = sizeof(int) * 8 - 1; shift >= 0; shift--)
            {
                j = 0;
                for (i = 0; i < arr.Length; i++)
                {
                    bool move = (arr[i] << shift) >= 0;
                    if (shift == 0 ? !move : move)
                        arr[i - j] = arr[i];
                    else
                        tmp[j++] = arr[i];
                }
                Array.Copy(tmp, 0, arr, arr.Length - j, j);
            }
        }
    }
}
