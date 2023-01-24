using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Interactivity;

namespace RmsoftControls.Behaviors
{
    public class DisableCutCopyPasteBehavior : Behavior<TextBox>
    {
        public bool EnableCut { get; set; }

        public bool EnableCopy { get; set; }

        public bool EnablePaste { get; set; }

        protected override void OnAttached()
        {
            if (AssociatedObject == null)
                return;

            CommandManager.AddPreviewCanExecuteHandler(AssociatedObject, CanExecuteHandler);
        }

        protected override void OnDetaching()
        {
            if (AssociatedObject == null)
                return;

            CommandManager.RemovePreviewCanExecuteHandler(AssociatedObject, CanExecuteHandler);
        }

        private void CanExecuteHandler(object sender, CanExecuteRoutedEventArgs e)
        {
            if (e.Command == ApplicationCommands.Cut)
            {
                e.CanExecute = EnableCut && AssociatedObject.Text?.Length > 0;
                e.Handled = true;
            }
            else if (e.Command == ApplicationCommands.Copy)
            {
                e.CanExecute = EnableCopy && AssociatedObject.Text?.Length > 0;
                e.Handled = true;
            }
            else if (e.Command == ApplicationCommands.Paste)
            {
                e.CanExecute = EnablePaste;
                e.Handled = true;
            }
        }
    }
}
