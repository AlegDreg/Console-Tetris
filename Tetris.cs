using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Timer = System.Timers.Timer;

namespace ConsoleApp1
{
    class Tetris
    {
        int[,] pole;
        int x;
        int y;
        int time_out;
        int time_reload;
        Timer timer;
        int[,] obj;
        int[,] new_obj;
        int nomer_obj;
        int nomer_pozicii = 0;
        int count_prize = 0;
        bool gameOver;

        public delegate void DataHandler(int[,] pole, int[,] obj, int prize, bool GameOver);
        public event DataHandler GetChanged;

        public void Start(int xs, int ys, int time, int time_rel)
        {
            x = xs;
            y = ys;
            time_out = time;
            time_reload = time_rel;

            timer = new Timer(time_out);

            Reload();

            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.AutoReset = true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Tim_el();
        }

        private void Tim_el()
        {
            if (!gameOver)
                if (Get_Down())
                {
                    if (Check_place())
                    {

                    }
                    else
                    {
                        Set_obj_to_pole();
                    }
                }

            GetChanged?.Invoke(pole, obj, count_prize, gameOver);
        }

        public void Read_key(int key) //0 - вращать, 1 - влево, 2 - вправо
        {
            if (obj[0, 0] != 0 || gameOver)
            {
                Control_obj(key);
            }
        }

        private void Control_obj(int key)
        {
            bool ok = false;

            switch (key)
            {
                case 0:
                    if (Vrashenie() && !gameOver)//пробуем повернуть, занеся временные координаты нового возможного в новый
                    {
                        ok = true;
                    }
                    if(gameOver)
                    {
                        Reload();
                    }
                    break;
                case 1:
                    if (LeftRitht_Control(1))
                    {
                        ok = true;
                    }
                    break;

                case 2:
                    if (LeftRitht_Control(2))
                    {
                        ok = true;
                    }
                    break;
                case 3:
                    Reload();
                    break;
            }

            if (ok)
            {
                if (Check_place())
                {

                }
            }
        }

        private void isLine()
        {
            for (int j = 1; j < pole.GetLength(1) - 1; j++)
            {
                int l = pole.GetLength(1);
                int count = 0;
                for (int i = 1; i < pole.GetLength(0) - 1; i++)
                {
                    if (pole[i, j] == 2)
                        count++;
                }

                if (count == pole.GetLength(0) - 2)
                {
                    Clear_line(j);
                    SlideDown(j);
                    count_prize++;
                }
            }
        }

        private void Clear_line(int y)
        {
            for (int i = 1; i < pole.GetLength(0) - 1; i++)
            {
                if (pole[i, y] == 2)
                {
                    pole[i, y] = 0;
                }
            }
        }

        private void SlideDown(int y)
        {
            try
            {
                for (int i = y - 1; i > 0; i--)
                {
                    for (int j = 1; j < pole.GetLength(0); j++)
                    {
                        if (pole[j, i + 1] == 0 && pole[j, i] == 2)
                        {
                            pole[j, i + 1] = 2;
                            pole[j, i] = 0;
                        }
                    }
                }
            }
            catch { }
        }

        private bool LeftRitht_Control(int key)//1 - влево, 2 - вправо
        {
            Clear_new_obj();

            int pos;

            if (key == 2)
                pos = 1;
            else
                pos = -1;

            try
            {
                for (int i = 0; i < 4; i++)
                {
                    new_obj[i, 0] = obj[i, 0] + pos;
                    new_obj[i, 1] = obj[i, 1];
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        private void Initialize(int x, int y)
        {
            pole = new int[x, y];//инициализация поля

            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                {
                    pole[i, j] = 0;
                }
            //инициализация границ

            for (int i = 0; i < y; i++)
                pole[0, i] = 1;

            for (int i = 0; i < y; i++)
                pole[x - 1, i] = 1;

            for (int i = 0; i < x; i++)
                pole[i, 0] = 1;

            for (int i = 0; i < x; i++)
                pole[i, y - 1] = 1;

            //инициализация фигуры
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 2; j++)
                    obj[i, j] = 0;

            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 2; j++)
                    new_obj[i, j] = 0;
        }

        private void Generate_obj()
        {
            Objects();

            if(Check_place())
            {
                ConvertToObj_from_newObj();
            }
            else
            {
                Gameover();
            }
        }

        private void Reload()
        {
            count_prize = 0;
            obj = new int[4, 2];
            new_obj = new int[4, 2];

            Initialize(x, y);
            Generate_obj();

            gameOver = false;
            timer.Enabled = true;
        }

        private void Gameover()
        {
            timer.Stop();
            gameOver = true;
            Tim_el();
        }

        private void ConvertToObj_from_newObj()
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 2; j++)
                    obj[i, j] = new_obj[i, j];
        }

