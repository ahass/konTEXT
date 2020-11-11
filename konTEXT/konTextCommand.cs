using System;
using System.ComponentModel.Design;
using System.Windows.Controls;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Task = System.Threading.Tasks.Task;

namespace konTEXT
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class KonTextCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        private uint _toolWindowInstanceId;
        
        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("6a3c14db-d9ae-484d-97c0-9ab09e328d9d");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage _package;

        private IVsWindowFrame _vsWindowFrame;
        private UmlToolWindowControl _umlToolWindowControl;


        /// <summary>
        /// Initializes a new instance of the <see cref="KonTextCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private KonTextCommand(AsyncPackage package, OleMenuCommandService commandService)
        {
            this._package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandId = new CommandID(CommandSet, CommandId);
            var menuItem = new MenuCommand(this.ShowUmlToolWindow, menuCommandId);
            commandService.AddCommand(menuItem);
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static KonTextCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package)
        {
            // Switch to the main thread - the call to AddCommand in konTextCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new KonTextCommand(package, commandService);
        }

        /// <summary>
        /// Shows the tool window with result.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        private void ShowUmlToolWindow(object sender, EventArgs e)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            const string toolWindowGuid = "6BF11835-8A31-496C-B01E-94F731AFC7CE";

            _umlToolWindowControl = new UmlToolWindowControl();
            _vsWindowFrame = CreateToolWindow("Uml Diagram " + _toolWindowInstanceId, toolWindowGuid, _umlToolWindowControl);
            
            ErrorHandler.ThrowOnFailure(_vsWindowFrame.Show());
        }
        
        private IVsWindowFrame CreateToolWindow(string caption, string guid, UserControl userControl)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            var guidNull = Guid.Empty;
            var position = new int[1];

            var uiShell = (IVsUIShell)_package.GetServiceAsync(typeof(SVsUIShell)).Result;

            var toolWindowPersistenceGuid = new Guid(guid);

            var result = uiShell.CreateToolWindow((uint)__VSCREATETOOLWIN.CTW_fMultiInstance,
                ++_toolWindowInstanceId, userControl, ref guidNull, ref toolWindowPersistenceGuid,
                ref guidNull, null, caption, position, out var windowFrame);

            ErrorHandler.ThrowOnFailure(result);

            return windowFrame;
        }

    }
}
