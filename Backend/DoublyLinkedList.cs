namespace Backend;

public class DoublyLinkedList<T> where T : IComparable<T>
{
    private DoubleNode<T>? _head;
    private DoubleNode<T>? _tail;

    private bool _isDescending;

    private int Compare(T a, T b)
    {
        return _isDescending ? b.CompareTo(a) : a.CompareTo(b);
    }

    public void Add(T data)
    {
        var newNode = new DoubleNode<T>(data);

        if (_head == null)
        {
            _head = newNode;
            _tail = newNode;
            return;
        }

        var current = _head;
        while (current != null && Compare(current.Data, data) <= 0)
        {
            current = current.Next;
        }

        if (current == null)
        {
            newNode.Prev = _tail;
            _tail!.Next = newNode;
            _tail = newNode;
            return;
        }

        if (current.Prev == null)
        {
            newNode.Next = _head;
            _head.Prev = newNode;
            _head = newNode;
            return;
        }

        newNode.Prev = current.Prev;
        newNode.Next = current;
        current.Prev.Next = newNode;
        current.Prev = newNode;
    }

    private DoubleNode<T>? FindFirst(T data)
    {
        var current = _head;
        while (current != null)
        {
            if (current.Data.CompareTo(data) == 0)
            {
                return current;
            }
            current = current.Next;
        }
        return null;
    }

    private void RemoveNode(DoubleNode<T> node)
    {
        if (node.Prev != null)
        {
            node.Prev.Next = node.Next;   
        }
        else
        {
            _head = node.Next;            
        }

        if (node.Next != null)
        {
            node.Next.Prev = node.Prev;   
        }
        else
        {
            _tail = node.Prev;            
        }
    }

    public bool Contains(T data)
    {
        return FindFirst(data) != null;
    }

    public bool Remove(T data)
    {
        var node = FindFirst(data);
        if (node == null)
        {
            return false;
        }

        RemoveNode(node);
        return true;
    }

    public int RemoveAll(T data)
    {
        var removed = 0;
        var current = _head;
        while (current != null)
        {
            var next = current.Next;      
            if (current.Data.CompareTo(data) == 0)
            {
                RemoveNode(current);
                removed++;
            }
            current = next;
        }
        return removed;
    }

    public void SortDescending()
    {
        if (_head == null || _isDescending)
        {
            return;   
        }

        Reverse();
        _isDescending = true;
    }

    private void Reverse()
    {
        var current = _head;
        while (current != null)
        {
            var originalNext = current.Next;
            current.Next = current.Prev;
            current.Prev = originalNext;
            current = originalNext;
        }

        (_head, _tail) = (_tail, _head);
    }

    public bool IsEmpty => _head == null;

    private List<(T Value, int Count)> GetOccurrences()
    {
        var occurrences = new List<(T Value, int Count)>();
        var current = _head;
        while (current != null)
        {
            var value = current.Data;
            var count = 0;
            while (current != null && current.Data.CompareTo(value) == 0)
            {
                count++;
                current = current.Next;
            }
            occurrences.Add((value, count));
        }
        return occurrences;
    }

    public List<T> GetModes()
    {
        var occurrences = GetOccurrences();

        var maxCount = 0;
        foreach (var (_, count) in occurrences)
        {
            if (count > maxCount)
            {
                maxCount = count;
            }
        }

        var modes = new List<T>();
        foreach (var (value, count) in occurrences)
        {
            if (count == maxCount)
            {
                modes.Add(value);
            }
        }
        return modes;
    }

    public string GetChart()
    {
        var output = string.Empty;
        foreach (var (value, count) in GetOccurrences())
        {
            output += $"{value} {new string('*', count)}{Environment.NewLine}";
        }
        return output;
    }

    public override string ToString()
    {
        var output = string.Empty;
        var current = _head;
        while (current != null)
        {
            output += $"{current.Data} <-> ";
            current = current.Next;
        }
        return $"{output}null";
    }

    public string ToStringReverse()
    {
        var output = string.Empty;
        var current = _tail;
        while (current != null)
        {
            output += $"{current.Data} <-> ";
            current = current.Prev;
        }
        return $"{output}null";
    }
}