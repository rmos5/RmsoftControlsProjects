using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace RmsoftControls.AnimatedControls
{
    /// <summary>
    /// Interaction logic for LoadingAnimation.xaml
    /// </summary>
    public partial class LoadingAnimation : UserControl
    {
        public bool IsLoadingMessageVisible
        {
            get { return (bool)GetValue(IsLoadingMessageVisibleProperty); }
            set { SetValue(IsLoadingMessageVisibleProperty, value); }
        }

        public static readonly DependencyProperty IsLoadingMessageVisibleProperty =
            DependencyProperty.Register("IsLoadingMessageVisible", typeof(bool), typeof(LoadingAnimation), new FrameworkPropertyMetadata(true));

        public Brush BlockBrush
        {
            get { return (Brush)GetValue(BlockBrushProperty); }
            set { SetValue(BlockBrushProperty, value); }
        }

        public static readonly DependencyProperty BlockBrushProperty =
            DependencyProperty.Register("BlockBrush", typeof(Brush), typeof(LoadingAnimation), new FrameworkPropertyMetadata(Brushes.Aqua, OnBlockBrushChanged));

        public string LoadingMessage
        {
            get { return (string)GetValue(LoadingMessageProperty); }
            set { SetValue(LoadingMessageProperty, value); }
        }

        public static readonly DependencyProperty LoadingMessageProperty =
            DependencyProperty.Register("LoadingMessage", typeof(string), typeof(LoadingAnimation), new FrameworkPropertyMetadata(ControlResources.LoadingMessage));

        public Brush MessageBackground
        {
            get { return (Brush)GetValue(MessageBackgroundProperty); }
            set { SetValue(MessageBackgroundProperty, value); }
        }

        public static readonly DependencyProperty MessageBackgroundProperty =
            DependencyProperty.Register("MessageBackground", typeof(Brush), typeof(LoadingAnimation), new FrameworkPropertyMetadata(Brushes.Transparent));

        public Brush MessageBorderBrush
        {
            get { return (Brush)GetValue(MessageBorderBrushProperty); }
            set { SetValue(MessageBorderBrushProperty, value); }
        }

        public static readonly DependencyProperty MessageBorderBrushProperty =
            DependencyProperty.Register("MessageBorderBrush", typeof(Brush), typeof(LoadingAnimation), new FrameworkPropertyMetadata(Brushes.Transparent));

        public Thickness MessageBorderThickness
        {
            get { return (Thickness)GetValue(MessageBorderThicknessProperty); }
            set { SetValue(MessageBorderThicknessProperty, value); }
        }

        public static readonly DependencyProperty MessageBorderThicknessProperty =
            DependencyProperty.Register("MessageBorderThickness", typeof(Thickness), typeof(LoadingAnimation), new FrameworkPropertyMetadata(default(Thickness)));

        public double MessageOpacity
        {
            get { return (double)GetValue(MessageOpacityProperty); }
            set { SetValue(MessageOpacityProperty, value); }
        }

        public static readonly DependencyProperty MessageOpacityProperty =
            DependencyProperty.Register("MessageOpacity", typeof(double), typeof(LoadingAnimation), new FrameworkPropertyMetadata(1.0));

        public LoadingAnimation()
        {
            InitializeComponent();
            Loaded += LoadingAnimation_Loaded;
        }

        private void LoadingAnimation_Loaded(object sender, RoutedEventArgs e)
        {
            if (!DesignerProperties.GetIsInDesignMode(this))
            {
                Storyboard storyboard = (Storyboard)Resources["progressAnimation"];
                BeginStoryboard(storyboard);
            }
        }

        private static void OnBlockBrushChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            Brush brush = e.NewValue as Brush ?? Brushes.Aqua;
            LoadingAnimation obj = d as LoadingAnimation;
            obj?.ApplyBlockBrush(brush);
        }

        private void ApplyBlockBrush(Brush brush)
        {
            DependencyObject dpo = LayoutRoot;
            List<Block> blocks = new List<Block>();
            FindChildren<Block>(dpo, blocks);
            foreach (Block block in blocks)
            {
                block.BlockBrush = brush;
            }
        }

        internal static void FindChildren<T>(DependencyObject startNode, List<T> results)
        where T : DependencyObject
        {
            int count = VisualTreeHelper.GetChildrenCount(startNode);
            for (int i = 0; i < count; i++)
            {
                DependencyObject current = VisualTreeHelper.GetChild(startNode, i);
                if ((current.GetType()).Equals(typeof(T)) || (current.GetType().IsSubclassOf(typeof(T))))
                {
                    T asType = (T)current;
                    results.Add(asType);
                }

                FindChildren<T>(current, results);
            }
        }
    }
}
