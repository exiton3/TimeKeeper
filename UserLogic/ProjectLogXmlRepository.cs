using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using UserLogic.Domain;

namespace UserLogic
{
    public class ProjectLogXmlRepository:IProjectLogRepository
    {
        private const string ProjectEntry = "ProjectEntry";
        private readonly string fileName;
        private readonly XDocument xDocument;


        public ProjectLogXmlRepository(string fileName)
        {
            this.fileName = fileName;
            if (File.Exists(fileName))
            {
                xDocument = XDocument.Load(fileName);
            }
            else
            {
                xDocument = new XDocument();
                xDocument.Add(new XElement("ProjectsLog"));
            }

        }

        #region Implementation of IProjectLogRepository

        public void Add(ProjectLog projectLog)
        {
            var xElement = new XElement(ProjectEntry);

            xElement.Add(new XAttribute("UserId",projectLog.UserId));
            xElement.Add(new XAttribute("ProjectId", projectLog.ProjectId));
            xElement.Add(new XAttribute("DurationTime", projectLog.DurationTime.ToString()));
            xElement.Add(new XAttribute("StartDate", projectLog.StartDate));

            if (xDocument.Root != null) 
                xDocument.Root.Add(xElement);
            
        }

        public void Delete(ProjectLog projectLog)
        {
            if (xDocument.Root != null)
            {
                var project =
                    xDocument.Root.Descendants(ProjectEntry).Where(
                        element => (DateTime.Parse(element.Attribute("StartDate").Value) == projectLog.StartDate
                                   )).SingleOrDefault();
                if (project != null)
                {
                    project.Remove();
                }
               
            }
           
        }

        public void Save()
        {
            xDocument.Save(fileName);
           
        }

        public List<ProjectLog> GetProjects()
        {
            if (xDocument.Root != null)
            {
                var projects = from project in xDocument.Root.Descendants(ProjectEntry)
                               select new ProjectLog
                                          {

                                              ProjectId = project.Attribute("ProjectId").Value,
                                              UserId = project.Attribute("UserId").Value,
                                              DurationTime = TimeSpan.Parse(project.Attribute("DurationTime").Value),
                                              StartDate = DateTime.Parse(project.Attribute("StartDate").Value)
                                          };
                    
                return projects.ToList();
            }
            return new List<ProjectLog>();

        }

        #endregion
    }
}