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
 //Меню
{
    public partial class Form2 : Form
    {
        public Form1 frm1; //объявляем Form1 и даем ей имя frm1
        public Form3 frm3; //объявляем Form3 и даём ей имя frm3
        public GameScreen frm4; //объвление формы с игрой

        Bitmap Pict; //переменная для рисунка
        Graphics g; //графическая поверхность для рисунка

        Pen my_pen = new Pen(Color.Black, 1); //создание пера
        SolidBrush my_brush = new SolidBrush(Color.Pink); //цвет текста
        Font my_font = new Font("Arial", 14, FontStyle.Bold); //шрифт текста
        public Form2()
        {
            InitializeComponent();
            g = this.CreateGraphics(); //создаем поверхность
            this.Width = 1024;
            this.Height = 576;
        }

        //Отвечает за фон и указатели
        private void Form2_Load(object sender, EventArgs e)
        {
            Pict = new Bitmap("D:/pict/2.png"); //для дома
            frm1 = new Form1(); //создаем указатель на форму 1
            frm3 = new Form3(); //создаем указатель на форму 3
            frm4 = new GameScreen(); //создаем указатель на форму 4
        }

        private void Form2_Paint(object sender, PaintEventArgs e)
        {
            g.DrawImage(Pict, 0, 0, Pict.Width, Pict.Height);
        }

        //Игра
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide(); //прячем форму 2
            frm4.Show(); //показывем форму 4
        }

        //Помощь
        private void button2_Click(object sender, EventArgs e)
        {
            frm3 = new Form3(); //создаем указатель на форму 3
            frm3.frm2 = this; //передаем форме 3 указатель на форму 2
            frm3.Show(); //показываем форму 3
            this.Hide(); //прячем форму 2
        }

        //Назад
        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide(); //прячем форму 2
            frm1.Show(); //показывем форму 1
        }
    }
}
