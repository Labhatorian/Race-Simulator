using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    public class Track
    {
        public string Name { get; set; }
        public LinkedList<Section> Sections { get; set; }

        public Track(string name, SectionTypes[] sections)
        {
            Name = name;
            Sections = ConvertSections(sections);
        }

        private LinkedList<Section> ConvertSections(SectionTypes[] sections)
        {
            LinkedList<Section> SectionList = new LinkedList<Section>();
            foreach (SectionTypes sectionType in sections)
            {
                Section section = new Section();
                section.SectionType = sectionType;
                SectionList.AddLast(section);
            }
            return SectionList;
        }
    }
}
