using ModelsLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class TagAccessor
    {
        public static IEnumerable<Tag> GetTagsList()
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                var tags = context.Tags.ToList();
                return tags;
            }
        }

        public static Tag GetTag(long id)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                var tag = context.Tags.FirstOrDefault(t => t.Tag_ID == id);
                return tag;
            }
        }

        public static long AddTag(Tag tag)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                context.Tags.Add(tag);
                context.SaveChanges();

                return tag.Tag_ID;
            }
        }

        public static void EditTag(Tag tag)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                Tag oldTag = context.Tags.FirstOrDefault(t => t.Tag_ID == tag.Tag_ID);
                oldTag.Tag_Name = tag.Tag_Name;

                context.SaveChanges();
            }
        }

        public static void RemoveTag(long id)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext())
            {
                Tag tag = context.Tags.FirstOrDefault(t => t.Tag_ID == id);

                context.Tags.Remove(tag);
                context.SaveChanges();
            }
        }

        public static bool CheckTagExists(string TagName) {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                Tag tag = context.Tags.FirstOrDefault(t => t.Tag_Name.ToLower().Trim() == TagName.ToLower().Trim());

                return tag != null;
            }
        }
    }
}
