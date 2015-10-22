﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rebot
{
    public partial class ProcessoWindow : Form
    {
        private bool isRodando = false;
        private int id;
        private string tituloProcesso;
        public List<string> memoria = new List<string>();
        Tasker task = new Tasker();
        BackgroundWorker worker;


        public ProcessoWindow(string var_nao_tratada)
        {
            // TRATANDO DO PARAMETRO DE ENTRADA
            string[] divisão = var_nao_tratada.Split(' ');
          tituloProcesso = divisão[1].ToString();
          id =  Convert.ToInt32(divisão[0]);
            // ==================================
          InitializeComponent();
            // ==== Setar a GUI ====================
            Text = titulo.Text = tituloProcesso;
            idProcesso.Text = divisão[0];
            // ===================================
            worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.DoWork += Worker_DoWork;
            worker.ProgressChanged += workerRetorno;
            worker.RunWorkerCompleted += workercompleto;
        }


        private void btnScan_Click(object sender, EventArgs e)
        {
            isRodando = !isRodando;
            if (isRodando)
            {
                if (pesquisa.Text != "")
                {
                    BtScan.Text = "Cancelar";
                    ulmem.CellBorderStyle = TableLayoutPanelCellBorderStyle.None;
                    memoria.Clear();
                    worker.RunWorkerAsync();
                    task.isRodando = true;
                    ulmem.Controls.Clear();
                }
                else
                {
                    MessageBox.Show("È muita coisa para pesquisar!");
                }
            }
            else
            {
                BtScan.Text = "Scan";
                task.isRodando = false;
                memoria = task.memoria;
              
            }
            
        }
        // RETORNO DO WORKER
        private void workerRetorno(object sender, ProgressChangedEventArgs e)
        {
            // Console.WriteLine(e.UserState + " progresso = "+e.ProgressPercentage);
            progresso.Value = e.ProgressPercentage;
 
        }
        // WOKER FAZER TRABALHO
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            task.lerProcesso(id, worker,pesquisa.Text);
           
        }
       // WORKER COMPLETO
        private void workercompleto(object sender, RunWorkerCompletedEventArgs e)
        {
            ulmem.Enabled = false;
            isRodando = false;
            listarMemoria();
            BtScan.Text = "Scan";
            Console.WriteLine("Completo");
           

        }
        private void ProcessoWindow_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
            memoria.Clear();
            task.isRodando = false;
        }

        private void listarMemoria()
        {
            for (int i = 0; i < memoria.Count; i++)
            {
                Console.WriteLine(" tem +" + memoria.Count);
                string[] divisão = memoria[i].Split(':');
                ulmem.Controls.Add(new Label() { Text = divisão[1] }, 0, i - 1);
                ulmem.Controls.Add(new TextBox() { Text = divisão[0], ReadOnly = true, BackColor = BackColor, BorderStyle = 0, ForeColor = System.Drawing.Color.White }, 0, i - 1);
                ulmem.Controls.Add(new TextBox() { Text = " " }, 2, i - 1);
            }
            ulmem.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            ulmem.Enabled = true;
            memoria.Clear();
        }
    }
}
