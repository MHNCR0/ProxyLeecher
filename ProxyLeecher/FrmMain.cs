using ProxyLeecher.Leecher;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProxyLeecher.Leecher.proxynova;
using ProxyLeecher.Leecher.proxyserverlist24;
using System.Threading;
using System.IO;

namespace ProxyLeecher
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private List<ILeecher> InitLeechers()
        {
            List<ILeecher> Leechers = new List<ILeecher>();

            Leecher.proxynova.Leecher leecher1 = new Leecher.proxynova.Leecher();
            Leecher.proxyserverlist24.Leecher leecher2 = new Leecher.proxyserverlist24.Leecher();

            Leechers.Add(leecher1);
            Leechers.Add(leecher2);

            return Leechers;
        }

        private void btn_count_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrWhiteSpace(txt_proxyFilter.Text))
                MessageBox.Show("Please enter any proxy filter or choose...");
            else
            {
                Statics.Count = (int)txt_count.Value;
                Statics.PortFilter = txt_proxyFilter.Text;

                var Leechers = InitLeechers();

                Thread[] t = new Thread[Leechers.Count];

                for (int i = 0; i < Leechers.Count - 1; i++)
                {
                    t[i] = new Thread(() =>
                    {
                        Task.Factory.StartNew(() =>
                        {
                            Invoke((MethodInvoker)delegate
                            {
                                foreach (var p in Leechers[i].StartLeech())
                                {
                                    listBox1.Items.Add(p);
                                    Statics.added++;
                                }
                            });
                        });
                    });
                    t[i].IsBackground = true;
                    t[i].Start();
                }

                List<String> li = new List<string>();

                foreach (var b in listBox1.Items)
                    li.Add(b.ToString());

                String path = Application.StartupPath + "Proxys.txt";
                File.WriteAllLines(path, li);

                MessageBox.Show($"Proxys will save to {Application.StartupPath + "Proxys.txt"}");

            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("https://t.me/MHNCR0Soft");
        }
    }
}
