﻿using System;
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

        /// <summary>
        /// Het circuit wordt hier gemaakt.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="sections"></param>
        public Track(string name, SectionTypes[] sections)
        {
            Name = name;
            Sections = ConvertSections(sections);
        }

        /// <summary>
        /// Converteert van array naar linkedlist
        /// </summary>
        /// <param name="sections"></param>
        /// <returns></returns>
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
