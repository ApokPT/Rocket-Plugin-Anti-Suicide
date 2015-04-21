using Rocket;
using Rocket.RocketAPI;

namespace ApokPT.RocketPlugins
{
    public class AntiSuicideConfiguration : RocketConfiguration
    {

        public bool Enabled { get; set; }

        public bool ResetSuicideCountAfterKick { get; set; }
        public byte MaxSuicidesBeforeKick { get; set; }

        public RocketConfiguration DefaultConfiguration
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
