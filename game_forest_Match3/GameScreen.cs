using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace game_forest_Match3
//Окно
{
    public partial class GameScreen : Form
    {
        public GameScreen()
        {
            InitializeComponent();
        }

        private void GameScreen_Load(object sender, EventArgs e) //Рисует
        {
            Paint += delegate { //Указатель на метод
                gameField.Refresh(); //Метод
            };

            m_timerCount = 60; //Таймер
            m_timer = new Timer();
            m_timer.Interval = 1000; //1 секунда
            timerLabel.Text = (m_timerCount / 60).ToString("00") + ":" + (m_timerCount - 60 * (m_timerCount / 60)).ToString("00"); //Вычисление времени
            m_timer.Tick += delegate //Указатель на метод
            {
                if (m_timerCount > 0)
                {
                    m_timerCount--;
                    timerLabel.Text = (m_timerCount / 60).ToString("00") + ":" + (m_timerCount - 60 * (m_timerCount / 60)).ToString("00");
                }
                //игра может завершиться, только когда активна
                else if (m_active)
                    Finish();
            };

            m_game = new Game(fieldSize, fieldSize, Enum.GetValues(typeof(ElementType)).Length); //Размеры картинок
            m_game.ElementRemoved += Game_ElementRemoved; //Метод удаления элементов
            m_game.MatchesRemoved += Game_MatchesRemoved; //Метод вывода очков
            m_game.ElementsFalled += Game_ElementsFalled; //Метод заполнения элементов
            m_game.FillMatrix(); //Метод создания матрицы

            gameField.Paint += GameField_Paint; //Метод рисования элементов

            m_elementSize = Math.Min(gameField.Width, gameField.Height) / fieldSize; //Размер фотографий элементов
            m_bitmaps = new Bitmap[5]; //Хранит файлы в пиксельном виде
            m_elements = new VisualElement[fieldSize, fieldSize]; //Вызов класса

            UpdateBitmaps(); //Вызов метода загрузки
            InitElements(); //Вызов метода работы с элементом
            UpdateElements(); //Вызов метода обновления элементов
            scoreLabel.Text = "Score: " + m_game.GetScore().ToString(); //Начальная строка счета (0)

            FormBorderStyle = FormBorderStyle.None; //Стиль окна
            WindowState = FormWindowState.Maximized; //Максимальный размер окна

            Active = true;

            m_timer.Start();
        }

        //Рисование самоцветиков из фоточек
        private void GameField_Paint(object sender, PaintEventArgs e)
        {
            for (int y = 0; y < fieldSize; ++y)
                for (int x = 0; x < fieldSize; ++x)
                {
                    VisualElement el = m_elements[y, x];
                    if (el.BackColor != Color.Transparent)
                    {
                        SolidBrush b = new SolidBrush(el.BackColor);
                        e.Graphics.FillRectangle(b, el.Rectangle);
                    }
                    if (el.Image != null)
                        e.Graphics.DrawImage(el.Image, el.Rectangle);
                }
        }

        //Удаление элементов
        private void Game_ElementRemoved(int x, int y)
        {
            m_elements[y, x].Image = null;
        }

        //Вывод очков
        private void Game_MatchesRemoved()
        {
            scoreLabel.Text = "Score: " + m_game.GetScore().ToString();
            //UpdateElements();
            m_game.Fall();
                //Active = true;
        }

        //Падение
        private void Game_ElementsFalled(List<Index> indices) //Метод 
        {
            FallAnimation anim = new FallAnimation(this); //Анимация падения
            List<VisualElement> elements = new List<VisualElement>(); //Лист элементов
            foreach (Index index in indices) //Цикл поиска индекса в листе индексов
                elements.Add(m_elements[index.Y, index.X]); //В лист элементов добавлются несколько элементов
            anim.AnimationEnd += delegate //Анимация падения завершена
            {
                if (m_game.Fall()) //Метод логики падения
                {

                    //UpdateElements();
                }
                else if (m_game.RemoveMatches() == false)
                    Active = true;
            };
            UpdateElements();
            anim.Start(elements);
        }

        //Финиш
        public void Finish()
        {
            m_active = false;
            MessageBox.Show("Игра окончена. Ваши очки: " + m_game.GetScore().ToString()+"\nДля выхода нажмите Esc", "Конец", MessageBoxButtons.OK);
            Close();
        }

        //Загрузка картинок
        public void UpdateBitmaps()
        {
            if (m_elementSize == 0)
                return;
            Size s = new Size(m_elementSize, m_elementSize);
            m_bitmaps[0] = new Bitmap(Properties.Resources.gem_red, s);
            m_bitmaps[1] = new Bitmap(Properties.Resources.gem_yellow, s);
            m_bitmaps[2] = new Bitmap(Properties.Resources.gem_green, s);
            m_bitmaps[3] = new Bitmap(Properties.Resources.gem_blue, s);
            m_bitmaps[4] = new Bitmap(Properties.Resources.gem_white, s);
        }

        //Обновление элементов
        public void UpdateElements()
        {
            Point start = new Point((gameField.Width - m_elementSize * fieldSize) / 2,
                                    (gameField.Height - m_elementSize * fieldSize) / 2);
            for (int y = 0; y < fieldSize; ++y)
                for (int x = 0; x < fieldSize; ++x)
                {
                    m_elements[y, x].Location = new Point(start.X + x * m_elementSize, start.Y + y * m_elementSize);
                    m_elements[y, x].Size = new Size(m_elementSize, m_elementSize);
                    sbyte value = m_game.GetValue(x, y);
                    if (value >= 0)
                        m_elements[y, x].Image = m_bitmaps[m_game.GetValue(x, y)];
                    else
                        m_elements[y, x].Image = null;
                }
        }

        //Работа с элементом 
        public void InitElements()
        {
            for (int y = 0; y < fieldSize; ++y)
                for (int x = 0; x < fieldSize; ++x)
                {
                    m_elements[y, x] = new VisualElement(this);
                    m_elements[y, x].Index = new Index(x, y);
                }
        }

        //Размер окна
        private void GameScreen_Resize(object sender, EventArgs e)
        {
            m_elementSize = Math.Min(gameField.Width, gameField.Height) / fieldSize;
            UpdateBitmaps();
            UpdateElements();
        }

        //Перемещение элементов
        private void swapElements(Index a, Index b)
        {
            VisualElement tmpEl = m_elements[a.Y, a.X];
            m_elements[a.Y, a.X] = m_elements[b.Y, b.X];
            m_elements[b.Y, b.X] = tmpEl;

            m_elements[a.Y, a.X].Index = new Index(a.X, a.Y);
            m_elements[b.Y, b.X].Index = new Index(b.X, b.Y);
        }

        //Анимация перемещения элементов
        public void MoveElements(VisualElement a, VisualElement b)
        {
            Index aInd = new Index(a.Index);
            Index bInd = new Index(b.Index);
            m_game.Swap(aInd, bInd);
            swapElements(aInd, bInd);

            bool result = m_game.RemoveMatches();
            if (result == false)
            {
                SwapAnimation swap = new SwapAnimation(this);
                swap.AnimationEnd += delegate
                {
                    //возврат                
                    aInd = new Index(a.Index);
                    bInd = new Index(b.Index);
                    m_game.Swap(aInd, bInd);
                    swapElements(aInd, bInd);
                    Active = true;
                };
                swap.Start(b, a);
            }
        }

        //Выход
        private void GameScreen_KeyDown(object sender, KeyEventArgs e)
        {
            //выход на ESC
            if (e.KeyCode == Keys.Escape && m_active)
            {
                m_timer.Dispose();
                Close();
            }
        }

        //Определение элемента
        private void gameField_MouseDown(object sender, MouseEventArgs e)
        {
            //определение элемента, на который нажали
            Point start = new Point((gameField.Width - m_elementSize * fieldSize) / 2,
                                    (gameField.Height - m_elementSize * fieldSize) / 2);
            Point pos = new Point(e.Location.X - start.X, e.Location.Y - start.Y);
            int col = pos.X / m_elementSize;
            int row = pos.Y / m_elementSize;
            if (col < fieldSize && row < fieldSize)
                m_elements[row, col].Click();
        }

        //размер можно поставить и больше
        const int fieldSize = 8;

        Bitmap[] m_bitmaps;
        VisualElement[,] m_elements;
        Game m_game;
        Timer m_timer;
        int m_elementSize;
        int m_timerCount;
        bool m_active;
        public bool Active
        {
            get { return m_active; }
            set
            {
                //игра может завершиться, только когда активна
                if (m_active == false && value == true && m_timerCount <= 0)
                    Finish();
                else
                    m_active = value;
            }
        }
    }

    //Список элементов
    public enum ElementType
    {
        Red = 0,
        Yellow,
        Green,
        Blue,
        White
    }

    //Класс взаимодействия с элементами
    public class VisualElement
    {
        public VisualElement(GameScreen game) : base()
        {
            m_game = game;
        }

        //Управление мышкой
        public void Click()
        {
            if (m_game.Active == false)
                return;
            if (checkedElement != null)
            {
                if (checkedElement == this)
                {
                    //отмена выбора
                    m_backColor = Color.Transparent;
                    checkedElement = null;
                }
                else
                {
                    bool near = false;

                    if ((checkedElement.m_index.X == m_index.X - 1 && checkedElement.m_index.Y == m_index.Y) ||
                        (checkedElement.m_index.X == m_index.X + 1 && checkedElement.m_index.Y == m_index.Y) ||
                        (checkedElement.m_index.X == m_index.X && checkedElement.m_index.Y == m_index.Y - 1) ||
                        (checkedElement.m_index.X == m_index.X && checkedElement.m_index.Y == m_index.Y + 1))
                        near = true;

                    if (!near)
                    {
                        //выбор другого элемента
                        checkedElement.m_backColor = Color.Transparent;
                        m_backColor = Color.DarkSlateBlue;
                        checkedElement = this;
                    }
                    else
                    {
                        //проверка двух соседних элементов
                        checkedElement.m_backColor = Color.Transparent;
                        m_game.Active = false;
                        VisualElement el = checkedElement;
                        checkedElement = null;
                        SwapAnimation swap = new SwapAnimation(m_game);
                        swap.AnimationEnd += delegate
                        {
                            m_game.MoveElements(el, this);
                        };
                        swap.Start(el, this);
                    }
                }
            }
            else
            {
                //выбор элемента
                m_backColor = Color.DarkSlateBlue;
                checkedElement = this;
            }
            m_game.Refresh();
        }

        public Index Index { get { return m_index; } set { m_index = new Index(value); } } //Класс индексов
        public ElementType Type { get { return m_type; } set { m_type = value; } } //Класс типов элементов
        public Point Location { get { return m_rect.Location; } set { m_rect = new Rectangle(value, m_rect.Size); } } //Класс очков
        public Size Size { get { return m_rect.Size; } set { m_rect = new Rectangle(m_rect.Location, value); } } //Класс размеров
        public Rectangle Rectangle { get { return m_rect; } set { m_rect = value; } } //Класс окна
        public Color BackColor { get { return m_backColor; } set { m_backColor = value; } } //Класс фона
        public Image Image { get { return m_image; } set { m_image = value; } } //Класс картинок

        //Объявление пременных
        Index m_index;
        ElementType m_type;
        GameScreen m_game;
        Rectangle m_rect;
        Color m_backColor;
        Image m_image;

        static VisualElement checkedElement = null; //Проверяемый элемент пустой
    };
}
