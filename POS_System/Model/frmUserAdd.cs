using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace POS_System.Model
{
    public partial class frmUserAdd : SampleAdd
    {
        public frmUserAdd()
        {
            InitializeComponent();
        }

        public int id = 0;  // Kullanıcı ID'si

        public override void btnSave_Click(object sender, EventArgs e)
        {
            if (MainClass.Validation(this) == false)
            {
                guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Error;
                guna2MessageDialog1.Show("Boşlukları doldurunuz!");
                return;
            }
            else
            {
                string qry = "";
                if (id == 0) // Yeni kullanıcı ekleme (Insert)
                {
                    qry = @"INSERT INTO users (uName, uUsername, uPass, uPhone, uImage) 
                            VALUES (@name, @username, @pass, @phone, @image)";
                }
                else // Var olan kullanıcıyı güncelleme (Update)
                {
                    qry = @"UPDATE users SET uName = @name, 
                                             uUsername = @username, 
                                             uPass = @pass, 
                                             uPhone = @phone, 
                                             uImage = @image 
                            WHERE userID = @id";
                }

                // Resmi byte array'e çevirme
                Image temp = new Bitmap(txtPic.Image);
                MemoryStream ms = new MemoryStream();
                temp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                byte[] imageByteArray = ms.ToArray();

                // Sorgu için parametreleri hashtable içine ekleme
                Hashtable ht = new Hashtable();
                ht.Add("@id", id);
                ht.Add("@name", txtName.Text);
                ht.Add("@username", txtUser.Text);
                ht.Add("@pass", txtPass.Text);
                ht.Add("@phone", txtPhone.Text);
                ht.Add("@image", imageByteArray);

                // Veritabanı işlemi
                if (MainClass.SQL(qry, ht) > 0)
                {
                    guna2MessageDialog1.Buttons = Guna.UI2.WinForms.MessageDialogButtons.OK;
                    guna2MessageDialog1.Icon = Guna.UI2.WinForms.MessageDialogIcon.Information;
                    guna2MessageDialog1.Show("Veri kaydedildi!");

                    // Formu temizleme
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

        public string filePath = ""; // Seçilen resmin dosya yolu
        private void btnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Images (.jpg, .png)|*.png;*jpg";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
                txtPic.Image = new Bitmap(filePath);
            }
        }

        private void LoadImage()
        {
            string qry = @"Select uImage from users where userId = " + id + "";
            SqlCommand cmd = new SqlCommand(qry,MainClass.con);
            DataTable dt = new DataTable();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(dt);
            if(dt.Rows.Count>0)
            {
                Byte[] imageArray = (byte[])dt.Rows[0]["uImage"];
                Byte[] imageByteArray = imageArray;
                txtPic.Image = Image.FromStream(new MemoryStream(imageByteArray));
            }
        }

        private void frmUserAdd_Load(object sender, EventArgs e)
        {

            if(id>0)
            {
                LoadImage();
            }

        }
    }
}