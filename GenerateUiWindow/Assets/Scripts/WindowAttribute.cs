using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class WindowAttribute : Attribute
{
    public string BundleName;
    public int WindowType;

    public WindowAttribute(string bundleName, int windowType)
    {
        this.BundleName = bundleName;
        this.WindowType = windowType;
    }
}
