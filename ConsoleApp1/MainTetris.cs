using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class MainTetris
    {
        Tetris tetris;
        Print print;

        int x = 10;//ширина
        int y = 15;//высота
        double time_out = 500;
        int time_reload = 1000; //время ожидания после падения фигуры
        bool isStart = false;

        public void Main()
        {
            Print();
            Console.WriteLine("Нажмите стрелку вниз, чтобы начать, или Enter, чтобы изменить настройки");

            while (true)
            {
                var k = Console.ReadKey();

                if(!isStart)
                {
                    if (k.Key == ConsoleKey.Enter)
                    {
                        ChangeSettings();
                    } else if(k.Key == ConsoleKey.DownArrow)
                    {
                        tetris = new Tetris();
                        print = new Print();

                        tetris.Start(x, y, Convert.ToInt32(time_out), time_reload);
                        isStart = true;
                        tetris.GetChanged += Tetris_GetChanged;
                    }

                }

                if(k.Key == ConsoleKey.RightArrow)
                {
                    tetris.Read_key(2);
                }else if(k.Key == ConsoleKey.LeftArrow)
                {
                    tetris.Read_key(1);
                }else if(k.Key == ConsoleKey.Enter)
                {
                    tetris.Read_key(0);
                }
            }
        }

        private void ChangeSettings()
        {
            Console.ResetColor();

            Console.Write("\nВведите ширину поля - ");

            bool g = Int32.TryParse(Console.ReadLine(), out x);
            if (!g || x < 10)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Укажите корректное значение!");
                ChangeSettings();
            }

            Console.Write("\nВведите высоту поля - ");
            g = Int32.TryParse(Console.ReadLine(), out y);
            if (!g || y < 10)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Укажите корректное значение!");
                ChangeSettings();
            }

            Console.Write("\nУкажите скорость обновления поля в секундах - ");
            g = Double.TryParse(Console.ReadLine(), out time_out);
            if (!g)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Укажите корректное значение!");
                ChangeSettings();
            }
            time_out = time_out * 1000;

            Main();
        }

        private void Tetris_GetChanged(int[,] pole, int[,] obj,int prize, bool gameOver)
        {
            print.Print_pole(pole, obj, Convert.ToInt32(time_out), prize, gameOver);
        }

        private void Print()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Параметры:\nШирина  - {x}, высота - {y}," +
                $"\nСкорость игры - {Convert.ToDouble(time_out/1000)} сек\n");
            Console.ResetColor();
        }
    }
}
