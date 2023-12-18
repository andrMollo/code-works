using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CodeWorksUI
{
    public partial class ExportAssemblyForm : Form
    {
        #region Public properties
        /// <summary>
        /// Check to export components
        /// </summary>
        public bool ExportCheck
        {
            get { return exportCheckBox.Checked; }
        }

        /// <summary>
        /// Check to print the components
        /// </summary>
        public bool PrintCheck
        {
            get { return printCheckBox.Checked; }
        }

        /// <summary>
        /// Check to update the components quantities
        /// </summary>
        public bool QuantityCheck
        {
            get { return compQtyCheckBox.Checked; }
        }
        
        /// <summary>
        /// The quantity property of the open assembly
        /// </summary>
        public string AssemblyQty
        {
            get { return assemblyQtyTextBox.Text; }
            set { assemblyQtyTextBox.Text = value; }
        }

        /// <summary>
        /// The job number \ sub-folder for the export
        /// </summary>
        public string JobNumber
        {
            get { return jobNbrTextBox.Text; }
            set { jobNbrTextBox.Text = value; }
        }
        #endregion
        public ExportAssemblyForm()
        {
            InitializeComponent();
        }

        private void okButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this .DialogResult = DialogResult.Cancel;
        }
    }
}
