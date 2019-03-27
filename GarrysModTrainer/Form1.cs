using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Memory;

namespace GarrysModTrainer
{
    public partial class Form1 : Form
    {

		//offsets
        const string LocalPlayer = "client.dll+0x655924";
        const string EntityPlayer = "client.dll+0x00671774";
        const string Health = LocalPlayer + ",0x90";
        const string epZ = EntityPlayer + "0x264";
        const string EHealth = EntityPlayer + ",0x90";
        const string Velocity = LocalPlayer + ",0xF4";
        const string Jump = LocalPlayer + ",0x350";
        const string inAttack = "client.dll+0x6D94E8";
		
        int attack;
        int plyHealth;
        int vel;
        int injump;
        [DllImport("user32", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int GetAsyncKeyState(int vKey);

        public Form1()
        {
            InitializeComponent();
        }

        public Mem m = new Mem();

        private void hackrz_DoWork(object sender, DoWorkEventArgs e)
        {
            int pID = m.getProcIDFromName("hl2");
            bool openProc = false;
            openProc = m.OpenProcess(pID);
            while (true)
            {
                if (openProc)
                {
                    //m.writeMemory("client.dll+0x6D94E8", "int", "5"); //autoclicker
                    //Thread.Sleep(1);
                    //m.writeMemory("client.dll+0x6D94E8", "int", "4");
                    if (bhopCheckBox.Checked)
                    {
                        if (GetAsyncKeyState(32) > 0)
                        {
                            if (injump == 257)
                            {
                                m.writeMemory("client.dll+0x6D94DC", "int", "5");
                                Thread.Sleep(2);
                                m.writeMemory("client.dll+0x6D94DC", "int", "4");
                            }
                        }
                    }
                    //if(GetAsyncKeyState(32) > 0)
                    //{
                    //    m.writeMemory("client.dll+0x6D94E8", "int", "5"); //autoclicker
                    //    Thread.Sleep(1);
                    //    m.writeMemory("client.dll+0x6D94E8", "int", "4");
                    //}
                    injump = m.readInt(Jump);
                    plyHealth = m.readByte(EHealth);
                    vel = m.readByte(Velocity);
                    attack = m.readByte(inAttack);

                    label1.Text = "" + plyHealth;
                    label2.Text = "" + attack;
                    //m.writeMemory(Jump, "int", "256");
                }
            }
        }

        private bool inAir()
        {
            if (injump == 257)
                return false;
            else
                return true;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (!hackrz.IsBusy)
                hackrz.RunWorkerAsync();
        }
    }
}
