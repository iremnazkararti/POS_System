using POS_System.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace POS_System.View
{
    public partial class frmUserView : SampleView
    {
        private ListBox listBox; // ListBox'ı bir üye değişkeni olarak tanımladık

        public frmUserView()
        {
            InitializeComponent();
            InitializeListBox(); // ListBox'ı form başlatılırken başlatıyoruz
        }

        private void frmUserView_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        public override void btnAdd_Click(object sender, EventArgs e)
        {
            MainClass.BlurBackground(new frmUserAdd());
        }

        public override void txtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            // ListBox'ı temizle
            listBox.Items.Clear();

            // Verileri ListBox'a ekle
            listBox.Items.Add(dgvid);
            listBox.Items.Add(dgvname);
            listBox.Items.Add(dgvuserName);
            listBox.Items.Add(dgvpass);
            listBox.Items.Add(dgvphon);

            string qry = @"Select userId ,uName ,uUsername ,uPass ,uPhone from users
                            where  uName like '%" + txtSearch.Text + "%' order by userID desc";
            MainClass.LoadData(qry, guna2DataGridView1, listBox);
        }

        private void InitializeListBox()
        {
            listBox = new ListBox();
            listBox.Location = new Point(10, 10); // ListBox'ın konumunu ayarlayın
            listBox.Size = new Size(200, 100); // ListBox'ın boyutunu ayarlayın
            this.Controls.Add(listBox); // ListBox'ı formun kontrollerine ekleyin
        }
    }
}
