using System.Windows.Controls;

namespace DiplomKarakuyumjyan
{
    public partial class MainMenuButton : UserControl
    {
        public MainMenuButton()
        {
            InitializeComponent();
        }
        
        public string ContentProperty { get; set; }
    }
}