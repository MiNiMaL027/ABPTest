using Dal.Models.HelperModel;

namespace Dal.Models
{
    public class TokenModel
    {
        public int id { get; set; }
        public string Token { get; set; }

        public virtual ICollection<TokenExperement> Experements { get; set; } = new List<TokenExperement>();
    }
}
