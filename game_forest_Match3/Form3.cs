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
//Помощь
{
    public partial class Form3 : Form
    {
        public Form1 frm1; //объявляем Form1 и даём ей имя frm1
        public Form2 frm2; //объявляем Form2 и даём ей имя frm2
        public Form5 frm5; //объявляем Form5 и даём ей имя frm5
        public Form6 frm6; //объявляем Form6 и даём ей имя frm6
        public Form7 frm7; //объявляем Form7 и даём ей имя frm7

        Bitmap Pict; //переменная для рисунка
        Graphics g; //графическая поверхность для рисунка

        public Form3() //объявили форму 3
        {
            InitializeComponent();
            g = this.CreateGraphics(); //создаем поверхность
            this.Width = 1024;
            this.Height = 576;
        }

        //Отвечает за фон и указатели
        private void Form3_Load(object sender, EventArgs e)
        {
            Pict = new Bitmap("D:/pict/помощь.png"); //для дома
            frm1 = new Form1(); //создаем указатель на форму 5
            frm2 = new Form2(); //создаем указатель на форму 2
        }

        private void Form3_Paint_1(object sender, PaintEventArgs e)
        {
            g.DrawImage(Pict, 0, 0, Pict.Width, Pict.Height);
        }

        //Суть игры
        private void button2_Click(object sender, EventArgs e)
        {
            frm5 = new Form5(); //создаем указатель на форму 5
            frm5.frm3 = this; //передаем форме 5 указатель на форму 3
            frm5.Show(); //показываем форму 5
            this.Hide(); //прячем форму 3
        }

        //Управление
        private void button3_Click(object sender, EventArgs e)
        {
           frm6 = new Form6(); //создаем указатель на форму 6
           frm6.frm3 = this; //передаем форме 5 указатель на форму 3
           frm6.Show(); //показываем форму 6
            this.Hide(); //прячем форму 3
        }

        //Сюжет игры
        private void button4_Click(object sender, EventArgs e)
        {
            frm7 = new Form7(); //создаем указатель на форму 7
            frm7.frm3 = this; //передаем форме 5 указатель на форму 3
            frm7.Show(); //показываем форму 7
            this.Hide(); //прячем форму 3
        }

        //Назад
        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide(); //прячем форму 3
            frm2.Show(); //показывем форму 2
        }

    }
}
