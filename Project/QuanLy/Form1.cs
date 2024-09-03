using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Data.SqlClient;

namespace QuanLy
{
    public partial class Form1 : Form
    {
        private string connectionString = "Data Source=927;Initial Catalog=master;Integrated Security=True";
        public Form1()
        {
            InitializeComponent();
        }

        private void LoadData()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "SELECT * FROM SinhVien";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            DataTable dataTable = new DataTable();
                            dataTable.Load(reader);
                            dgvSinhVien.DataSource = dataTable;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
        }
        private void btnThem_Click(object sender, EventArgs e)
        {
            string name = txtName.Text;
            string email = txtEmail.Text;
            int sdt = Convert.ToInt32(txtSDT.Text);

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();

                string sql = "INSERT INTO SinhVien (SV_Name, SV_Phone, SV_Email) VALUES (@Ten, @SDT, @Email)";

                using (SqlCommand command = new SqlCommand(sql, con))
                {
                    command.Parameters.AddWithValue("@Ten", name);
                    command.Parameters.AddWithValue("@SDT", sdt);
                    command.Parameters.AddWithValue("@Email", email);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Sinh viên đã được thêm vào cơ sở dữ liệu thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Đã xảy ra lỗi khi thêm sinh viên vào cơ sở dữ liệu.");
                    }
                }
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvSinhVien.SelectedRows.Count == 1)
            {
                int studentId = Convert.ToInt32(dgvSinhVien.SelectedRows[0].Cells[0].Value);
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa sinh viên này?", "Xác nhận", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        using (SqlConnection connection = new SqlConnection(connectionString))
                        {
                            connection.Open();
                            string sql = "DELETE FROM SinhVien WHERE SV_ID = @SV_ID";
                            using (SqlCommand command = new SqlCommand(sql, connection))
                            {
                                command.Parameters.AddWithValue("@SV_ID", studentId);
                                int rowsAffected = command.ExecuteNonQuery();
                                if (rowsAffected > 0)
                                {
                                    MessageBox.Show("Sinh viên đã được xóa thành công!");
                                    LoadData();
                                }
                                else
                                {
                                    MessageBox.Show("Không tìm thấy sinh viên có ID này trong cơ sở dữ liệu.");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đã xảy ra lỗi khi xóa sinh viên: " + ex.Message);
                    }
                }
            }
        }
        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát không?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string sql = "SELECT * FROM SinhVien";
            adapter.SelectCommand = new SqlCommand(sql, new SqlConnection(connectionString));
            adapter.Fill(table);
            dgvSinhVien.DataSource = table;
        }

        private void dgvSinhVien_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvSinhVien.Rows[e.RowIndex];
                txtName.Text = row.Cells["Ten"].Value.ToString();
                txtEmail.Text = row.Cells["Email"].Value.ToString();
                txtSDT.Text = row.Cells["SDT"].Value.ToString();
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            if(dgvSinhVien.SelectedRows.Count == 1 )
            {
                string name = txtName.Text;
                string email = txtEmail.Text;
                int sdt = Convert.ToInt32(txtSDT.Text);
                int studentId = Convert.ToInt32(dgvSinhVien.SelectedRows[0].Cells[0].Value);
                try
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        string sql = "UPDATE SinhVien SET SV_Name = @Ten, SV_Phone = @SDT, SV_Email = @Email WHERE SV_ID = @StudentId";
                        using (SqlCommand command = new SqlCommand(sql, con))
                        {
                            command.Parameters.AddWithValue("@Ten", name);
                            command.Parameters.AddWithValue("@Email", email);
                            command.Parameters.AddWithValue("@SDT", sdt);
                            command.Parameters.AddWithValue("@StudentId", studentId);

                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Thông tin sinh viên đã được cập nhật thành công!");
                                LoadData();
                            }
                            else
                            {
                                MessageBox.Show("Không tìm thấy sinh viên có ID này trong cơ sở dữ liệu.");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi khi cập nhật thông tin sinh viên: " + ex.Message);
                }
            }    
        }
    }
}

