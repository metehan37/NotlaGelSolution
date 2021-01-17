using NotlaGel.BusinessLayer.Abstract;
using NotlaGel.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NotlaGel.BusinessLayer
{
    public class CategoryManager:ManagerBase<Category>
    {
        public override int Delete(Category category)
        {
            //NoteManager noteManager = new NoteManager();
            //LikedManager likedManager = new LikedManager();
            //CommentManager commentManager = new CommentManager();

            //foreach (Note note in category.Notes.ToList())
            //{
            //    foreach (Liked like in note.Likes.ToList())
            //    {
            //        likedManager.Delete(like);
            //    }
            //    foreach (Comment comment in note.Comments)
            //    {
            //        commentManager.Delete(comment);
            //    }
            //    noteManager.Delete(note);
            //}

            return base.Delete(category);
        }
    }
}
