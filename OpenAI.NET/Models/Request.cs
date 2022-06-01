using Newtonsoft.Json;

namespace OpenAI.NET.Models
{
    public class Request
    {
        [JsonIgnore]
        public string Action
        {
            get
            {
                return string.IsNullOrEmpty(_action) ? null : _action;
            }
            set
            {
                _action = value;
            }
        }
        [JsonIgnore]
        private string _action;

        [JsonIgnore]
        public string Engine
        {
            get
            {
                return _engine switch
                {
                    "ada" => "text-ada-001",
                    "curie" => "text-curie-001",
                    "babbage" => "text-babbage-001",
                    "davinci" => "text-davinci-002",
                    _ => null
                };
            }
            set
            {
                _engine = value?.ToLower();
            }
        }
        [JsonIgnore]
        private string _engine;

        [JsonProperty("prompt")]
        public string Prompt
        {
            get
            {
                return string.IsNullOrEmpty(_prompt) ? null : _prompt;
            }
            set
            {
                _prompt = value;
            }
        }
        [JsonIgnore]
        private string _prompt;

        [JsonProperty("temperature")]
        public double? Temperature { get; set; }

        [JsonProperty("max_tokens")]
        public int? MaxTokens { get; set; }

        [JsonProperty("top_p")]
        public double? TopP { get; set; }

        [JsonProperty("frequency_penalty")]
        public double? FrequencyPenalty { get; set; }

        [JsonProperty("presence_penalty")]
        public double? PresencePenalty { get; set; }

        [JsonProperty("best_of")]
        public int? BestOf { get; set; }

        public void SetDefaultValues()
        {
            Temperature ??= 0.5;
            MaxTokens ??= 20;
            TopP ??= 1;
            FrequencyPenalty ??= 0.1;
            PresencePenalty ??= 0.1;
            BestOf ??= 1;
        }
    }
}