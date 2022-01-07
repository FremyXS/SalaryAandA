using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exe
{
    public class EasySorting
    {
        // Сортировка пузырьком. Сложность O(n^2). Меняются соседние элементы
        public static void BubbleSort(int[] arr)
        {
            int temp;
            for (int i = 0; i < arr.Length - 1; i++)
            {
                for (int j = 0; j < arr.Length - i - 1; j++)
                {
                    if (arr[j + 1] < arr[j])
                    {
                        temp = arr[j + 1];
                        arr[j + 1] = arr[j];
                        arr[j] = temp;
                    }
                }
            }
        }
        // Сортировка вставками. Сложность O(n^2). Элемент протаскивается влево до своего места
        public static void InsertionSort(int[] arr)
        {
            for (var i = 1; i < arr.Length; i++)
            {
                var key = arr[i];
                var j = i;
                while (j > 1 && arr[j - 1] > key)
                {
                    var temp = arr[j - 1];
                    arr[j - 1] = arr[j];
                    arr[j] = temp;
                    j--;
                }

                arr[j] = key;
            }
        }
        // Сортировка выбором. Сложность O(n^2). Наименьший элемент перемещается в отсортированную последовательность
        public static void SelectionSort(int[] arr)
        {
            for (int i = 0; i < arr.Length - 1; i++)
            {
                int min = i;
                for (int j = i + 1; j < arr.Length; j++)
                    if (arr[j] < arr[min])
                        min = j;

                int temp = arr[i];
                arr[i] = arr[min];
                arr[min] = temp;
            }
        }
        // Шейкерная сортировка. Сложность O(n^2). Наибольшие и наименьшие элементы протаскиваются в конец и начало, границы сортировки сужаются
        public static void ShakerSort(int[] arr)
        {
            int left = 0,
                right = arr.Length - 1;

            while (left < right)
            {
                for (int i = left; i < right; i++)
                {
                    if (arr[i] > arr[i + 1])
                        Swap(ref arr[i], ref arr[i + 1]);
                }
                right--;

                for (int i = right; i > left; i--)
                {
                    if (arr[i - 1] > arr[i])
                        Swap(ref arr[i - 1], ref arr[i]);
                }
                left++;
            }

            void Swap(ref int e1, ref int e2)
            {
                var temp = e1;
                e1 = e2;
                e2 = temp;
            }
        }
        // Сортировка Шелла. Сложность O(n^2). Элементы сортируются с уменьшающимся расстоянием между ними
        public static void ShellSort(int[] arr)
        {
            var d = arr.Length / 2;
            while (d >= 1)
            {
                for (var i = d; i < arr.Length; i++)
                {
                    var j = i;
                    while (j >= d && arr[j - d] > arr[j])
                    {
                        Swap(ref arr[j], ref arr[j - d]);
                        j -= d;
                    }
                }
                d /= 2;
            }

            void Swap(ref int a, ref int b)
            {
                var t = a;
                a = b;
                b = t;
            }
        }
        // Быстрая сортировка. O(n*log2 n). Массив разбивается на левый и правый от опорного элемента
        public static void QuickSort(int[] array, int minIndex, int maxIndex)
        {
            if (minIndex >= maxIndex)
                return;

            var pivotIndex = Partition(array, minIndex, maxIndex);
            QuickSort(array, minIndex, pivotIndex - 1);
            QuickSort(array, pivotIndex + 1, maxIndex);

            int Partition(int[] array, int minIndex, int maxIndex)
            {
                var pivot = minIndex - 1;
                for (var i = minIndex; i <= maxIndex; i++)
                {
                    if (array[i] < array[maxIndex] || i == maxIndex)
                    {
                        pivot++;
                        Swap(ref array[pivot], ref array[i]);
                    }
                }
                return pivot;
            }
            void Swap(ref int x, ref int y)
            {
                var t = x;
                x = y;
                y = t;
            }
        }
        // Алгоритм бинарного поиска. O(log2 n). В два раза сужаем область поиска.
        public static int BinarySearch(int[] arr, int key, int leftEdge, int rightEdge)
        {
            int mid = (leftEdge + rightEdge) / 2;
            if (arr[mid] == key)
                return mid;
            return arr[mid] < key ? BinarySearch(arr, key, mid + 1, rightEdge) : BinarySearch(arr, key, leftEdge, mid - 1);
        }
    }
}
