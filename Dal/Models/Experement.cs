using Dal.Models.HelperModel;

namespace Dal.Models
{
    public class Experement
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public virtual ICollection<TokenExperement> Tokens { get; set; } = new List<TokenExperement>();
    }
}
