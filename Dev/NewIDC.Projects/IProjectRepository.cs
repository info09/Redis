using System.Collections.Generic;

namespace NewIDC.Projects
{
	public interface IProjectRepository
	{
		List<ProjectConfig> GetProjects();

		ProjectConfig GetProject(int project_id);

		void AddProject(ProjectConfig project);

		void UpdateProject(ProjectConfig project);

		void DeleteProject(int project_id);

	}

}

