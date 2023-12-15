using System;
namespace Lab3.Models
{
	public class ViewModelPA
	{
		public IEnumerable<NoteDetail> NoteDetailList { get; set; }
        public IEnumerable<NoteColaborators> ColabList { get; set; }
        public IEnumerable<OwnerDetail> OwnerDetailList { get; set; }
    }
}

