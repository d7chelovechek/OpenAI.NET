using Newtonsoft.Json;

namespace OpenAI.NET.Models.Api.Complete
{
    public class CompleteRequestParameters
    {
        [JsonIgnore]
        public string Engine
        {
            get
            {
                switch (_engine)
                {
                    case "ada": return "text-ada-001";
                    case "curie": return "text-curie-001";
                    case "babbage": return "text-babbage-001";
                    case "davinci": return "text-davinci-002";
                    case "text-ada-001": return "text-ada-001";
                    case "text-curie-001": return "text-curie-001";
                    case "text-babbage-001": return "text-babbage-001";
                    case "text-davinci-002": return "text-davinci-002";
                    default: return null;
                }
            }
            set => _engine = value?.ToLower();
        }
        [JsonIgnore]
        private string _engine = string.Empty;

        [JsonProperty("prompt")]
        public string Prompt
        {
            get => string.IsNullOrEmpty(_prompt) ? null : _prompt;
            set => _prompt = value;
        }
        [JsonIgnore]
        private string _prompt = string.Empty;

        [JsonProperty("temperature")]
        public double Temperature
        {
            get => _temperature;
            set => _temperature = value;
        }
        [JsonIgnore]
        private double _temperature = 0.5;

        [JsonProperty("max_tokens")]
        public int MaxTokens
        {
            get => _maxTokens;
            set => _maxTokens = value;
        }
        [JsonIgnore]
        private int _maxTokens = 20;

        [JsonProperty("top_p")]
        public double TopP
        {
            get => _topP;
            set => _topP = value;
        }
        [JsonIgnore]
        private double _topP = 1;

        [JsonProperty("frequency_penalty")]
        public double FrequencyPenalty
        {
            get => _frequencyPenalty;
            set => _frequencyPenalty = value;
        }
        [JsonIgnore]
        private double _frequencyPenalty = 0.1;

        [JsonProperty("presence_penalty")]
        public double PresencePenalty
        {
            get => _presencePenalty;
            set => _presencePenalty = value;
        }
        [JsonIgnore]
        private double _presencePenalty = 0.1;

        [JsonProperty("best_of")]
        public int BestOf
        {
            get => _bestOf;
            set => _bestOf = value;
        }
        [JsonIgnore]
        private int _bestOf = 15;
    }
}