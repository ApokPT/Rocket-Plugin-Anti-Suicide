using Rocket;
using Rocket.API;

namespace ApokPT.RocketPlugins
{
    public class AntiSuicideConfiguration : IRocketPluginConfiguration
    {

        public bool Enabled { get; set; }

        public bool ResetSuicideCountAfterKick { get; set; }
        public byte MaxSuicidesBeforeKick { get; set; }

        public IRocketPluginConfiguration DefaultConfiguration
        {
            get
            {
                AntiSuicideConfiguration config = new AntiSuicideConfiguration();
                config.Enabled = true;
                config.MaxSuicidesBeforeKick = 3;
                config.ResetSuicideCountAfterKick = false;
                return config;
            }
        }

        
    }
}
