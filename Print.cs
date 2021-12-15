using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Print
    {
        public void Print_pole(int[,] pole, int[,] obj, int time, int prize, bool gmovr)
        {
            Console.Clear();


            for (int j = 0; j < pole.GetLength(1); j++)
            {
                for (int i = 0; i < pole.GetLength(0); i++)
                {
                    if (pole[i, j] == 1) //препятсивме
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                    else if (pole[i, j] == 0) //пустота
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else //упавший объект
                    {
                        Console.ForegroundColor = ConsoleColor.Blue;
                    }

                    for (int z = 0; z < 4; z++)
                        if (obj[z, 0] == i && obj[z, 1] == j)
                        {
                            Console.ForegroundColor = ConsoleColor.Green;
                        }

                    Console.Write("0");
                    Console.ResetColor();
                }

                if (j == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    if (!gmovr)
                    {
                        Console.Write($"    Количество очков - {prize}");
                    }
                    else
                    {
                        Console.Write($"    Проигрыш!");
                    }
                }
                if (j == 1 && gmovr)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write($"    Нажмите Enter, чтобы начать сначала.");
                }

                if(j == 2)
                {
                    //Console.ForegroundColor = ConsoleColor.Yellow;
                    //Console.Write($"    Powered by Alegdreg");
                }

                Console.WriteLine();
            }


            Console.WriteLine($"Параметры:\nШирина  - {pole.GetLength(0)}, высота - {pole.GetLength(1)}," +
                $"\nСкорость игры - {Convert.ToDouble(time) / 1000} сек,");


            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"Enter - вращение, стрелки - перемещение");

        }
    }
}
