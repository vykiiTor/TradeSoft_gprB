using System;

public class ObjectEventArgs<T> : EventArgs
{
    public T Data { get; }

    public ObjectEventArgs(T data)
    {
        Data = data;
    }
}

public abstract class ObjectReceptor<T>
{
    static Semaphore SyncObject = new Semaphore(1, 1);
    private List<T> ObjectList = new List<T>();
    public Semaphore getSyncObject()
    {
        return SyncObject;
    }

    public List<T> getObjectList()
    {
        return ObjectList;
    }
    public ObjectReceptor()
    {

    }

    public virtual void DataReception(Object sender, ObjectEventArgs<T> e)
    {
        Thread receptor = new Thread(() =>
        {
            SyncObject.WaitOne();
            ObjectList.Add(e.Data);
            Console.WriteLine($"Received Object: {e.Data.ToString}");
            SyncObject.Release();
        });
        receptor.Start();
    }

}

public class TicksReceptor : ObjectReceptor<Ticks_Data>
{
    public override void DataReception(Object sender, ObjectEventArgs<Ticks_Data> e)
    {
        Thread market = new Thread(() =>
        {
            this.getSyncObject().WaitOne();
            //Console.WriteLine($"Received Time: {e.Tick.Time} and received Price : {e.Tick.Price}");
            getObjectList().Add(e.Data);
            Console.WriteLine(" ticks price : " + getObjectList().Last().Price);
            this.getSyncObject().Release();
        });
        market.Start();
    }
}

public class OrderReceptor : ObjectReceptor<Order>
{
    public override void DataReception(Object sender, ObjectEventArgs<Order> e)
    {
        Thread market = new Thread(() =>
        {
            this.getSyncObject().WaitOne();
            //Console.WriteLine($"Received Time: {e.Tick.Time} and received Price : {e.Tick.Price}");
            getObjectList().Add(e.Data);
            Console.WriteLine(" ticks price : " + getObjectList().Last().Price);
            this.getSyncObject().Release();
        });
        market.Start();
    }
}
