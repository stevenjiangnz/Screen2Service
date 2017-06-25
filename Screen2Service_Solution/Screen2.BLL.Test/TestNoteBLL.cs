using Microsoft.VisualStudio.TestTools.UnitTesting;
using Screen2.DAL;
using Screen2.Entity;
using Screen2.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Screen2.BLL.Test
{
    [TestClass]
    public class TestNoteBLL
    {
        UnitWork _unit = new UnitWork(new DataContext());

        [TestMethod]
        public void Test_Create()
        {
            NoteBLL bll = new NoteBLL(_unit);

            Note note = new Note {
                Comment = "Test comment",
                Create = DateTime.Now,
                ShareId = 1585,
                Type = NoteType.General.ToString(),
                CreatedBy = "2b658482-6a38-4ed3-b356-77fe9b1569f1"
            };

            bll.Create(note);
        }

        [TestMethod]
        public void Test_GetNotesByShareAndUser()
        {
            NoteBLL bll = new NoteBLL(_unit);

            List<Note> nList = bll.GetNotesByShareAndUser(2433, 20160527, "2b658482-6a38-4ed3-b356-77fe9b1569f1");
        }
    }
}
