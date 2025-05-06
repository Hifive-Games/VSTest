public interface IState<T>
{
    void Enter(T owner);
    void Tick(T owner);
    void Exit(T owner);
}

public class StateMachine<T>
{
    T _owner;
    IState<T> _current;

    public void Initialize(T owner, IState<T> startState)
    {
        _owner    = owner;
        _current  = startState;
        _current.Enter(_owner);
    }

    public void ChangeState(IState<T> next)
    {
        _current.Exit(_owner);
        _current = next;
        _current.Enter(_owner);
    }

    public void Update()
    {
        if (_current != null)
            _current.Tick(_owner);
    }

    public IState<T> CurrentState
    {
        get { return _current; }
    }
}