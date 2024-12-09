using Systems.Mining.Tools.Tractor_Beam;

namespace Tools.Tractor_Beam
{
    public abstract class BaseTractorBeamState
    {
        protected readonly TractorBeam Context;

        protected BaseTractorBeamState(TractorBeam context)
        {
            Context = context;
        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }

    public enum TractorBeamState
    {
        Idle, Attracting, Holding, Pushing
    }
}
