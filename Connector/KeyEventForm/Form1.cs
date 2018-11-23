using DesktopClient.Modules.Helpers;
using DesktopClient.Modules.SubtitleManager;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using DesktopClient.Modules.MPCManager;

namespace DesktopClient
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();

            new MPC().MPCManager();

        }

        
    }
}
