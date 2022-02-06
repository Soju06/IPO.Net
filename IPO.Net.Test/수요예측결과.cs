﻿namespace IPO.Net.Test {
    public partial class 수요예측결과 : TabActionControl {
        public 수요예측결과() {
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
                Ipo.GetDemandForecastResultsAsync(new DateTimeRange(dateTimePicker1.Value, dateTimePicker2.Value)) :
                Ipo.GetDemandForecastResultsAsync(new PageRange((int)numericUpDown1.Value - 1, (int)numericUpDown2.Value - (int)numericUpDown1.Value + 1)));
            view.DataSource = table.CreateDataTable();
        }

        private void button1_Click(object sender, EventArgs e) {
            OnRefresh();
        }
    }
}
