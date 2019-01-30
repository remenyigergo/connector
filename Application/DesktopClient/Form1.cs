using System.Windows.Forms;
using DesktopClient.Modules.ApplicationManager;
using DesktopClient.Modules.ProcessManager;
using DesktopClient.Modules.MPCManager;

namespace DesktopClient
{
    public partial class Form1 : Form
    {
        

        public Form1()
        {
            InitializeComponent();

            new MPC().MPCManager();
            new ApplicationManager().RunApplicationManager();
        }

        
    }
}
