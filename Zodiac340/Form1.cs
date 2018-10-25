using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/// <summary>
/// Copyright (c) 2018 AThousandLittleIdeas.com. All Rights Reserved.
/// </summary>
namespace Zodiac340
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        // The main background worker
        BackgroundWorker bw = new BackgroundWorker();

        // The top match so far
        ProgressObject TopProgress;
        // The set of current candidates
        public List<Candidate> Candidates;
        Random r;
        private void Form1_Load(object sender, EventArgs e)
        {
            
            bw.WorkerReportsProgress = true;
            bw.DoWork += Bw_DoWork;
            bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
            bw.ProgressChanged += Bw_ProgressChanged;
            bw.RunWorkerAsync();
            r = new Random();

        }

        private void Bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (TopProgress == null)
                TopProgress = new ProgressObject() { Generation = 0 };

            ProgressObject po = (ProgressObject)e.UserState;

            // We have a new top contender.  Update the left text box
            if (TopProgress.TopCandidate.Fitness < (po.TopCandidate.Fitness))
            {
                TopProgress = po;
                updateTopCandidate();
            }

            // Update the right text box with info ont he best of this generation
            updateLog(po);

        }

        /// <summary>
        /// Update the right textbox with the best of every generation
        /// </summary>
        /// <param name="po"></param>
        private void updateLog(ProgressObject po)
        {
            txtLog.AppendText(string.Format(Environment.NewLine + Environment.NewLine + "Generation: {0}"
                + Environment.NewLine + "Fitness: {1}"
                + Environment.NewLine + "Candidates: {2}"
                + Environment.NewLine + "Key: {3}"
                + Environment.NewLine + "Result:" +
                Environment.NewLine + "{4}" ,
                po.Generation, po.TopCandidate.Fitness, po.Candidates, po.TopCandidate.CipherKey, po.TopCandidate.ResultString));
            

               
        }

        /// <summary>
        /// Update the overall top candidate into the left text box
        /// </summary>
        private void updateTopCandidate()
        {
            txtTopMatch.Text = string.Empty;
            txtTopMatch.Text = string.Format("Generation: {0}"
                + Environment.NewLine + "Fitness: {1}"
                + Environment.NewLine + "Key: {2}"
                + Environment.NewLine + "Result:" +
                Environment.NewLine + "{3}", TopProgress.Generation, TopProgress.TopCandidate.Fitness, TopProgress.TopCandidate.CipherKey, TopProgress.TopCandidate.ResultString);
        }

    
        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //nothing
        }
        
        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            int generations = 10000;
            double mutationProbability = 0.20;
            // An even number, please
            int maxCandidates = 100;
            if(Candidates==null)
            {
                Candidates = new List<Candidate>();
                for(int i=0;i<maxCandidates;i++)
                {
                    // For the first round, add random candidates
                    Candidates.Add(new Candidate());
                }
            }

            // Loop a set amount of generations
            for (int g = 0; g < generations; g++)
            {
                int cnt = Candidates.Count;
                
                // The top one should be the first one after orderimg
                Candidates = Candidates.OrderByDescending(c => c.Fitness).ToList();

                // Report back the top candidate from this generation
                ProgressObject po = new ProgressObject() { Generation = g, TopCandidate = Candidates[0], Candidates = cnt };
                bw.ReportProgress(0,po);
                
              
                    Candidates.RemoveRange(cnt / 2, cnt / 2);
            
                
                cnt = Candidates.Count;
               
                // Assume an even count, crossover/mate the strongest ones with a random candidate
                for(int i=0;i<cnt;i++)
                {
                    int mate = r.Next(0, cnt - 1);
                    while(mate==i)
                    {
                        mate = r.Next(0, cnt - 1);
                    }
                    Candidate cnd=Candidates[i].MateWith(Candidates[mate], mutationProbability);
                    
                    Candidates.Add(cnd);
                 
    
                }
            }
        }
    }
}
