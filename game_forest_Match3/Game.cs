using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace game_forest_Match3
 //Игра
{
    public class Index
    {
        public Index() //Начальные координаты
        {
            m_x = 0;
            m_y = 0;
        }

        public Index(int x, int y) //Координаты по х,у
        {
            m_x = x;
            m_y = y;
        }

        public Index(Index r) //Координаты r
        {
            m_x = r.m_x;
            m_y = r.m_y;
        }

        public int X { get { return m_x; } set { m_x = value; } } //Свойство х
        public int Y { get { return m_y; } set { m_y = value; } } //Свойство у

        int m_x, m_y;
    }

    //Линии
    public class Line
    {
        public Line() //Начальная и конечная линии
        {
            m_start = new Index();
            m_finish = new Index();
        }

        public Line(Index start, Index finish) //Линии по r
        {
            m_start = new Index(start);
            m_finish = new Index(finish);
        }

        public Index Start { get { return m_start; } set { m_start = new Index(value); } } //Свойство старт
        public Index Finish { get { return m_finish; } set { m_finish = new Index(value); } } //Свойство финиш

        Index m_start;
        Index m_finish;
    }

    //Игра
    public class Game
    {
        public Game(int fieldWidth, int fieldHeight, int typesCount)
        {
            m_score = 0; //Начальные очки
            FieldWidth = fieldWidth; //Высота
            FieldHeight = fieldHeight; //Ширина
            TypesCount = typesCount; //Количество типов
            m_matrix = new sbyte[FieldHeight, FieldWidth];
        }

        //Заполнение матрицы 
        public void FillMatrix()
        {
            Random random = new Random();
            //заполнение матрицы, избегая 3 совпадений подряд
            for (int y = 0; y < FieldHeight; y++) //Цикл
                for (int x = 0; x < FieldWidth; x++) //Цикл
                {
                    sbyte value; //Тип данных принимающий значения от -128 до 127
                    bool repeat = false; //Да или нет
                    do
                    {
                        value = (sbyte)random.Next(0, TypesCount); //Переменной передается значение от 0 до количества типов
                        repeat = false;
                        if (x >= 2 && (m_matrix[y, x - 2] == m_matrix[y, x - 1] && m_matrix[y, x - 2] == value))
                            repeat = true;
                        else if (y >= 2 && (m_matrix[y - 2, x] == m_matrix[y - 1, x] && m_matrix[y - 2, x] == value))
                            repeat = true;
                    }
                    while (repeat != false);
                    m_matrix[y, x] = value;
                }
        }

        //Линии
        public bool RemoveMatches()
        {
            List<Line> lines = new List<Line>(); //Создание листа линий
            //поиск горизонтальных линий
            sbyte[,] tmpMatrix = (sbyte[,])m_matrix.Clone(); //Создание массива
            for (int y = 0; y < FieldHeight; ++y) //Цикл
                for (int x = 0; x < FieldWidth; ++x)
                {
                    if (tmpMatrix[y, x] == -1)
                        continue;
                    int count = 1;
                    for (int i = x + 1; i < FieldWidth; ++i)
                        if (tmpMatrix[y, i] == tmpMatrix[y, x])
                            count++;
                        else
                            break;
                    if (count >= 3)
                    {
                        for (int i = x; i < x + count; ++i)
                            tmpMatrix[y, i] = -1;
                        lines.Add(new Line(new Index(x, y), new Index(x + count - 1, y))); //Добавление в лист линий новые линии
                    }
                }

            //поиск вертикальных линий
            tmpMatrix = (sbyte[,])m_matrix.Clone(); //Использование предыдущего массива
            for (int y = 0; y < FieldHeight; ++y) //Цикл
                for (int x = 0; x < FieldWidth; ++x)
                {
                    if (tmpMatrix[y, x] == -1)
                        continue;
                    int count = 1;
                    for (int i = y + 1; i < FieldHeight; ++i)
                        if (tmpMatrix[i, x] == tmpMatrix[y, x])
                            count++;
                        else
                            break;
                    if (count >= 3)
                    {
                        for (int i = y; i < y + count; ++i)
                            tmpMatrix[i, x] = -1;
                        lines.Add(new Line(new Index(x, y), new Index(x, y + count - 1))); //Добавление в лист линий новые линии
                    }
                }

            if (lines.Count == 0) //Если линий нет, тогда возвращается false
                return false;

            int baseValue = 10; //Базовое значение
            foreach (Line line in lines) //Поиск линий по листу линий
            {
                int count = 0;
                //горизонтальная линия
                if (line.Start.Y == line.Finish.Y)
                    for (int i = line.Start.X; i <= line.Finish.X; ++i)
                    {
                        m_matrix[line.Start.Y, i] = -1;
                        if (ElementRemoved != null)
                            ElementRemoved(i, line.Start.Y);
                        count++;
                    }
                //вертикальная линия
                else
                    for (int i = line.Start.Y; i <= line.Finish.Y; ++i)
                    {
                        m_matrix[i, line.Start.X] = -1;
                        if (ElementRemoved != null)
                            ElementRemoved(line.Start.X, i);
                        count++;
                    }
                int value = (count - 2) * baseValue;
                m_score += count * value;
            }
            if (MatchesRemoved != null)
                MatchesRemoved();
            return true;
        }

        //Падение
        public bool Fall() 
        {
            List<Index> elements = new List<Index>(); //Создается лист элементов
            //сдвигаем каждый элемент вниз, если есть место
            for (int y = FieldHeight - 2; y >= 0; --y) //Цикл
                for (int x = FieldWidth - 1; x >= 0; --x) //Цикл
                {
                    if (m_matrix[y + 1, x] == -1) //Условие
                    {
                        m_matrix[y + 1, x] = m_matrix[y, x];
                        m_matrix[y, x] = -1;
                        elements.Add(new Index(x, y + 1)); //Добавление элемента в лист
                    }
                }
            //добавляем новые элементы сверху
            Random random = new Random();
            for (int x = 0; x < FieldWidth; ++x) //Цикл
                if (m_matrix[0, x] == -1) //Условие
                {
                    m_matrix[0, x] = (sbyte)random.Next(0, TypesCount);
                    elements.Add(new Index(x, 0)); //Добавление элемента в лист
                }
            if (ElementsFalled != null && elements.Count != 0) //Цикл
                ElementsFalled(elements);
            if (elements.Count == 0) //Условие
                return false;
            else
                return true;
        }

        //Получение значения
        public sbyte GetValue(int x, int y)
        {
            return m_matrix[y, x];
        }

        //Поминки мест
        public void Swap(Index a, Index b) 
        {
            sbyte t = m_matrix[a.Y, a.X]; //Обозначение переменной
            m_matrix[a.Y, a.X] = m_matrix[b.Y, b.X]; //Пузырьковая сортировка
            m_matrix[b.Y, b.X] = t; //Пузырьковая сортировка
        }

        //Получение очков
        public int GetScore()
        {
            return m_score;
        }
        
        public readonly int FieldWidth; //Ширина
        public readonly int FieldHeight; //Высота
        public readonly int TypesCount; //Количество типов
        
        sbyte[,] m_matrix; //Массив
        int m_score; //Очки

        public delegate void ElementRemoveHandler(int x, int y);
        public event ElementRemoveHandler ElementRemoved;

        public delegate void MatchesRemoveHandler();
        public event MatchesRemoveHandler MatchesRemoved;

        public delegate void ElementsFallHandler(List<Index> elements);
        public event ElementsFallHandler ElementsFalled;
    }


}
