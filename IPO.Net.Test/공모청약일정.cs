namespace IPO.Net.Test {
    public partial class 공모청약일정 : TabActionControl {
        public 공모청약일정() {
            InitializeComponent();
        }

        DataGridView view;

        public override void OnViewLoad(DataGridView view) {
            this.view = view;
            view.ReadOnly = true;
        }

        public override void OnEnter() {
            if (view.DataSource == null)
                OnRefresh();
        }

        async public override void OnRefresh() {
            if (radioButton1.Checked && dateTimePicker1.Value > dateTimePicker2.Value) {
                MessageBox.Show("시작일이 종료일보다 클 수 없습니다.");
                return;
            }
            if (!radioButton1.Checked && numericUpDown1.Value > numericUpDown2.Value) {
                MessageBox.Show("시작 페이지보다 종료페이지가 클 수 없습니다.");
                return;
            }
            var table = await (radioButton1.Checked ?
                Ipo.GetIPOSubscriptionSchedulesAsync(new DateTimeRange(dateTimePicker1.Value, dateTimePicker2.Value)) :
                radioButton2.Checked ?
                Ipo.GetIPOSubscriptionSchedulesAsync(new PageRange((int)numericUpDown1.Value - 1, (int)numericUpDown2.Value - (int)numericUpDown1.Value + 1)) :
                Ipo.GetIPOSubscriptionSchedulesAsync(DateTimeRange.BackToEnd(DateTime.Now), false));
            view.DataSource = table.CreateDataTable();
            view.Columns[1].DefaultCellStyle.Format = "yyyy/MM/dd ~| MM/dd";
        }

        private void 공모청약일정_Load(object sender, EventArgs e) {
            OnRefresh();
        }

        private void button2_Click(object sender, EventArgs e) {
            OnRefresh();
        }
    }
}
