using System.ComponentModel.Design;
using System.IO;
using System.Threading.Tasks;
using Conan.VisualStudio.Core;
using Conan.VisualStudio.Services;

namespace Conan.VisualStudio.Menu
{
    /// <summary>
    /// Command to install the Conan prop files into the project file.
    /// </summary>
    internal sealed class IntegrateIntoProjectCommand : MenuCommandBase
    {
        protected override int CommandId => 4130;

        private readonly IVcProjectService _vcProjectService;

        public IntegrateIntoProjectCommand(
            IMenuCommandService commandService,
            IDialogService dialogService,
            IVcProjectService vcProjectService) : base(commandService, dialogService)
        {
            _vcProjectService = vcProjectService;
        }

        protected internal override async Task MenuItemCallback()
        {
            var project = _vcProjectService.GetActiveProject();
            var projectDirectory = project.ProjectDirectory;
            var conanfileDirectory = await ConanPathHelper.GetNearestConanfilePath(projectDirectory);
            var propFilePath = Path.Combine(conanfileDirectory, @"conan\conanbuildinfo_multi.props");
            var relativePropFilePath = ConanPathHelper.GetRelativePath(projectDirectory, propFilePath);
            await _vcProjectService.AddPropsImport(project.ProjectFile, relativePropFilePath);
        }
    }
}
