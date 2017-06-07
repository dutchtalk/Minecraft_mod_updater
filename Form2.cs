using System;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using Ionic.Zip;
using System.Diagnostics;

namespace MinecraftModUpdater
{

    public partial class Form2 : Form
    {
        public string ModsDir { get; set; }
        public string ConfigDir { get; set; }
        public string ZipPath { get; set; }
        public string ExtractPath { get; set; }
        public Form2(String dir, String zip)
        {
            InitializeComponent();
            labelLoad.Text = "";
            labelLoadDesc.Text = "";
            this.Show();
            ModsDir = Path.Combine(dir, "mods/");
            ConfigDir = Path.Combine(dir, "config/");
            ZipPath = zip;
            
            Work();
        }

        private void Work()
        {
            
            BackgroundWorker worker = new BackgroundWorker();
            worker.WorkerReportsProgress = true;
            labelLoad.Text = "Even geduld...";
            worker.ProgressChanged += (o, e) => { this.progressBar1.Value = e.ProgressPercentage; string[] loadText = e.UserState.ToString().Split(','); this.labelLoad.Text = loadText[0]; this.labelLoadDesc.Text = loadText[1]; };
            worker.DoWork += (o, e) =>
            {

                ExtractPath = Path.Combine(Path.GetTempPath(), "mods");
                if (Directory.Exists(ExtractPath))
                {
                    Directory.Delete(ExtractPath, true);
                }
                Directory.CreateDirectory(ExtractPath);

                worker.ReportProgress(5, ("Even geduld...,Tijdelijke map aanmaken..."));
                
                using (ZipFile zip = ZipFile.Read(ZipPath))
                {
                    double step = (100.0 / zip.Count);
                    double percentComplete = 0;
                    foreach (ZipEntry file in zip)
                    {
                        file.Extract(ExtractPath, ExtractExistingFileAction.OverwriteSilently);
                        percentComplete += step;
                        int progress = (int)(percentComplete / 4 + 5);
                        worker.ReportProgress(progress, ("Updates uitpakken...," + file.FileName + " uitpakken..."));
                    }

                    
                    //Oude mods verwijderen
                    worker.ReportProgress(30, ("Oude mods verwijderen..., "));

                    try
                    {
                        string[] oldMods = File.ReadAllText(Path.Combine(ExtractPath, "config.txt")).Split(',');
                        File.Delete(Path.Combine(ExtractPath, "config.txt"));
                        if (oldMods.Length > 1)
                        {
                            step = (100.0 / oldMods.Length);
                            percentComplete = 0;
                            foreach (var fileName in oldMods)
                            {
                                File.Delete(Path.Combine(ModsDir, fileName));
                                percentComplete += step;
                                int progress = (int)(percentComplete / 5 + 30);
                                worker.ReportProgress(progress, ("Oude mods verwijderen...," + fileName));
                            }
                        }
                    }
                    catch (System.IO.FileNotFoundException t)
                    {
                        Debug.WriteLine(t.Message);
                        Directory.Delete(ExtractPath, true);
                        MessageBox.Show("config.txt bestand niet gevonden!", "FOUT!");
                        Application.Exit();
                    }

                    //Nieuwe mods kopiëren
                    if(Directory.Exists(Path.Combine(ExtractPath, "mods/")))
                    {
                        string[] newMods = Directory.GetFiles(Path.Combine(ExtractPath, "mods/"));
                        if (newMods.Length > 0)
                        {
                            worker.ReportProgress(50, ("Updates kopiëren..., "));
                            step = (100.0 / newMods.Length);
                            percentComplete = 0;

                            foreach (string fileName in newMods)
                            {
                                percentComplete += step;
                                int progress = (int)(percentComplete / 5 + 50);
                                worker.ReportProgress(progress, ("Updates kopiëren...," + Path.GetFileName(fileName) + " kopiëren..."));
                                File.Copy(fileName, Path.Combine(ModsDir, Path.GetFileName(fileName)), true);
                            }
                        }
                        else
                        {
                            worker.ReportProgress(70, ("Even geduld..., "));
                        }
                    }
                    else
                    {
                        worker.ReportProgress(70, ("Even geduld..., "));
                    }

                    //config bestanden kopiëren...
                    if (Directory.Exists(Path.Combine(ExtractPath, "config/")))
                    {
                        worker.ReportProgress(70, ("Config bestanden kopiëren..., "));
                        string[] newConfig = Directory.GetFiles(Path.Combine(ExtractPath, "config/"));
                        if (newConfig.Length > 0)
                        {
                            step = (100.0 / newConfig.Length);
                            percentComplete = 0;
                            foreach (string fileName in newConfig)
                            {
                                percentComplete += step;
                                int progress = (int)(percentComplete / 10 + 70);
                                worker.ReportProgress(progress, ("Config bestanden kopiëren...," + Path.GetFileName(fileName) + " kopiëren..."));
                                File.Copy(fileName, Path.Combine(ConfigDir, Path.GetFileName(fileName)), true);
                            }
                        }
                        
                        //mappen kopiëren...
                        string[] newConfigDir = Directory.GetDirectories(Path.Combine(ExtractPath, "config"));
                        if (newConfigDir.Length > 0)
                        {
                            step = (100.0 / newConfigDir.Length);
                            percentComplete = 0;
                            foreach (string dirName in newConfigDir)
                            {
                                string dir = Path.Combine(ConfigDir, Path.GetFileName(dirName));
                                if (Directory.Exists(dir))
                                {
                                    Directory.Delete(dir, true);
                                }
                                Directory.CreateDirectory(dir);
                                string[] files = Directory.GetFiles(dirName);
                                if (files.Length > 0)
                                {
                                    foreach (string fileName in files)
                                    {
                                        File.Copy(fileName, Path.Combine(dir, Path.GetFileName(fileName)), true);
                                    }
                                }

                                int progress = (int)(percentComplete / 10 + 80);
                                worker.ReportProgress(progress, ("Config bestanden kopiëren...," + Path.GetDirectoryName(dirName) + " kopiëren..."));
                                percentComplete += step;
                            }
                        }
                    }

                    worker.ReportProgress(90, ("Even geduld..., "));

                    Directory.Delete(ExtractPath, true);
                    for (int i = 0; i < 11; i++)
                    {
                        worker.ReportProgress(i + 90, ("Even geduld...,Tijdelijke bestanden opruimen..."));
                        Thread.Sleep(100);
                    }
                    MessageBox.Show("Klaar met updaten, doei!", "Doei!");
                    Application.Exit();
                }
            };
            worker.RunWorkerAsync();
           
            
        }
    }
}
