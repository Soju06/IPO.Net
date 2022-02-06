using System.Data;

namespace IPO.Net.Test {
    public partial class Main : Form {
        Ipo ipo = new();
        List<TabActionControl> controls = new();

        public Main() {
            InitializeComponent();
            controls.Add(new 청구종목());
            controls.Add(new 승인종목());
            controls.Add(new 수요예측일정());
            controls.Add(new 수요예측결과());
            controls.Add(new 공모청약일정());
            controls.Add(new 신규상장());
            controls.Add(new 기업분석());
            Init();
        }

        async void Init() {
            foreach (var control in controls) MakeTab(control);
            tab.Selecting += Tab_Selecting;
        }

        private void Tab_Selecting(object? sender, TabControlCancelEventArgs e) {
            CurrentAction.OnEnter();
        }

        void MakeTab(TabActionControl control) {
            var t = new TabPage(control.Name);
            control.Dock = DockStyle.Right;
            control.Size = new Size(300, 800);
            t.Controls.Add(control);
            DataGridView view = new();
            view.Dock = DockStyle.Fill;
            t.Controls.Add(view);
            view.BringToFront();
            t.Tag = control;
            control.Ipo = ipo;
            tab.TabPages.Add(t);
            control.OnViewLoad(view);
        }

        TabActionControl CurrentAction => (TabActionControl)tab.SelectedTab.Tag;

        void OnShown(object sender, EventArgs e) {
            CurrentAction.OnEnter();
        }

        protected override void OnKeyDown(KeyEventArgs e) {
            if (e.KeyCode == Keys.F5) CurrentAction.OnRefresh();
            base.OnKeyDown(e);
        }
    }

    public class TabActionControl : UserControl {
        public Ipo Ipo;

        public virtual void OnViewLoad(DataGridView view) {

        }

        public virtual void OnEnter() {

        }

        public virtual void OnRefresh() {

        }
    }
}