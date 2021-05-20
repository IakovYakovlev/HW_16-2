using System;
using System.Diagnostics;

namespace Delegate
{
    class Program
    {

        // Создаем делегат.
        public delegate int BinaryOp(int x, int y);

        static void Main(string[] args)
        {
            #region Задание
            // Вычислить количество чисел на промежутке 
            // от 1 000 000 000
            // до 2 000 000 000

            // в которых сумма цифр числа кратна последней цифре числа

            // Число        Сумма цифр      Последняя цифра     Кратность
            //    11                 2                    1            Да    
            //    12                 3                    2           Нет     
            //    19                10                    9           Нет      
            //    20                 2                    0            Да      
            //   123                 6                    3            Да       
            // 
            #endregion

            // Создаем таймер.
            Stopwatch sw = new Stopwatch();

            // Создаем переменные для за писи промежутка.
            int start = 1_000_000_000;
            int end = 2_000_000_000;

            // Переменная для результата по заданию.
            int result = 0;

            // Стартуем таймер.
            sw.Start();

            // Записываем диапазон.
            int range = end - start;

            // Запсываем кол-во потоков.
            int numThreads = (int)Environment.ProcessorCount;

            // Создаем массив делегатов.
            BinaryOp[] b = new BinaryOp[numThreads];

            // Создаем массив интерфейсов.
            IAsyncResult[] it1 = new IAsyncResult[numThreads];

            // Разбиваем диапазон на кол-во потоков.
            int chunk = range / numThreads;

            // Запускаем расчет 
            for(int i = 0; i < numThreads; i++)
            {
                // Записываем начало и конец для каждого следующего потока.
                int chunkStart = start + i * chunk;
                int chunkEnd = chunkStart + chunk;

                // Запускаем итый поток.
                b[i] = new BinaryOp(Multiplicity);
                it1[i] = b[i].BeginInvoke(chunkStart, chunkEnd, null, null);
            }

            
            // Получаем общий результат.
            for(int i = 0; i < numThreads; i++)
            {
                result += b[i].EndInvoke(it1[i]);
            }

            // Останавливаем таймер.
            sw.Stop();

            // Переводим время в удобный формат.
            TimeSpan ts = sw.Elapsed;
            string time = string.Format("{0:00}:{1:00}:{2:00}.{3:00}",
                ts.Hours, ts.Minutes, ts.Seconds, ts.Milliseconds);

            // Выводим на консоль и ожидаем.
            Console.WriteLine($"Время: {time}");
            Console.WriteLine($"Результат: {result} ");
            Console.ReadKey();
        }

        /// <summary>
        /// Метод пробегает по всему диапазону.
        /// </summary>
        /// <param name="start">Начало диапазона</param>
        /// <param name="end">Конец диапазона</param>
        /// <returns>Кол-во кратных.</returns>
        static int Multiplicity(int start, int end)
        {
            // Создаем переменнюу для записи результата.
            int mass = 0;

            // Пробегаем по всем всему диапазону и если кратен,
            // тогда добавляем +1 к результату.
            for(int num = start; num < end; ++num)
            {
                if (IfMultiplicity(num))
                {
                    mass = mass + 1;
                }
            }
            return mass;
        }

        /// <summary>
        /// Метод для выяснения, кратен или нет?
        /// </summary>
        /// <param name="x">Значение.</param>
        /// <returns>true or false</returns>
        static bool IfMultiplicity(int x)
        {
            int s = 0;

            while (x > 0)
            {
                s = s + x % 10;

                x = x / 10;
            }

            int end = s % 10;

            if (end == 0 || s % end == 0)
            {
                return true;
            }
            else
                return false;
        }
    }
}
