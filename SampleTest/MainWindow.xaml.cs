using System;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace SampleTest
{
    // MainWindow.xaml에 대한 상호 작용 논리
    public partial class MainWindow : Window
    {
        // 임시 DataTable
        private DataTable Dt;
        
        // 선택된 Row
        private DataRowView SelectedRow;

        //이전에 선택된 행의 값
        string Past_Grade;
        string Past_No;
        string Past_Score;
        string Past_Cclass;
        string Past_Name;

        SqlConnection conDB = new SqlConnection();

        public MainWindow()
        {
            InitializeComponent();

            Create_DtColumn(); // DataTable Column 생성.

            Connect_DB(); // DB Connect
        }

        // 컬럼추가
        private void Create_DtColumn()
        {
            DataTable D_table = new DataTable();
            D_table.Columns.Add("Grade");
            D_table.Columns.Add("Cclass");
            D_table.Columns.Add("No");
            D_table.Columns.Add("Name");
            D_table.Columns.Add("Score");

            Dt = D_table;
        }

        // DB 연결
        private void Connect_DB()
        {
            try
            {
                // DB 연결
                conDB.ConnectionString = @"Server=DESKTOP-CRNFB09;database=SampleTest;uid=geun;pwd=1;";
                conDB.Open();

                // DB Select 문 실행.
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conDB;
                cmd.CommandText = "SELECT * FROM SampleTest";

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                DataSet Dset = new DataSet();

                adapter.Fill(Dset, "SAMPLETEST");

                Dt = Dset.Tables[0].Copy();

                if (Dt.Rows.Count > 0)
                    SampleTest.ItemsSource = Dt.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fail");
                throw;
            }
            finally
            {
                //{
                //    if(conDB != null)
                //    {
                //        conDB.Close();
                //        MessageBox.Show("연결 해제");
                //    }
            }
        }

        // Read 버튼 Event
        // <param name="sender"></param>
        // <param name="e"></param>
        private void BtnRead_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();

                // Grid 초기화
                Dt = null;
                Create_DtColumn();
                SampleTest.ItemsSource = Dt.DefaultView;

                cmd.Connection = conDB;
                cmd.CommandText = "SELECT * FROM SAMPLETEST";

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                DataSet Dset = new DataSet();

                adapter.Fill(Dset, "SAMPLETEST");

                Dt = Dset.Tables[0].Copy();

                if (Dt.Rows.Count > 0)
                    SampleTest.ItemsSource = Dt.DefaultView;

            }
            catch (Exception)
            {

                throw;
            }
        }

        // Create 버튼 Event
        // <param name="sender"></param>
        // <param name="e"></param>
        private void BtnCreate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedRow == null)
                {
                    MessageBox.Show("선택된 행이 없습니다.");
                    return;
                }

                string Grade = SelectedRow["Grade"].ToString();
                string Cclass = SelectedRow["Cclass"].ToString();
                string No = SelectedRow["No"].ToString();
                string Name = SelectedRow["Name"].ToString();
                string Score = SelectedRow["Score"].ToString();


                if (SelectedRow["Grade"].ToString() == string.Empty ||
                    SelectedRow["Cclass"].ToString() == string.Empty ||
                    SelectedRow["No"].ToString() == string.Empty ||
                    SelectedRow["Name"].ToString() == string.Empty ||
                    SelectedRow["Score"].ToString() == string.Empty)
                {
                    MessageBox.Show("행의 빈 값이 존재합니다. 값을 기입해주세요.");
                    return;
                }

                // DB Insert 문 실행.
                string Insert_Query = "INSERT INTO SAMPLETEST (Grade, Cclass, No, Name, Score) values (@Value1, @Value2, @Value3, @Value4, @Value5)";
                
                SqlCommand cmd = new SqlCommand(Insert_Query, conDB);

                cmd.Parameters.AddWithValue("@Value1", Grade);
                cmd.Parameters.AddWithValue("@Value2", Cclass);
                cmd.Parameters.AddWithValue("@Value3", No);
                cmd.Parameters.AddWithValue("@Value4", Name);
                cmd.Parameters.AddWithValue("@Value5", Score);

                cmd.ExecuteNonQuery();

                // Grid 초기화
                Dt = null;
                Create_DtColumn();
                SampleTest.ItemsSource = Dt.DefaultView;

                cmd.Connection = conDB;
                cmd.CommandText = "SELECT * FROM SAMPLETEST";

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                DataSet Dset = new DataSet();

                adapter.Fill(Dset, "SAMPLETEST");

                Dt = Dset.Tables[0].Copy();

                if (Dt.Rows.Count > 0)
                    SampleTest.ItemsSource = Dt.DefaultView;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Update 버튼 Event
        // <param name="sender"></param>
        // <param name="e"></param>
        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedRow == null)
                {
                    MessageBox.Show("선택된 행이 없습니다.");
                    return;
                }

                string Grade = SelectedRow["Grade"].ToString();
                string Cclass = SelectedRow["Cclass"].ToString();
                string No = SelectedRow["No"].ToString();
                string Name = SelectedRow["Name"].ToString();
                string Score = SelectedRow["Score"].ToString();


                if (SelectedRow["Grade"].ToString() == string.Empty ||
                    SelectedRow["Cclass"].ToString() == string.Empty ||
                    SelectedRow["No"].ToString() == string.Empty ||
                    SelectedRow["Name"].ToString() == string.Empty ||
                    SelectedRow["Score"].ToString() == string.Empty)
                {
                    MessageBox.Show("행의 빈 값이 존재합니다. 값을 기입해주세요.");
                    return;
                }

                // DB Update 문 실행.
                string Insert_Query = "UPDATE SAMPLETEST SET Grade = @Value1, Cclass = @Value2, No = @Value3, Name = @Value4, Score = @Value5 WHERE Grade = @Past_Value1 AND Cclass = @Past_Value2 AND No = @Past_Value3 AND Name = @Past_Value4 AND Score = @Past_Value5";

                SqlCommand cmd = new SqlCommand(Insert_Query, conDB);

                cmd.Parameters.AddWithValue("@Value1", Grade);
                cmd.Parameters.AddWithValue("@Value2", Cclass);
                cmd.Parameters.AddWithValue("@Value3", No);
                cmd.Parameters.AddWithValue("@Value4", Name);
                cmd.Parameters.AddWithValue("@Value5", Score);

                cmd.Parameters.AddWithValue("@Past_Value1", Past_Grade);
                cmd.Parameters.AddWithValue("@Past_Value2", Past_Cclass);
                cmd.Parameters.AddWithValue("@Past_Value3", Past_No);
                cmd.Parameters.AddWithValue("@Past_Value4", Past_Name);
                cmd.Parameters.AddWithValue("@Past_Value5", Past_Score);

                cmd.ExecuteNonQuery();

                // Grid 초기화
                Dt = null;
                Create_DtColumn();
                SampleTest.ItemsSource = Dt.DefaultView;

                cmd.Connection = conDB;
                cmd.CommandText = "SELECT * FROM SAMPLETEST";

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                DataSet Dset = new DataSet();

                adapter.Fill(Dset, "SAMPLETEST");

                Dt = Dset.Tables[0].Copy();

                if (Dt.Rows.Count > 0)
                    SampleTest.ItemsSource = Dt.DefaultView;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Remove 버튼 Event
        // <param name="sender"></param>
        // <param name="e"></param>
        private void BtnRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedRow == null)
                {
                    MessageBox.Show("선택된 행이 없습니다.");
                    return;
                }

                string Grade = SelectedRow["Grade"].ToString();
                string Cclass = SelectedRow["Cclass"].ToString();
                string No = SelectedRow["No"].ToString();
                string Name = SelectedRow["Name"].ToString();
                string Score = SelectedRow["Score"].ToString();


                if (SelectedRow["Grade"].ToString() == string.Empty ||
                    SelectedRow["Cclass"].ToString() == string.Empty ||
                    SelectedRow["No"].ToString() == string.Empty ||
                    SelectedRow["Name"].ToString() == string.Empty ||
                    SelectedRow["Score"].ToString() == string.Empty)
                {
                    MessageBox.Show("행의 빈 값이 존재합니다. 값을 기입해주세요.");
                    return;
                }

                // DB Insert 문 실행.
                string Insert_Query = "DELETE FROM SAMPLETEST " +
                                       "WHERE Grade = @Value1 AND Cclass = @Value2 AND No = @Value3 AND Name = @Value4 AND Score = @Value5";

                SqlCommand cmd = new SqlCommand(Insert_Query, conDB);

                cmd.Parameters.AddWithValue("@Value1", Grade);
                cmd.Parameters.AddWithValue("@Value2", Cclass);
                cmd.Parameters.AddWithValue("@Value3", No);
                cmd.Parameters.AddWithValue("@Value4", Name);
                cmd.Parameters.AddWithValue("@Value5", Score);

                cmd.ExecuteNonQuery();

                // Grid 초기화
                Dt = null;
                Create_DtColumn();
                SampleTest.ItemsSource = Dt.DefaultView;

                cmd.Connection = conDB;
                cmd.CommandText = "SELECT * FROM SAMPLETEST";

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);

                DataSet Dset = new DataSet();

                adapter.Fill(Dset, "SAMPLETEST");

                Dt = Dset.Tables[0].Copy();

                if (Dt.Rows.Count > 0)
                    SampleTest.ItemsSource = Dt.DefaultView;
            }
            catch (Exception)
            {
                throw;
            }
        }

        // Enter Key 입력시 Row 추가 방지 Event.
        // <param name="sender"></param>
        // <param name="e"></param>
        private void SampleTest_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MessageBox.Show("행의 값을 입력 후, Create 버튼을 눌러주세요.");
                return;
            }
        }

        // SelectedRow Change Event.
        // <param name="sender"></param>
        // <param name="e"></param>
        private void SampleTest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SelectedRow = (this.SampleTest.SelectedItem as DataRowView);

            if(SelectedRow != null)
            {
                Past_Score = SelectedRow["Score"].ToString();
                Past_No = SelectedRow["No"].ToString();
                Past_Grade = SelectedRow["Grade"].ToString();
                Past_Name = SelectedRow["Name"].ToString();
                Past_Cclass = SelectedRow["Cclass"].ToString();
            }
        }
    }
}
