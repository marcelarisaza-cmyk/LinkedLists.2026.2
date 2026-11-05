using System;
using System.Collections.Generic;
using System.Text;

namespace Backend;

public class DoubleNode<T>
{
        public DoubleNode ( T data)
    {
        Data = data;
    }

    public DoubleNode<T>? Prev { get; set; }
    public T Data { get; set; }

    public DoubleNode<T>? Next { get; set; }
}




