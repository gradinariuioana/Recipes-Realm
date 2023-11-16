using ModelsLibrary;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess {
    public class TagAccessor
    {
        public static IEnumerable<Tag> GetTagsList()
        {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                var tags = context.Tags.ToList();
                return tags;
            }
        }

        public static long AddTag(Tag tag)
        {
            using (var context = new DatabaseRepository.RecipesRealmContext()) {
                context.Tags.Add(tag);
                context.SaveChanges();

                return tag.Tag_ID;
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
