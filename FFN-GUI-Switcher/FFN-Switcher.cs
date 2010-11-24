using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

using FFN_Switcher.Logging;
using FFN_Switcher.Settings;

namespace FFN_Switcher
{
    /// <summary>
    /// Das ist die Klasse die die Main Methode enthält und dementsprechend als Einstieg in den Switcher dient.
    /// </summary>
    static class FFNSwitcher
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            if ((!FFN_Switcher.Properties.Settings.Default.SetupWizardDone) || (!FFN_Switcher.Properties.Settings.Default.LicenseAccepted))
            {
                #region Setup Wizard
                SetupWizard_Step1 Step1 = new SetupWizard_Step1();

                Step1.ShowDialog();

                if (FFN_Switcher.Properties.Settings.Default.LicenseAccepted)
                {
                    FFN_Switcher.Properties.Settings.Default.Save();

                    SetupWizard_Step2 Step2 = new SetupWizard_Step2();

                    Step2.ShowDialog();

                    if (FFN_Switcher.Properties.Settings.Default.Wizard_Step2_Done)
                    {
                        SetupWizard_Step3 Step3 = new SetupWizard_Step3();

                        Step3.ShowDialog();

                        if (FFN_Switcher.Properties.Settings.Default.Wizard_Step3_Done)
                        {
                            SetupWizard_Step4 Step4 = new SetupWizard_Step4();
                            Step4.ShowDialog();

                            if (FFN_Switcher.Properties.Settings.Default.Wizard_Step4_Done)
                            {
                                SetupWizard_Step5 Step5 = new SetupWizard_Step5();
                                Step5.ShowDialog();

                                if (FFN_Switcher.Properties.Settings.Default.Wizard_Step5_Done)
                                {
                                    SetupWizard_Step6 Step6 = new SetupWizard_Step6();
                                    Step6.ShowDialog();

                                    if (FFN_Switcher.Properties.Settings.Default.Wizard_Step6_Done)
                                    {
                                        SetupWizard_Step7 Step7 = new SetupWizard_Step7();
                                        Step7.ShowDialog();

                                        if (FFN_Switcher.Properties.Settings.Default.Wizard_Step7_Done)
                                        {
                                            SetupWizard_Step8 Step8 = new SetupWizard_Step8();
                                            Step8.ShowDialog();

                                            if (FFN_Switcher.Properties.Settings.Default.Wizard_Step8_Done)
                                            {
                                                SetupWizard_Step9 Step9 = new SetupWizard_Step9();
                                                Step9.ShowDialog();

                                                if (FFN_Switcher.Properties.Settings.Default.Wizard_Step9_Done)
                                                {
                                                    SetupWizard_Step10 Step10 = new SetupWizard_Step10();
                                                    Step10.ShowDialog();

                                                    if (FFN_Switcher.Properties.Settings.Default.Wizard_Step10_Done)
                                                    {
                                                        FFN_Switcher.Properties.Settings.Default.SetupWizardDone = true;
                                                        FFN_Switcher.Properties.Settings.Default.Save();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion
            }

            if (FFN_Switcher.Properties.Settings.Default.LicenseAccepted)
            {
                MainWindow mainWindow = new MainWindow();

                #region Data
                Switcher Switcher = new Switcher();
                mainWindow.internalSwitcher = Switcher;
                #endregion

                #region ConsoleOutputLogger
                ConsoleOutputLogger._Window = mainWindow;
                ConsoleOutputLogger.verbose = true;
                ConsoleOutputLogger.writeLogfile = false;
                #endregion

                #region Logo
                ConsoleOutputLogger.WriteLine("[SWITCHER] Freies Funknetz Switcher Tool");
                ConsoleOutputLogger.WriteLine("[SWITCHER] Version "+Version.VersionString);
                ConsoleOutputLogger.WriteLine("[SWITCHER] "+Version.LicenseString);
                ConsoleOutputLogger.WriteLine("[SWITCHER] "+Version.CopyrightString);
                #endregion

                #region load settings
                ConsoleOutputLogger.verbose = FFN_Switcher.Properties.Settings.Default.LogToScreenEnabled;
                if (!FFN_Switcher.Properties.Settings.Default.LogToFileEnabled)
                {
                    ConsoleOutputLogger.writeLogfile = false;
                }
                else
                {
                    ConsoleOutputLogger.Logfilename = FFN_Switcher.Properties.Settings.Default.Logfile;
                    ConsoleOutputLogger.writeLogfile = true;
                }
                //}
                #endregion

                #region start Switcher...
                Thread SwitcherThread = new Thread(new ThreadStart(Switcher.Run));
                SwitcherThread.Start();
                #endregion

                Application.Run(mainWindow);

                #region Shutdown
                Switcher.Shutdown();
                Switcher.done = true;
                ConsoleOutputLogger.WriteLine("[SWITCHER] Herunterfahren...");
                ConsoleOutputLogger.ShutdownLog();
                #endregion
            }
            else
            {
                MessageBox.Show("Akzeptieren Sie die Lizenz dieser Software um sie zu verwenden.");
            }
        }
    }
}
