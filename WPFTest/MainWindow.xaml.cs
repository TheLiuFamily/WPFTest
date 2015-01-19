using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var arts= Article.CreateArticles("http://meiwenrishang.com/");
            list.ItemsSource = arts;
        }

        private void list_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            var v = list.SelectedItem as Article;
            var art = Article.CreateArticle(v.Url);
            TextConent pop = new TextConent();
            pop.image1.Source = new BitmapImage(new Uri(art.ImageUrl, UriKind.Absolute));
            pop.txt1.Text = art.Content;
            pop.head1.Text = art.Head;
            pop.ShowDialog();
        }

    }
}
