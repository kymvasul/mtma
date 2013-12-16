using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
//using TeamProMobile.Annotations;

namespace TeamProMobileApplicationIOS.Internals
{
    public class Project : IComparable<Project>
    {
        private String _name;
        private String _id;
        private String _path;
        private String _pathD;
        private String _projectParent;
        private bool _projectReportable;
        //private DateTime _projectCreated;

        public String Name { get { return _name; } private set { _name = value; } }
        public String Path { get { return _path; } private set { _path = value; } }
        public String PathD { get { return _pathD; } private set { _pathD = value; } }
        public String Id { get { return _id; } private set { _id = value; } }
        public String ProjectParent { get { return _projectParent; } private set { _projectParent = value; } }
        public bool ProjectReportable { get { return _projectReportable; } private set { _projectReportable = value; } }
        public String ShortPath
        {
            get { return Path.Split('/').LastOrDefault(); }
        }

        // ConstructorS
        public Project(String name, String id, String path, String pathD, String projectParent, bool projectReportable)
        {
            Name = name;
            Id = id;
            Path = path;
            PathD = pathD;
            ProjectParent = projectParent;
            ProjectReportable = projectReportable;
        }

        // Override ToString method
        public override string ToString()
        {
            return _name;
        }

        //IComparable<Shape> Member
        public int CompareTo(Project other)
        {
            return (Id == other.Id) ? 0 : -1;
        }
        
        public Boolean IsEven { get; set; }
    }
}
