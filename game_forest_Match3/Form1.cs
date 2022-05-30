using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace game_forest_Match3
//Начальная страница
{
    public partial class Form1 : Form
    {
        public Form2 frm2;  //объявляем Form2 и даем ей имя frm2

        Bitmap Pict; //переменная для рисунка
        Graphics g; //графическая поверхность для рисунка

        SolidBrush my_brush = new SolidBrush(Color.Pink);//цвет текста
        Font my_font = new Font("Arial", 14, FontStyle.Bold);//шрифт

        public Form1()
        {
            InitializeComponent();
            g = this.CreateGraphics(); //создаем поверхность
            this.Width = 1024;
            this.Height = 576;
        }

        //Отвечает за фон и указатели
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            g.DrawImage(Pict, 0, 0, Pict.Width, Pict.Height);
            g.DrawString("Быстрова Алиса 107г1\nПреподаватель Календарёва СТ\nГод создания 2022", my_font, my_brush, 25, 710);
        }

        private void Form1_Load_1(object sender, EventArgs e)
        {
            Pict = new Bitmap("D:/pict/корол.png"); //для дома

            frm2 = new Form2(); //создаем указатель на форму 2
        }

        //Меню
        private void button1_Click(object sender, EventArgs e)
        {
            frm2 = new Form2(); //создаем указатель на форму 2
            frm2.frm1 = this; //передаем форме 2 указатель на форму 1
            frm2.Show(); //показываем форму 2
            this.Hide(); //прячем форму 1
        }

        //Выход
        private void button2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }
}
