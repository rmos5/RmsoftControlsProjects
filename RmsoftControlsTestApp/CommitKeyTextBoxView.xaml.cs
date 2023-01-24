using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace RmsoftControlsTestApp
{
    /// <summary>
    /// Interaction logic for CommitKeyTextBox.xaml
    /// </summary>
    public partial class CommitKeyTextBoxView : UserControl
    {
        public abstract class CommandBase : ICommand
        {
            public event EventHandler CanExecuteChanged;

            public abstract bool CanExecute(object parameter);

            public abstract void Execute(object parameter);

            public void RaiseCanExecuteChanged()
            {
                EventHandler h = CanExecuteChanged;
                h?.Invoke(this, EventArgs.Empty);
            }
        }

        class SearchCommandImpl : CommandBase
        {
            CommitKeyTextBoxView View { get; }

            public SearchCommandImpl(CommitKeyTextBoxView view)
            {
                View = view;
            }

            public override bool CanExecute(object parameter)
            {
                return (parameter as InputObject) != null;
            }

            public async override void Execute(object parameter)
            {
                InputObject obj = (InputObject)parameter;
                await Task.Run(() => View.Search(obj));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public CommandBase SearchCommand { get; }

        public CommitKeyTextBoxView()
        {
            InitializeComponent();
            SearchCommand = new SearchCommandImpl(this);
        }

        ObservableCollection<InputObject> SearchesList { get; } = new ObservableCollection<InputObject>();
        public IEnumerable<InputObject> Searches { get => SearchesList; }

        private InputObject selectedInputObject;

        public InputObject SelectedInputObject
        {
            get { return selectedInputObject; }
            set
            {
                selectedInputObject = value;
                OnPropertyChanged(nameof(SelectedInputObject));
                SearchCommand.RaiseCanExecuteChanged();
            }
        }

        private void TextBox2_TextCommitted(object sender, TextChangedEventArgs e)
        {
            string name = textBox2.Text.Trim();
            Debug.WriteLine($"{nameof(TextBox2_TextCommitted)}: Text={name}", GetType().Name);
            InputObject obj = SearchesList.FirstOrDefault(o => o.Name == name);
            if (obj == null)
            {
                obj = new InputObject(name);
            }
            else
            {
                SearchesList.Remove(obj);
                obj.UpdateCount();
                //SearchesList.Move(SearchesList.IndexOf(obj), 0);
            }

            SearchesList.Insert(0, obj);
        }

        private void Search(InputObject obj)
        {

        }

        private void TextBox2_TextChanged(object sender, TextChangedEventArgs e)
        {
            Debug.WriteLine($"{nameof(TextBox2_TextChanged)}: Text={textBox2.Text}", GetType().Name);
        }

        private void TextBox2_TextInput(object sender, TextCompositionEventArgs e)
        {
            Debug.WriteLine($"{nameof(TextBox2_TextInput)}: Text={textBox2.Text}", GetType().Name);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler h = PropertyChanged;
            h?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