        private void Objects()
        {
            Random random = new Random();
            int k = random.Next(0, 4);
            int z = pole.GetLength(0) / 2;
            nomer_pozicii = 0;

            switch (k)
            {
                case 0:
                    new_obj[0, 0] = z;//квадрат
                    new_obj[0, 1] = 1;

                    new_obj[1, 0] = z + 1;
                    new_obj[1, 1] = 1;

                    new_obj[2, 0] = z;
                    new_obj[2, 1] = 2;

                    new_obj[3, 0] = z + 1;
                    new_obj[3, 1] = 2;
                    nomer_obj = 0;
                    break;

                case 1:
                    new_obj[0, 0] = z- 2;//палка
                    new_obj[0, 1] = 1;

                    new_obj[1, 0] = z - 1;
                    new_obj[1, 1] = 1;

                    new_obj[2, 0] = z;
                    new_obj[2, 1] = 1;

                    new_obj[3, 0] = z + 1;
                    new_obj[3, 1] = 1;
                    nomer_obj = 1;
                    break;

                case 2:
                    new_obj[2, 0] = z;//зигзаг
                    new_obj[2, 1] = 1;

                    new_obj[1, 0] = z;
                    new_obj[1, 1] = 2;

                    new_obj[3, 0] = z + 1;
                    new_obj[3, 1] = 1;

                    new_obj[0, 0] = z - 1;
                    new_obj[0, 1] = 2;
                    nomer_obj = 2;
                    break;

                case 3:
                    new_obj[0, 0] = z;//L
                    new_obj[0, 1] = 1;

                    new_obj[1, 0] = z;
                    new_obj[1, 1] = 2;

                    new_obj[2, 0] = z;
                    new_obj[2, 1] = 3;

                    new_obj[3, 0] = z + 1;
                    new_obj[3, 1] = 3;
                    nomer_obj = 3;
                    break;
            }
        }

        private bool Check_place()//true - если свободно
        {
            int count_oks = 0;

            for (int i = 0; i < 4; i++)
            {
                int x = new_obj[i, 0];
                int y = new_obj[i, 1];

                try
                {
                    if (pole[x, y] == 0)
                    {
                        count_oks++;
                    }
                }
                catch { }
            }

            if (count_oks == 4)
            {
                ConvertToObj_from_newObj();
                return true;
            }
            else
                return false;

        }

