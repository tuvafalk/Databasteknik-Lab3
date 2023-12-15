using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.Models
{
	public class NoteDetail
	{
        //Constructor
        public NoteDetail() { }

        //Public attributes
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public int Owner { get; set; }
    }
}

