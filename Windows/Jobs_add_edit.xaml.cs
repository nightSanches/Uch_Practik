using System.Windows;

namespace Airport.Windows
{
    public partial class Jobs_add_edit : Window
    {
        public string Job_naim;
        public Jobs_add_edit()
        {
            InitializeComponent();
        }
        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            if (Job_naim != null)
            {
                job_naim.Text = Job_naim.Trim();
            }
        }

        private void SaveClick(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(job_naim.Text) || !Classes.Common.CheckRegex.Match("^[а-яА-ЯёЁ0-9\\s]+$", job_naim.Text))
            {
                MessageBox.Show("Неправильно указано наименование должности", "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                this.DialogResult = true;
            }
        }

        public string Naimenovanye
        {
            get { return job_naim.Text; }
        }
    }
}
