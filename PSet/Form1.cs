using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace PSet
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Process[] processes;
        ProcessPriorityClass Pclass;
        bool Pboost;
        int[] cores = new int[Environment.ProcessorCount];
        int core;
        int dtWidth = 0;
        bool fmoving = false;
        Point spoint = new Point(0, 0);
        private void Form1_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < cores.Length; i++)
            {
                if (i == 0)
                {
                    cores[i] = 1;
                }
                else
                {
                    cores[i] = cores[i - 1] + cores[i - 1];
                }
            }
            dataGridView1.Columns.Add("Pid", "PID");
            dataGridView1.Columns.Add("PName", "Name");
            dataGridView1.Columns.Add("PStat", "Priority");
            dataGridView1.Columns.Add("PBoost", "Boost");
            comboBox1.SelectedText = "Low";
            Refresh_Table();
        }
        private void Apply_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedItem)
            {
                case "Realtime":
                    Pclass = ProcessPriorityClass.RealTime;
                    Pboost = true;
                    break;
                case "High":
                    Pclass = ProcessPriorityClass.High;
                    Pboost = true;
                    break;
                case "Above Normal":
                    Pclass = ProcessPriorityClass.AboveNormal;
                    Pboost = true;
                    break;
                case "Normal":
                    Pclass = ProcessPriorityClass.Normal;
                    Pboost = false;
                    break;
                case "Below Normal":
                    Pclass = ProcessPriorityClass.BelowNormal;
                    Pboost = false;
                    break;
                case "Low":
                    Pclass = ProcessPriorityClass.Idle;
                    Pboost = false;
                    break;
                default:
                    Pclass = ProcessPriorityClass.Idle;
                    Pboost = false;
                    break;
            }
            processes = Process.GetProcesses();
            foreach (DataGridViewRow dataGridViewRow in dataGridView1.SelectedRows)
            {
                foreach (var proc in processes)
                {
                    if (proc.Id == (int)dataGridViewRow.Cells["Pid"].Value)
                    {
                        try
                        {
                            core = 0;
                            if (Environment.ProcessorCount >= 12)
                            {
                                if (proc.ProcessName == "svchost" || proc.ProcessName == Process.GetCurrentProcess().ProcessName)
                                {
                                    proc.PriorityClass = Pclass;
                                }
                                else
                                {
                                    proc.PriorityClass = Pclass;
                                    proc.PriorityBoostEnabled = Pboost;
                                    if (Pclass == ProcessPriorityClass.Idle)
                                    {
                                        for (int a = 0; a < cores.Length; a++)
                                        {
                                            if (a == 0 || a == 1 || a == (cores.Length / 2) || a == cores.Length / 2 + 1)
                                            {
                                                
                                            }
                                            else
                                            {
                                                core += cores[a];
                                            }
                                        }
                                        proc.ProcessorAffinity = (IntPtr)core;
                                    }
                                    else if (Pclass == ProcessPriorityClass.BelowNormal)
                                    {
                                        for (int a = 0; a < cores.Length; a++)
                                        {
                                            if (a == 0 || a == 1 || a == (cores.Length / 2) || a == cores.Length / 2 + 1)
                                            {

                                            }
                                            else
                                            {
                                                core += cores[a];
                                            }
                                        }
                                        proc.ProcessorAffinity = (IntPtr)core;
                                    }
                                    else if (Pclass == ProcessPriorityClass.Normal)
                                    {
                                        for (int a = 0; a < cores.Length; a++)
                                        {
                                            if (a == 0 || a == 1 || a == (cores.Length / 2) || a == cores.Length / 2 + 1)
                                            {

                                            }
                                            else
                                            {
                                                core += cores[a];
                                            }
                                        }
                                        proc.ProcessorAffinity = (IntPtr)core;
                                    }
                                    else if (Pclass == ProcessPriorityClass.AboveNormal)
                                    {
                                        for (int a = 0; a < cores.Length; a++)
                                        {
                                            if (a == 0 || a == (cores.Length / 2) - 1)
                                            {

                                            }
                                            else
                                            {
                                                core += cores[a];
                                            }
                                        }
                                        proc.ProcessorAffinity = (IntPtr)core;
                                    }
                                    else if (Pclass == ProcessPriorityClass.High)
                                    {
                                        for (int a = 0; a < cores.Length; a++)
                                        {
                                            if (a == 0 || a == (cores.Length / 2) - 1)
                                            {

                                            }
                                            else
                                            {
                                                core += cores[a];
                                            }
                                        }
                                        proc.ProcessorAffinity = (IntPtr)core;
                                    }
                                    else if (Pclass == ProcessPriorityClass.RealTime)
                                    {
                                        for (int a = 0; a < cores.Length; a++)
                                        {
                                            core += cores[a];
                                        }
                                        proc.ProcessorAffinity = (IntPtr)core;
                                    }
                                }
                            }
                            else
                            {
                                if (proc.ProcessName == "svchost" || proc.ProcessName == Process.GetCurrentProcess().ProcessName)
                                {
                                    
                                }
                                else
                                {
                                    proc.PriorityClass = Pclass;
                                }
                            }
                        }
                        catch { }
                    }
                }
            }
            Refresh_Table();
        }
        private void Refresh_Table()
        {
            dtWidth = 0;
            dataGridView1.Rows.Clear();
            processes = Process.GetProcesses();
            foreach (var proc in processes)
            {
                try
                {
                    dataGridView1.Rows.Add(proc.Id, proc.ProcessName + ".exe", proc.PriorityClass, proc.PriorityBoostEnabled);
                }
                catch { }
            }
            dataGridView1.Sort(dataGridView1.Columns[1], ListSortDirection.Ascending);
            foreach (DataGridViewColumn clmn in dataGridView1.Columns)
            {
                dtWidth += clmn.Width;
            }
            dataGridView1.Width = dtWidth + 20;
            this.Width = dataGridView1.Width + 25;
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Refresh_Table();
        }

        private void Label1_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Application.Exit();
            }
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            fmoving = true;
            spoint = new Point(e.X, e.Y);
        }

        private void Form1_MouseUp(object sender, MouseEventArgs e)
        {
            fmoving = false;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            if (fmoving)
            {
                Point p = PointToScreen(e.Location);
                Location = new Point(p.X - this.spoint.X, p.Y - this.spoint.Y);
            }
        }

        private void Label2_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
    }
}