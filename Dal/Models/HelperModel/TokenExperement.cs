namespace Dal.Models.HelperModel
{
    public class TokenExperement
    {
        public int TokenId { get; set; }
        public TokenModel TokenModel { get; set; }

        public int ExperementId { get; set; }
        public Experement Experement { get; set; }
    }
}
