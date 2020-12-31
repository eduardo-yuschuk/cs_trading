using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UserInterface.WinForms
{
    public partial class DataInspector : Form
    {
        public DataInspector()
        {
            InitializeComponent();
        }

        private void DataInspector_Load(object sender, EventArgs e)
        {
            LoadAssets();
        }

        private void LoadAssets()
        {
            comboBoxAsset.Items.Clear();
            if(!Directory.Exists(textBoxRootPath.Text))
            {
                Directory.CreateDirectory(textBoxRootPath.Text);
            }
            foreach (var directory in Directory.EnumerateDirectories(textBoxRootPath.Text))
            {
                comboBoxAsset.Items.Add(FileSystemItem.Create(directory));
            }
        }

        class FileSystemItem
        {
            public string Path { get; set; }

            public static FileSystemItem Create(string path)
            {
                return new FileSystemItem {Path = path};
            }

            public override string ToString()
            {
                DirectoryInfo info = new DirectoryInfo(Path);
                return info.Name;
            }
        }

        private void ComboBoxAsset_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDependant(comboBoxAsset, comboBoxProvider, Directory.EnumerateDirectories);
        }

        private void UpdateDependant(ComboBox changed, ComboBox dependant,
            Func<string, IEnumerable<string>> enumeratorMethod)
        {
            dependant.Items.Clear();
            FileSystemItem item = (FileSystemItem) changed.SelectedItem;
            if (item != null)
            {
                foreach (var directory in enumeratorMethod(item.Path))
                {
                    dependant.Items.Add(FileSystemItem.Create(directory));
                }
            }
        }

        private void ComboBoxProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDependant(comboBoxProvider, comboBoxYear, Directory.EnumerateDirectories);
        }

        private void ComboBoxYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDependant(comboBoxYear, comboBoxMonth, Directory.EnumerateDirectories);
        }

        private void ComboBoxMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateDependant(comboBoxMonth, listBoxDays, Directory.EnumerateFiles);
        }

        private void UpdateDependant(ComboBox changed, ListBox dependant,
            Func<string, IEnumerable<string>> enumeratorMethod)
        {
            dependant.Items.Clear();
            FileSystemItem item = (FileSystemItem) changed.SelectedItem;
            if (item != null)
            {
                foreach (var directory in enumeratorMethod(item.Path))
                {
                    dependant.Items.Add(FileSystemItem.Create(directory));
                }
            }
        }

        private void ButtonDraw_Click(object sender, EventArgs e)
        {
        }
    }
}