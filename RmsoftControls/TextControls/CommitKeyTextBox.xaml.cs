using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace RmsoftControls.TextControls
{
    /// <summary>
    /// Textbox exposing an input commit key <see cref="CommitKey"/>. Default is <see cref="Key.Return"/>.
    /// When input detects commit key, <see cref="TextCommitted"/> event raised and textbox text is optionally selected <see cref="SelectOnCommit"/>.
    /// When textbox is empty or <see cref="IsReadOnly"/> is true, commit event is not raised.
    /// <see cref="TextChangedEvent"/> helps to track text input.
    /// </summary>
    public partial class CommitKeyTextBox : UserControl
    {
        /// <summary>
        /// Event for "Text has changed"
        /// </summary>
        public static readonly RoutedEvent TextChangedEvent = TextBoxBase.TextChangedEvent.AddOwner(typeof(CommitKeyTextBox));

        /// <summary>
        /// Event fired from this text box when its inner content
        /// has been changed.
        /// </summary>
        public event TextChangedEventHandler TextChanged
        {
            add
            {
                AddHandler(TextChangedEvent, value);
            }

            remove
            {
                RemoveHandler(TextChangedEvent, value);
            }
        }

        public static readonly RoutedEvent TextCommittedEvent = EventManager.RegisterRoutedEvent(
            "TextCommitted", // Event name
            RoutingStrategy.Bubble, //
            typeof(TextChangedEventHandler), //
            typeof(CommitKeyTextBox)); //

        public event TextChangedEventHandler TextCommitted
        {
            add
            {
                AddHandler(TextCommittedEvent, value);
            }

            remove
            {
                RemoveHandler(TextCommittedEvent, value);
            }
        }

        public static readonly DependencyProperty TextProperty = TextBox.TextProperty.AddOwner(
            typeof(CommitKeyTextBox));

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty IsReadOnlyProperty = TextBoxBase.IsReadOnlyProperty.AddOwner(typeof(CommitKeyTextBox));

        public bool IsReadOnly
        {
            get { return (bool)GetValue(IsReadOnlyProperty); }
            set { SetValue(IsReadOnlyProperty, value); }
        }

        public static readonly DependencyProperty SelectOnCommitProperty = DependencyProperty.Register(
            "SelectOnCommit",
            typeof(bool),
            typeof(CommitKeyTextBox),
            new FrameworkPropertyMetadata(true));

        public bool SelectOnCommit
        {
            get => (bool)GetValue(SelectOnCommitProperty);
            set => SetValue(SelectOnCommitProperty, value);
        }

        public static readonly DependencyProperty CommitKeyProperty = KeyBinding.KeyProperty.AddOwner(
            typeof(CommitKeyTextBox),
            new FrameworkPropertyMetadata(Key.Return));

        public Key CommitKey
        {
            get => (Key)GetValue(CommitKeyProperty);
            set => SetValue(CommitKeyProperty, value);
        }

        public CommitKeyTextBox()
        {
            InitializeComponent();
            TextBox1.TextChanged += TextBox1_TextChanged;
        }
        
        TextChangedEventArgs textChangedEventArgs = null;

        private void TextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            textChangedEventArgs = e;
            Text = TextBox1?.Text;
        }

        private void Commit_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            bool result = TextBox1?.Text?.Length > 0 && !IsReadOnly;
            e.CanExecute = result;
        }

        private void Commit_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            if (SelectOnCommit)
            {
                TextBox1.SelectAll();
            }

            RaiseEvent(new TextChangedEventArgs(TextCommittedEvent, textChangedEventArgs.UndoAction, textChangedEventArgs.Changes));
        }

        protected override void OnPropertyChanged(DependencyPropertyChangedEventArgs e)
        {
            //Debug.WriteLine($"{nameof(OnPropertyChanged)}: Name={e.Property.Name};Owner={e.Property.OwnerType};old={e.OldValue};new={e.NewValue}");
            if (IsInitialized)
            {
                if (e.Property == TextProperty)
                {
                    TextBox1.Text = e.NewValue as string;
                }
                else if (e.Property == CommitKeyProperty)
                {
                    CommitKeyBinding.Key = (Key)e.NewValue;
                }
            }

            base.OnPropertyChanged(e);
        }
    }
}
