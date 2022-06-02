using Newtonsoft.Json;

namespace OpenAI.NET.Models.Api.Complete
{
    public class CompleteRequestParameters
    {
        [JsonIgnore]
        public string Action
        {
            get => string.IsNullOrEmpty(_action) ? null : _action;
            set => _action = value;
        }
        [JsonIgnore]
        private string _action = string.Empty;

        [JsonIgnore]
        public string Engine
        {
            get => _engine switch
            {
                "ada" => "text-ada-001",
                "curie" => "text-curie-001",
                "babbage" => "text-babbage-001",
                "davinci" => "text-davinci-002",
                _ => null
            };
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