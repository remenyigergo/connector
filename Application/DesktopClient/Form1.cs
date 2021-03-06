﻿using DesktopClient.Modules.ApplicationManager;
using DesktopClient.Modules.MPCManager;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DesktopClient
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Task.Run(async () =>
            {
                new MPC().MPCManager();
            });


            Task.Run(async () =>
            {
                new ApplicationManager().RunApplicationManager();
            });
        }
    }
}