

public interface ITractorBeamState
{
    void EnterState(TractorBeamController context);
    void UpdateState(TractorBeamController context);
    void ExitState(TractorBeamController context);
}
