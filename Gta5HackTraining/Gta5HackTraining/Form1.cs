using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gta5HackTraining
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}
		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case 0x84:
					base.WndProc(ref m);
					if ((int)m.Result == 0x1)
						m.Result = (IntPtr)0x2;
					return;
			}

			base.WndProc(ref m);
		}
		int sira = 0;
		int tick = 0;
		int time = 0;
		bool animationStopped = false;
		int fails = 0;
		int completedColumnNumbers = 0;
		string[] rows = { "a", "b", "c", "d", "e", "f" };
		int[] randoms;
		void resetCircles()
		{
			foreach (var item in groupBox1.Controls)
			{
				OvalPBox pb = (OvalPBox)item;
				pb.BackColor = Color.Black;
			}
		}
		void resetVariables()
		{
			lblErr.Text = "";
			lblTime.Text = "";
			time = 0;
			sira = 0;
			tick = 0;
			fails = 0;
			completedColumnNumbers = 0;
			animationStopped = false;
			timer1.Stop();
		}

		void setPassword()
		{
			randoms = new int[6];
			Random rnd = new Random();

			for (int i = 0; i < 6; i++)
			{
				randoms[i] = rnd.Next(1, 6);
				OvalPBox pb = (OvalPBox)groupBox1.Controls.Find((rows[i] + randoms[i]), true)[0];
				pb.BackColor = Color.Cyan;
			}
		}
		void repeatPassword()
		{
			for (int i = 0; i < 6; i++)
			{
				OvalPBox pb = (OvalPBox)groupBox1.Controls.Find((rows[i] + randoms[i]), true)[0];
				pb.BackColor = Color.Cyan;
			}
		}
		private void button1_Click(object sender, EventArgs e)
		{
			timer1.Interval = 500;
			timer1.Start();
			
		}
		private void timer1_Tick(object sender, EventArgs e)
		{
			if (tick < 10)
			{
				if (tick % 2 == 0) { if (tick < 6) setPassword(); else repeatPassword(); }
				else { resetCircles(); }
				tick++;
			}
			else
			{
				if (!animationStopped) animationStopped = true;
				if (timer1.Interval != 1000) timer1.Interval = 1000;
				lblTime.Text = time.ToString();
				time++;

			}

		}

		private void Form1_Load(object sender, EventArgs e)
		{
			SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
			foreach (var item in groupBox1.Controls)
			{
				OvalPBox pb = (OvalPBox)item;
				pb.Click += Pb_Click;
			}
		}

		private void Pb_Click(object sender, EventArgs e)
		{
			OvalPBox t = (OvalPBox)sender;
			int column = Array.IndexOf(rows, t.Name[0].ToString());
			if (animationStopped && sira == column)
			{


				int choosen = int.Parse(t.Name[1].ToString());

				if (choosen == randoms[sira])
				{
					t.BackColor = Color.Cyan;
					completedColumnNumbers++;
					if (completedColumnNumbers == 6)
					{

						string text = "Tamamlandı Süre: " + time.ToString() + "sn Hata Sayısı: " + fails.ToString();
						MessageBox.Show(text,"Sonuç",MessageBoxButtons.OK,MessageBoxIcon.Information);
						resetVariables();
						resetCircles();
						return;

					}
					sira++;

				}
				else
				{
					t.BackColor = Color.Red;
					fails++;
					lblErr.Text = fails.ToString();
				}
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}
	}
}
