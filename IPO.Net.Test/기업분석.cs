using HtmlAgilityPack;

namespace IPO.Net.Test {
    public partial class 기업분석 : TabActionControl {
        DataGridView view;

        public 기업분석() {
            InitializeComponent();
        }

        public override void OnViewLoad(DataGridView view) {
            this.view = view;
            view.ReadOnly = true;
        }

        async public override void OnRefresh() {
            if (string.IsNullOrWhiteSpace(textBox1.Text)) {
                MessageBox.Show("분석 Id가 없습니다.");
                return;
            }

            try {
                var table = await Ipo.GetCorporateAnalysisAsync(textBox1.Text);
                view.DataSource = table.CreateDataTable();
            } catch (NodeNotFoundException) {
                MessageBox.Show("기업분석이 없습니다.");
            }
        }

        public override void OnEnter() {
            if (view.DataSource == null)
                OnRefresh();
        }

        private void button1_Click(object sender, EventArgs e) {
            OnRefresh();
        }
    }
}
