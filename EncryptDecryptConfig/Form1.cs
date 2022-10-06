using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;

namespace EncryptDecryptConfig
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        string FileName = string.Empty;

        private static void CryptConfig(string sectionKey, string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
            { 
                MessageBox.Show("File not exist");
            }
                else
                {
                    Configuration config = ConfigurationManager.OpenExeConfiguration(fileName);
                    ConfigurationSection section = config.GetSection(sectionKey);
                    if (section != null)
                    {
                        if (section.ElementInformation.IsLocked)
                        {
                            MessageBox.Show("Section:" + sectionKey + "is locked", "Error");
                        }
                        else
                        {
                            if (!section.SectionInformation.IsProtected)
                            {
                                //section.SectionInformation.ProtectSection("RsaProtectedConfigurationProvider");
                                section.SectionInformation.ProtectSection("DataProtectionConfigurationProvider");
                                section.SectionInformation.ForceSave = true;
                                MessageBox.Show("Encrypting: " + section.SectionInformation.Name + " " + section.SectionInformation.SectionName, "Encryption");
                            }
                            else
                            { 
                                // display values for current config application name value pairs
                                //foreach (KeyValueConfigurationElement x in config.AppSettings.Settings)
                                //{
                                //    MessageBox.Show("Key: " + x.Key + " " + x.Value, "display values for current config application name value pairs");
                                //}
                                //foreach (ConnectionStringSettings x in config.ConnectionStrings.ConnectionStrings)
                                //{
                                //    MessageBox.Show("Name: " + x.Name + " Provider: " + x.ProviderName + " Cs: " + x.ConnectionString, "DIsplay");
                                //}
                                section.SectionInformation.UnprotectSection();
                                section.SectionInformation.ForceSave = true;
                                MessageBox.Show("Decrypting: " + section.SectionInformation.Name + " " + section.SectionInformation.SectionName, "DIsplay");

                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Section:" + sectionKey + "is null", "Error");

                    }
                    //
                    config.Save(ConfigurationSaveMode.Full);
                    MessageBox.Show("Saving file:" + config.FilePath, "Success");
                    Process.Start("notepad.exe", config.FilePath);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void button2_Click(object sender, EventArgs e)
        {
            CryptConfig("appSettings", FileName);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFile();
        }
        private void OpenFile()
        {
            openFileDialog1.Filter = ".Net Executables|*.exe";
            openFileDialog1.FileName = "";
            openFileDialog1.ShowDialog();
            FileName = openFileDialog1.FileName;
            txtEncryption.Text = FileName;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CryptConfig("connectionStrings", FileName);
        }
    }
}