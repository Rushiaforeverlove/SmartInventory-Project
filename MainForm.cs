using SmartInventory.Data;
using SmartInventory.Models;
using SmartInventory.Services;
using System.ComponentModel;
using System.Diagnostics;

namespace SmartInventory
{
    public partial class MainForm : Form
    {
        // 畫面（Designer）已備好下列控件，直接照投影片使用即可：
        //   dgv（清單）、txtSearch、cmbCategory、txtName/txtCategory/txtQuantity/txtPrice、
        //   btnAdd/btnUpdate/btnDelete、btnCheck、lblTotal
        //
        // TODO（13-1）：宣告全部商品清單
        //   private List<Product> all = new List<Product>();
        private List<Product> all = new List<Product>();

        //綁定畫面用
        private BindingList<Product> view = new BindingList<Product>();
        public MainForm()
        {
            InitializeComponent();
            dgv.DataSource = view;
            dgv.AllowUserToAddRows = false; // ← 拿掉最後那條可輸入的空白列
            dgv.AllowUserToDeleteRows = false; // 不讓在表格上直接刪列
            dgv.MultiSelect = false; // 一次只選一列

            //設定ComboBox
            cmbCategory.Items.Add("全部");
            cmbCategory.Items.AddRange(ProductService.Categories);
            cmbCategory.SelectedIndex = 0;

            Dbhelper.InitDb();
            all = Dbhelper.GetAllProducts();

            foreach (var p in all)
            {
                Debug.WriteLine(p);
            }
            RefreshView();

            // TODO（13-1）：啟動就讀資料庫

            //   all = DbHelper.GetAllProducts();

            // TODO（13-2）：接上畫面
            //   宣告 BindingList<Product> view，dgv.DataSource = view;
            //   cmbCategory 加入「全部/電子/生活/文具/食品」並 SelectedIndex = 0;
            //   掛事件：txtSearch.TextChanged、cmbCategory.SelectedIndexChanged、dgv.CellClick → RefreshView/帶入欄位;
            //   呼叫 RefreshView();

            // TODO（13-3）：動態加「統計圖表」按鈕
            //   var btnChart = new Button { Text = "統計圖表", AutoSize = true };
            //   btnChart.Click += (_, _) => new ChartForm(all).ShowDialog();
            //   flowLayoutPanel1.Controls.Add(btnChart);
        }

        public void RefreshView()
        {
            //篩選機制
            var filtered = ProductService.Search(all, txtSearch.Text.Trim(), cmbCategory.Text.Trim());

            view.Clear();

            foreach (var p in filtered)
            {
                view.Add(p);
            }
        }

        private bool ReadInput(out Product product)
        {
            product = new Product();
            if (txtName.Text.Trim() == "" || txtCategory.Text.Trim() == "")
            {
                MessageBox.Show("商品名稱或分類不能為空!");
                return false;
            }

            if (!int.TryParse(txtQuantity.Text, out int q) || q <= 0)
            {
                MessageBox.Show("數量輸入不正確!");
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text, out decimal p) || p <= 0)
            {
                MessageBox.Show("金額輸入不正確!");
                return false;
            }

            product.Name = txtName.Text;
            product.Category = txtCategory.Text;
            product.Quantity = q;
            product.Price = p;

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            //增加是否授權



            if (!ReadInput(out Product p)) return;

            //插入資料庫
            Dbhelper.InsertProduct(p);
            all = Dbhelper.GetAllProducts();

            //更新畫面
            RefreshView();
            ClearInput();


        }

        private void ClearInput()
        {
            TextBox[] boxs = { txtName, txtCategory, txtPrice, txtQuantity };
            foreach (var b in boxs) b.Text = string.Empty;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearInput();
        }

        private void dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.RowIndex >= view.Count) return;
            var p = view[e.RowIndex];
            txtName.Text = p.Name;
            txtCategory.Text = p.Category;
            txtPrice.Text = p.Price.ToString();
            txtQuantity.Text = p.Quantity.ToString();

            //MessageBox.Show($"click:{e.RowIndex}");

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (dgv.CurrentRow == null) return;

            int index = dgv.CurrentRow.Index;
            var p = view[index];

            if (MessageBox.Show($"是否刪除:{p.Id}-{p.Name}", "確認", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            Dbhelper.DeleteProducts(p);

            all = Dbhelper.GetAllProducts();
            RefreshView();

            if (view.Count > 0)
            {
                if (index >= view.Count) index = view.Count - 1;
            }

            //維持當下位置
            dgv.Rows[index].Selected = true;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            if (dgv.CurrentRow == null) return;

            if (!ReadInput(out Product p)) return;

            int index = dgv.CurrentRow.Index;

            p.Id = view[index].Id;

            if (MessageBox.Show($"是否更新:{p.Id}-{p.Name}", "確認", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            //更新商品
            Dbhelper.UpdateProducts(p);
            all = Dbhelper.GetAllProducts();
            RefreshView();


            //維持當下位置
            dgv.Rows[index].Selected = true;
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            RefreshView();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            RefreshView();
        }

        // ───── 以下方法 13-2 才會寫（按鈕事件可在 Designer 雙擊自動產生）─────
        // 13-2：RefreshView()             刷新清單與總價值（用 ProductService.Search 過濾）
        // 13-2：ReadInput(out Product p)  讀輸入＋TryParse 驗證
        // 13-2：ClearInput()              清空輸入框
        // 13-2：btnAdd_Click             新增 → InsertProduct → 重讀 → RefreshView → ClearInput
        // 13-2：dgv_CellClick            點選列帶回輸入欄
        // 13-2：btnUpdate_Click          修改（p.Id 沿用主鍵）→ UpdateProduct → 重讀 → RefreshView
        // 13-2：btnDelete_Click          確認後 DeleteProduct → 重讀 → RefreshView
        // 13-2：btnCheck_Click           ProductService.GetLowStock → MessageBox 列出
    }
}