        private bool Get_Down()
        {
            Clear_new_obj();
            try
            {
                for (int i = 0; i < 4; i++)
                {
                    new_obj[i, 1] = obj[i, 1] + 1;
                    new_obj[i, 0] = obj[i, 0];
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void Set_obj_to_pole()
        {
            timer.Stop();

            for (int i = 0; i < pole.GetLength(0); i ++)
                for(int j = 0; j < pole.GetLength(1); j++)
                {
                    for(int z = 0; z < 4; z++)
                        if (obj[z, 0] == i && obj[z, 1] == j)
                        {
                            pole[i, j] = 2;
                        }
                    
                }
            Clear_new_obj();
            Clear_obj();

            isLine();
            Thread.Sleep(time_reload);
            timer.Enabled = true;

            Generate_obj();
        }

        private bool Vrashenie()
        {
            int vrem = nomer_pozicii;

            Clear_new_obj();
            try
            {
                switch (nomer_obj)
                {
                    case 0://квадрат
                        break;

                    case 1://палка
                        if (nomer_pozicii == 0)
                        {
                            new_obj[0, 0] = obj[0, 0] + 1;
                            new_obj[0, 1] = obj[0, 1] - 1;

                            new_obj[1, 0] = obj[1, 0];
                            new_obj[1, 1] = obj[1, 1];

                            new_obj[2, 0] = obj[2, 0] - 1;
                            new_obj[2, 1] = obj[2, 1] + 1;

                            new_obj[3, 0] = obj[3, 0] - 2;
                            new_obj[3, 1] = obj[3, 1] + 2;
                            nomer_pozicii = 1;
                        }
                        else
                        {
                            new_obj[0, 0] = obj[0, 0] - 1;
                            new_obj[0, 1] = obj[0, 1] + 1;

                            new_obj[1, 0] = obj[1, 0];
                            new_obj[1, 1] = obj[1, 1];

                            new_obj[2, 0] = obj[2, 0] + 1;
                            new_obj[2, 1] = obj[2, 1] - 1;

                            new_obj[3, 0] = obj[3, 0] + 2;
                            new_obj[3, 1] = obj[3, 1] - 2;
                            nomer_pozicii = 0;
                        }
                        break;

                    case 2://зигзаг
                        if (nomer_pozicii == 0)
                        {
                            new_obj[0, 0] = obj[0, 0] + 1;
                            new_obj[0, 1] = obj[0, 1] - 1;

                            new_obj[1, 0] = obj[1, 0];
                            new_obj[1, 1] = obj[1, 1];

                            new_obj[2, 0] = obj[2, 0] + 1;
                            new_obj[2, 1] = obj[2, 1] + 1;

                            new_obj[3, 0] = obj[3, 0];
                            new_obj[3, 1] = obj[3, 1] + 2;
                            nomer_pozicii = 1;
                        }
                        else
                        {
                            new_obj[0, 0] = obj[0, 0] - 1;
                            new_obj[0, 1] = obj[0, 1] + 1;

                            new_obj[1, 0] = obj[1, 0];
                            new_obj[1, 1] = obj[1, 1];

                            new_obj[2, 0] = obj[2, 0] - 1;
                            new_obj[2, 1] = obj[2, 1] - 1;

                            new_obj[3, 0] = obj[3, 0];
                            new_obj[3, 1] = obj[3, 1] - 2;
                            nomer_pozicii = 0;
                        }
                        break;

                    case 3://L
                        if (nomer_pozicii == 0)
                        {
                            new_obj[0, 0] = obj[0, 0] + 1;
                            new_obj[0, 1] = obj[0, 1] + 1;

                            new_obj[1, 0] = obj[1, 0];
                            new_obj[1, 1] = obj[1, 1];

                            new_obj[2, 0] = obj[2, 0] - 1;
                            new_obj[2, 1] = obj[2, 1] - 1;

                            new_obj[3, 0] = obj[3, 0] - 2;
                            new_obj[3, 1] = obj[3, 1];
                            nomer_pozicii = 1;
                        }
                        else if (nomer_pozicii == 1)
                        {
                            new_obj[0, 0] = obj[0, 0] - 1;
                            new_obj[0, 1] = obj[0, 1] + 1;

                            new_obj[1, 0] = obj[1, 0];
                            new_obj[1, 1] = obj[1, 1];

                            new_obj[2, 0] = obj[2, 0] + 1;
                            new_obj[2, 1] = obj[2, 1] - 1;

                            new_obj[3, 0] = obj[3, 0];
                            new_obj[3, 1] = obj[3, 1] - 2;
                            nomer_pozicii = 2;
                        }
                        else if (nomer_pozicii == 2)
                        {
                            new_obj[0, 0] = obj[0, 0] - 1;
                            new_obj[0, 1] = obj[0, 1] - 1;

                            new_obj[1, 0] = obj[1, 0];
                            new_obj[1, 1] = obj[1, 1];

                            new_obj[2, 0] = obj[2, 0] + 1;
                            new_obj[2, 1] = obj[2, 1] + 1;

                            new_obj[3, 0] = obj[3, 0] + 2;
                            new_obj[3, 1] = obj[3, 1];
                            nomer_pozicii = 3;
                        }
                        else
                        {
                            new_obj[0, 0] = obj[0, 0] + 1;
                            new_obj[0, 1] = obj[0, 1] - 1;

                            new_obj[1, 0] = obj[1, 0];
                            new_obj[1, 1] = obj[1, 1];

                            new_obj[2, 0] = obj[2, 0] - 1;
                            new_obj[2, 1] = obj[2, 1] + 1;

                            new_obj[3, 0] = obj[3, 0];
                            new_obj[3, 1] = obj[3, 1] + 2;
                            nomer_pozicii = 0;
                        }
                        break;
                }

                return true;
            }
            catch
            {
                nomer_pozicii = vrem;
                return false;
            }
        }

        private void Clear_new_obj()
        {
            if(new_obj[0,0] != 0)
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 2; j++)
                        new_obj[i, j] = 0;
        }

        private void Clear_obj()
        {
            for (int i = 0; i < 4; i++)
                for (int j = 0; j < 2; j++)
                    obj[i, j] = 0;
        }
    }
}
