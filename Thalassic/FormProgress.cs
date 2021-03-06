﻿using System;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Thalassic
{
    public partial class FormProgress : Form
    {
        private readonly Thread Thread;
        public FormProgress(Thread thread)
        {
            InitializeComponent();
            Thread = thread;
        }

        private void FormProgress_Load(object sender, EventArgs e)
        {

        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (Thread.IsAlive)
            {
                Invoke((MethodInvoker)delegate
                {
                    var num = LabelWorking.Text.Count(c => c.Equals('.'));
                    num++;
                    if (num >= 5)
                    {
                        num = 1;
                    }

                    var text = "Working";
                    for (int i = 0; i < num; i++)
                    {
                        text += ".";
                    }

                    LabelWorking.Text = text;
                });
            }
            else
            {
                Close();
            }
        }
    }
}
