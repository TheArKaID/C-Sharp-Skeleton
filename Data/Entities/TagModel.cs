using RSAHyundai.Data.Entities.ManyToMany;
using RSAHyundai.Data.Entities.ManyToMany;
using System;
using System.Collections.Generic;

namespace RSAHyundai.Data.Entities
{
    public class TagModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public UserModel Owner { get; set; }
        public IList<MMProjectTag> ProjectTags { get; set; }
    }
}
