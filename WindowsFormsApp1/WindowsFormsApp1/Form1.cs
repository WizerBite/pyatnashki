using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using System.IO;


namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            main_menu_panel.Visible = true;
            settings_game_panel.Visible = false;
            Score_panel.Visible = false;
            panel_score_add.Visible = false;
            main_menu_panel.Location = new Point(12, 12);
            settings_game_panel.Location = new Point(12, 12);
            Score_panel.Location = new Point(12, 12);
            panel_score_add.Location = new Point(0, 0);
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
            pictureBox1.Location = new Point(0, 0);
            ClientSize = new Size(24 + main_menu_panel.Width, 24 + main_menu_panel.Height);
            swap_size();

            string workingDirectory = Environment.CurrentDirectory;
            string connection = "Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename = " + Directory.GetParent(workingDirectory).Parent.Parent.FullName + "\\WindowsFormsApp1\\Database1.mdf; Integrated Security = True";
            sqlConnection = new SqlConnection(@connection);

            

            sqlConnection.Open();
        }

        public int size_field_1 = 0;
        public int size_field_2 = 0;
        public int zero_title_x = 0;
        public int zero_title_y = 0;
        Button[,] button;
        int[,] arr;
        bool start = false;
        int turns = 0;
        private SqlConnection sqlConnection = null;
        private SqlCommandBuilder sqlBuilder = null;
        private SqlDataAdapter sqlDataAdapter = null;
        private DataSet dataSet = null;

        private void Play_button_Click(object sender, EventArgs e)
        {
            ClientSize = new Size(24 + settings_game_panel.Width, 24 + settings_game_panel.Height);
            settings_game_panel.Visible = true;
            main_menu_panel.Visible = false;
            size_custom_1_textBox.Text = "";
            size_custom_2_textBox.Text = "";
        }

        private void swap_size()
        {
            size_3_button.BackColor = SystemColors.GradientInactiveCaption;
            size_4_button.BackColor = SystemColors.GradientInactiveCaption;
            size_6_button.BackColor = SystemColors.GradientInactiveCaption;
            size_8_button.BackColor = SystemColors.GradientInactiveCaption;
            size_custom_button.BackColor = SystemColors.GradientInactiveCaption;
        }

        private void size_3_button_Click(object sender, EventArgs e)
        {
            swap_size();
            size_3_button.BackColor = SystemColors.ActiveCaption;
            size_field_1 = 3;
            size_field_2 = 3;
        }

        private void size_4_button_Click(object sender, EventArgs e)
        {
            swap_size();
            size_4_button.BackColor = SystemColors.ActiveCaption;
            size_field_1 = 4;
            size_field_2 = 4;
        }

        private void size_6_button_Click(object sender, EventArgs e)
        {
            swap_size();
            size_6_button.BackColor = SystemColors.ActiveCaption;
            size_field_1 = 6;
            size_field_2 = 6;
        }

        private void size_8_button_Click(object sender, EventArgs e)
        {
            swap_size();
            size_8_button.BackColor = SystemColors.ActiveCaption;
            size_field_1 = 8;
            size_field_2 = 8;
        }

        private void size_custom_button_Click(object sender, EventArgs e)
        {
            if (size_custom_1_textBox.Text.Length > 0 && size_custom_2_textBox.Text.Length > 0
                && int.Parse(size_custom_1_textBox.Text) > 1 && int.Parse(size_custom_2_textBox.Text) > 1)
            {
                swap_size();
                size_custom_button.BackColor = SystemColors.ActiveCaption;
                size_field_1 = int.Parse(size_custom_1_textBox.Text);
                size_field_2 = int.Parse(size_custom_2_textBox.Text);
            }
            else
                MessageBox.Show("Введите размерность начиная с 2x2");
        }

        private void start_button_Click(object sender, EventArgs e)
        {
            if (size_field_1 > 0 && size_field_2 > 0)
            {
                pictureBox2.Location = new Point(size_field_2 * 55 + 36 - 60, 0);
                pictureBox3.Location = new Point(0, size_field_1 * 55 + 36 - 60);
                pictureBox4.Location = new Point(size_field_2 * 55 + 36 - 60, size_field_1 * 55 + 36 - 60);
                pictureBox1.Visible = true;
                pictureBox2.Visible = true;
                pictureBox3.Visible = true;
                pictureBox4.Visible = true;
                arr = new int[size_field_1, size_field_2];
                button = new Button[size_field_1, size_field_2];
                for (int i = 0; i < size_field_1; i++)
                {
                    for (int j = 0; j < size_field_2; j++)
                    {
                        if ((j + 1) * (i + 1) == size_field_1 * size_field_2)
                        {
                            arr[i, j] = 0;
                            button[i, j] = new Button();
                            button[i, j].Name = "title" + (i + 1).ToString() + "_" + (j + 1).ToString();
                            button[i, j].Click += swap;
                            int size_button = 45;
                            button[i, j].Size = new Size(size_button, size_button);
                            button[i, j].Location = new Point(23 + (size_button + 10) * j, 23 + (size_button + 10) * i);
                            this.Controls.Add(button[i, j]);
                            zero_title_x = i;
                            zero_title_y = j;
                            //button[i, j].Font = new Font("Segoe Print", 14, FontStyle.Regular);
                            button[i, j].BringToFront();
                        }
                        else
                        {
                            arr[i, j] = (i * size_field_2) + j + 1;
                            button[i, j] = new Button();
                            button[i, j].Name = "title" + (i + 1).ToString() + "_" + (j + 1).ToString();
                            button[i, j].Text = arr[i, j].ToString();
                            button[i, j].Click += swap;
                            int size_button = 45;
                            button[i, j].Size = new Size(size_button, size_button);
                            button[i, j].Location = new Point(23 + (size_button + 10) * j, 23 + (size_button + 10) * i);
                            this.Controls.Add(button[i, j]);
                            //button[i, j].Font = new Font("Segoe Print", 14, FontStyle.Regular);
                            button[i, j].BringToFront();
                        }
                    }
                }
                turns = 0;
                swap_size();
                randomize_field();
                this.ClientSize = new System.Drawing.Size(size_field_2 * 55 + 36, size_field_1 * 55 + 36);
                
                settings_game_panel.Visible = false;
            }
        }

        private void randomize_field()
        {
            // 1 - лево
            // 2 - право
            // 3 - верх
            // 4 - низ
            Random rnd = new Random();
            int move;
            start = true;
            for (int i = 0; i < size_field_1 * size_field_2 * 1000; i++)
            {
                if (zero_title_x == 0)
                {
                    if (zero_title_y == 0)
                    {
                        move = getRandom(rnd, 2, 5, 3);
                    }
                    else if (zero_title_y == size_field_2 - 1)
                    {
                        move = rnd.Next(2, 4);
                    }
                    else
                    {
                        move = rnd.Next(2, 5);
                    }
                }
                else if (zero_title_x == size_field_1 - 1)
                {
                    if (zero_title_y == 0)
                    {
                        move = getRandom(rnd, 1, 5, 2, 3);
                    }
                    else if (zero_title_y == size_field_2 - 1)
                    {
                        move = getRandom(rnd, 1, 4, 2);
                    }
                    else
                    {
                        move = getRandom(rnd, 1, 5, 2);
                    }
                }
                else
                {
                    if (zero_title_y == 0)
                    {
                        move = getRandom(rnd, 1, 5, 3);
                    }
                    else if (zero_title_y == size_field_2 - 1)
                    {
                        move = rnd.Next(1, 4);
                    }
                    else
                    {
                        move = rnd.Next(1, 5);
                    }
                }
                random_swaping(move);
            }
            start = false;
        }

        public void random_swaping(int move)
        {
            switch (move)
            {
                case 1:
                    {
                        swap_title(zero_title_x - 1, zero_title_y);
                        break;
                    }
                case 2:
                    {
                        swap_title(zero_title_x + 1, zero_title_y);
                        break;
                    }
                case 3:
                    {
                        swap_title(zero_title_x, zero_title_y - 1);
                        break;
                    }
                case 4:
                    {
                        swap_title(zero_title_x, zero_title_y + 1);
                        break;
                    }
            }
        }

        public static int getRandom(Random rand, int min, int max, params int[] exclude)
        {
            int result = 0;
            do
            {
                result = rand.Next(min, max);
            }
            while (exclude.Contains(result));

            return result;
        }

        private void swap(object sender, EventArgs eventArgs)
        {
            string mes = (sender as Button).Name;
            bool check = false;
            string x = "", y = "";
            for (int i = 5; i < mes.Length; i++)
            {
                if (!check)
                    if (mes[i].ToString() == "_")
                        check = true;
                    else
                        x += mes[i];
                else
                    y += mes[i];
            }
            
            if (int.Parse(x) - 1 == zero_title_x && int.Parse(y) == zero_title_y)
            {
                swap_title(int.Parse(x) - 1, int.Parse(y) - 1);
            }
            else if (int.Parse(x) - 1 == zero_title_x && int.Parse(y) - 2 == zero_title_y)
            {
                swap_title(int.Parse(x) - 1, int.Parse(y) - 1);
            }
            else if ((int.Parse(x) - 2) == zero_title_x && int.Parse(y) - 1 == zero_title_y)
            {
                swap_title(int.Parse(x) - 1, int.Parse(y) - 1);
            }
            else if ((int.Parse(x)) == zero_title_x && int.Parse(y) - 1 == zero_title_y)
            {
                swap_title(int.Parse(x) - 1, int.Parse(y) - 1);
            }
        }

        private void swap_title(int x, int y)
        {
            button[zero_title_x, zero_title_y].BackColor = Color.Tan;
            button[zero_title_x, zero_title_y].Text = button[x, y].Text;
            arr[zero_title_x, zero_title_y] = int.Parse(button[x, y].Text);
            arr[x, y] = 0;
            button[x, y].Text = "";
            button[x, y].BackColor = Color.SandyBrown;
            zero_title_x = x;
            zero_title_y = y;
            if (!start)
            {
                turns++;
                check_true(); 
            }
        }

        private void check_true()
        {
            bool fail = false;
            for (int i = 0; i < size_field_1; i++)
            {
                for (int j = 0; j < size_field_2; j++)
                {
                    if (arr[i, j] != (i * size_field_2) + j + 1)
                    {
                        if (!(i == size_field_1 - 1 && j == size_field_2 - 1))
                            fail = true;
                        break;
                    }
                }
                if (fail)
                    break;
            }
            if (!fail)
                victory();
        }
        
        private void victory()
        {
            if (MessageBox.Show("Вы собрали пятнашки за " + turns + " ходов!" + '\n' + "Записать результат?",
                "Question", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                turns_label_score_add.Text = turns.ToString();
                field_label_score_add.Text = size_field_1.ToString() + "x" + size_field_2.ToString();
                for (int i = 0; i < size_field_1; i++)
                {
                    for (int j = 0; j < size_field_2; j++)
                    {
                        this.Controls.Remove(button[i, j]);
                        button[i, j].Dispose();
                    }
                }
                panel_score_add.Visible = true;
                this.ClientSize = new System.Drawing.Size(283, 219);
            }
            else
            {
                for (int i = 0; i < size_field_1; i++)
                {
                    for (int j = 0; j < size_field_2; j++)
                    {
                        this.Controls.Remove(button[i, j]);
                        button[i, j].Dispose();
                    }
                }
                ClientSize = new Size(24 + main_menu_panel.Width, 24 + main_menu_panel.Height);
                size_field_1 = 0;
                size_field_2 = 0;
                main_menu_panel.Visible = true;
            }
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
            pictureBox3.Visible = false;
            pictureBox4.Visible = false;
        }

        private void size_custom_1_textBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.')) { e.Handled = true; }
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1)) { e.Handled = true; }
        }

        private void Highscore_button_Click(object sender, EventArgs e)
        {
            BackColor = Color.Gold;
            ClientSize = new Size(24 + Score_panel.Width, 24 + Score_panel.Height);
            main_menu_panel.Visible = false;
            Score_panel.Visible = true;
            Score_Size_1_textBox.Text = "";
            Score_Size_2_textBox.Text = "";
            sqlDataAdapter = new SqlDataAdapter("SELECT * FROM Score ORDER BY Size, Turns, User", sqlConnection);
            sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
            dataSet = new DataSet();
            sqlDataAdapter.Fill(dataSet, "Score");
            dataGridView_Score.DataSource = dataSet.Tables["Score"];
            Score_Size_Label.Text = "Все рекорды";
        }

        private void Update_Button_Click(object sender, EventArgs e)
        {
            int score_size_1 = 0;
            int score_size_2 = 0;
            try
            {
                score_size_1 = int.Parse(Score_Size_1_textBox.Text);
                score_size_2 = int.Parse(Score_Size_2_textBox.Text);
            }
            catch
            {

            }
            
            if (score_size_1 > 1 && score_size_2 > 1)
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT * FROM Score WHERE Size=\'" + Score_Size_1_textBox.Text + "x" + Score_Size_2_textBox.Text + "\' ORDER BY Turns", sqlConnection);
                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
                dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet, "Score");
                dataGridView_Score.DataSource = dataSet.Tables["Score"];
                Score_Size_Label.Text = $"{Score_Size_1_textBox.Text}x{Score_Size_2_textBox.Text}";
            }
            else
            {
                sqlDataAdapter = new SqlDataAdapter("SELECT * FROM Score ORDER BY Size, Turns", sqlConnection);
                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);
                dataSet = new DataSet();
                sqlDataAdapter.Fill(dataSet, "Score");
                dataGridView_Score.DataSource = dataSet.Tables["Score"];
                Score_Size_Label.Text = "Все рекорды";
            }
        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void accept_button_Click(object sender, EventArgs e)
        {
            if (name_textbox_score_add.Text.Length >= 3)
            {
                sqlDataAdapter = new SqlDataAdapter($"SELECT * FROM Score", sqlConnection);

                sqlBuilder = new SqlCommandBuilder(sqlDataAdapter);

                dataSet = new DataSet();

                sqlDataAdapter.Fill(dataSet, "Score");

                DataRow NewScore = dataSet.Tables["Score"].NewRow();

                NewScore["Size"] = size_field_1.ToString() + "x" + size_field_2.ToString();
                NewScore["Turns"] = turns;
                NewScore["User"] = name_textbox_score_add.Text;

                dataSet.Tables["Score"].Rows.Add(NewScore);

                sqlDataAdapter.Update(dataSet, "Score");

                size_field_1 = 0;
                size_field_2 = 0;
                name_textbox_score_add.Text = "";
                panel_score_add.Visible = false;
                main_menu_panel.Visible = true;
                ClientSize = new Size(24 + main_menu_panel.Width, 24 + main_menu_panel.Height);
            }
            else
            {
                MessageBox.Show("Введите имя от 3 до 6 символов.");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ClientSize = new Size(24 + main_menu_panel.Width, 24 + main_menu_panel.Height);
            BackColor = Color.SandyBrown;
            settings_game_panel.Visible = false;
            Score_panel.Visible = false;
            main_menu_panel.Visible = true;
        }
    }
}

