using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace RmsoftControls.AnimatedControls
{
    /// <summary>
    /// Interaction logic for Block.xaml
    /// </summary>
    public partial class Block : UserControl
    {
        public Block()
        {
            InitializeComponent();
        }
        
        public Brush BlockBrush
        {
            get { return (Brush)GetValue(BlockBrushProperty); }
            set { SetValue(BlockBrushProperty, value); }
        }

        public static readonly DependencyProperty BlockBrushProperty =
            DependencyProperty.Register("BlockBrush", typeof(Brush), typeof(Block), new FrameworkPropertyMetadata(Brushes.Aqua));
    }
}
