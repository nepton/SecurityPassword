namespace SecurityPassword.OneTimePassword
{
    /// <summary>
    /// Provide security code generation procedures, security code generation, within the specified time valid
    /// </summary>
    public class TotpProvider : ITotpProvider
    {
        private readonly DateTime     _unixEpoch = new(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        private readonly TimeSpan     _timeStep;
        private readonly int          _timeStepBackward;
        private readonly int          _timeStepForward;
        private readonly HotpProvider _hotpProvider;

        public TotpProvider(TotpOptions options)
        {
            if (options == null)
                throw new ArgumentNullException(nameof(options));

            if (string.IsNullOrEmpty(options.SecurityToken))
                throw new ArgumentException("Missing TOTP security token");

            if (options.CodeLength is < 2 or > 8)
                throw new ArgumentOutOfRangeException(nameof(options.CodeLength), "The length of the code must be between 2 and 8");

            if (options.TimeStepForward is < 0 or > 30)
                throw new ArgumentOutOfRangeException(nameof(options.TimeStepForward), "The time step forward must be between 1 and 30");

            if (options.TimeStepBackward is < 0 or > 30)
                throw new ArgumentOutOfRangeException(nameof(options.TimeStepBackward), "The time step backward must be between 0 and 30");

            _timeStep         = TimeSpan.FromSeconds(options.EffectiveInSeconds);
            _timeStepBackward = options.TimeStepBackward;
            _timeStepForward  = options.TimeStepForward;
            _hotpProvider = new HotpProvider(new HotpOptions
            {
                HashType      = options.HashType,
                CodeLength    = options.CodeLength,
                SecurityToken = options.SecurityToken
            });
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsConfigure"></param>
        public TotpProvider(Action<TotpOptions> optionsConfigure) : this(ApplyOptions(optionsConfigure))
        {
        }

        private static TotpOptions ApplyOptions(Action<TotpOptions> optionsConfigure)
        {
            var options = new TotpOptions();
            optionsConfigure(options);
            return options;
        }

        /// <summary>
        /// Generate a one time password for the given time and modifier
        /// </summary>
        /// <param name="modifier">Can be null or mobile number, Email etc</param>
        /// <param name="time">生成指定时间的CODE</param>
        /// <returns></returns>
        public string Generate(string? modifier, DateTime time)
        {
            return GenerateCore(modifier, time);
        }

        /// <summary>
        /// Generate a one time password for the modifier, using the current time
        /// </summary>
        /// <param name="modifier"></param>
        /// <returns></returns>
        public string Generate(string? modifier)
        {
            return GenerateCore(modifier, DateTime.UtcNow);
        }

        /// <summary>
        /// Verify a one time password for the given time and modifier
        /// </summary>
        /// <param name="modifier"></param>
        /// <param name="code"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public bool Verify(string? modifier, string code, DateTime time)
        {
            // make range like 0, -1, 1, -2, 2, -3, 3, 4, 5
            var range = from i in Enumerable.Range(-_timeStepBackward, _timeStepBackward + _timeStepForward + 1)
                        orderby Math.Abs(i), i
                        select i;

            // check the code is valid in current time step to the past
            foreach (var i in range)
            {
                var timeStep      = time.AddSeconds(i * _timeStep.TotalSeconds);
                var generatedCode = GenerateCore(modifier, timeStep);
                if (generatedCode == code)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Verify a one time password for the modifier, using the current time
        /// </summary>
        /// <param name="modifier"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool Verify(string? modifier, string code)
        {
            return Verify(modifier, code, DateTime.UtcNow);
        }

        /// <summary>
        /// Generate a one time password for the given time and modifier
        /// </summary>
        /// <param name="modifier"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        private string GenerateCore(string? modifier, DateTime time)
        {
            var timeStep = GetTimeStepNumber(time);
            return _hotpProvider.Generate(modifier, timeStep);
        }

        private long GetTimeStepNumber(DateTime time)
        {
            var delta = time.ToUniversalTime() - _unixEpoch;
            return delta.Ticks / _timeStep.Ticks;
        }
    };
}
