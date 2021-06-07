/*
 * Copyright (c) 2016 Phạm Minh Hoàng
 * Framework:   MyClasses
 * Class:       MySize (version 1.0)
 */

[System.Serializable]
public struct MySize
{
    public readonly static MySize Zero = new MySize(0, 0);

    public float Width;
    public float Height;

    public MySize(float width, float height)
    {
        Width = width;
        Height = height;
    }

    public override string ToString()
    {
        return string.Format("({0}, {1}", Width, Height);
    }
}