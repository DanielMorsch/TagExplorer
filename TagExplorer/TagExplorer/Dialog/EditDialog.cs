using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TagExplorer
{
    public partial class EditDialog : Form
    {
        public bool Selected
        {
            get { return DialogResult == DialogResult.OK; }
        }

        public EditDialog()
        {
            InitializeComponent();
        }


        public string ShowEditDialog( string sTags, string sFileName )
        {
            string sReturn = sTags;
            labelFileName.Text = sFileName;
            txtTags.Text = sTags;
            txtTags.Focus();

            if ( ShowDialog() == DialogResult.OK )
            {
                sReturn = txtTags.Text;
            }

            return sReturn;
        }


        private void OkButtonClick( object sender, EventArgs e )
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void CancelButtonClick( object sender, EventArgs e )
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }


    }
}