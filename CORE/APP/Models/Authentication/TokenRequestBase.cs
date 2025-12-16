using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CORE.APP.Models.Authentication 
{ 
    // Represents a request to generate a token response 
    public class TokenRequestBase
    {

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [JsonIgnore]
        public string SecurityKey { get; set; }

        [JsonIgnore]
        public string Issuer { get; set; }

        [JsonIgnore]
        public string Audience { get; set; }
    }
}