using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WFM.Mobile.OOS
{
    public partial class Splash : Form
    {
        public Splash()
        {
            InitializeComponent();
            label1.Text = Global.AssemblyVersion;
        }

        private void Splash_Load(object sender, EventArgs e)
        {
            
        }


        
    }
}