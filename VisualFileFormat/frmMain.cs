using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace VisualFileFormat
{
    public partial class frmMain : Form
    {
        CompoundType root = new CompoundType("NewFileFormat", "NewFileFormat", 1, null);
        Dictionary<string, Variable> vars = new Dictionary<string, Variable>();

        public frmMain()
        {
            InitializeComponent();
        }

        public void PrintError(string error, int lineNumber)
        {
            lstErrors.Items.Add("Line " + lineNumber + ": " + error);
            tabErrors.Text = lstErrors.Items.Count + " Errors";
        }

        public void PrintWarning(string warning, int lineNumber)
        {
            lstWarnings.Items.Add("Line " + lineNumber + ": " + warning);
            tabWarnings.Text = lstWarnings.Items.Count + " Warnings";
        }

        private void pgProperties_Click(object sender, EventArgs e)
        {
            
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            pgProperties.Text = "Properties";
            pgProperties.SelectedObject = root;

            vars.Add(root.Name, root);
            tvTree.Nodes.Add(root.Name, root.ToString());
        }

        private void tvTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            pgProperties.SelectedObject = vars[tvTree.SelectedNode.Name];
        }

        private void mnuInsertBasicDataTypeByte_Click(object sender, EventArgs e)
        {
            
        }
    }
}
