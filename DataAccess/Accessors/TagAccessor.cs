using DatabaseRepository;
using ModelsLibrary;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess {
    public class TagAccessor : ITagAccessor {

        public RecipesRealmContext context;

        public TagAccessor(IRecipesRealmContext db) {
            context = (RecipesRealmContext)db;
        }

        public Tag GetTagById(long id) {
            var tag = context.Tags.FirstOrDefault(c => c.Tag_ID == id);
            return tag;
        }
        public IEnumerable<Tag> GetTagsList() {
            var tags = context.Tags.ToList();
            return tags;
        }

        public long AddTag(Tag tag) {
            context.Tags.Add(tag);
            context.SaveChanges();

            return tag.Tag_ID;
        }

        public bool CheckTagExists(string TagName) {
            Tag tag = context.Tags.FirstOrDefault(t => t.Tag_Name.ToLower().Trim() == TagName.ToLower().Trim());

            return tag != null;
        }
    }

    public interface ITagAccessor {
        Tag GetTagById(long id);
        IEnumerable<Tag> GetTagsList();

        long AddTag(Tag tag);

        bool CheckTagExists(string TagName);
    }
}
