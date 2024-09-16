using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections;
using System.IO;


namespace POS_System.Model
{
    public partial class frmUserAdd : SampleAdd
    {
        public frmUserAdd()
        {
            InitializeComponent();
        }
        public int id = 0;

        public override void btnSave_Click(object sender, EventArgs e)
        {

            if (MainClass.Validation(this)==false)
            {
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Show("Boşlukları Doldurunuz!");
                return;

            }
            else
            {
                string qry = "";
                if (id == 0) //Insert
                {

                    qry = @"Insert into users values(@name,@username,@pass,@phone,@image)";
                   
                }
                else //update
                {
                    qry = @"UPDATE users set uName =@name
                                uUsername =@username,
                                uPass =@pass,
                                uPhone =@phone,
                                uImage =@image,
                                where userID =@id";

                }

                Image temp = new Bitmap(txtPic.Image);
                MemoryStream ms = new MemoryStream();
                temp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                imageByteArray = ms.ToArray();

                Hashtable ht = new Hashtable();
                ht.Add("@id", id);
                ht.Add("@name", txtName.Text);
                ht.Add("@username", txtUser.Text);
                ht.Add("@pass", txtPass.Text);
                ht.Add("@phone", txtPhone.Text);
                ht.Add("@image", imageByteArray);

                if (MainClass.SQL(qry,ht)>0)
                {
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    guna2MessageDialog1.Show("Data kaydedildi!");
                    id = 0;
                    txtName.Text = "";
                    txtUser.Text = "";
                    txtPass.Text = "";
                    txtPhone.Text = "";
                    txtPic.Image = POS_System.Properties.Resources.userPic;
                    txtName.Focus();

                }
            }

        }

        public string filePath = "";
        Byte[] imageByteArray;
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "images(.jpg, .png)|?.png, ?jpg";
            if (ofd.ShowDialog() == DialogResult.OK )
            {
                filePath = ofd.FileName;
                txtPic.Image = new Bitmap(filePath);
             
            }

        }
    }
}
