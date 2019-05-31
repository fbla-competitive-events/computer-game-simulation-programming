using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public delegate void UpdateStackEvent();

public class ObservableStack<T> : Stack<T>
{
    private ObservableStack<Item> items;

    /// <summary>
    /// Constructer used to copy a new instance of a list
    /// </summary>
    /// <param name="items">The list to copy</param>
    public ObservableStack(ObservableStack<T> items) : base(items)
    {
        
    }

    /// <summary>
    /// Default constructor
    /// </summary>
    public ObservableStack()
    {

    }

    public event UpdateStackEvent OnPush;
    public event UpdateStackEvent OnPop;
    public event UpdateStackEvent OnClear;

    /// <summary>
    /// Pushes an item onto the stack and calles the OnPush event
    /// </summary>
    /// <param name="item">The item to push</param>
    public new void Push(T item)
    {
        base.Push(item);

        if (OnPush != null)
        {
            OnPush();
        }
    }

    /// <summary>
    /// Pops an item off of the stack and calls the OnPop event
    /// </summary>
    /// <returns>The item popped</returns>
    public new T Pop()
    {
        T item = base.Pop();

        if (OnPop != null)
        {
            OnPop();
        }

        return item;
    }

    /// <summary>
    /// Clears the stack and calls the OnClear event
    /// </summary>
    public new void Clear()
    {
        base.Clear();

        if (OnClear != null)
        {
            OnClear();
        }
    }
}
