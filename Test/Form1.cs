using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmMedigo frm = new frmMedigo();
            frm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmDuocquocgia frm = new frmDuocquocgia();
            frm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            frmGPB frm = new frmGPB();
            frm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            SignVisnam frm = new SignVisnam();
            frm.Show();
        }
    }
}
