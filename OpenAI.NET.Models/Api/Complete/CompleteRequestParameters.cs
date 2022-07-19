using Newtonsoft.Json;

namespace OpenAI.NET.Models.Api.Complete
{
    /// <summary>
    /// Parameters for sending a request to Complete action.
    /// </summary>
    public class CompleteRequestParameters
    {
        /// <summary>
        /// Model which will generate completion.
        /// Some models are suitable for natural language tasks,
        /// others specialize in code.
        /// </summary>
        [JsonIgnore]
        public string Engine
        {
            get => _engine switch
            {
                "ada" or "text-ada-001" => "text-ada-001",
                "curie" or "text-curie-001" => "text-curie-001",
                "babbage" or "text-babbage-001" => "text-babbage-001",
                "davinci" or "text-davinci-002" => "text-davinci-002",
                _ => null,
            };
            set => _engine = value?.ToLower();
        }
        [JsonIgnore]
        private string _engine = string.Empty;

        /// <summary>
        /// Text to continue.
        /// </summary>
        [JsonProperty("prompt")]
        public string Prompt
        {
            get
            {
                return string.IsNullOrEmpty(_prompt) ?
                    null : _prompt;
            }
            set => _prompt = value;
        }
        [JsonIgnore]
        private string _prompt = string.Empty;

        /// <summary>
        /// <see cref="Prompt"/>'s language.
        /// </summary>
        [JsonIgnore]
        public string RequestLanguage
        {
            get
            {
                return string.IsNullOrEmpty(_requestLanguage) ?
                    null : _requestLanguage;
            }
            set => _requestLanguage = value?.ToLower();
        }
        [JsonIgnore]
        private string _requestLanguage = "en";

        /// <summary>
        /// <see cref="CompleteResponseBody.Completion"/>'s language.
        /// </summary>
        [JsonIgnore]
        public string ResponseLanguage
        {
            get
            {
                return string.IsNullOrEmpty(_responseLanguage) ?
                    null : _responseLanguage;
            }

            set => _responseLanguage = value?.ToLower();
        }
        [JsonIgnore]
        private string _responseLanguage = "en";

        /// <summary>
        /// Controls randomness:
        /// Lowering results in less random completions.
        /// As temperature approaches zero,
        /// model will become deterministic and repetitive.
        /// </summary>
        [JsonProperty("temperature")]
        public double Temperature
        {
            get => _temperature;
            set => _temperature = value;
        }
        [JsonIgnore]
        private double _temperature = 0.5;

        /// <summary>
        /// Maximum number of tokens to generate.
        /// Requests can use up to 2,048 or 4,000 tokens shared between prompt and completion.
        /// Exact limit varies by model.
        /// </summary>
        [JsonProperty("max_tokens")]
        public int MaxTokens
        {
            get => _maxTokens;
            set => _maxTokens = value;
        }
        [JsonIgnore]
        private int _maxTokens = 20;

        /// <summary>
        /// Controls diversity visa nucleus sampling:
        /// 0.5 means half of all likelihood-weighted options are considered.
        /// </summary>
        [JsonProperty("top_p")]
        public double TopP
        {
            get => _topP;
            set => _topP = value;
        }
        [JsonIgnore]
        private double _topP = 1;

        /// <summary>
        /// How much to penalize new tokens
        /// based on their existing frequency in text so far.
        /// Decreases model's likelihood to repeat same line verbatim.
        /// </summary>
        [JsonProperty("frequency_penalty")]
        public double FrequencyPenalty
        {
            get => _frequencyPenalty;
            set => _frequencyPenalty = value;
        }
        [JsonIgnore]
        private double _frequencyPenalty = 0.1;

        /// <summary>
        /// How much to penalize new tokens
        /// based on whether they appear in text so far.
        /// Increases model's likelihood to talk about new topics.
        /// </summary>
        [JsonProperty("presence_penalty")]
        public double PresencePenalty
        {
            get => _presencePenalty;
            set => _presencePenalty = value;
        }
        [JsonIgnore]
        private double _presencePenalty = 0.1;

        /// <summary>
        /// Generates multiple completions server-side,
        /// and displays only the best.
        /// Streaming only works when set to 1.
        /// Since it acts as a multiplier on number of completions,
        /// this parameters can eat into your token quota very quickly - use caution.
        /// </summary>
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