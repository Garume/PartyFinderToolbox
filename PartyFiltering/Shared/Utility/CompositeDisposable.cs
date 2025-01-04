using System.Collections;

namespace PartyFinderToolbox.Shared.Utility;

public class CompositeDisposable : ICollection<IDisposable>
{
    private readonly List<IDisposable> _disposables = [];
    private bool _isDisposed;

    public int Count
    {
        get
        {
            lock (_disposables)
            {
                return _disposables.Count;
            }
        }
    }

    public bool IsReadOnly => false;

    public void Add(IDisposable item)
    {
        ArgumentNullException.ThrowIfNull(item);

        lock (_disposables)
        {
            if (_isDisposed)
                item.Dispose();
            else
                _disposables.Add(item);
        }
    }

    public bool Remove(IDisposable item)
    {
        ArgumentNullException.ThrowIfNull(item);

        lock (_disposables)
        {
            if (_disposables.Remove(item))
            {
                item.Dispose();
                return true;
            }

            return false;
        }
    }

    public void Clear()
    {
        lock (_disposables)
        {
            foreach (var disposable in _disposables) disposable.Dispose();
            _disposables.Clear();
        }
    }

    public bool Contains(IDisposable item)
    {
        lock (_disposables)
        {
            return _disposables.Contains(item);
        }
    }

    public void CopyTo(IDisposable[] array, int arrayIndex)
    {
        lock (_disposables)
        {
            _disposables.CopyTo(array, arrayIndex);
        }
    }

    public IEnumerator<IDisposable> GetEnumerator()
    {
        lock (_disposables)
        {
            return _disposables.ToArray().AsEnumerable().GetEnumerator();
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(Action? action)
    {
        ArgumentNullException.ThrowIfNull(action);

        Add(new ActionDisposable(action));
    }

    public void Dispose()
    {
        lock (_disposables)
        {
            if (_isDisposed) return;

            foreach (var disposable in _disposables) disposable.Dispose();
            _disposables.Clear();
            _isDisposed = true;
        }
    }

    private class ActionDisposable(Action? action) : IDisposable
    {
        private Action? _action = action ?? throw new ArgumentNullException(nameof(action));
        private bool _isDisposed;

        public void Dispose()
        {
            if (_isDisposed) return;
            _isDisposed = true;
            _action?.Invoke();
            _action = null;
        }
    }
}