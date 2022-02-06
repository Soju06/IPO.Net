using System.Data;

namespace IPO.Net.Test {
    public partial class Main : Form {
        Ipo ipo = new();
        List<TabActionControl> controls = new();

        public Main() {
            InitializeComponent();
            controls.Add(new û������());
            controls.Add(new ��������());
            controls.Add(new ���俹������());
            controls.Add(new ���俹�����());
            controls.Add(new ����û������());
            controls.Add(new �űԻ���());
            controls.Add(new ����м�());
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